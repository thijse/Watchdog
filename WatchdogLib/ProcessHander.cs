using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utilities;

namespace WatchdogLib
{

    // todo: http://msdn.microsoft.com/en-us/library/system.diagnostics.process.exitcode(v=vs.110).aspx
    //http://stackoverflow.com/questions/2279181/catch-another-process-unhandled-exception
    // http://social.msdn.microsoft.com/Forums/vstudio/en-US/62259e21-3280-4d10-a27c-740d35efe51c/catch-another-process-unhandled-exception?forum=csharpgeneral
    public class ProgressEventArgs : EventArgs
    {
        public float Progress { get; private set; }
        public Process Process { get; private set; }
        public ProgressEventArgs(float progress, Process process)
        {
            Progress = progress;
            Process = process;
        }
    }

    public class ProcessMessageArgs : EventArgs
    {
        public string Message { get; private set; }
        public Process Process { get; private set; }
        public ProcessMessageArgs(string message, Process process)
        {
            Message = message;
            Process = process;
        }
    }

    public class ProcessStatusArgs : EventArgs
    {
        public int ExitCode { get; private set; }
        public Process Process { get; private set; }
        public ProcessStatusArgs(int exitCode, Process process)
        {
            ExitCode = exitCode;
            Process = process;
        }
    }

    public class ProcessHandler
    {

        private readonly object _exitedLock = new object();
        private Stopwatch _nonresponsiveInterval;
        private Stopwatch _fromStart;

        public DataReceivedEventHandler OutputHandler;
        public EventHandler<ProcessMessageArgs> ErrorOutputHandler;
        
        public event EventHandler<ProcessStatusArgs> ExitHandler;
        public event EventHandler<ProcessMessageArgs> ErrorHandler;


 
        public int NonResponsiveInterval  { get; set; }
        public string Executable          { get; set; }

        public string Args                { get; set; }

        public bool WaitForExit           { get; set; }
        public bool RunInDir              { get; set; }
        public uint NonresponsiveInterval { get; set; }

        public uint StartingInterval      { get; set; }

        public bool HasExited
        {
            get { return (Process == null) || Process.HasExited; }
        }

        public bool Responding
        {
            get
            {
                // todo: add heartbeat
                if (!Process.Responding)
                {
                    if (!_nonresponsiveInterval.IsRunning) _nonresponsiveInterval.Restart();
                }
                else
                {
                    _nonresponsiveInterval.Reset();
                }

                return (Process == null) || Process.Responding;
            }
        }


        public ProcessHandler()
        {
            WaitForExit            = true;
            RunInDir               = true;
            NonresponsiveInterval  = 2000;
            StartingInterval       = 5000;
            _nonresponsiveInterval = new Stopwatch();
            _fromStart             = new Stopwatch();
        }


        public void CallExecutable(string executable, string args)
        {
            Args = args;
            Executable = executable;
            CallExecutable();
        }

        public bool MonitorProcess(Process process)
        {
            Process = process;
            try
            {
                Process.OutputDataReceived += Output;
                Process.ErrorDataReceived  += OutputError;
                Name                        = Process.ProcessName;
                _fromStart.Restart();
                if (WaitForExit)
                {
                    Process.WaitForExit();
                    EndProcess();
                }
                else
                {
                    Process.Exited += ProcessExited;
                }
            }
            catch (Exception ex)
            {
                if (ErrorHandler != null) ErrorHandler(this, new ProcessMessageArgs(ex.Message, Process));
                if (Process.HasExited)
                {
                    if (ExitHandler != null) ExitHandler(this, new ProcessStatusArgs(-1, Process));
                    return false;
                }

            }
            return true;
        }

        private void EndProcess()
        {
            if (Process==null) return;
            Process.Close();
            Process.Dispose();
            Process = null;
        }


        public void CallExecutable()
        {
            
            if (!File.Exists(Executable)) return;
            var commandLine = Executable;
            Trace.WriteLine("Running command: " + Executable + " " + Args);
            var psi = new ProcessStartInfo(commandLine)
            {
                UseShellExecute = false,
                LoadUserProfile = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                Arguments = Args
            };
            if (RunInDir)
            {
                var path = Path.GetDirectoryName(Executable);
                if (path != null) psi.WorkingDirectory = path;
            }
            Process = new Process { StartInfo = psi };
            try
            {               
                Process.Start();
                Process.BeginOutputReadLine();
                Process.BeginErrorReadLine();
                Process.OutputDataReceived += Output;
                Process.ErrorDataReceived += OutputError;
                _fromStart.Restart();
                Name = Process.ProcessName;

                // Watch process for not responding
                if (WaitForExit)
                {
                    Process.WaitForExit();
                    EndProcess();
                }
                else
                {
                    Process.Exited += ProcessExited;
                }
            }
            catch (Exception ex)
            {
                if (ErrorHandler != null) ErrorHandler(this, new ProcessMessageArgs(ex.Message, Process));
                if (!ProcessUtils.ProcessRunning(Executable))
                {
                    if (ExitHandler != null) ExitHandler(this, new ProcessStatusArgs(-1, Process));
                }

            }
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            Running = true;
        }

        public bool Running { get; set; }


        public Process Process { get; private set; }
        public string Name { get; private set; }

        public bool NotRespondingAfterInterval
        {
            get { return (!Responding && _nonresponsiveInterval.ElapsedMilliseconds > NonresponsiveInterval); }
        }

        public bool IsStarting
        {
            get { return (_fromStart.ElapsedMilliseconds < StartingInterval); }
        }


        public void Close()
        {
            EndProcess();
        }

   
        private void Output(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (string.IsNullOrEmpty(dataReceivedEventArgs.Data)) return;

            var output = dataReceivedEventArgs.Data;
            // Fire Output event
            if (OutputHandler != null) OutputHandler(sender, dataReceivedEventArgs);
        }

        private void OutputError(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (string.IsNullOrEmpty(dataReceivedEventArgs.Data)) return;
            // Fire OutputError event
            var progressEventArgs = new ProcessMessageArgs(dataReceivedEventArgs.Data, Process);
            if (ErrorOutputHandler != null) ErrorOutputHandler(sender, progressEventArgs);
        }

        public bool Kill()
        {
            return ProcessUtils.KillProcess(Process);
            // Todo set state indicating that kill has been tried
        }
    }


}
