namespace MrRobot
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.RequestTimer = new System.Windows.Forms.Timer(this.components);
            this.OrderUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SignChecker = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RequestTimer
            // 
            this.RequestTimer.Enabled = true;
            this.RequestTimer.Interval = 1000;
            this.RequestTimer.Tick += new System.EventHandler(this.RequestTimer_Tick);
            // 
            // OrderUpdateTimer
            // 
            this.OrderUpdateTimer.Enabled = true;
            this.OrderUpdateTimer.Interval = 6000;
            this.OrderUpdateTimer.Tick += new System.EventHandler(this.OrderUpdateTimer_Tick);
            // 
            // SignChecker
            // 
            this.SignChecker.Location = new System.Drawing.Point(21, 21);
            this.SignChecker.Name = "SignChecker";
            this.SignChecker.Size = new System.Drawing.Size(75, 23);
            this.SignChecker.TabIndex = 0;
            this.SignChecker.Text = "Test";
            this.SignChecker.UseVisualStyleBackColor = true;
            this.SignChecker.Click += new System.EventHandler(this.SignChecker_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SignChecker);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer RequestTimer;
        private System.Windows.Forms.Timer OrderUpdateTimer;
        private System.Windows.Forms.Button SignChecker;
    }
}

