using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using DLP_Lib;


namespace DLP_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            patCfg.currentRed = 100;
            patCfg.currentGreen = 100;
            patCfg.currentBlue = 100;
            patCfg.repeat = false;
            patCfg.framePeriod = 1200000;//微秒
            patCfg.exposurePeriod = 1000000;//微秒

            numericUpDown1.Value = patCfg.framePeriod;
            numericUpDown2.Value = patCfg.exposurePeriod;
            numericUpDown3.Value = patCfg.currentRed;
            numericUpDown4.Value = patCfg.currentGreen;
            numericUpDown5.Value = patCfg.currentBlue;
            checkBox1.Checked = patCfg.repeat;
            comboBox1.Items.Clear();
            foreach (string statusName in Enum.GetNames(typeof(LedColorEnum)))
            { 
                comboBox1.Items.Add(Enum.Parse(typeof(LedColorEnum), statusName)); 
            }
            comboBox1.SelectedIndex = 7;
            numericUpDown6.Value = 8;
        }
        private Int32 deviceIdx = 0;
        private String path = @"\\?\hid#vid_0451&pid_6401&mi_00#8&ad2cbc2&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
        private PatternConfigStruct patCfg = new PatternConfigStruct();
        private List<PatternSequenceStruct> patSeq = new List<PatternSequenceStruct>();

        public delegate void LOGInfoDelegate(string str);
        public void LOG(string str)
        {
            if (listBox1.InvokeRequired == true)
            {
                LOGInfoDelegate d = new LOGInfoDelegate(LOG);
                this.Invoke(d, str);
            }
            else
            {
                listBox1.Items.Add(DateTime.Now.ToLongTimeString() + ":" + str);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                listBox1.ClearSelected();
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (DLP.OpenByPath(deviceIdx, path) < 0)
            {
                label1.Text = "打开失败!";
            }
            else
            {
                label1.Text = "打开成功!";
            }
            LOG(label1.Text);
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            patCfg.currentRed = (byte)numericUpDown3.Value;
            patCfg.currentGreen = (byte)numericUpDown4.Value;
            patCfg.currentBlue = (byte)numericUpDown5.Value;
            patCfg.repeat = checkBox1.Checked;
            patCfg.framePeriod = (uint)numericUpDown1.Value;//微秒
            patCfg.exposurePeriod = (uint)numericUpDown2.Value;//微秒

            patSeq.Clear();
            PatternSequenceStruct _seq = new PatternSequenceStruct();

            _seq.ledColor = (LedColorEnum)comboBox1.SelectedIndex;
            _seq.patNum = 0;
            _seq.bitDepth = (int)numericUpDown6.Value;
            _seq.imageIdx = 0;
            patSeq.Add(_seq);

            //_seq.ledColor = LedColorEnum.Red;
            //_seq.patNum = 0;
            //_seq.bitDepth = 1;
            //_seq.imageIdx = 1;
            //patSeq.Add(_seq);

            //_seq.ledColor = LedColorEnum.Green;
            //_seq.patNum = 0;
            //_seq.bitDepth = 1;
            //_seq.imageIdx = 2;
            //patSeq.Add(_seq);

            //_seq.ledColor = LedColorEnum.Blue;
            //_seq.patNum = 0;
            //_seq.bitDepth = 1;
            //_seq.imageIdx = 3;
            //patSeq.Add(_seq);

            //_seq.ledColor = LedColorEnum.Yellow;
            //_seq.patNum = 0;
            //_seq.bitDepth = 1;
            //_seq.imageIdx = 4;
            //patSeq.Add(_seq);


            if (DLP.ConfigPatternSequence(deviceIdx, patCfg, patSeq) < 0)
            {
                label1.Text = ("参数配置失败!");
            }
            else
            {
                label1.Text = ("参数配置成功!");
            }
            LOG(label1.Text);
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (DLP.PatternDisplay(deviceIdx, DisplayAction.Start) < 0)
            {
                label1.Text = ("开始播放失败!");
            }
            else
            {
                label1.Text = ("开始播放成功!");
            }
            LOG(label1.Text);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (DLP.PatternDisplay(deviceIdx, DisplayAction.Stop) < 0)
            {
                label1.Text = ("停止播放失败!");
            }
            else
            {
                label1.Text = ("停止播放成功!");
            }
            LOG(label1.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (DLP.Close(deviceIdx) < 0)
            {
                label1.Text = ("关闭失败!");
            }
            else
            {
                label1.Text = ("关闭成功!");
            }
            LOG(label1.Text);
        }

    }
}
