namespace FTPClient
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbxServerIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxPassword = new System.Windows.Forms.TextBox();
            this.btnlogin = new System.Windows.Forms.Button();
            this.btnlogout = new System.Windows.Forms.Button();
            this.tbxloginmessage = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btndownload = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lstbxFtpResources = new System.Windows.Forms.ListBox();
            this.lstbxFtpState = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkbxAnonymous = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP服务器";
            // 
            // tbxServerIp
            // 
            this.tbxServerIp.Location = new System.Drawing.Point(79, 24);
            this.tbxServerIp.Name = "tbxServerIp";
            this.tbxServerIp.Size = new System.Drawing.Size(136, 21);
            this.tbxServerIp.TabIndex = 1;
            this.tbxServerIp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxServerIp_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户名";
            // 
            // tbxUsername
            // 
            this.tbxUsername.Location = new System.Drawing.Point(79, 55);
            this.tbxUsername.Name = "tbxUsername";
            this.tbxUsername.Size = new System.Drawing.Size(112, 21);
            this.tbxUsername.TabIndex = 3;
            this.tbxUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxUsername_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "密码";
            // 
            // tbxPassword
            // 
            this.tbxPassword.Location = new System.Drawing.Point(79, 88);
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.PasswordChar = '*';
            this.tbxPassword.Size = new System.Drawing.Size(136, 21);
            this.tbxPassword.TabIndex = 5;
            this.tbxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxPassword_KeyPress);
            // 
            // btnlogin
            // 
            this.btnlogin.Location = new System.Drawing.Point(79, 126);
            this.btnlogin.Name = "btnlogin";
            this.btnlogin.Size = new System.Drawing.Size(57, 23);
            this.btnlogin.TabIndex = 6;
            this.btnlogin.Text = "登录";
            this.btnlogin.UseVisualStyleBackColor = true;
            this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
            // 
            // btnlogout
            // 
            this.btnlogout.Location = new System.Drawing.Point(142, 126);
            this.btnlogout.Name = "btnlogout";
            this.btnlogout.Size = new System.Drawing.Size(60, 23);
            this.btnlogout.TabIndex = 7;
            this.btnlogout.Text = "注销";
            this.btnlogout.UseVisualStyleBackColor = true;
            this.btnlogout.Click += new System.EventHandler(this.btnlogout_Click);
            // 
            // tbxloginmessage
            // 
            this.tbxloginmessage.BackColor = System.Drawing.SystemColors.MenuBar;
            this.tbxloginmessage.Location = new System.Drawing.Point(19, 170);
            this.tbxloginmessage.Name = "tbxloginmessage";
            this.tbxloginmessage.Size = new System.Drawing.Size(196, 21);
            this.tbxloginmessage.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btndownload);
            this.groupBox1.Controls.Add(this.btnUpload);
            this.groupBox1.Controls.Add(this.lstbxFtpResources);
            this.groupBox1.Location = new System.Drawing.Point(252, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 252);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "我的文件列表";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(249, 218);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btndownload
            // 
            this.btndownload.Location = new System.Drawing.Point(90, 218);
            this.btndownload.Name = "btndownload";
            this.btndownload.Size = new System.Drawing.Size(58, 23);
            this.btndownload.TabIndex = 2;
            this.btndownload.Text = "下载";
            this.btndownload.UseVisualStyleBackColor = true;
            this.btndownload.Click += new System.EventHandler(this.btndownload_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(24, 218);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(60, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "上传";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lstbxFtpResources
            // 
            this.lstbxFtpResources.FormattingEnabled = true;
            this.lstbxFtpResources.ItemHeight = 12;
            this.lstbxFtpResources.Location = new System.Drawing.Point(6, 12);
            this.lstbxFtpResources.Name = "lstbxFtpResources";
            this.lstbxFtpResources.Size = new System.Drawing.Size(304, 196);
            this.lstbxFtpResources.TabIndex = 0;
            this.lstbxFtpResources.DoubleClick += new System.EventHandler(this.lstbxFtpResources_DoubleClick);
            // 
            // lstbxFtpState
            // 
            this.lstbxFtpState.FormattingEnabled = true;
            this.lstbxFtpState.ItemHeight = 12;
            this.lstbxFtpState.Location = new System.Drawing.Point(17, 267);
            this.lstbxFtpState.Name = "lstbxFtpState";
            this.lstbxFtpState.Size = new System.Drawing.Size(562, 64);
            this.lstbxFtpState.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "连接状态：";
            // 
            // chkbxAnonymous
            // 
            this.chkbxAnonymous.AutoSize = true;
            this.chkbxAnonymous.Location = new System.Drawing.Point(197, 57);
            this.chkbxAnonymous.Name = "chkbxAnonymous";
            this.chkbxAnonymous.Size = new System.Drawing.Size(48, 16);
            this.chkbxAnonymous.TabIndex = 12;
            this.chkbxAnonymous.Text = "匿名";
            this.chkbxAnonymous.UseVisualStyleBackColor = true;
            this.chkbxAnonymous.Click += new System.EventHandler(this.chkbxAnonymous_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 352);
            this.Controls.Add(this.chkbxAnonymous);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lstbxFtpState);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxloginmessage);
            this.Controls.Add(this.btnlogout);
            this.Controls.Add(this.btnlogin);
            this.Controls.Add(this.tbxPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxServerIp);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "FTP文件上传下载器";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxServerIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxPassword;
        private System.Windows.Forms.Button btnlogin;
        private System.Windows.Forms.Button btnlogout;
        private System.Windows.Forms.TextBox tbxloginmessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btndownload;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ListBox lstbxFtpResources;
        private System.Windows.Forms.ListBox lstbxFtpState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkbxAnonymous;
    }
}

