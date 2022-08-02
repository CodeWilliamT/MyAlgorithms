namespace FTPServer
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnFtpServerStartStop = new System.Windows.Forms.Button();
            this.lstboxStatus = new System.Windows.Forms.ListBox();
            this.tbxFtpServerIp = new System.Windows.Forms.TextBox();
            this.tbxFtpServerPort = new System.Windows.Forms.TextBox();
            this.tbxFtpRoot = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnFtpServerStartStop
            // 
            this.btnFtpServerStartStop.Location = new System.Drawing.Point(386, 394);
            this.btnFtpServerStartStop.Name = "btnFtpServerStartStop";
            this.btnFtpServerStartStop.Size = new System.Drawing.Size(99, 23);
            this.btnFtpServerStartStop.TabIndex = 0;
            this.btnFtpServerStartStop.Text = "启动FTP服务器";
            this.btnFtpServerStartStop.UseVisualStyleBackColor = true;
            this.btnFtpServerStartStop.Click += new System.EventHandler(this.btnFtpServerStartStop_Click);
            // 
            // lstboxStatus
            // 
            this.lstboxStatus.FormattingEnabled = true;
            this.lstboxStatus.ItemHeight = 12;
            this.lstboxStatus.Location = new System.Drawing.Point(12, 9);
            this.lstboxStatus.Name = "lstboxStatus";
            this.lstboxStatus.Size = new System.Drawing.Size(473, 328);
            this.lstboxStatus.TabIndex = 1;
            // 
            // tbxFtpServerIp
            // 
            this.tbxFtpServerIp.Location = new System.Drawing.Point(36, 367);
            this.tbxFtpServerIp.Name = "tbxFtpServerIp";
            this.tbxFtpServerIp.Size = new System.Drawing.Size(100, 21);
            this.tbxFtpServerIp.TabIndex = 2;
            // 
            // tbxFtpServerPort
            // 
            this.tbxFtpServerPort.Location = new System.Drawing.Point(175, 367);
            this.tbxFtpServerPort.Name = "tbxFtpServerPort";
            this.tbxFtpServerPort.Size = new System.Drawing.Size(100, 21);
            this.tbxFtpServerPort.TabIndex = 3;
            // 
            // tbxFtpRoot
            // 
            this.tbxFtpRoot.Location = new System.Drawing.Point(375, 367);
            this.tbxFtpRoot.Name = "tbxFtpRoot";
            this.tbxFtpRoot.Size = new System.Drawing.Size(110, 21);
            this.tbxFtpRoot.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 439);
            this.Controls.Add(this.tbxFtpRoot);
            this.Controls.Add(this.tbxFtpServerPort);
            this.Controls.Add(this.tbxFtpServerIp);
            this.Controls.Add(this.lstboxStatus);
            this.Controls.Add(this.btnFtpServerStartStop);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFtpServerStartStop;
        private System.Windows.Forms.ListBox lstboxStatus;
        private System.Windows.Forms.TextBox tbxFtpServerIp;
        private System.Windows.Forms.TextBox tbxFtpServerPort;
        private System.Windows.Forms.TextBox tbxFtpRoot;
    }
}

