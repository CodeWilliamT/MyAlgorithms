namespace ToolKits.Map
{
    partial class MapCanvas
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapCanvas));
            this.gbMapOps = new System.Windows.Forms.GroupBox();
            this.btnMirror = new SelfControl.VaryButton();
            this.btnRotateMap = new SelfControl.VaryButton();
            this.cmbMirMode = new SelfControl.ColorComboBox();
            this.cmbRotateMode = new SelfControl.ColorComboBox();
            this.btnLoadMap = new SelfControl.VaryButton();
            this.btnMapSave = new SelfControl.VaryButton();
            this.gbDieOps = new System.Windows.Forms.GroupBox();
            this.txtBin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.txtCol = new System.Windows.Forms.TextBox();
            this.binRing = new SelfControl.ColorComboBox();
            this.gbBinSet = new System.Windows.Forms.GroupBox();
            this.btnOpenBinInfoFrm = new SelfControl.VaryButton();
            this.btnExSetLeftBin = new SelfControl.VaryButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MainCvs = new System.Windows.Forms.PictureBox();
            this.gbRefDieSet = new System.Windows.Forms.GroupBox();
            this.btnSetRefDie = new SelfControl.VaryButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbNotchDir = new System.Windows.Forms.GroupBox();
            this.pbNotchDir = new System.Windows.Forms.PictureBox();
            this.AuxCvs = new System.Windows.Forms.PictureBox();
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.gbMapOps.SuspendLayout();
            this.gbDieOps.SuspendLayout();
            this.gbBinSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainCvs)).BeginInit();
            this.gbRefDieSet.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbNotchDir.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotchDir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuxCvs)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMapOps
            // 
            this.gbMapOps.Controls.Add(this.btnMirror);
            this.gbMapOps.Controls.Add(this.btnRotateMap);
            this.gbMapOps.Controls.Add(this.cmbMirMode);
            this.gbMapOps.Controls.Add(this.cmbRotateMode);
            this.gbMapOps.Controls.Add(this.btnLoadMap);
            this.gbMapOps.Controls.Add(this.btnMapSave);
            this.gbMapOps.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbMapOps.Location = new System.Drawing.Point(0, 487);
            this.gbMapOps.Name = "gbMapOps";
            this.gbMapOps.Size = new System.Drawing.Size(232, 160);
            this.gbMapOps.TabIndex = 11;
            this.gbMapOps.TabStop = false;
            this.gbMapOps.Text = "Map操作";
            // 
            // btnMirror
            // 
            this.btnMirror.BackColor = System.Drawing.Color.Transparent;
            this.btnMirror.DownImage = ((System.Drawing.Image)(resources.GetObject("btnMirror.DownImage")));
            this.btnMirror.Image = null;
            this.btnMirror.IsShowBorder = true;
            this.btnMirror.Location = new System.Drawing.Point(119, 58);
            this.btnMirror.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnMirror.MoveImage")));
            this.btnMirror.Name = "btnMirror";
            this.btnMirror.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnMirror.NormalImage")));
            this.btnMirror.Size = new System.Drawing.Size(110, 45);
            this.btnMirror.TabIndex = 6;
            this.btnMirror.Text = "镜像";
            this.btnMirror.UseVisualStyleBackColor = false;
            this.btnMirror.Click += new System.EventHandler(this.btnMirror_Click);
            // 
            // btnRotateMap
            // 
            this.btnRotateMap.BackColor = System.Drawing.Color.Transparent;
            this.btnRotateMap.DownImage = ((System.Drawing.Image)(resources.GetObject("btnRotateMap.DownImage")));
            this.btnRotateMap.Image = null;
            this.btnRotateMap.IsShowBorder = true;
            this.btnRotateMap.Location = new System.Drawing.Point(119, 9);
            this.btnRotateMap.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnRotateMap.MoveImage")));
            this.btnRotateMap.Name = "btnRotateMap";
            this.btnRotateMap.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnRotateMap.NormalImage")));
            this.btnRotateMap.Size = new System.Drawing.Size(110, 46);
            this.btnRotateMap.TabIndex = 6;
            this.btnRotateMap.Text = "旋转";
            this.btnRotateMap.UseVisualStyleBackColor = false;
            this.btnRotateMap.Click += new System.EventHandler(this.btnRotateMap_Click);
            // 
            // cmbMirMode
            // 
            this.cmbMirMode.ArrowColor = System.Drawing.Color.White;
            this.cmbMirMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.cmbMirMode.BaseColor = System.Drawing.Color.Black;
            this.cmbMirMode.BorderColor = System.Drawing.Color.Black;
            this.cmbMirMode.FormattingEnabled = true;
            this.cmbMirMode.Location = new System.Drawing.Point(9, 70);
            this.cmbMirMode.Name = "cmbMirMode";
            this.cmbMirMode.Size = new System.Drawing.Size(90, 22);
            this.cmbMirMode.TabIndex = 12;
            this.cmbMirMode.SelectedIndexChanged += new System.EventHandler(this.cmbMirMode_SelectedIndexChanged);
            // 
            // cmbRotateMode
            // 
            this.cmbRotateMode.ArrowColor = System.Drawing.Color.White;
            this.cmbRotateMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.cmbRotateMode.BaseColor = System.Drawing.Color.Black;
            this.cmbRotateMode.BorderColor = System.Drawing.Color.Black;
            this.cmbRotateMode.FormattingEnabled = true;
            this.cmbRotateMode.Location = new System.Drawing.Point(9, 21);
            this.cmbRotateMode.Name = "cmbRotateMode";
            this.cmbRotateMode.Size = new System.Drawing.Size(90, 22);
            this.cmbRotateMode.TabIndex = 12;
            this.cmbRotateMode.SelectedIndexChanged += new System.EventHandler(this.cbRotateMode_SelectedIndexChanged);
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadMap.DownImage = ((System.Drawing.Image)(resources.GetObject("btnLoadMap.DownImage")));
            this.btnLoadMap.Image = null;
            this.btnLoadMap.IsShowBorder = true;
            this.btnLoadMap.Location = new System.Drawing.Point(3, 112);
            this.btnLoadMap.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnLoadMap.MoveImage")));
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnLoadMap.NormalImage")));
            this.btnLoadMap.Size = new System.Drawing.Size(110, 44);
            this.btnLoadMap.TabIndex = 11;
            this.btnLoadMap.Text = "载入Map";
            this.btnLoadMap.UseVisualStyleBackColor = false;
            this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);
            // 
            // btnMapSave
            // 
            this.btnMapSave.BackColor = System.Drawing.Color.Transparent;
            this.btnMapSave.DownImage = ((System.Drawing.Image)(resources.GetObject("btnMapSave.DownImage")));
            this.btnMapSave.Image = null;
            this.btnMapSave.IsShowBorder = true;
            this.btnMapSave.Location = new System.Drawing.Point(119, 112);
            this.btnMapSave.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnMapSave.MoveImage")));
            this.btnMapSave.Name = "btnMapSave";
            this.btnMapSave.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnMapSave.NormalImage")));
            this.btnMapSave.Size = new System.Drawing.Size(110, 44);
            this.btnMapSave.TabIndex = 11;
            this.btnMapSave.Text = "Map保存";
            this.btnMapSave.UseVisualStyleBackColor = false;
            this.btnMapSave.Click += new System.EventHandler(this.btnMapSave_Click);
            // 
            // gbDieOps
            // 
            this.gbDieOps.Controls.Add(this.txtBin);
            this.gbDieOps.Controls.Add(this.label4);
            this.gbDieOps.Controls.Add(this.label3);
            this.gbDieOps.Controls.Add(this.label1);
            this.gbDieOps.Controls.Add(this.label2);
            this.gbDieOps.Controls.Add(this.txtRow);
            this.gbDieOps.Controls.Add(this.txtCol);
            this.gbDieOps.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbDieOps.Location = new System.Drawing.Point(79, 0);
            this.gbDieOps.Name = "gbDieOps";
            this.gbDieOps.Size = new System.Drawing.Size(151, 79);
            this.gbDieOps.TabIndex = 9;
            this.gbDieOps.TabStop = false;
            this.gbDieOps.Text = "Die操作";
            // 
            // txtBin
            // 
            this.txtBin.BackColor = System.Drawing.SystemColors.Control;
            this.txtBin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBin.Enabled = false;
            this.txtBin.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtBin.ForeColor = System.Drawing.Color.Green;
            this.txtBin.Location = new System.Drawing.Point(75, 58);
            this.txtBin.Name = "txtBin";
            this.txtBin.Size = new System.Drawing.Size(50, 15);
            this.txtBin.TabIndex = 13;
            this.txtBin.Text = "0";
            this.txtBin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9F);
            this.label4.Location = new System.Drawing.Point(12, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 12;
            this.label4.Text = "当前Bin:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9F);
            this.label3.Location = new System.Drawing.Point(126, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 11;
            this.label3.Text = "列";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9F);
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "当前坐标:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9F);
            this.label2.Location = new System.Drawing.Point(126, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 10;
            this.label2.Text = "行";
            // 
            // txtRow
            // 
            this.txtRow.BackColor = System.Drawing.SystemColors.Control;
            this.txtRow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRow.Enabled = false;
            this.txtRow.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtRow.ForeColor = System.Drawing.Color.Green;
            this.txtRow.Location = new System.Drawing.Point(75, 19);
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(50, 15);
            this.txtRow.TabIndex = 8;
            this.txtRow.Text = "0";
            this.txtRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCol
            // 
            this.txtCol.BackColor = System.Drawing.SystemColors.Control;
            this.txtCol.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCol.Enabled = false;
            this.txtCol.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCol.ForeColor = System.Drawing.Color.Green;
            this.txtCol.Location = new System.Drawing.Point(75, 39);
            this.txtCol.Name = "txtCol";
            this.txtCol.Size = new System.Drawing.Size(50, 15);
            this.txtCol.TabIndex = 9;
            this.txtCol.Text = "0";
            this.txtCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // binRing
            // 
            this.binRing.ArrowColor = System.Drawing.Color.White;
            this.binRing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.binRing.BaseColor = System.Drawing.Color.Black;
            this.binRing.BorderColor = System.Drawing.Color.Black;
            this.binRing.FormattingEnabled = true;
            this.binRing.Location = new System.Drawing.Point(9, 31);
            this.binRing.Name = "binRing";
            this.binRing.Size = new System.Drawing.Size(90, 22);
            this.binRing.TabIndex = 5;
            this.binRing.SelectedIndexChanged += new System.EventHandler(this.binRing_SelectedIndexChanged);
            // 
            // gbBinSet
            // 
            this.gbBinSet.Controls.Add(this.btnOpenBinInfoFrm);
            this.gbBinSet.Controls.Add(this.binRing);
            this.gbBinSet.Controls.Add(this.btnExSetLeftBin);
            this.gbBinSet.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbBinSet.Location = new System.Drawing.Point(0, 303);
            this.gbBinSet.Name = "gbBinSet";
            this.gbBinSet.Size = new System.Drawing.Size(232, 114);
            this.gbBinSet.TabIndex = 5;
            this.gbBinSet.TabStop = false;
            this.gbBinSet.Text = "Bin设置";
            // 
            // btnOpenBinInfoFrm
            // 
            this.btnOpenBinInfoFrm.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenBinInfoFrm.DownImage = ((System.Drawing.Image)(resources.GetObject("btnOpenBinInfoFrm.DownImage")));
            this.btnOpenBinInfoFrm.Image = null;
            this.btnOpenBinInfoFrm.IsShowBorder = true;
            this.btnOpenBinInfoFrm.Location = new System.Drawing.Point(116, 64);
            this.btnOpenBinInfoFrm.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnOpenBinInfoFrm.MoveImage")));
            this.btnOpenBinInfoFrm.Name = "btnOpenBinInfoFrm";
            this.btnOpenBinInfoFrm.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnOpenBinInfoFrm.NormalImage")));
            this.btnOpenBinInfoFrm.Size = new System.Drawing.Size(113, 44);
            this.btnOpenBinInfoFrm.TabIndex = 7;
            this.btnOpenBinInfoFrm.Text = "Bin等级表";
            this.btnOpenBinInfoFrm.UseVisualStyleBackColor = false;
            this.btnOpenBinInfoFrm.Click += new System.EventHandler(this.btnOpenBinInfoFrm_Click);
            // 
            // btnExSetLeftBin
            // 
            this.btnExSetLeftBin.BackColor = System.Drawing.Color.Transparent;
            this.btnExSetLeftBin.DownImage = ((System.Drawing.Image)(resources.GetObject("btnExSetLeftBin.DownImage")));
            this.btnExSetLeftBin.Image = null;
            this.btnExSetLeftBin.IsShowBorder = true;
            this.btnExSetLeftBin.Location = new System.Drawing.Point(116, 19);
            this.btnExSetLeftBin.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnExSetLeftBin.MoveImage")));
            this.btnExSetLeftBin.Name = "btnExSetLeftBin";
            this.btnExSetLeftBin.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnExSetLeftBin.NormalImage")));
            this.btnExSetLeftBin.Size = new System.Drawing.Size(113, 44);
            this.btnExSetLeftBin.TabIndex = 6;
            this.btnExSetLeftBin.Text = "设为左侧Bin";
            this.btnExSetLeftBin.UseVisualStyleBackColor = false;
            this.btnExSetLeftBin.Click += new System.EventHandler(this.btnExSetLeftBin_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MainCvs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbMapOps);
            this.splitContainer1.Panel2.Controls.Add(this.gbRefDieSet);
            this.splitContainer1.Panel2.Controls.Add(this.gbBinSet);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.AuxCvs);
            this.splitContainer1.Size = new System.Drawing.Size(1010, 715);
            this.splitContainer1.SplitterDistance = 774;
            this.splitContainer1.TabIndex = 2;
            // 
            // MainCvs
            // 
            this.MainCvs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainCvs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainCvs.Location = new System.Drawing.Point(0, 0);
            this.MainCvs.Margin = new System.Windows.Forms.Padding(0);
            this.MainCvs.Name = "MainCvs";
            this.MainCvs.Padding = new System.Windows.Forms.Padding(10);
            this.MainCvs.Size = new System.Drawing.Size(774, 715);
            this.MainCvs.TabIndex = 1;
            this.MainCvs.TabStop = false;
            // 
            // gbRefDieSet
            // 
            this.gbRefDieSet.Controls.Add(this.btnSetRefDie);
            this.gbRefDieSet.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbRefDieSet.Location = new System.Drawing.Point(0, 417);
            this.gbRefDieSet.Name = "gbRefDieSet";
            this.gbRefDieSet.Size = new System.Drawing.Size(232, 70);
            this.gbRefDieSet.TabIndex = 7;
            this.gbRefDieSet.TabStop = false;
            this.gbRefDieSet.Text = "Ref Die设置";
            // 
            // btnSetRefDie
            // 
            this.btnSetRefDie.BackColor = System.Drawing.Color.Transparent;
            this.btnSetRefDie.DownImage = ((System.Drawing.Image)(resources.GetObject("btnSetRefDie.DownImage")));
            this.btnSetRefDie.Image = null;
            this.btnSetRefDie.IsShowBorder = true;
            this.btnSetRefDie.Location = new System.Drawing.Point(119, 17);
            this.btnSetRefDie.MoveImage = ((System.Drawing.Image)(resources.GetObject("btnSetRefDie.MoveImage")));
            this.btnSetRefDie.Name = "btnSetRefDie";
            this.btnSetRefDie.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSetRefDie.NormalImage")));
            this.btnSetRefDie.Size = new System.Drawing.Size(110, 44);
            this.btnSetRefDie.TabIndex = 6;
            this.btnSetRefDie.Text = "设为参考点";
            this.btnSetRefDie.UseVisualStyleBackColor = false;
            this.btnSetRefDie.Click += new System.EventHandler(this.btnSetRefDie_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbDieOps);
            this.panel1.Controls.Add(this.gbNotchDir);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 224);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 79);
            this.panel1.TabIndex = 12;
            // 
            // gbNotchDir
            // 
            this.gbNotchDir.Controls.Add(this.pbNotchDir);
            this.gbNotchDir.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbNotchDir.Location = new System.Drawing.Point(0, 0);
            this.gbNotchDir.Name = "gbNotchDir";
            this.gbNotchDir.Size = new System.Drawing.Size(79, 79);
            this.gbNotchDir.TabIndex = 10;
            this.gbNotchDir.TabStop = false;
            this.gbNotchDir.Text = "NOTCH";
            // 
            // pbNotchDir
            // 
            this.pbNotchDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbNotchDir.Image = ((System.Drawing.Image)(resources.GetObject("pbNotchDir.Image")));
            this.pbNotchDir.Location = new System.Drawing.Point(3, 18);
            this.pbNotchDir.Name = "pbNotchDir";
            this.pbNotchDir.Size = new System.Drawing.Size(73, 58);
            this.pbNotchDir.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNotchDir.TabIndex = 0;
            this.pbNotchDir.TabStop = false;
            this.pbNotchDir.MouseHover += new System.EventHandler(this.pbNotchDir_MouseHover);
            // 
            // AuxCvs
            // 
            this.AuxCvs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AuxCvs.Dock = System.Windows.Forms.DockStyle.Top;
            this.AuxCvs.Location = new System.Drawing.Point(0, 0);
            this.AuxCvs.Margin = new System.Windows.Forms.Padding(0);
            this.AuxCvs.Name = "AuxCvs";
            this.AuxCvs.Size = new System.Drawing.Size(232, 224);
            this.AuxCvs.TabIndex = 3;
            this.AuxCvs.TabStop = false;
            // 
            // MapCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MapCanvas";
            this.Size = new System.Drawing.Size(1010, 715);
            this.gbMapOps.ResumeLayout(false);
            this.gbDieOps.ResumeLayout(false);
            this.gbDieOps.PerformLayout();
            this.gbBinSet.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainCvs)).EndInit();
            this.gbRefDieSet.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbNotchDir.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbNotchDir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuxCvs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MainCvs;
        private System.Windows.Forms.GroupBox gbBinSet;
        private System.Windows.Forms.PictureBox AuxCvs;
        private System.Windows.Forms.TextBox txtBin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCol;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbDieOps;
        private SelfControl.VaryButton btnExSetLeftBin;
        private SelfControl.ColorComboBox binRing;
        private System.Windows.Forms.GroupBox gbMapOps;
        private SelfControl.VaryButton btnMapSave;
        private SelfControl.VaryButton btnLoadMap;
        private SelfControl.VaryButton btnMirror;
        private SelfControl.VaryButton btnRotateMap;
        private SelfControl.ColorComboBox cmbRotateMode;
        private SelfControl.ColorComboBox cmbMirMode;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbRefDieSet;
        private SelfControl.VaryButton btnSetRefDie;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbNotchDir;
        private System.Windows.Forms.PictureBox pbNotchDir;
        private System.Windows.Forms.ToolTip tipInfo;
        private SelfControl.VaryButton btnOpenBinInfoFrm;

    }
}
