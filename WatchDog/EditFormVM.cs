using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;
using NLog;
using Utilities;
using WatchdogLib;
using ProcessHandler = WatchdogLib.ProcessHandler;

namespace WatchDog
{
    public  class EditFormVm
    {
        private readonly EditForm _editApplicationsForm;
        private readonly Configuration _configuration;
        private readonly ApplicationHandlerConfig _applicationHandlerConfig;

        public EditFormVm(EditForm editApplicationsForm, ApplicationHandlerConfig applicationHandlerConfig, Configuration configuration)
        {
            _editApplicationsForm                                                    = editApplicationsForm;
            _applicationHandlerConfig                                                = applicationHandlerConfig;
            _configuration                                                           = configuration;
            SetForm();
            _editApplicationsForm.buttonSelectFile.Click                            += ButtonSelectFileClick;
            _editApplicationsForm.buttonStartOnce.Click                             += ButtonStartOnceClick;
            _editApplicationsForm.buttonActivate.Click                              += ButtonActivateClick;
            _editApplicationsForm.buttonDeactivate.Click                            += ButtonDeactivateClick;
            _editApplicationsForm.buttonAcceptChanges.Click                         += ButtonAcceptClick;
            _editApplicationsForm.textBoxApplicationPath.TextChanged                += TextBoxApplicationPathTextChanged;
        }

        private void TextBoxApplicationPathTextChanged(object sender, EventArgs e)
        {
            _editApplicationsForm.textBoxProcessName.Text =
                Path.GetFileNameWithoutExtension(_editApplicationsForm.textBoxApplicationPath.Text)??"";
        }

        private void ButtonStartOnceClick(object sender, EventArgs e)
        {
            // Start new process
            var applicationName                 = _editApplicationsForm.textBoxProcessName.Text                     ;
            var applicationPath                 = _editApplicationsForm.textBoxApplicationPath.Text                 ;
            var applicationArguments            = _editApplicationsForm.textBoxApplicationArguments.Text            ;
            
            if (File.Exists(applicationPath))
            {
                var processHandler = new ProcessHandler{                  };
                processHandler.CallExecutable(applicationPath, applicationName, applicationArguments);                    
            }
            else
            {
                MessageBox.Show("path does not exist",
                    "Application path does not exist, cannot start application",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                
            }
        }

        private void ButtonSelectFileClick(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter           = "executable files |*.exe;*.com;*.bat|All files|*.*",
                RestoreDirectory = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filenamePath = openFileDialog1.FileName;

                    if (File.Exists(filenamePath))
                    {
                        _editApplicationsForm.textBoxApplicationPath.Text = filenamePath;
                        _editApplicationsForm.textBoxProcessName.Text = System.IO.Path.GetFileNameWithoutExtension(filenamePath);//  FileUtils.GetBaseName(filenamePath)
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void SetActive(bool isActive)
        {
            _editApplicationsForm.buttonActivate.Enabled = !isActive;
            _editApplicationsForm.buttonDeactivate.Enabled = isActive;
        }
       
        private void ButtonAcceptClick(object sender, EventArgs e)
        {
            if (_editApplicationsForm.textBoxApplicationPath.Text == "")
            {
                DialogResult result = MessageBox.Show(
                    "The application path is missing or in correct. Do you want to close this window and lose your settings [OK] or continue editing [Cancel]",
                    "Missing processname", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK) _editApplicationsForm.Close();
            }
            else
            {
                AcceptChanges();
                _editApplicationsForm.Close();
            }
        }


        private void AcceptChanges()
        {
            FillHandlerConfig();
        }

        private void ButtonDeactivateClick(object sender, EventArgs e)
        {
            SetActive(false);
        }

        private void ButtonActivateClick(object sender, EventArgs e)
        {
            SetActive(true);
        }

        private void FillHandlerConfig()
        {
           _applicationHandlerConfig.NonResponsiveInterval           = int.Parse(_editApplicationsForm.textBoxUnresponsiveInterval.Text)    ;
           _applicationHandlerConfig.HeartbeatInterval               = int.Parse(_editApplicationsForm.textBoxHeartbeatInterval.Text)       ;
           _applicationHandlerConfig.MaxProcesses                    = int.Parse(_editApplicationsForm.textBoxMaxProcesses.Text)            ;
           _applicationHandlerConfig.MinProcesses                    = int.Parse(_editApplicationsForm.textBoxMinProcesses.Text)            ;                                                                                                              
           _applicationHandlerConfig.ApplicationName                 = _editApplicationsForm.textBoxProcessName.Text                        ;
           _applicationHandlerConfig.ApplicationPath                 = _editApplicationsForm.textBoxApplicationPath.Text                    ;
           _applicationHandlerConfig.ApplicationArguments            = _editApplicationsForm.textBoxApplicationArguments.Text               ;
           _applicationHandlerConfig.UseHeartbeat                    = _editApplicationsForm.checkBoxUseHeartbeat.Checked                   ;
           _applicationHandlerConfig.IgnoreHeartBeatIfNeverAcquired  = _editApplicationsForm.checkBoxIgnoreHeartBeatIfNeverAcquired.Checked ;
            
           _applicationHandlerConfig.GrantKillRequest                = _editApplicationsForm.checkBoxGrantKillRequest.Checked               ;
           _applicationHandlerConfig.StartupMonitorDelay             = int.Parse(_editApplicationsForm.textBoxStartupMonitorDelay.Text)     ;
           _applicationHandlerConfig.Active                          = _editApplicationsForm.buttonDeactivate.Enabled                       ;
           _applicationHandlerConfig.TimeBetweenRetry                = int.Parse(_editApplicationsForm.textBoxTimeBetweenRetry.Text)        ;
            


        }

        private void SetForm()
        {
            _editApplicationsForm.textBoxUnresponsiveInterval.Text               = _applicationHandlerConfig.NonResponsiveInterval.ToString();
            _editApplicationsForm.textBoxHeartbeatInterval.Text                  = _applicationHandlerConfig.HeartbeatInterval.ToString();
            _editApplicationsForm.textBoxMaxProcesses.Text                       = _applicationHandlerConfig.MaxProcesses.ToString();
            _editApplicationsForm.textBoxMinProcesses.Text                       = _applicationHandlerConfig.MinProcesses.ToString();  
            _editApplicationsForm.textBoxProcessName.Text                        = _applicationHandlerConfig.ApplicationName;
            _editApplicationsForm.textBoxApplicationPath.Text                    = _applicationHandlerConfig.ApplicationPath;
            _editApplicationsForm.textBoxApplicationArguments.Text               = _applicationHandlerConfig.ApplicationArguments;
            _editApplicationsForm.checkBoxUseHeartbeat.Checked                   = _applicationHandlerConfig.UseHeartbeat;
            _editApplicationsForm.checkBoxIgnoreHeartBeatIfNeverAcquired.Checked = _applicationHandlerConfig.IgnoreHeartBeatIfNeverAcquired;
            _editApplicationsForm.checkBoxGrantKillRequest.Checked               = _applicationHandlerConfig.GrantKillRequest;
            _editApplicationsForm.textBoxStartupMonitorDelay.Text                = _applicationHandlerConfig.StartupMonitorDelay.ToString();
            _editApplicationsForm.textBoxTimeBetweenRetry.Text                   = _applicationHandlerConfig.TimeBetweenRetry.ToString();

            SetActive(_applicationHandlerConfig.Active);
        }
    }
}
