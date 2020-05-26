using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using NLog;
using NLog.Targets;
using System.Timers;
using NLog.Config;

namespace Utilities
{
    public class LogEventArgs : EventArgs
    {
        public List<string> LogLines { get; private set; }

        public LogEventArgs(List<string> logLines)
        {
            LogLines = logLines;
        }
    }

    [Target("NlogEvent")]
    public class NlogEventTarget : TargetWithLayout
    {
        public EventHandler<LogEventArgs>         OnLogEvent;
        private readonly ConcurrentQueue<string> _logQueue;

        private static NlogEventTarget _instance;

        public static NlogEventTarget Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NlogEventTarget();
                    Register(_instance);
                }
                return _instance;
            }
        }

        public NlogEventTarget()
        {
            _logQueue      = new ConcurrentQueue<string>();
            var timer      = new Timer(100);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public static void Register(NlogEventTarget nlogEventTarget)
        {

            nlogEventTarget.Name = "event";
            nlogEventTarget.Layout = "${longdate} ${uppercase:${level}} ${message}";

            var config = LogManager.Configuration;
            config.AddTarget("nlogEvent", nlogEventTarget);
            var rule = new LoggingRule("*", LogLevel.Trace, nlogEventTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            LogManager.Configuration.Reload();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var logList    = new List<string>();
            var localValue = "";
            while (_logQueue.TryDequeue(out localValue)) logList.Add(localValue);

            if (logList.Count <= 0) return;
            if (OnLogEvent == null) return;
            var args = new LogEventArgs(logList);

            OnLogEvent(this, args);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = this.Layout.Render(logEvent);
            _logQueue.Enqueue(logMessage);
        }
    }
}