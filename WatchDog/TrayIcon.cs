using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NLog;
using Utilities;
using WatchdogLib;

namespace WatchDog
{
    namespace TrayIconTest
    {
        class TrayIcon : ApplicationContext
        {
            private bool _disposed;

            //Component declarations
            private NotifyIcon _trayIcon;
            private LogForm _logForm;
            private ContextMenuStrip _trayIconContextMenu;
            private ToolStripMenuItem _showLogMenuItem;
            private ToolStripMenuItem _closeMenuItem;
            private Logger _logger;
            private MainForm  _mainForm;
            private Configuration _configuration;
            private ConfigurationSerializer<Configuration> _configurationSerializer;

            public TrayIcon()
            {
                Application.ApplicationExit += OnApplicationExit;
                InitializeComponent();

                _configuration = new Configuration();
                _configurationSerializer = new Utilities.ConfigurationSerializer<Configuration>("configuration.json",_configuration);
                _configuration =  _configurationSerializer.Deserialize();


                // Todo visibility dependent on configuration. If not, only show config form on 2nd startup

                _logForm = new LogForm() {Visible = false };
                _mainForm = new MainForm();

                _trayIcon.Visible = _configuration.ShowTrayIcon;
                InitializeApplication();

                //Debug.WriteLine(Reboot.GetUptime());
                //Debug.WriteLine(Reboot.GetUptime2());
                //Reboot.Shutdown(ShutdownType.Reboot, ForceExit.Force,ShutdownReason.MinorOther);
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

                    _logger = LogManager.GetLogger("WatchdogServer");
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

                    // Show log
                    _showLogMenuItem = new ToolStripMenuItem
                    {
                        Name = "_showLogMenuItem",
                        Size = new Size(152, 22),
                        Text = "Show watchdog log"
                    };
                    _showLogMenuItem.Click += ShowLogMenuItemClick;
                    _trayIconContextMenu.Items.AddRange(new ToolStripItem[] { _showLogMenuItem });

                    // CloseMenuItem
                    _closeMenuItem = new ToolStripMenuItem
                    {
                        Name = "_suspendResumeMenuItem",
                        Size = new Size(152, 22),
                        Text = "Suspend watchdog server"
                    };
                    _closeMenuItem.Click += CloseMenuItemClick;
                    _trayIconContextMenu.Items.AddRange(new ToolStripItem[] { _closeMenuItem });



                    _trayIconContextMenu.ResumeLayout(false);
                    _trayIcon.ContextMenuStrip = _trayIconContextMenu;
                }
                catch (Exception ex)
                {
                    ExceptionsManager.ServerCrash("Exception Watchdog initialization", "Exception during Watchdog initialization :" + ex.Message, true);
                }
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
                ApplicationExit();
            }


            private void InitializeApplication()
            {
                try
                {
                    _logger = LogManager.GetLogger("WatchdogServer");
                    var nlogEventTarget =  Utilities.NlogEventTarget.Instance;
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


            private void OnLogEvent(object sender, Utilities.LogEventArgs logEventArgs)
            {

                _logForm.LoggingView.AddEntry(logEventArgs.LogLines.ToArray());
                foreach (var logLine in logEventArgs.LogLines)
                {
                    Debug.WriteLine(logLine);
                }
            }


            private void MainApplication(string[] args)
            {
                ApplicationWatcher applicationWatcher = new ApplicationWatcher(_logger);
                applicationWatcher.Deserialize(_configuration);
                //applicationWatcher.AddMonitoredApplication("MonitoredApplication", @"D:\DevelPers\WatchDog\MonitoredApplication\bin\Release\MonitoredApplication.exe",10,10,1,1,false,true,true,true);
                //var applicationHandler = new ApplicationHandler("MonitoredApplication", @"D:\DevelPers\WatchDog\MonitoredApplication\bin\Release\MonitoredApplication.exe", 10, 10, 1, 1,false, true) {Active = true};
                //applicationWatcher.ApplicationHandlers.Add(applicationHandler);

                new MainFormVm(_mainForm, applicationWatcher, _configuration, _configurationSerializer);
            }



            private void ApplicationExit()
            {
                _trayIcon.Visible = false;
                OnApplicationExit(this, null);
            }

        }
    }
}
