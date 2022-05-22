namespace FtpClient
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
            this.button_UpLoad = new System.Windows.Forms.Button();
            this.button_DownLoad = new System.Windows.Forms.Button();
            this.button_FtpLink = new System.Windows.Forms.Button();
            this.textBox_UpFile = new System.Windows.Forms.TextBox();
            this.button_SelectUp = new System.Windows.Forms.Button();
            this.lstbxFtpResources = new System.Windows.Forms.ListBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_UpLoad
            // 
            this.button_UpLoad.Location = new System.Drawing.Point(574, 143);
            this.button_UpLoad.Name = "button_UpLoad";
            this.button_UpLoad.Size = new System.Drawing.Size(75, 23);
            this.button_UpLoad.TabIndex = 0;
            this.button_UpLoad.Text = "上传";
            this.button_UpLoad.UseVisualStyleBackColor = true;
            this.button_UpLoad.Click += new System.EventHandler(this.button_UpLoad_Click);
            // 
            // button_DownLoad
            // 
            this.button_DownLoad.Location = new System.Drawing.Point(574, 238);
            this.button_DownLoad.Name = "button_DownLoad";
            this.button_DownLoad.Size = new System.Drawing.Size(75, 23);
            this.button_DownLoad.TabIndex = 1;
            this.button_DownLoad.Text = "下载";
            this.button_DownLoad.UseVisualStyleBackColor = true;
            this.button_DownLoad.Click += new System.EventHandler(this.button_DownLoad_Click);
            // 
            // button_FtpLink
            // 
            this.button_FtpLink.Location = new System.Drawing.Point(37, 81);
            this.button_FtpLink.Name = "button_FtpLink";
            this.button_FtpLink.Size = new System.Drawing.Size(101, 23);
            this.button_FtpLink.TabIndex = 10;
            this.button_FtpLink.Text = "创建FTP链接";
            this.button_FtpLink.UseVisualStyleBackColor = true;
            this.button_FtpLink.Click += new System.EventHandler(this.button_FtpLink_Click);
            // 
            // textBox_UpFile
            // 
            this.textBox_UpFile.Location = new System.Drawing.Point(37, 143);
            this.textBox_UpFile.Name = "textBox_UpFile";
            this.textBox_UpFile.Size = new System.Drawing.Size(383, 21);
            this.textBox_UpFile.TabIndex = 11;
            // 
            // button_SelectUp
            // 
            this.button_SelectUp.Location = new System.Drawing.Point(473, 141);
            this.button_SelectUp.Name = "button_SelectUp";
            this.button_SelectUp.Size = new System.Drawing.Size(75, 23);
            this.button_SelectUp.TabIndex = 13;
            this.button_SelectUp.Text = "选择文件";
            this.button_SelectUp.UseVisualStyleBackColor = true;
            this.button_SelectUp.Click += new System.EventHandler(this.button_SelectUp_Click);
            // 
            // lstbxFtpResources
            // 
            this.lstbxFtpResources.FormattingEnabled = true;
            this.lstbxFtpResources.ItemHeight = 12;
            this.lstbxFtpResources.Location = new System.Drawing.Point(12, 238);
            this.lstbxFtpResources.Name = "lstbxFtpResources";
            this.lstbxFtpResources.Size = new System.Drawing.Size(556, 76);
            this.lstbxFtpResources.TabIndex = 14;
            this.lstbxFtpResources.DoubleClick += new System.EventHandler(this.lstbxFtpResources_DoubleClick);
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(93, 40);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(100, 21);
            this.textBox_IP.TabIndex = 15;
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(310, 40);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(100, 21);
            this.textBox_Port.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "端口:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 452);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.lstbxFtpResources);
            this.Controls.Add(this.button_SelectUp);
            this.Controls.Add(this.textBox_UpFile);
            this.Controls.Add(this.button_FtpLink);
            this.Controls.Add(this.button_DownLoad);
            this.Controls.Add(this.button_UpLoad);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_UpLoad;
        private System.Windows.Forms.Button button_DownLoad;
        private System.Windows.Forms.Button button_FtpLink;
        private System.Windows.Forms.TextBox textBox_UpFile;
        private System.Windows.Forms.Button button_SelectUp;
        private System.Windows.Forms.ListBox lstbxFtpResources;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

