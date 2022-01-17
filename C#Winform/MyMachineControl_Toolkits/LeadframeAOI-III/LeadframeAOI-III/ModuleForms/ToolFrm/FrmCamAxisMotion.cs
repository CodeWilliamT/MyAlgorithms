using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;

namespace LeadframeAOI
{
    public partial class FrmCamAxisMotion : Form
    {
        #region 参数
        public static FrmCamAxisMotion Instance = null;
        double distance = 0;
        bool MoveEnable = true;
        #endregion
        public FrmCamAxisMotion()
        {
            InitializeComponent();
            this.htWindow.SetTablePanelVisible(false);
            distance = (double)numDistance.Value;
            for(int i=0;i<App.obj_Vision.obj_camera.Count;i++)
            {
                cbb_Cam.Items.Add("Camera_"+i);
            }
            timer1.Enabled = true;
            if (Obj_Camera.SelectedIndex > -1 && Obj_Camera.SelectedIndex < App.obj_Vision.obj_camera.Count)
                cbb_Cam.SelectedIndex = Obj_Camera.SelectedIndex;
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Instance = this;
        }

        private void numDistance_ValueChanged(object sender, EventArgs e)
        {
            distance = (double)numDistance.Value;
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (!MoveEnable) return;
            MoveEnable = false;
            if (!App.obj_Chuck.Y_RSMove(distance))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
            MoveEnable = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (!MoveEnable) return;
            MoveEnable = false;
            if (!App.obj_Chuck.Y_RSMove(-distance))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
            MoveEnable = true;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (!MoveEnable) return;
            MoveEnable = false;
            if (!App.obj_Chuck.X_RSMove(-distance))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
            MoveEnable = true;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (!MoveEnable) return;
            MoveEnable = false;
            if (!App.obj_Chuck.X_RSMove(distance))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
            MoveEnable = true;
        }

        private void btnZfocus_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.Z_Move(App.obj_Pdt.ZFocus))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
        }

        private void btnZsafe_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.MoveToSafeZPos())
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = "X(mm):"+App.obj_Chuck.GetXPos().ToString("F3");
            label3.Text = "Y(mm):"+App.obj_Chuck.GetYPos().ToString("F3");
            label4.Text = "Z(mm):"+App.obj_Chuck.GetZPos().ToString("F3");
        }

        private void FrmCamAxisMotion_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Space:
                    btnCapture_Click(null, null);
                    break;
                case Keys.A:
                    btnLeft_Click(null, null);
                    break;
                case Keys.W:
                    btnUp_Click(null, null);
                    break;
                case Keys.D:
                    btnRight_Click(null, null);
                    break;
                case Keys.S:
                    btnDown_Click(null, null);
                    break;
                case Keys.Q:
                    btnZUp_Click(null, null);
                    break;
                case Keys.E:
                    btnZDown_Click(null, null);
                    break;
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
                App.obj_Chuck.SWPosTrig();
                //4. 取图
                HOperatorSet.GenEmptyObj(out App.obj_Vision.Image);
                string errStr = "";
                HTuple width, height;
                //HOperatorSet.ReadImage(out App.obj_Vision.Image, "D:\\123.png");
                errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref App.obj_Vision.Image, 1, 5000);//获取图片
                HOperatorSet.GetImageSize(App.obj_Vision.Image, out width, out height);
                var a = width / 2;
                var b = height / 2;
                HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion,b, a, width / 20, 0);
                App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
                if (errStr != "")
                {
                    MessageBox.Show(errStr);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnZUp_Click(object sender, EventArgs e)
        {
            if (!MoveEnable) return;
            MoveEnable = false;
            if (!App.obj_Chuck.Z_RSMove(distance))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
            MoveEnable = true;
        }

        private void btnZDown_Click(object sender, EventArgs e)
        {
            if (!MoveEnable) return;
            MoveEnable = false;
            if (!App.obj_Chuck.Z_RSMove(-distance))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
            MoveEnable = true;
        }
        
        private void btnConfig_Cam_Click(object sender, EventArgs e)
        {
            if (cbb_Cam.SelectedIndex > -1 && cbb_Cam.SelectedIndex < App.obj_Vision.obj_camera.Count)
            {
                Obj_Camera.SelectedIndex = cbb_Cam.SelectedIndex;
                App.obj_Vision.SaveCameraData();
            }
        }

        private void btnSaveSnap_Click(object sender, EventArgs e)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            Directory.CreateDirectory(App.obj_Vision.imageFolder + "\\Snap");
            HOperatorSet.WriteImage(App.obj_Vision.Image, "tiff", 0, App.obj_Vision.imageFolder + "\\Snap\\" + fileName + ".tiff");
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(App.obj_Vision.imageFolder + "\\Snap");
            System.Diagnostics.Process.Start("explorer.exe", App.obj_Vision.imageFolder + "\\Snap");
        }
    }
}
