using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using Utilities;

namespace WatchdogLib
{
    public class ApplicationHandler
    {
        private readonly HeartbeatServer _heartbeatServer;
        //private ApplicationHandlerConfig _applicationHandlerConfig;

        public List<ProcessHandler> ProcessHandlers { get; set; }
        public int NonResponsiveInterval             { get; set; }
        public string ApplicationPath                { get; set; }
        public string ApplicationName                { get; set; }
        public bool UseHeartbeat                     { get; set; }
        public Logger Logger                         { get; set; }
        public bool GrantKillRequest                 { get; set; }
        public uint HeartbeatInterval                { get; set; }
        public int MaxProcesses                      { get; set; }
        public int MinProcesses                      { get; set; }

        public bool Active                           { get; set; }
        public bool KeepExistingNoProcesses          { get; set; }
        public uint StartupMonitorDelay               { get; set; }
        public ApplicationHandler(string applicationName,string applicationPath, int nonResponsiveInterval, uint heartbeatInterval = 15, int minProcesses = 1, int maxProcesses=1, bool keepExistingNoProcesses = false, bool useHeartbeat = false, bool grantKillRequest = true, uint startupMonitorDelay=20, bool active= true)
        {
            Logger                  = LogManager.GetLogger("WatchdogServer");
            ProcessHandlers         = new List<ProcessHandler>();
            ApplicationName         = applicationName;
            ApplicationPath         = applicationPath;
            NonResponsiveInterval   = nonResponsiveInterval;
            HeartbeatInterval       = heartbeatInterval;
            MinProcesses            = minProcesses;
            MaxProcesses            = maxProcesses;
            KeepExistingNoProcesses = keepExistingNoProcesses;
            UseHeartbeat            = useHeartbeat;
            GrantKillRequest        = grantKillRequest;
            StartupMonitorDelay     = startupMonitorDelay;

            _heartbeatServer        = HeartbeatServer.Instance;
            Active                  = active;
        }

        public ApplicationHandler(ApplicationHandlerConfig applicationHandlerConfig)
        {
            Logger = LogManager.GetLogger("WatchdogServer");
            ProcessHandlers = new List<ProcessHandler>();
            Set(applicationHandlerConfig);
            _heartbeatServer = HeartbeatServer.Instance;
            ;
        }

        public void Set(ApplicationHandlerConfig applicationHandlerConfig)
        {
            ApplicationName         = applicationHandlerConfig.ApplicationName;
            ApplicationPath         = applicationHandlerConfig.ApplicationPath;
            NonResponsiveInterval   = applicationHandlerConfig.NonResponsiveInterval;
            HeartbeatInterval       = applicationHandlerConfig.HeartbeatInterval;
            MinProcesses            = applicationHandlerConfig.MinProcesses;
            MaxProcesses            = applicationHandlerConfig.MaxProcesses;
            KeepExistingNoProcesses = applicationHandlerConfig.KeepExistingNoProcesses;
            UseHeartbeat            = applicationHandlerConfig.UseHeartbeat;
            GrantKillRequest        = applicationHandlerConfig.GrantKillRequest;
            Active                  = applicationHandlerConfig.Active;
            StartupMonitorDelay     = applicationHandlerConfig.StartupMonitorDelay;
            Active                  = applicationHandlerConfig.Active;
        }

        public void Check()
        {         
            if (!Active) return;
            // Check if  new unmonitored process is running 
            HandleDuplicateProcesses();
            HandleNonResponsiveProcesses();
            HandleExitedProcesses();
            HandleUnmonitoredProcesses();
            HandleProcessNotRunning();
        }

        

        private void HandleProcessNotRunning()
        {            
            var processes = Process.GetProcessesByName(ApplicationName);

            if (processes.Length == 0)
            {
                // Start new process
                var processHandler = new ProcessHandler
                {
                    WaitForExit = false,
                    NonResponsiveInterval = NonResponsiveInterval,
                    StartingInterval = StartupMonitorDelay
                };
                Logger.Info("No process of application {0} is running, so one will be started", ApplicationName);
                processHandler.CallExecutable(ApplicationPath, "");
                ProcessHandlers.Add(processHandler);
            }        
        }

        private void HandleExitedProcesses()
        {
            for (int index = 0; index < ProcessHandlers.Count; index++)
            {
                var processHandler = ProcessHandlers[index];
                if (processHandler.HasExited)
                {
                    Logger.Warn("Process {0} has exited", processHandler.Name);
                    //Debug.WriteLine("{0} has exited", processHandler.Name);
                    processHandler.Close();
                   
                    var notEnoughProcesses      = (ProcessNo(processHandler.Name) < MinProcesses);
                    var lessProcessesThanBefore = (ProcessNo(processHandler.Name) < MaxProcesses) &&  KeepExistingNoProcesses;

                    if (notEnoughProcesses || lessProcessesThanBefore)
                    {
                        if (notEnoughProcesses) Logger.Info("Process {0} has exited and no others are running, so start new", processHandler.Name);
                        if (lessProcessesThanBefore) Logger.Info("Process {0} has exited, and number of processed needs to maintained , so start new", processHandler.Name);
                        processHandler.CallExecutable();
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
            for (var index = 0; index < ProcessHandlers.Count; index++)
            {
                var processHandler = ProcessHandlers[index];
                
                if (processHandler.HasExited)  continue; // We will deal with this later
                if (processHandler.IsStarting)
                {
                    //Logger.Info("Process {0} is still in startup phase, no checking is being performed", processHandler.Name);
                    Debug.WriteLine("Process {0} is still in startup phase, no checking is being performed", processHandler.Name);
                    continue; // Is still starting up, so ignore
                }
                if (_heartbeatServer.HeartbeatTimedOut(processHandler.Name, HeartbeatInterval/2) && UseHeartbeat)
                {
                    //todo: add throttling
                    Logger.Warn("No heartbeat received from process {0} within the soft limit", processHandler.Name);
                    //Debug.WriteLine("Process {0} has no heartbeat within soft limit", processHandler.Name);
                }

                //if (processHandler.Responding && !_heartbeatServer.HeartbeatHardTimeout(processHandler.Name)) continue;
                //Debug.WriteLine("Process {0} not responding", processHandler.Name);

                var notRespondingAfterInterval = processHandler.NotRespondingAfterInterval;
                var noHeartbeat = _heartbeatServer.HeartbeatTimedOut(processHandler.Name,HeartbeatInterval) && UseHeartbeat;
                var requestedKill = _heartbeatServer.KillRequested(processHandler.Name);

                var performKill = notRespondingAfterInterval || noHeartbeat || requestedKill;

                var reason = notRespondingAfterInterval ? "not responding" : noHeartbeat ? "not sending a heartbeat signal within hard limit" : "requesting to be killed"; 

                if (performKill)
                {
                    Logger.Error("process {0} is {1}, and will be killed ", processHandler.Name, reason); 
                    //Debug.WriteLine("Process {0} is not responsive, or no heartbeat within hard limit",processHandler.Name);

                    if (processHandler.Kill())
                    {
                      
                        Logger.Error("Process {0} was {1} and has been successfully killed ", processHandler.Name, reason);
                        //Debug.WriteLine("Process {0} has been killed due to non-responsiveness not responding",processHandler.Name);
                        processHandler.Close();
                        var notEnoughProcesses      = (ProcessNo(processHandler.Name) < MinProcesses);
                        var lessProcessesThanBefore = (ProcessNo(processHandler.Name) < MaxProcesses) && KeepExistingNoProcesses;


                        //if ((ProcessNo(processHandler.Name) == 0) || (ProcessNo(processHandler.Name) > 0) && (KeepExistingNoProcesses && !EnsureSingleProcess))
                        if (notEnoughProcesses || lessProcessesThanBefore)
                        {
                            processHandler.CallExecutable();
                        }
                        else
                        {
                            ProcessHandlers.Remove(processHandler);
                        }
                    }
                    else
                    {
                        // todo smarter handling of this case (try again in next loop, put to sleep, etc)
                        Logger.Error("Process {0} was {1} but could not be successfully killed ", processHandler.Name, reason);
                        //Debug.WriteLine("Process {0} could not be killed after getting non responsive",processHandler.Name);
                    }
                }
            }
        }

        public void HandleDuplicateProcesses()
        {
            //if (ProcessNo(ApplicationName) < MaxProcesses))
            {

                var processes = Process.GetProcessesByName(ApplicationName);


                if (processes.Length <= MaxProcesses) return;
                //if (processes.Length <= 1) return ;


                Logger.Error("multiple processes of application {0} are running, all but one will be killed ", ApplicationName);

                var remainingProcesses = new List<Process>();
                var result = true;

                var nummProcesses = processes.Length;
                //Wield out the bad applications first
                foreach (var process in processes)
                {
                    var processHandler = FindProcessHandler(process);

                    // Make sure we leave at least one process running
                    if (nummProcesses <= MaxProcesses) break;
                    if (!process.Responding)
                    {
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

                    //Return the other process instance.  
                }

                //Loop through the running processes in with the same name  
                for (var index = MaxProcesses; index < remainingProcesses.Count; index++)
                {
                    var process = remainingProcesses[index];
                    var processHandler = FindProcessHandler(process);
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
                        // This process o
                        var processHandler = new ProcessHandler
                        {
                            WaitForExit = false,
                            NonResponsiveInterval = NonResponsiveInterval,
                        };
                        processHandler.MonitorProcess(process);
                        ProcessHandlers.Add(processHandler);
                    }

                    // Perform 
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("Error while starting to monitor application {0}: {1}", ApplicationName, ex.Message);
                //Debug.WriteLine(ex.Message);
            }

            return true;
        }


    }
}
