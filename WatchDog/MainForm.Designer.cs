namespace WatchDog
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listBoxMonitoredApplications = new System.Windows.Forms.ListBox();
            this.buttonAddProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxApplicationPath = new System.Windows.Forms.TextBox();
            this.textBoxProcessName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEditProcess = new System.Windows.Forms.Button();
            this.buttonDeleteProcess = new System.Windows.Forms.Button();
            this.buttonRebootSettings = new System.Windows.Forms.Button();
            this.buttonGeneralSettings = new System.Windows.Forms.Button();
            this.buttonStartStopWatchdog = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxMonitoredApplications
            // 
            this.listBoxMonitoredApplications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMonitoredApplications.FormattingEnabled = true;
            this.listBoxMonitoredApplications.Location = new System.Drawing.Point(12, 4);
            this.listBoxMonitoredApplications.Name = "listBoxMonitoredApplications";
            this.listBoxMonitoredApplications.Size = new System.Drawing.Size(481, 95);
            this.listBoxMonitoredApplications.TabIndex = 0;
            // 
            // buttonAddProcess
            // 
            this.buttonAddProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddProcess.Location = new System.Drawing.Point(11, 105);
            this.buttonAddProcess.Name = "buttonAddProcess";
            this.buttonAddProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonAddProcess.TabIndex = 1;
            this.buttonAddProcess.Text = "Add Application";
            this.buttonAddProcess.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // textBoxApplicationPath
            // 
            this.textBoxApplicationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApplicationPath.Enabled = false;
            this.textBoxApplicationPath.Location = new System.Drawing.Point(62, 28);
            this.textBoxApplicationPath.Name = "textBoxApplicationPath";
            this.textBoxApplicationPath.Size = new System.Drawing.Size(372, 20);
            this.textBoxApplicationPath.TabIndex = 2;
            // 
            // textBoxProcessName
            // 
            this.textBoxProcessName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProcessName.Enabled = false;
            this.textBoxProcessName.Location = new System.Drawing.Point(62, 58);
            this.textBoxProcessName.Name = "textBoxProcessName";
            this.textBoxProcessName.Size = new System.Drawing.Size(372, 20);
            this.textBoxProcessName.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxProcessName);
            this.groupBox1.Controls.Add(this.textBoxApplicationPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 103);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application";
            // 
            // buttonEditProcess
            // 
            this.buttonEditProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEditProcess.Location = new System.Drawing.Point(92, 105);
            this.buttonEditProcess.Name = "buttonEditProcess";
            this.buttonEditProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonEditProcess.TabIndex = 3;
            this.buttonEditProcess.Text = "Edit";
            this.buttonEditProcess.UseVisualStyleBackColor = true;
            // 
            // buttonDeleteProcess
            // 
            this.buttonDeleteProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDeleteProcess.Location = new System.Drawing.Point(173, 105);
            this.buttonDeleteProcess.Name = "buttonDeleteProcess";
            this.buttonDeleteProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteProcess.TabIndex = 4;
            this.buttonDeleteProcess.Text = "Delete";
            this.buttonDeleteProcess.UseVisualStyleBackColor = true;
            // 
            // buttonRebootSettings
            // 
            this.buttonRebootSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRebootSettings.Location = new System.Drawing.Point(381, 297);
            this.buttonRebootSettings.Name = "buttonRebootSettings";
            this.buttonRebootSettings.Size = new System.Drawing.Size(111, 28);
            this.buttonRebootSettings.TabIndex = 26;
            this.buttonRebootSettings.Text = "Reboot settings";
            this.buttonRebootSettings.UseVisualStyleBackColor = true;
            // 
            // buttonGeneralSettings
            // 
            this.buttonGeneralSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGeneralSettings.Location = new System.Drawing.Point(265, 297);
            this.buttonGeneralSettings.Name = "buttonGeneralSettings";
            this.buttonGeneralSettings.Size = new System.Drawing.Size(111, 28);
            this.buttonGeneralSettings.TabIndex = 27;
            this.buttonGeneralSettings.Text = "General settings";
            this.buttonGeneralSettings.UseVisualStyleBackColor = true;
            // 
            // buttonStartStopWatchdog
            // 
            this.buttonStartStopWatchdog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStartStopWatchdog.Location = new System.Drawing.Point(11, 297);
            this.buttonStartStopWatchdog.Name = "buttonStartStopWatchdog";
            this.buttonStartStopWatchdog.Size = new System.Drawing.Size(167, 28);
            this.buttonStartStopWatchdog.TabIndex = 28;
            this.buttonStartStopWatchdog.Text = "Start Watchdog";
            this.buttonStartStopWatchdog.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 329);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "global key:  [CTRL] [ALT] w";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 349);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonStartStopWatchdog);
            this.Controls.Add(this.buttonGeneralSettings);
            this.Controls.Add(this.buttonRebootSettings);
            this.Controls.Add(this.buttonDeleteProcess);
            this.Controls.Add(this.buttonEditProcess);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonAddProcess);
            this.Controls.Add(this.listBoxMonitoredApplications);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Watchdog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox listBoxMonitoredApplications;
        public System.Windows.Forms.Button buttonAddProcess;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBoxApplicationPath;
        public System.Windows.Forms.TextBox textBoxProcessName;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button buttonEditProcess;
        public System.Windows.Forms.Button buttonDeleteProcess;
        public System.Windows.Forms.Button buttonRebootSettings;
        public System.Windows.Forms.Button buttonGeneralSettings;
        public System.Windows.Forms.Button buttonStartStopWatchdog;
        private System.Windows.Forms.Label label3;
    }
}