using System;
using System.Windows.Forms;

namespace Utilities.Controls
{


    public partial class ListInputDialog : Form
    {
        public ListDialogResult ListDialogResult { get; set; }

        public static ListDialogResult Show(string text, string caption)
        {
            var dialog = new ListInputDialog
            {
                Name = caption,
                labelMessage = {Text = text},
            };
            dialog.ShowDialog();
            return dialog.ListDialogResult;
        }

        public ListInputDialog()
        {
            InitializeComponent();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            ListDialogResult = new ListDialogResult()
            {
                //Input = textBox1.Text,
                Button = DialogResult.OK
            };
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            ListDialogResult = new ListDialogResult()
            {
                //Input = textBox1.Text,
                Button = DialogResult.Cancel
            };
            Close();

        }
    }

    public class ListDialogResult
    {
        public DialogResult Button { get; set; }
        public string Input { get; set; }
    }
}
