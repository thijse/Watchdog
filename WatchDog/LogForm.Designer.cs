using Utilities.Controls;

namespace WatchDog
{
    partial class LogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.loggingView = new Utilities.Controls.LoggingView();
            this.SuspendLayout();
            // 
            // loggingView
            // 
            this.loggingView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggingView.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.loggingView.FollowLastItem = true;
            this.loggingView.FormattingEnabled = true;
            this.loggingView.Location = new System.Drawing.Point(0, 0);
            this.loggingView.MaxEntriesInListBox = 3000;
            this.loggingView.Name = "loggingView";
            this.loggingView.Size = new System.Drawing.Size(500, 400);
            this.loggingView.TabIndex = 0;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.Controls.Add(this.loggingView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogForm";
            this.Text = "Logging Watchdog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogFormFormClosing);
            this.Resize += new System.EventHandler(this.LogFormResize);
            this.ResumeLayout(false);

        }

        #endregion

        private LoggingView loggingView;
    }
}