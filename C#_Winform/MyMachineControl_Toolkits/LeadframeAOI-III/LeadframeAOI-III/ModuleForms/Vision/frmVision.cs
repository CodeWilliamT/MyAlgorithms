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
using HTHalControl;

namespace LeadframeAOI
{
    public partial class VisionUI : Form
    {
        public VisionUI()
        {
            InitializeComponent();
        }
        public HObject Image = new HObject();
        Panel paneltemp1, paneltemp2;
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
            Program.obj_Vision.Read();
            tbCameraname.Text=Program.obj_Vision.cameraName;
            this.tbExposure.Text = Program.obj_Vision.exposure.ToString();
            this.tbGain.Text = Program.obj_Vision.gain.ToString();
            this.tbImageFolder.Text = Program.obj_Vision.imageFolder;
            tbExposure1.Text = Program.obj_Vision.exposure1.ToString();
            tbGain1.Text = Program.obj_Vision.gain1.ToString();
            tbExposure2.Text = Program.obj_Vision.exposure2.ToString();
            tbGain2.Text = Program.obj_Vision.gain2.ToString();
            tbExposure3.Text = Program.obj_Vision.exposure3.ToString();
            tbGain3.Text = Program.obj_Vision.gain3.ToString();
            txtTestImageFolder.Text = Program.obj_Vision.TestImagePath;
            doGrab = false;
            btnSnap.Enabled = true;
            btnGrab.Enabled = true;
            if (Program.obj_Vision.Camera.IsSoftwareTrigger)
            {
                label7.BackColor = Color.Lime;
                label7.Text = "触发模式：软触发";
            }
            else
            {
                label7.BackColor = Color.Turquoise;
                label7.Text = "触发模式：硬触发";
            }
            try
            {
                paneltemp1 = new Panel();
                paneltemp1.Location = new Point(frmJobs.Instance.htWindow.Location.X + 32 + (frmJobs.Instance.htWindow.Size.Width - 32) / 2 - 1, frmJobs.Instance.htWindow.Location.Y + frmJobs.Instance.htWindow.Size.Height / 2 - 40);
                paneltemp1.Size = new Size(2, 80);
                paneltemp1.Enabled = false;
                paneltemp1.BackColor = Color.Green;
                frmJobs.Instance.htWindow.Controls.Add(paneltemp1);
                paneltemp1.BringToFront();
                paneltemp2 = new Panel();
                paneltemp2.Location = new Point(frmJobs.Instance.htWindow.Location.X + 32 + (frmJobs.Instance.htWindow.Size.Width - 32) / 2 - 40, frmJobs.Instance.htWindow.Location.Y + frmJobs.Instance.htWindow.Size.Height / 2 - 1);
                paneltemp2.Size = new Size(80, 2);
                paneltemp2.Enabled = false;
                paneltemp2.BackColor = Color.Green;
                frmJobs.Instance.htWindow.Controls.Add(paneltemp2);
                paneltemp2.BringToFront();
            }
            catch (Exception EXP)
            {
                LOG.Error(EXP);
            }

        }

        private void btnVisionSave_Click(object sender, EventArgs e)
        {
            Program.obj_Vision.cameraName = tbCameraname.Text;
            Program.obj_Vision.exposure=Convert.ToDouble(this.tbExposure.Text.Trim());
            Program.obj_Vision.gain = Convert.ToDouble(this.tbGain.Text.Trim());
            Program.obj_Vision.SetExposure(Program.obj_Vision.exposure);
            Program.obj_Vision.SetGain(Program.obj_Vision.gain);
            Program.obj_Vision.exposure1 = Convert.ToDouble(tbExposure1.Text);
            Program.obj_Vision.gain1 = Convert.ToDouble(tbGain1.Text);
            Program.obj_Vision.exposure2 = Convert.ToDouble(tbExposure2.Text);
            Program.obj_Vision.gain2 = Convert.ToDouble(tbGain2.Text);
            Program.obj_Vision.exposure3 = Convert.ToDouble(tbExposure3.Text);
            Program.obj_Vision.gain3 = Convert.ToDouble(tbGain3.Text);
            Program.obj_Vision.Save();
        }

        private void btnConfigImagePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                tbImageFolder.Text = Fbd.SelectedPath;
            }
            Program.obj_Vision.imageFolder = tbImageFolder.Text.Trim();
            Program.obj_Vision.visionPath = Program.obj_Vision.imageFolder;
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
                    frmJobs.Instance.htWindow.RefreshWindow(this.Image, null, "fit");
                }
            }
            catch
            {
                MSG.Error("读取图片失败！");
            }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();//首先根据打开文件对话框，选择excel表格
                Image = frmJobs.Instance.htWindow.Image.CopyObj(1, -1); ;
                sfd.InitialDirectory = Program.obj_Vision.imageFolder;
                sfd.Filter = "tiff files (*.tiff)|*.tiff|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(sfd.FileName)) return;
                    HOperatorSet.WriteImage(Image, "tiff", 0, sfd.FileName);
                    this.Image.Dispose();
                }
            }
            catch
            {
                MSG.Error("储存图片失败！");
            }
        }
        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                btnSnap.Enabled = false;
                btnGrab.Enabled = false;
                Program.obj_Vision.Acq.Image.Dispose();
                Program.obj_Vision.Acq = Program.obj_Vision.Snap(1, 5000);
                lbGrabStatus.Text = Program.obj_Vision.Acq.GrabStatus + ":" + (Program.obj_Vision.Acq.index + 1).ToString();
                switch (Program.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        lbGrabStatus.BackColor = Color.Green;
                        //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                        this.Image.Dispose();
                        this.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                        frmJobs.Instance.htWindow.RefreshWindow(this.Image, null, "fit");
                        HTuple imgNum;
                        HOperatorSet.CountChannels(this.Image, out imgNum);
                        lbImgNum.Text = imgNum.TupleSum().I.ToString();
                        if (cbSaveImage.Checked == true)
                            HOperatorSet.WriteImage(this.Image, "tiff", 0, Program.obj_Vision.visionPath+"\\Snap.tiff");
                        Program.obj_Vision.Acq.Image.Dispose();
                        break;
                    default:
                        lbGrabStatus.BackColor = Color.Red;
                        lbImgNum.Text = "0";
                        break;
                }
            }
            catch
            {
                MSG.Error("采集图片失败！");
            }
            finally
            {
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;
            }
        }
        bool doGrab=false;
        private void btnGrab_Click(object sender, EventArgs e)
        {
            try
            {
                doGrab = true;
                btnSnap.Enabled = false;
                btnGrab.Enabled = false;
                Task.Factory.StartNew(Grab);
            }
            catch
            {
                MSG.Error("采集图片失败！");
            }
        }

        private void btnStopGrab_Click(object sender, EventArgs e)
        {
            doGrab = false;
            btnSnap.Enabled = true;
            btnGrab.Enabled = true;
        }
        void Grab()
        {
            while (doGrab)
            {
                Program.obj_Vision.Acq.Image.Dispose();
                Program.obj_Vision.Acq = Program.obj_Vision.Snap(1, 5000);
                switch (Program.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                        this.Image.Dispose();
                        this.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                        Program.mainWindow.ShowImage(frmCaptureImage.Instance.htHalconTestImage, this.Image, null);
                        //HOperatorSet.WriteImage(acq.Image, "tiff", 0, "E:\\test.tiff");
                        Program.obj_Vision.Acq.Image.Dispose();
                        break;
                    default:
                        break;
                }
            }
        }
        private delegate void ShowImageDelegate(HTWindowControl htWindow, HObject image, HObject region);
        public void ShowImage(HTWindowControl htWindow, HObject image, HObject region)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowImageDelegate(ShowImage), new object[] { htWindow, image, region });
            }
            else
            {
                htWindow.ColorName = "red";
                htWindow.SetInteractive(false);
                htWindow.RefreshWindow(image, region, "");//可以不显示区域
                htWindow.SetInteractive(true);
                htWindow.ColorName = "green";
            }
        }
        private void btnGetExposure_Click(object sender, EventArgs e)
        {
            Double exposure;
            Program.obj_Vision.GetExposure(out exposure);
            tbExposure.Text = exposure.ToString();
        }

        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            Double exposure = Convert.ToDouble(tbExposure.Text);
            Program.obj_Vision.SetExposure(exposure);
        }

        private void btnGetGain_Click(object sender, EventArgs e)
        {
            Double gain;
            Program.obj_Vision.GetGain(out gain);
            tbGain.Text = gain.ToString();
        }

        private void btnSetGain_Click(object sender, EventArgs e)
        {
            Double gain = Convert.ToDouble(tbGain.Text);
            Program.obj_Vision.SetGain(gain);
        }

        private void btnChgTrigSrc_Click(object sender, EventArgs e)
        {
            btnChgTrigSrc.Enabled = false;
            Program.obj_Vision.Camera.ChangeTriggerSource(!Program.obj_Vision.Camera.IsSoftwareTrigger);
            if (Program.obj_Vision.Camera.IsSoftwareTrigger)
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


        private void VisionUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            doGrab = false;
            btnSnap.Enabled = true;
            btnGrab.Enabled = true;
            //Program.mainWindow.tbVision.Enabled = true;
        }

        private void btnGetEG_Click(object sender, EventArgs e)
        {
            tbExposure1.Text = Program.obj_Vision.exposure1.ToString();
            tbGain1.Text = Program.obj_Vision.gain1.ToString();
            tbExposure2.Text = Program.obj_Vision.exposure2.ToString();
            tbGain2.Text = Program.obj_Vision.gain2.ToString();
            tbExposure3.Text = Program.obj_Vision.exposure3.ToString();
            tbGain3.Text = Program.obj_Vision.gain3.ToString();
        }

        private void btnSetEG_Click(object sender, EventArgs e)
        {
            Program.obj_Vision.exposure1 = Convert.ToDouble(tbExposure1.Text);
            Program.obj_Vision.gain1 = Convert.ToDouble(tbGain1.Text);
            Program.obj_Vision.exposure2 = Convert.ToDouble(tbExposure2.Text);
            Program.obj_Vision.gain2 = Convert.ToDouble(tbGain2.Text);
            Program.obj_Vision.exposure3 = Convert.ToDouble(tbExposure3.Text);
            Program.obj_Vision.gain3 = Convert.ToDouble(tbGain3.Text);
        }

        private void btnInspectionConfig_Click(object sender, EventArgs e)
        {
            VisionFunctionConfig_UI visionFunctionConfig_UI = new VisionFunctionConfig_UI();
            visionFunctionConfig_UI.ShowDialog();
        }

        private void btnGetFocusRegion_Click(object sender, EventArgs e)
        {
            btnGetFocusRegion.Enabled = false;
            if (Process.focusPara == null)
            {
                Process.focusPara = new Vision.AutoFocusParam();
                Process.focusPara.focusRegion = new HObject();
            }
            ToolKits.FunctionModule.Vision.GenROI(frmJobs.Instance.htWindow, "region", ref Process.focusPara.focusRegion);
            HOperatorSet.WriteRegion(Process.focusPara.focusRegion, Application.StartupPath + "\\focusRegion.reg");
            btnGetFocusRegion.Enabled = true;
        }

        private void btnShowFocusRegion_Click(object sender, EventArgs e)
        {
            btnShowFocusRegion.Enabled = false;
            frmJobs.Instance.htWindow.RefreshWindow(this.Image, Process.focusPara.focusRegion, "fit");
            btnShowFocusRegion.Enabled = true;
        }

        private void cbSaveImage_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnConfigTestPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
             txtTestImageFolder.Text = Fbd.SelectedPath;
            }
            Program.obj_Vision.TestImagePath = txtTestImageFolder.Text.Trim();
        }

        private void btnfuctionTest_Click(object sender, EventArgs e)
        {
            FuctionTest fuctionTest = new FuctionTest();
            fuctionTest.ShowDialog();
        }
    }
}
