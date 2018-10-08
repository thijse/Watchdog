using System;
using System.Collections.Generic;

namespace WatchdogLib
{
    public class Configuration
    {

        public enum RebootModes              { ShutDown, Reboot, PowerOff, HybridShutdown };
        public enum ForceModes               { Normal, Force, ForceIfHung };
        public enum RebootAfterTimeSlotModes { FirstOccasion, TryNextDay };

        public List<ApplicationHandlerConfig> ApplicationHandlers         { get; set; }
        public bool ShowTrayIcon                                          { get; set; }
        public RebootConfig Reboot                                        { get; set; }
        public bool         RestartOnTask                                 { get; set; }
        public bool StartOnWindowsStart                                   { get; set; }

        public Configuration()
        {
            ApplicationHandlers = new List<ApplicationHandlerConfig>();
            ShowTrayIcon        = true;
            Reboot              = new RebootConfig();
            RestartOnTask       = true;
            StartOnWindowsStart = true;
        }
    }

    public class RebootConfig
    {
        public bool PeriodicReboot                                        { get; set; }
        public int RebootPeriod                                           { get; set; }
        public DateTime RebootBefore                                      { get; set; }
        public DateTime RebootAfter                                       { get; set; }
        public Configuration.RebootModes RebootMode                       { get; set; }
        public Configuration.ForceModes ForceMode                         { get; set; }
        public Configuration.RebootAfterTimeSlotModes RebootAfterTimeSlot { get; set; }

        public RebootConfig()
        {
            PeriodicReboot      = false;
            RebootPeriod        = 30; // days
            RebootAfter         = new DateTime(2000,1,1,23,00,00);
            RebootBefore        = new DateTime(2000, 1, 1, 23, 59, 00);
            RebootMode          = Configuration.RebootModes.Reboot;
            ForceMode           = Configuration.ForceModes.ForceIfHung;
            RebootAfterTimeSlot = Configuration.RebootAfterTimeSlotModes.TryNextDay;
        }
    }

    public class ApplicationHandlerConfig
    {
        public int NonResponsiveInterval                                  { get; set; }
        public string ApplicationPath                                     { get; set; }
        public string ApplicationName                                     { get; set; }
        public bool UseHeartbeat                                          { get; set; }
        public bool IgnoreHeartBeatIfNeverAcquired                        { get; set; }
        public bool GrantKillRequest                                      { get; set; }
        public int HeartbeatInterval                                      { get; set; }
        public int MaxProcesses                                           { get; set; }
        public int MinProcesses                                           { get; set; }
        public bool Active                                                { get; set; }
        public bool KeepExistingNoProcesses                               { get; set; }
        public int StartupMonitorDelay                                    { get; set; }
        public int TimeBetweenRetry                                       { get; set; }
        public string ApplicationArguments                                { get; set; }
        

        public ApplicationHandlerConfig()
        {
            NonResponsiveInterval          = 20;
            ApplicationPath                = "";
            ApplicationName                = "";
            ApplicationArguments           = "";
            UseHeartbeat                   = false;
            IgnoreHeartBeatIfNeverAcquired = true;
            GrantKillRequest               = true;
            HeartbeatInterval              = 20;
            MinProcesses                   = 1;
            MaxProcesses                   = 10;
            Active                         = true;
            KeepExistingNoProcesses        = true;
            StartupMonitorDelay            = 20;
            TimeBetweenRetry               = 60; 
        }
    }


}