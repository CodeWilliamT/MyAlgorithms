namespace LeadframeAOI
{
    partial class VisionUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisionUI));
            this.tbImageFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSaveImage = new System.Windows.Forms.CheckBox();
            this.btnConfigImagePath = new SelfControl.ColorButton();
            this.btnVisionSave = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLoadImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSnap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGrab = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStopGrab = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveImage = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnTestDetect = new SelfControl.ColorButton();
            this.btnTestLocate = new SelfControl.ColorButton();
            this.btnTestCode = new SelfControl.ColorButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbGain3 = new System.Windows.Forms.TextBox();
            this.tbExposure3 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbGain2 = new System.Windows.Forms.TextBox();
            this.tbExposure2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSetEG = new System.Windows.Forms.Button();
            this.btnGetEG = new System.Windows.Forms.Button();
            this.tbGain1 = new System.Windows.Forms.TextBox();
            this.tbExposure1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnChgTrigSrc = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSetGain = new System.Windows.Forms.Button();
            this.btnGetGain = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbVResultDetect = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVResultY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbVResultX = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnSetExposure = new System.Windows.Forms.Button();
            this.btnGetExposure = new System.Windows.Forms.Button();
            this.tbGain = new System.Windows.Forms.TextBox();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCameraname = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnfuctionTest = new System.Windows.Forms.Button();
            this.btnShowFocusRegion = new System.Windows.Forms.Button();
            this.btnGetFocusRegion = new System.Windows.Forms.Button();
            this.btnInspectionConfig = new System.Windows.Forms.Button();
            this.lbGrabStatus = new System.Windows.Forms.Label();
            this.lbImgNum = new System.Windows.Forms.Label();
            this.btnConfigTestPath = new SelfControl.ColorButton();
            this.txtTestImageFolder = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbImageFolder
            // 
            this.tbImageFolder.Location = new System.Drawing.Point(189, 155);
            this.tbImageFolder.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbImageFolder.Name = "tbImageFolder";
            this.tbImageFolder.Size = new System.Drawing.Size(317, 21);
            this.tbImageFolder.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(72, 155);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "图像保存位置：";
            // 
            // cbSaveImage
            // 
            this.cbSaveImage.AutoSize = true;
            this.cbSaveImage.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbSaveImage.Location = new System.Drawing.Point(354, 245);
            this.cbSaveImage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbSaveImage.Name = "cbSaveImage";
            this.cbSaveImage.Size = new System.Drawing.Size(128, 21);
            this.cbSaveImage.TabIndex = 18;
            this.cbSaveImage.Text = "Snap自动保存图像";
            this.cbSaveImage.UseVisualStyleBackColor = true;
            this.cbSaveImage.CheckedChanged += new System.EventHandler(this.cbSaveImage_CheckedChanged);
            // 
            // btnConfigImagePath
            // 
            this.btnConfigImagePath.BorderColor = System.Drawing.Color.Empty;
            this.btnConfigImagePath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfigImagePath.GlassEffect = true;
            this.btnConfigImagePath.Location = new System.Drawing.Point(525, 155);
            this.btnConfigImagePath.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnConfigImagePath.Name = "btnConfigImagePath";
            this.btnConfigImagePath.Radius_All = 5;
            this.btnConfigImagePath.Radius_BottomLeft = 5;
            this.btnConfigImagePath.Radius_BottomRight = 5;
            this.btnConfigImagePath.Radius_TopLeft = 5;
            this.btnConfigImagePath.Radius_TopRight = 5;
            this.btnConfigImagePath.Size = new System.Drawing.Size(85, 25);
            this.btnConfigImagePath.TabIndex = 19;
            this.btnConfigImagePath.Text = "配置路径";
            this.btnConfigImagePath.UseVisualStyleBackColor = false;
            this.btnConfigImagePath.Click += new System.EventHandler(this.btnConfigImagePath_Click);
            // 
            // btnVisionSave
            // 
            this.btnVisionSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVisionSave.BackgroundImage")));
            this.btnVisionSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnVisionSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVisionSave.Location = new System.Drawing.Point(562, 230);
            this.btnVisionSave.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnVisionSave.Name = "btnVisionSave";
            this.btnVisionSave.Size = new System.Drawing.Size(48, 51);
            this.btnVisionSave.TabIndex = 21;
            this.btnVisionSave.UseVisualStyleBackColor = true;
            this.btnVisionSave.Click += new System.EventHandler(this.btnVisionSave_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadImage,
            this.toolStripSeparator1,
            this.btnSnap,
            this.toolStripSeparator3,
            this.btnGrab,
            this.toolStripSeparator4,
            this.btnStopGrab,
            this.toolStripSeparator2,
            this.btnSaveImage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(34, 289);
            this.toolStrip1.TabIndex = 22;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadImage.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadImage.Image")));
            this.btnLoadImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLoadImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(32, 36);
            this.btnLoadImage.Text = "载入图像";
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(32, 6);
            // 
            // btnSnap
            // 
            this.btnSnap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSnap.Image = ((System.Drawing.Image)(resources.GetObject("btnSnap.Image")));
            this.btnSnap.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(32, 36);
            this.btnSnap.Text = "单次拍照";
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(32, 6);
            // 
            // btnGrab
            // 
            this.btnGrab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGrab.Image = ((System.Drawing.Image)(resources.GetObject("btnGrab.Image")));
            this.btnGrab.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGrab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(32, 36);
            this.btnGrab.Text = "连续拍照";
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(32, 6);
            // 
            // btnStopGrab
            // 
            this.btnStopGrab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStopGrab.Image = ((System.Drawing.Image)(resources.GetObject("btnStopGrab.Image")));
            this.btnStopGrab.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStopGrab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStopGrab.Name = "btnStopGrab";
            this.btnStopGrab.Size = new System.Drawing.Size(32, 36);
            this.btnStopGrab.Text = "停止拍照";
            this.btnStopGrab.Click += new System.EventHandler(this.btnStopGrab_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(32, 6);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveImage.Image")));
            this.btnSaveImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(32, 36);
            this.btnSaveImage.Text = "保存图像";
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnChgTrigSrc);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnSetGain);
            this.groupBox1.Controls.Add(this.btnGetGain);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnSetExposure);
            this.groupBox1.Controls.Add(this.btnGetExposure);
            this.groupBox1.Controls.Add(this.tbGain);
            this.groupBox1.Controls.Add(this.tbExposure);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(36, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(303, 139);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数设置";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnTestDetect);
            this.groupBox4.Controls.Add(this.btnTestLocate);
            this.groupBox4.Controls.Add(this.btnTestCode);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(369, 192);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Size = new System.Drawing.Size(167, 187);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "测试";
            this.groupBox4.Visible = false;
            // 
            // btnTestDetect
            // 
            this.btnTestDetect.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnTestDetect.BorderColor = System.Drawing.Color.Empty;
            this.btnTestDetect.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestDetect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTestDetect.GlassEffect = true;
            this.btnTestDetect.Location = new System.Drawing.Point(8, 135);
            this.btnTestDetect.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnTestDetect.Name = "btnTestDetect";
            this.btnTestDetect.Radius_All = 5;
            this.btnTestDetect.Radius_BottomLeft = 5;
            this.btnTestDetect.Radius_BottomRight = 5;
            this.btnTestDetect.Radius_TopLeft = 5;
            this.btnTestDetect.Radius_TopRight = 5;
            this.btnTestDetect.Size = new System.Drawing.Size(154, 32);
            this.btnTestDetect.TabIndex = 7;
            this.btnTestDetect.Text = "芯片检测";
            this.btnTestDetect.UseVisualStyleBackColor = false;
            // 
            // btnTestLocate
            // 
            this.btnTestLocate.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnTestLocate.BorderColor = System.Drawing.Color.Empty;
            this.btnTestLocate.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestLocate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTestLocate.GlassEffect = true;
            this.btnTestLocate.Location = new System.Drawing.Point(8, 90);
            this.btnTestLocate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnTestLocate.Name = "btnTestLocate";
            this.btnTestLocate.Radius_All = 5;
            this.btnTestLocate.Radius_BottomLeft = 5;
            this.btnTestLocate.Radius_BottomRight = 5;
            this.btnTestLocate.Radius_TopLeft = 5;
            this.btnTestLocate.Radius_TopRight = 5;
            this.btnTestLocate.Size = new System.Drawing.Size(154, 32);
            this.btnTestLocate.TabIndex = 6;
            this.btnTestLocate.Text = "芯片定位";
            this.btnTestLocate.UseVisualStyleBackColor = false;
            // 
            // btnTestCode
            // 
            this.btnTestCode.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnTestCode.BorderColor = System.Drawing.Color.Empty;
            this.btnTestCode.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTestCode.GlassEffect = true;
            this.btnTestCode.Location = new System.Drawing.Point(8, 42);
            this.btnTestCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnTestCode.Name = "btnTestCode";
            this.btnTestCode.Radius_All = 5;
            this.btnTestCode.Radius_BottomLeft = 5;
            this.btnTestCode.Radius_BottomRight = 5;
            this.btnTestCode.Radius_TopLeft = 5;
            this.btnTestCode.Radius_TopRight = 5;
            this.btnTestCode.Size = new System.Drawing.Size(154, 32);
            this.btnTestCode.TabIndex = 5;
            this.btnTestCode.Text = "二维码定位";
            this.btnTestCode.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbGain3);
            this.groupBox3.Controls.Add(this.tbExposure3);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.tbGain2);
            this.groupBox3.Controls.Add(this.tbExposure2);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.btnSetEG);
            this.groupBox3.Controls.Add(this.btnGetEG);
            this.groupBox3.Controls.Add(this.tbGain1);
            this.groupBox3.Controls.Add(this.tbExposure1);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(15, 157);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(323, 222);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "扫描时相机配置";
            this.groupBox3.Visible = false;
            // 
            // tbGain3
            // 
            this.tbGain3.Location = new System.Drawing.Point(133, 194);
            this.tbGain3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbGain3.Name = "tbGain3";
            this.tbGain3.Size = new System.Drawing.Size(76, 23);
            this.tbGain3.TabIndex = 30;
            // 
            // tbExposure3
            // 
            this.tbExposure3.Location = new System.Drawing.Point(133, 166);
            this.tbExposure3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbExposure3.Name = "tbExposure3";
            this.tbExposure3.Size = new System.Drawing.Size(76, 23);
            this.tbExposure3.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 198);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 17);
            this.label12.TabIndex = 28;
            this.label12.Text = "三次增益(33-512)：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 168);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(125, 17);
            this.label13.TabIndex = 27;
            this.label13.Text = "三次曝光(27-10^7)：";
            // 
            // tbGain2
            // 
            this.tbGain2.Location = new System.Drawing.Point(133, 123);
            this.tbGain2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbGain2.Name = "tbGain2";
            this.tbGain2.Size = new System.Drawing.Size(76, 23);
            this.tbGain2.TabIndex = 22;
            // 
            // tbExposure2
            // 
            this.tbExposure2.Location = new System.Drawing.Point(133, 95);
            this.tbExposure2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbExposure2.Name = "tbExposure2";
            this.tbExposure2.Size = new System.Drawing.Size(76, 23);
            this.tbExposure2.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 126);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "二次增益(33-512)：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 98);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 17);
            this.label11.TabIndex = 19;
            this.label11.Text = "二次曝光(27-10^7)：";
            // 
            // btnSetEG
            // 
            this.btnSetEG.Location = new System.Drawing.Point(266, 193);
            this.btnSetEG.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetEG.Name = "btnSetEG";
            this.btnSetEG.Size = new System.Drawing.Size(40, 22);
            this.btnSetEG.TabIndex = 16;
            this.btnSetEG.Text = "Set";
            this.btnSetEG.UseVisualStyleBackColor = true;
            this.btnSetEG.Click += new System.EventHandler(this.btnSetEG_Click);
            // 
            // btnGetEG
            // 
            this.btnGetEG.Location = new System.Drawing.Point(223, 193);
            this.btnGetEG.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetEG.Name = "btnGetEG";
            this.btnGetEG.Size = new System.Drawing.Size(40, 22);
            this.btnGetEG.TabIndex = 15;
            this.btnGetEG.Text = "Get";
            this.btnGetEG.UseVisualStyleBackColor = true;
            this.btnGetEG.Click += new System.EventHandler(this.btnGetEG_Click);
            // 
            // tbGain1
            // 
            this.tbGain1.Location = new System.Drawing.Point(133, 58);
            this.tbGain1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbGain1.Name = "tbGain1";
            this.tbGain1.Size = new System.Drawing.Size(76, 23);
            this.tbGain1.TabIndex = 14;
            // 
            // tbExposure1
            // 
            this.tbExposure1.Location = new System.Drawing.Point(133, 30);
            this.tbExposure1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbExposure1.Name = "tbExposure1";
            this.tbExposure1.Size = new System.Drawing.Size(76, 23);
            this.tbExposure1.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 61);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "首次增益(33-512)：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 31);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 17);
            this.label9.TabIndex = 11;
            this.label9.Text = "首次曝光(27-10^7)：";
            // 
            // btnChgTrigSrc
            // 
            this.btnChgTrigSrc.Location = new System.Drawing.Point(208, 94);
            this.btnChgTrigSrc.Name = "btnChgTrigSrc";
            this.btnChgTrigSrc.Size = new System.Drawing.Size(84, 22);
            this.btnChgTrigSrc.TabIndex = 9;
            this.btnChgTrigSrc.Text = "切触发模式";
            this.btnChgTrigSrc.UseVisualStyleBackColor = true;
            this.btnChgTrigSrc.Click += new System.EventHandler(this.btnChgTrigSrc_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "触发模式：硬触发";
            // 
            // btnSetGain
            // 
            this.btnSetGain.Location = new System.Drawing.Point(252, 58);
            this.btnSetGain.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetGain.Name = "btnSetGain";
            this.btnSetGain.Size = new System.Drawing.Size(40, 23);
            this.btnSetGain.TabIndex = 7;
            this.btnSetGain.Text = "Set";
            this.btnSetGain.UseVisualStyleBackColor = true;
            this.btnSetGain.Click += new System.EventHandler(this.btnSetGain_Click);
            // 
            // btnGetGain
            // 
            this.btnGetGain.Location = new System.Drawing.Point(208, 58);
            this.btnGetGain.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetGain.Name = "btnGetGain";
            this.btnGetGain.Size = new System.Drawing.Size(40, 23);
            this.btnGetGain.TabIndex = 6;
            this.btnGetGain.Text = "Get";
            this.btnGetGain.UseVisualStyleBackColor = true;
            this.btnGetGain.Click += new System.EventHandler(this.btnGetGain_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbVResultDetect);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbVResultY);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbVResultX);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(342, 22);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox2.Size = new System.Drawing.Size(168, 284);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试结果";
            this.groupBox2.Visible = false;
            // 
            // tbVResultDetect
            // 
            this.tbVResultDetect.Location = new System.Drawing.Point(70, 116);
            this.tbVResultDetect.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbVResultDetect.Name = "tbVResultDetect";
            this.tbVResultDetect.Size = new System.Drawing.Size(79, 23);
            this.tbVResultDetect.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "检测结果";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(88, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "(pixel)";
            // 
            // tbVResultY
            // 
            this.tbVResultY.Location = new System.Drawing.Point(70, 79);
            this.tbVResultY.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbVResultY.Name = "tbVResultY";
            this.tbVResultY.Size = new System.Drawing.Size(79, 23);
            this.tbVResultY.TabIndex = 17;
            this.tbVResultY.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 81);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "定位中心Y";
            // 
            // tbVResultX
            // 
            this.tbVResultX.Location = new System.Drawing.Point(70, 44);
            this.tbVResultX.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbVResultX.Name = "tbVResultX";
            this.tbVResultX.Size = new System.Drawing.Size(79, 23);
            this.tbVResultX.TabIndex = 15;
            this.tbVResultX.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(7, 46);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 17);
            this.label15.TabIndex = 14;
            this.label15.Text = "定位中心X";
            // 
            // btnSetExposure
            // 
            this.btnSetExposure.Location = new System.Drawing.Point(252, 31);
            this.btnSetExposure.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetExposure.Name = "btnSetExposure";
            this.btnSetExposure.Size = new System.Drawing.Size(40, 22);
            this.btnSetExposure.TabIndex = 5;
            this.btnSetExposure.Text = "Set";
            this.btnSetExposure.UseVisualStyleBackColor = true;
            this.btnSetExposure.Click += new System.EventHandler(this.btnSetExposure_Click);
            // 
            // btnGetExposure
            // 
            this.btnGetExposure.Location = new System.Drawing.Point(208, 31);
            this.btnGetExposure.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetExposure.Name = "btnGetExposure";
            this.btnGetExposure.Size = new System.Drawing.Size(40, 22);
            this.btnGetExposure.TabIndex = 4;
            this.btnGetExposure.Text = "Get";
            this.btnGetExposure.UseVisualStyleBackColor = true;
            this.btnGetExposure.Click += new System.EventHandler(this.btnGetExposure_Click);
            // 
            // tbGain
            // 
            this.tbGain.Location = new System.Drawing.Point(129, 59);
            this.tbGain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(76, 23);
            this.tbGain.TabIndex = 3;
            // 
            // tbExposure
            // 
            this.tbExposure.Location = new System.Drawing.Point(129, 31);
            this.tbExposure.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(76, 23);
            this.tbExposure.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 62);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "增益(33-512)：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 33);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "曝光(27-10^7)：";
            // 
            // tbCameraname
            // 
            this.tbCameraname.Location = new System.Drawing.Point(472, 23);
            this.tbCameraname.Margin = new System.Windows.Forms.Padding(2);
            this.tbCameraname.Name = "tbCameraname";
            this.tbCameraname.Size = new System.Drawing.Size(116, 21);
            this.tbCameraname.TabIndex = 37;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(352, 27);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 12);
            this.label14.TabIndex = 36;
            this.label14.Text = "相机设备编号：";
            // 
            // btnfuctionTest
            // 
            this.btnfuctionTest.Location = new System.Drawing.Point(531, 111);
            this.btnfuctionTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnfuctionTest.Name = "btnfuctionTest";
            this.btnfuctionTest.Size = new System.Drawing.Size(146, 26);
            this.btnfuctionTest.TabIndex = 35;
            this.btnfuctionTest.Text = "检测算法测试界面";
            this.btnfuctionTest.UseVisualStyleBackColor = true;
            this.btnfuctionTest.Click += new System.EventHandler(this.btnfuctionTest_Click);
            // 
            // btnShowFocusRegion
            // 
            this.btnShowFocusRegion.Location = new System.Drawing.Point(532, 62);
            this.btnShowFocusRegion.Name = "btnShowFocusRegion";
            this.btnShowFocusRegion.Size = new System.Drawing.Size(145, 26);
            this.btnShowFocusRegion.TabIndex = 34;
            this.btnShowFocusRegion.Text = "显示自动聚焦兴趣区";
            this.btnShowFocusRegion.UseVisualStyleBackColor = true;
            this.btnShowFocusRegion.Click += new System.EventHandler(this.btnShowFocusRegion_Click);
            // 
            // btnGetFocusRegion
            // 
            this.btnGetFocusRegion.Location = new System.Drawing.Point(354, 63);
            this.btnGetFocusRegion.Name = "btnGetFocusRegion";
            this.btnGetFocusRegion.Size = new System.Drawing.Size(146, 26);
            this.btnGetFocusRegion.TabIndex = 33;
            this.btnGetFocusRegion.Text = "获取自动聚焦兴趣区";
            this.btnGetFocusRegion.UseVisualStyleBackColor = true;
            this.btnGetFocusRegion.Click += new System.EventHandler(this.btnGetFocusRegion_Click);
            // 
            // btnInspectionConfig
            // 
            this.btnInspectionConfig.Location = new System.Drawing.Point(354, 113);
            this.btnInspectionConfig.Margin = new System.Windows.Forms.Padding(2);
            this.btnInspectionConfig.Name = "btnInspectionConfig";
            this.btnInspectionConfig.Size = new System.Drawing.Size(146, 26);
            this.btnInspectionConfig.TabIndex = 32;
            this.btnInspectionConfig.Text = "检测算法配置界面";
            this.btnInspectionConfig.UseVisualStyleBackColor = true;
            this.btnInspectionConfig.Click += new System.EventHandler(this.btnInspectionConfig_Click);
            // 
            // lbGrabStatus
            // 
            this.lbGrabStatus.AutoSize = true;
            this.lbGrabStatus.Location = new System.Drawing.Point(612, 5);
            this.lbGrabStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbGrabStatus.Name = "lbGrabStatus";
            this.lbGrabStatus.Size = new System.Drawing.Size(65, 12);
            this.lbGrabStatus.TabIndex = 26;
            this.lbGrabStatus.Text = "GrabStatus";
            // 
            // lbImgNum
            // 
            this.lbImgNum.AutoSize = true;
            this.lbImgNum.Location = new System.Drawing.Point(612, 40);
            this.lbImgNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbImgNum.Name = "lbImgNum";
            this.lbImgNum.Size = new System.Drawing.Size(53, 12);
            this.lbImgNum.TabIndex = 27;
            this.lbImgNum.Text = "ImageNum";
            // 
            // btnConfigTestPath
            // 
            this.btnConfigTestPath.BorderColor = System.Drawing.Color.Empty;
            this.btnConfigTestPath.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfigTestPath.GlassEffect = true;
            this.btnConfigTestPath.Location = new System.Drawing.Point(525, 197);
            this.btnConfigTestPath.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnConfigTestPath.Name = "btnConfigTestPath";
            this.btnConfigTestPath.Radius_All = 5;
            this.btnConfigTestPath.Radius_BottomLeft = 5;
            this.btnConfigTestPath.Radius_BottomRight = 5;
            this.btnConfigTestPath.Radius_TopLeft = 5;
            this.btnConfigTestPath.Radius_TopRight = 5;
            this.btnConfigTestPath.Size = new System.Drawing.Size(85, 25);
            this.btnConfigTestPath.TabIndex = 40;
            this.btnConfigTestPath.Text = "配置路径";
            this.btnConfigTestPath.UseVisualStyleBackColor = false;
            this.btnConfigTestPath.Click += new System.EventHandler(this.btnConfigTestPath_Click);
            // 
            // txtTestImageFolder
            // 
            this.txtTestImageFolder.Location = new System.Drawing.Point(189, 198);
            this.txtTestImageFolder.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtTestImageFolder.Name = "txtTestImageFolder";
            this.txtTestImageFolder.Size = new System.Drawing.Size(317, 21);
            this.txtTestImageFolder.TabIndex = 38;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(36, 197);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(128, 17);
            this.label16.TabIndex = 39;
            this.label16.Text = "测试用图像保存位置：";
            // 
            // VisionUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(699, 289);
            this.Controls.Add(this.btnConfigTestPath);
            this.Controls.Add(this.txtTestImageFolder);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tbCameraname);
            this.Controls.Add(this.lbImgNum);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lbGrabStatus);
            this.Controls.Add(this.btnVisionSave);
            this.Controls.Add(this.btnfuctionTest);
            this.Controls.Add(this.btnShowFocusRegion);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetFocusRegion);
            this.Controls.Add(this.btnInspectionConfig);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnConfigImagePath);
            this.Controls.Add(this.tbImageFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSaveImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "VisionUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VisionUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VisionUI_FormClosed);
            this.Load += new System.EventHandler(this.VisionUI_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbImageFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbSaveImage;
        private SelfControl.ColorButton btnConfigImagePath;
        private System.Windows.Forms.Button btnVisionSave;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbVResultDetect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbVResultY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbVResultX;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox4;
        private SelfControl.ColorButton btnTestDetect;
        private SelfControl.ColorButton btnTestLocate;
        private SelfControl.ColorButton btnTestCode;
        private System.Windows.Forms.ToolStripButton btnLoadImage;
        private System.Windows.Forms.ToolStripButton btnSnap;
        private System.Windows.Forms.ToolStripButton btnGrab;
        private System.Windows.Forms.ToolStripButton btnStopGrab;
        private System.Windows.Forms.ToolStripButton btnSaveImage;
        private System.Windows.Forms.TextBox tbGain;
        private System.Windows.Forms.TextBox tbExposure;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbGrabStatus;
        private System.Windows.Forms.Label lbImgNum;
        private System.Windows.Forms.Button btnSetGain;
        private System.Windows.Forms.Button btnGetGain;
        private System.Windows.Forms.Button btnSetExposure;
        private System.Windows.Forms.Button btnGetExposure;
        private System.Windows.Forms.Button btnChgTrigSrc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbGain3;
        private System.Windows.Forms.TextBox tbExposure3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbGain2;
        private System.Windows.Forms.TextBox tbExposure2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSetEG;
        private System.Windows.Forms.Button btnGetEG;
        private System.Windows.Forms.TextBox tbGain1;
        private System.Windows.Forms.TextBox tbExposure1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnInspectionConfig;
        private System.Windows.Forms.Button btnGetFocusRegion;
        private System.Windows.Forms.Button btnShowFocusRegion;
        private System.Windows.Forms.Button btnfuctionTest;
        private System.Windows.Forms.TextBox tbCameraname;
        private System.Windows.Forms.Label label14;
        private SelfControl.ColorButton btnConfigTestPath;
        private System.Windows.Forms.TextBox txtTestImageFolder;
        private System.Windows.Forms.Label label16;
    }
}