namespace WatchDog
{
    partial class EditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.textBoxProcessName = new System.Windows.Forms.TextBox();
            this.textBoxApplicationPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxMaxProcesses = new System.Windows.Forms.TextBox();
            this.textBoxMinProcesses = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxGrantKillRequest = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxHeartbeatInterval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxUseHeartbeat = new System.Windows.Forms.CheckBox();
            this.buttonDeactivate = new System.Windows.Forms.Button();
            this.buttonActivate = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxStartupMonitorDelay = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxUnresponsiveInterval = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonAcceptChanges = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSelectFile);
            this.groupBox2.Controls.Add(this.textBoxProcessName);
            this.groupBox2.Controls.Add(this.textBoxApplicationPath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Path";
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonSelectFile.Location = new System.Drawing.Point(442, 25);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(31, 20);
            this.buttonSelectFile.TabIndex = 7;
            this.buttonSelectFile.Text = "...";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            // 
            // textBoxProcessName
            // 
            this.textBoxProcessName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProcessName.Location = new System.Drawing.Point(64, 56);
            this.textBoxProcessName.Name = "textBoxProcessName";
            this.textBoxProcessName.Size = new System.Drawing.Size(372, 20);
            this.textBoxProcessName.TabIndex = 8;
            // 
            // textBoxApplicationPath
            // 
            this.textBoxApplicationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApplicationPath.Location = new System.Drawing.Point(64, 26);
            this.textBoxApplicationPath.Name = "textBoxApplicationPath";
            this.textBoxApplicationPath.Size = new System.Drawing.Size(372, 20);
            this.textBoxApplicationPath.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Path";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxMaxProcesses);
            this.groupBox3.Controls.Add(this.textBoxMinProcesses);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(12, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(481, 73);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Processes";
            // 
            // textBoxMaxProcesses
            // 
            this.textBoxMaxProcesses.Location = new System.Drawing.Point(142, 50);
            this.textBoxMaxProcesses.Name = "textBoxMaxProcesses";
            this.textBoxMaxProcesses.Size = new System.Drawing.Size(37, 20);
            this.textBoxMaxProcesses.TabIndex = 11;
            this.textBoxMaxProcesses.Text = "1";
            // 
            // textBoxMinProcesses
            // 
            this.textBoxMinProcesses.Location = new System.Drawing.Point(142, 24);
            this.textBoxMinProcesses.Name = "textBoxMinProcesses";
            this.textBoxMinProcesses.Size = new System.Drawing.Size(37, 20);
            this.textBoxMinProcesses.TabIndex = 10;
            this.textBoxMinProcesses.Text = "1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(128, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Max number of processes";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Min number of processes";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxGrantKillRequest);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.textBoxHeartbeatInterval);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.checkBoxUseHeartbeat);
            this.groupBox4.Location = new System.Drawing.Point(12, 197);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(481, 113);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Heartbeat";
            // 
            // checkBoxGrantKillRequest
            // 
            this.checkBoxGrantKillRequest.AutoSize = true;
            this.checkBoxGrantKillRequest.Location = new System.Drawing.Point(140, 82);
            this.checkBoxGrantKillRequest.Name = "checkBoxGrantKillRequest";
            this.checkBoxGrantKillRequest.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxGrantKillRequest.Size = new System.Drawing.Size(15, 14);
            this.checkBoxGrantKillRequest.TabIndex = 22;
            this.checkBoxGrantKillRequest.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Grant kill requests";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Use heartbeat";
            // 
            // textBoxHeartbeatInterval
            // 
            this.textBoxHeartbeatInterval.Location = new System.Drawing.Point(140, 50);
            this.textBoxHeartbeatInterval.Name = "textBoxHeartbeatInterval";
            this.textBoxHeartbeatInterval.Size = new System.Drawing.Size(26, 20);
            this.textBoxHeartbeatInterval.TabIndex = 19;
            this.textBoxHeartbeatInterval.Text = "10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Max interval hearbeats (s)";
            // 
            // checkBoxUseHeartbeat
            // 
            this.checkBoxUseHeartbeat.AutoSize = true;
            this.checkBoxUseHeartbeat.Location = new System.Drawing.Point(140, 26);
            this.checkBoxUseHeartbeat.Name = "checkBoxUseHeartbeat";
            this.checkBoxUseHeartbeat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxUseHeartbeat.Size = new System.Drawing.Size(15, 14);
            this.checkBoxUseHeartbeat.TabIndex = 17;
            this.checkBoxUseHeartbeat.UseVisualStyleBackColor = true;
            // 
            // buttonDeactivate
            // 
            this.buttonDeactivate.Location = new System.Drawing.Point(118, 422);
            this.buttonDeactivate.Name = "buttonDeactivate";
            this.buttonDeactivate.Size = new System.Drawing.Size(98, 28);
            this.buttonDeactivate.TabIndex = 18;
            this.buttonDeactivate.Text = "Deactivate";
            this.buttonDeactivate.UseVisualStyleBackColor = true;
            // 
            // buttonActivate
            // 
            this.buttonActivate.Location = new System.Drawing.Point(12, 422);
            this.buttonActivate.Name = "buttonActivate";
            this.buttonActivate.Size = new System.Drawing.Size(98, 28);
            this.buttonActivate.TabIndex = 17;
            this.buttonActivate.Text = "Activate";
            this.buttonActivate.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxStartupMonitorDelay);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.textBoxUnresponsiveInterval);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(12, 316);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(482, 93);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Interval";
            // 
            // textBoxStartupMonitorDelay
            // 
            this.textBoxStartupMonitorDelay.Location = new System.Drawing.Point(142, 54);
            this.textBoxStartupMonitorDelay.Name = "textBoxStartupMonitorDelay";
            this.textBoxStartupMonitorDelay.Size = new System.Drawing.Size(37, 20);
            this.textBoxStartupMonitorDelay.TabIndex = 24;
            this.textBoxStartupMonitorDelay.Text = "20";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Startup monitor delay";
            // 
            // textBoxUnresponsiveInterval
            // 
            this.textBoxUnresponsiveInterval.Location = new System.Drawing.Point(142, 27);
            this.textBoxUnresponsiveInterval.Name = "textBoxUnresponsiveInterval";
            this.textBoxUnresponsiveInterval.Size = new System.Drawing.Size(37, 20);
            this.textBoxUnresponsiveInterval.TabIndex = 22;
            this.textBoxUnresponsiveInterval.Text = "20";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Max unresponsive interval";
            // 
            // buttonAcceptChanges
            // 
            this.buttonAcceptChanges.Location = new System.Drawing.Point(396, 422);
            this.buttonAcceptChanges.Name = "buttonAcceptChanges";
            this.buttonAcceptChanges.Size = new System.Drawing.Size(98, 28);
            this.buttonAcceptChanges.TabIndex = 22;
            this.buttonAcceptChanges.Text = "Accept";
            this.buttonAcceptChanges.UseVisualStyleBackColor = true;
            // 
            // EditApplicationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 462);
            this.Controls.Add(this.buttonDeactivate);
            this.Controls.Add(this.buttonAcceptChanges);
            this.Controls.Add(this.buttonActivate);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditApplicationsForm";
            this.Text = "Watchdog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Button buttonSelectFile;
        public System.Windows.Forms.TextBox textBoxProcessName;
        public System.Windows.Forms.TextBox textBoxApplicationPath;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.TextBox textBoxMaxProcesses;
        public System.Windows.Forms.TextBox textBoxMinProcesses;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.CheckBox checkBoxGrantKillRequest;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox textBoxHeartbeatInterval;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox checkBoxUseHeartbeat;
        public System.Windows.Forms.Button buttonDeactivate;
        public System.Windows.Forms.Button buttonActivate;
        public System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.TextBox textBoxStartupMonitorDelay;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox textBoxUnresponsiveInterval;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Button buttonAcceptChanges;
    }
}