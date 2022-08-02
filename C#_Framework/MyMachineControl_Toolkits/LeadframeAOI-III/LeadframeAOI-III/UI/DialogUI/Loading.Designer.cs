namespace LeadframeAOI.UI
{
    partial class Loading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loading));
            this.picbLoading = new System.Windows.Forms.PictureBox();
            this.lbltxt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // picbLoading
            // 
            this.picbLoading.BackColor = System.Drawing.Color.Bisque;
            this.picbLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picbLoading.Image = ((System.Drawing.Image)(resources.GetObject("picbLoading.Image")));
            this.picbLoading.InitialImage = null;
            this.picbLoading.Location = new System.Drawing.Point(0, 0);
            this.picbLoading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picbLoading.Name = "picbLoading";
            this.picbLoading.Size = new System.Drawing.Size(477, 140);
            this.picbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbLoading.TabIndex = 4;
            this.picbLoading.TabStop = false;
            // 
            // lbltxt
            // 
            this.lbltxt.BackColor = System.Drawing.Color.Gainsboro;
            this.lbltxt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbltxt.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltxt.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbltxt.Location = new System.Drawing.Point(0, 140);
            this.lbltxt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbltxt.Name = "lbltxt";
            this.lbltxt.Size = new System.Drawing.Size(477, 62);
            this.lbltxt.TabIndex = 3;
            this.lbltxt.Text = "Being Initialize";
            this.lbltxt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(477, 202);
            this.Controls.Add(this.picbLoading);
            this.Controls.Add(this.lbltxt);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Loading";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading";
            ((System.ComponentModel.ISupportInitialize)(this.picbLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picbLoading;
        private System.Windows.Forms.Label lbltxt;
    }
}