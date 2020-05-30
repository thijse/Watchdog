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
Per application many watchdog parameters can be customized  
![Watchdog application settings](/Screenshots/ApplicationSettingsMenu.png)

## Persistence 
The application can be configured to start at on startup and/or be periodically checked to be running  
![Watchdog general settings, determine restart behaviour](/Screenshots/GeneralSettingsMenu.png)

## Automatic reboot
The application allows automatic reboots  
![Watchdog reboot menu](/Screenshots/RebootMenu.png)
