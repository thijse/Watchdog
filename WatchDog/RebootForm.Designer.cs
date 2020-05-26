namespace WatchDog
{
    partial class RebootForm
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
            this.textBoxRebootAfterDays = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerRebootAfter = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerRebootBefore = new System.Windows.Forms.DateTimePicker();
            this.comboBoxRebootAfterWindow = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxRebootMode = new System.Windows.Forms.ComboBox();
            this.comboBoxRebootForce = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonAcceptChanges = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxReboot = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxRebootAfterDays
            // 
            this.textBoxRebootAfterDays.Location = new System.Drawing.Point(143, 33);
            this.textBoxRebootAfterDays.Name = "textBoxRebootAfterDays";
            this.textBoxRebootAfterDays.Size = new System.Drawing.Size(37, 20);
            this.textBoxRebootAfterDays.TabIndex = 12;
            this.textBoxRebootAfterDays.Text = "30";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Reboot period (days)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Reboot between";
            // 
            // dateTimePickerRebootAfter
            // 
            this.dateTimePickerRebootAfter.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerRebootAfter.Location = new System.Drawing.Point(143, 60);
            this.dateTimePickerRebootAfter.Name = "dateTimePickerRebootAfter";
            this.dateTimePickerRebootAfter.ShowUpDown = true;
            this.dateTimePickerRebootAfter.Size = new System.Drawing.Size(95, 20);
            this.dateTimePickerRebootAfter.TabIndex = 14;
            this.dateTimePickerRebootAfter.Value = new System.DateTime(2016, 9, 21, 23, 0, 0, 0);
            // 
            // dateTimePickerRebootBefore
            // 
            this.dateTimePickerRebootBefore.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerRebootBefore.Location = new System.Drawing.Point(277, 60);
            this.dateTimePickerRebootBefore.Name = "dateTimePickerRebootBefore";
            this.dateTimePickerRebootBefore.ShowUpDown = true;
            this.dateTimePickerRebootBefore.Size = new System.Drawing.Size(95, 20);
            this.dateTimePickerRebootBefore.TabIndex = 15;
            this.dateTimePickerRebootBefore.Value = new System.DateTime(2016, 9, 21, 23, 59, 0, 0);
            // 
            // comboBoxRebootAfterWindow
            // 
            this.comboBoxRebootAfterWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRebootAfterWindow.FormattingEnabled = true;
            this.comboBoxRebootAfterWindow.Items.AddRange(new object[] {
            "First occasion",
            "Try next day"});
            this.comboBoxRebootAfterWindow.Location = new System.Drawing.Point(143, 91);
            this.comboBoxRebootAfterWindow.Name = "comboBoxRebootAfterWindow";
            this.comboBoxRebootAfterWindow.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRebootAfterWindow.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "After reboot period";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Reboot mode";
            // 
            // comboBoxRebootMode
            // 
            this.comboBoxRebootMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRebootMode.FormattingEnabled = true;
            this.comboBoxRebootMode.Items.AddRange(new object[] {
            "ShutDown      ",
            "Reboot        ",
            "Power Off      ",
            "Hybrid Shutdown"});
            this.comboBoxRebootMode.Location = new System.Drawing.Point(143, 120);
            this.comboBoxRebootMode.Name = "comboBoxRebootMode";
            this.comboBoxRebootMode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRebootMode.TabIndex = 19;
            // 
            // comboBoxRebootForce
            // 
            this.comboBoxRebootForce.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRebootForce.FormattingEnabled = true;
            this.comboBoxRebootForce.Items.AddRange(new object[] {
            " Normal     ",
            " Force      ",
            " Force if hung"});
            this.comboBoxRebootForce.Location = new System.Drawing.Point(270, 120);
            this.comboBoxRebootForce.Name = "comboBoxRebootForce";
            this.comboBoxRebootForce.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRebootForce.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "and";
            // 
            // buttonAcceptChanges
            // 
            this.buttonAcceptChanges.Location = new System.Drawing.Point(318, 176);
            this.buttonAcceptChanges.Name = "buttonAcceptChanges";
            this.buttonAcceptChanges.Size = new System.Drawing.Size(111, 28);
            this.buttonAcceptChanges.TabIndex = 25;
            this.buttonAcceptChanges.Text = "Accept";
            this.buttonAcceptChanges.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Perform periodic reboot";
            // 
            // checkBoxReboot
            // 
            this.checkBoxReboot.AutoSize = true;
            this.checkBoxReboot.Location = new System.Drawing.Point(143, 9);
            this.checkBoxReboot.Name = "checkBoxReboot";
            this.checkBoxReboot.Size = new System.Drawing.Size(15, 14);
            this.checkBoxReboot.TabIndex = 27;
            this.checkBoxReboot.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(201, 176);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(111, 28);
            this.buttonCancel.TabIndex = 28;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // RebootForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 215);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxReboot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonAcceptChanges);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxRebootForce);
            this.Controls.Add(this.comboBoxRebootMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxRebootAfterWindow);
            this.Controls.Add(this.dateTimePickerRebootBefore);
            this.Controls.Add(this.dateTimePickerRebootAfter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxRebootAfterDays);
            this.Controls.Add(this.label11);
            this.Name = "RebootForm";
            this.Text = "RebootForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBoxRebootAfterDays;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker dateTimePickerRebootAfter;
        public System.Windows.Forms.DateTimePicker dateTimePickerRebootBefore;
        public System.Windows.Forms.ComboBox comboBoxRebootAfterWindow;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox comboBoxRebootMode;
        public System.Windows.Forms.ComboBox comboBoxRebootForce;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button buttonAcceptChanges;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox checkBoxReboot;
        public System.Windows.Forms.Button buttonCancel;
    }
}