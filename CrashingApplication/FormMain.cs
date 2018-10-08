using System;
using System.Windows.Forms;
using WatchdogClient;


using Timer = System.Timers.Timer;
namespace CrashingApplication
{
    public partial class FormMain : Form
    {

        private readonly Timer  _heartbeatTimer;
        private readonly Heartbeat _heartbeat;

        public FormMain()
        {
            InitializeComponent();
            // Watchdog
            _heartbeat = Heartbeat.Instance;
            _heartbeatTimer = new Timer(1000)
            { 
                Enabled = true,
                Interval = 5000
            };
        }

        private void ButtonUnhandledExceptionClick(object sender, EventArgs e)
        {
            throw new Exception("Unhandled Exception");
        }

        private void ButtonUnresponsiveClick(object sender, EventArgs e)
        {
            Random rnd1 = new Random();
            
            ulong i = 0;
            while (true)
            {
                i++;
                if (i == ulong.MaxValue - 10) {i = 0;}
                double w = Math.Cos(Math.Sin(rnd1.NextDouble()*2*3.14));
                if (w < 0.5 ) { i = 0; }
                //Console.Out.WriteLine("Bogus output, item {0}",i++);
            }
        }

        private void ButtonCerrOutClick(object sender, EventArgs e)
        {
            Console.Error.WriteLine("Error message to console");
        }

        private void ButtonCoutClick(object sender, EventArgs e)
        {
            Console.Out.WriteLine("Normal message to console");
        }

        private void ButtonStopHeartBeatClick(object sender, EventArgs e)
        {
            StopHeartBeat();
        }

        private void FormMainLoad(object sender, EventArgs e)
        {
            // Invoke heartbeat on the main thread so it will only send if the main thread is responsive
            _heartbeatTimer.Elapsed += (s, ee) =>
            {
                try { Invoke(new MethodInvoker(delegate {_heartbeat.SendHeartbeat();})); } catch { }
            };
            StartHeartBeat();
        }

        private void StartHeartBeat()
        {
            buttonStopHeartBeat.Enabled = true;
            buttonStartBeat.Enabled     = false;
            _heartbeatTimer.Start();
        }

        private void StopHeartBeat()
        {
            buttonStopHeartBeat.Enabled = false;
            buttonStartBeat.Enabled     = true;
            _heartbeatTimer.Stop();
        }

        private void ButtonStartBeatClick(object sender, EventArgs e)
        {
            StartHeartBeat();
        }
    }
}
