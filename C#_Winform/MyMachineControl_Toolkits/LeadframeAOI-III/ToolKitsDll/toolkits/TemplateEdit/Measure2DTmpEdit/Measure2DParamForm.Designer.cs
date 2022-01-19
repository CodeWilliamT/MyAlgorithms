namespace ToolKits.TemplateEdit.Measure2DTmpEdit
{
    partial class Measure2DParamForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudMeasureNums = new System.Windows.Forms.NumericUpDown();
            this.nudMeasureSigma = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMinScore = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMeasureThreshold = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudMeasureLen2 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudMeasureLen1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbMeasureSelect = new ControlWrap.ComboBoxEx();
            this.cbMeasureTransition = new ControlWrap.ComboBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.btnOK = new ControlWrap.ButtonEx();
            this.nudInstanceNums = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudStartAngle = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudEndAngle = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cbApplyOther = new SelfControl.VaryCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureNums)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureSigma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLen2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLen1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInstanceNums)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "测量矩形个数:";
            // 
            // nudMeasureNums
            // 
            this.nudMeasureNums.Location = new System.Drawing.Point(91, 16);
            this.nudMeasureNums.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudMeasureNums.Name = "nudMeasureNums";
            this.nudMeasureNums.Size = new System.Drawing.Size(75, 21);
            this.nudMeasureNums.TabIndex = 1;
            this.nudMeasureNums.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMeasureNums.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // nudMeasureSigma
            // 
            this.nudMeasureSigma.DecimalPlaces = 1;
            this.nudMeasureSigma.Location = new System.Drawing.Point(91, 50);
            this.nudMeasureSigma.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.nudMeasureSigma.Name = "nudMeasureSigma";
            this.nudMeasureSigma.Size = new System.Drawing.Size(75, 21);
            this.nudMeasureSigma.TabIndex = 3;
            this.nudMeasureSigma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMeasureSigma.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "高斯滤波系数:";
            // 
            // nudMinScore
            // 
            this.nudMinScore.DecimalPlaces = 1;
            this.nudMinScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMinScore.Location = new System.Drawing.Point(91, 85);
            this.nudMinScore.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinScore.Name = "nudMinScore";
            this.nudMinScore.Size = new System.Drawing.Size(75, 21);
            this.nudMinScore.TabIndex = 5;
            this.nudMinScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMinScore.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "匹配分数:";
            // 
            // nudMeasureThreshold
            // 
            this.nudMeasureThreshold.DecimalPlaces = 1;
            this.nudMeasureThreshold.Location = new System.Drawing.Point(254, 85);
            this.nudMeasureThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMeasureThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureThreshold.Name = "nudMeasureThreshold";
            this.nudMeasureThreshold.Size = new System.Drawing.Size(75, 21);
            this.nudMeasureThreshold.TabIndex = 11;
            this.nudMeasureThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMeasureThreshold.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "边缘梯度:";
            // 
            // nudMeasureLen2
            // 
            this.nudMeasureLen2.DecimalPlaces = 1;
            this.nudMeasureLen2.Location = new System.Drawing.Point(254, 50);
            this.nudMeasureLen2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureLen2.Name = "nudMeasureLen2";
            this.nudMeasureLen2.Size = new System.Drawing.Size(75, 21);
            this.nudMeasureLen2.TabIndex = 9;
            this.nudMeasureLen2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMeasureLen2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(177, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "测量矩形宽:";
            // 
            // nudMeasureLen1
            // 
            this.nudMeasureLen1.DecimalPlaces = 1;
            this.nudMeasureLen1.Location = new System.Drawing.Point(254, 16);
            this.nudMeasureLen1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureLen1.Name = "nudMeasureLen1";
            this.nudMeasureLen1.Size = new System.Drawing.Size(75, 21);
            this.nudMeasureLen1.TabIndex = 7;
            this.nudMeasureLen1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMeasureLen1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(177, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "测量矩形长:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "边缘选取:";
            // 
            // cbMeasureSelect
            // 
            this.cbMeasureSelect.ArrowColor = System.Drawing.Color.White;
            this.cbMeasureSelect.BackColor = System.Drawing.SystemColors.Control;
            this.cbMeasureSelect.BaseColor = System.Drawing.Color.Black;
            this.cbMeasureSelect.BorderColor = System.Drawing.Color.Black;
            this.cbMeasureSelect.DropDownHeight = 250;
            this.cbMeasureSelect.DropDownWidth = 105;
            this.cbMeasureSelect.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMeasureSelect.FormattingEnabled = true;
            this.cbMeasureSelect.IntegralHeight = false;
            this.cbMeasureSelect.Items.AddRange(new object[] {
            "all",
            "first",
            "last"});
            this.cbMeasureSelect.Location = new System.Drawing.Point(91, 188);
            this.cbMeasureSelect.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbMeasureSelect.Name = "cbMeasureSelect";
            this.cbMeasureSelect.Size = new System.Drawing.Size(75, 20);
            this.cbMeasureSelect.TabIndex = 31;
            // 
            // cbMeasureTransition
            // 
            this.cbMeasureTransition.ArrowColor = System.Drawing.Color.White;
            this.cbMeasureTransition.BackColor = System.Drawing.SystemColors.Control;
            this.cbMeasureTransition.BaseColor = System.Drawing.Color.Black;
            this.cbMeasureTransition.BorderColor = System.Drawing.Color.Black;
            this.cbMeasureTransition.DropDownHeight = 250;
            this.cbMeasureTransition.DropDownWidth = 105;
            this.cbMeasureTransition.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMeasureTransition.FormattingEnabled = true;
            this.cbMeasureTransition.IntegralHeight = false;
            this.cbMeasureTransition.Items.AddRange(new object[] {
            "all",
            "negative",
            "positive",
            "uniform"});
            this.cbMeasureTransition.Location = new System.Drawing.Point(91, 221);
            this.cbMeasureTransition.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbMeasureTransition.Name = "cbMeasureTransition";
            this.cbMeasureTransition.Size = new System.Drawing.Size(75, 20);
            this.cbMeasureTransition.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "边缘变化:";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Maroon;
            this.btnOK.BorderColor = System.Drawing.Color.Empty;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.GlassEffect = true;
            this.btnOK.Location = new System.Drawing.Point(254, 190);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Radius_All = 3;
            this.btnOK.Radius_BottomLeft = 3;
            this.btnOK.Radius_BottomRight = 3;
            this.btnOK.Radius_TopLeft = 3;
            this.btnOK.Radius_TopRight = 3;
            this.btnOK.Size = new System.Drawing.Size(75, 53);
            this.btnOK.TabIndex = 34;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // nudInstanceNums
            // 
            this.nudInstanceNums.Location = new System.Drawing.Point(254, 119);
            this.nudInstanceNums.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudInstanceNums.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInstanceNums.Name = "nudInstanceNums";
            this.nudInstanceNums.Size = new System.Drawing.Size(75, 21);
            this.nudInstanceNums.TabIndex = 36;
            this.nudInstanceNums.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudInstanceNums.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(177, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 35;
            this.label9.Text = "拟合个数:";
            // 
            // nudStartAngle
            // 
            this.nudStartAngle.DecimalPlaces = 1;
            this.nudStartAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudStartAngle.Location = new System.Drawing.Point(91, 119);
            this.nudStartAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudStartAngle.Name = "nudStartAngle";
            this.nudStartAngle.Size = new System.Drawing.Size(75, 21);
            this.nudStartAngle.TabIndex = 38;
            this.nudStartAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "起始角度/°:";
            // 
            // nudEndAngle
            // 
            this.nudEndAngle.DecimalPlaces = 1;
            this.nudEndAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudEndAngle.Location = new System.Drawing.Point(91, 154);
            this.nudEndAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudEndAngle.Name = "nudEndAngle";
            this.nudEndAngle.Size = new System.Drawing.Size(75, 21);
            this.nudEndAngle.TabIndex = 40;
            this.nudEndAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudEndAngle.Value = new decimal(new int[] {
            360,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 39;
            this.label11.Text = "终止角度/°:";
            // 
            // cbApplyOther
            // 
            this.cbApplyOther.AutoSize = true;
            this.cbApplyOther.BackColor = System.Drawing.Color.Transparent;
            this.cbApplyOther.Location = new System.Drawing.Point(182, 151);
            this.cbApplyOther.Name = "cbApplyOther";
            this.cbApplyOther.Size = new System.Drawing.Size(132, 16);
            this.cbApplyOther.TabIndex = 41;
            this.cbApplyOther.Text = "应用于其它同类模型";
            this.cbApplyOther.UseVisualStyleBackColor = false;
            // 
            // Measure2DParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 245);
            this.Controls.Add(this.cbApplyOther);
            this.Controls.Add(this.nudEndAngle);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.nudStartAngle);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.nudInstanceNums);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbMeasureTransition);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbMeasureSelect);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudMeasureThreshold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudMeasureLen2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudMeasureLen1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudMinScore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudMeasureSigma);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudMeasureNums);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Measure2DParamForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "2D测量参数";
            this.Load += new System.EventHandler(this.Measure2DParamForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureNums)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureSigma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLen2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLen1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInstanceNums)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMeasureNums;
        private System.Windows.Forms.NumericUpDown nudMeasureSigma;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMinScore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudMeasureThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudMeasureLen2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudMeasureLen1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ControlWrap.ComboBoxEx cbMeasureSelect;
        private ControlWrap.ComboBoxEx cbMeasureTransition;
        private System.Windows.Forms.Label label8;
        private ControlWrap.ButtonEx btnOK;
        private System.Windows.Forms.NumericUpDown nudInstanceNums;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudStartAngle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudEndAngle;
        private System.Windows.Forms.Label label11;
        private SelfControl.VaryCheckBox cbApplyOther;
    }
}