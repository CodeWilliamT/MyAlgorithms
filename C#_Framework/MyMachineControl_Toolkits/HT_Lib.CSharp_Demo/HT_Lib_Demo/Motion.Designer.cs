namespace WindowsFormsApp1
{
    partial class Motion
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
            this.button_Init = new System.Windows.Forms.Button();
            this.button_Discard = new System.Windows.Forms.Button();
            this.button_LoadUI = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_Pop = new System.Windows.Forms.Button();
            this.button_LoadToolUi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Init
            // 
            this.button_Init.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_Init.Location = new System.Drawing.Point(0, 481);
            this.button_Init.Name = "button_Init";
            this.button_Init.Size = new System.Drawing.Size(208, 23);
            this.button_Init.TabIndex = 0;
            this.button_Init.Text = "初始化";
            this.button_Init.UseVisualStyleBackColor = true;
            this.button_Init.Click += new System.EventHandler(this.button_Init_Click);
            // 
            // button_Discard
            // 
            this.button_Discard.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_Discard.Location = new System.Drawing.Point(0, 504);
            this.button_Discard.Name = "button_Discard";
            this.button_Discard.Size = new System.Drawing.Size(208, 23);
            this.button_Discard.TabIndex = 1;
            this.button_Discard.Text = "释放";
            this.button_Discard.UseVisualStyleBackColor = true;
            this.button_Discard.Click += new System.EventHandler(this.button_Discard_Click);
            // 
            // button_LoadUI
            // 
            this.button_LoadUI.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_LoadUI.Location = new System.Drawing.Point(0, 0);
            this.button_LoadUI.Name = "button_LoadUI";
            this.button_LoadUI.Size = new System.Drawing.Size(208, 23);
            this.button_LoadUI.TabIndex = 2;
            this.button_LoadUI.Text = "嵌入窗口";
            this.button_LoadUI.UseVisualStyleBackColor = true;
            this.button_LoadUI.Click += new System.EventHandler(this.button_LoadUI_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(651, 527);
            this.panel1.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_LoadToolUi);
            this.splitContainer1.Panel1.Controls.Add(this.button_Pop);
            this.splitContainer1.Panel1.Controls.Add(this.button_Init);
            this.splitContainer1.Panel1.Controls.Add(this.button_Discard);
            this.splitContainer1.Panel1.Controls.Add(this.button_LoadUI);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(863, 527);
            this.splitContainer1.SplitterDistance = 208;
            this.splitContainer1.TabIndex = 5;
            // 
            // button_Pop
            // 
            this.button_Pop.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_Pop.Location = new System.Drawing.Point(0, 23);
            this.button_Pop.Name = "button_Pop";
            this.button_Pop.Size = new System.Drawing.Size(208, 23);
            this.button_Pop.TabIndex = 4;
            this.button_Pop.Text = "弹出非模态窗口";
            this.button_Pop.UseVisualStyleBackColor = true;
            this.button_Pop.Click += new System.EventHandler(this.button_Pop_Click);
            // 
            // button_LoadToolUi
            // 
            this.button_LoadToolUi.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_LoadToolUi.Location = new System.Drawing.Point(0, 46);
            this.button_LoadToolUi.Name = "button_LoadToolUi";
            this.button_LoadToolUi.Size = new System.Drawing.Size(208, 23);
            this.button_LoadToolUi.TabIndex = 5;
            this.button_LoadToolUi.Text = "加载工具界面";
            this.button_LoadToolUi.UseVisualStyleBackColor = true;
            this.button_LoadToolUi.Click += new System.EventHandler(this.button_LoadToolUi_Click);
            // 
            // Motion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 527);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Motion";
            this.Text = "Motion";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Init;
        private System.Windows.Forms.Button button_Discard;
        private System.Windows.Forms.Button button_LoadUI;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button_Pop;
        private System.Windows.Forms.Button button_LoadToolUi;
    }
}