using System;
using System.Windows.Forms;
using NLog;

namespace WatchDog
{
    class ExceptionsManager
    {
        static ExceptionsManager()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        public  static NotifyIcon TrayIcon { get; set; }
        public  static Logger       Logger { get; set; }

        public static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
                var ex = (Exception)e.ExceptionObject;
               var message = (ex != null) ? ex.Message : "unknown error";
               ServerCrash("Unhandled error in " + "Watchdog", "Unhandled error in watchdog server : " + message, true);
                Application.Exit();
        }

        public static void ServerCrash(string title, string message, bool exit)
        {
            try
            {
                if (Logger != null) Logger.Fatal(message);
            }
            catch
            {
                // Do nothing
            }
            try
            {
                if (TrayIcon != null)
                {
                    TrayIcon.BalloonTipIcon = ToolTipIcon.Error;
                    TrayIcon.BalloonTipText = message;
                    TrayIcon.BalloonTipTitle = title;
                    TrayIcon.Text = "Watchdog server";
                    TrayIcon.ShowBalloonTip(1000);
                }
                else
                {
                    MessageBox.Show(
                        message,
                        title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RightAlign,
                        true);
                }
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                if (exit) Application.Exit();
            }
        }
    }
}
