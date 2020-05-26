using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Utilities
{
    public class ProcessUtils
    {
        public static bool StopOtherInstances(bool stopOnlyFromSameExe)
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            var result = true;

            //Loop through the running processes in with the same name  
            foreach (var process in processes)
            {
                //Ignore the current process  
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.  
                    if ((Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName) || !stopOnlyFromSameExe)
                    {
                        //Return the other process instance.  
                        result = result && ProcessUtils.KillProcess(process);
                    }
                }
            }
            //No other instance was found, return null.  
            return result;
        }

        public static Process StopAllButOneInstance(Process[] processes)
        {
            if (processes.Length == 0) return null;
            if (processes.Length == 1) return processes[0];

            var remainingProcesses = new List<Process>();
            var result = true;

            var nummProcesses = processes.Length;
            //Wield out the bad applications first
            foreach (var process in processes)
            {
                // Make sure we leave at least one process running
                if (nummProcesses<=1) break;
                if (!process.Responding)
                {
                    result = result && ProcessUtils.KillProcess(process);
                    nummProcesses--;
                }
                else
                {
                    remainingProcesses.Add(process);
                }
                
                //Return the other process instance.  
            }

            //Loop through the running processes in with the same name  
            for (var index = 1; index < remainingProcesses.Count; index++)
            {
                var process = remainingProcesses[index];
                result = result && ProcessUtils.KillProcess(process);
            }

            return remainingProcesses[0];
        }

        public static bool ProcessRunning(string processname)
        {
            processname = GetProcessName(processname);          
            return (Process.GetProcessesByName(processname).Length > 0);
        }



        private static string StripExtension(string processname, string[] extensions = null)
        {
            if (extensions==null) extensions = new string[] { ".exe", ".com", ".dll", ".bat", ".cmd", ".bin" };
            processname = processname.ToLowerInvariant();            
            processname = extensions.Aggregate(processname, (current, extension) => current.Replace(extension, ""));
            return processname;
        }

        public static string GetProcessName(string processname)
        {
            return StripExtension(Path.GetFileName(StripParameters(processname)));
        }

        private static string StripParameters(string processname)
        {
            var execVars = processname.Split(new char[] { ' ' }, 2);
            return execVars[0].Trim();
        }

        public static bool KillProcess(Process process)
        {
            const int timeoutNice = 500;
            const int timeoutTotal = 1500;
            {
                // 1. Ask nicely
                try
                {
                    process.CloseMainWindow();
                    process.WaitForExit(timeoutNice);
                }
                catch (Exception)
                {
                }
                if (process.HasExited) return true;

                // 2. Force process to stop
                try
                {
                    process.Kill();
                }
                catch (Exception)
                {
                }
            }
            // See if processes are still running          
            return process.WaitForExit(timeoutTotal);
        }

        public static bool KillProcess(string processname)
        {
            processname = GetProcessName(processname);           
            const int timeoutNice = 500;
            const int timeoutTotal = 500;
            foreach (var process in Process.GetProcessesByName(processname))
            {
                bool stopped;

                // 1. Ask nicely
                try
                {
                    process.CloseMainWindow();
                    stopped = process.WaitForExit(timeoutNice);
                }
                catch (Exception)
                {
                    stopped = false;
                }
                if (stopped) break;

                // 2. Force process to stop
                try
                {
                    process.Kill();
                }
                catch (Exception)
                {
                }
            }
            // See if processes are still running            
            return Process.GetProcessesByName(processname).Select(process => process.WaitForExit(timeoutTotal)).All(result => result);            
        }

        public static bool StopAll(Process[] processes)
        {
            var result = true;
            foreach (var process in processes)
            {
                // Make sure we leave at least one process running
                    result = result && ProcessUtils.KillProcess(process);
            }
            return result;
        }

        public static bool KillProcessAggresively(Process process)
        {
            return KillProcess(process);
        }
    }
}
