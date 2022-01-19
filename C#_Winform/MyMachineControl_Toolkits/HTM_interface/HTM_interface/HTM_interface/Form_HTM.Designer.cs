namespace HTM_interface
{
    partial class Form_HTM
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label_Message = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label_Lv_Inf = new System.Windows.Forms.Label();
            this.label_State = new System.Windows.Forms.Label();
            this.label_Lv_program = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(-3, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 539);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "轴配置/调试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(976, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IO配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(976, 510);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "其它设备";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label_Message
            // 
            this.label_Message.ForeColor = System.Drawing.Color.Blue;
            this.label_Message.Location = new System.Drawing.Point(12, 554);
            this.label_Message.Name = "label_Message";
            this.label_Message.Size = new System.Drawing.Size(33, 16);
            this.label_Message.TabIndex = 1;
            this.label_Message.Text = "消息:";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(721, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 26);
            this.button1.TabIndex = 2;
            this.button1.Text = "时效分析";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Location = new System.Drawing.Point(802, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 26);
            this.button2.TabIndex = 3;
            this.button2.Text = "串口助手";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.Location = new System.Drawing.Point(883, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 26);
            this.button3.TabIndex = 4;
            this.button3.Text = "小窗口";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label_Lv_Inf
            // 
            this.label_Lv_Inf.AutoEllipsis = true;
            this.label_Lv_Inf.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Lv_Inf.ForeColor = System.Drawing.Color.Blue;
            this.label_Lv_Inf.Location = new System.Drawing.Point(633, 9);
            this.label_Lv_Inf.Name = "label_Lv_Inf";
            this.label_Lv_Inf.Size = new System.Drawing.Size(52, 16);
            this.label_Lv_Inf.TabIndex = 0;
            this.label_Lv_Inf.Text = "版本信息";
            this.label_Lv_Inf.Click += new System.EventHandler(this.label_Lv_Inf_Click);
            // 
            // label_State
            // 
            this.label_State.ForeColor = System.Drawing.Color.Red;
            this.label_State.Location = new System.Drawing.Point(918, 554);
            this.label_State.Name = "label_State";
            this.label_State.Size = new System.Drawing.Size(52, 16);
            this.label_State.TabIndex = 5;
            this.label_State.Text = "脱机状态";
            // 
            // label_Lv_program
            // 
            this.label_Lv_program.ForeColor = System.Drawing.Color.Silver;
            this.label_Lv_program.Location = new System.Drawing.Point(393, 5);
            this.label_Lv_program.Name = "label_Lv_program";
            this.label_Lv_program.Size = new System.Drawing.Size(168, 23);
            this.label_Lv_program.TabIndex = 6;
            this.label_Lv_program.Text = "x64_1.00_2018.XX.XX";
            this.label_Lv_program.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_HTM
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(979, 575);
            this.Controls.Add(this.label_Lv_program);
            this.Controls.Add(this.label_State);
            this.Controls.Add(this.label_Lv_Inf);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_Message);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form_HTM";
            this.ShowInTaskbar = false;
            this.Text = "HTM_Form";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label_Message;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label_Lv_Inf;
        private System.Windows.Forms.Label label_State;
        private System.Windows.Forms.Label label_Lv_program;

    }
}