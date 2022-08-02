using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using HalconDotNet;
using System.Collections.Concurrent;
using System.IO;
using Utils;

namespace LeadframeAOI
{
    public partial class VisionUI : Form
    {
        public VisionUI()
        {
            InitializeComponent();
        }
        public HObject Image = new HObject();
        private void btnInitCamera_Click(object sender, EventArgs e)
        {
            //初始化相机
            //try
            //{
            //    if (Program.obj_Vision.camera.InitCamera()) MSG.Info("相机初始化成功!");
            //    else MSG.Error("相机初始化失败!");
            //}
            //catch (Exception err)
            //{
            //    MSG.Error("相机初始化失败!");
            //    return;
            //}
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            ////Vision.len = Convert.ToInt32(tbLen.Text.Trim());//配置单行扫描张数
            //Double exposure = Convert.ToDouble(tbExposure.Text.Trim());
            //Double gain = Convert.ToDouble(tbGain.Text.Trim());
            //Program.obj_Vision.camera.SetExposure(exposure);
            //Program.obj_Vision.camera.SetGain(gain);
            //if (!Program.obj_Vision.camera.OpenCamera())
            //{
            //    MSG.Error("打开相机失败!");
            //    return;
            //}
            //tbExposure.Enabled = false;
            //tbGain.Enabled = false;
            //Program.obj_Vision.camera.GetExposure(out exposure);
            //Program.obj_Vision.camera.GetGain(out gain);
            //tbExposure.Text = exposure.ToString();
            //tbGain.Text = gain.ToString();
            //MSG.Info("打开相机成功!");
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {

            //tbExposure.Enabled = true;
            //tbGain.Enabled = true;
            //if (Program.obj_Vision.camera.CloseCamera()) MSG.Info("关闭相机成功!");
            //else MSG.Error("关闭相机失败!");
        }




        private void VisionUI_Load(object sender, EventArgs e)
        {
            try
            {
                Program.obj_Vision.Read();
                tbCameraname.Text = Program.obj_Vision.cameraName0;
                //tbCameraname.Text = Program.obj_Vision.cameraNames[0];
                this.tbExposure.Text = Program.obj_Vision.obj_camera[0].exposure.ToString();
                this.tbGain.Text = Program.obj_Vision.obj_camera[0].gain.ToString();
                this.tbImageFolder.Text = Program.obj_Vision.imageFolder;
                tbExposure0.Text = Program.obj_Vision.obj_camera[0].Cur_Exposure[0].ToString();
                tbGain0.Text = Program.obj_Vision.obj_camera[0].Cur_Gain[0].ToString();
                tbExposure1.Text = Program.obj_Vision.obj_camera[0].Cur_Exposure[1].ToString();
                tbGain1.Text = Program.obj_Vision.obj_camera[0].Cur_Gain[1].ToString();
                tbExposure2.Text = Program.obj_Vision.obj_camera[0].Cur_Exposure[2].ToString();
                tbGain2.Text = Program.obj_Vision.obj_camera[0].Cur_Gain[2].ToString();
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
                if (Program.obj_Vision.obj_camera[0].camera.IsSoftwareTrigger)
                {
                    label7.BackColor = Color.Lime;
                    label7.Text = "触发模式：软触发";
                }
                else
                {
                    label7.BackColor = Color.Turquoise;
                    label7.Text = "触发模式：硬触发";
                }
            }
            catch (Exception ex)
            {
                MSG.Error("视觉模块参数初始化失败！\n" + ex.Message);
            }
        }

        private void btnVisionSave_Click(object sender, EventArgs e)
        {
            try
            {
                Program.obj_Vision.cameraName0 = tbCameraname.Text;
                Program.obj_Vision.obj_camera[0].exposure = Convert.ToDouble(this.tbExposure.Text.Trim());
                Program.obj_Vision.obj_camera[0].gain = Convert.ToDouble(this.tbGain.Text.Trim());
                Program.obj_Vision.obj_camera[0].SetExposure(Program.obj_Vision.obj_camera[0].exposure);
                Program.obj_Vision.obj_camera[0].SetGain(Program.obj_Vision.obj_camera[0].gain);
                Program.obj_Vision.obj_camera[0].Cur_Exposure[0] = Convert.ToDouble(tbExposure0.Text);
                Program.obj_Vision.obj_camera[0].Cur_Gain[0] = Convert.ToDouble(tbGain0.Text);
                Program.obj_Vision.obj_camera[0].Cur_Exposure[1] = Convert.ToDouble(tbExposure1.Text);
                Program.obj_Vision.obj_camera[0].Cur_Gain[1] = Convert.ToDouble(tbGain1.Text);
                Program.obj_Vision.obj_camera[0].Cur_Exposure[2] = Convert.ToDouble(tbExposure2.Text);
                Program.obj_Vision.obj_camera[0].Cur_Gain[2] = Convert.ToDouble(tbGain2.Text);
                Program.obj_Vision.Save();
                MSG.Info("保存成功！");
            }
            catch (Exception ex)
            {
                MSG.Error("视觉模块参数保存失败！\n" + ex.Message);
            }
        }

        private void btnConfigImagePath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog Fbd = new FolderBrowserDialog();
                if (Fbd.ShowDialog() == DialogResult.OK)
                {
                    tbImageFolder.Text = Fbd.SelectedPath;
                }
                Program.obj_Vision.imageFolder = tbImageFolder.Text.Trim();
                Program.obj_Vision.visionPath = Program.obj_Vision.imageFolder;
            }
            catch (Exception ex)
            {
                MSG.Error("图像存储目录保存失败！\n" + ex.Message);
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "TIFF图像|*.tiff|PNG图像|*.png|BMP图像|*.bmp|JPEG图像|*.jpg|所有文件|*.*";
                ofd.FilterIndex = 5;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(ofd.FileName)) return;
                    this.Image.Dispose();
                    HOperatorSet.ReadImage(out this.Image, ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MSG.Error("读取图片失败！\n" + ex.Message);
            }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();//首先根据打开文件对话框，选择excel表格
                sfd.InitialDirectory = Program.obj_Vision.imageFolder;
                sfd.Filter = "tiff files (*.tiff)|*.tiff|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(sfd.FileName)) return;
                    HOperatorSet.WriteImage(Image, "tiff", 0, sfd.FileName);
                    this.Image.Dispose();
                }
            }
            catch (Exception ex)
            {
                MSG.Error("储存图片失败！\n" + ex.Message);
            }
        }
        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnap.Enabled = false;
                btnGrab.Enabled = false;
                if (!Program.obj_Vision.obj_camera[0].camera.IsSoftwareTrigger)
                {
                    if (!Program.obj_Vision.obj_camera[0].ChangeTriggerSource(true))
                    {
                        MSG.Error("采集图片失败！\n" + "相机更改为软触发失败！");
                        return;
                    }
                    label7.BackColor = Color.Lime;
                    label7.Text = "触发模式：软触发";
                }

                Program.obj_Vision.Acq.Image.Dispose();
                Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[0].Snap(1, 5000);
                switch (Program.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        lbGrabStatus.BackColor = Color.Green;
                        this.Image.Dispose();
                        this.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                        HTuple imgNum;
                        HOperatorSet.CountChannels(this.Image, out imgNum);
                        lbGrabStatus.Text = "GrabPass:" + imgNum.TupleSum().I.ToString();
                        if (cbSaveImage.Checked == true)
                            HOperatorSet.WriteImage(this.Image, "tiff", 0, Program.obj_Vision.visionPath + "\\Snap.tiff");
                        Program.obj_Vision.Acq.Image.Dispose();
                        break;
                    default:
                        lbGrabStatus.BackColor = Color.Red;
                        lbGrabStatus.Text = "GrabFail:0";
                        break;
                }
            }
            catch (Exception ex)
            {
                MSG.Error("采集图片失败！\n" + ex.Message);
            }
            finally
            {
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
            }
        }
        private void btnGrab_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnap.Enabled = false;
                btnGrab.Enabled = false;

                if (!Program.obj_Vision.obj_camera[0].camera.IsSoftwareTrigger)
                {
                    if (!Program.obj_Vision.obj_camera[0].ChangeTriggerSource(true))
                    {
                        MSG.Error("启动连续采图失败！\n" + "相机更改为软触发失败！");
                        return;
                    }
                    label7.BackColor = Color.Lime;
                    label7.Text = "触发模式：软触发";
                }
                timer_Grab.Enabled = true;
            }
            catch (Exception ex)
            {
                MSG.Error("启动连续采图失败！\n" + ex.Message);
            }
        }

        private void btnStopGrab_Click(object sender, EventArgs e)
        {
            try
            {
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
            }
            catch (Exception ex)
            {
                MSG.Error("停止连续采图失败！\n" + ex.Message);
            }
        }

        private void timer_Grab_Tick(object sender, EventArgs e)
        {
            try
            {

                Program.obj_Vision.Acq.Image.Dispose();
                Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[0].Snap(1, 5000);
                switch (Program.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        this.Image.Dispose();
                        this.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                        HTuple imgNum;
                        HOperatorSet.CountChannels(this.Image, out imgNum);
                        lbGrabStatus.Text = "GrabPass:" + imgNum.TupleSum().I.ToString();
                        Program.obj_Vision.Acq.Image.Dispose();
                        break;
                    default:
                        lbGrabStatus.BackColor = Color.Red;
                        lbGrabStatus.Text = "GrabFail:0";
                        break;
                }
            }
            catch (Exception ex)
            {
                MSG.Error("连续采图失败！\n" + ex.Message);
            }
        }

        private void btnGetExposure_Click(object sender, EventArgs e)
        {
            try
            {
                Double exposure;
                Program.obj_Vision.obj_camera[0].GetExposure(out exposure);
                tbExposure.Text = exposure.ToString();
            }
            catch (Exception ex)
            {
                MSG.Error("获取曝光失败！\n" + ex.Message);
            }
        }

        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            try
            {
                Double exposure = Convert.ToDouble(tbExposure.Text);
                Program.obj_Vision.obj_camera[0].SetExposure(exposure);
            }
            catch (Exception ex)
            {
                MSG.Error("配置曝光失败！\n" + ex.Message);
            }
        }

        private void btnGetGain_Click(object sender, EventArgs e)
        {
            try
            {
                Double gain;
                Program.obj_Vision.obj_camera[0].GetGain(out gain);
                tbGain.Text = gain.ToString();
            }
            catch (Exception ex)
            {
                MSG.Error("获取增益失败！\n" + ex.Message);
            }
        }

        private void btnSetGain_Click(object sender, EventArgs e)
        {
            try
            {
                Double gain = Convert.ToDouble(tbGain.Text);
                Program.obj_Vision.obj_camera[0].SetGain(gain);
            }
            catch (Exception ex)
            {
                MSG.Error("配置增益失败！\n" + ex.Message);
            }
        }

        private void btnChgTrigSrc_Click(object sender, EventArgs e)
        {
            try
            {
                btnChgTrigSrc.Enabled = false;
                Program.obj_Vision.obj_camera[0].ChangeTriggerSource(!Program.obj_Vision.obj_camera[0].camera.IsSoftwareTrigger);
                if (Program.obj_Vision.obj_camera[0].camera.IsSoftwareTrigger)
                {
                    label7.BackColor = Color.Lime;
                    label7.Text = "触发模式：软触发";
                }
                else
                {
                    label7.BackColor = Color.Turquoise;
                    label7.Text = "触发模式：硬触发";
                }
                btnChgTrigSrc.Enabled = true;
            }
            catch (Exception ex)
            {
                MSG.Error("改变触发方式失败！\n" + ex.Message);
            }
        }


        private void VisionUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
                //Program.mainWindow.tbVision.Enabled = true;
            }
            catch (Exception ex)
            {
                MSG.Error("获取增益失败！\n" + ex.Message);
            }
        }

        private void btnGetEG_Click(object sender, EventArgs e)
        {
            try
            {
                tbExposure0.Text = Program.obj_Vision.obj_camera[0].Cur_Exposure[0].ToString();
                tbGain0.Text = Program.obj_Vision.obj_camera[0].Cur_Gain[0].ToString();
                tbExposure1.Text = Program.obj_Vision.obj_camera[0].Cur_Exposure[1].ToString();
                tbGain1.Text = Program.obj_Vision.obj_camera[0].Cur_Gain[1].ToString();
                tbExposure2.Text = Program.obj_Vision.obj_camera[0].Cur_Exposure[2].ToString();
                tbGain2.Text = Program.obj_Vision.obj_camera[0].Cur_Gain[2].ToString();
            }
            catch (Exception ex)
            {
                MSG.Error("获取各次拍照曝光增益失败！\n" + ex.Message);
            }
        }

        private void btnSetEG_Click(object sender, EventArgs e)
        {
            try
            {
                Program.obj_Vision.obj_camera[0].Cur_Exposure[0] = Convert.ToDouble(tbExposure0.Text);
                Program.obj_Vision.obj_camera[0].Cur_Gain[0] = Convert.ToDouble(tbGain0.Text);
                Program.obj_Vision.obj_camera[0].Cur_Exposure[1] = Convert.ToDouble(tbExposure1.Text);
                Program.obj_Vision.obj_camera[0].Cur_Gain[1] = Convert.ToDouble(tbGain1.Text);
                Program.obj_Vision.obj_camera[0].Cur_Exposure[2] = Convert.ToDouble(tbExposure2.Text);
                Program.obj_Vision.obj_camera[0].Cur_Gain[2] = Convert.ToDouble(tbGain2.Text);
            }
            catch (Exception ex)
            {
                MSG.Error("配置各次拍照曝光增益失败！\n" + ex.Message);
            }
        }

        private void btnInspectionConfig_Click(object sender, EventArgs e)
        {
            try
            {
                VisionFunctionConfig_UI visionFunctionConfig_UI = new VisionFunctionConfig_UI();
                visionFunctionConfig_UI.ShowDialog();
            }
            catch (Exception ex)
            {
                MSG.Error("打开检测算法参数配置页面失败！\n" + ex.Message);
            }
        }

        private void btnGetFocusRegion_Click(object sender, EventArgs e)
        {
            try
            {
                btnGetFocusRegion.Enabled = false;
                if (Process.focusPara == null)
                {
                    Process.focusPara = new Vision.AutoFocusParam();
                    Process.focusPara.focusRegion = new HObject();
                }
                HOperatorSet.WriteRegion(Process.focusPara.focusRegion, Application.StartupPath + "\\focusRegion.reg");
                btnGetFocusRegion.Enabled = true;
            }
            catch (Exception ex)
            {
                MSG.Error("获取聚焦区域失败！\n" + ex.Message);
            }
        }

        private void btnShowFocusRegion_Click(object sender, EventArgs e)
        {
            try
            {
                btnShowFocusRegion.Enabled = false;
                btnShowFocusRegion.Enabled = true;
            }
            catch (Exception ex)
            {
                MSG.Error("展示聚焦区域失败！\n" + ex.Message);
            }
        }


        private void btnfuctionTest_Click(object sender, EventArgs e)
        {
            try
            {
                FuctionTest fuctionTest = new FuctionTest();
                fuctionTest.ShowDialog();
            }
            catch (Exception ex)
            {
                MSG.Error("打开检测算法离线测试界面失败！\n" + ex.Message);
            }
        }

    }
}