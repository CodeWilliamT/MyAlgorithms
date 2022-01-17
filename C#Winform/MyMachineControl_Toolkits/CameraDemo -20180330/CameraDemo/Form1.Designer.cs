namespace WindowsFormsApplication1
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
            this.components = new System.ComponentModel.Container();
            this.btnInitCamera = new System.Windows.Forms.Button();
            this.btnOpenCamera = new System.Windows.Forms.Button();
            this.btnCloseCamera = new System.Windows.Forms.Button();
            this.btnSnap = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.tbGain = new System.Windows.Forms.TextBox();
            this.btnGetExposure = new System.Windows.Forms.Button();
            this.btnSetExposure = new System.Windows.Forms.Button();
            this.btnGetGain = new System.Windows.Forms.Button();
            this.btnSetGain = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCameraSN = new System.Windows.Forms.TextBox();
            this.lbGrabStatus = new System.Windows.Forms.Label();
            this.cmbCameraType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.htWindow = new HTHalControl.HTWindowControl();
            this.lbImgNum = new System.Windows.Forms.Label();
            this.btnGrab = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.btnSoftRoop = new System.Windows.Forms.Button();
            this.lbIntvl = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnInitCamera
            // 
            this.btnInitCamera.Location = new System.Drawing.Point(664, 81);
            this.btnInitCamera.Margin = new System.Windows.Forms.Padding(2);
            this.btnInitCamera.Name = "btnInitCamera";
            this.btnInitCamera.Size = new System.Drawing.Size(140, 39);
            this.btnInitCamera.TabIndex = 1;
            this.btnInitCamera.Text = "初始化相机";
            this.btnInitCamera.UseVisualStyleBackColor = true;
            this.btnInitCamera.Click += new System.EventHandler(this.btnInitCamera_Click);
            // 
            // btnOpenCamera
            // 
            this.btnOpenCamera.Location = new System.Drawing.Point(664, 137);
            this.btnOpenCamera.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenCamera.Name = "btnOpenCamera";
            this.btnOpenCamera.Size = new System.Drawing.Size(140, 39);
            this.btnOpenCamera.TabIndex = 1;
            this.btnOpenCamera.Text = "打开相机";
            this.btnOpenCamera.UseVisualStyleBackColor = true;
            this.btnOpenCamera.Click += new System.EventHandler(this.btnOpenCamera_Click);
            // 
            // btnCloseCamera
            // 
            this.btnCloseCamera.Location = new System.Drawing.Point(664, 192);
            this.btnCloseCamera.Margin = new System.Windows.Forms.Padding(2);
            this.btnCloseCamera.Name = "btnCloseCamera";
            this.btnCloseCamera.Size = new System.Drawing.Size(140, 39);
            this.btnCloseCamera.TabIndex = 1;
            this.btnCloseCamera.Text = "关闭相机";
            this.btnCloseCamera.UseVisualStyleBackColor = true;
            this.btnCloseCamera.Click += new System.EventHandler(this.btnCloseCamera_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.Location = new System.Drawing.Point(664, 253);
            this.btnSnap.Margin = new System.Windows.Forms.Padding(2);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(140, 39);
            this.btnSnap.TabIndex = 1;
            this.btnSnap.Text = "抓拍";
            this.btnSnap.UseVisualStyleBackColor = true;
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(634, 417);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "曝光:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(634, 449);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "增益:";
            // 
            // tbExposure
            // 
            this.tbExposure.Location = new System.Drawing.Point(672, 415);
            this.tbExposure.Margin = new System.Windows.Forms.Padding(2);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(76, 21);
            this.tbExposure.TabIndex = 3;
            // 
            // tbGain
            // 
            this.tbGain.Location = new System.Drawing.Point(672, 449);
            this.tbGain.Margin = new System.Windows.Forms.Padding(2);
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(76, 21);
            this.tbGain.TabIndex = 3;
            // 
            // btnGetExposure
            // 
            this.btnGetExposure.Location = new System.Drawing.Point(752, 415);
            this.btnGetExposure.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetExposure.Name = "btnGetExposure";
            this.btnGetExposure.Size = new System.Drawing.Size(32, 20);
            this.btnGetExposure.TabIndex = 4;
            this.btnGetExposure.Text = "Get";
            this.btnGetExposure.UseVisualStyleBackColor = true;
            this.btnGetExposure.Click += new System.EventHandler(this.btnGetExposure_Click);
            // 
            // btnSetExposure
            // 
            this.btnSetExposure.Location = new System.Drawing.Point(789, 415);
            this.btnSetExposure.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetExposure.Name = "btnSetExposure";
            this.btnSetExposure.Size = new System.Drawing.Size(32, 20);
            this.btnSetExposure.TabIndex = 4;
            this.btnSetExposure.Text = "Set";
            this.btnSetExposure.UseVisualStyleBackColor = true;
            this.btnSetExposure.Click += new System.EventHandler(this.btnSetExposure_Click);
            // 
            // btnGetGain
            // 
            this.btnGetGain.Location = new System.Drawing.Point(752, 447);
            this.btnGetGain.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetGain.Name = "btnGetGain";
            this.btnGetGain.Size = new System.Drawing.Size(32, 20);
            this.btnGetGain.TabIndex = 4;
            this.btnGetGain.Text = "Get";
            this.btnGetGain.UseVisualStyleBackColor = true;
            this.btnGetGain.Click += new System.EventHandler(this.btnGetGain_Click);
            // 
            // btnSetGain
            // 
            this.btnSetGain.Location = new System.Drawing.Point(789, 447);
            this.btnSetGain.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetGain.Name = "btnSetGain";
            this.btnSetGain.Size = new System.Drawing.Size(32, 20);
            this.btnSetGain.TabIndex = 4;
            this.btnSetGain.Text = "Set";
            this.btnSetGain.UseVisualStyleBackColor = true;
            this.btnSetGain.Click += new System.EventHandler(this.btnSetGain_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(626, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "相机SN:";
            // 
            // tbCameraSN
            // 
            this.tbCameraSN.Location = new System.Drawing.Point(672, 48);
            this.tbCameraSN.Margin = new System.Windows.Forms.Padding(2);
            this.tbCameraSN.Name = "tbCameraSN";
            this.tbCameraSN.Size = new System.Drawing.Size(121, 21);
            this.tbCameraSN.TabIndex = 6;
            this.tbCameraSN.Text = "00823048818";
            // 
            // lbGrabStatus
            // 
            this.lbGrabStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbGrabStatus.Location = new System.Drawing.Point(628, 479);
            this.lbGrabStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbGrabStatus.Name = "lbGrabStatus";
            this.lbGrabStatus.Size = new System.Drawing.Size(202, 30);
            this.lbGrabStatus.TabIndex = 7;
            this.lbGrabStatus.Text = "GrabStatus";
            this.lbGrabStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbCameraType
            // 
            this.cmbCameraType.FormattingEnabled = true;
            this.cmbCameraType.Location = new System.Drawing.Point(670, 14);
            this.cmbCameraType.Margin = new System.Windows.Forms.Padding(2);
            this.cmbCameraType.Name = "cmbCameraType";
            this.cmbCameraType.Size = new System.Drawing.Size(122, 20);
            this.cmbCameraType.TabIndex = 8;
            this.cmbCameraType.SelectedIndexChanged += new System.EventHandler(this.cmbCameraType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(624, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "调用库:";
            // 
            // htWindow
            // 
            this.htWindow.BackColor = System.Drawing.Color.Transparent;
            this.htWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.htWindow.ColorName = "red";
            this.htWindow.ColorType = 0;
            this.htWindow.Column = null;
            this.htWindow.Column1 = null;
            this.htWindow.Column2 = null;
            this.htWindow.Image = null;
            this.htWindow.Length1 = null;
            this.htWindow.Length2 = null;
            this.htWindow.Location = new System.Drawing.Point(3, 1);
            this.htWindow.Margin = new System.Windows.Forms.Padding(4);
            this.htWindow.Name = "htWindow";
            this.htWindow.Phi = null;
            this.htWindow.Radius = null;
            this.htWindow.Radius1 = null;
            this.htWindow.Radius2 = null;
            this.htWindow.Region = null;
            this.htWindow.RegionType = null;
            this.htWindow.Row = null;
            this.htWindow.Row1 = null;
            this.htWindow.Row2 = null;
            this.htWindow.Size = new System.Drawing.Size(620, 549);
            this.htWindow.TabIndex = 10;
            // 
            // lbImgNum
            // 
            this.lbImgNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbImgNum.Location = new System.Drawing.Point(628, 509);
            this.lbImgNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbImgNum.Name = "lbImgNum";
            this.lbImgNum.Size = new System.Drawing.Size(202, 31);
            this.lbImgNum.TabIndex = 7;
            this.lbImgNum.Text = "ImageNum";
            this.lbImgNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGrab
            // 
            this.btnGrab.Location = new System.Drawing.Point(664, 312);
            this.btnGrab.Margin = new System.Windows.Forms.Padding(2);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(140, 39);
            this.btnGrab.TabIndex = 1;
            this.btnGrab.Text = "连续采集";
            this.btnGrab.UseVisualStyleBackColor = true;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(672, 369);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(76, 21);
            this.textBox1.TabIndex = 3;
            // 
            // btnSoftRoop
            // 
            this.btnSoftRoop.Location = new System.Drawing.Point(839, 81);
            this.btnSoftRoop.Margin = new System.Windows.Forms.Padding(2);
            this.btnSoftRoop.Name = "btnSoftRoop";
            this.btnSoftRoop.Size = new System.Drawing.Size(112, 39);
            this.btnSoftRoop.TabIndex = 11;
            this.btnSoftRoop.Text = "无脑连采";
            this.btnSoftRoop.UseVisualStyleBackColor = true;
            this.btnSoftRoop.Click += new System.EventHandler(this.btnSoftRoop_Click);
            // 
            // lbIntvl
            // 
            this.lbIntvl.AutoSize = true;
            this.lbIntvl.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbIntvl.Location = new System.Drawing.Point(752, 370);
            this.lbIntvl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbIntvl.Name = "lbIntvl";
            this.lbIntvl.Size = new System.Drawing.Size(95, 19);
            this.lbIntvl.TabIndex = 12;
            this.lbIntvl.Text = "采集间隔:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(848, 153);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 16);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 549);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lbIntvl);
            this.Controls.Add(this.btnSoftRoop);
            this.Controls.Add(this.htWindow);
            this.Controls.Add(this.cmbCameraType);
            this.Controls.Add(this.lbImgNum);
            this.Controls.Add(this.lbGrabStatus);
            this.Controls.Add(this.tbCameraSN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSetGain);
            this.Controls.Add(this.btnGetGain);
            this.Controls.Add(this.btnSetExposure);
            this.Controls.Add(this.btnGetExposure);
            this.Controls.Add(this.tbGain);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tbExposure);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGrab);
            this.Controls.Add(this.btnSnap);
            this.Controls.Add(this.btnCloseCamera);
            this.Controls.Add(this.btnOpenCamera);
            this.Controls.Add(this.btnInitCamera);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInitCamera;
        private System.Windows.Forms.Button btnOpenCamera;
        private System.Windows.Forms.Button btnCloseCamera;
        private System.Windows.Forms.Button btnSnap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbExposure;
        private System.Windows.Forms.TextBox tbGain;
        private System.Windows.Forms.Button btnGetExposure;
        private System.Windows.Forms.Button btnSetExposure;
        private System.Windows.Forms.Button btnGetGain;
        private System.Windows.Forms.Button btnSetGain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCameraSN;
        private System.Windows.Forms.Label lbGrabStatus;
        private System.Windows.Forms.ComboBox cmbCameraType;
        private System.Windows.Forms.Label label4;
        private HTHalControl.HTWindowControl htWindow;
        private System.Windows.Forms.Label lbImgNum;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.TextBox textBox1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btnSoftRoop;
        private System.Windows.Forms.Label lbIntvl;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

