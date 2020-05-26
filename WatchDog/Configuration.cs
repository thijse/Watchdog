using System.Collections.Generic;
using WatchdogLib;

namespace WatchDog
{
    public class Configuration
    {
        public List<ApplicationHandler> ApplicationHandlersConfig { get; set; }
        public bool ShowTrayIcon { get; set; }

        public Configuration()
        {
            ApplicationHandlersConfig = new List<ApplicationHandler>();
            ShowTrayIcon              = true;

            var app = new ApplicationHandlerConfig()
            {
                ApplicationPath = @"D:\DevelPers\WatchDog\MonitoredApplication\bin\Release\MonitoredApplication.exe",
                ApplicationPath = "MonitoredApplication",
                Active = true
            };
            //var applicationHandler = new ApplicationHandler("MonitoredApplication", @"D:\DevelPers\WatchDog\MonitoredApplication\bin\Release\MonitoredApplication.exe", 10, 10, 1, 1,false, true) {Active = true};
        }

    }


    public class ApplicationHandlerConfig
    {
        public int NonResponsiveInterval    { get; set; }
        public string ApplicationPath       { get; set; }
        public string ApplicationName       { get; set; }
        public bool UseHeartbeat            { get; set; }
        public bool GrantKillRequest        { get; set; }
        public uint HeartbeatInterval       { get; set; }
        public int MaxProcesses             { get; set; }
        public int MinProcesses             { get; set; }
        public bool Active                  { get; set; }
        public bool KeepExistingNoProcesses { get; set; }

        public ApplicationHandlerConfig()
        {
            NonResponsiveInterval   = 20;
            ApplicationPath         = "";
            ApplicationName         = "";
            UseHeartbeat            = false;
            GrantKillRequest        = true;
            HeartbeatInterval       = 20;
            MinProcesses            = 1;
            MaxProcesses            = 10;
            Active                  = false;
            KeepExistingNoProcesses = true;
        }
    }


}