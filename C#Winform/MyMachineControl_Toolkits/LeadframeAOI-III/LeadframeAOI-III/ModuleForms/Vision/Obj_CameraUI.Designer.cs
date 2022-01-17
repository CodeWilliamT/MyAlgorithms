namespace LeadframeAOI.Modules.Vision
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
            this.cmbCameraList = new System.Windows.Forms.ComboBox();
            this.btnSaveConfig = new SelfControl.ColorButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInitCamera = new SelfControl.ColorButton();
            this.btnOpenCamera = new SelfControl.ColorButton();
            this.btnCloseCamera = new SelfControl.ColorButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbGrabStatus = new System.Windows.Forms.Label();
            this.textBox_SN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
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
            this.btnSaveConfig.Location = new System.Drawing.Point(15, 129);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Radius_All = 5;
            this.btnSaveConfig.Radius_BottomLeft = 5;
            this.btnSaveConfig.Radius_BottomRight = 5;
            this.btnSaveConfig.Radius_TopLeft = 5;
            this.btnSaveConfig.Radius_TopRight = 5;
            this.btnSaveConfig.Size = new System.Drawing.Size(306, 81);
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
            this.btnOpenCamera.Location = new System.Drawing.Point(337, 126);
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
            this.btnCloseCamera.Location = new System.Drawing.Point(450, 126);
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
            this.groupBox1.Controls.Add(this.lbGrabStatus);
            this.groupBox1.Controls.Add(this.textBox_SN);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbCameraList);
            this.groupBox1.Controls.Add(this.btnSaveConfig);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnInitCamera);
            this.groupBox1.Controls.Add(this.btnOpenCamera);
            this.groupBox1.Controls.Add(this.btnCloseCamera);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBox1.Size = new System.Drawing.Size(573, 234);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机";
            // 
            // lbGrabStatus
            // 
            this.lbGrabStatus.AutoSize = true;
            this.lbGrabStatus.Location = new System.Drawing.Point(238, 50);
            this.lbGrabStatus.Name = "lbGrabStatus";
            this.lbGrabStatus.Size = new System.Drawing.Size(72, 17);
            this.lbGrabStatus.TabIndex = 21;
            this.lbGrabStatus.Text = "GrabStatas";
            // 
            // textBox_SN
            // 
            this.textBox_SN.Location = new System.Drawing.Point(87, 93);
            this.textBox_SN.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox_SN.Name = "textBox_SN";
            this.textBox_SN.Size = new System.Drawing.Size(146, 23);
            this.textBox_SN.TabIndex = 20;
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
            // Obj_CameraUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 234);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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

        private System.Windows.Forms.ComboBox cmbCameraList;
        private SelfControl.ColorButton btnSaveConfig;
        private System.Windows.Forms.Label label1;
        private SelfControl.ColorButton btnInitCamera;
        private SelfControl.ColorButton btnOpenCamera;
        private SelfControl.ColorButton btnCloseCamera;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_SN;
        private System.Windows.Forms.Label lbGrabStatus;
    }
}