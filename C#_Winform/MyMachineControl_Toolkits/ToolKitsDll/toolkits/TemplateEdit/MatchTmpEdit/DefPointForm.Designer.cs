namespace ToolKits.TemplateEdit.MatchTmpEdit
{
    partial class DefPointForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefPointForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExGetLine2 = new ControlWrap.ButtonEx();
            this.btnExShowLine2 = new ControlWrap.ButtonEx();
            this.btnExDelete = new ControlWrap.ButtonEx();
            this.btnExGetLine1 = new ControlWrap.ButtonEx();
            this.btnExAutoCalculate = new ControlWrap.ButtonEx();
            this.btnExShowLine1 = new ControlWrap.ButtonEx();
            this.btnExAdd = new ControlWrap.ButtonEx();
            this.dgvxDrawInfo = new ControlWrap.DataGridViewEx();
            this.drawType = new System.Windows.Forms.DataGridViewImageColumn();
            this.Display = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvxDrawInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(275, 459);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "画映射点";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvxDrawInfo, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 16);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(271, 441);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(2, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(267, 121);
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
            this.label1.Size = new System.Drawing.Size(263, 103);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. 在图像窗口选择画直线按钮\r\n2. 在图像中沿目标对角线画直线，右击鼠标，完成操作\r\n3. 左击【获取直线1】按钮，获取最新一次直线信息\r\n4. 重复步骤1-" +
    "3，获取直线2信息\r\n5. 左击【添加】按钮，获取最新一次映射点信息\r\n6. 如需多个映射点，只需重复步骤1-5\r\n7. 也可以用过画点按钮，直接添加多个映射点" +
    "\r\n8. 左击【下一步】按钮";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnExGetLine2);
            this.groupBox3.Controls.Add(this.btnExShowLine2);
            this.groupBox3.Controls.Add(this.btnExDelete);
            this.groupBox3.Controls.Add(this.btnExGetLine1);
            this.groupBox3.Controls.Add(this.btnExAutoCalculate);
            this.groupBox3.Controls.Add(this.btnExShowLine1);
            this.groupBox3.Controls.Add(this.btnExAdd);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(2, 127);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(267, 170);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作按钮";
            // 
            // btnExGetLine2
            // 
            this.btnExGetLine2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExGetLine2.BackColor = System.Drawing.Color.Maroon;
            this.btnExGetLine2.BorderColor = System.Drawing.Color.Empty;
            this.btnExGetLine2.ForeColor = System.Drawing.Color.White;
            this.btnExGetLine2.GlassEffect = true;
            this.btnExGetLine2.Location = new System.Drawing.Point(138, 29);
            this.btnExGetLine2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExGetLine2.Name = "btnExGetLine2";
            this.btnExGetLine2.Radius_All = 3;
            this.btnExGetLine2.Radius_BottomLeft = 3;
            this.btnExGetLine2.Radius_BottomRight = 3;
            this.btnExGetLine2.Radius_TopLeft = 3;
            this.btnExGetLine2.Radius_TopRight = 3;
            this.btnExGetLine2.Size = new System.Drawing.Size(94, 27);
            this.btnExGetLine2.TabIndex = 14;
            this.btnExGetLine2.Text = "获取直线2";
            this.btnExGetLine2.UseVisualStyleBackColor = false;
            this.btnExGetLine2.Click += new System.EventHandler(this.btnExGetLine2_Click);
            // 
            // btnExShowLine2
            // 
            this.btnExShowLine2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExShowLine2.BackColor = System.Drawing.Color.Maroon;
            this.btnExShowLine2.BorderColor = System.Drawing.Color.Empty;
            this.btnExShowLine2.ForeColor = System.Drawing.Color.White;
            this.btnExShowLine2.GlassEffect = true;
            this.btnExShowLine2.Location = new System.Drawing.Point(138, 64);
            this.btnExShowLine2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExShowLine2.Name = "btnExShowLine2";
            this.btnExShowLine2.Radius_All = 3;
            this.btnExShowLine2.Radius_BottomLeft = 3;
            this.btnExShowLine2.Radius_BottomRight = 3;
            this.btnExShowLine2.Radius_TopLeft = 3;
            this.btnExShowLine2.Radius_TopRight = 3;
            this.btnExShowLine2.Size = new System.Drawing.Size(94, 27);
            this.btnExShowLine2.TabIndex = 14;
            this.btnExShowLine2.Text = "显示直线2";
            this.btnExShowLine2.UseVisualStyleBackColor = false;
            this.btnExShowLine2.Click += new System.EventHandler(this.btnExShowLine2_Click);
            // 
            // btnExDelete
            // 
            this.btnExDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExDelete.BackColor = System.Drawing.Color.Maroon;
            this.btnExDelete.BorderColor = System.Drawing.Color.Empty;
            this.btnExDelete.ForeColor = System.Drawing.Color.White;
            this.btnExDelete.GlassEffect = true;
            this.btnExDelete.Location = new System.Drawing.Point(138, 133);
            this.btnExDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExDelete.Name = "btnExDelete";
            this.btnExDelete.Radius_All = 3;
            this.btnExDelete.Radius_BottomLeft = 3;
            this.btnExDelete.Radius_BottomRight = 3;
            this.btnExDelete.Radius_TopLeft = 3;
            this.btnExDelete.Radius_TopRight = 3;
            this.btnExDelete.Size = new System.Drawing.Size(94, 27);
            this.btnExDelete.TabIndex = 14;
            this.btnExDelete.Text = "删除";
            this.tip.SetToolTip(this.btnExDelete, "删除选中行");
            this.btnExDelete.UseVisualStyleBackColor = false;
            this.btnExDelete.Click += new System.EventHandler(this.btnExDelete_Click);
            // 
            // btnExGetLine1
            // 
            this.btnExGetLine1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnExGetLine1.BackColor = System.Drawing.Color.Maroon;
            this.btnExGetLine1.BorderColor = System.Drawing.Color.Empty;
            this.btnExGetLine1.ForeColor = System.Drawing.Color.White;
            this.btnExGetLine1.GlassEffect = true;
            this.btnExGetLine1.Location = new System.Drawing.Point(38, 29);
            this.btnExGetLine1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExGetLine1.Name = "btnExGetLine1";
            this.btnExGetLine1.Radius_All = 3;
            this.btnExGetLine1.Radius_BottomLeft = 3;
            this.btnExGetLine1.Radius_BottomRight = 3;
            this.btnExGetLine1.Radius_TopLeft = 3;
            this.btnExGetLine1.Radius_TopRight = 3;
            this.btnExGetLine1.Size = new System.Drawing.Size(94, 27);
            this.btnExGetLine1.TabIndex = 14;
            this.btnExGetLine1.Text = "获取直线1";
            this.btnExGetLine1.UseVisualStyleBackColor = false;
            this.btnExGetLine1.Click += new System.EventHandler(this.btnExGetLine1_Click);
            // 
            // btnExAutoCalculate
            // 
            this.btnExAutoCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExAutoCalculate.BackColor = System.Drawing.Color.Maroon;
            this.btnExAutoCalculate.BorderColor = System.Drawing.Color.Empty;
            this.btnExAutoCalculate.ForeColor = System.Drawing.Color.White;
            this.btnExAutoCalculate.GlassEffect = true;
            this.btnExAutoCalculate.Location = new System.Drawing.Point(38, 98);
            this.btnExAutoCalculate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExAutoCalculate.Name = "btnExAutoCalculate";
            this.btnExAutoCalculate.Radius_All = 3;
            this.btnExAutoCalculate.Radius_BottomLeft = 3;
            this.btnExAutoCalculate.Radius_BottomRight = 3;
            this.btnExAutoCalculate.Radius_TopLeft = 3;
            this.btnExAutoCalculate.Radius_TopRight = 3;
            this.btnExAutoCalculate.Size = new System.Drawing.Size(194, 27);
            this.btnExAutoCalculate.TabIndex = 14;
            this.btnExAutoCalculate.Text = "自动计算映射点";
            this.tip.SetToolTip(this.btnExAutoCalculate, "自动计算显示区的中心");
            this.btnExAutoCalculate.UseVisualStyleBackColor = false;
            this.btnExAutoCalculate.Click += new System.EventHandler(this.btnExAutoCalculate_Click);
            // 
            // btnExShowLine1
            // 
            this.btnExShowLine1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnExShowLine1.BackColor = System.Drawing.Color.Maroon;
            this.btnExShowLine1.BorderColor = System.Drawing.Color.Empty;
            this.btnExShowLine1.ForeColor = System.Drawing.Color.White;
            this.btnExShowLine1.GlassEffect = true;
            this.btnExShowLine1.Location = new System.Drawing.Point(38, 64);
            this.btnExShowLine1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExShowLine1.Name = "btnExShowLine1";
            this.btnExShowLine1.Radius_All = 3;
            this.btnExShowLine1.Radius_BottomLeft = 3;
            this.btnExShowLine1.Radius_BottomRight = 3;
            this.btnExShowLine1.Radius_TopLeft = 3;
            this.btnExShowLine1.Radius_TopRight = 3;
            this.btnExShowLine1.Size = new System.Drawing.Size(94, 27);
            this.btnExShowLine1.TabIndex = 14;
            this.btnExShowLine1.Text = "显示直线1";
            this.btnExShowLine1.UseVisualStyleBackColor = false;
            this.btnExShowLine1.Click += new System.EventHandler(this.btnExShowLine1_Click);
            // 
            // btnExAdd
            // 
            this.btnExAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnExAdd.BackColor = System.Drawing.Color.Maroon;
            this.btnExAdd.BorderColor = System.Drawing.Color.Empty;
            this.btnExAdd.ForeColor = System.Drawing.Color.White;
            this.btnExAdd.GlassEffect = true;
            this.btnExAdd.Location = new System.Drawing.Point(38, 133);
            this.btnExAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExAdd.Name = "btnExAdd";
            this.btnExAdd.Radius_All = 3;
            this.btnExAdd.Radius_BottomLeft = 3;
            this.btnExAdd.Radius_BottomRight = 3;
            this.btnExAdd.Radius_TopLeft = 3;
            this.btnExAdd.Radius_TopRight = 3;
            this.btnExAdd.Size = new System.Drawing.Size(94, 27);
            this.btnExAdd.TabIndex = 14;
            this.btnExAdd.Text = "添加";
            this.tip.SetToolTip(this.btnExAdd, "添加一行");
            this.btnExAdd.UseVisualStyleBackColor = false;
            this.btnExAdd.Click += new System.EventHandler(this.btnExAdd_Click);
            // 
            // dgvxDrawInfo
            // 
            this.dgvxDrawInfo.AllowUserToAddRows = false;
            this.dgvxDrawInfo.AllowUserToDeleteRows = false;
            this.dgvxDrawInfo.AllowUserToResizeRows = false;
            this.dgvxDrawInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvxDrawInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvxDrawInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvxDrawInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvxDrawInfo.ColumnHeadersHeight = 30;
            this.dgvxDrawInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvxDrawInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.drawType,
            this.Display});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvxDrawInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvxDrawInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvxDrawInfo.EnableHeadersVisualStyles = false;
            this.dgvxDrawInfo.Location = new System.Drawing.Point(2, 301);
            this.dgvxDrawInfo.Margin = new System.Windows.Forms.Padding(2);
            this.dgvxDrawInfo.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.dgvxDrawInfo.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvxDrawInfo.MergeColumnNames")));
            this.dgvxDrawInfo.Name = "dgvxDrawInfo";
            this.dgvxDrawInfo.RowHeadersVisible = false;
            this.dgvxDrawInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvxDrawInfo.RowTemplate.Height = 30;
            this.dgvxDrawInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvxDrawInfo.Size = new System.Drawing.Size(267, 125);
            this.dgvxDrawInfo.TabIndex = 1;
            this.dgvxDrawInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvxDrawInfo_CellContentClick);
            // 
            // drawType
            // 
            this.drawType.FillWeight = 10F;
            this.drawType.HeaderText = "";
            this.drawType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.drawType.Name = "drawType";
            this.drawType.ReadOnly = true;
            // 
            // Display
            // 
            this.Display.FillWeight = 90F;
            this.Display.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Display.HeaderText = "显示";
            this.Display.Name = "Display";
            this.Display.Text = "显示";
            this.Display.UseColumnTextForButtonValue = true;
            // 
            // DefPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "DefPointForm";
            this.Size = new System.Drawing.Size(275, 459);
            this.Load += new System.EventHandler(this.DefPointForm_Load);
            this.VisibleChanged += new System.EventHandler(this.DefPointForm_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvxDrawInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private ControlWrap.ButtonEx btnExGetLine2;
        private ControlWrap.ButtonEx btnExShowLine2;
        private ControlWrap.ButtonEx btnExDelete;
        private System.Windows.Forms.ToolTip tip;
        private ControlWrap.ButtonEx btnExGetLine1;
        private ControlWrap.ButtonEx btnExShowLine1;
        private ControlWrap.ButtonEx btnExAdd;
        private ControlWrap.DataGridViewEx dgvxDrawInfo;
        private System.Windows.Forms.DataGridViewImageColumn drawType;
        private System.Windows.Forms.DataGridViewButtonColumn Display;
        private ControlWrap.ButtonEx btnExAutoCalculate;

    }
}
