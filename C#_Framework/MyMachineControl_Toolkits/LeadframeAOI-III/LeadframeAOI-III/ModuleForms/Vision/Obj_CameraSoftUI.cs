using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using HTHalControl;
using HalconDotNet;
using SelfControl;
using System.Threading;
using HT_Lib;

namespace LeadframeAOI.Modules.Vision
{
    public partial class Obj_CameraSoftUI : Form
    {
        public Obj_CameraSoftUI()
        {
            InitializeComponent();
        }

        public HObject Image = new HObject();

        private void Obj_CameraSoftUI_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (Obj_Camera obj_cam in App.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;

                for (int i = 0; i < App.obj_Vision.obj_camera.Count; i++)
                {
                    cmbCameraList.Items.Add("Camera_" + i.ToString());
                }
                if (App.obj_Vision.obj_camera.Count != 0)
                {
                    cmbCameraList.SelectedIndex = 0;
                    if (App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Camera.IsSoftwareTrigger)
                    {
                        label5.BackColor = Color.Lime;
                        label5.Text = "触发模式：软触发";
                    }
                    else
                    {
                        label5.BackColor = Color.Turquoise;
                        label5.Text = "触发模式：硬触发";
                    }
                    this.textBox_Exposure.Text = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].exposure.ToString();
                    this.textBox_Gain.Text = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].gain.ToString();
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取相机初始化数据失败！\n" + ex.ToString());
            }
        }

        private void cmbCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Camera.IsSoftwareTrigger)
                {
                    label5.BackColor = Color.Lime;
                    label5.Text = "触发模式：软触发";
                }
                else
                {
                    label5.BackColor = Color.Turquoise;
                    label5.Text = "触发模式：硬触发";
                }
                this.textBox_Exposure.Text = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].exposure.ToString();
                this.textBox_Gain.Text = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].gain.ToString();
                if (App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].isGrab)
                {
                    timer_Grab.Enabled = true;
                    btnSnap.Enabled = false;
                    btnGrab.Enabled = false;
                }
                else
                {
                    timer_Grab.Enabled = false;
                    btnSnap.Enabled = true;
                    btnGrab.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取该相机数据失败！\n" + ex.ToString());
            }
        }

        private void btnInitCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].InitCamera())
                {
                    HTUi.PopError("初始化相机失败！");
                    return;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("初始化相机失败！\n" + ex.ToString());
            }
        }

        private void btnGetExposure_Click(object sender, EventArgs e)
        {
            try
            {
                Double exposure;
                App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].GetExposure(out exposure);
                textBox_Exposure.Text = exposure.ToString();
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取曝光失败！\n" + ex.ToString());
            }
        }

        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            try
            {
                Double exposure = Convert.ToDouble(textBox_Exposure.Text);
                App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].SetExposure(exposure);
            }
            catch (Exception ex)
            {
                HTUi.PopError("配置曝光失败！\n" + ex.ToString());
            }
        }

        private void btnGetGain_Click(object sender, EventArgs e)
        {
            try
            {
                Double gain;
                App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].GetGain(out gain);
                textBox_Gain.Text = gain.ToString();
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取增益失败！\n" + ex.ToString());
            }
        }

        private void btnSetGain_Click(object sender, EventArgs e)
        {
            try
            {
                Double gain = Convert.ToDouble(textBox_Gain.Text);
                App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].SetGain(gain);
            }
            catch (Exception ex)
            {
                HTUi.PopError("配置增益失败！\n" + ex.ToString());
            }
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].OpenCamera())
                {
                    HTUi.PopError("打开相机失败！");
                    return;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("打开相机失败！\n" + ex.ToString());
            }
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].CloseCamera())
                {
                    HTUi.PopError("关闭相机失败！");
                    return;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("关闭相机失败！\n" + ex.ToString());
            }
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnap.Enabled = false;
                btnGrab.Enabled = false;
                App.obj_Vision.Acq.Image.Dispose();
                if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Camera.IsSoftwareTrigger)
                {
                    if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].ChangeTriggerSource(true))
                    {
                        HTUi.PopError("采集图片失败！\n" + "相机更改为软触发失败！");
                        return;
                    }
                }
                App.obj_Vision.Acq = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Snap(1, 5000);
                lbGrabStatus.Text = App.obj_Vision.Acq.GrabStatus + ":" + (App.obj_Vision.Acq.index + 1).ToString();
                switch (App.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        lbGrabStatus.BackColor = Color.Green;
                        //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                        this.Image.Dispose();
                        this.Image = App.obj_Vision.Acq.Image.CopyObj(1, -1);
                        FormJobs.Instance.htWindow.RefreshWindow(this.Image, null, "fit");
                        HTuple imgNum;
                        HOperatorSet.CountChannels(this.Image, out imgNum);
                        lbGrabStatus.Text = "GrabPass:" + imgNum.TupleSum().I.ToString();
                        App.obj_Vision.Acq.Image.Dispose();
                        break;
                    default:
                        lbGrabStatus.BackColor = Color.Red;
                        lbGrabStatus.Text = "GrabFail:0";
                        break;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("采集图片失败！\n" + ex.ToString());
            }
            finally
            {
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
            }
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            if (!timer_Grab.Enabled)
            {
                try
                {
                    btnSnap.Enabled = false;
                    //若硬触发，切为软触发
                    if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Camera.IsSoftwareTrigger)
                    {
                        if (!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].ChangeTriggerSource(true))
                        {
                            HTUi.PopError("启动连续采图失败！\n" + "相机更改为软触发失败！");
                            btnSnap.Enabled = true;
                            return;
                        }
                    }
                    timer_Grab.Enabled = true;
                    btnGrab.Text = "停止采集";
                    App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].isGrab = true;
                }
                catch (Exception ex)
                {
                    btnSnap.Enabled = true;
                    HTUi.PopError("启动连续采图失败！\n" + ex.ToString());
                }
            }
            else
            {
                try
                {
                    timer_Grab.Enabled = false;
                    btnGrab.Text = "连续采集";
                    App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].isGrab = false;
                    btnSnap.Enabled = true;
                }
                catch (Exception ex)
                {
                    HTUi.PopError("停止连续采图失败！\n" + ex.ToString());
                }
            }
        }


        private void timer_Grab_Tick(object sender, EventArgs e)
        {

        }
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                App.obj_Vision.SaveCameraData();
                for (int i = 0; i < Obj_Camera.Num_Camera; i++)
                {
                    if (App.obj_Vision.obj_camera[i].Camera == null) return;
                    App.obj_Vision.obj_camera[i].SetExposure(App.obj_Vision.obj_camera[i].exposure);
                    App.obj_Vision.obj_camera[i].SetGain(App.obj_Vision.obj_camera[i].gain);
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存数据失败！\n" + ex.ToString());
            }
        }

        private void Obj_CameraSoftUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                foreach (Obj_Camera obj_cam in App.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
            }
            catch (Exception ex)
            {
                HTUi.PopError("窗口关闭后保存数据失败！\n" + ex.ToString());
            }
        }

        private void btnChgTrigSrc_Click(object sender, EventArgs e)
        {
            try
            {
                btnChgTrigSrc.Enabled = false;
                App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].ChangeTriggerSource(!App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Camera.IsSoftwareTrigger);
                if (App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].Camera.IsSoftwareTrigger)
                {
                    label5.BackColor = Color.Lime;
                    label5.Text = "触发模式：软触发";
                }
                else
                {
                    label5.BackColor = Color.Turquoise;
                    label5.Text = "触发模式：硬触发";
                }
                btnChgTrigSrc.Enabled = true;
            }
            catch (Exception ex)
            {
                HTUi.PopError("改变触发方式失败！\n" + ex.ToString());
            }
        }
    }
}
