namespace LeadframeAOI.Modules.Vision
{
    partial class Obj_CameraSoftUI
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
            this.btnChgTrigSrc = new SelfControl.ColorButton();
            this.label5 = new System.Windows.Forms.Label();
            this.lbGrabStatus = new System.Windows.Forms.Label();
            this.cmbCameraList = new System.Windows.Forms.ComboBox();
            this.btnSetGain = new SelfControl.ColorButton();
            this.btnSetExposure = new SelfControl.ColorButton();
            this.btnGetGain = new SelfControl.ColorButton();
            this.btnGetExposure = new SelfControl.ColorButton();
            this.textBox_Gain = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Exposure = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaveConfig = new SelfControl.ColorButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSnap = new SelfControl.ColorButton();
            this.btnGrab = new SelfControl.ColorButton();
            this.timer_Grab = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnChgTrigSrc
            // 
            this.btnChgTrigSrc.BorderColor = System.Drawing.Color.Empty;
            this.btnChgTrigSrc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChgTrigSrc.GlassEffect = true;
            this.btnChgTrigSrc.Location = new System.Drawing.Point(431, 35);
            this.btnChgTrigSrc.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnChgTrigSrc.Name = "btnChgTrigSrc";
            this.btnChgTrigSrc.Radius_All = 5;
            this.btnChgTrigSrc.Radius_BottomLeft = 5;
            this.btnChgTrigSrc.Radius_BottomRight = 5;
            this.btnChgTrigSrc.Radius_TopLeft = 5;
            this.btnChgTrigSrc.Radius_TopRight = 5;
            this.btnChgTrigSrc.Size = new System.Drawing.Size(139, 38);
            this.btnChgTrigSrc.TabIndex = 40;
            this.btnChgTrigSrc.Text = "切触发模式";
            this.btnChgTrigSrc.UseVisualStyleBackColor = false;
            this.btnChgTrigSrc.Click += new System.EventHandler(this.btnChgTrigSrc_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(467, 120);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 39;
            this.label5.Text = "触发模式：硬触发";
            // 
            // lbGrabStatus
            // 
            this.lbGrabStatus.AutoSize = true;
            this.lbGrabStatus.Location = new System.Drawing.Point(267, 35);
            this.lbGrabStatus.Name = "lbGrabStatus";
            this.lbGrabStatus.Size = new System.Drawing.Size(65, 12);
            this.lbGrabStatus.TabIndex = 38;
            this.lbGrabStatus.Text = "GrabStatas";
            // 
            // cmbCameraList
            // 
            this.cmbCameraList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraList.FormattingEnabled = true;
            this.cmbCameraList.Location = new System.Drawing.Point(98, 32);
            this.cmbCameraList.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.cmbCameraList.Name = "cmbCameraList";
            this.cmbCameraList.Size = new System.Drawing.Size(146, 20);
            this.cmbCameraList.TabIndex = 37;
            this.cmbCameraList.SelectedIndexChanged += new System.EventHandler(this.cmbCameraList_SelectedIndexChanged);
            // 
            // btnSetGain
            // 
            this.btnSetGain.BorderColor = System.Drawing.Color.Empty;
            this.btnSetGain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetGain.GlassEffect = true;
            this.btnSetGain.Location = new System.Drawing.Point(264, 120);
            this.btnSetGain.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSetGain.Name = "btnSetGain";
            this.btnSetGain.Radius_All = 5;
            this.btnSetGain.Radius_BottomLeft = 5;
            this.btnSetGain.Radius_BottomRight = 5;
            this.btnSetGain.Radius_TopLeft = 5;
            this.btnSetGain.Radius_TopRight = 5;
            this.btnSetGain.Size = new System.Drawing.Size(68, 47);
            this.btnSetGain.TabIndex = 35;
            this.btnSetGain.Text = "设置";
            this.btnSetGain.UseVisualStyleBackColor = false;
            this.btnSetGain.Click += new System.EventHandler(this.btnSetGain_Click);
            // 
            // btnSetExposure
            // 
            this.btnSetExposure.BorderColor = System.Drawing.Color.Empty;
            this.btnSetExposure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetExposure.GlassEffect = true;
            this.btnSetExposure.Location = new System.Drawing.Point(264, 68);
            this.btnSetExposure.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSetExposure.Name = "btnSetExposure";
            this.btnSetExposure.Radius_All = 5;
            this.btnSetExposure.Radius_BottomLeft = 5;
            this.btnSetExposure.Radius_BottomRight = 5;
            this.btnSetExposure.Radius_TopLeft = 5;
            this.btnSetExposure.Radius_TopRight = 5;
            this.btnSetExposure.Size = new System.Drawing.Size(68, 47);
            this.btnSetExposure.TabIndex = 34;
            this.btnSetExposure.Text = "设置";
            this.btnSetExposure.UseVisualStyleBackColor = false;
            this.btnSetExposure.Click += new System.EventHandler(this.btnSetExposure_Click);
            // 
            // btnGetGain
            // 
            this.btnGetGain.BorderColor = System.Drawing.Color.Empty;
            this.btnGetGain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetGain.GlassEffect = true;
            this.btnGetGain.Location = new System.Drawing.Point(193, 120);
            this.btnGetGain.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnGetGain.Name = "btnGetGain";
            this.btnGetGain.Radius_All = 5;
            this.btnGetGain.Radius_BottomLeft = 5;
            this.btnGetGain.Radius_BottomRight = 5;
            this.btnGetGain.Radius_TopLeft = 5;
            this.btnGetGain.Radius_TopRight = 5;
            this.btnGetGain.Size = new System.Drawing.Size(68, 47);
            this.btnGetGain.TabIndex = 33;
            this.btnGetGain.Text = "获取";
            this.btnGetGain.UseVisualStyleBackColor = false;
            this.btnGetGain.Click += new System.EventHandler(this.btnGetGain_Click);
            // 
            // btnGetExposure
            // 
            this.btnGetExposure.BorderColor = System.Drawing.Color.Empty;
            this.btnGetExposure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetExposure.GlassEffect = true;
            this.btnGetExposure.Location = new System.Drawing.Point(193, 68);
            this.btnGetExposure.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnGetExposure.Name = "btnGetExposure";
            this.btnGetExposure.Radius_All = 5;
            this.btnGetExposure.Radius_BottomLeft = 5;
            this.btnGetExposure.Radius_BottomRight = 5;
            this.btnGetExposure.Radius_TopLeft = 5;
            this.btnGetExposure.Radius_TopRight = 5;
            this.btnGetExposure.Size = new System.Drawing.Size(68, 47);
            this.btnGetExposure.TabIndex = 36;
            this.btnGetExposure.Text = "获取";
            this.btnGetExposure.UseVisualStyleBackColor = false;
            this.btnGetExposure.Click += new System.EventHandler(this.btnGetExposure_Click);
            // 
            // textBox_Gain
            // 
            this.textBox_Gain.Location = new System.Drawing.Point(91, 133);
            this.textBox_Gain.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox_Gain.Name = "textBox_Gain";
            this.textBox_Gain.Size = new System.Drawing.Size(94, 21);
            this.textBox_Gain.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(24, 136);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "增益值:";
            // 
            // textBox_Exposure
            // 
            this.textBox_Exposure.Location = new System.Drawing.Point(91, 78);
            this.textBox_Exposure.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.textBox_Exposure.Name = "textBox_Exposure";
            this.textBox_Exposure.Size = new System.Drawing.Size(94, 21);
            this.textBox_Exposure.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(24, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "曝光值:";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.BorderColor = System.Drawing.Color.Empty;
            this.btnSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveConfig.GlassEffect = true;
            this.btnSaveConfig.Location = new System.Drawing.Point(26, 180);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Radius_All = 5;
            this.btnSaveConfig.Radius_BottomLeft = 5;
            this.btnSaveConfig.Radius_BottomRight = 5;
            this.btnSaveConfig.Radius_TopLeft = 5;
            this.btnSaveConfig.Radius_TopRight = 5;
            this.btnSaveConfig.Size = new System.Drawing.Size(306, 81);
            this.btnSaveConfig.TabIndex = 28;
            this.btnSaveConfig.Text = "保存配置";
            this.btnSaveConfig.UseVisualStyleBackColor = false;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "相机名称:";
            // 
            // btnSnap
            // 
            this.btnSnap.BorderColor = System.Drawing.Color.Empty;
            this.btnSnap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSnap.GlassEffect = true;
            this.btnSnap.Location = new System.Drawing.Point(350, 176);
            this.btnSnap.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Radius_All = 5;
            this.btnSnap.Radius_BottomLeft = 5;
            this.btnSnap.Radius_BottomRight = 5;
            this.btnSnap.Radius_TopLeft = 5;
            this.btnSnap.Radius_TopRight = 5;
            this.btnSnap.Size = new System.Drawing.Size(108, 81);
            this.btnSnap.TabIndex = 25;
            this.btnSnap.Text = "单张采集";
            this.btnSnap.UseVisualStyleBackColor = false;
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // btnGrab
            // 
            this.btnGrab.BorderColor = System.Drawing.Color.Empty;
            this.btnGrab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGrab.GlassEffect = true;
            this.btnGrab.Location = new System.Drawing.Point(462, 176);
            this.btnGrab.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Radius_All = 5;
            this.btnGrab.Radius_BottomLeft = 5;
            this.btnGrab.Radius_BottomRight = 5;
            this.btnGrab.Radius_TopLeft = 5;
            this.btnGrab.Radius_TopRight = 5;
            this.btnGrab.Size = new System.Drawing.Size(108, 81);
            this.btnGrab.TabIndex = 24;
            this.btnGrab.Text = "连续采集";
            this.btnGrab.UseVisualStyleBackColor = false;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // timer_Grab
            // 
            this.timer_Grab.Tick += new System.EventHandler(this.timer_Grab_Tick);
            // 
            // Obj_CameraSoftUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 281);
            this.Controls.Add(this.btnChgTrigSrc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbGrabStatus);
            this.Controls.Add(this.cmbCameraList);
            this.Controls.Add(this.btnSetGain);
            this.Controls.Add(this.btnSetExposure);
            this.Controls.Add(this.btnGetGain);
            this.Controls.Add(this.btnGetExposure);
            this.Controls.Add(this.textBox_Gain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Exposure);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSnap);
            this.Controls.Add(this.btnGrab);
            this.Name = "Obj_CameraSoftUI";
            this.Text = "Obj_CameraSoft";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Obj_CameraSoftUI_FormClosed);
            this.Load += new System.EventHandler(this.Obj_CameraSoftUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SelfControl.ColorButton btnChgTrigSrc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbGrabStatus;
        private System.Windows.Forms.ComboBox cmbCameraList;
        private SelfControl.ColorButton btnSetGain;
        private SelfControl.ColorButton btnSetExposure;
        private SelfControl.ColorButton btnGetGain;
        private SelfControl.ColorButton btnGetExposure;
        private System.Windows.Forms.TextBox textBox_Gain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Exposure;
        private System.Windows.Forms.Label label2;
        private SelfControl.ColorButton btnSaveConfig;
        private System.Windows.Forms.Label label1;
        private SelfControl.ColorButton btnSnap;
        private SelfControl.ColorButton btnGrab;
        private System.Windows.Forms.Timer timer_Grab;
    }
}