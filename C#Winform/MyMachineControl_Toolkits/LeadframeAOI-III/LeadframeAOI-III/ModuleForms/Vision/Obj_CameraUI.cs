using System;
using System.Windows.Forms;
using HalconDotNet;
using HT_Lib;

namespace LeadframeAOI.Modules.Vision
{
    public partial class Obj_CameraUI : Form
    {
        public Obj_CameraUI()
        {
            InitializeComponent();
        }
        public HObject Image = new HObject();

        private void Obj_CameraUI_Load(object sender, EventArgs e)
        {
            try
            {
                foreach(Obj_Camera obj_cam in App.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }

                for (int i = 0; i < App.obj_Vision.obj_camera.Count; i++)
                {
                    cmbCameraList.Items.Add("Camera_" + i.ToString());
                }
                if (App.obj_Vision.obj_camera.Count != 0)
                {
                    cmbCameraList.SelectedIndex = 0;
                    textBox_SN.Text = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].cameraName;
                }

            }
            catch(Exception ex)
            {
                HTUi.PopError("获取相机初始化数据失败！\n" + ex.ToString());
            }
        }

        private void cmbCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox_SN.Text = App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].cameraName;
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

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                App.obj_Vision.SaveCameraData();
                App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].cameraName = textBox_SN.Text;
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

        private void Obj_CameraUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                foreach (Obj_Camera obj_cam in App.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("窗口关闭后保存数据失败！\n" + ex.ToString());
            }
        }

    }
}
