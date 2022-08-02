using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HT_Lib;
using HalconDotNet;


namespace LeadframeAOI
{
    public partial class frmChuckModules : Form
    {
        public static frmChuckModules Instance = null;
        public frmChuckModules()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            Instance = this;
        }
        public void SetupUI()
        {
            //tbCCodeX.Text = App.obj_Chuck.x_ScanCode.ToString("F3");
            //tbCCodeY.Text = App.obj_Chuck.y_ScanCode.ToString("F3");
            //tbCCodeZ.Text = App.obj_Chuck.z_ScanCode.ToString("F3");

            //tbCLoadX.Text = App.obj_Chuck.x_Load.ToString("F3");
            //tbCUnloadX.Text = App.obj_Chuck.x_Unload.ToString("F3");
            //tbCSafeZ.Text = App.obj_Chuck.z_Safe.ToString("F3");
          

            tbSCalibX.Text = App.obj_Chuck.x_Calib.ToString("F3");
            tbSCalibY.Text = App.obj_Chuck.y_Calib.ToString("F3");
            tbSCalibZ.Text = App.obj_Chuck.z_Calib.ToString("F3");

            tbSNumUsed.Text = App.obj_Chuck.calib_usedNum.ToString();
            tbSCalibNum.Text = App.obj_Chuck.calibNum.ToString();
            tbSXrange.Text = App.obj_Chuck.x_range.ToString();
            tbSYrange.Text = App.obj_Chuck.y_range.ToString();

            //tbCRefX.Text = App.obj_Chuck.ref_x.ToString("F3");
            //tbCRefY.Text = App.obj_Chuck.ref_y.ToString("F3");
            //tbCRefZ.Text = App.obj_Chuck.ref_z.ToString("F3");

            //txtMarkX.Text = App.obj_Chuck.X_MarkingPen.ToString("F3");
            //txtMarkY.Text = App.obj_Chuck.Y_MarkingPen.ToString("F3");
            //txtMarkZ.Text = App.obj_Chuck.AZMarkingPen.ToString("F3");
            propertyGrid1.SelectedObject = App.obj_Chuck;

        }

        /// <summary>
        /// 使得所有按钮不可用
        /// </summary>
        /// <param name="enable"></param>
        private void SetButtonEnadble(bool enable)
        {
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                ctrl.Enabled = enable;
            }
        }
        private void btnChuckSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    App.obj_Chuck.x_Load = Convert.ToDouble(tbCLoadX.Text);
            //    App.obj_Load.x_ChuckLoadFramePos = Convert.ToDouble(tbCLoadX.Text);
            //    App.obj_Chuck.x_Unload = Convert.ToDouble(tbCUnloadX.Text);
            //    App.obj_Chuck.x_ScanCode = Convert.ToDouble(tbCCodeX.Text);
            //    App.obj_Chuck.y_ScanCode = Convert.ToDouble(tbCCodeY.Text);
            //    App.obj_Chuck.z_ScanCode = Convert.ToDouble(tbCCodeZ.Text);
            //    App.obj_Chuck.ref_x = Convert.ToDouble(tbCRefX.Text);
            //    App.obj_Chuck.ref_y = Convert.ToDouble(tbCRefY.Text);
            //    App.obj_Chuck.ref_z = Convert.ToDouble(tbCRefZ.Text);
            //    App.obj_Chuck.X_MarkingPen = Convert.ToDouble(txtMarkX.Text);
            //    App.obj_Chuck.Y_MarkingPen = Convert.ToDouble(txtMarkY.Text);
            //    App.obj_Chuck.AZMarkingPen = Convert.ToDouble(txtMarkZ.Text);
            //    App.obj_Chuck.calMarkPenOffset();
            //    //yxy
            //    App.obj_Chuck.z_Safe = Convert.ToDouble(tbCSafeZ.Text);
            //}
            //catch (Exception exp)
            //{
            //    HTUi.PopError(exp.ToString());
            //    return;
            //}
            if (App.obj_Chuck.Save())
            {
                HTUi.TipHint("保存成功!");
            }
            else
            {
                HTUi.PopError(App.obj_Chuck.GetLastErrorString());
            }
        }
        

     

        private async void btnChuckHome_Click(object sender, EventArgs e)
        {
            
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Chuck.Home();
            });
            Form_Wait.CloseForm();
            if (!result)
            {
                HTUi.PopError("【检测台模块】"+App.obj_Chuck.GetLastErrorString());
                return;
            }
            HTUi.TipHint("【检测台模块】回零成功!");
            HTLog.Info("【检测台模块】回零成功!");
        }
        private void btnMoveToSafeZ_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.MoveToSafeZPos())
            {
                HTUi.PopError(App.obj_Chuck.GetLastErrorString());
                return;
            }
            HTUi.TipHint("Z轴移动到安全位成功");
        }
     
  

        private void btnCM2TrigStartZ_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.MoveToZStart())
            {
                HTUi.PopError("Z轴移动至聚焦起始点失败！");
                return;
            }
        }

        private void btnCM2TrigEndZ_Click(object sender, EventArgs e)
        {
            //if (!App.obj_Chuck.Move2ZEnd())
            //{
            //    HTUi.PopError("Z轴移动至聚焦终止点失败！");
            //    return;
            //}
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (HTM.LoadUI() < 0)
            {
                HTUi.PopError("打开轴调试助手界面失败");
            }
        }
        

        private void btnMoveMarkXy_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.MoveMarkingPenXy())
            {
                HTUi.PopError("移动至参考点位置失败！");
                return;
            }
        }
        

        private void btnMoveMarkZ_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.MoveMarkingPenZ())
            {
                HTUi.PopError("移动至参考点位置失败！");
                return;
            }
        }
        

        private async void btnCatchFrame_Click(object sender, EventArgs e)
        {
            bool result = await Task.Run(() =>
            {
                return App.obj_Chuck.CatchFrame();
            });
            if (!result)
            {
                HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                return;
            }
            HTUi.TipHint("【检测台模块】固定料片成功!");
        }

        private async void btnLooseFrame_Click(object sender, EventArgs e)
        {
            bool result = await Task.Run(() =>
            {
                return App.obj_Chuck.LooseFrame();
            });
            if (!result)
            {
                HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                return;
            }
            HTUi.TipHint("【检测台模块】松开料片成功!");
        }

        private void btnScanCode2D_Click(object sender, EventArgs e)
        {
            //if (App.obj_ReadQRCode.SendMes(new byte[] { 0X16, 0X54, 0X0D }) == false)

            //{
            //    HTLog.Error("打开二维码扫描器串口失败！");
            //}
            //if (App.obj_ReadQRCode.ScannerWaitTime(30000) == false) //等待10秒钟
            //{
            //    App.obj_ReadQRCode.SendMes(new byte[] { 0X16, 0X55, 0X0D });
            //    HTLog.Error("手动输入二维码！");
            //    return;
            //    //App.obj_Chuck.CurrentModuleState = ModuleState.MANUAL_INPUT;
            //}
            //App.obj_ReadQRCode.SendMes(new byte[] { 0X16, 0X55, 0X0D });
            //App.obj_Vision.frameFolderName = Encoding.ASCII.GetString(App.obj_ReadQRCode.Buffer.ToArray());
            //HTLog.Info("二维码为:" + App.obj_Vision.frameFolderName);
            //if (App.obj_Vision.CreateFrameFolder() == false)
            //{
            //    HTUi.PopError("Create file failed！");
            //}
            if (!App.obj_Vision.ScanCode2D())
            {
                HTLog.Error("二维码识别失败!");
                return;
            }
            HTLog.Info("二维码为:" + App.obj_Vision.frameName);
        }

        private async void btnMoveToCode2D_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                //1.移动
                if (!App.obj_Chuck.XYZ_Move(App.obj_Pdt.X_Code2D, App.obj_Pdt.Y_Code2D,App.obj_Pdt.ZFocus)) return false;
                //2.拍照
                try
                {
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[0].CameraName].Camera.ClearImageQueue();
                    App.obj_Chuck.SWPosTrig();//触发
                                              //3.拍图
                    string errStr = App.obj_Vision.obj_camera[App.obj_ImageInformSet[0].CameraName].CaputreImages(ref App.obj_Vision.Image, 1, 1000);
                    if (errStr != "")
                    {
                        App.obj_Vision.Image.Dispose();
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
                //3.回安全位
                if (!App.obj_Chuck.MoveToSafeZPos())
                {
                    HTUi.PopError(App.obj_Chuck.GetLastErrorString());
                    return false;
                }
                return true;
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Chuck.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }

        private void colorButton1_Click(object sender, EventArgs e)
        {
            Form_InputCode2D.ShowDialogForm();
            if (Form_InputCode2D.Instance.DialogResult == DialogResult.OK)
            {
                HTLog.Info("二维码为:" + Form_InputCode2D.Code2D);
            }
            Form_InputCode2D.Instance.Dispose();
        }

        private void btnEc_Jet_Click(object sender, EventArgs e)
        {
            Form_Ec_Jet.ShowForm();
        }

        private void btnCmrAxisTool_Click(object sender, EventArgs e)
        {

            FrmCamAxisMotion frm = FrmCamAxisMotion.Instance;
            if (frm == null || frm.IsDisposed)
            {
                frm = new FrmCamAxisMotion();
                frm.TopMost = true;
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                frm.Show();
                frm.Location = new Point((SW - frm.Size.Width) / 2, (SH - frm.Size.Height) / 2);
            }
            else
            {
                frm.Activate();
            }
        }
    }
}
