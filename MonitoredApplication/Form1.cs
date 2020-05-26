using System;
using System.Configuration;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using WatchdogClient;
namespace MonitoredApplication
{
    /// <summary>
    /// The Monitored Application
    /// </summary>
    public partial class MonitoredApplicationForm : Form
    {

        private System.Timers.Timer _timer;
        private Heartbeat _heartbeat;
        private int _heartbeatCount;
        public MonitoredApplicationForm()
        {
            InitializeComponent();
            _heartbeat = new Heartbeat();  // initialize heartbeat

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;

            #region WatchdogWatcher initialization

            int watchDogMonitorInterval = 5000;

            try
            {
                watchDogMonitorInterval = Convert.ToInt32(ConfigurationManager.AppSettings["WatchDogMonitorInterval"]);
                if (watchDogMonitorInterval != 0)
                {
                    watchDogMonitorInterval = 5000;
                }
            }
            catch (Exception ex)
            {
                watchDogMonitorInterval = 5000;
                MessageBox.Show("Exception WatchdogMonitor : " + ex.StackTrace);
            }


            #endregion
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // Invoke heartbeat on the main thread otherwise it will send even if the main thread is unresponsive
             Invoke(new MethodInvoker(delegate
             {
                 _heartbeat.SendHeartbeat();
                 toolStripStatusLabelComments.Text = "Heartbeat " + _heartbeatCount++;
                 Debug.WriteLine("Heartbeat " + _heartbeatCount);
             }));
            
            
        }

        /// <summary>
        /// Terminate the monitored application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTerminateClick(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            while(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _timer.Stop();
        }

        private void ButtonDirectKillClick(object sender, EventArgs e)
        {
            _heartbeat.RequestKill();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _heartbeat.RequestKill(10);
        }
    }
}
