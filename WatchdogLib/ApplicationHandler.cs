using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using NLog;
using utilities.Windows;
using Utilities;

namespace WatchdogLib
{
    public class ApplicationHandler
    {
        private readonly HeartbeatServer _heartbeatServer;
        private DateTime _lastTimeStarted;
        private const int MaxTries = 3;

        public List<ProcessHandler> ProcessHandlers  { get; set; }
        public int NonResponsiveInterval             { get; set; }
        public string ApplicationPath                { get; set; }
        public string ApplicationName                { get; set; }
        public string ApplicationArguments           { get; set; }
        public bool UseHeartbeat                     { get; set; }

        public bool IgnoreHeartBeatIfNeverAcquired   { get; set; }
        public Logger Logger                         { get; set; }
        public bool GrantKillRequest                 { get; set; }
        public int HeartbeatInterval                 { get; set; }
        public int MaxProcesses                      { get; set; }
        public int MinProcesses                      { get; set; }
   
        public bool Active                           { get; set; }
        public bool KeepExistingNoProcesses          { get; set; }
        public int StartingInterval                  { get; set; }
        public int TimeBetweenRetry                  { get; set; }

        //public ApplicationHandler(string applicationName,string applicationPath, string applicationArguments, int nonResponsiveInterval, int heartbeatInterval, int minProcesses, int maxProcesses, bool keepExistingNoProcesses, bool useHeartbeat, bool grantKillRequest, int startingInterval, bool active, int timeBetweenRetry)
        //{
        //    Logger                  = LogManager.GetLogger("WatchdogServer");
        //    ProcessHandlers         = new List<ProcessHandler>();
        //    ApplicationName         = applicationName;
        //    ApplicationPath         = applicationPath;
        //    ApplicationArguments    = applicationArguments;
        //    HeartbeatInterval       = heartbeatInterval;
        //    MinProcesses            = minProcesses;
        //    MaxProcesses            = maxProcesses;
        //    KeepExistingNoProcesses = keepExistingNoProcesses;
        //    UseHeartbeat            = useHeartbeat;
        //    GrantKillRequest        = grantKillRequest;
        //    StartingInterval        = startingInterval;
        //    NonResponsiveInterval   = nonResponsiveInterval;
        //    Active                  = active;
        //    TimeBetweenRetry        = timeBetweenRetry;
        //    _heartbeatServer        = HeartbeatServer.Instance;
        //    _lastTimeStarted        = DateTime.MinValue;
            
        //}

        public ApplicationHandler(ApplicationHandlerConfig applicationHandlerConfig)
        {
            Logger = LogManager.GetLogger("WatchdogServer");
            ProcessHandlers = new List<ProcessHandler>();
            Set(applicationHandlerConfig);
            _heartbeatServer = HeartbeatServer.Instance;
            SleepDetector.Instance.SleepTimeOut = 60*1000;
        }

        public void Set(ApplicationHandlerConfig applicationHandlerConfig)
        {
            ApplicationName                = applicationHandlerConfig.ApplicationName;
            ApplicationPath                = applicationHandlerConfig.ApplicationPath;
            ApplicationArguments           = applicationHandlerConfig.ApplicationArguments;
            NonResponsiveInterval          = applicationHandlerConfig.NonResponsiveInterval;
            HeartbeatInterval              = applicationHandlerConfig.HeartbeatInterval;
            MinProcesses                   = applicationHandlerConfig.MinProcesses;
            MaxProcesses                   = applicationHandlerConfig.MaxProcesses;
            KeepExistingNoProcesses        = applicationHandlerConfig.KeepExistingNoProcesses;
            UseHeartbeat                   = applicationHandlerConfig.UseHeartbeat;
            IgnoreHeartBeatIfNeverAcquired = applicationHandlerConfig.IgnoreHeartBeatIfNeverAcquired;
            GrantKillRequest               = applicationHandlerConfig.GrantKillRequest;
            Active                         = applicationHandlerConfig.Active;
            StartingInterval               = applicationHandlerConfig.StartupMonitorDelay;
            Active                         = applicationHandlerConfig.Active;
            TimeBetweenRetry               = applicationHandlerConfig.TimeBetweenRetry;
        }

        public void Check()
        {
            try
            {
                if (!Active) return;                
                // Check if  new unmonitored process is running 
                HandleDuplicateProcesses();
                HandleNonResponsiveProcesses();
                HandleExitedProcesses();
                HandleUnmonitoredProcesses();
                HandleProcessNotRunning();
                HandleExceptionPopup();
            }
            catch (Exception ex)
            {
                Logger.Fatal("Watchdog monitoring thread threw exception {0}", ex.Message);
            }
        }

        

        public void Restart()
        {
             _heartbeatServer.Restart();
            foreach (var processHandler in ProcessHandlers)
            {
                processHandler.Reset();               
            }
            
        }

        private void HandleProcessNotRunning()
        {
            if (DateTime.Now - _lastTimeStarted < TimeSpan.FromSeconds(TimeBetweenRetry))
            {
                return;
            }        
            var processes = Process.GetProcessesByName(ApplicationName);

            if (processes.Length == 0)
            {
                // Start new process
                var processHandler = new ProcessHandler
                {
                    WaitForExit           = false,
                    StartingInterval      = StartingInterval,
                    NonResponsiveInterval = NonResponsiveInterval,                   
                };
                if (File.Exists(ApplicationPath))
                {
                    Logger.Info("No process of application {0} is running, so one will be started", ApplicationName);
                    processHandler.CallExecutable(ApplicationPath, ApplicationName, ApplicationArguments);                    
                    ProcessHandlers.Add(processHandler);
                }
                else
                {
                    Logger.Error("Application path does not exist, cannot start app");
                }
                _lastTimeStarted = DateTime.Now;
            }        
        }

        private void HandleExitedProcesses()
        {        
            if (DateTime.Now - _lastTimeStarted < TimeSpan.FromSeconds(TimeBetweenRetry))
            {
                return;
            }  
            for (int index = 0; index < ProcessHandlers.Count; index++)
            {
                var processHandler = ProcessHandlers[index];
                if (processHandler.HasExited)
                {
                    if (string.IsNullOrEmpty(processHandler.Name))
                    {
                        ProcessHandlers.Remove(processHandler);
                    }
                    Logger.Warn("Process {0} has exited", processHandler.Name);
                    processHandler.Close();
                   
                    var notEnoughProcesses      = (ProcessNo(processHandler.Name) < MinProcesses);
                    var lessProcessesThanBefore = (ProcessNo(processHandler.Name) < MaxProcesses) &&  KeepExistingNoProcesses;

                    if ((notEnoughProcesses || lessProcessesThanBefore) && File.Exists(processHandler.Executable))
                    {
                        if (notEnoughProcesses) Logger.Info("Process {0} has exited and no others are running, so start new", processHandler.Name);
                        if (lessProcessesThanBefore) Logger.Info("Process {0} has exited, and number of processed needs to maintained , so start new", processHandler.Name);
                        if (!processHandler.CallExecutable())
                        {
                            Logger.Error("Process restart has failed ");
                            ProcessHandlers.Remove(processHandler);
                        }
                        _lastTimeStarted = DateTime.Now;
                    }
                    else
                    {
                        Logger.Info("Process {0} has exited, but no requirement to start new one", processHandler.Name);
                        ProcessHandlers.Remove(processHandler);
                    }
                }
            }
        }

        private void HandleNonResponsiveProcesses()
        {
            //if (SleepDetector.Instance.JustSlept()) return;
            for (var index = 0; index < ProcessHandlers.Count; index++)
            {
                var noHeartbeat = false;
                var processHandler = ProcessHandlers[index];
                var heartbeatClient = _heartbeatServer.FindByProcessName(processHandler.Name);

                if (processHandler.HasExited)  continue; // We will deal with this later
                if (processHandler.IsStarting || SleepDetector.Instance.JustSlept())
                {
                    Debug.WriteLine("Process {0} is still in startup phase, no checking is being performed", processHandler.Name);
                    processHandler.Reset();
                    processHandler.Tries = 0;
                    if (heartbeatClient != null)
                    {
                        heartbeatClient.Reset();
                        heartbeatClient.Tries = 0;
                    }
                    continue; // Is still starting up, so ignore
                } else
                {
                    // Not starting up anymore, so see if we have a heartbeat connection
                    if (heartbeatClient==null && !IgnoreHeartBeatIfNeverAcquired )
                    {
                        noHeartbeat = true;
                    }
                }


                if (_heartbeatServer.HeartbeatTimedOut(processHandler.Name, (uint)HeartbeatInterval/(MaxTries*2)) && UseHeartbeat)
                {
                    //todo: add throttling
                    Logger.Warn("No heartbeat received from process {0} within the soft limit", processHandler.Name);
                }

                
                
                if (_heartbeatServer.HeartbeatTimedOut(processHandler.Name, (uint)HeartbeatInterval/MaxTries) && UseHeartbeat && heartbeatClient!=null)
                {
                    try{
                        
                        heartbeatClient.Tries++;
                        if (heartbeatClient.Tries >= MaxTries)
                        {
                            Logger.Warn("Again no heartbeat received from process {0} within the limit, ", processHandler.Name);
                            heartbeatClient.Reset();
                            heartbeatClient.Tries = 0;
                            noHeartbeat = true;
                        }
                        else
                        {
                            Logger.Warn("First time no heartbeat received from process {0} within the limit, ", processHandler.Name);
                            heartbeatClient.Reset();
                        
                        }
                    } catch { Logger.Warn("Error trying to access Heartbeat client ");}                    
                }else
                {
                    try { 
                        if (heartbeatClient!=null) heartbeatClient.Tries = 0;
                    } catch {  }    

                }

                var notRespondingAfterInterval = false;
                if (processHandler.NotRespondingAfterInterval == Responsiveness.Unresponding)
                {
                    processHandler.Tries++;
                    if (processHandler.Tries >= MaxTries)
                    {
                        Logger.Warn("Again no response from process {0} within the hard limit, ", processHandler.Name);
                        processHandler.Reset();
                        processHandler.Tries = 0;
                         notRespondingAfterInterval = true;
                    }
                    else
                    {
                        Logger.Warn("First time no response from process {0} within the hard limit, ", processHandler.Name);
                        processHandler.Reset();
                       
                    }
                } else if(processHandler.NotRespondingAfterInterval == Responsiveness.Responding)
                {
                    processHandler.Tries = 0;
                }

                var requestedKill = _heartbeatServer.KillRequested(processHandler.Name);
                var performKill   = (notRespondingAfterInterval || noHeartbeat || requestedKill) && !SleepDetector.Instance.JustSlept();
                var reason        = notRespondingAfterInterval ? "not responding" : noHeartbeat ? "not sending a heartbeat signal within hard limit" : "requesting to be killed"; 

                if (performKill)
                {
                    if (SleepDetector.Instance.JustSlept()) return;
                    Logger.Error("process {0} is {1}, and will be killed ", processHandler.Name, reason); 
                    if (processHandler.Kill())
                    {
                      
                        Logger.Error("Process {0} was {1} and has been successfully killed ", processHandler.Name, reason);

                        processHandler.Close();
                        var notEnoughProcesses      = (ProcessNo(processHandler.Name) < MinProcesses);
                        var lessProcessesThanBefore = (ProcessNo(processHandler.Name) < MaxProcesses) && KeepExistingNoProcesses;

                        if (notEnoughProcesses || lessProcessesThanBefore)
                        {
                            processHandler.CallExecutable();
                            _lastTimeStarted = DateTime.Now;
                        }
                        else
                        {
                            ProcessHandlers.Remove(processHandler);
                        }
                    }
                    else
                    {
                        // to do smarter handling of this case (try again in next loop, put to sleep, etc)
                        Logger.Error("Process {0} was {1} but could not be successfully killed ", processHandler.Name, reason);
                    }
                }
            }
        }

        public void HandleDuplicateProcesses()
        {
            {
                if (SleepDetector.Instance.JustSlept()) return;
                var processes = Process.GetProcessesByName(ApplicationName);
                if (processes.Length <= MaxProcesses) return;

                Logger.Error("multiple processes of application {0} are running, all but one will be killed ", ApplicationName);

                var remainingProcesses = new List<Process>();
                var result             = true;
                var nummProcesses      = processes.Length;
                //Wield out the bad applications first
                foreach (var process in processes)
                {
                    var processHandler = FindProcessHandler(process);

                    // Make sure we leave at least one process running
                    if (nummProcesses <= MaxProcesses) break;
                    if (!process.Responding)
                    {
                        if (SleepDetector.Instance.JustSlept()) return;
                        Logger.Warn("unresponsive duplicate process {0} will now be killed ", ApplicationName);                       
                        var currResult = (processHandler != null)?processHandler.Kill():ProcessUtils.KillProcess(process);
                        if (currResult && processHandler != null)
                        {
                            ProcessHandlers.Remove(processHandler);
                            processHandler.Close();
                            Logger.Info("Unresponsive duplicate process {0} has been killed ", ApplicationName);
                        }
                        else
                        {
                            Logger.Error("Unresponsive duplicate process {0} could not be killed ", ApplicationName);
                        }
                        result = result && currResult;
                        nummProcesses--;
                    }
                    else
                    {
                        remainingProcesses.Add(process);
                    } 
                }

                //Loop through the running processes in with the same name  
                for (var index = MaxProcesses; index < remainingProcesses.Count; index++)
                {
                    var process        = remainingProcesses[index];
                    var processHandler = FindProcessHandler(process);
                    if (SleepDetector.Instance.JustSlept()) return;
                    Logger.Warn("unresponsive duplicate process {0} will now be killed ", ApplicationName);
                    var currResult = ProcessUtils.KillProcess(process);
                    if (currResult && processHandler != null)
                    {
                        ProcessHandlers.Remove(processHandler);
                        processHandler.Close();
                        Logger.Info("Duplicate process {0} has been killed ", ApplicationName);
                    }
                    else
                    {
                        Logger.Error("Unresponsive duplicate process {0} could not be killed ", ApplicationName);
                    }
                    result = result && currResult;
                }
            }
        }

        private void HandleExceptionPopup()
        {
            // Loop through all windows
            //var windows = CWindow.TopLevelWindows;
            //foreach (var window in windows)
            //{
            //    window.
            //}
        }


        private ProcessHandler FindProcessHandler(Process process)
        {
            return ProcessHandlers.Find((processHander) => processHander.Process.Id == process.Id);
        }

        private int ProcessNo(string applicationName)
        {
            return Process.GetProcessesByName(applicationName).Length;
        }

        public bool HandleUnmonitoredProcesses()
        {
            try
            {
                var processes = Process.GetProcessesByName(ApplicationName);
                foreach (var process in processes)
                {
                    if (ProcessHandlers.All(procHandle => procHandle.Process==null || procHandle.Process.Id != process.Id))
                    {
                        var processHandler = new ProcessHandler
                        {
                            WaitForExit = false,
                            NonResponsiveInterval = NonResponsiveInterval,
                            StartingInterval = StartingInterval
                        };
                        processHandler.MonitorProcess(process);
                        ProcessHandlers.Add(processHandler);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("Error while starting to monitor application {0}: {1}", ApplicationName, ex.Message);
                return false;
            }

            return true;
        }
    }
}
