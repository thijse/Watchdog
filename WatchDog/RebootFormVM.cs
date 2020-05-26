using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchdogLib;

namespace WatchDog
{
    public class RebootFormVM
    {
        private RebootForm _rebootForm;
        private RebootHandler _rebootHandler;
        private Configuration _configuration;

        public RebootFormVM(RebootForm rebootForm, RebootHandler rebootHandler, Configuration configuration)
        {
            _rebootForm = rebootForm;
            _rebootHandler = rebootHandler;
            _configuration = configuration;
            _rebootForm.comboBoxRebootMode.SelectedIndex = 0;
            _rebootForm.comboBoxRebootForce.SelectedIndex = 0;
            _rebootForm.comboBoxRebootAfterWindow.SelectedIndex = 0;
        }

        public void GetForm()
        {
            _configuration.PeriodicReboot = _rebootForm.checkBoxReboot.Checked;
            _configuration.RebootPeriod = int.Parse(_rebootForm.textBoxRebootAfterDays.Text);
            _configuration.RebootAfter = _rebootForm.dateTimePickerRebootAfter.Value.Date;
            _configuration.RebootBefore = _rebootForm.dateTimePickerRebootBefore.Value.Date;

            _configuration.RebootMode          = (Configuration.RebootModes)_rebootForm.comboBoxRebootMode.SelectedIndex; 
            _configuration.ForceMode           = (Configuration.RebootModes)_rebootForm.comboBoxRebootForce.SelectedIndex;
            _configuration.RebootAfterTimeSlot = (Configuration.RebootAfterTimeSlotModes)_rebootForm.comboBoxRebootAfterWindow.SelectedIndex;            
        }
    }
}
