using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using GlobalHotKey;
using Utilities;
using WatchdogLib;


namespace WatchDog
{
    public  class MainFormVm
    {
        private readonly MainForm _mainForm;
        private readonly ApplicationWatcher _applicationWatcher;
        private readonly Configuration _configuration;
        private ApplicationHandlerConfig _selectedItem;
        private int _selectedItemNo;
        private readonly JsonConfigurationSerializer<Configuration> _serializer;
        private readonly RebootHandler _rebootHandler;
        private bool _watchdogRunning;
        private HotKeyManager _hotKeyManager;
        private HotKey _hotKeyStartStopWatchdog;

        public bool WatchdogRunning {
            get{
                return _watchdogRunning;
            }
            set
            {
                _watchdogRunning = value;
                 if (_watchdogRunning) {
                    _applicationWatcher.Start();
                    _mainForm.buttonStartStopWatchdog.Text = "Stop Watchdog";
                } else {
                    _applicationWatcher.Stop();
                    _mainForm.buttonStartStopWatchdog.Text = "Start Watchdog";
                }
                OnWatchdogRunningChanged(null);
            }
         }
        public event EventHandler WatchdogRunningChanged;

        protected virtual void OnWatchdogRunningChanged(EventArgs e)
        {
            if (WatchdogRunningChanged != null)
                WatchdogRunningChanged(this,e);
        }

        public MainFormVm(MainForm mainForm, ApplicationWatcher applicationWatcher, RebootHandler rebootHandler, Configuration configuration, JsonConfigurationSerializer<Configuration> serializer)
        {
            _mainForm                                                    = mainForm;
            _applicationWatcher                                          = applicationWatcher;
            _rebootHandler                                               = rebootHandler;
            _configuration                                               = configuration;
            _serializer                                                  = serializer;
            _selectedItem                                                = null;

            _mainForm.FormClosing                                       += FormClosing;
            _mainForm.listBoxMonitoredApplications.SelectedIndexChanged += this.ListBoxMonitoredApplicationsSelectedIndexChanged;

            _mainForm.buttonAddProcess.Click                            += ButtonAddProcessClick;
            _mainForm.buttonDeleteProcess.Click                         +=ButtonDeleteProcessOnClick;
            _mainForm.buttonEditProcess.Click                           += ButtonEditProcessClick;
            _mainForm.buttonRebootSettings.Click                        += ButtonRebootSettingsClick;
            _mainForm.buttonGeneralSettings.Click                       += ButtonGeneralSettingsClick;
            _mainForm.buttonStartStopWatchdog.Click                     += ButtonStartStopWatchdogClick;

            foreach (var applicationHandlerConfig in configuration.ApplicationHandlers)
            {
                _mainForm.listBoxMonitoredApplications.Items.Add(applicationHandlerConfig.ApplicationName);
            }
            SelectMenuItemInList(0);
            WatchdogRunning = true;
            Subscribe();
        }

        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            Unsubscribe();
        }

        public void Subscribe()
        {

            _hotKeyManager = new HotKeyManager();
            _hotKeyStartStopWatchdog = _hotKeyManager.Register(Key.W, ModifierKeys.Control | ModifierKeys.Alt);

            // Handle hotkey presses.
            _hotKeyManager.KeyPressed += HotKeyManagerPressed;
        }

        private void HotKeyManagerPressed(object sender, KeyPressedEventArgs e)
        {
            WatchdogRunning =! WatchdogRunning;
            Debug.WriteLine("Keypressed, watchdog is"+(WatchdogRunning?" running":" not running"));
        }

        public void Unsubscribe()
        {
            _hotKeyManager.Unregister(_hotKeyStartStopWatchdog);
            _hotKeyManager.Dispose();
        }


        private void ButtonRebootSettingsClick(object sender, EventArgs e)
        {
            var rebootForm = new RebootForm();
            var rebootFormVm = new RebootFormVm(rebootForm,_rebootHandler,_configuration.Reboot);
           
            rebootForm.ShowDialog(_mainForm);
            _serializer.Serialize(_configuration);
        }


        private void ButtonGeneralSettingsClick(object sender, EventArgs e)
        {
            var generalSettingsForm = new GeneralSettingsForm();
            var generalSettingsFormVm = new GeneralSettingsFormVM(generalSettingsForm,_configuration);
            generalSettingsForm.ShowDialog(_mainForm);
            _serializer.Serialize(_configuration);
        }

        private void ButtonStartStopWatchdogClick(object sender, EventArgs e)
        {
            WatchdogRunning =!WatchdogRunning;        
        }
        private void ButtonAddProcessClick(object sender, EventArgs e)
        {
            var applicationHandlerConfig = new ApplicationHandlerConfig();
            

            var editForm = new EditForm();
            var editFormVm = new EditFormVm(editForm,applicationHandlerConfig, _configuration);
            editForm.ShowDialog(_mainForm);
            _configuration.ApplicationHandlers.Add(applicationHandlerConfig);
            var applicationHandler = new ApplicationHandler(applicationHandlerConfig);
            _applicationWatcher.ApplicationHandlers.Add(applicationHandler);
            _serializer.Serialize(_configuration);

            _mainForm.listBoxMonitoredApplications.Items.Add(applicationHandlerConfig.ApplicationName);
            SelectMenuItemInList(_mainForm.listBoxMonitoredApplications.Items.Count - 1);

            SetForm(applicationHandlerConfig);

        }

         private void ButtonEditProcessClick(object sender, EventArgs e)
         {
             var i = _mainForm.listBoxMonitoredApplications.SelectedIndex;
             var applicationHandlerConfig = _configuration.ApplicationHandlers[i];

            var editForm = new EditForm();
            var editFormVm = new EditFormVm(editForm,applicationHandlerConfig, _configuration);
            editForm.ShowDialog(_mainForm);

            _serializer.Serialize(_configuration);
            SetForm(applicationHandlerConfig);
            _mainForm.listBoxMonitoredApplications.Items[i] = _selectedItem.ApplicationName;
            var applicationHandler = new ApplicationHandler(applicationHandlerConfig);
            _applicationWatcher.ApplicationHandlers[i] = applicationHandler;

         }

        private void ButtonDeleteProcessOnClick(object sender, EventArgs eventArgs)
        {
            var i = _mainForm.listBoxMonitoredApplications.SelectedIndex;
            if (i<0) return;
            _mainForm.listBoxMonitoredApplications.Items.RemoveAt(i);
            _configuration.ApplicationHandlers.RemoveAt(i);            
            _serializer.Serialize(_configuration);

            i = Math.Max(0, i - 1);
            SelectMenuItemInList(Math.Max(0,i-1));
            if (_configuration.ApplicationHandlers.Count > 0)
            {
                SetForm(_configuration.ApplicationHandlers[i]);
            }
        }
  
        private void ButtonAcceptClick(object sender, EventArgs e)
        {
            AcceptChanges();
        }


        private void AcceptChanges()
        {
             var i = _mainForm.listBoxMonitoredApplications.SelectedIndex;
             _mainForm.listBoxMonitoredApplications.Items[i] = _selectedItem.ApplicationName;
        }



        private void SelectMenuItemInList(int i)
        {
            if (i < _mainForm.listBoxMonitoredApplications.Items.Count)
            {
                _mainForm.listBoxMonitoredApplications.SelectedIndex = i;
            }
        }

        private void SetSelectedItem(int i)
        {            
            if (i < _configuration.ApplicationHandlers.Count)
            {
            _selectedItem = _configuration.ApplicationHandlers[i];
            _selectedItemNo = i;
            SetForm(_selectedItem);
            }        
        }


        private void SetForm(ApplicationHandlerConfig applicationHandlerConfig)
        {
     
            _mainForm.textBoxProcessName.Text          = applicationHandlerConfig.ApplicationName;
            _mainForm.textBoxApplicationPath.Text      = applicationHandlerConfig.ApplicationPath;
            _mainForm.listBoxMonitoredApplications.SelectedItem = applicationHandlerConfig.ApplicationName;         
        }

        private bool SettingsSame(ApplicationHandlerConfig applicationHandlerConfig)
        {
            return _mainForm.textBoxProcessName.Text == applicationHandlerConfig.ApplicationName &&
                   _mainForm.textBoxApplicationPath.Text == applicationHandlerConfig.ApplicationPath;
        }

        private void ListBoxMonitoredApplicationsSelectedIndexChanged(object sender, EventArgs e)
        {
            var index = _mainForm.listBoxMonitoredApplications.SelectedIndex;
            if (index<0) return;
            SetSelectedItem(index);
        }
    }
}
