using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HT_Lib;
using HTHalControl;
using HalconDotNet;
using SelfControl;
using ToolKits.HTCamera;

namespace LeadframeAOI
{
    /// <summary>
    /// 外设信息输入界面，包括相机个数、相机名称、相机型号、相机序列号、光源序列号
    /// </summary>
    public partial class FormExternalDevices : Form
    {
        public static FormExternalDevices Instance = null;
        public FormExternalDevices()
        {
            InitializeComponent();
            Instance = this;
            //cmbCameraType.Items.AddRange(new object[] { "basler相机使用NI工具", "basler相机", "灰点相机", "SVS相机", "matrox 采集卡+basler相机" , "虚拟相机", "Euresys相机" });
        }
        public HObject Image = new HObject();

        public void SetupUI()
        {
            try
            {
                for (int i = 0; i < App.obj_Vision.obj_camera.Count; i++)
                {
                    listBox_Cam.Items.Add("Camera_" + i);
                }
                if(listBox_Cam.Items.Count!=0) listBox_Cam.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取相机初始化数据失败！\n" + ex.ToString());
            }
        }


        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_Vision.obj_camera[listBox_Cam.SelectedIndex].InitCamera())
                {
                    HTUi.PopError("初始化相机失败！");
                    return;
                }
                HTUi.TipHint("初始化相机成功！");
                HTLog.Info("初始化相机成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError("初始化相机失败！\n" + ex.ToString());
            }
        }


        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_Vision.obj_camera[listBox_Cam.SelectedIndex].OpenCamera())
                {
                    HTUi.PopError("打开相机失败！");
                    return;
                }
                HTUi.TipHint("打开相机成功！");
                HTLog.Info("打开相机成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError("打开相机失败！\n" + ex.ToString());
            }
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_Vision.obj_camera[listBox_Cam.SelectedIndex].CloseCamera())
                {
                    HTUi.PopError("关闭相机失败！");
                    return;
                }
                HTUi.TipHint("关闭相机成功！");
                HTLog.Info("关闭相机成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError("关闭相机失败！\n" + ex.ToString());
            }
        }

        /// <summary>
        /// 保存配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                //App.obj_ExternalDevices.LightSN = this.txtOptSn.Text.ToString();

                //if (App.obj_ExternalDevices.Save() == false)
                //{
                //    HTUi.PopError(App.obj_ExternalDevices.GetLastErrorString());
                //    return;
                //}
                //if (App.obj_light.Save() == false)
                //{
                //    HTUi.PopError(App.obj_ExternalDevices.GetLastErrorString());
                //    return;
                //}
                //App.obj_Vision.obj_camera[cmbCameraList.SelectedIndex].cameraType = (CameraEnum)cmbCameraType.SelectedIndex;
                App.obj_Vision.SaveCameraData();
                for (int i = 0; i < Obj_Camera.Num_Camera; i++)
                {
                    if (App.obj_Vision.obj_camera[i].Camera == null) break;
                    App.obj_Vision.obj_camera[i].SetExposure(App.obj_Vision.obj_camera[i].exposure);
                    App.obj_Vision.obj_camera[i].SetGain(App.obj_Vision.obj_camera[i].gain);
                }
                //App.obj_Vision.InitializeAllCamera();
                HTUi.TipHint("保存数据成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存数据失败！\n" + ex.ToString());
            }
        }

        private void listBox_Cam_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid_Cam.SelectedObject = App.obj_Vision.obj_camera[listBox_Cam.SelectedIndex];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                App.obj_Vision.obj_camera.Add(new Obj_Camera());
                listBox_Cam.Items.Add("Camera_" + (App.obj_Vision.obj_camera.Count - 1));
                if (App.obj_Vision.obj_camera.Count > 0) listBox_Cam.SelectedIndex = App.obj_Vision.obj_camera.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            try
            {
                if (App.obj_AlgApp.P.mainIcPara.Count > 0)
                {
                    int selectIdx = listBox_Cam.SelectedIndex;
                    listBox_Cam.SelectedIndex = 0;
                    listBox_Cam.Items.RemoveAt(selectIdx);
                    App.obj_Vision.obj_camera.RemoveAt(selectIdx);
                }
                else
                {
                    HTUi.PopError("没有元素可供删除！\n");
                }
                if (App.obj_Vision.obj_camera.Count > 0) listBox_Cam.SelectedIndex = App.obj_Vision.obj_camera.Count - 1;
            }
            catch (Exception ex)
            {
                HTUi.PopError("删除失败！\n" + ex.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            propertyGrid_Cam.Refresh();
        }
    }
}
