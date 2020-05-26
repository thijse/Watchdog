using System;
using System.Windows.Forms;

namespace Utilities.Controls
{


    public partial class LineInputDialog : Form
    {
        public LineDialogResult LineDialogResult { get; set; }

        public static LineDialogResult Show(string text, string caption)
        {
            var dialog = new LineInputDialog
            {
                Name = caption,
                labelMessage = {Text = text},
            };
            dialog.ShowDialog();
            return dialog.LineDialogResult;
        }

        public LineInputDialog()
        {
            InitializeComponent();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            LineDialogResult = new LineDialogResult() {Input = textBox1.Text, DialogResult = DialogResult.OK};
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            LineDialogResult = new LineDialogResult() {Input = textBox1.Text, DialogResult = DialogResult.Cancel};
            Close();

        }
    }

    public class LineDialogResult
    {
        public DialogResult DialogResult { get; set; }
        public string Input { get; set; }
    }
}
