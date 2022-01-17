namespace LeadframeAOI.UI
{
    partial class ShowLogo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowLogo));
            this.status_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtbInfoView = new LeadframeAOI.UI.ControlWrap.RichTextBoxEx();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // status_ProgressBar
            // 
            this.status_ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status_ProgressBar.ForeColor = System.Drawing.Color.White;
            this.status_ProgressBar.Location = new System.Drawing.Point(0, 0);
            this.status_ProgressBar.MarqueeAnimationSpeed = 20;
            this.status_ProgressBar.Name = "status_ProgressBar";
            this.status_ProgressBar.Size = new System.Drawing.Size(798, 20);
            this.status_ProgressBar.Step = 20;
            this.status_ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.status_ProgressBar.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.status_ProgressBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 406);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 20);
            this.panel1.TabIndex = 4;
            // 
            // rtbInfoView
            // 
            this.rtbInfoView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInfoView.Enabled = false;
            this.rtbInfoView.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbInfoView.ForeColor = System.Drawing.Color.White;
            this.rtbInfoView.Location = new System.Drawing.Point(531, 155);
            this.rtbInfoView.Name = "rtbInfoView";
            this.rtbInfoView.ReadOnly = true;
            this.rtbInfoView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbInfoView.Size = new System.Drawing.Size(234, 141);
            this.rtbInfoView.TabIndex = 5;
            this.rtbInfoView.Text = "";
            // 
            // ShowLogo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(798, 426);
            this.Controls.Add(this.rtbInfoView);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShowLogo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowLogo";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar status_ProgressBar;
        private System.Windows.Forms.Panel panel1;
        private ControlWrap.RichTextBoxEx rtbInfoView;
    }
}