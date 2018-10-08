using System;
using System.Collections.Generic;
using System.Timers;
using NLog;

namespace WatchdogLib
{
    public class RebootHandler
    {
        private Logger _logger;
        private Timer _timer;
       // private bool _periodicReboot;
        private RebootConfig _rebootConfig;

        private Dictionary<Configuration.RebootModes, ShutdownType> _rebootModeLookup = new Dictionary<Configuration.RebootModes, ShutdownType>()
        {
            { Configuration.RebootModes.Reboot, ShutdownType.Reboot },
            { Configuration.RebootModes.HybridShutdown, ShutdownType.HybridShutdown },
            { Configuration.RebootModes.PowerOff, ShutdownType.PowerOff },
            { Configuration.RebootModes.ShutDown, ShutdownType.ShutDown },
        };

        private Dictionary<Configuration.ForceModes, ForceExit> _forceModeLookup = new Dictionary<Configuration.ForceModes, ForceExit>()
        {
            { Configuration.ForceModes.Force, ForceExit.Force },
            { Configuration.ForceModes.ForceIfHung, ForceExit.ForceIfHung },
            { Configuration.ForceModes.Normal, ForceExit.Normal},
        };


        public RebootHandler(Logger logger)
        {
            _logger = logger;

            _timer = new Timer(10000);
            _timer.AutoReset = false; // makes it fire only once
            _timer.Elapsed += TimerElapsed;
            //_timer.Enabled = true; // Enable it
        }

         void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var uptime = Reboot.GetUptime();
             //if (uptime.TotalDays< _rebootConfig.RebootPeriod) return;
             if ((int) uptime.TotalDays == _rebootConfig.RebootPeriod) // If exact amount of days has past
             {
                 var timeOfDay = DateTime.Now.TimeOfDay;
                 if (timeOfDay > _rebootConfig.RebootAfter.TimeOfDay)
                 {
                     if (timeOfDay < _rebootConfig.RebootBefore.TimeOfDay)
                     {
                        RebootPc();
                        return;
                     }
                     else
                     {
                         if (_rebootConfig.RebootAfterTimeSlot == Configuration.RebootAfterTimeSlotModes.FirstOccasion)
                            RebootPc();
                            return;
                    }
                 }
             }
             else if ((int)uptime.TotalDays > _rebootConfig.RebootPeriod) // If more days have past
             {
                 if (_rebootConfig.RebootAfterTimeSlot == Configuration.RebootAfterTimeSlotModes.FirstOccasion)
                 {
                     RebootPc();
                     return;
                 }
                 else
                 {
                    var timeOfDay = DateTime.Now.TimeOfDay;
                     if (timeOfDay > _rebootConfig.RebootAfter.TimeOfDay &&
                         timeOfDay < _rebootConfig.RebootBefore.TimeOfDay)
                     {
                         RebootPc();
                        return;
                    }
                 }
             }
             _timer.Start();
        }

        private void RebootPc()
        {
            Reboot.Shutdown(_rebootModeLookup[_rebootConfig.RebootMode], _forceModeLookup[_rebootConfig.ForceMode]);
        }

        public void Set(RebootConfig rebootConfig)
        {
            _rebootConfig = rebootConfig;
            _timer.Enabled = _rebootConfig.PeriodicReboot;
        }
    }
}
