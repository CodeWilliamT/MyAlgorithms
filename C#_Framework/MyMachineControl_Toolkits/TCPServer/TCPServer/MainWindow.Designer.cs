namespace TCPServer
{
    partial class MainWindow
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
            this.button_crtServer = new System.Windows.Forms.Button();
            this.button_send = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbmessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_crtServer
            // 
            this.button_crtServer.Location = new System.Drawing.Point(12, 4);
            this.button_crtServer.Name = "button_crtServer";
            this.button_crtServer.Size = new System.Drawing.Size(75, 23);
            this.button_crtServer.TabIndex = 0;
            this.button_crtServer.Text = "创建服务器线程";
            this.button_crtServer.UseVisualStyleBackColor = true;
            this.button_crtServer.Click += new System.EventHandler(this.button_crtServer_Click);
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(286, 427);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 1;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 33);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(349, 295);
            this.textBox1.TabIndex = 2;
            this.textBox1.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "链接状态";
            // 
            // tbmessage
            // 
            this.tbmessage.Location = new System.Drawing.Point(12, 334);
            this.tbmessage.Multiline = true;
            this.tbmessage.Name = "tbmessage";
            this.tbmessage.Size = new System.Drawing.Size(349, 76);
            this.tbmessage.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 461);
            this.Controls.Add(this.tbmessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.button_crtServer);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_crtServer;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbmessage;
    }
}

