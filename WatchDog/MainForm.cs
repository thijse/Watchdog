using System;
using System.IO;
using System.Windows.Forms;

namespace WatchDog
{
    public partial class MainForm : Form
    {
        public EventHandler ActivateEvent;
        public MainForm()
        {
            InitializeComponent();
        }

        private void ButtonSelectFileClick(object sender, EventArgs e)
        {

            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "executable files |*.exe;*.com;*.bat|All files|*.*",

                RestoreDirectory = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filenamePath = openFileDialog1.FileName;

                    if (File.Exists(filenamePath))
                    {
                        textBoxApplicationPath.Text = filenamePath;
                        textBoxProcessName.Text          = System.IO.Path.GetFileNameWithoutExtension(filenamePath);//  FileUtils.GetBaseName(filenamePath)
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }


    }
}
