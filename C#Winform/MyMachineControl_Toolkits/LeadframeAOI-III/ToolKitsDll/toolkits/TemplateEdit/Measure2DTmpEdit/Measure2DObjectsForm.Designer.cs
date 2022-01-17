namespace ToolKits.TemplateEdit.Measure2DTmpEdit
{
    partial class Measure2DObjectsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Measure2DObjectsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvMsr2DObjects = new ControlWrap.DataGridViewEx();
            this.drawType = new System.Windows.Forms.DataGridViewImageColumn();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Display = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Msr2DPrm = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExDelete = new ControlWrap.ButtonEx();
            this.btnExAdd = new ControlWrap.ButtonEx();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMsr2DObjects)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvMsr2DObjects);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 459);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "画测量区";
            // 
            // dgvMsr2DObjects
            // 
            this.dgvMsr2DObjects.AllowUserToAddRows = false;
            this.dgvMsr2DObjects.AllowUserToDeleteRows = false;
            this.dgvMsr2DObjects.AllowUserToResizeRows = false;
            this.dgvMsr2DObjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMsr2DObjects.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvMsr2DObjects.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMsr2DObjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMsr2DObjects.ColumnHeadersHeight = 30;
            this.dgvMsr2DObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMsr2DObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.drawType,
            this.colIndex,
            this.Display,
            this.Msr2DPrm});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMsr2DObjects.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvMsr2DObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMsr2DObjects.EnableHeadersVisualStyles = false;
            this.dgvMsr2DObjects.Location = new System.Drawing.Point(3, 204);
            this.dgvMsr2DObjects.Margin = new System.Windows.Forms.Padding(2);
            this.dgvMsr2DObjects.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.dgvMsr2DObjects.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvMsr2DObjects.MergeColumnNames")));
            this.dgvMsr2DObjects.Name = "dgvMsr2DObjects";
            this.dgvMsr2DObjects.ReadOnly = true;
            this.dgvMsr2DObjects.RowHeadersVisible = false;
            this.dgvMsr2DObjects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMsr2DObjects.RowTemplate.Height = 30;
            this.dgvMsr2DObjects.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMsr2DObjects.Size = new System.Drawing.Size(304, 252);
            this.dgvMsr2DObjects.TabIndex = 15;
            this.dgvMsr2DObjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMsr2DObjects_CellContentClick);
            // 
            // drawType
            // 
            this.drawType.FillWeight = 10F;
            this.drawType.HeaderText = "";
            this.drawType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.drawType.Name = "drawType";
            this.drawType.ReadOnly = true;
            // 
            // colIndex
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colIndex.DefaultCellStyle = dataGridViewCellStyle2;
            this.colIndex.FillWeight = 15F;
            this.colIndex.HeaderText = "索引";
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            this.colIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colIndex.ToolTipText = "测量区域索引号";
            // 
            // Display
            // 
            this.Display.FillWeight = 50F;
            this.Display.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Display.HeaderText = "显示";
            this.Display.Name = "Display";
            this.Display.ReadOnly = true;
            this.Display.Text = "显示";
            this.Display.UseColumnTextForButtonValue = true;
            // 
            // Msr2DPrm
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Msr2DPrm.DefaultCellStyle = dataGridViewCellStyle3;
            this.Msr2DPrm.FillWeight = 25F;
            this.Msr2DPrm.HeaderText = "测量参数";
            this.Msr2DPrm.Name = "Msr2DPrm";
            this.Msr2DPrm.ReadOnly = true;
            this.Msr2DPrm.Text = "...";
            this.Msr2DPrm.ToolTipText = "测量参数";
            this.Msr2DPrm.UseColumnTextForButtonValue = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnExDelete);
            this.groupBox3.Controls.Add(this.btnExAdd);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 138);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(304, 66);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作按钮";
            // 
            // btnExDelete
            // 
            this.btnExDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExDelete.BackColor = System.Drawing.Color.Maroon;
            this.btnExDelete.BorderColor = System.Drawing.Color.Empty;
            this.btnExDelete.ForeColor = System.Drawing.Color.White;
            this.btnExDelete.GlassEffect = true;
            this.btnExDelete.Location = new System.Drawing.Point(175, 23);
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
            this.btnExDelete.UseVisualStyleBackColor = false;
            this.btnExDelete.Click += new System.EventHandler(this.btnExDelete_Click);
            // 
            // btnExAdd
            // 
            this.btnExAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExAdd.BackColor = System.Drawing.Color.Maroon;
            this.btnExAdd.BorderColor = System.Drawing.Color.Empty;
            this.btnExAdd.ForeColor = System.Drawing.Color.White;
            this.btnExAdd.GlassEffect = true;
            this.btnExAdd.Location = new System.Drawing.Point(38, 23);
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
            this.btnExAdd.UseVisualStyleBackColor = false;
            this.btnExAdd.Click += new System.EventHandler(this.btnExAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 17);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(304, 121);
            this.groupBox2.TabIndex = 3;
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
            this.label1.Size = new System.Drawing.Size(300, 103);
            this.label1.TabIndex = 1;
            this.label1.Text = "1. 在图像窗口选择交互按钮，可选择圆、椭圆、\r\n有方向矩形和无方向矩形四种交互类型\r\n2. 在图像中画出测量区域，右击鼠标，完成操作\r\n3. 左击【增加】按钮，" +
    "可以获取最新一次测量区域信息\r\n4. 如需多个测量区域，只需重复步骤1-3\r\n5. 左击【下一步】按钮";
            // 
            // Measure2DObjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "Measure2DObjectsForm";
            this.Load += new System.EventHandler(this.Measure2DObjectsForm_Load);
            this.VisibleChanged += new System.EventHandler(this.Measure2DObjectsForm_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMsr2DObjects)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private ControlWrap.ButtonEx btnExDelete;
        private ControlWrap.ButtonEx btnExAdd;
        private ControlWrap.DataGridViewEx dgvMsr2DObjects;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewImageColumn drawType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewButtonColumn Display;
        private System.Windows.Forms.DataGridViewButtonColumn Msr2DPrm;
    }
}
