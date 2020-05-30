Watchdog
====================
[![License](https://img.shields.io/badge/license-MIT%20License-blue.svg)](http://doge.mit-license.org)

*CSharp Watchdog application. It will monitor applications and restart if necessary.*

Watchdog is an application that can monitor as many applications as you want from the system tray. If an application exits it can be restarted. The watchdog is very configurable, e.g. how often it is polled, how often it will try to restart etc.

The watchdog also comes with a single client library that you can integrate in your own application. It implements a heartbeat system. This means if the application is not sending a heartbeat anymore, the Watchdog can restart on this.

# Overview
## System tray
The application lives in the system tray, where it can be enabled, disabled and configured. The application can be configured through Settings and stopped or exited. Note that if your exit the watchdog it may automatically respawn, based on the general settings. The watchdog can also be stopped using the global shortcut using [CTR][ALT][W]. This is useful if the application that is monitored blocks the watchdog UI.
![Watchdog in system tray](/Screenshots/TaskbarMenu.png)

## Selecting applications to be watched
The application allows watching as many applications as you need. The Crashing Application demonstrates different manners in which an application can exit, crash or freeze.   
![Watchdog application selection](/Screenshots/ConfigurationForm.png)

## Watchdog settings 
![Watchdog application settings](/Screenshots/ApplicationSettingsMenu.png)  
Multiple watchdog parameters can be modified per application  
* Path         - The path of the application. The working directory is also based on this path
* Arguments    - Startup arguments of the application
* Process name - Name of the process when running. This is often the application name, but not always. It is used to monitor if the application (and how many) is running. 
* Start once   - Allows you to test if the settings are correct and will indeed start up the application
* Min number of processes - The minimal number of processes of the application that should be running, this is typically 1, but for some applications (like servers) you may want to run more
* Max number of processes - If more than the indicated number of processes are running, processes will be killed
* Use Heartbeat  - This refers to the heartbeat library you can implement in your own application
* Ignore Heartbeat if never acquired - This means that if your application will only be restarted if it had a heartbeat at some point but it stopped
* Max interval heartbeats - Maximum time between hearbeats. If more time occurs between two heartbeats, the watchdog wil restart. Make sure your application sends a heartbeat more often (at least a factor 2)
* Max unresponsive interval - Maximum time that the application may be unresponsive.
* Startup monitor delay - the time between starting an application and the first time that polling occurs. It may take an application some time to start properly and become responsive
* Time between retries - the time between restarting the application
* Active / in active - monitoring starts when the application is set to "Active" (and the watchdog is running)

## Persistence 
The application can be configured to start at on startup and/or be periodically checked to be running  
![Watchdog general settings, determine restart behaviour](/Screenshots/GeneralSettingsMenu.png)  
* Start Watchdog on Windows Startup - The application starts when the user logs in (the application does not run as a service) 
* Periodically check if Watchdog is running - If enabled the Task Scheduler will try to start the application every 5 minutes
## Automatic reboot
The application allows automatic, periodic reboots This may be useful to keep the system and it's applications run reliably.   
![Watchdog reboot menu](/Screenshots/RebootMenu.png)
