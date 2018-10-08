using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;

namespace WatchdogStarter
{
    public class RegisterWatchdogTask
    {
         const string taskName = "WatchdogStarter";
        public bool CreateTask()
        {
            try
            {
                using (var taskService = new TaskService())
                {
                   // Create a new task definition and assign properties
                   var task = taskService.NewTask();
                   task.RegistrationInfo.Description = "Starts watchdog if not running";
                   task.Principal.LogonType = TaskLogonType.InteractiveToken;

                   // Fire trigger every 5 minutes
                    var trigger = new TimeTrigger
                    {
                        Repetition =
                        {
                            Interval = TimeSpan.FromMinutes(5),
                            Duration = TimeSpan.FromMinutes(1)
                        }
                    };
                    task.Triggers.Add(trigger);
            
                   // Add an action that will launch Notepad whenever the trigger fires
                   task.Actions.Add(new ExecAction("WatchdogStarter.exe", "", null));


                   // Register the task in the root folder
                   taskService.RootFolder.RegisterTaskDefinition(taskName, task);

                }
            }
            catch (Exception)
            {
                return false;

            }
            return true;
        }

        public bool FindTask()
        {
            using (var taskService = new TaskService())
            {
                var task = taskService.GetTask(taskName);
                if (task == null || task.Name != taskName) return false;
            }
        }

    }


}
