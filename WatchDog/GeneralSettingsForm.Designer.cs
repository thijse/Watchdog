namespace WatchDog
{
    partial class GeneralSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralSettingsForm));
            this.checkBoxRestartOnTask = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAcceptChanges = new System.Windows.Forms.Button();
            this.checkBoxStartOnWindowsStart = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxRestartOnTask
            // 
            this.checkBoxRestartOnTask.AutoSize = true;
            this.checkBoxRestartOnTask.Location = new System.Drawing.Point(220, 32);
            this.checkBoxRestartOnTask.Name = "checkBoxRestartOnTask";
            this.checkBoxRestartOnTask.Size = new System.Drawing.Size(15, 14);
            this.checkBoxRestartOnTask.TabIndex = 29;
            this.checkBoxRestartOnTask.UseVisualStyleBackColor = true;
            this.checkBoxRestartOnTask.CheckedChanged += new System.EventHandler(this.checkBoxRestartOnTask_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Periodically check if Watchdog is running";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(78, 102);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(111, 28);
            this.buttonCancel.TabIndex = 31;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonAcceptChanges
            // 
            this.buttonAcceptChanges.Location = new System.Drawing.Point(195, 102);
            this.buttonAcceptChanges.Name = "buttonAcceptChanges";
            this.buttonAcceptChanges.Size = new System.Drawing.Size(111, 28);
            this.buttonAcceptChanges.TabIndex = 30;
            this.buttonAcceptChanges.Text = "Accept";
            this.buttonAcceptChanges.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartOnWindowsStart
            // 
            this.checkBoxStartOnWindowsStart.AutoSize = true;
            this.checkBoxStartOnWindowsStart.Location = new System.Drawing.Point(220, 8);
            this.checkBoxStartOnWindowsStart.Name = "checkBoxStartOnWindowsStart";
            this.checkBoxStartOnWindowsStart.Size = new System.Drawing.Size(15, 14);
            this.checkBoxStartOnWindowsStart.TabIndex = 33;
            this.checkBoxStartOnWindowsStart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Start Watchdog on Windows startup";
            // 
            // GeneralSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 142);
            this.Controls.Add(this.checkBoxStartOnWindowsStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAcceptChanges);
            this.Controls.Add(this.checkBoxRestartOnTask);
            this.Controls.Add(this.label5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GeneralSettingsForm";
            this.Text = "General Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox checkBoxRestartOnTask;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonAcceptChanges;
        public System.Windows.Forms.CheckBox checkBoxStartOnWindowsStart;
        public System.Windows.Forms.Label label1;
    }
}