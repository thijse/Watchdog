// <copyright file="ApplicationWatcher.cs" company="DevThread">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Thijs Elenbaas</author>
// <summary>The ApplicationWatcher class implementation. This is the actual watchdog.</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NLog;
using Utilities;

namespace WatchdogLib
{
    public class ApplicationWatcher
    {
        private readonly Stopwatch _sleepStopwatch;
        private readonly Logger _logger;


        //public List<ProcessHandler> ProcessHandlers { get; set; }
        public List<ApplicationHandler> ApplicationHandlers { get; set; }


        public ApplicationWatcher(Logger logger)
        {
            //ProcessHandlers= new List<ProcessHandler>();
            ApplicationHandlers = new List<ApplicationHandler>();
            _sleepStopwatch = new Stopwatch();
            var asyncWorkerMonitor = new AsyncWorker(MonitorJob) {Name = "ApplicationWatcher"};
            asyncWorkerMonitor.Start();
            _logger = logger;

        }

        private bool MonitorJob()
        {
            // Walk through list of applications to see which ones are running
            _sleepStopwatch.Restart();
            foreach (var applicationHandler in ApplicationHandlers.ToArray())
            {                
                applicationHandler.Check();                
            }
            Thread.Sleep(Math.Max(0, 500 - (int)_sleepStopwatch.ElapsedMilliseconds));
            return true;
        }

  

        public bool AddMonitoredApplication(string applicationName, string applicationPath, int nonResponsiveInterval, uint heartbeatInterval = 15, int minProcesses = 1, int maxProcesses = 1, bool keepExistingNoProcesses = false, bool useHeartbeat = false, bool grantKillRequest = true, bool active=false, uint startupMonitorDelay=20)
        {
            _logger.Trace("Registering {0} for monitoring", applicationName);
            ApplicationHandlers.Add(item: new ApplicationHandler(applicationName, applicationPath, nonResponsiveInterval, heartbeatInterval, minProcesses, maxProcesses, keepExistingNoProcesses, useHeartbeat, grantKillRequest, startupMonitorDelay) { Logger = _logger }) ;
            return true; // todo
        }


        public void Deserialize(Configuration configuration)
        {
            foreach (var applicationHandlerConfig in configuration.ApplicationHandlers)
            {
                var applicationHandler = new ApplicationHandler(applicationHandlerConfig);
                ApplicationHandlers.Add(applicationHandler);
            }
        }
    }
}
