namespace ToolKits.TemplateEdit.MatchTmpEdit
{
    partial class ModelRegionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelRegionForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvxDrawInfo = new ControlWrap.DataGridViewEx();
            this.drawType = new System.Windows.Forms.DataGridViewImageColumn();
            this.colAcceptMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Display = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExDelete = new ControlWrap.ButtonEx();
            this.btnExAdd = new ControlWrap.ButtonEx();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvxDrawInfo)).BeginInit();
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
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "画模板区";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvxDrawInfo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 16);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.15493F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.84507F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 253F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
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
            this.groupBox2.Size = new System.Drawing.Size(267, 97);
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
            this.label1.Size = new System.Drawing.Size(263, 79);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. 在图像窗口选择交互按钮\r\n2. 在图像中画出感兴趣区域，右击鼠标，完成操作\r\n3. 左击【增加】按钮，可以获取最新一次区域信息\r\n4. 如需多个区域，只需重" +
    "复步骤1-3\r\n5. 左击【下一步】按钮";
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
            this.colAcceptMode,
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
            this.dgvxDrawInfo.Location = new System.Drawing.Point(2, 173);
            this.dgvxDrawInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvxDrawInfo.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.dgvxDrawInfo.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvxDrawInfo.MergeColumnNames")));
            this.dgvxDrawInfo.Name = "dgvxDrawInfo";
            this.dgvxDrawInfo.RowHeadersVisible = false;
            this.dgvxDrawInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvxDrawInfo.RowTemplate.Height = 30;
            this.dgvxDrawInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvxDrawInfo.Size = new System.Drawing.Size(267, 249);
            this.dgvxDrawInfo.TabIndex = 1;
            this.dgvxDrawInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvxDrawInfo_CellContentClick);
            this.dgvxDrawInfo.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvxDrawInfo_EditingControlShowing);
            // 
            // drawType
            // 
            this.drawType.FillWeight = 10F;
            this.drawType.HeaderText = "";
            this.drawType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.drawType.Name = "drawType";
            this.drawType.ReadOnly = true;
            // 
            // colAcceptMode
            // 
            this.colAcceptMode.FillWeight = 45F;
            this.colAcceptMode.HeaderText = "接受类型";
            this.colAcceptMode.Name = "colAcceptMode";
            // 
            // Display
            // 
            this.Display.DataPropertyName = "Show";
            this.Display.FillWeight = 45F;
            this.Display.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Display.HeaderText = "显示";
            this.Display.Name = "Display";
            this.Display.Text = "显示";
            this.Display.UseColumnTextForButtonValue = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnExDelete);
            this.groupBox3.Controls.Add(this.btnExAdd);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(2, 103);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(267, 66);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作按钮";
            // 
            // btnExDelete
            // 
            this.btnExDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExDelete.BackColor = System.Drawing.Color.Maroon;
            this.btnExDelete.BorderColor = System.Drawing.Color.Empty;
            this.btnExDelete.ForeColor = System.Drawing.Color.White;
            this.btnExDelete.GlassEffect = true;
            this.btnExDelete.Location = new System.Drawing.Point(138, 23);
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
            // btnExAdd
            // 
            this.btnExAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
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
            this.tip.SetToolTip(this.btnExAdd, "添加一行");
            this.btnExAdd.UseVisualStyleBackColor = false;
            this.btnExAdd.Click += new System.EventHandler(this.btnExAdd_Click);
            // 
            // ModelRegionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ModelRegionForm";
            this.Size = new System.Drawing.Size(275, 459);
            this.VisibleChanged += new System.EventHandler(this.ModelRegionForm_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvxDrawInfo)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private ControlWrap.ButtonEx btnExDelete;
        private System.Windows.Forms.ToolTip tip;
        private ControlWrap.ButtonEx btnExAdd;
        private System.Windows.Forms.GroupBox groupBox3;
        private ControlWrap.DataGridViewEx dgvxDrawInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewImageColumn drawType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAcceptMode;
        private System.Windows.Forms.DataGridViewButtonColumn Display;
    }
}
