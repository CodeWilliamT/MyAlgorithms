namespace BSP_Example
{
    partial class BSP_Example_Form
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
            this.button_Init = new System.Windows.Forms.Button();
            this.button_LoadUI = new System.Windows.Forms.Button();
            this.button_LoadTool = new System.Windows.Forms.Button();
            this.listBox_DebugInfo = new System.Windows.Forms.ListBox();
            this.button_Time = new System.Windows.Forms.Button();
            this.checkBox_Remote = new System.Windows.Forms.CheckBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_Init
            // 
            this.button_Init.Location = new System.Drawing.Point(12, 41);
            this.button_Init.Name = "button_Init";
            this.button_Init.Size = new System.Drawing.Size(89, 23);
            this.button_Init.TabIndex = 0;
            this.button_Init.Text = "初始化";
            this.button_Init.UseVisualStyleBackColor = true;
            this.button_Init.Click += new System.EventHandler(this.Button_Init_Click);
            // 
            // button_LoadUI
            // 
            this.button_LoadUI.Enabled = false;
            this.button_LoadUI.Location = new System.Drawing.Point(12, 104);
            this.button_LoadUI.Name = "button_LoadUI";
            this.button_LoadUI.Size = new System.Drawing.Size(89, 23);
            this.button_LoadUI.TabIndex = 1;
            this.button_LoadUI.Text = "加载面板";
            this.button_LoadUI.UseVisualStyleBackColor = true;
            this.button_LoadUI.Click += new System.EventHandler(this.button_LoadUI_Click);
            // 
            // button_LoadTool
            // 
            this.button_LoadTool.Enabled = false;
            this.button_LoadTool.Location = new System.Drawing.Point(12, 146);
            this.button_LoadTool.Name = "button_LoadTool";
            this.button_LoadTool.Size = new System.Drawing.Size(89, 23);
            this.button_LoadTool.TabIndex = 2;
            this.button_LoadTool.Text = "加载工具面板";
            this.button_LoadTool.UseVisualStyleBackColor = true;
            this.button_LoadTool.Click += new System.EventHandler(this.button_LoadTool_Click);
            // 
            // listBox_DebugInfo
            // 
            this.listBox_DebugInfo.FormattingEnabled = true;
            this.listBox_DebugInfo.ItemHeight = 12;
            this.listBox_DebugInfo.Location = new System.Drawing.Point(107, 41);
            this.listBox_DebugInfo.Name = "listBox_DebugInfo";
            this.listBox_DebugInfo.Size = new System.Drawing.Size(380, 292);
            this.listBox_DebugInfo.TabIndex = 3;
            // 
            // button_Time
            // 
            this.button_Time.Location = new System.Drawing.Point(12, 185);
            this.button_Time.Name = "button_Time";
            this.button_Time.Size = new System.Drawing.Size(89, 23);
            this.button_Time.TabIndex = 4;
            this.button_Time.Text = "加载时效面板";
            this.button_Time.UseVisualStyleBackColor = true;
            this.button_Time.Click += new System.EventHandler(this.button_Time_Click);
            // 
            // checkBox_Remote
            // 
            this.checkBox_Remote.AutoSize = true;
            this.checkBox_Remote.Location = new System.Drawing.Point(23, 12);
            this.checkBox_Remote.Name = "checkBox_Remote";
            this.checkBox_Remote.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Remote.TabIndex = 5;
            this.checkBox_Remote.Text = "远程模式";
            this.checkBox_Remote.UseVisualStyleBackColor = true;
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(107, 7);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(208, 21);
            this.textBox_IP.TabIndex = 6;
            this.textBox_IP.Text = "localhost:5474";
            // 
            // BSP_Example_Form
            // 
            this.ClientSize = new System.Drawing.Size(499, 345);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.checkBox_Remote);
            this.Controls.Add(this.button_Time);
            this.Controls.Add(this.listBox_DebugInfo);
            this.Controls.Add(this.button_LoadTool);
            this.Controls.Add(this.button_LoadUI);
            this.Controls.Add(this.button_Init);
            this.Name = "BSP_Example_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_Init;
        private System.Windows.Forms.Button button_LoadUI;
        private System.Windows.Forms.Button button_LoadTool;
        private System.Windows.Forms.ListBox listBox_DebugInfo;
        private System.Windows.Forms.Button button_Time;
        private System.Windows.Forms.CheckBox checkBox_Remote;
        private System.Windows.Forms.TextBox textBox_IP;
    }
}

