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
using HalconDotNet;
using HTHalControl;
using System.Threading;
using HT_Lib;
using ToolKits.TemplateEdit;
using System.IO;
using IniDll;
using System.Reflection;

namespace LeadframeAOI
{
    public partial class FrmScanTest : Form
    {
        #region 参数
        public static FrmScanTest Instance = null;
        /// <summary>
        /// 拍照位位置信息
        /// </summary>
        public string mappingImgDir = "ScanImgDir";
        public HTuple snapM;
        public HTuple snapN;
        public double scanStartX = 0;
        public double scanStartY = 0;
        public double scanEndX = 0;
        public double scanEndY = 0;
        public int clipColNum = 0;
        public int clipRowNum = 0;
        
        bool ScanWork = false;
        int scan_index = 0;
        HTuple width = 5120, height = 5120;
        HObject Image, showRegion;
        MainTemplateForm mTmpLctMdlFrm = null;
        MainTemplateForm mTmpCheckPosMdlFrm = null;
        Form modelForm = null;
        private List<ImagePosition> ScanPostions;
        #endregion

        public FrmScanTest()
        {
            InitializeComponent();
            Instance = this;
        }

        public void SetupUI()
        {
            try
            {
                ScanPostions = new List<ImagePosition>();
                App.formIniConfig.LoadObj(this);
                numStartX.Value = (decimal)scanStartX;
                numStartY.Value = (decimal)scanStartY;
                numEndX.Value = (decimal)scanEndX;
                numEndY.Value = (decimal)scanEndY;
                numClipColNum.Value = (decimal)clipColNum;
                numClipRowNum.Value = (decimal)clipRowNum;
            }
            catch (Exception ex)
            {
                HTUi.PopError("" + ex.ToString());
            }
        }
        #region 方法

        /// <summary>
        /// 获取所有拍照位的拍摄位置信息   by TWL
        /// </summary>
        /// <returns></returns>
        public string GetImagePositions()
        {
            mappingImgDir = "MappingImgDir";
            //清除之前的点位
            if (ScanPostions != null)
            {
                ScanPostions.Clear();
            }
            else
            {
                ScanPostions = new List<ImagePosition>();
            }


            //1.根据die与die之间的关系计算得出在一个block里行方向需要几个拍照位、列方向需要几个拍照位
            //2.跟距标定后的结果，准确获取第一个block里所有die的位置信息
            //3.计算第一个block内的拍照位
            //4.根据block标定结果计算出所有的block拍照位信息
            //5.将所有拍照位按照以下顺序排列
            /*
                --> --> -->
                            |
                            |
                <-- <-- <--
              */
            //6.将所有拍照位所有的位置添加到list里 B=16 R=8 C=2 M=1 N=2
            double[] start = new double[2] { scanStartX, scanStartY };//new double[2] { -182.184, -6.053 };//左上角视野中心

            double distanceX = Math.Abs((scanEndX - scanStartX) / clipColNum);//distanceBlockX / App.obj_Pdt.ColumnNumber;
            double distanceY = Math.Abs((scanEndY - scanStartY) / clipRowNum);//(end[1] - start[1]) / (NumM - 1);
            int times_X = clipColNum;
            int times_Y = clipRowNum;
            ImagePosition imagePosition = new ImagePosition();
            imagePosition.z = App.obj_Pdt.ZFocus;

            //string sPath = App.obj_Vision.imageFolder + "\\" + mappingImgDir;
            //Directory.CreateDirectory(sPath);
            //IniDll.IniFiles config = new IniDll.IniFiles(sPath + "\\point.ini");
            imagePosition.b = 0;
            int step = 0;
            for (int i = 0; i < times_Y; i++)
            {
                imagePosition.r = i;
                imagePosition.y = start[1] - i * distanceY;
                //if (imagePosition.y < scanEndY) imagePosition.y = scanEndY;
                for (int j = 0; j < times_X; j++)
                {
                    imagePosition.c = j;
                    imagePosition.x = start[0] + j * distanceX;
                    //if (imagePosition.x > scanEndX) imagePosition.x = scanEndX;
                    ScanPostions.Add(imagePosition);
                    //X_motion.Append(imagePosition.x);
                    //Y_motion.Append(imagePosition.y);
                    //config.WriteString("ScanPoint", "step" +step.ToString(), i + "-" + j + "(" + imagePosition.x.ToString() + "," + imagePosition.y + ")");
                    step++;
                }
            }
            return "";
        }
        /// <summary>
        /// 同一个拍照位拍摄多张图 包含2d和3d采集到的图  by M.Bing
        /// </summary>
        /// <param name="b">所在block</param>
        /// <param name="r">所在行</param>
        /// <param name="c">所在列</param>
        /// <returns></returns>
        public string CaputreMultipleImages(ref ImageCache imageCache)
        {
            string errStr = "";
            if (App.obj_Vision.RunMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE)) return errStr;
            //4. 取图 
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();//触发
            Thread.Sleep(10);
            //3.拍图
            errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref imageCache._2dImage, 1, 1000);
            if (errStr != "")
            {
                imageCache.Dispose();
                return errStr;
            }
            imageCache.b = ScanPostions[scan_index].b;
            imageCache.r = ScanPostions[scan_index].r;
            imageCache.c = ScanPostions[scan_index].c;
            HObject ImageSelect = new HObject();
            HOperatorSet.SelectObj(imageCache._2dImage, out ImageSelect, 1);
            HOperatorSet.GetImageSize(ImageSelect, out width, out height);
            //string sPath = App.obj_Vision.imageFolder + "\\" + mappingImgDir;
            //Directory.CreateDirectory(sPath);

            int imageR = imageCache.r, imageC = imageCache.c;
            //Task.Run(() =>
            //{
            //    HOperatorSet.ZoomImageFactor(ImageSelect, out ImageSelect, App.obj_Vision.scaleFactor, App.obj_Vision.scaleFactor, "constant");
            //    HOperatorSet.WriteImage(ImageSelect, "tiff", 0, sPath + "\\" + imageR + "-" + imageC + ".tiff");
            //});
            return errStr;
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
                htWindow.ColorName = "green";
                htWindow.SetInteractive(false);
                if (htWindow.Image == null || !htWindow.Image.IsInitialized())
                    htWindow.RefreshWindow(image, region, "fit");//适应窗口
                else
                    htWindow.RefreshWindow(image, region, "");//可以不显示区域
                htWindow.SetInteractive(true);
            }
        }
        #endregion
        private void btnSnapMapStart_Click(object sender, EventArgs e)
        {
            if (App.obj_Chuck.Move2ImagePosition(scanStartX, scanStartY, App.obj_Pdt.ZFocus) == false)
            {
                HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                            scanStartX,
                                            scanStartY,
                                            App.obj_Pdt.ZFocus,
                                            App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                return;
            }
            if (App.obj_Vision.RunMode == 1) return;
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();
            //4. 取图

            HOperatorSet.GenEmptyObj(out Image);
            string errStr = "";
            errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref Image, 1, 5000);//获取图片
            ShowImage(htWindow, Image, null);
            if (errStr != "")
            {
                HTUi.PopError(errStr);
            }
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (HTM.LoadUI() < 0)
            {
                HTUi.PopError("打开轴调试助手界面失败");
            }
        }

        private void btnGenPosMap_Click(object sender, EventArgs e)
        {

        }

        private async void btnScanMapImg_Click(object sender, EventArgs e)
        {
            try
            {
                GetImagePositions();
                HTUi.TipHint("采集Map用点生成完成.");
                HTLog.Info("采集Map用点生成完成.");
            }
            catch (Exception ex)
            {
                HTUi.PopError("" + ex.ToString());
                return;
            }
            if (ScanWork)
            {
                ScanWork = false;
                btnScanMapImg.Text = "开始扫描";
            }
            else
            {
                string sPath = App.obj_Vision.imageFolder + "\\" + mappingImgDir;
                if (Directory.Exists(sPath)) Directory.Delete(sPath, true);
                ScanWork = true;
                btnScanMapImg.Text = "停止扫描";
                await Task.Run(() =>
                {
                    scan_index = 0;
                    if (ScanPostions == null) ScanPostions = new List<ImagePosition>();
                    string errStr = GetImagePositions();
                    if (errStr != "")
                    {
                        HTUi.PopError(string.Format("初始化扫描位置信息失败!,详细信息:{0}", App.obj_Vision.GetLastErrorString()));
                        return;
                    }
                    scan_index = 0;
                    while (ScanWork)
                    {
                        if (App.obj_Chuck.Move2ImagePosition(ScanPostions[scan_index].x, ScanPostions[scan_index].y, ScanPostions[scan_index].z) == false)
                        {
                            HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                                        ScanPostions[scan_index].b,
                                                        ScanPostions[scan_index].r,
                                                        ScanPostions[scan_index].c,
                                                        App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                            ScanWork = false;
                            btnScanMapImg.BeginInvoke(new MethodInvoker(() => { btnScanMapImg.Text = "采集Mapping用图"; }));
                            return;
                        }
                        ImageCache imageCache = new ImageCache();
                        errStr = CaputreMultipleImages(ref imageCache);
                        //App.obj_Process.qAllImageCache.Enqueue(imageCache);
                        if (errStr == "")//拍照完成
                        {
                            ShowImage(htWindow, imageCache._2dImage.SelectObj(1), null);
                        }
                        scan_index++;
                        if (scan_index >= ScanPostions.Count)
                        {
                            break;
                        }
                    }
                    if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                        App.obj_Chuck.ref_y ,
                        App.obj_Chuck.z_Safe))
                    {
                        HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                    }
                    HTUi.TipHint("采集结束.");
                    HTLog.Info("采集结束.");
                    scan_index = 0;
                    ScanWork = false;
                    btnScanMapImg.BeginInvoke(new MethodInvoker(() => { btnScanMapImg.Text = "开始扫描"; }));
                });
            }
        }
        

        private void btnSetXY2StartPos_Click(object sender, EventArgs e)
        {
            try
            {
                double nowX = App.obj_Chuck.GetXPos();
                double nowY = App.obj_Chuck.GetYPos();
                numStartX.Value = (decimal)nowX;
                numStartY.Value = (decimal)nowY;
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取当前点位失败！\n" + ex.ToString());
            }
        }
        
        private void btnSetXYEndPos_Click(object sender, EventArgs e)
        {
            try
            {
                double nowX = App.obj_Chuck.GetXPos();
                double nowY = App.obj_Chuck.GetYPos();
                numEndX.Value = (decimal)nowX;
                numEndY.Value = (decimal)nowY;
            }
            catch (Exception ex)
            {
                HTUi.PopError("获取当前点位失败！\n" + ex.ToString());
            }
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();
            //4. 取图
            HOperatorSet.GenEmptyObj(out Image);
            string errStr = "";
            errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref Image, 1, 5000);//获取图片
            ShowImage(htWindow, Image, null);
            if (errStr != "")
            {
                HTUi.PopError(errStr);
            }
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

        private void btnSavePara_Click(object sender, EventArgs e)
        {
            try
            {
                scanStartX = (double)numStartX.Value;
                scanStartY = (double)numStartY.Value;
                scanEndX = (double)numEndX.Value;
                scanEndY = (double)numEndY.Value;
                clipColNum = (int)numClipColNum.Value;
                clipRowNum = (int)numClipRowNum.Value;
                App.formIniConfig.SaveObj(this);
                HTUi.TipHint("保存成功");
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存失败。\n" + ex.ToString());
            }
        }
        
        private void btnSnapMapEnd_Click(object sender, EventArgs e)
        {
            if (App.obj_Chuck.Move2ImagePosition(scanEndX, scanEndY, App.obj_Pdt.ZFocus) == false)
            {
                HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                            scanEndX,
                                            scanEndY,
                                            App.obj_Pdt.ZFocus,
                                            App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                return;
            }
            if (App.obj_Vision.RunMode == 1) return;
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();
            //4. 取图

            HOperatorSet.GenEmptyObj(out Image);
            string errStr = "";
            errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref Image, 1, 5000);//获取图片
            ShowImage(htWindow, Image, null);
            if (errStr != "")
            {
                HTUi.PopError(errStr);
            }
        }
    }
}