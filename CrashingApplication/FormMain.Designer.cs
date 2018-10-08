namespace CrashingApplication
{
    partial class FormMain
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
            this.buttonUnhandledException = new System.Windows.Forms.Button();
            this.buttonUnresponsive = new System.Windows.Forms.Button();
            this.buttonCerrOut = new System.Windows.Forms.Button();
            this.buttonCout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStopHeartBeat = new System.Windows.Forms.Button();
            this.buttonStartBeat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonUnhandledException
            // 
            this.buttonUnhandledException.Location = new System.Drawing.Point(12, 12);
            this.buttonUnhandledException.Name = "buttonUnhandledException";
            this.buttonUnhandledException.Size = new System.Drawing.Size(99, 35);
            this.buttonUnhandledException.TabIndex = 0;
            this.buttonUnhandledException.Text = "Unhandled Exception";
            this.buttonUnhandledException.UseVisualStyleBackColor = true;
            this.buttonUnhandledException.Click += new System.EventHandler(this.ButtonUnhandledExceptionClick);
            // 
            // buttonUnresponsive
            // 
            this.buttonUnresponsive.Location = new System.Drawing.Point(126, 12);
            this.buttonUnresponsive.Name = "buttonUnresponsive";
            this.buttonUnresponsive.Size = new System.Drawing.Size(99, 35);
            this.buttonUnresponsive.TabIndex = 1;
            this.buttonUnresponsive.Text = "Unresponsive";
            this.buttonUnresponsive.UseVisualStyleBackColor = true;
            this.buttonUnresponsive.Click += new System.EventHandler(this.ButtonUnresponsiveClick);
            // 
            // buttonCerrOut
            // 
            this.buttonCerrOut.Location = new System.Drawing.Point(103, 218);
            this.buttonCerrOut.Name = "buttonCerrOut";
            this.buttonCerrOut.Size = new System.Drawing.Size(88, 35);
            this.buttonCerrOut.TabIndex = 2;
            this.buttonCerrOut.Text = "Cerr message";
            this.buttonCerrOut.UseVisualStyleBackColor = true;
            this.buttonCerrOut.Click += new System.EventHandler(this.ButtonCerrOutClick);
            // 
            // buttonCout
            // 
            this.buttonCout.Location = new System.Drawing.Point(12, 218);
            this.buttonCout.Name = "buttonCout";
            this.buttonCout.Size = new System.Drawing.Size(85, 35);
            this.buttonCout.TabIndex = 3;
            this.buttonCout.Text = "Cout message";
            this.buttonCout.UseVisualStyleBackColor = true;
            this.buttonCout.Click += new System.EventHandler(this.ButtonCoutClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "build 101";
            // 
            // buttonStopHeartBeat
            // 
            this.buttonStopHeartBeat.Location = new System.Drawing.Point(12, 63);
            this.buttonStopHeartBeat.Name = "buttonStopHeartBeat";
            this.buttonStopHeartBeat.Size = new System.Drawing.Size(99, 35);
            this.buttonStopHeartBeat.TabIndex = 5;
            this.buttonStopHeartBeat.Text = "Stop HeartBeat";
            this.buttonStopHeartBeat.UseVisualStyleBackColor = true;
            this.buttonStopHeartBeat.Click += new System.EventHandler(this.ButtonStopHeartBeatClick);
            // 
            // buttonStartBeat
            // 
            this.buttonStartBeat.Location = new System.Drawing.Point(126, 63);
            this.buttonStartBeat.Name = "buttonStartBeat";
            this.buttonStartBeat.Size = new System.Drawing.Size(99, 35);
            this.buttonStartBeat.TabIndex = 6;
            this.buttonStartBeat.Text = "Start HeartBeat";
            this.buttonStartBeat.UseVisualStyleBackColor = true;
            this.buttonStartBeat.Click += new System.EventHandler(this.ButtonStartBeatClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonStartBeat);
            this.Controls.Add(this.buttonStopHeartBeat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCout);
            this.Controls.Add(this.buttonCerrOut);
            this.Controls.Add(this.buttonUnresponsive);
            this.Controls.Add(this.buttonUnhandledException);
            this.Name = "FormMain";
            this.Text = "Crashing application";
            this.Load += new System.EventHandler(this.FormMainLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUnhandledException;
        private System.Windows.Forms.Button buttonUnresponsive;
        private System.Windows.Forms.Button buttonCerrOut;
        private System.Windows.Forms.Button buttonCout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStopHeartBeat;
        private System.Windows.Forms.Button buttonStartBeat;
    }
}

