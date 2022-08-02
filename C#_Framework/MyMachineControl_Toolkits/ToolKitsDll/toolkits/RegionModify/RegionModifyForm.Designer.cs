namespace ToolKits.RegionModify
{
    partial class RegionModifyForm
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
            this.btnExModify = new ControlWrap.ButtonEx();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExDelete = new ControlWrap.ButtonEx();
            this.btnExAdd = new ControlWrap.ButtonEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExModify
            // 
            this.btnExModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExModify.BackColor = System.Drawing.Color.Maroon;
            this.btnExModify.BorderColor = System.Drawing.Color.Empty;
            this.btnExModify.ForeColor = System.Drawing.Color.White;
            this.btnExModify.GlassEffect = true;
            this.btnExModify.Location = new System.Drawing.Point(88, 25);
            this.btnExModify.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExModify.Name = "btnExModify";
            this.btnExModify.Radius_All = 3;
            this.btnExModify.Radius_BottomLeft = 3;
            this.btnExModify.Radius_BottomRight = 3;
            this.btnExModify.Radius_TopLeft = 3;
            this.btnExModify.Radius_TopRight = 3;
            this.btnExModify.Size = new System.Drawing.Size(110, 27);
            this.btnExModify.TabIndex = 12;
            this.btnExModify.Text = "优化";
            this.btnExModify.UseVisualStyleBackColor = false;
            this.btnExModify.Click += new System.EventHandler(this.btnExModify_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(294, 463);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "优化区域";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 16);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.55696F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.44304F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 176F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 445);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(2, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(286, 96);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作提示";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(282, 78);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. 选择“点”交互类型\r\n1. 在需要修改的区域内画点，右击完成，表明此区域\r\n   需要修改\r\n2. 左击操作按钮\r\n3. 若需添加区域，需要在图像窗口中画出" +
    "区域，左击\r\n  【添加】";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnExDelete);
            this.groupBox3.Controls.Add(this.btnExAdd);
            this.groupBox3.Controls.Add(this.btnExModify);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(2, 102);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(286, 148);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作按钮";
            // 
            // btnExDelete
            // 
            this.btnExDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExDelete.BackColor = System.Drawing.Color.Maroon;
            this.btnExDelete.BorderColor = System.Drawing.Color.Empty;
            this.btnExDelete.ForeColor = System.Drawing.Color.White;
            this.btnExDelete.GlassEffect = true;
            this.btnExDelete.Location = new System.Drawing.Point(88, 68);
            this.btnExDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExDelete.Name = "btnExDelete";
            this.btnExDelete.Radius_All = 3;
            this.btnExDelete.Radius_BottomLeft = 3;
            this.btnExDelete.Radius_BottomRight = 3;
            this.btnExDelete.Radius_TopLeft = 3;
            this.btnExDelete.Radius_TopRight = 3;
            this.btnExDelete.Size = new System.Drawing.Size(110, 27);
            this.btnExDelete.TabIndex = 12;
            this.btnExDelete.Text = "删除";
            this.btnExDelete.UseVisualStyleBackColor = false;
            this.btnExDelete.Click += new System.EventHandler(this.btnExDelete_Click);
            // 
            // btnExAdd
            // 
            this.btnExAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExAdd.BackColor = System.Drawing.Color.Maroon;
            this.btnExAdd.BorderColor = System.Drawing.Color.Empty;
            this.btnExAdd.ForeColor = System.Drawing.Color.White;
            this.btnExAdd.GlassEffect = true;
            this.btnExAdd.Location = new System.Drawing.Point(88, 110);
            this.btnExAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExAdd.Name = "btnExAdd";
            this.btnExAdd.Radius_All = 3;
            this.btnExAdd.Radius_BottomLeft = 3;
            this.btnExAdd.Radius_BottomRight = 3;
            this.btnExAdd.Radius_TopLeft = 3;
            this.btnExAdd.Radius_TopRight = 3;
            this.btnExAdd.Size = new System.Drawing.Size(110, 27);
            this.btnExAdd.TabIndex = 12;
            this.btnExAdd.Text = "增加";
            this.btnExAdd.UseVisualStyleBackColor = false;
            this.btnExAdd.Click += new System.EventHandler(this.btnExAdd_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(294, 500);
            this.splitContainer1.SplitterDistance = 463;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 14;
            // 
            // RegionModifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RegionModifyForm";
            this.Size = new System.Drawing.Size(294, 500);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ControlWrap.ButtonEx btnExModify;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private ControlWrap.ButtonEx btnExDelete;
        private ControlWrap.ButtonEx btnExAdd;
    }
}
