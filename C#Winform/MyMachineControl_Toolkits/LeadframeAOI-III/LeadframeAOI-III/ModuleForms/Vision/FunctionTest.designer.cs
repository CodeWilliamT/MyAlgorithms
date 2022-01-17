namespace LeadframeAOI
{
    partial class FunctionTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionTest));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnInspect = new System.Windows.Forms.Button();
            this.htWindow = new HTHalControl.HTWindowControl();
            this.tbImagefolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNowRowIdx = new System.Windows.Forms.TextBox();
            this.btInspection2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mappingControl2 = new HTMappingControl.MappingControl();
            this.listBox_Defect = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbBox_ChannelSelect = new System.Windows.Forms.ComboBox();
            this.cbBox_ImgSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tbNowColIdx = new System.Windows.Forms.TextBox();
            this.checkBox_Inspect = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox_Lot = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_Frame = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRefreshMap = new System.Windows.Forms.Button();
            this.btnRefreshList = new System.Windows.Forms.Button();
            this.btnSavePara = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.mappingControl1 = new HTMappingControl.MappingControl();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvInspectionPara = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInspectionPara)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInspect
            // 
            this.btnInspect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnInspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInspect.FlatAppearance.BorderSize = 0;
            this.btnInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnInspect.Location = new System.Drawing.Point(3, 189);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(93, 87);
            this.btnInspect.TabIndex = 0;
            this.btnInspect.Text = "检测";
            this.btnInspect.UseVisualStyleBackColor = false;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
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
            this.htWindow.Location = new System.Drawing.Point(5, 6);
            this.htWindow.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
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
            this.htWindow.Size = new System.Drawing.Size(323, 513);
            this.htWindow.TabIndex = 1;
            // 
            // tbImagefolder
            // 
            this.tbImagefolder.Location = new System.Drawing.Point(72, 15);
            this.tbImagefolder.Name = "tbImagefolder";
            this.tbImagefolder.ReadOnly = true;
            this.tbImagefolder.Size = new System.Drawing.Size(124, 23);
            this.tbImagefolder.TabIndex = 6;
            this.tbImagefolder.Text = "E:\\ImageFolder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "当前检测位:";
            // 
            // tbNowRowIdx
            // 
            this.tbNowRowIdx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbNowRowIdx.Enabled = false;
            this.tbNowRowIdx.Location = new System.Drawing.Point(4, 4);
            this.tbNowRowIdx.Margin = new System.Windows.Forms.Padding(4);
            this.tbNowRowIdx.Name = "tbNowRowIdx";
            this.tbNowRowIdx.ReadOnly = true;
            this.tbNowRowIdx.Size = new System.Drawing.Size(53, 23);
            this.tbNowRowIdx.TabIndex = 9;
            this.tbNowRowIdx.Text = "0";
            // 
            // btInspection2
            // 
            this.btInspection2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btInspection2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btInspection2.FlatAppearance.BorderSize = 0;
            this.btInspection2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInspection2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btInspection2.Location = new System.Drawing.Point(3, 282);
            this.btInspection2.Name = "btInspection2";
            this.btInspection2.Size = new System.Drawing.Size(93, 87);
            this.btInspection2.TabIndex = 12;
            this.btInspection2.Text = "连续检测";
            this.btInspection2.UseVisualStyleBackColor = false;
            this.btInspection2.Click += new System.EventHandler(this.btInspection2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "图像目录:";
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnConfig.FlatAppearance.BorderSize = 0;
            this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfig.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnConfig.Location = new System.Drawing.Point(203, 14);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(57, 26);
            this.btnConfig.TabIndex = 16;
            this.btnConfig.Text = "浏览...";
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 467F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1249, 759);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.splitContainer1, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(679, 4);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(459, 751);
            this.tableLayoutPanel4.TabIndex = 84;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 527);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mappingControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox_Defect);
            this.splitContainer1.Size = new System.Drawing.Size(453, 221);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 96;
            // 
            // mappingControl2
            // 
            this.mappingControl2.BackColor = System.Drawing.Color.Silver;
            this.mappingControl2.BorderThickness = 2D;
            this.mappingControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mappingControl2.Location = new System.Drawing.Point(0, 0);
            this.mappingControl2.MinCellHeight = 30D;
            this.mappingControl2.MinCellWidth = 30D;
            this.mappingControl2.Name = "mappingControl2";
            this.mappingControl2.Size = new System.Drawing.Size(225, 221);
            this.mappingControl2.TabIndex = 2;
            // 
            // listBox_Defect
            // 
            this.listBox_Defect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Defect.FormattingEnabled = true;
            this.listBox_Defect.ItemHeight = 17;
            this.listBox_Defect.Location = new System.Drawing.Point(0, 0);
            this.listBox_Defect.Name = "listBox_Defect";
            this.listBox_Defect.Size = new System.Drawing.Size(224, 221);
            this.listBox_Defect.TabIndex = 0;
            this.listBox_Defect.SelectedIndexChanged += new System.EventHandler(this.listBox_Defect_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbBox_ChannelSelect);
            this.panel1.Controls.Add(this.cbBox_ImgSelect);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tableLayoutPanel6);
            this.panel1.Controls.Add(this.checkBox_Inspect);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnConfig);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbImagefolder);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 266);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 254);
            this.panel1.TabIndex = 0;
            // 
            // cbBox_ChannelSelect
            // 
            this.cbBox_ChannelSelect.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.cbBox_ChannelSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox_ChannelSelect.FormattingEnabled = true;
            this.cbBox_ChannelSelect.Location = new System.Drawing.Point(76, 148);
            this.cbBox_ChannelSelect.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_ChannelSelect.Name = "cbBox_ChannelSelect";
            this.cbBox_ChannelSelect.Size = new System.Drawing.Size(95, 25);
            this.cbBox_ChannelSelect.TabIndex = 95;
            this.cbBox_ChannelSelect.SelectedIndexChanged += new System.EventHandler(this.cbBox_ChannelSelect_SelectedIndexChanged);
            // 
            // cbBox_ImgSelect
            // 
            this.cbBox_ImgSelect.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.cbBox_ImgSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox_ImgSelect.FormattingEnabled = true;
            this.cbBox_ImgSelect.Location = new System.Drawing.Point(76, 105);
            this.cbBox_ImgSelect.Margin = new System.Windows.Forms.Padding(4);
            this.cbBox_ImgSelect.Name = "cbBox_ImgSelect";
            this.cbBox_ImgSelect.Size = new System.Drawing.Size(95, 25);
            this.cbBox_ImgSelect.TabIndex = 94;
            this.cbBox_ImgSelect.SelectedIndexChanged += new System.EventHandler(this.cbBox_ImgSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 92;
            this.label1.Text = "图片通道：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 91;
            this.label4.Text = "图片索引：";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.tbNowRowIdx, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tbNowColIdx, 1, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(78, 50);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(122, 38);
            this.tableLayoutPanel6.TabIndex = 86;
            // 
            // tbNowColIdx
            // 
            this.tbNowColIdx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbNowColIdx.Enabled = false;
            this.tbNowColIdx.Location = new System.Drawing.Point(65, 4);
            this.tbNowColIdx.Margin = new System.Windows.Forms.Padding(4);
            this.tbNowColIdx.Name = "tbNowColIdx";
            this.tbNowColIdx.ReadOnly = true;
            this.tbNowColIdx.Size = new System.Drawing.Size(53, 23);
            this.tbNowColIdx.TabIndex = 84;
            this.tbNowColIdx.Text = "0";
            // 
            // checkBox_Inspect
            // 
            this.checkBox_Inspect.AutoSize = true;
            this.checkBox_Inspect.Location = new System.Drawing.Point(268, 16);
            this.checkBox_Inspect.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_Inspect.Name = "checkBox_Inspect";
            this.checkBox_Inspect.Size = new System.Drawing.Size(87, 21);
            this.checkBox_Inspect.TabIndex = 85;
            this.checkBox_Inspect.Text = "选中即检测";
            this.checkBox_Inspect.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(451, 254);
            this.tableLayoutPanel5.TabIndex = 87;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox_Lot);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(4, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(217, 246);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "批次列表";
            // 
            // listBox_Lot
            // 
            this.listBox_Lot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Lot.FormattingEnabled = true;
            this.listBox_Lot.ItemHeight = 17;
            this.listBox_Lot.Location = new System.Drawing.Point(4, 20);
            this.listBox_Lot.Margin = new System.Windows.Forms.Padding(4);
            this.listBox_Lot.Name = "listBox_Lot";
            this.listBox_Lot.Size = new System.Drawing.Size(209, 222);
            this.listBox_Lot.TabIndex = 85;
            this.listBox_Lot.SelectedIndexChanged += new System.EventHandler(this.listBox_Lot_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_Frame);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(229, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(218, 246);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "料盘列表";
            // 
            // listBox_Frame
            // 
            this.listBox_Frame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Frame.FormattingEnabled = true;
            this.listBox_Frame.ItemHeight = 17;
            this.listBox_Frame.Location = new System.Drawing.Point(4, 20);
            this.listBox_Frame.Margin = new System.Windows.Forms.Padding(4);
            this.listBox_Frame.Name = "listBox_Frame";
            this.listBox_Frame.Size = new System.Drawing.Size(210, 222);
            this.listBox_Frame.TabIndex = 85;
            this.listBox_Frame.SelectedIndexChanged += new System.EventHandler(this.listBox_Frame_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnRefreshMap, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnRefreshList, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSavePara, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.btnInspect, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btInspection2, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1146, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(99, 751);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnRefreshMap
            // 
            this.btnRefreshMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnRefreshMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefreshMap.FlatAppearance.BorderSize = 0;
            this.btnRefreshMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshMap.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnRefreshMap.Location = new System.Drawing.Point(3, 96);
            this.btnRefreshMap.Name = "btnRefreshMap";
            this.btnRefreshMap.Size = new System.Drawing.Size(93, 87);
            this.btnRefreshMap.TabIndex = 86;
            this.btnRefreshMap.Text = "刷新MAP";
            this.btnRefreshMap.UseVisualStyleBackColor = false;
            this.btnRefreshMap.Click += new System.EventHandler(this.btnRefreshMap_Click);
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnRefreshList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefreshList.FlatAppearance.BorderSize = 0;
            this.btnRefreshList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshList.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnRefreshList.Location = new System.Drawing.Point(3, 3);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(93, 87);
            this.btnRefreshList.TabIndex = 85;
            this.btnRefreshList.Text = "刷新列表";
            this.btnRefreshList.UseVisualStyleBackColor = false;
            this.btnRefreshList.Click += new System.EventHandler(this.btnRefreshList_Click);
            // 
            // btnSavePara
            // 
            this.btnSavePara.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSavePara.BackgroundImage")));
            this.btnSavePara.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSavePara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSavePara.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSavePara.Location = new System.Drawing.Point(3, 654);
            this.btnSavePara.Name = "btnSavePara";
            this.btnSavePara.Size = new System.Drawing.Size(93, 94);
            this.btnSavePara.TabIndex = 83;
            this.btnSavePara.UseVisualStyleBackColor = true;
            this.btnSavePara.Click += new System.EventHandler(this.btnSavePara_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.mappingControl1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(667, 751);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // mappingControl1
            // 
            this.mappingControl1.BackColor = System.Drawing.Color.Silver;
            this.mappingControl1.BorderThickness = 2D;
            this.mappingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mappingControl1.Location = new System.Drawing.Point(4, 529);
            this.mappingControl1.Margin = new System.Windows.Forms.Padding(4);
            this.mappingControl1.MinCellHeight = 30D;
            this.mappingControl1.MinCellWidth = 30D;
            this.mappingControl1.Name = "mappingControl1";
            this.mappingControl1.Size = new System.Drawing.Size(659, 218);
            this.mappingControl1.TabIndex = 11;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.dgvInspectionPara, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.htWindow, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(667, 525);
            this.tableLayoutPanel7.TabIndex = 12;
            // 
            // dgvInspectionPara
            // 
            this.dgvInspectionPara.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInspectionPara.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInspectionPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInspectionPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInspectionPara.Location = new System.Drawing.Point(337, 4);
            this.dgvInspectionPara.Margin = new System.Windows.Forms.Padding(4);
            this.dgvInspectionPara.Name = "dgvInspectionPara";
            this.dgvInspectionPara.RowHeadersVisible = false;
            this.dgvInspectionPara.RowTemplate.Height = 23;
            this.dgvInspectionPara.Size = new System.Drawing.Size(326, 517);
            this.dgvInspectionPara.TabIndex = 2;
            // 
            // FunctionTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 759);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FunctionTest";
            this.Text = "算法测试";
            this.Enter += new System.EventHandler(this.FunctionTest_Enter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInspectionPara)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInspect;
        public HTHalControl.HTWindowControl htWindow;
        private System.Windows.Forms.TextBox tbImagefolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNowRowIdx;
        private System.Windows.Forms.Button btInspection2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSavePara;
        private System.Windows.Forms.TextBox tbNowColIdx;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public HTMappingControl.MappingControl mappingControl1;
        private System.Windows.Forms.ListBox listBox_Frame;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRefreshList;
        private System.Windows.Forms.CheckBox checkBox_Inspect;
        private HTMappingControl.MappingControl mappingControl2;
        private System.Windows.Forms.Button btnRefreshMap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox_Lot;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBox_ImgSelect;
        private System.Windows.Forms.ComboBox cbBox_ChannelSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.DataGridView dgvInspectionPara;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox_Defect;
    }
}

