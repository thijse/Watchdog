using System;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
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
        private readonly ConfigurationSerializer<Configuration> _serializer;

        public MainFormVm(MainForm mainForm, ApplicationWatcher applicationWatcher, Configuration configuration, Utilities.ConfigurationSerializer<Configuration> serializer)
        {
            _mainForm                                                    = mainForm;
            _applicationWatcher                                          = applicationWatcher;
            _configuration                                               = configuration;
            _serializer                                                  = serializer;
            _selectedItem                                                = null;
            // todo load item
            _mainForm.listBoxMonitoredApplications.SelectedIndexChanged += this.ListBoxMonitoredApplicationsSelectedIndexChanged;

            _mainForm.buttonAddProcess.Click                            += ButtonAddProcessClick;
            _mainForm.buttonDeleteProcess.Click                         +=ButtonDeleteProcessOnClick;
            _mainForm.buttonEditProcess.Click                           += ButtonEditProcessClick;
            _mainForm.buttonRebootSettings.Click                        += ButtonRebootSettingsClick;

            foreach (var applicationHandlerConfig in configuration.ApplicationHandlers)
            {
                _mainForm.listBoxMonitoredApplications.Items.Add(applicationHandlerConfig.ApplicationName);
            }
            SelectMenuItemInList(0);
        }

        private void ButtonRebootSettingsClick(object sender, EventArgs e)
        {
            var rebootForm = new RebootForm();
            //var rebootFormVm = new RebootFormVm(editForm,applicationHandlerConfig, _configuration);
            rebootForm.ShowDialog(_mainForm);
        }


        private void ButtonAddProcessClick(object sender, EventArgs e)
        {
            var applicationHandlerConfig = new ApplicationHandlerConfig();
            _configuration.ApplicationHandlers.Add(applicationHandlerConfig);

            var editForm = new EditForm();
            var editFormVm = new EditFormVm(editForm,applicationHandlerConfig, _configuration);
            editForm.ShowDialog(_mainForm);

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

        }

        private void ButtonDeleteProcessOnClick(object sender, EventArgs eventArgs)
        {
            var i = _mainForm.listBoxMonitoredApplications.SelectedIndex;
            _mainForm.listBoxMonitoredApplications.Items.RemoveAt(i);
            _configuration.ApplicationHandlers.RemoveAt(i);            
            _serializer.Serialize(_configuration);

            i = Math.Max(0, i - 1);
            SelectMenuItemInList(Math.Max(0,i-1));
            SetForm(_configuration.ApplicationHandlers[i]);

        }

        private void UpdateHandler()
        {
           // throw new NotImplementedException();
        }


        
        private void ButtonAcceptClick(object sender, EventArgs e)
        {
            AcceptChanges();
        }


        private void AcceptChanges()
        {
            //FillHandlerConfig(_selectedItem);
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
            // Check old state
            //if (i != _selectedItemNo)
            //{
            //    // check if settings have changed, if so, ask for Accept
            //    //_selectedItem = _configuration.ApplicationHandlers[_selectedItemNo];
            //    if (!SettingsSame(_selectedItem))
            //    {
            //        var result = MessageBox.Show("This Application has not been applied. Do you want to apply and store it? If not, these updates will be lost",
            //            "Update Monitored Application",
            //            MessageBoxButtons.YesNo,
            //            MessageBoxIcon.Question);
            //        if (result == DialogResult.Yes)
            //        {
            //            AcceptChanges();
            //        }
            //    }
            //}


            if (i < _configuration.ApplicationHandlers.Count)
            {
            _selectedItem = _configuration.ApplicationHandlers[i];
            _selectedItemNo = i;

            SetForm(_selectedItem);
            }        
        }



        //private void SetDefaultSettings()
        //{
        //    _mainForm.textBoxUnresponsiveInterval.Text = "30";
        //    _mainForm.textBoxHeartbeatInterval.Text    = "15";
        //    _mainForm.textBoxHeartbeatInterval.Text    = "15";
        //    _mainForm.textBoxMaxProcesses.Text         = "1";
        //    _mainForm.textBoxMinProcesses.Text         = "1";
        //    _mainForm.textBoxProcessName.Text          = "";
        //    _mainForm.textBoxApplicationPath.Text      = "";
        //    _mainForm.checkBoxUseHeartbeat.Checked     = false;
        //    _mainForm.checkBoxGrantKillRequest.Checked = true;
        //    _mainForm.textBoxStartupMonitorDelay.Text  = "30";
        //    applicationHandlerConfig.Active 
        //}

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
