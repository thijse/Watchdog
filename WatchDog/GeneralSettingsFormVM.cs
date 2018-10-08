using System;
using WatchdogLib;

namespace WatchDog
{
    public class GeneralSettingsFormVM
    {
        private readonly GeneralSettingsForm _generalSettingsForm;        
        private readonly Configuration _configuration;

        public GeneralSettingsFormVM(GeneralSettingsForm generalSettingsForm,  Configuration configuration)
        {
            _generalSettingsForm                                = generalSettingsForm;
            _configuration                                      = configuration;

            _generalSettingsForm.buttonAcceptChanges.Click     += ButtonAcceptChangesClicks;
            _generalSettingsForm.buttonCancel.Click            += ButtonCancelClick;


            SetForm();
        }


        private void ButtonCancelClick(object sender, EventArgs e)
        {
            _generalSettingsForm.Close();
        }

        private void ButtonAcceptChangesClicks(object sender, EventArgs e)
        {
            GetForm();
            RegisterWatchdogTask.SetTask(_configuration.RestartOnTask);
            Startup.SetStartup(_configuration.StartOnWindowsStart);
            _generalSettingsForm.Close();
        }


        private void GetForm()
        {
            _configuration.RestartOnTask       = _generalSettingsForm.checkBoxRestartOnTask.Checked;
            _configuration.StartOnWindowsStart = _generalSettingsForm.checkBoxStartOnWindowsStart.Checked;
        }

        private void SetForm()
        {
            _generalSettingsForm.checkBoxRestartOnTask.Checked       = _configuration.RestartOnTask;
            _generalSettingsForm.checkBoxStartOnWindowsStart.Checked = _configuration.StartOnWindowsStart;
        }
    }
}
