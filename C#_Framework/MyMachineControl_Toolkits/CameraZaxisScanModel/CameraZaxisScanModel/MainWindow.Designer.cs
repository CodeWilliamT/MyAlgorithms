namespace CameraZaxisScanModel
{
    partial class MainWindow
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.htWindow = new HTHalControl.HTWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_listClear = new System.Windows.Forms.Button();
            this.button_HTMUI = new System.Windows.Forms.Button();
            this.numericUpDown_Delay = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_NowPos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_SavePic = new System.Windows.Forms.CheckBox();
            this.button_Move = new System.Windows.Forms.Button();
            this.numericUpDown_End = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Down = new System.Windows.Forms.Button();
            this.numericUpDown_Start = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Up = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown_distance = new System.Windows.Forms.NumericUpDown();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Delay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_distance)).BeginInit();
            this.SuspendLayout();
            // 
            // htWindow
            // 
            this.htWindow.BackColor = System.Drawing.Color.Transparent;
            this.htWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.htWindow.ColorName = "red";
            this.htWindow.ColorType = 0;
            this.htWindow.Column = null;
            this.htWindow.Column1 = null;
            this.htWindow.Column2 = null;
            this.htWindow.Image = null;
            this.htWindow.Length1 = null;
            this.htWindow.Length2 = null;
            this.htWindow.Location = new System.Drawing.Point(12, 12);
            this.htWindow.Name = "htWindow";
            this.htWindow.Phi = null;
            this.htWindow.Radius = null;
            this.htWindow.Radius1 = null;
            this.htWindow.Radius2 = null;
            this.htWindow.Region = null;
            this.htWindow.RegionType = null;
            this.htWindow.Row = null;
            this.htWindow.Row1 = null;
            this.htWindow.Row2 = null;
            this.htWindow.Size = new System.Drawing.Size(665, 487);
            this.htWindow.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(693, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 377);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_listClear);
            this.groupBox1.Controls.Add(this.button_HTMUI);
            this.groupBox1.Controls.Add(this.numericUpDown_Delay);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_NowPos);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.checkBox_SavePic);
            this.groupBox1.Controls.Add(this.button_Move);
            this.groupBox1.Controls.Add(this.numericUpDown_End);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_Down);
            this.groupBox1.Controls.Add(this.numericUpDown_Start);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button_Up);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDown_distance);
            this.groupBox1.Location = new System.Drawing.Point(693, 395);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 124);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主控";
            // 
            // button_listClear
            // 
            this.button_listClear.Location = new System.Drawing.Point(363, 83);
            this.button_listClear.Name = "button_listClear";
            this.button_listClear.Size = new System.Drawing.Size(81, 23);
            this.button_listClear.TabIndex = 18;
            this.button_listClear.Text = "清空信息框";
            this.button_listClear.UseVisualStyleBackColor = true;
            this.button_listClear.Click += new System.EventHandler(this.button_listClear_Click);
            // 
            // button_HTMUI
            // 
            this.button_HTMUI.Location = new System.Drawing.Point(450, 83);
            this.button_HTMUI.Name = "button_HTMUI";
            this.button_HTMUI.Size = new System.Drawing.Size(108, 23);
            this.button_HTMUI.TabIndex = 17;
            this.button_HTMUI.Text = "打开HTM配置界面";
            this.button_HTMUI.UseVisualStyleBackColor = true;
            this.button_HTMUI.Click += new System.EventHandler(this.button_HTMUI_Click);
            // 
            // numericUpDown_Delay
            // 
            this.numericUpDown_Delay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_Delay.Location = new System.Drawing.Point(207, 85);
            this.numericUpDown_Delay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_Delay.Name = "numericUpDown_Delay";
            this.numericUpDown_Delay.Size = new System.Drawing.Size(69, 21);
            this.numericUpDown_Delay.TabIndex = 16;
            this.numericUpDown_Delay.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "每次步进后延迟多少毫秒继续步进";
            // 
            // textBox_NowPos
            // 
            this.textBox_NowPos.Enabled = false;
            this.textBox_NowPos.Location = new System.Drawing.Point(74, 14);
            this.textBox_NowPos.Name = "textBox_NowPos";
            this.textBox_NowPos.Size = new System.Drawing.Size(100, 21);
            this.textBox_NowPos.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "当前位置";
            // 
            // checkBox_SavePic
            // 
            this.checkBox_SavePic.AutoSize = true;
            this.checkBox_SavePic.Location = new System.Drawing.Point(462, 54);
            this.checkBox_SavePic.Name = "checkBox_SavePic";
            this.checkBox_SavePic.Size = new System.Drawing.Size(96, 16);
            this.checkBox_SavePic.TabIndex = 12;
            this.checkBox_SavePic.Text = "每次步进拍照";
            this.checkBox_SavePic.UseVisualStyleBackColor = true;
            // 
            // button_Move
            // 
            this.button_Move.Location = new System.Drawing.Point(373, 51);
            this.button_Move.Name = "button_Move";
            this.button_Move.Size = new System.Drawing.Size(75, 23);
            this.button_Move.TabIndex = 10;
            this.button_Move.Text = "开始移动";
            this.button_Move.UseVisualStyleBackColor = true;
            this.button_Move.Click += new System.EventHandler(this.button_Move_Click);
            // 
            // numericUpDown_End
            // 
            this.numericUpDown_End.DecimalPlaces = 4;
            this.numericUpDown_End.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_End.Location = new System.Drawing.Point(257, 52);
            this.numericUpDown_End.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown_End.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            -2147483648});
            this.numericUpDown_End.Name = "numericUpDown_End";
            this.numericUpDown_End.Size = new System.Drawing.Size(82, 21);
            this.numericUpDown_End.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "移动至";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(344, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "步进距离";
            // 
            // button_Down
            // 
            this.button_Down.Location = new System.Drawing.Point(266, 13);
            this.button_Down.Name = "button_Down";
            this.button_Down.Size = new System.Drawing.Size(75, 23);
            this.button_Down.TabIndex = 6;
            this.button_Down.Text = "下移";
            this.button_Down.UseVisualStyleBackColor = true;
            this.button_Down.Click += new System.EventHandler(this.button_Down_Click);
            // 
            // numericUpDown_Start
            // 
            this.numericUpDown_Start.DecimalPlaces = 4;
            this.numericUpDown_Start.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_Start.Location = new System.Drawing.Point(116, 52);
            this.numericUpDown_Start.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown_Start.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            -2147483648});
            this.numericUpDown_Start.Name = "numericUpDown_Start";
            this.numericUpDown_Start.Size = new System.Drawing.Size(84, 21);
            this.numericUpDown_Start.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Z轴以步进距离从";
            // 
            // button_Up
            // 
            this.button_Up.Location = new System.Drawing.Point(185, 13);
            this.button_Up.Name = "button_Up";
            this.button_Up.Size = new System.Drawing.Size(75, 23);
            this.button_Up.TabIndex = 3;
            this.button_Up.Text = "上移";
            this.button_Up.UseVisualStyleBackColor = true;
            this.button_Up.Click += new System.EventHandler(this.button_Up_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "脉冲位";
            // 
            // numericUpDown_distance
            // 
            this.numericUpDown_distance.DecimalPlaces = 4;
            this.numericUpDown_distance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_distance.Location = new System.Drawing.Point(412, 13);
            this.numericUpDown_distance.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDown_distance.Name = "numericUpDown_distance";
            this.numericUpDown_distance.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown_distance.TabIndex = 1;
            this.numericUpDown_distance.Value = new decimal(new int[] {
            10000,
            0,
            0,
            262144});
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 525);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1254, 88);
            this.listBox1.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 625);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.htWindow);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Delay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_distance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public HTHalControl.HTWindowControl htWindow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown_distance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Up;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown_Start;
        private System.Windows.Forms.Button button_Down;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_End;
        private System.Windows.Forms.Button button_Move;
        private System.Windows.Forms.CheckBox checkBox_SavePic;
        private System.Windows.Forms.TextBox textBox_NowPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_Delay;
        private System.Windows.Forms.Button button_HTMUI;
        private System.Windows.Forms.Button button_listClear;
    }
}

