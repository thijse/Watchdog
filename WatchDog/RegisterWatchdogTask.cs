using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using Utilities;

namespace WatchDog
{
    public class RegisterWatchdogTask
    {
        const string TaskName = "WatchdogStarter";
        const string ExecutableName = "Watchdog.exe";

        public static bool SetTask(bool setTask)
        {
            if (setTask) return CreateTask(); else return DisableTask();
        }

        private static bool DisableTask()
        {
            try
            {

                using (var taskService = new TaskService())
                {
                    var task = taskService.FindTask(TaskName);
                    if (task == null) return true;

                    task.Enabled = false;
                    taskService.RootFolder.DeleteTask(TaskName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;

            }

        }

        public static bool CreateTask()
        {
            try
            {
                if (TaskExists()) return true;

                var path     = Path.GetDirectoryName(Application.ExecutablePath);
                var fileName = FileUtils.Combine(path, ExecutableName);
                if (!File.Exists(fileName)) return false;            


                using (var taskService = new TaskService())
                {
                   // Create a new task definition and assign properties
                   var taskDefinition = taskService.NewTask();
                   taskDefinition.RegistrationInfo.Description = "Starts watchdog if not running";
                   taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
                   try { taskDefinition.Settings.DisallowStartIfOnBatteries = false; } catch { }
                   try { taskDefinition.Settings.StopIfGoingOnBatteries    = false;  } catch { }

                   // Fire trigger every 5 minutes
                    var trigger = new TimeTrigger
                    {
                        Repetition =
                        {
                            Interval = TimeSpan.FromMinutes(5),
                        }
                    };
                    taskDefinition.Triggers.Add(trigger);         

                   // Add an action that will launch Notepad whenever the trigger fires
                   taskDefinition.Actions.Add(new ExecAction(fileName, "", null));

                   // Register the task in the root folder
                   taskService.RootFolder.RegisterTaskDefinition(TaskName, taskDefinition);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;

            }
            return true;
        }

        public static bool TaskExists()
        {
            var path     = Path.GetDirectoryName(Application.ExecutablePath);
            var fileName = FileUtils.Combine(path, ExecutableName);
            using (var taskService = new TaskService())
            {
                var task = taskService.FindTask(TaskName);
                if (task == null) return false;

                var action = (ExecAction) task.Definition.Actions[0];
                var filePath = action.Path;
                if (filePath != fileName)
                {
                    // Watchdog location changed, delete to be re-instantiated
                    taskService.RootFolder.DeleteTask(TaskName);
                    return false;
                }

                return true;
            }
            
        }

    }


}
