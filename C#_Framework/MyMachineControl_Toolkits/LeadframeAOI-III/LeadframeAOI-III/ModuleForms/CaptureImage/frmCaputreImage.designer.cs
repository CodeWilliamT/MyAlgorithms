namespace LeadframeAOI
{
    partial class frmCaptureImage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCaptureImage));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.mappingControl2 = new HTMappingControl.MappingControl();
            this.mappingControl1 = new HTMappingControl.MappingControl();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.htWindow = new HTHalControl.HTWindowControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ckbWaitCheckPos = new System.Windows.Forms.CheckBox();
            this.ckbUseAlg = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnConfigImagePath = new System.Windows.Forms.Button();
            this.txtImageFolder = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.cbBox_ChannelSelect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBox_ImgSelect = new System.Windows.Forms.ComboBox();
            this.btnOpenImageFolder = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvWorkImagesInfo = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCameraList = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMutiScan = new System.Windows.Forms.Button();
            this.btnSnapClip = new System.Windows.Forms.Button();
            this.btn_Mark = new System.Windows.Forms.Button();
            this.btnEnhanLight = new System.Windows.Forms.Button();
            this.btnScanCode2D = new System.Windows.Forms.Button();
            this.btn_CheckPos = new System.Windows.Forms.Button();
            this.btnCmrAxisTool = new System.Windows.Forms.Button();
            this.btnSnap = new System.Windows.Forms.Button();
            this.btnGrab = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnCaputreOneDieImages = new System.Windows.Forms.Button();
            this.brnAutoFocus = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTool = new System.Windows.Forms.Button();
            this.btnTrainCode2D = new System.Windows.Forms.Button();
            this.btnMoveToCode2D = new System.Windows.Forms.Button();
            this.timer_Grab = new System.Windows.Forms.Timer(this.components);
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.numDelayMax = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkImagesInfo)).BeginInit();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayMax)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1333, 874);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1124, 860);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.78354F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.21646F));
            this.tableLayoutPanel6.Controls.Add(this.mappingControl2, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.mappingControl1, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 648);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1118, 209);
            this.tableLayoutPanel6.TabIndex = 90;
            // 
            // mappingControl2
            // 
            this.mappingControl2.BackColor = System.Drawing.Color.Silver;
            this.mappingControl2.BorderThickness = 2D;
            this.mappingControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mappingControl2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mappingControl2.Location = new System.Drawing.Point(773, 6);
            this.mappingControl2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.mappingControl2.MinCellHeight = 30D;
            this.mappingControl2.MinCellWidth = 30D;
            this.mappingControl2.Name = "mappingControl2";
            this.mappingControl2.Size = new System.Drawing.Size(340, 197);
            this.mappingControl2.TabIndex = 12;
            // 
            // mappingControl1
            // 
            this.mappingControl1.BackColor = System.Drawing.Color.Silver;
            this.mappingControl1.BorderThickness = 2D;
            this.mappingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mappingControl1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mappingControl1.Location = new System.Drawing.Point(4, 4);
            this.mappingControl1.Margin = new System.Windows.Forms.Padding(4);
            this.mappingControl1.MinCellHeight = 30D;
            this.mappingControl1.MinCellWidth = 30D;
            this.mappingControl1.Name = "mappingControl1";
            this.mappingControl1.Size = new System.Drawing.Size(760, 201);
            this.mappingControl1.TabIndex = 11;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel7.Controls.Add(this.htWindow, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1116, 637);
            this.tableLayoutPanel7.TabIndex = 0;
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
            this.htWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htWindow.Image = null;
            this.htWindow.Length1 = null;
            this.htWindow.Length2 = null;
            this.htWindow.Location = new System.Drawing.Point(4, 4);
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
            this.htWindow.Size = new System.Drawing.Size(661, 629);
            this.htWindow.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(673, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 212F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(439, 629);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel5.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.panel4, 0, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 417);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(439, 212);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnDel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 44);
            this.panel1.TabIndex = 90;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(323, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 17);
            this.label3.TabIndex = 85;
            this.label3.Text = "当前检测线程数:0";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(31, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 33);
            this.btnAdd.TabIndex = 80;
            this.btnAdd.Text = "增加拍照";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnDel.FlatAppearance.BorderSize = 0;
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            this.btnDel.Location = new System.Drawing.Point(160, 5);
            this.btnDel.Margin = new System.Windows.Forms.Padding(1);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 33);
            this.btnDel.TabIndex = 79;
            this.btnDel.Text = "删除拍照";
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numDelayMax);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.ckbWaitCheckPos);
            this.panel2.Controls.Add(this.ckbUseAlg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 44);
            this.panel2.TabIndex = 91;
            // 
            // ckbWaitCheckPos
            // 
            this.ckbWaitCheckPos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ckbWaitCheckPos.AutoSize = true;
            this.ckbWaitCheckPos.Location = new System.Drawing.Point(314, 10);
            this.ckbWaitCheckPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ckbWaitCheckPos.Name = "ckbWaitCheckPos";
            this.ckbWaitCheckPos.Size = new System.Drawing.Size(111, 21);
            this.ckbWaitCheckPos.TabIndex = 89;
            this.ckbWaitCheckPos.Text = "启用矫正点停留";
            this.ckbWaitCheckPos.UseVisualStyleBackColor = true;
            // 
            // ckbUseAlg
            // 
            this.ckbUseAlg.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ckbUseAlg.AutoSize = true;
            this.ckbUseAlg.Checked = true;
            this.ckbUseAlg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbUseAlg.Location = new System.Drawing.Point(219, 10);
            this.ckbUseAlg.Margin = new System.Windows.Forms.Padding(4);
            this.ckbUseAlg.Name = "ckbUseAlg";
            this.ckbUseAlg.Size = new System.Drawing.Size(75, 21);
            this.ckbUseAlg.TabIndex = 84;
            this.ckbUseAlg.Text = "启用算法";
            this.ckbUseAlg.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.btnConfigImagePath);
            this.panel3.Controls.Add(this.txtImageFolder);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(5, 109);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(429, 44);
            this.panel3.TabIndex = 92;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 60;
            this.label6.Text = "图像目录：";
            // 
            // btnConfigImagePath
            // 
            this.btnConfigImagePath.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnConfigImagePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnConfigImagePath.FlatAppearance.BorderSize = 0;
            this.btnConfigImagePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigImagePath.ForeColor = System.Drawing.Color.White;
            this.btnConfigImagePath.Location = new System.Drawing.Point(338, 10);
            this.btnConfigImagePath.Margin = new System.Windows.Forms.Padding(1);
            this.btnConfigImagePath.Name = "btnConfigImagePath";
            this.btnConfigImagePath.Size = new System.Drawing.Size(78, 25);
            this.btnConfigImagePath.TabIndex = 61;
            this.btnConfigImagePath.Text = "浏览...";
            this.btnConfigImagePath.UseVisualStyleBackColor = false;
            this.btnConfigImagePath.Click += new System.EventHandler(this.btnConfigImagePath_Click);
            // 
            // txtImageFolder
            // 
            this.txtImageFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtImageFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txtImageFolder.Location = new System.Drawing.Point(89, 10);
            this.txtImageFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtImageFolder.Name = "txtImageFolder";
            this.txtImageFolder.Size = new System.Drawing.Size(242, 23);
            this.txtImageFolder.TabIndex = 59;
            this.txtImageFolder.WordWrap = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel9);
            this.panel4.Controls.Add(this.tableLayoutPanel8);
            this.panel4.Controls.Add(this.btnOpenImageFolder);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 161);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(429, 46);
            this.panel4.TabIndex = 93;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.10145F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.89855F));
            this.tableLayoutPanel9.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.cbBox_ChannelSelect, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(280, 2);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(141, 43);
            this.tableLayoutPanel9.TabIndex = 88;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 17);
            this.label5.TabIndex = 95;
            this.label5.Text = "图片通道:";
            // 
            // cbBox_ChannelSelect
            // 
            this.cbBox_ChannelSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbBox_ChannelSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox_ChannelSelect.FormattingEnabled = true;
            this.cbBox_ChannelSelect.Location = new System.Drawing.Point(70, 4);
            this.cbBox_ChannelSelect.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_ChannelSelect.Name = "cbBox_ChannelSelect";
            this.cbBox_ChannelSelect.Size = new System.Drawing.Size(67, 25);
            this.cbBox_ChannelSelect.TabIndex = 96;
            this.cbBox_ChannelSelect.SelectedIndexChanged += new System.EventHandler(this.cbBox_ChannelSelect_SelectedIndexChanged);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.47887F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.52113F));
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.cbBox_ImgSelect, 1, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(109, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(156, 40);
            this.tableLayoutPanel8.TabIndex = 87;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 92;
            this.label4.Text = "图片索引:";
            // 
            // cbBox_ImgSelect
            // 
            this.cbBox_ImgSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbBox_ImgSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox_ImgSelect.FormattingEnabled = true;
            this.cbBox_ImgSelect.Location = new System.Drawing.Point(76, 4);
            this.cbBox_ImgSelect.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_ImgSelect.Name = "cbBox_ImgSelect";
            this.cbBox_ImgSelect.Size = new System.Drawing.Size(76, 25);
            this.cbBox_ImgSelect.TabIndex = 93;
            this.cbBox_ImgSelect.SelectedIndexChanged += new System.EventHandler(this.cbBox_ImgSelect_SelectedIndexChanged);
            // 
            // btnOpenImageFolder
            // 
            this.btnOpenImageFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOpenImageFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnOpenImageFolder.FlatAppearance.BorderSize = 0;
            this.btnOpenImageFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenImageFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenImageFolder.Location = new System.Drawing.Point(7, 7);
            this.btnOpenImageFolder.Margin = new System.Windows.Forms.Padding(1);
            this.btnOpenImageFolder.Name = "btnOpenImageFolder";
            this.btnOpenImageFolder.Size = new System.Drawing.Size(97, 27);
            this.btnOpenImageFolder.TabIndex = 62;
            this.btnOpenImageFolder.Text = "打开存储目录";
            this.btnOpenImageFolder.UseVisualStyleBackColor = false;
            this.btnOpenImageFolder.Click += new System.EventHandler(this.btnOpenImageFolder_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(431, 359);
            this.tabControl1.TabIndex = 84;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabPage1.Controls.Add(this.dgvWorkImagesInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(423, 329);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "拍照配置";
            // 
            // dgvWorkImagesInfo
            // 
            this.dgvWorkImagesInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWorkImagesInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvWorkImagesInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkImagesInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkImagesInfo.Location = new System.Drawing.Point(4, 4);
            this.dgvWorkImagesInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvWorkImagesInfo.Name = "dgvWorkImagesInfo";
            this.dgvWorkImagesInfo.RowHeadersVisible = false;
            this.dgvWorkImagesInfo.RowTemplate.Height = 23;
            this.dgvWorkImagesInfo.Size = new System.Drawing.Size(415, 321);
            this.dgvWorkImagesInfo.TabIndex = 0;
            this.dgvWorkImagesInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkImagesInfo_CellContent_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.cmbCameraList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(4, 371);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(431, 42);
            this.panel5.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 43;
            this.label1.Text = "相机名称:";
            // 
            // cmbCameraList
            // 
            this.cmbCameraList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbCameraList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraList.FormattingEnabled = true;
            this.cmbCameraList.Location = new System.Drawing.Point(118, 8);
            this.cmbCameraList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbCameraList.Name = "cmbCameraList";
            this.cmbCameraList.Size = new System.Drawing.Size(123, 25);
            this.cmbCameraList.TabIndex = 53;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btnMutiScan, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.btnSnapClip, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btn_Mark, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.btnEnhanLight, 0, 12);
            this.tableLayoutPanel4.Controls.Add(this.btnScanCode2D, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.btn_CheckPos, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnCmrAxisTool, 0, 13);
            this.tableLayoutPanel4.Controls.Add(this.btnSnap, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.btnGrab, 0, 10);
            this.tableLayoutPanel4.Controls.Add(this.btnScan, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.btnCaputreOneDieImages, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.brnAutoFocus, 0, 11);
            this.tableLayoutPanel4.Controls.Add(this.btnSave, 0, 15);
            this.tableLayoutPanel4.Controls.Add(this.btnTool, 0, 14);
            this.tableLayoutPanel4.Controls.Add(this.btnTrainCode2D, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.btnMoveToCode2D, 0, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1142, 7);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 16;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(184, 860);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // btnMutiScan
            // 
            this.btnMutiScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnMutiScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMutiScan.Enabled = false;
            this.btnMutiScan.FlatAppearance.BorderSize = 0;
            this.btnMutiScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMutiScan.ForeColor = System.Drawing.Color.White;
            this.btnMutiScan.Location = new System.Drawing.Point(1, 372);
            this.btnMutiScan.Margin = new System.Windows.Forms.Padding(1);
            this.btnMutiScan.Name = "btnMutiScan";
            this.btnMutiScan.Size = new System.Drawing.Size(182, 51);
            this.btnMutiScan.TabIndex = 88;
            this.btnMutiScan.Text = "连续扫描";
            this.btnMutiScan.UseVisualStyleBackColor = false;
            this.btnMutiScan.Click += new System.EventHandler(this.btnMutiScan_Click);
            // 
            // btnSnapClip
            // 
            this.btnSnapClip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnSnapClip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSnapClip.FlatAppearance.BorderSize = 0;
            this.btnSnapClip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapClip.ForeColor = System.Drawing.Color.White;
            this.btnSnapClip.Location = new System.Drawing.Point(1, 107);
            this.btnSnapClip.Margin = new System.Windows.Forms.Padding(1);
            this.btnSnapClip.Name = "btnSnapClip";
            this.btnSnapClip.Size = new System.Drawing.Size(182, 51);
            this.btnSnapClip.TabIndex = 54;
            this.btnSnapClip.Text = "移至选中芯片拍摄";
            this.btnSnapClip.UseVisualStyleBackColor = false;
            this.btnSnapClip.Click += new System.EventHandler(this.btnSnapClip_Click);
            // 
            // btn_Mark
            // 
            this.btn_Mark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btn_Mark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Mark.FlatAppearance.BorderSize = 0;
            this.btn_Mark.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Mark.ForeColor = System.Drawing.Color.White;
            this.btn_Mark.Location = new System.Drawing.Point(1, 425);
            this.btn_Mark.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Mark.Name = "btn_Mark";
            this.btn_Mark.Size = new System.Drawing.Size(182, 51);
            this.btn_Mark.TabIndex = 54;
            this.btn_Mark.Text = "标记芯片";
            this.btn_Mark.UseVisualStyleBackColor = false;
            this.btn_Mark.Click += new System.EventHandler(this.btn_Mark_Click);
            // 
            // btnEnhanLight
            // 
            this.btnEnhanLight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnEnhanLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnhanLight.FlatAppearance.BorderSize = 0;
            this.btnEnhanLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnhanLight.ForeColor = System.Drawing.Color.White;
            this.btnEnhanLight.Location = new System.Drawing.Point(1, 637);
            this.btnEnhanLight.Margin = new System.Windows.Forms.Padding(1);
            this.btnEnhanLight.Name = "btnEnhanLight";
            this.btnEnhanLight.Size = new System.Drawing.Size(182, 51);
            this.btnEnhanLight.TabIndex = 87;
            this.btnEnhanLight.Text = "配置增亮光源";
            this.btnEnhanLight.UseVisualStyleBackColor = false;
            this.btnEnhanLight.Click += new System.EventHandler(this.btnEnhanLight_Click);
            // 
            // btnScanCode2D
            // 
            this.btnScanCode2D.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnScanCode2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnScanCode2D.FlatAppearance.BorderSize = 0;
            this.btnScanCode2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanCode2D.ForeColor = System.Drawing.Color.White;
            this.btnScanCode2D.Location = new System.Drawing.Point(1, 266);
            this.btnScanCode2D.Margin = new System.Windows.Forms.Padding(1);
            this.btnScanCode2D.Name = "btnScanCode2D";
            this.btnScanCode2D.Size = new System.Drawing.Size(182, 51);
            this.btnScanCode2D.TabIndex = 86;
            this.btnScanCode2D.Text = "二维码识别";
            this.btnScanCode2D.UseVisualStyleBackColor = false;
            this.btnScanCode2D.Click += new System.EventHandler(this.btnScanCode2D_Click);
            // 
            // btn_CheckPos
            // 
            this.btn_CheckPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btn_CheckPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CheckPos.FlatAppearance.BorderSize = 0;
            this.btn_CheckPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CheckPos.ForeColor = System.Drawing.Color.White;
            this.btn_CheckPos.Location = new System.Drawing.Point(1, 1);
            this.btn_CheckPos.Margin = new System.Windows.Forms.Padding(1);
            this.btn_CheckPos.Name = "btn_CheckPos";
            this.btn_CheckPos.Size = new System.Drawing.Size(182, 51);
            this.btn_CheckPos.TabIndex = 80;
            this.btn_CheckPos.Text = "识别矫正点";
            this.btn_CheckPos.UseVisualStyleBackColor = false;
            this.btn_CheckPos.Click += new System.EventHandler(this.btn_CheckPos_Click);
            // 
            // btnCmrAxisTool
            // 
            this.btnCmrAxisTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnCmrAxisTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCmrAxisTool.FlatAppearance.BorderSize = 0;
            this.btnCmrAxisTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCmrAxisTool.ForeColor = System.Drawing.Color.White;
            this.btnCmrAxisTool.Location = new System.Drawing.Point(1, 690);
            this.btnCmrAxisTool.Margin = new System.Windows.Forms.Padding(1);
            this.btnCmrAxisTool.Name = "btnCmrAxisTool";
            this.btnCmrAxisTool.Size = new System.Drawing.Size(182, 51);
            this.btnCmrAxisTool.TabIndex = 83;
            this.btnCmrAxisTool.Text = "相机运动助手";
            this.btnCmrAxisTool.UseVisualStyleBackColor = false;
            this.btnCmrAxisTool.Click += new System.EventHandler(this.btnCmrAxisTool_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnSnap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSnap.FlatAppearance.BorderSize = 0;
            this.btnSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnap.ForeColor = System.Drawing.Color.White;
            this.btnSnap.Location = new System.Drawing.Point(1, 478);
            this.btnSnap.Margin = new System.Windows.Forms.Padding(1);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(182, 51);
            this.btnSnap.TabIndex = 5;
            this.btnSnap.Text = "单次拍照";
            this.btnSnap.UseVisualStyleBackColor = false;
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // btnGrab
            // 
            this.btnGrab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnGrab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGrab.FlatAppearance.BorderSize = 0;
            this.btnGrab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrab.ForeColor = System.Drawing.Color.White;
            this.btnGrab.Location = new System.Drawing.Point(1, 531);
            this.btnGrab.Margin = new System.Windows.Forms.Padding(1);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(182, 51);
            this.btnGrab.TabIndex = 4;
            this.btnGrab.Text = "连续拍照";
            this.btnGrab.UseVisualStyleBackColor = false;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnScan.FlatAppearance.BorderSize = 0;
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.ForeColor = System.Drawing.Color.White;
            this.btnScan.Location = new System.Drawing.Point(1, 319);
            this.btnScan.Margin = new System.Windows.Forms.Padding(1);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(182, 51);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "扫描料片";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnCaputreOneDieImages
            // 
            this.btnCaputreOneDieImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnCaputreOneDieImages.FlatAppearance.BorderSize = 0;
            this.btnCaputreOneDieImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaputreOneDieImages.ForeColor = System.Drawing.Color.White;
            this.btnCaputreOneDieImages.Location = new System.Drawing.Point(1, 54);
            this.btnCaputreOneDieImages.Margin = new System.Windows.Forms.Padding(1);
            this.btnCaputreOneDieImages.Name = "btnCaputreOneDieImages";
            this.btnCaputreOneDieImages.Size = new System.Drawing.Size(182, 51);
            this.btnCaputreOneDieImages.TabIndex = 2;
            this.btnCaputreOneDieImages.Text = "移至选中视野拍摄";
            this.btnCaputreOneDieImages.UseVisualStyleBackColor = false;
            this.btnCaputreOneDieImages.Click += new System.EventHandler(this.btnCaputreOneDieImages_Click);
            // 
            // brnAutoFocus
            // 
            this.brnAutoFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.brnAutoFocus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brnAutoFocus.FlatAppearance.BorderSize = 0;
            this.brnAutoFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brnAutoFocus.ForeColor = System.Drawing.Color.White;
            this.brnAutoFocus.Location = new System.Drawing.Point(1, 584);
            this.brnAutoFocus.Margin = new System.Windows.Forms.Padding(1);
            this.brnAutoFocus.Name = "brnAutoFocus";
            this.brnAutoFocus.Size = new System.Drawing.Size(182, 51);
            this.brnAutoFocus.TabIndex = 34;
            this.brnAutoFocus.Text = "Z轴线扫";
            this.brnAutoFocus.UseVisualStyleBackColor = false;
            this.brnAutoFocus.Click += new System.EventHandler(this.brnAutoFocus_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(3, 799);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(178, 57);
            this.btnSave.TabIndex = 32;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTool
            // 
            this.btnTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTool.FlatAppearance.BorderSize = 0;
            this.btnTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTool.ForeColor = System.Drawing.Color.White;
            this.btnTool.Location = new System.Drawing.Point(1, 743);
            this.btnTool.Margin = new System.Windows.Forms.Padding(1);
            this.btnTool.Name = "btnTool";
            this.btnTool.Size = new System.Drawing.Size(182, 51);
            this.btnTool.TabIndex = 77;
            this.btnTool.Text = "打开轴调试助手";
            this.btnTool.UseVisualStyleBackColor = false;
            this.btnTool.Click += new System.EventHandler(this.btnTool_Click);
            // 
            // btnTrainCode2D
            // 
            this.btnTrainCode2D.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnTrainCode2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTrainCode2D.FlatAppearance.BorderSize = 0;
            this.btnTrainCode2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrainCode2D.ForeColor = System.Drawing.Color.White;
            this.btnTrainCode2D.Location = new System.Drawing.Point(1, 213);
            this.btnTrainCode2D.Margin = new System.Windows.Forms.Padding(1);
            this.btnTrainCode2D.Name = "btnTrainCode2D";
            this.btnTrainCode2D.Size = new System.Drawing.Size(182, 51);
            this.btnTrainCode2D.TabIndex = 84;
            this.btnTrainCode2D.Text = "二维码训练";
            this.btnTrainCode2D.UseVisualStyleBackColor = false;
            this.btnTrainCode2D.Click += new System.EventHandler(this.btnTrainCode2D_Click);
            // 
            // btnMoveToCode2D
            // 
            this.btnMoveToCode2D.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnMoveToCode2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMoveToCode2D.FlatAppearance.BorderSize = 0;
            this.btnMoveToCode2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveToCode2D.ForeColor = System.Drawing.Color.White;
            this.btnMoveToCode2D.Location = new System.Drawing.Point(1, 160);
            this.btnMoveToCode2D.Margin = new System.Windows.Forms.Padding(1);
            this.btnMoveToCode2D.Name = "btnMoveToCode2D";
            this.btnMoveToCode2D.Size = new System.Drawing.Size(182, 51);
            this.btnMoveToCode2D.TabIndex = 85;
            this.btnMoveToCode2D.Text = "拍摄二维码位置";
            this.btnMoveToCode2D.UseVisualStyleBackColor = false;
            this.btnMoveToCode2D.Click += new System.EventHandler(this.btnMoveToCode2D_Click);
            // 
            // timer_Grab
            // 
            this.timer_Grab.Tick += new System.EventHandler(this.timer_Grab_Tick);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 17);
            this.label2.TabIndex = 91;
            this.label2.Text = "存储最长等待时间";
            // 
            // numDelayMax
            // 
            this.numDelayMax.Location = new System.Drawing.Point(118, 5);
            this.numDelayMax.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numDelayMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numDelayMax.Name = "numDelayMax";
            this.numDelayMax.Size = new System.Drawing.Size(82, 23);
            this.numDelayMax.TabIndex = 92;
            // 
            // frmCaptureImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 874);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCaptureImage";
            this.Text = "frmCaptureImage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkImagesInfo)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayMax)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnCaputreOneDieImages;
        private System.Windows.Forms.Button btnSnap;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Timer timer_Grab;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfigImagePath;
        private System.Windows.Forms.ComboBox cmbCameraList;
        private System.Windows.Forms.TextBox txtImageFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Panel panel5;
        public HTHalControl.HTWindowControl htWindow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public HTMappingControl.MappingControl mappingControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.DataGridView dgvWorkImagesInfo;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.CheckBox ckbUseAlg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_CheckPos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckbWaitCheckPos;
        private System.Windows.Forms.Button btnTrainCode2D;
        private System.Windows.Forms.Button btnMoveToCode2D;
        private System.Windows.Forms.Button btnScanCode2D;
        private System.Windows.Forms.Button btnCmrAxisTool;
        private System.Windows.Forms.Button brnAutoFocus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTool;
        private System.Windows.Forms.Button btnOpenImageFolder;
        private System.Windows.Forms.ComboBox cbBox_ImgSelect;
        private System.Windows.Forms.ComboBox cbBox_ChannelSelect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnEnhanLight;
        private System.Windows.Forms.Button btn_Mark;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        public HTMappingControl.MappingControl mappingControl2;
        private System.Windows.Forms.Button btnSnapClip;
        private System.Windows.Forms.Button btnMutiScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numDelayMax;
    }
}