using System;
using WatchdogLib;

namespace WatchDog
{
    public class RebootFormVm
    {
        private readonly RebootForm _rebootForm;
        private readonly RebootHandler _rebootHandler;
        private readonly RebootConfig _rebootConfiguration;

        public RebootFormVm(RebootForm rebootForm, RebootHandler rebootHandler, RebootConfig rebootConfiguration)
        {
            _rebootForm                                         = rebootForm;
            _rebootHandler                                      = rebootHandler;
            _rebootConfiguration                                = rebootConfiguration;
            _rebootForm.comboBoxRebootMode.SelectedIndex        = 0;
            _rebootForm.comboBoxRebootForce.SelectedIndex       = 0;
            _rebootForm.comboBoxRebootAfterWindow.SelectedIndex = 0;

            _rebootForm.buttonAcceptChanges.Click              += ButtonAcceptChangesClicks;
            _rebootForm.buttonCancel.Click                     += ButtonCancelClick;

            _rebootForm.dateTimePickerRebootAfter.MinDate       = DateTime.MinValue;
            _rebootForm.dateTimePickerRebootAfter.MaxDate       = DateTime.MaxValue;
            _rebootForm.dateTimePickerRebootBefore.MinDate      = DateTime.MinValue;
            _rebootForm.dateTimePickerRebootBefore.MaxDate      = DateTime.MaxValue;

            SetForm();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            _rebootForm.Close();
        }

        private void ButtonAcceptChangesClicks(object sender, EventArgs e)
        {
            GetForm();
            _rebootHandler.Set(_rebootConfiguration);
            _rebootForm.Close();
        }

        private void GetForm()
        {
            _rebootConfiguration.PeriodicReboot                 = _rebootForm.checkBoxReboot.Checked;
            _rebootConfiguration.RebootPeriod                   = int.Parse(_rebootForm.textBoxRebootAfterDays.Text);
            _rebootConfiguration.RebootAfter                    = _rebootForm.dateTimePickerRebootAfter.Value;
            _rebootConfiguration.RebootBefore                   = _rebootForm.dateTimePickerRebootBefore.Value;

            _rebootConfiguration.RebootMode                     = (Configuration.RebootModes)_rebootForm.comboBoxRebootMode.SelectedIndex; 
            _rebootConfiguration.ForceMode                      = (Configuration.ForceModes)_rebootForm.comboBoxRebootForce.SelectedIndex;
            _rebootConfiguration.RebootAfterTimeSlot            = (Configuration.RebootAfterTimeSlotModes)_rebootForm.comboBoxRebootAfterWindow.SelectedIndex;            
        }

        private void SetForm()
        {
            _rebootForm.checkBoxReboot.Checked                  = _rebootConfiguration.PeriodicReboot;
            _rebootForm.textBoxRebootAfterDays.Text             = _rebootConfiguration.RebootPeriod.ToString();
            _rebootForm.dateTimePickerRebootAfter.Value         = _rebootConfiguration.RebootAfter;
            _rebootForm.dateTimePickerRebootBefore.Value        = _rebootConfiguration.RebootBefore;

            _rebootForm.comboBoxRebootMode.SelectedIndex        = (int)_rebootConfiguration.RebootMode;
            _rebootForm.comboBoxRebootForce.SelectedIndex       = (int)_rebootConfiguration.ForceMode;
            _rebootForm.comboBoxRebootAfterWindow.SelectedIndex = (int)_rebootConfiguration.RebootAfterTimeSlot;
        }
    }
}
