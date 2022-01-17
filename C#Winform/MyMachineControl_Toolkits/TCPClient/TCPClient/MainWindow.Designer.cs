namespace TCPClient
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
            this.tbmessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.button_cntServer = new System.Windows.Forms.Button();
            this.textBox_ServerIP = new System.Windows.Forms.TextBox();
            this.textBox_IdPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbmessage
            // 
            this.tbmessage.Location = new System.Drawing.Point(12, 337);
            this.tbmessage.Multiline = true;
            this.tbmessage.Name = "tbmessage";
            this.tbmessage.Size = new System.Drawing.Size(349, 76);
            this.tbmessage.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "链接状态";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 36);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(349, 295);
            this.textBox1.TabIndex = 7;
            this.textBox1.WordWrap = false;
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(286, 430);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 6;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // button_cntServer
            // 
            this.button_cntServer.Location = new System.Drawing.Point(12, 7);
            this.button_cntServer.Name = "button_cntServer";
            this.button_cntServer.Size = new System.Drawing.Size(75, 23);
            this.button_cntServer.TabIndex = 5;
            this.button_cntServer.Text = "建立链接";
            this.button_cntServer.UseVisualStyleBackColor = true;
            this.button_cntServer.Click += new System.EventHandler(this.button_cntServer_Click);
            // 
            // textBox_ServerIP
            // 
            this.textBox_ServerIP.Location = new System.Drawing.Point(182, 9);
            this.textBox_ServerIP.Name = "textBox_ServerIP";
            this.textBox_ServerIP.Size = new System.Drawing.Size(100, 21);
            this.textBox_ServerIP.TabIndex = 10;
            // 
            // textBox_IdPort
            // 
            this.textBox_IdPort.Location = new System.Drawing.Point(288, 9);
            this.textBox_IdPort.Name = "textBox_IdPort";
            this.textBox_IdPort.Size = new System.Drawing.Size(73, 21);
            this.textBox_IdPort.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 461);
            this.Controls.Add(this.textBox_IdPort);
            this.Controls.Add(this.textBox_ServerIP);
            this.Controls.Add(this.tbmessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.button_cntServer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbmessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Button button_cntServer;
        private System.Windows.Forms.TextBox textBox_ServerIP;
        private System.Windows.Forms.TextBox textBox_IdPort;
    }
}

