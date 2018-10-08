using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using NLog;
using Utilities;
using WatchdogLib;

namespace WatchDog
{
    namespace TrayIconTest
    {
        // Todo
        // - Make task per application
        // - Allow monitoring other path/processname
        // - Add logging to reboot code
        // - Halt reboot when system is not idle
        // - Re-add logging screen
        // - Add critical section start/stop to watchdoglib
        // - Add watchdog executable to send heartbeat etc
        // - Add different kill methods
        // - Add restore window position, size, z-order
        // - Show screenshot during application restart
        // - Add reboot countdown
        

        class TrayIcon : ApplicationContext
        {
            private bool _disposed;
            private NotifyIcon _trayIcon;
            private ContextMenuStrip _trayIconContextMenu;
            private ToolStripMenuItem _showLogMenuItem;
            private ToolStripMenuItem _closeMenuItem;
            private Logger _logger;
            private readonly LogForm _logForm;
            private readonly MainForm  _mainForm;
            private readonly Configuration _configuration;
            private readonly JsonConfigurationSerializer<Configuration> _configurationSerializer;
            private ApplicationWatcher _applicationWatcher;
            private RebootHandler _rebootHandler;
            private ToolStripMenuItem _startStopMenuItem;
            private ToolStripMenuItem _mainMenuItem;
            private MainFormVm _mainFormVm;

            public TrayIcon()
            {
                _logger = LogManager.GetLogger("WatchdogServer");
                if (SystemUtils.InstanceAlreadyRunning())
                {
                    _logger.Info("Another instance of watchdog is already running, aborting");
                     System.Windows.Forms.Application.Exit();
                     System.Environment.Exit(0);
                     return;
                }

                Application.ApplicationExit     += OnApplicationExit;
                SystemEvents.PowerModeChanged   += OnPowerChange;
                InitializeComponent();

                _configuration = new Configuration();
                _configurationSerializer = new JsonConfigurationSerializer<Configuration>("configuration.json",_configuration);
                _configuration =  _configurationSerializer.Deserialize();

                RegisterWatchdogTask.SetTask(_configuration.RestartOnTask);    
                Startup.SetStartup(_configuration.StartOnWindowsStart);            
                _mainForm = new MainForm();
                _logForm = new LogForm()
                {
                    Visible = false
                };
                _trayIcon.Visible = _configuration.ShowTrayIcon;
                InitializeApplication();
            }

            ~TrayIcon()
            {
                if (_trayIcon != null)
                {
                    _trayIcon.Visible = false;
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // free other managed objects that implement
                        if (_trayIcon != null)
                        {
                            _trayIcon.Visible = false;
                            _trayIcon.Dispose();
                            _trayIcon = null;
                        }
                    }

                    // release any unmanaged objects
                    // set object references to null

                    _disposed = true;
                }

                base.Dispose(disposing);
            }


            private void InitializeComponent()
            {
                try
                {
                    var path = Path.GetDirectoryName(Application.ExecutablePath);
                    if (!String.IsNullOrEmpty(path) && Directory.Exists(path))
                        Directory.SetCurrentDirectory(path);

                    
                    ExceptionsManager.Logger = _logger;

                    _trayIcon = new NotifyIcon();
                    ExceptionsManager.TrayIcon = _trayIcon;
                    _trayIcon.Text = "Watchdog";                    
                    _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                    _trayIcon.BalloonTipText = "Starting watchdog";
                    _trayIcon.BalloonTipTitle = "Starting watchdog";
                    _trayIcon.Text = "Watchdog";

                    //The icon is added to the project resources.
                    _trayIcon.Icon = Resources.watchdog;

                    //Optional - handle double-clicks on the icon:
                    _trayIcon.Click += TrayIconClick;
                    _trayIcon.DoubleClick += TrayIconDoubleClick;

                    //Optional - Add a context menu to the TrayIcon:
                    _trayIconContextMenu = new ContextMenuStrip();

                    _trayIconContextMenu.SuspendLayout();
                                    
                    _trayIconContextMenu.Name = "_trayIconContextMenu";
                    _trayIconContextMenu.Size = new Size(153, 70);


                     // Watchdog menu
                    
                    _mainMenuItem = new ToolStripMenuItem
                    {
                        Name = "_startStopMenuItem",
                        Size = new Size(152, 22),
                        Text = "Watchdog settings"
                    };
                    _mainMenuItem.Click += MainMenuItemClick;
                    _trayIconContextMenu.Items.AddRange(new ToolStripItem[] { _mainMenuItem });


                    // Show log
                    _showLogMenuItem = new ToolStripMenuItem
                    {
                        Name = "_showLogMenuItem",
                        Size = new Size(152, 22),
                        Text = "Show log"
                    };
                    _showLogMenuItem.Click += ShowLogMenuItemClick;
                    _trayIconContextMenu.Items.AddRange(new ToolStripItem[] { _showLogMenuItem });
                    
                    // StartStopMenuItem
                    
                    _startStopMenuItem = new ToolStripMenuItem
                    {
                        Name = "_startStopMenuItem",
                        Size = new Size(152, 22),
                        Text = _mainFormVm==null?"Start watchdog":_mainFormVm.WatchdogRunning?"Stop watchdog":"Start watchdog"
                    };
                    
                    _startStopMenuItem.Click += StartStopMenuItemClick;
                    _trayIconContextMenu.Items.AddRange(new ToolStripItem[] { _startStopMenuItem });
                    _trayIconContextMenu.ResumeLayout(false);
                    _trayIcon.ContextMenuStrip = _trayIconContextMenu;


                    _trayIconContextMenu.Items.Add("-");
                    
                    // CloseMenuItem
                    _closeMenuItem = new ToolStripMenuItem
                    {
                        Name = "_exitMenuItem",
                        Size = new Size(152, 22),
                        Text = "Exit watchdog"
                    };
                    _closeMenuItem.Click += CloseMenuItemClick;
                    _trayIconContextMenu.Items.AddRange(new ToolStripItem[] { _closeMenuItem });
                    _trayIconContextMenu.ResumeLayout(false);
                    _trayIcon.ContextMenuStrip = _trayIconContextMenu;

                    //var register = new RegisterWatchdogTask();
                    
                    //RegisterWatchdogTask.CreateTask();

                }
                catch (Exception ex)
                {
                    ExceptionsManager.ServerCrash("Exception Watchdog initialization", "Exception during Watchdog initialization :" + ex.Message, true);
                }
            }

            private void MainMenuItemClick(object sender, EventArgs e)
            {
                _mainForm.Visible = true;
            }

            private void StartStopMenuItemClick(object sender, EventArgs e)
            {
               
                _mainFormVm.WatchdogRunning = !_mainFormVm.WatchdogRunning;
                _startStopMenuItem.Text = _mainFormVm==null?"Start watchdog":_mainFormVm.WatchdogRunning?"Stop watchdog":"Start watchdog";
               
                
            }

            private void ShowLogMenuItemClick(object sender, EventArgs e)
            {
                _logForm.Visible = true;
            }

            private void OnApplicationExit(object sender, EventArgs e)
            {
                _logger.Info("Stopping the watchdog application");

                if (_trayIcon != null) _trayIcon.Visible = false;


            }

            private void TrayIconClick(object sender, EventArgs e)
            {
                var me = (MouseEventArgs)e;
                if (me.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    //_logForm.Visible = true;
                    _mainForm.Visible = true;
                }
            }

            private void TrayIconDoubleClick(object sender, EventArgs e)
            {
                _logForm.Visible = true;
            }

            private void CloseMenuItemClick(object sender, EventArgs e)
            {

                var message = "The Watchdog will now exit. The watchdog will ";
                    message = message + (_configuration.RestartOnTask? "restart within 5 minutes" :_configuration.StartOnWindowsStart ? "restart when the system is rebooted" : "not restart automatically");
                    message = message + ". This behavior can be configured under \"Watchdog settings\" > \"General settings\"";
                MessageBox.Show(message,
                    "Exit Watchdog",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                ApplicationExit();
                Application.Exit();
            }

            private void InitializeApplication()
            {
                try
                {
                    _logger = LogManager.GetLogger("WatchdogServer");
                    var nlogEventTarget =  NlogEventTarget.Instance;
                    nlogEventTarget.OnLogEvent+= OnLogEvent;

                    Directory.CreateDirectory(FileUtils.Combine(Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath), "\\logs"));

                    _logger.Info("Initializing watchdog application");
                    string[] args = Environment.GetCommandLineArgs();

                    // Strip application from arguments
                    args = args.Where(w => w != args[0]).ToArray();
                    MainApplication(args);
                }
                catch (Exception ex)
                {
                    ExceptionsManager.ServerCrash("Exception during Watchdog initialization", "Exception during Watchdog initialization :" + ex.Message, true);
                }
            }


            private void OnLogEvent(object sender, LogEventArgs logEventArgs)
            {
                _logForm.LoggingView.AddEntry(logEventArgs.LogLines.ToArray());
                foreach (var logLine in logEventArgs.LogLines)
                {
                    Debug.WriteLine(logLine);
                }
            }


            private void MainApplication(string[] args)
            {
                _applicationWatcher = new ApplicationWatcher(_logger);
                _rebootHandler = new RebootHandler(_logger);
                _applicationWatcher.Deserialize(_configuration);
                _rebootHandler.Set(_configuration.Reboot);
                _mainFormVm = new MainFormVm(_mainForm, _applicationWatcher, _rebootHandler, _configuration, _configurationSerializer);
                _startStopMenuItem.Text = _mainFormVm==null?"Start watchdog":_mainFormVm.WatchdogRunning?"Stop watchdog":"Start watchdog";
                _mainFormVm.WatchdogRunningChanged+=(e,s)=> {
                    _startStopMenuItem.Text =_mainFormVm.WatchdogRunning?"Stop watchdog":"Start watchdog";
                }
                ;
            }

        private void OnPowerChange(object sender, PowerModeChangedEventArgs e)
        {
            switch ( e.Mode ) 
            {
                case PowerModes.Resume: 
                    _logger.Info("PC is waking up from sleep. ");
                    if (_applicationWatcher!=null) _applicationWatcher.Restart();
                break;
                case PowerModes.Suspend:
                    _logger.Info("PC is going to sleep.");
                    if (_applicationWatcher!=null) _applicationWatcher.Restart();
                break;
            }
        }

            private void ApplicationExit()
            {
                _trayIcon.Visible = false;
                OnApplicationExit(this, null);
            }
        }
    }
}
