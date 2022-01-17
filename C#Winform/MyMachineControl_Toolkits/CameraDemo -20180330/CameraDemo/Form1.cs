using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _instance = this;
        }

        static Form1 _instance;
        public static Form1 GetForm1()
        {
            return _instance;
        }

        ToolKits.HTCamera.CameraMgr camera;
        ToolKits.HTCamera.CameraEnum cameraType;
        string cameraName = "";
        private void btnInitCamera_Click(object sender, EventArgs e)
        {
            if (tbCameraSN.Text == "")
            {
                MessageBox.Show("请添加相机序列号！");
                return;
            }
            cameraName = tbCameraSN.Text;
            camera = new ToolKits.HTCamera.CameraMgr(cameraType, cameraName);
            camera.IsSoftwareTrigger = true;
            //camera.SetPixelFormat(ToolKits.HTCamera.BaslerPixelFormat.Mono8);
            camera.SetPixelFormat(ToolKits.HTCamera.EuresysPixelFormat.Y8);
            if (camera.InitCamera()) MessageBox.Show("相机初始化成功!");
            else MessageBox.Show("相机初始化失败!");
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            if (!camera.OpenCamera())
            {
                MessageBox.Show("打开相机失败!");
                return;
            }
            double exposure, gain;
            camera.GetExposure(out exposure);
            camera.GetGain(out gain);
            camera.ChangeTriggerLight(false);
            tbExposure.Text = exposure.ToString();
            tbGain.Text = gain.ToString();
            MessageBox.Show("打开相机成功!");
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (camera.CloseCamera()) MessageBox.Show("关闭相机成功!");
            else MessageBox.Show("关闭相机失败!");
        }
        ToolKits.HTCamera.Acquisition acq = new ToolKits.HTCamera.Acquisition();
        private void btnSnap_Click(object sender, EventArgs e)
        {            
            acq.Image.Dispose();
            //HTCamera.Acqusition acq = await camera.GetFramesAsync(5, 3000);
            acq = camera.Snap(1, 5000);
            lbGrabStatus.Text = acq.GrabStatus + ":" + (acq.index + 1).ToString();
            switch (acq.GrabStatus)
            {
                case "GrabPass":
                    lbGrabStatus.BackColor = Color.Green;
                    //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                    this.htWindow.RefreshWindow(acq.Image, null, "");
                    HTuple imgNum;
                    HOperatorSet.CountChannels(acq.Image, out imgNum);
                    lbImgNum.Text = imgNum.TupleSum().I.ToString();
                    //HOperatorSet.WriteImage(acq.Image, "tiff", 0, "E:\\test.tiff");
                    break;
                default:
                    lbGrabStatus.BackColor = Color.Red;
                    lbImgNum.Text = "0";
                    break;
            }

            //lbGrabStatus.Text = "OK";
        }
        int Snap()
        {
            Thread.Sleep(5000);
            return 100;
        }
        ToolKits.HTCamera.Acquisition TestTask()
        {
            ToolKits.HTCamera.Acquisition acq = camera.GetFrames(5, 3000);
            lbGrabStatus.Text = acq.GrabStatus + ":" + (acq.index + 1).ToString();
            switch (acq.GrabStatus)
            {
                case "GrabPass":
                    lbGrabStatus.BackColor = Color.Green;
                    //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                    break;
                default:
                    lbGrabStatus.BackColor = Color.Red;
                    break;
            }
            acq.Image.Dispose();
            return acq;
        }

        private void btnGetExposure_Click(object sender, EventArgs e)
        {
            double exposure;
            camera.GetExposure(out exposure);
            tbExposure.Text = exposure.ToString();
        }

        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            double exposure = Convert.ToDouble(tbExposure.Text);
            bool flag = camera.SetExposure(exposure);
        }

        private void btnGetGain_Click(object sender, EventArgs e)
        {
            double gain;
            camera.GetGain(out gain);
            tbGain.Text = gain.ToString();
        }

        private void btnSetGain_Click(object sender, EventArgs e)
        {
            double gain = Convert.ToDouble(tbGain.Text);
            camera.SetGain(gain);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            cmbCameraType.Items.AddRange(Enum.GetNames(typeof(ToolKits.HTCamera.CameraEnum)));
            cmbCameraType.SelectedIndex = 6;
            
        }

        private void cmbCameraType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cameraType = (ToolKits.HTCamera.CameraEnum)Enum.Parse(typeof(ToolKits.HTCamera.CameraEnum), cmbCameraType.Text);
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            btnGrab.Text = timer1.Enabled ? "停止采集" : "连续采集";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnSnap_Click(sender, e);
        }
        Boolean SoftRoopFlag=false;
        Task doSoftRoop;
        double snapintvl=0;
        StreamWriter sw = null;
        int RoopIndex=0;
        private void btnSoftRoop_Click(object sender, EventArgs e)
        {
            SoftRoopFlag = !SoftRoopFlag;
            try
            {
                if (SoftRoopFlag)
                {
                    RoopIndex = 0;
                    btnSoftRoop.Text = "停止连采";
                    doSoftRoop = Task.Factory.StartNew(SoftRoop);
                    sw = new StreamWriter(@"D:\snapintvl.txt");
                }
                else
                {
                    sw.Close();
                    btnSoftRoop.Text = "无脑连采";
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void SoftRoop()
        {
            do
            {
                snapintvl = DateTime.Now.Ticks * 0.0000001;
                acq.Image.Dispose();
                //HTCamera.Acqusition acq = await camera.GetFramesAsync(5, 3000);
                acq = camera.Snap(1, 5000);
                lbGrabStatus.Text = acq.GrabStatus + ":" + (acq.index + 1).ToString();
                switch (acq.GrabStatus)
                {
                    case "GrabPass":
                        lbGrabStatus.BackColor = Color.Green;
                        //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                        this.htWindow.RefreshWindow(acq.Image, null, "");
                        HTuple imgNum;
                        HOperatorSet.CountChannels(acq.Image, out imgNum);
                        lbImgNum.Text = imgNum.TupleSum().I.ToString();
                        snapintvl = DateTime.Now.Ticks * 0.0000001 - snapintvl;
                        lbIntvl.Text = "采集间隔:"+snapintvl.ToString();
                        sw.WriteLine("第"+RoopIndex.ToString()+"次用时"+snapintvl.ToString());
                        RoopIndex++;
                        //HOperatorSet.WriteImage(acq.Image, "tiff", 0, "E:\\test.tiff");
                        break;
                    default:
                        sw.WriteLine("第" + RoopIndex.ToString() + "次：采集失败");
                        RoopIndex++;
                        lbGrabStatus.BackColor = Color.Red;
                        lbImgNum.Text = "0";
                        break;
                }
                sw.Flush();
            }
            while (SoftRoopFlag);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == camera.IsTriggerLight)
                if (!camera.ChangeTriggerLight(checkBox1.Checked))
                {
                    checkBox1.Checked = !checkBox1.Checked;
                    MessageBox.Show("更改是否触发光源失败！");
                    return;
                }
        }
    }
}
