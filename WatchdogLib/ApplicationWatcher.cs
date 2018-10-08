// <copyright file="ApplicationWatcher.cs" company="DevThread">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Thijs Elenbaas</author>
// <summary>The ApplicationWatcher class implementation. This is the actual watchdog.</summary>

using System.Collections.Generic;
using System.Threading;
using NLog;
using Utilities;

namespace WatchdogLib
{
    public class ApplicationWatcher
    {
        private readonly CountDown _threadSleepCountDown;
        private readonly Logger _logger;
        private readonly AsyncWorker _asyncWorkerMonitor;

        public List<ApplicationHandler> ApplicationHandlers { get; set; }

        public ApplicationWatcher(Logger logger)
        {
            ApplicationHandlers   = new List<ApplicationHandler>();
            _threadSleepCountDown = new CountDown(1000);
            _asyncWorkerMonitor   = new AsyncWorker(MonitorJob) {Name = "ApplicationWatcher"};
            _asyncWorkerMonitor.Start();
            _logger = logger;
        }

        private bool MonitorJob()
        {
            // Walk through list of applications to see which ones are running
            _threadSleepCountDown.Restart();
            foreach (var applicationHandler in ApplicationHandlers.ToArray())
            {                
                applicationHandler.Check();                
            }
            Thread.Sleep( (int)_threadSleepCountDown.RemainingMilliSeconds);
            _threadSleepCountDown.Reset();
            return true;
        }

        public void Deserialize(Configuration configuration)
        {
            foreach (var applicationHandlerConfig in configuration.ApplicationHandlers)
            {
                var applicationHandler = new ApplicationHandler(applicationHandlerConfig);
                ApplicationHandlers.Add(applicationHandler);
            }
        }

        public void Restart()
        {
            foreach (var applicationHandler in ApplicationHandlers)
            {
                applicationHandler.Restart();
            }
        }

        public void Start()
        {
            _logger.Info("Restarting watchdog");
            Restart();
           _asyncWorkerMonitor.Start();            
        }

        public void Stop()
        {
            _logger.Info("Stopping watchdog");
           _asyncWorkerMonitor.Stop();
        }
    }
}
