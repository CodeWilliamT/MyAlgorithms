
namespace Sub_Translator
{
    partial class Sub_Translator
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbSubfilepath = new System.Windows.Forms.TextBox();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFolderTranslate = new System.Windows.Forms.Button();
            this.tbSubfoldername = new System.Windows.Forms.TextBox();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.cbReplace = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboBoxFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxTo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(909, 42);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(150, 46);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbSubfilepath
            // 
            this.tbSubfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubfilepath.Location = new System.Drawing.Point(198, 42);
            this.tbSubfilepath.Margin = new System.Windows.Forms.Padding(6);
            this.tbSubfilepath.Name = "tbSubfilepath";
            this.tbSubfilepath.Size = new System.Drawing.Size(695, 35);
            this.tbSubfilepath.TabIndex = 1;
            // 
            // btnTranslate
            // 
            this.btnTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTranslate.Location = new System.Drawing.Point(1105, 42);
            this.btnTranslate.Margin = new System.Windows.Forms.Padding(6);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(150, 46);
            this.btnTranslate.TabIndex = 2;
            this.btnTranslate.Text = "翻译";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "字幕路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 180);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "字幕文件夹：";
            // 
            // btnFolderTranslate
            // 
            this.btnFolderTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderTranslate.Location = new System.Drawing.Point(1105, 170);
            this.btnFolderTranslate.Margin = new System.Windows.Forms.Padding(6);
            this.btnFolderTranslate.Name = "btnFolderTranslate";
            this.btnFolderTranslate.Size = new System.Drawing.Size(150, 46);
            this.btnFolderTranslate.TabIndex = 6;
            this.btnFolderTranslate.Text = "翻译";
            this.btnFolderTranslate.UseVisualStyleBackColor = true;
            this.btnFolderTranslate.Click += new System.EventHandler(this.btnFolderTranslate_Click);
            // 
            // tbSubfoldername
            // 
            this.tbSubfoldername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubfoldername.Location = new System.Drawing.Point(198, 170);
            this.tbSubfoldername.Margin = new System.Windows.Forms.Padding(6);
            this.tbSubfoldername.Name = "tbSubfoldername";
            this.tbSubfoldername.Size = new System.Drawing.Size(695, 35);
            this.tbSubfoldername.TabIndex = 5;
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderBrowse.Location = new System.Drawing.Point(909, 170);
            this.btnFolderBrowse.Margin = new System.Windows.Forms.Padding(6);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(150, 46);
            this.btnFolderBrowse.TabIndex = 4;
            this.btnFolderBrowse.Text = "浏览";
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // cbReplace
            // 
            this.cbReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbReplace.AutoSize = true;
            this.cbReplace.Location = new System.Drawing.Point(1093, 256);
            this.cbReplace.Margin = new System.Windows.Forms.Padding(6);
            this.cbReplace.Name = "cbReplace";
            this.cbReplace.Size = new System.Drawing.Size(162, 28);
            this.cbReplace.TabIndex = 8;
            this.cbReplace.Text = "替换原文件";
            this.cbReplace.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 291);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1301, 42);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(65, 32);
            this.toolStripStatusLabel1.Text = "准备";
            // 
            // comboBoxFrom
            // 
            this.comboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Location = new System.Drawing.Point(594, 252);
            this.comboBoxFrom.Name = "comboBoxFrom";
            this.comboBoxFrom.Size = new System.Drawing.Size(174, 32);
            this.comboBoxFrom.TabIndex = 13;
            // 
            // comboBoxTo
            // 
            this.comboBoxTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTo.FormattingEnabled = true;
            this.comboBoxTo.Location = new System.Drawing.Point(885, 252);
            this.comboBoxTo.Name = "comboBoxTo";
            this.comboBoxTo.Size = new System.Drawing.Size(174, 32);
            this.comboBoxTo.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(525, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 24);
            this.label3.TabIndex = 15;
            this.label3.Text = "From:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(811, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 24);
            this.label4.TabIndex = 16;
            this.label4.Text = "To:";
            // 
            // Sub_Translator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 333);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxTo);
            this.Controls.Add(this.comboBoxFrom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFolderTranslate);
            this.Controls.Add(this.tbSubfoldername);
            this.Controls.Add(this.btnFolderBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.tbSubfilepath);
            this.Controls.Add(this.btnBrowse);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Sub_Translator";
            this.Text = "Sub_Translator";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbSubfilepath;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFolderTranslate;
        private System.Windows.Forms.TextBox tbSubfoldername;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.CheckBox cbReplace;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.ComboBox comboBoxTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

