namespace ToolKits.TemplateEdit.MatchTmpEdit
{
    partial class TmpPrmForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbTmpPrm = new System.Windows.Forms.GroupBox();
            this.nudNumLevel = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cbModelType = new ControlWrap.ComboBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.numScore = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numAngleExtent = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numAngleStart = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbCreateTmp = new SelfControl.VaryCheckBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbTmpPrm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngleExtent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngleStart)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(275, 459);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "创建模板";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbTmpPrm, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 16);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(271, 441);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(2, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(267, 66);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作提示";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. 选择是否创建模板\r\n1. 选择创建的模板类型\r\n2. 左击【下一步】按钮";
            // 
            // gbTmpPrm
            // 
            this.gbTmpPrm.Controls.Add(this.nudNumLevel);
            this.gbTmpPrm.Controls.Add(this.label6);
            this.gbTmpPrm.Controls.Add(this.cbModelType);
            this.gbTmpPrm.Controls.Add(this.label4);
            this.gbTmpPrm.Controls.Add(this.numScore);
            this.gbTmpPrm.Controls.Add(this.label3);
            this.gbTmpPrm.Controls.Add(this.numAngleExtent);
            this.gbTmpPrm.Controls.Add(this.label2);
            this.gbTmpPrm.Controls.Add(this.numAngleStart);
            this.gbTmpPrm.Controls.Add(this.label5);
            this.gbTmpPrm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTmpPrm.Location = new System.Drawing.Point(2, 122);
            this.gbTmpPrm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbTmpPrm.Name = "gbTmpPrm";
            this.gbTmpPrm.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbTmpPrm.Size = new System.Drawing.Size(267, 108);
            this.gbTmpPrm.TabIndex = 19;
            this.gbTmpPrm.TabStop = false;
            this.gbTmpPrm.Text = "模板参数";
            // 
            // nudNumLevel
            // 
            this.nudNumLevel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nudNumLevel.BackColor = System.Drawing.Color.White;
            this.nudNumLevel.Location = new System.Drawing.Point(188, 48);
            this.nudNumLevel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudNumLevel.Name = "nudNumLevel";
            this.nudNumLevel.Size = new System.Drawing.Size(52, 21);
            this.nudNumLevel.TabIndex = 32;
            this.nudNumLevel.ValueChanged += new System.EventHandler(this.nudNumLevel_ValueChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(124, 52);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 31;
            this.label6.Text = "金字塔层数:";
            // 
            // cbModelType
            // 
            this.cbModelType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbModelType.ArrowColor = System.Drawing.Color.White;
            this.cbModelType.BackColor = System.Drawing.SystemColors.Control;
            this.cbModelType.BaseColor = System.Drawing.Color.Black;
            this.cbModelType.BorderColor = System.Drawing.Color.Black;
            this.cbModelType.DropDownHeight = 250;
            this.cbModelType.DropDownWidth = 105;
            this.cbModelType.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbModelType.FormattingEnabled = true;
            this.cbModelType.IntegralHeight = false;
            this.cbModelType.Location = new System.Drawing.Point(65, 80);
            this.cbModelType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbModelType.Name = "cbModelType";
            this.cbModelType.Size = new System.Drawing.Size(70, 20);
            this.cbModelType.TabIndex = 30;
            this.cbModelType.SelectedIndexChanged += new System.EventHandler(this.cbModelType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(4, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "模板类型:";
            // 
            // numScore
            // 
            this.numScore.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numScore.BackColor = System.Drawing.Color.White;
            this.numScore.DecimalPlaces = 2;
            this.numScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numScore.Location = new System.Drawing.Point(188, 18);
            this.numScore.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numScore.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numScore.Name = "numScore";
            this.numScore.Size = new System.Drawing.Size(52, 21);
            this.numScore.TabIndex = 25;
            this.numScore.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numScore.ValueChanged += new System.EventHandler(this.numScore_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "匹配分数:";
            // 
            // numAngleExtent
            // 
            this.numAngleExtent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numAngleExtent.BackColor = System.Drawing.Color.White;
            this.numAngleExtent.DecimalPlaces = 2;
            this.numAngleExtent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numAngleExtent.Location = new System.Drawing.Point(65, 48);
            this.numAngleExtent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numAngleExtent.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numAngleExtent.Name = "numAngleExtent";
            this.numAngleExtent.Size = new System.Drawing.Size(50, 21);
            this.numAngleExtent.TabIndex = 23;
            this.numAngleExtent.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numAngleExtent.ValueChanged += new System.EventHandler(this.numAngleExtent_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "角度范围:";
            // 
            // numAngleStart
            // 
            this.numAngleStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numAngleStart.BackColor = System.Drawing.Color.White;
            this.numAngleStart.DecimalPlaces = 2;
            this.numAngleStart.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numAngleStart.Location = new System.Drawing.Point(65, 18);
            this.numAngleStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numAngleStart.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numAngleStart.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numAngleStart.Name = "numAngleStart";
            this.numAngleStart.Size = new System.Drawing.Size(50, 21);
            this.numAngleStart.TabIndex = 21;
            this.numAngleStart.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numAngleStart.ValueChanged += new System.EventHandler(this.numAngleStart_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "起始角度:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbCreateTmp);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(2, 72);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(267, 46);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作按钮";
            // 
            // cbCreateTmp
            // 
            this.cbCreateTmp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCreateTmp.AutoSize = true;
            this.cbCreateTmp.BackColor = System.Drawing.Color.Transparent;
            this.cbCreateTmp.Location = new System.Drawing.Point(100, 22);
            this.cbCreateTmp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbCreateTmp.Name = "cbCreateTmp";
            this.cbCreateTmp.Size = new System.Drawing.Size(72, 16);
            this.cbCreateTmp.TabIndex = 0;
            this.cbCreateTmp.Text = "创建模板";
            this.cbCreateTmp.UseVisualStyleBackColor = false;
            this.cbCreateTmp.CheckedChanged += new System.EventHandler(this.cbCreateTmp_CheckedChanged);
            // 
            // TmpPrmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Name = "TmpPrmForm";
            this.Size = new System.Drawing.Size(275, 459);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.gbTmpPrm.ResumeLayout(false);
            this.gbTmpPrm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngleExtent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngleStart)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbTmpPrm;
        private ControlWrap.ComboBoxEx cbModelType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numScore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numAngleExtent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numAngleStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private SelfControl.VaryCheckBox cbCreateTmp;
        private System.Windows.Forms.NumericUpDown nudNumLevel;
        private System.Windows.Forms.Label label6;
    }
}
