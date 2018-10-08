using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Controls;

namespace WatchDog
{
    public partial class LogForm : Form
    {
        public LoggingView LoggingView { get { return loggingView;  } }

        public LogForm()
        {
            InitializeComponent();
        }

        private void LogFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void LogFormResize(object sender, EventArgs e)
        {

            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
    
    }
}
