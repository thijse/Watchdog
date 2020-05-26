using System;
using System.Collections.Generic;

namespace WatchdogLib
{
    public class Configuration
    {
        public enum RebootModes
        {
            ShutDown,      
            Reboot,        
            PowerOff,      
            HybridShutdown,
        };


        public enum ForceModes
        {
             Normal,     
             Force,      
             ForceIfHung,
        };

        public enum RebootAfterTimeSlotModes
        { 
            FirstOccasion,
            TryNextDay
        };

    
        public List<ApplicationHandlerConfig> ApplicationHandlers { get; set; }
        public bool ShowTrayIcon { get; set; }
        public bool PeriodicReboot { get; set; }
        public int RebootPeriod { get; set; }
        public DateTime RebootBefore { get; set; }
        public DateTime RebootAfter { get; set; }
        public RebootModes RebootMode { get; set; }
        public RebootModes ForceMode { get; set; }
        public RebootAfterTimeSlotModes RebootAfterTimeSlot { get; set; }

        public Configuration()
        {
            ApplicationHandlers = new List<ApplicationHandlerConfig>();
            ShowTrayIcon = true;

            //var app = new ApplicationHandlerConfig()
            //{
            //    ApplicationPath = @"D:\DevelPers\WatchDog\MonitoredApplication\bin\Release\MonitoredApplication.exe",
            //    ApplicationName = "MonitoredApplication",
            //    Active = true
            //};
            //ApplicationHandlers.Add(app);
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

        public uint StartupMonitorDelay      { get; set; }

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
            StartupMonitorDelay     = 20;
        }
    }


}