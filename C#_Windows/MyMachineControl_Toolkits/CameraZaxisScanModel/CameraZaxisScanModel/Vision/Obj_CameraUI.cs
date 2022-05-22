using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTHalControl;
using HalconDotNet;
using SelfControl;

namespace CameraZaxisScanModel
{
    public partial class Obj_CameraUI : Form
    {
        public Obj_CameraUI()
        {
            InitializeComponent(); 
            try
            {
                foreach (Obj_Camera obj_cam in Program.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                cmbCameraList.Items.Clear();
                for (int i = 0; i < Program.obj_Vision.obj_camera.Count; i++)
                {
                    cmbCameraList.Items.Add("Camera_" + i.ToString());
                }
                cmbCameraList.SelectedIndex = 0;
                Obj_Camera.SelectedIndex = 0;
                textBox_SN.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraName;
                tbCameraPath.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath;
                foreach (Obj_Camera obj_cam in Program.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;

                if (Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
                {
                    label5.BackColor = Color.Lime;
                    label5.Text = "触发模式：软触发";
                }
                else
                {
                    label5.BackColor = Color.Turquoise;
                    label5.Text = "触发模式：硬触发";
                }
                this.textBox_Exposure.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].exposure.ToString();
                this.textBox_Gain.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].gain.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取相机初始化数据失败！\n" + ex.Message);
            }
            _instance = this;
        }
        public HObject Image = new HObject();


        static Obj_CameraUI _instance;

        public static Obj_CameraUI GetForm()
        {
            return _instance;
        }

        private void Obj_CameraUI_Load(object sender, EventArgs e)
        {
            
        }

        private void cmbCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Obj_Camera.SelectedIndex = cmbCameraList.SelectedIndex;
                textBox_SN.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraName;
                tbCameraPath.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath;
                foreach (Obj_Camera obj_cam in Program.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;

                if (Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
                {
                    label5.BackColor = Color.Lime;
                    label5.Text = "触发模式：软触发";
                }
                else
                {
                    label5.BackColor = Color.Turquoise;
                    label5.Text = "触发模式：硬触发";
                }
                this.textBox_Exposure.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].exposure.ToString();
                this.textBox_Gain.Text = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].gain.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取该相机数据失败！\n" + ex.Message);
            }
        }

        private void btnInitCamera_Click(object sender, EventArgs e)
        {
            try
            {
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraName = textBox_SN.Text;
                if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].InitCamera())
                {
                    MessageBox.Show("初始化相机失败！");
                    return;
                }
                MessageBox.Show("初始化相机成功！");
                Program.obj_Vision.SaveCameraData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化相机失败！\n" + ex.Message);
            }
        }



        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].OpenCamera())
                {
                    MessageBox.Show("打开相机失败！");
                    return;
                }
                MessageBox.Show("打开相机成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开相机失败！\n" + ex.Message);
            }
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CloseCamera())
                {
                    MessageBox.Show("关闭相机失败！");
                    return;
                }
                MessageBox.Show("关闭相机成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭相机失败！\n" + ex.Message);
            }
        }

        private void btnChgTrigSrc_Click(object sender, EventArgs e)
        {
            try
            {
                btnChgTrigSrc.Enabled = false;
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].ChangeTriggerSource(!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger);
                if (Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
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
                MessageBox.Show("改变触发方式失败！\n" + ex.Message);
            }
        }

        private void btnGetExposure_Click(object sender, EventArgs e)
        {
            try
            {
                Double exposure;
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].GetExposure(out exposure);
                textBox_Exposure.Text = exposure.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取曝光失败！\n" + ex.Message);
            }
        }

        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            try
            {
                Double exposure = Convert.ToDouble(textBox_Exposure.Text);
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].SetExposure(exposure);
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置曝光失败！\n" + ex.Message);
            }
        }

        private void btnGetGain_Click(object sender, EventArgs e)
        {
            try
            {
                Double gain;
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].GetGain(out gain);
                textBox_Gain.Text = gain.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取增益失败！\n" + ex.Message);
            }
        }

        private void btnSetGain_Click(object sender, EventArgs e)
        {
            try
            {
                Double gain = Convert.ToDouble(textBox_Gain.Text);
                Program.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].SetGain(gain);
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置增益失败！\n" + ex.Message);
            }
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                Program.obj_Vision.Acq.Image.Dispose();
                if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
                {
                    if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].ChangeTriggerSource(true))
                    {
                        MessageBox.Show("采集图片失败！\n" + "相机更改为软触发失败！");
                        return;
                    }
                    if (Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
                    {
                        label5.BackColor = Color.Lime;
                        label5.Text = "触发模式：软触发";
                    }
                    else
                    {
                        label5.BackColor = Color.Turquoise;
                        label5.Text = "触发模式：硬触发";
                    }
                }

                Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Snap(1, 5000);
                lbGrabStatus.Text = Program.obj_Vision.Acq.GrabStatus + ":" + (Program.obj_Vision.Acq.index + 1).ToString();
                switch (Program.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        lbGrabStatus.BackColor = Color.Green;
                        //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                        this.Image.Dispose();
                        this.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                        Program.mainWindow.htWindow.RefreshWindow(this.Image, null, "fit");
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
                MessageBox.Show("采集图片失败！\n" + ex.Message);
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
                    if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
                    {
                        if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].ChangeTriggerSource(true))
                        {
                            MessageBox.Show("启动连续采图失败！\n" + "相机更改为软触发失败！");
                            btnSnap.Enabled = true;
                            return;
                        }
                    }
                    timer_Grab.Enabled = true;
                    btnGrab.Text = "停止采集";
                    Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].isGrab = true;
                }
                catch (Exception ex)
                {
                    btnSnap.Enabled = true;
                    MessageBox.Show("启动连续采图失败！\n" + ex.Message);
                }
            }
            else
            {
                try
                {
                    timer_Grab.Enabled = false;
                    btnGrab.Text = "连续采集";
                    Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].isGrab = false;
                    btnSnap.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("停止连续采图失败！\n" + ex.Message);
                }
            }
        }
        private void timer_Grab_Tick(object sender, EventArgs e)
        {
            btnSnap_Click(btnGrab, e);
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraName = textBox_SN.Text;
                Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath = tbCameraPath.Text;
                Program.obj_Vision.SaveCameraData();
                for (int i = 0; i < Obj_Camera.Num_Camera; i++)
                {

                    if (Program.obj_Vision.obj_camera[i].camera == null) continue;
                    try
                    {
                        if (!Program.obj_Vision.obj_camera[i].camera.IsConnected) continue;
                    }
                    catch
                    {
                        continue;
                    }
                    Program.obj_Vision.obj_camera[i].SetExposure(Program.obj_Vision.obj_camera[i].exposure);
                    Program.obj_Vision.obj_camera[i].SetGain(Program.obj_Vision.obj_camera[i].gain);
                }
                MessageBox.Show("数据保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据失败！\n" + ex.Message);
            }
        }

        private void Obj_CameraUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                foreach (Obj_Camera obj_cam in Program.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("窗口关闭后保存数据失败！\n" + ex.Message);
            }
        }

        private void btnConfigFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                tbCameraPath.Text = Fbd.SelectedPath;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", tbCameraPath.Text);
        }

    }
}
