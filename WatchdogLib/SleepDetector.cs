using System;
using System.Timers;
using Microsoft.Win32;
using NLog;

namespace WatchdogLib
{
    public class SleepDetector
    {

        private static SleepDetector _instance;
        private DateTime             _lastTime;
        private DateTime             _lastSleepTime;
        private readonly int         _pollTimeOut;
        //private readonly int         _sleepTimeOut;
        private readonly Timer       _timer;
        private bool _elapsed;
        private Logger _logger;

        public int SleepTimeOut { get; set; }

        private bool JustSlept(int sleepTimeOut)
        {
            if ((DateTime.Now - _lastSleepTime) < TimeSpan.FromMilliseconds(sleepTimeOut))
            {
                //_logger.Info("PC just slept");
                return true;
            } else
            {
               // _logger.Info("PC did not sleep");
                return false;
            } 
        }

        public bool SleepTimeOutElapsed
        {
            get
            {
                if (JustSlept() || _elapsed) return false;
                _elapsed = true;
                return true;
            }
        }

        public TimeSpan SinceSleep()
        {
            return DateTime.Now - _lastSleepTime;
        }

        public DateTime LastSleep()
        {
            return _lastSleepTime;
        }

        public bool JustSlept()
        {
            return JustSlept(SleepTimeOut);
        }

        public static SleepDetector Instance
        {
            get { return _instance ?? (_instance = new SleepDetector()); }
        }

        private SleepDetector()
        {
            _logger = LogManager.GetLogger("WatchdogServer");
            SleepTimeOut                   = 10*1000;
            _pollTimeOut                   = 100;             
            _lastSleepTime                 = DateTime.MinValue;
            _lastTime                      = DateTime.Now;
            _timer                         = new Timer(_pollTimeOut);
            _timer.Elapsed                += TimerElapsed;
            _timer.AutoReset               = false;
            SystemEvents.PowerModeChanged += OnPowerChange;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - _lastTime) > TimeSpan.FromMilliseconds(2*_pollTimeOut))
            {
                _lastSleepTime = DateTime.Now;
                _elapsed = false;
            }
            _lastTime = DateTime.Now;
            _timer.Start();
        }

        private void OnPowerChange(object sender, PowerModeChangedEventArgs e)
        {
            switch ( e.Mode ) 
            {
                case PowerModes.Resume:
                case PowerModes.Suspend:
                case PowerModes.StatusChange: 
                    _lastSleepTime = DateTime.Now;  
                    _elapsed = false;
                    break;
            }
        }


    }
}
