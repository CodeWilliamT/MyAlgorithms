
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sub_Translator));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbSubfilepath = new System.Windows.Forms.TextBox();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.lb_path = new System.Windows.Forms.Label();
            this.lb_folder = new System.Windows.Forms.Label();
            this.btnFolderTranslate = new System.Windows.Forms.Button();
            this.tbSubfoldername = new System.Windows.Forms.TextBox();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.comboBoxFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxTo = new System.Windows.Forms.ComboBox();
            this.lb_from = new System.Windows.Forms.Label();
            this.lb_to = new System.Windows.Forms.Label();
            this.comboBox_Format = new System.Windows.Forms.ComboBox();
            this.lb_format = new System.Windows.Forms.Label();
            this.lb_server = new System.Windows.Forms.Label();
            this.comboBox_Server = new System.Windows.Forms.ComboBox();
            this.lb_language = new System.Windows.Forms.Label();
            this.comboBox_language = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(1008, 44);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(150, 48);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbSubfilepath
            // 
            this.tbSubfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubfilepath.Location = new System.Drawing.Point(198, 48);
            this.tbSubfilepath.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbSubfilepath.Name = "tbSubfilepath";
            this.tbSubfilepath.Size = new System.Drawing.Size(796, 31);
            this.tbSubfilepath.TabIndex = 1;
            // 
            // btnTranslate
            // 
            this.btnTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTranslate.Location = new System.Drawing.Point(1204, 44);
            this.btnTranslate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(150, 48);
            this.btnTranslate.TabIndex = 2;
            this.btnTranslate.Text = "翻译";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // lb_path
            // 
            this.lb_path.AutoSize = true;
            this.lb_path.Location = new System.Drawing.Point(38, 54);
            this.lb_path.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_path.Name = "lb_path";
            this.lb_path.Size = new System.Drawing.Size(117, 25);
            this.lb_path.TabIndex = 3;
            this.lb_path.Text = "字幕路径：";
            // 
            // lb_folder
            // 
            this.lb_folder.AutoSize = true;
            this.lb_folder.Location = new System.Drawing.Point(36, 144);
            this.lb_folder.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_folder.Name = "lb_folder";
            this.lb_folder.Size = new System.Drawing.Size(138, 25);
            this.lb_folder.TabIndex = 7;
            this.lb_folder.Text = "字幕文件夹：";
            // 
            // btnFolderTranslate
            // 
            this.btnFolderTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderTranslate.Location = new System.Drawing.Point(1204, 135);
            this.btnFolderTranslate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnFolderTranslate.Name = "btnFolderTranslate";
            this.btnFolderTranslate.Size = new System.Drawing.Size(150, 48);
            this.btnFolderTranslate.TabIndex = 6;
            this.btnFolderTranslate.Text = "翻译";
            this.btnFolderTranslate.UseVisualStyleBackColor = true;
            this.btnFolderTranslate.Click += new System.EventHandler(this.btnFolderTranslate_Click);
            // 
            // tbSubfoldername
            // 
            this.tbSubfoldername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubfoldername.Location = new System.Drawing.Point(198, 138);
            this.tbSubfoldername.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbSubfoldername.Name = "tbSubfoldername";
            this.tbSubfoldername.Size = new System.Drawing.Size(796, 31);
            this.tbSubfoldername.TabIndex = 5;
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderBrowse.Location = new System.Drawing.Point(1008, 135);
            this.btnFolderBrowse.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(150, 48);
            this.btnFolderBrowse.TabIndex = 4;
            this.btnFolderBrowse.Text = "浏览";
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 323);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1400, 42);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(64, 32);
            this.toolStripStatusLabel1.Text = "准备";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 30);
            this.toolStripProgressBar1.Visible = false;
            // 
            // comboBoxFrom
            // 
            this.comboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Location = new System.Drawing.Point(416, 238);
            this.comboBoxFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxFrom.Name = "comboBoxFrom";
            this.comboBoxFrom.Size = new System.Drawing.Size(174, 33);
            this.comboBoxFrom.TabIndex = 13;
            // 
            // comboBoxTo
            // 
            this.comboBoxTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTo.FormattingEnabled = true;
            this.comboBoxTo.Location = new System.Drawing.Point(690, 238);
            this.comboBoxTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxTo.Name = "comboBoxTo";
            this.comboBoxTo.Size = new System.Drawing.Size(174, 33);
            this.comboBoxTo.TabIndex = 14;
            // 
            // lb_from
            // 
            this.lb_from.AutoSize = true;
            this.lb_from.Location = new System.Drawing.Point(344, 242);
            this.lb_from.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_from.Name = "lb_from";
            this.lb_from.Size = new System.Drawing.Size(67, 25);
            this.lb_from.TabIndex = 15;
            this.lb_from.Text = "From:";
            // 
            // lb_to
            // 
            this.lb_to.AutoSize = true;
            this.lb_to.Location = new System.Drawing.Point(630, 240);
            this.lb_to.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_to.Name = "lb_to";
            this.lb_to.Size = new System.Drawing.Size(43, 25);
            this.lb_to.TabIndex = 16;
            this.lb_to.Text = "To:";
            // 
            // comboBox_Format
            // 
            this.comboBox_Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Format.FormattingEnabled = true;
            this.comboBox_Format.Location = new System.Drawing.Point(988, 237);
            this.comboBox_Format.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_Format.Name = "comboBox_Format";
            this.comboBox_Format.Size = new System.Drawing.Size(136, 33);
            this.comboBox_Format.TabIndex = 17;
            // 
            // lb_format
            // 
            this.lb_format.AutoSize = true;
            this.lb_format.Location = new System.Drawing.Point(896, 240);
            this.lb_format.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_format.Name = "lb_format";
            this.lb_format.Size = new System.Drawing.Size(85, 25);
            this.lb_format.TabIndex = 18;
            this.lb_format.Text = "Format:";
            // 
            // lb_server
            // 
            this.lb_server.AutoSize = true;
            this.lb_server.Location = new System.Drawing.Point(38, 242);
            this.lb_server.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_server.Name = "lb_server";
            this.lb_server.Size = new System.Drawing.Size(81, 25);
            this.lb_server.TabIndex = 20;
            this.lb_server.Text = "Server:";
            // 
            // comboBox_Server
            // 
            this.comboBox_Server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Server.FormattingEnabled = true;
            this.comboBox_Server.Location = new System.Drawing.Point(138, 233);
            this.comboBox_Server.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_Server.Name = "comboBox_Server";
            this.comboBox_Server.Size = new System.Drawing.Size(174, 33);
            this.comboBox_Server.TabIndex = 19;
            // 
            // lb_language
            // 
            this.lb_language.AutoSize = true;
            this.lb_language.Location = new System.Drawing.Point(992, 319);
            this.lb_language.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_language.Name = "lb_language";
            this.lb_language.Size = new System.Drawing.Size(114, 25);
            this.lb_language.TabIndex = 22;
            this.lb_language.Text = "Language:";
            // 
            // comboBox_language
            // 
            this.comboBox_language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_language.FormattingEnabled = true;
            this.comboBox_language.Location = new System.Drawing.Point(1116, 312);
            this.comboBox_language.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_language.Name = "comboBox_language";
            this.comboBox_language.Size = new System.Drawing.Size(272, 33);
            this.comboBox_language.TabIndex = 21;
            this.comboBox_language.SelectedIndexChanged += new System.EventHandler(this.comboBox_language_SelectedIndexChanged);
            // 
            // Sub_Translator
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 365);
            this.Controls.Add(this.comboBox_language);
            this.Controls.Add(this.lb_language);
            this.Controls.Add(this.lb_server);
            this.Controls.Add(this.comboBox_Server);
            this.Controls.Add(this.lb_format);
            this.Controls.Add(this.comboBox_Format);
            this.Controls.Add(this.lb_to);
            this.Controls.Add(this.lb_from);
            this.Controls.Add(this.comboBoxTo);
            this.Controls.Add(this.comboBoxFrom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lb_folder);
            this.Controls.Add(this.btnFolderTranslate);
            this.Controls.Add(this.tbSubfoldername);
            this.Controls.Add(this.btnFolderBrowse);
            this.Controls.Add(this.lb_path);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.tbSubfilepath);
            this.Controls.Add(this.btnBrowse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Sub_Translator";
            this.Text = "Sub Translator By WilliamT";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Sub_Translator_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Sub_Translator_DragEnter);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbSubfilepath;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Label lb_path;
        private System.Windows.Forms.Label lb_folder;
        private System.Windows.Forms.Button btnFolderTranslate;
        private System.Windows.Forms.TextBox tbSubfoldername;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.ComboBox comboBoxTo;
        private System.Windows.Forms.Label lb_from;
        private System.Windows.Forms.Label lb_to;
        private System.Windows.Forms.ComboBox comboBox_Format;
        private System.Windows.Forms.Label lb_format;
        private System.Windows.Forms.Label lb_server;
        private System.Windows.Forms.ComboBox comboBox_Server;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Label lb_language;
        private System.Windows.Forms.ComboBox comboBox_language;
    }
}

