using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatchdogLib;
using WatchDog.TrayIconTest;

namespace WatchDog
{

    using System.Windows.Forms;
    /// <summary>
    /// The Watchdog Application
    /// </summary>
    /// 
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ExceptionsManager.Logger = null;
            ExceptionsManager.TrayIcon = null;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayIcon());

    
        }
    }
}
