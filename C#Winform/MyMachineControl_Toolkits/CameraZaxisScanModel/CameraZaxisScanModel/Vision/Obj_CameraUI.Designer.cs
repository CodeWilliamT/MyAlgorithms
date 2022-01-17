namespace CameraZaxisScanModel
{
    partial class Obj_CameraUI
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
            this.cmbCameraList = new System.Windows.Forms.ComboBox();
            this.btnSaveConfig = new SelfControl.ColorButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInitCamera = new SelfControl.ColorButton();
            this.btnOpenCamera = new SelfControl.ColorButton();
            this.btnCloseCamera = new SelfControl.ColorButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConfigFolder = new SelfControl.ColorButton();
            this.tbCameraPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSetGain = new SelfControl.ColorButton();
            this.lbGrabStatus = new System.Windows.Forms.Label();
            this.btnSetExposure = new SelfControl.ColorButton();
            this.label5 = new System.Windows.Forms.Label();
            this.btnGetGain = new SelfControl.ColorButton();
            this.btnChgTrigSrc = new SelfControl.ColorButton();
            this.btnGetExposure = new SelfControl.ColorButton();
            this.textBox_SN = new System.Windows.Forms.TextBox();
            this.textBox_Gain = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Exposure = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGrab = new SelfControl.ColorButton();
            this.btnSnap = new SelfControl.ColorButton();
            this.timer_Grab = new System.Windows.Forms.Timer(this.components);
            this.btnOpenFolder = new SelfControl.ColorButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbCameraList
            // 
            this.cmbCameraList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraList.FormattingEnabled = true;
            this.cmbCameraList.Location = new System.Drawing.Point(87, 47);
            this.cmbCameraList.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.cmbCameraList.Name = "cmbCameraList";
            this.cmbCameraList.Size = new System.Drawing.Size(146, 25);
            this.cmbCameraList.TabIndex = 18;
            this.cmbCameraList.SelectedIndexChanged += new System.EventHandler(this.cmbCameraList_SelectedIndexChanged);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BorderColor = System.Drawing.Color.Empty;
            this.btnSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveConfig.GlassEffect = true;
            this.btnSaveConfig.Location = new System.Drawing.Point(479, 314);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Radius_All = 5;
            this.btnSaveConfig.Radius_BottomLeft = 5;
            this.btnSaveConfig.Radius_BottomRight = 5;
            this.btnSaveConfig.Radius_TopLeft = 5;
            this.btnSaveConfig.Radius_TopRight = 5;
            this.btnSaveConfig.Size = new System.Drawing.Size(78, 30);
            this.btnSaveConfig.TabIndex = 10;
            this.btnSaveConfig.Text = "保存配置";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "相机名称:";
            // 
            // btnInitCamera
            // 
            this.btnInitCamera.BorderColor = System.Drawing.Color.Empty;
            this.btnInitCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInitCamera.GlassEffect = true;
            this.btnInitCamera.Location = new System.Drawing.Point(337, 41);
            this.btnInitCamera.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnInitCamera.Name = "btnInitCamera";
            this.btnInitCamera.Radius_All = 5;
            this.btnInitCamera.Radius_BottomLeft = 5;
            this.btnInitCamera.Radius_BottomRight = 5;
            this.btnInitCamera.Radius_TopLeft = 5;
            this.btnInitCamera.Radius_TopRight = 5;
            this.btnInitCamera.Size = new System.Drawing.Size(220, 79);
            this.btnInitCamera.TabIndex = 7;
            this.btnInitCamera.Text = "初始化相机";
            this.btnInitCamera.UseVisualStyleBackColor = false;
            this.btnInitCamera.Click += new System.EventHandler(this.btnInitCamera_Click);
            // 
            // btnOpenCamera
            // 
            this.btnOpenCamera.BorderColor = System.Drawing.Color.Empty;
            this.btnOpenCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenCamera.GlassEffect = true;
            this.btnOpenCamera.Location = new System.Drawing.Point(338, 129);
            this.btnOpenCamera.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnOpenCamera.Name = "btnOpenCamera";
            this.btnOpenCamera.Radius_All = 5;
            this.btnOpenCamera.Radius_BottomLeft = 5;
            this.btnOpenCamera.Radius_BottomRight = 5;
            this.btnOpenCamera.Radius_TopLeft = 5;
            this.btnOpenCamera.Radius_TopRight = 5;
            this.btnOpenCamera.Size = new System.Drawing.Size(108, 81);
            this.btnOpenCamera.TabIndex = 5;
            this.btnOpenCamera.Text = "打开相机";
            this.btnOpenCamera.UseVisualStyleBackColor = false;
            this.btnOpenCamera.Click += new System.EventHandler(this.btnOpenCamera_Click);
            // 
            // btnCloseCamera
            // 
            this.btnCloseCamera.BorderColor = System.Drawing.Color.Empty;
            this.btnCloseCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseCamera.GlassEffect = true;
            this.btnCloseCamera.Location = new System.Drawing.Point(450, 129);
            this.btnCloseCamera.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnCloseCamera.Name = "btnCloseCamera";
            this.btnCloseCamera.Radius_All = 5;
            this.btnCloseCamera.Radius_BottomLeft = 5;
            this.btnCloseCamera.Radius_BottomRight = 5;
            this.btnCloseCamera.Radius_TopLeft = 5;
            this.btnCloseCamera.Radius_TopRight = 5;
            this.btnCloseCamera.Size = new System.Drawing.Size(108, 81);
            this.btnCloseCamera.TabIndex = 2;
            this.btnCloseCamera.Text = "关闭相机";
            this.btnCloseCamera.UseVisualStyleBackColor = false;
            this.btnCloseCamera.Click += new System.EventHandler(this.btnCloseCamera_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenFolder);
            this.groupBox1.Controls.Add(this.btnConfigFolder);
            this.groupBox1.Controls.Add(this.tbCameraPath);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnSetGain);
            this.groupBox1.Controls.Add(this.lbGrabStatus);
            this.groupBox1.Controls.Add(this.btnSetExposure);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnGetGain);
            this.groupBox1.Controls.Add(this.btnChgTrigSrc);
            this.groupBox1.Controls.Add(this.btnGetExposure);
            this.groupBox1.Controls.Add(this.textBox_SN);
            this.groupBox1.Controls.Add(this.textBox_Gain);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbCameraList);
            this.groupBox1.Controls.Add(this.textBox_Exposure);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSaveConfig);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGrab);
            this.groupBox1.Controls.Add(this.btnSnap);
            this.groupBox1.Controls.Add(this.btnInitCamera);
            this.groupBox1.Controls.Add(this.btnOpenCamera);
            this.groupBox1.Controls.Add(this.btnCloseCamera);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox1.Size = new System.Drawing.Size(573, 414);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机";
            // 
            // btnConfigFolder
            // 
            this.btnConfigFolder.BorderColor = System.Drawing.Color.Empty;
            this.btnConfigFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfigFolder.GlassEffect = true;
            this.btnConfigFolder.Location = new System.Drawing.Point(393, 314);
            this.btnConfigFolder.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnConfigFolder.Name = "btnConfigFolder";
            this.btnConfigFolder.Radius_All = 5;
            this.btnConfigFolder.Radius_BottomLeft = 5;
            this.btnConfigFolder.Radius_BottomRight = 5;
            this.btnConfigFolder.Radius_TopLeft = 5;
            this.btnConfigFolder.Radius_TopRight = 5;
            this.btnConfigFolder.Size = new System.Drawing.Size(82, 30);
            this.btnConfigFolder.TabIndex = 57;
            this.btnConfigFolder.Text = "浏览...";
            this.btnConfigFolder.UseVisualStyleBackColor = false;
            this.btnConfigFolder.Click += new System.EventHandler(this.btnConfigFolder_Click);
            // 
            // tbCameraPath
            // 
            this.tbCameraPath.Location = new System.Drawing.Point(107, 318);
            this.tbCameraPath.Name = "tbCameraPath";
            this.tbCameraPath.Size = new System.Drawing.Size(279, 23);
            this.tbCameraPath.TabIndex = 56;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 321);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 17);
            this.label6.TabIndex = 55;
            this.label6.Text = "图像存储目录:";
            // 
            // btnSetGain
            // 
            this.btnSetGain.BorderColor = System.Drawing.Color.Empty;
            this.btnSetGain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetGain.GlassEffect = true;
            this.btnSetGain.Location = new System.Drawing.Point(260, 190);
            this.btnSetGain.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSetGain.Name = "btnSetGain";
            this.btnSetGain.Radius_All = 5;
            this.btnSetGain.Radius_BottomLeft = 5;
            this.btnSetGain.Radius_BottomRight = 5;
            this.btnSetGain.Radius_TopLeft = 5;
            this.btnSetGain.Radius_TopRight = 5;
            this.btnSetGain.Size = new System.Drawing.Size(68, 47);
            this.btnSetGain.TabIndex = 50;
            this.btnSetGain.Text = "设置";
            this.btnSetGain.UseVisualStyleBackColor = false;
            this.btnSetGain.Click += new System.EventHandler(this.btnSetGain_Click);
            // 
            // lbGrabStatus
            // 
            this.lbGrabStatus.AutoSize = true;
            this.lbGrabStatus.Location = new System.Drawing.Point(238, 93);
            this.lbGrabStatus.Name = "lbGrabStatus";
            this.lbGrabStatus.Size = new System.Drawing.Size(72, 17);
            this.lbGrabStatus.TabIndex = 52;
            this.lbGrabStatus.Text = "GrabStatas";
            // 
            // btnSetExposure
            // 
            this.btnSetExposure.BorderColor = System.Drawing.Color.Empty;
            this.btnSetExposure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetExposure.GlassEffect = true;
            this.btnSetExposure.Location = new System.Drawing.Point(260, 138);
            this.btnSetExposure.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSetExposure.Name = "btnSetExposure";
            this.btnSetExposure.Radius_All = 5;
            this.btnSetExposure.Radius_BottomLeft = 5;
            this.btnSetExposure.Radius_BottomRight = 5;
            this.btnSetExposure.Radius_TopLeft = 5;
            this.btnSetExposure.Radius_TopRight = 5;
            this.btnSetExposure.Size = new System.Drawing.Size(68, 47);
            this.btnSetExposure.TabIndex = 49;
            this.btnSetExposure.Text = "设置";
            this.btnSetExposure.UseVisualStyleBackColor = false;
            this.btnSetExposure.Click += new System.EventHandler(this.btnSetExposure_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(224, 263);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 17);
            this.label5.TabIndex = 53;
            this.label5.Text = "触发模式：硬触发";
            // 
            // btnGetGain
            // 
            this.btnGetGain.BorderColor = System.Drawing.Color.Empty;
            this.btnGetGain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetGain.GlassEffect = true;
            this.btnGetGain.Location = new System.Drawing.Point(189, 190);
            this.btnGetGain.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnGetGain.Name = "btnGetGain";
            this.btnGetGain.Radius_All = 5;
            this.btnGetGain.Radius_BottomLeft = 5;
            this.btnGetGain.Radius_BottomRight = 5;
            this.btnGetGain.Radius_TopLeft = 5;
            this.btnGetGain.Radius_TopRight = 5;
            this.btnGetGain.Size = new System.Drawing.Size(68, 47);
            this.btnGetGain.TabIndex = 48;
            this.btnGetGain.Text = "获取";
            this.btnGetGain.UseVisualStyleBackColor = false;
            this.btnGetGain.Click += new System.EventHandler(this.btnGetGain_Click);
            // 
            // btnChgTrigSrc
            // 
            this.btnChgTrigSrc.BorderColor = System.Drawing.Color.Empty;
            this.btnChgTrigSrc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChgTrigSrc.GlassEffect = true;
            this.btnChgTrigSrc.Location = new System.Drawing.Point(15, 248);
            this.btnChgTrigSrc.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnChgTrigSrc.Name = "btnChgTrigSrc";
            this.btnChgTrigSrc.Radius_All = 5;
            this.btnChgTrigSrc.Radius_BottomLeft = 5;
            this.btnChgTrigSrc.Radius_BottomRight = 5;
            this.btnChgTrigSrc.Radius_TopLeft = 5;
            this.btnChgTrigSrc.Radius_TopRight = 5;
            this.btnChgTrigSrc.Size = new System.Drawing.Size(109, 47);
            this.btnChgTrigSrc.TabIndex = 54;
            this.btnChgTrigSrc.Text = "切触发模式";
            this.btnChgTrigSrc.UseVisualStyleBackColor = false;
            this.btnChgTrigSrc.Click += new System.EventHandler(this.btnChgTrigSrc_Click);
            // 
            // btnGetExposure
            // 
            this.btnGetExposure.BorderColor = System.Drawing.Color.Empty;
            this.btnGetExposure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetExposure.GlassEffect = true;
            this.btnGetExposure.Location = new System.Drawing.Point(189, 138);
            this.btnGetExposure.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnGetExposure.Name = "btnGetExposure";
            this.btnGetExposure.Radius_All = 5;
            this.btnGetExposure.Radius_BottomLeft = 5;
            this.btnGetExposure.Radius_BottomRight = 5;
            this.btnGetExposure.Radius_TopLeft = 5;
            this.btnGetExposure.Radius_TopRight = 5;
            this.btnGetExposure.Size = new System.Drawing.Size(68, 47);
            this.btnGetExposure.TabIndex = 51;
            this.btnGetExposure.Text = "获取";
            this.btnGetExposure.UseVisualStyleBackColor = false;
            this.btnGetExposure.Click += new System.EventHandler(this.btnGetExposure_Click);
            // 
            // textBox_SN
            // 
            this.textBox_SN.Location = new System.Drawing.Point(87, 93);
            this.textBox_SN.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox_SN.Name = "textBox_SN";
            this.textBox_SN.Size = new System.Drawing.Size(146, 23);
            this.textBox_SN.TabIndex = 20;
            // 
            // textBox_Gain
            // 
            this.textBox_Gain.Location = new System.Drawing.Point(87, 203);
            this.textBox_Gain.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox_Gain.Name = "textBox_Gain";
            this.textBox_Gain.Size = new System.Drawing.Size(94, 23);
            this.textBox_Gain.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "相机编号:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(20, 206);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 46;
            this.label3.Text = "增益值:";
            // 
            // textBox_Exposure
            // 
            this.textBox_Exposure.Location = new System.Drawing.Point(87, 148);
            this.textBox_Exposure.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox_Exposure.Name = "textBox_Exposure";
            this.textBox_Exposure.Size = new System.Drawing.Size(94, 23);
            this.textBox_Exposure.TabIndex = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(20, 152);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 44;
            this.label2.Text = "曝光值:";
            // 
            // btnGrab
            // 
            this.btnGrab.BorderColor = System.Drawing.Color.Empty;
            this.btnGrab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGrab.GlassEffect = true;
            this.btnGrab.Location = new System.Drawing.Point(450, 218);
            this.btnGrab.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Radius_All = 5;
            this.btnGrab.Radius_BottomLeft = 5;
            this.btnGrab.Radius_BottomRight = 5;
            this.btnGrab.Radius_TopLeft = 5;
            this.btnGrab.Radius_TopRight = 5;
            this.btnGrab.Size = new System.Drawing.Size(108, 81);
            this.btnGrab.TabIndex = 41;
            this.btnGrab.Text = "连续采集";
            this.btnGrab.UseVisualStyleBackColor = false;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.BorderColor = System.Drawing.Color.Empty;
            this.btnSnap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSnap.GlassEffect = true;
            this.btnSnap.Location = new System.Drawing.Point(337, 218);
            this.btnSnap.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Radius_All = 5;
            this.btnSnap.Radius_BottomLeft = 5;
            this.btnSnap.Radius_BottomRight = 5;
            this.btnSnap.Radius_TopLeft = 5;
            this.btnSnap.Radius_TopRight = 5;
            this.btnSnap.Size = new System.Drawing.Size(108, 81);
            this.btnSnap.TabIndex = 42;
            this.btnSnap.Text = "单张采集";
            this.btnSnap.UseVisualStyleBackColor = false;
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // timer_Grab
            // 
            this.timer_Grab.Tick += new System.EventHandler(this.timer_Grab_Tick);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BorderColor = System.Drawing.Color.Empty;
            this.btnOpenFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFolder.GlassEffect = true;
            this.btnOpenFolder.Location = new System.Drawing.Point(393, 352);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Radius_All = 5;
            this.btnOpenFolder.Radius_BottomLeft = 5;
            this.btnOpenFolder.Radius_BottomRight = 5;
            this.btnOpenFolder.Radius_TopLeft = 5;
            this.btnOpenFolder.Radius_TopRight = 5;
            this.btnOpenFolder.Size = new System.Drawing.Size(82, 30);
            this.btnOpenFolder.TabIndex = 58;
            this.btnOpenFolder.Text = "打开存图目录";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // Obj_CameraUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 414);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Obj_CameraUI";
            this.Text = "相机配置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Obj_CameraUI_FormClosed);
            this.Load += new System.EventHandler(this.Obj_CameraUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ComboBox cmbCameraList;
        private SelfControl.ColorButton btnSaveConfig;
        private System.Windows.Forms.Label label1;
        private SelfControl.ColorButton btnInitCamera;
        private SelfControl.ColorButton btnOpenCamera;
        private SelfControl.ColorButton btnCloseCamera;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_SN;
        private SelfControl.ColorButton btnChgTrigSrc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbGrabStatus;
        private SelfControl.ColorButton btnSetGain;
        private SelfControl.ColorButton btnSetExposure;
        private SelfControl.ColorButton btnGetGain;
        private SelfControl.ColorButton btnGetExposure;
        private System.Windows.Forms.TextBox textBox_Gain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Exposure;
        private System.Windows.Forms.Label label2;
        private SelfControl.ColorButton btnSnap;
        private SelfControl.ColorButton btnGrab;
        private System.Windows.Forms.Timer timer_Grab;
        private System.Windows.Forms.Label label6;
        private SelfControl.ColorButton btnConfigFolder;
        private System.Windows.Forms.TextBox tbCameraPath;
        private SelfControl.ColorButton btnOpenFolder;
    }
}