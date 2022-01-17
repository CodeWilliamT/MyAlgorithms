namespace ToolKits.LightChart
{
    partial class LightChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LightChart));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSavePrm = new SelfControl.VaryPicButton();
            this.dgvLightChart = new SelfControl.ColorDataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSavePrm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLightChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.23256F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.6124F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.34884F));
            this.tableLayoutPanel1.Controls.Add(this.btnSavePrm, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvLightChart, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.6831F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.3169F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(516, 568);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnSavePrm
            // 
            this.btnSavePrm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSavePrm.Image = global::AutomaticAOI.Properties.Resources.Save;
            this.btnSavePrm.Location = new System.Drawing.Point(413, 483);
            this.btnSavePrm.Name = "btnSavePrm";
            this.btnSavePrm.Size = new System.Drawing.Size(100, 82);
            this.btnSavePrm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnSavePrm.TabIndex = 1;
            this.btnSavePrm.TabStop = false;
            this.btnSavePrm.Click += new System.EventHandler(this.btnSavePrm_Click);
            // 
            // dgvLightChart
            // 
            this.dgvLightChart.AllowUserToAddRows = false;
            this.dgvLightChart.AllowUserToDeleteRows = false;
            this.dgvLightChart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLightChart.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLightChart.BackgroundColor = System.Drawing.Color.PaleTurquoise;
            this.dgvLightChart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dgvLightChart, 3);
            this.dgvLightChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLightChart.Location = new System.Drawing.Point(3, 3);
            this.dgvLightChart.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.dgvLightChart.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvLightChart.MergeColumnNames")));
            this.dgvLightChart.Name = "dgvLightChart";
            this.dgvLightChart.RowHeadersVisible = false;
            this.dgvLightChart.RowTemplate.Height = 27;
            this.dgvLightChart.Size = new System.Drawing.Size(510, 474);
            this.dgvLightChart.TabIndex = 0;
            this.dgvLightChart.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLightChart_CellContentClick);
            // 
            // LightChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LightChart";
            this.Size = new System.Drawing.Size(516, 568);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnSavePrm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLightChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private SelfControl.VaryPicButton btnSavePrm;
        private SelfControl.ColorDataGridView dgvLightChart;
    }
}
