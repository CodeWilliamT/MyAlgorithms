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
using ToolKits.RegionModify;
using VisionMethonDll;

namespace LeadframeAOI
{
    public partial class FrmAutoMapping : Form
    {
        #region 参数
        public static FrmAutoMapping Instance = null;
        /// <summary>
        /// 拍照位位置信息
        /// </summary>
        public string mappingImgDir = "MappingImgDir";
        public HTuple snapM;
        public HTuple snapN;



        HTuple width = 5120, height = 5120;
        HObject Image, showRegion;
        MainTemplateForm mTmpLctMdlFrm = null;
        MainTemplateForm mTmpCheckPosMdlFrm = null;
        Form_FixMapPos mTmpFixClipPosFrm = null;
        Form toolForm = null;
        Model LoctionModels;
        private List<ImagePosition> GenMapPostions;
        HTuple X_motion = null;
        HTuple Y_motion = null;
        //HTuple mapU;
        //HTuple mapV;
        HTuple hv_iFlag;
        #endregion

        public FrmAutoMapping()
        {
            InitializeComponent();
            Instance = this;
        }

        public void SetupUI()
        {
            try
            {
                GenMapPostions = new List<ImagePosition>();
                App.formIniConfig.LoadObj(this);
                numStartX.Value = (decimal)App.obj_Vision.genMapStartX;
                numStartY.Value = (decimal)App.obj_Vision.genMapStartY;
                numEndX.Value = (decimal)App.obj_Vision.genMapEndX;
                numEndY.Value = (decimal)App.obj_Vision.genMapEndY;
                numSameSpace.Value = (decimal)App.obj_Vision.sameSpace;
                numWidthFactor.Value = (decimal)App.obj_Vision.widthFactor;
                numHeightFactor.Value = (decimal)App.obj_Vision.heightFactor;
                numScaleFactor.Value = (decimal)App.obj_Vision.scaleFactor;
                numLctScoreThresh.Value = (decimal)App.obj_Vision.lctScoreThresh;
                numCheckPosX.Value = (decimal)App.obj_Vision.checkPosX;
                numCheckPosY.Value = (decimal)App.obj_Vision.checkPosY;
                numCheckPosScoreThresh.Value = (decimal)App.obj_Vision.checkPosScoreThresh;
            }
            catch (Exception ex)
            {
                HTUi.PopError(this.GetType().Name + "UI加载失败！\n" + ex.ToString());
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
            if (GenMapPostions != null)
            {
                GenMapPostions.Clear();
            }
            else
            {
                GenMapPostions = new List<ImagePosition>();
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
            if(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex].Type==HTupleType.INTEGER)
            {
                throw new Exception("请先标定该相机！");
            }
            //6.将所有拍照位所有的位置添加到list里 B=16 R=8 C=2 M=1 N=2
            double[] start = new double[2] { App.obj_Vision.genMapStartX, App.obj_Vision.genMapStartY };//new double[2] { -182.184, -6.053 };//左上角视野中心
            double viewWidth = Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D * width.D);
            double viewHeight = Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][3].D * height.D);

            double distanceX = viewWidth - App.obj_Vision.sameSpace;//distanceBlockX / App.obj_Pdt.ColumnNumber;
            double distanceY = viewHeight - App.obj_Vision.sameSpace;//(end[1] - start[1]) / (NumM - 1);
            int times_X = (int)(Math.Abs(App.obj_Vision.genMapEndX - App.obj_Vision.genMapStartX) / distanceX) + 2;
            int times_Y = (int)(Math.Abs(App.obj_Vision.genMapEndY - App.obj_Vision.genMapStartY) / distanceY) + 2;
            ImagePosition imagePosition = new ImagePosition();
            imagePosition.z = App.obj_Pdt.ZFocus;

            string sPath = App.obj_Vision.imageFolder + "\\" + mappingImgDir;
            Directory.CreateDirectory(sPath);
            IniDll.IniFiles config = new IniDll.IniFiles(sPath + "\\point.ini");
            imagePosition.b = 0;
            int step = 0;
            for (int i = 0; i < times_Y; i++)
            {
                imagePosition.r = i;
                imagePosition.y = start[1] - i * distanceY;
                //if (imagePosition.y < App.obj_Vision.genMapEndY) imagePosition.y = App.obj_Vision.genMapEndY;
                for (int j = 0; j < times_X; j++)
                {
                    imagePosition.c = j;
                    imagePosition.x = start[0] + j * distanceX;
                    //if (imagePosition.x > App.obj_Vision.genMapEndX) imagePosition.x = App.obj_Vision.genMapEndX;
                    GenMapPostions.Add(imagePosition);
                    //X_motion.Append(imagePosition.x);
                    //Y_motion.Append(imagePosition.y);
                    config.WriteString("ScanPoint", "step" + step.ToString(), i + "-" + j + "(" + imagePosition.x.ToString() + "," + imagePosition.y + ")");
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
            double nowX = App.obj_Chuck.GetXPos();
            double nowY = App.obj_Chuck.GetYPos();
            X_motion.Append(nowX);
            Y_motion.Append(nowY);
            imageCache.b = GenMapPostions[scan_index].b;
            imageCache.r = GenMapPostions[scan_index].r;
            imageCache.c = GenMapPostions[scan_index].c;
            HObject ImageSelect = new HObject();
            App.obj_Vision.scaleFactor = Convert.ToDouble(numScaleFactor.Value);
            HOperatorSet.SelectObj(imageCache._2dImage, out ImageSelect, 1);
            HOperatorSet.GetImageSize(ImageSelect, out width, out height);
            string sPath = App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + mappingImgDir;

            int imageR = imageCache.r, imageC = imageCache.c;
            Task.Run(() =>
            {
                HOperatorSet.ZoomImageFactor(ImageSelect, out ImageSelect, App.obj_Vision.scaleFactor, App.obj_Vision.scaleFactor, "constant");
                HOperatorSet.WriteImage(ImageSelect, "tiff", 0, sPath + "\\" + imageR + "-" + imageC + ".tiff");
            });
            return errStr;
        }

        #endregion
        private void btnSnapMapStart_Click(object sender, EventArgs e)
        {
            if (App.obj_Chuck.Move2ImagePosition(App.obj_Vision.genMapStartX, App.obj_Vision.genMapStartY, App.obj_Pdt.ZFocus) == false)
            {
                HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                            App.obj_Vision.genMapStartX,
                                            App.obj_Vision.genMapStartY,
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
            App.obj_Vision.ShowImage(htWindow, Image, null);
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


        bool ScanWork = false;
        int scan_index = 0;
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
                btnScanMapImg.Text = "采集Mapping用图";
            }
            else
            {
                ScanWork = true;
                btnScanMapImg.Text = "停止采集";
                await Task.Run(() =>
                {
                    scan_index = 0;
                    if (GenMapPostions == null) GenMapPostions = new List<ImagePosition>();
                    X_motion = new HTuple();
                    Y_motion = new HTuple();
                    string errStr = "";
                    scan_index = 0;
                    string sPath = App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + mappingImgDir;
                    if (Directory.Exists(sPath)) Directory.Delete(sPath, true);
                    Directory.CreateDirectory(sPath);
                    while (ScanWork)
                    {
                        if (App.obj_Chuck.Move2ImagePosition(GenMapPostions[scan_index].x, GenMapPostions[scan_index].y, GenMapPostions[scan_index].z) == false)
                        {
                            HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                                        GenMapPostions[scan_index].b,
                                                        GenMapPostions[scan_index].r,
                                                        GenMapPostions[scan_index].c,
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
                            App.obj_Vision.ShowImage(htWindow, imageCache._2dImage.SelectObj(1), null);
                        }
                        scan_index++;
                        if (scan_index >= GenMapPostions.Count)
                        {
                            break;
                        }
                    }
                    HOperatorSet.WriteTuple(X_motion, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + mappingImgDir + "\\Xpoint.dat");
                    HOperatorSet.WriteTuple(Y_motion, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + mappingImgDir + "\\Ypoint.dat");
                    if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                        App.obj_Chuck.ref_y,
                        App.obj_Chuck.z_Safe))
                    {
                        HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                    }
                    HTUi.TipHint("采集Map采集结束.");
                    HTLog.Info("采集Map采集结束.");
                    scan_index = 0;
                    ScanWork = false;
                    btnScanMapImg.BeginInvoke(new MethodInvoker(() => { btnScanMapImg.Text = "采集Mapping用图"; }));
                });
            }
        }

        private void btnAutoGenMap_Click(object sender, EventArgs e)
        {
            try
            {
                VisionMethon.gen_map_images(out App.obj_Vision.frameMapImg, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + mappingImgDir, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex], App.obj_Vision.scaleFactor, out App.obj_Vision.hv_xSnapPosLT, out App.obj_Vision.hv_ySnapPosLT, out hv_iFlag);
                if (hv_iFlag != "")
                {
                    HTUi.PopError("生成图谱失败.");
                    return;
                }
                HOperatorSet.WriteImage(App.obj_Vision.frameMapImg, "tiff", 0, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\frameMapImg.tiff");
                HOperatorSet.WriteTuple(App.obj_Vision.hv_xSnapPosLT, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\hv_xSnapPosLT.tup");
                HOperatorSet.WriteTuple(App.obj_Vision.hv_ySnapPosLT, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\hv_ySnapPosLT.tup");
                App.obj_Vision.ShowImage(htWindow, App.obj_Vision.frameMapImg, null);
                HTUi.TipHint("生成图谱完成.");
                HTLog.Info("生成图谱完成.");
            }
            catch (Exception ex)
            {
                HTUi.PopError("生成图谱失败.\n" + ex.ToString());
            }
        }


        private void btnCrtLctMdls_Click(object sender, EventArgs e)
        {
            if (toolForm == null || toolForm.IsDisposed)
            {
                toolForm = new Form();
                if (this.mTmpLctMdlFrm != null)
                    if(!this.mTmpLctMdlFrm.IsDisposed) this.mTmpLctMdlFrm.Dispose();
                this.mTmpLctMdlFrm = new MainTemplateForm(ToolKits.TemplateEdit.MainTemplateForm.TemplateScence.Match,
                                 this.htWindow, new MainTemplateForm.TemplateParam());
                toolForm.Controls.Clear();
                this.toolForm.Controls.Add(this.mTmpLctMdlFrm);
                this.mTmpLctMdlFrm.Dock = DockStyle.Fill;
                toolForm.Size = new Size(300, 600);
                toolForm.TopMost = true;
                toolForm.Text = "模板制作";
                toolForm.Show();
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                toolForm.Location = new Point(SW - toolForm.Size.Width, SH / 8);
                Task.Run(() =>
                {
                    while(!mTmpLctMdlFrm.WorkOver)
                    {
                        Thread.Sleep(200);
                    }
                    HTUi.TipHint("创建芯片定位模板完成!");
                    HTLog.Info("创建芯片定位模板完成!");
                    toolForm.Close();
                });
            }
            else
            {
                toolForm.Activate();
            }
        }

        private async void btnSaveLctMdls_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.mTmpLctMdlFrm.tmpResult.createTmpOK)
                {
                    HTUi.PopError("创建定位模板失败！");
                    return;
                }
                this.LoctionModels = new Model();
                this.LoctionModels.Dispose();
                this.LoctionModels.showContour = this.mTmpLctMdlFrm.tmpResult.showContour.CopyObj(1, -1);
                this.LoctionModels.defRows = this.mTmpLctMdlFrm.tmpResult.defRows;
                this.LoctionModels.defCols = this.mTmpLctMdlFrm.tmpResult.defCols;
                this.LoctionModels.modelType = this.mTmpLctMdlFrm.tmpResult.modelType;
                this.LoctionModels.modelID = Vision.CopyModel(this.mTmpLctMdlFrm.tmpResult.modelID, this.mTmpLctMdlFrm.tmpResult.modelType);
                this.LoctionModels.scoreThresh = this.mTmpLctMdlFrm.mte_TmpPrmValues.Score;
                this.LoctionModels.angleStart = this.mTmpLctMdlFrm.mte_TmpPrmValues.AngleStart;
                this.LoctionModels.angleExtent = this.mTmpLctMdlFrm.mte_TmpPrmValues.AngleExtent;
                //保存至硬盘
                string modelPath = App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\LoctionModels"; ;
                App.obj_Operations.CalibrUV2XYModelPath = modelPath;

                Form_Wait.ShowForm();
                await Task.Run(new Action(() =>
                {
                    if (!this.LoctionModels.WriteModel(modelPath))
                    {
                        HTUi.PopError("保存标定模板失败！");
                        HTLog.Error("保存标定模板失败！");
                        return;
                    }
                    HTUi.TipHint("保存定位模板成功！");
                    HTLog.Info("保存定位模板成功！");
                }));

                Form_Wait.CloseForm();
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存定位模板失败！\n" + ex.ToString());
            }
        }

        private void btnGenMapPos_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.LoctionModels == null)
                {
                    this.LoctionModels = new Model();
                    if (!this.LoctionModels.ReadModel(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\LoctionModels"))
                    {
                        HTUi.PopError("未创建定位模板！");
                        return;
                    }
                }
                if (App.obj_Vision.frameMapImg == null)
                {
                    HTUi.PopError("未生成或加载料片图！");
                    return;
                }
                HObject _clipRegion = null, clipImage;
                HTuple _row1, _col1, _row2, _col2;
                HTuple hom2D;
                HOperatorSet.GenRegionContourXld(this.LoctionModels.showContour, out _clipRegion, "filled");
                HOperatorSet.SmallestRectangle1(_clipRegion, out _row1, out _col1, out _row2, out _col2);
                HOperatorSet.VectorAngleToRigid(_row1, _col1, 0, 0, 0, 0, out hom2D);
                HOperatorSet.CropRectangle1(App.obj_Vision.frameMapImg, out clipImage, _row1, _col1, _row2, _col2);
                Directory.CreateDirectory(App.ProductDir + "\\" + ProductMagzine.ActivePdt);
                HOperatorSet.WriteImage(clipImage, "tiff", 0, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "ClipImage.tiff");

                VisionMethon.get_mapping_coords(App.obj_Vision.frameMapImg, this.LoctionModels.showContour, this.LoctionModels.modelType, this.LoctionModels.modelID,
                    this.LoctionModels.defRows, this.LoctionModels.defCols, App.obj_Vision.lctScoreThresh, App.obj_Vision.hv_xSnapPosLT, App.obj_Vision.hv_ySnapPosLT, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex], App.obj_Vision.scaleFactor,
                    App.obj_Pdt.RowNumber, App.obj_Pdt.ColumnNumber * App.obj_Pdt.BlockNumber,
                    out App.obj_Vision.clipMapX, out App.obj_Vision.clipMapY, out App.obj_Vision.clipMapRow, out App.obj_Vision.clipMapCol,
                    out App.obj_Vision.clipMapU, out App.obj_Vision.clipMapV, out App.obj_Vision.hv_dieWidth, out App.obj_Vision.hv_dieHeight, out hv_iFlag);
                if (hv_iFlag.S != "")
                {
                    HTUi.PopError("生成芯片点位失败." + hv_iFlag.S);
                    return;
                }
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapX, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapX.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapY, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapY.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapRow, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapRow.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapCol, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapCol.dat");
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "dieWidth", App.obj_Vision.hv_dieWidth.D);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "dieHeight", App.obj_Vision.hv_dieHeight.D);
                //清除之前的点位
                if (App.obj_Vision.ClipMapPostions != null)
                {
                    App.obj_Vision.ClipMapPostions.Clear();
                }
                else
                {
                    App.obj_Vision.ClipMapPostions = new List<ImagePosition>();
                }
                ImagePosition imagePosition = new ImagePosition();
                imagePosition.z = App.obj_Pdt.ZFocus;
                imagePosition.b = 0;
                App.obj_Vision.clipPosNum = App.obj_Vision.clipMapX.Length;
                for (int i = 0; i < App.obj_Vision.clipPosNum; i++)
                {
                    imagePosition.x = App.obj_Vision.clipMapX.TupleSelect(i);
                    imagePosition.y = App.obj_Vision.clipMapY.TupleSelect(i);
                    imagePosition.r = App.obj_Vision.clipMapRow.TupleSelect(i);
                    imagePosition.c = App.obj_Vision.clipMapCol.TupleSelect(i);
                    App.obj_Vision.ClipMapPostions.Add(imagePosition);
                }
                if (showRegion == null) showRegion = new HObject();
                showRegion.Dispose();
                HOperatorSet.GenCrossContourXld(out showRegion, App.obj_Vision.clipMapU, App.obj_Vision.clipMapV, 26, 0);
                App.obj_Vision.ShowImage(htWindow, App.obj_Vision.frameMapImg, showRegion);
                //HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                //HOperatorSet.DispObj(App.obj_Vision.frameMapImg,htWindow.HTWindow.HalconWindow);
                //HOperatorSet.DispObj(Region, htWindow.HTWindow.HalconWindow);
                //Region.Dispose();


                HTUi.TipHint("生成芯片点位完成.");
                HTLog.Info("生成芯片点位完成.");
            }
            catch (Exception ex)
            {
                HTUi.PopError("生成芯片点位失败。\n" + ex.ToString());
                HTLog.Error("生成芯片点位失败。\n" + ex.ToString());
            }
        }
        private void btnFixMapPos_Click(object sender, EventArgs e)
        {
            if (this.mTmpFixClipPosFrm == null || this.mTmpFixClipPosFrm.IsDisposed)
            {
                if (this.LoctionModels == null)
                {
                    this.LoctionModels = new Model();
                    if (!this.LoctionModels.ReadModel(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\LoctionModels"))
                    {
                        HTUi.PopError("未创建定位模板！");
                        return;
                    }
                }
                if(App.obj_Vision.frameMapImg==null)
                {
                    HTUi.PopError("未生成或加载料片图！");
                    return;
                }
                try
                {
                    HObject _clipRegion = null, clipImage;
                    HTuple _row1, _col1, _row2, _col2;
                    HTuple hom2D;
                    HOperatorSet.GenRegionContourXld(this.LoctionModels.showContour, out _clipRegion, "filled");
                    HOperatorSet.SmallestRectangle1(_clipRegion, out _row1, out _col1, out _row2, out _col2);
                    HOperatorSet.VectorAngleToRigid(_row1, _col1, 0, 0, 0, 0, out hom2D);
                    HOperatorSet.CropRectangle1(App.obj_Vision.frameMapImg, out clipImage, _row1, _col1, _row2, _col2);
                    Directory.CreateDirectory(App.ProductDir + "\\" + ProductMagzine.ActivePdt);
                    HOperatorSet.WriteImage(clipImage, "tiff", 0, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "ClipImage.tiff");

                    this.mTmpFixClipPosFrm = new Form_FixMapPos(htWindow, this.LoctionModels, RegionModifyForm.RegionMode.contour);
                    this.mTmpFixClipPosFrm.TopMost = true;
                    this.mTmpFixClipPosFrm.Text = "手动生成芯片点位";
                    this.mTmpFixClipPosFrm.Show();
                    int SH = Screen.PrimaryScreen.Bounds.Height;
                    int SW = Screen.PrimaryScreen.Bounds.Width;
                    this.mTmpFixClipPosFrm.Location = new Point((SW - this.mTmpFixClipPosFrm.Size.Width) / 2, (SH - this.mTmpFixClipPosFrm.Size.Height) / 2);
                }
                catch(Exception ex)
                {
                    HTUi.PopError("截取芯片图出错！\n"+ex.ToString());
                }
            }
            else
            {
                this.mTmpFixClipPosFrm.Activate();
            }
        }
        
        private void btnGenScanPos_Click(object sender, EventArgs e)
        {
            try
            {
                if (htWindow.Image == null)
                {
                    HTUi.PopError("生成失败.\n未在窗口显示图片.");
                    return;
                }
                //HOperatorSet.GetImageSize(htWindow.Image, out width, out height);
                if (showRegion == null) showRegion = new HObject();
                showRegion.Dispose();
                VisionMethon.get_scan_points(out showRegion, width, height, App.obj_Vision.widthFactor, App.obj_Vision.heightFactor, App.obj_Vision.scaleFactor,
                    App.obj_Vision.hv_dieWidth, App.obj_Vision.hv_dieHeight, App.obj_Vision.clipMapX, App.obj_Vision.clipMapY, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex],
                    App.obj_Vision.hv_xSnapPosLT, App.obj_Vision.hv_ySnapPosLT, App.obj_Pdt.RowNumber, App.obj_Pdt.ColumnNumber * App.obj_Pdt.BlockNumber,
                    out snapM, out snapN, out App.obj_Vision.snapMapX, out App.obj_Vision.snapMapY,
                    out App.obj_Vision.snapMapRow, out App.obj_Vision.snapMapCol, out hv_iFlag);
                if (hv_iFlag != "")
                {
                    HTUi.PopError("生成扫描点位失败.");
                    return;
                }
                App.obj_Vision.scanRowNum = App.obj_Vision.snapMapRow.TupleMax() + 1;
                App.obj_Vision.scanColNum = App.obj_Vision.snapMapCol.TupleMax() + 1;
                App.obj_Vision.ShowImage(htWindow, App.obj_Vision.frameMapImg, showRegion);
                HOperatorSet.WriteTuple(App.obj_Vision.snapMapX, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapX.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.snapMapY, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapY.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.snapMapRow, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapRow.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.snapMapCol, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapCol.dat");

                //清除之前的点位
                if (App.obj_Vision.ScanMapPostions != null)
                {
                    App.obj_Vision.ScanMapPostions.Clear();
                }
                else
                {
                    App.obj_Vision.ScanMapPostions = new List<ImagePosition>();
                }
                ImagePosition imagePosition = new ImagePosition();
                imagePosition.z = App.obj_Pdt.ZFocus;
                App.obj_Vision.scanPosNum = App.obj_Vision.snapMapX.Length;
                for (int i = 0; i < App.obj_Vision.scanPosNum; i++)
                {
                    imagePosition.x = App.obj_Vision.snapMapX.TupleSelect(i);
                    imagePosition.y = App.obj_Vision.snapMapY.TupleSelect(i);
                    imagePosition.r = App.obj_Vision.snapMapRow.TupleSelect(i);
                    imagePosition.c = App.obj_Vision.snapMapCol.TupleSelect(i);
                    App.obj_Vision.ScanMapPostions.Add(imagePosition);
                }
                HTUi.TipHint("生成扫描点位完成.");
                HTLog.Info("生成扫描点位完成.");
            }
            catch (Exception ex)
            {
                HTUi.PopError("生成扫描点位失败。\n" + ex.ToString());
                HTLog.Error("生成扫描点位失败。\n" + ex.ToString());
            }
        }

        private void btnSnapCheckPos_Click(object sender, EventArgs e)
        {
            if (App.obj_Chuck.Move2ImagePosition(App.obj_Vision.checkPosX, App.obj_Vision.checkPosY, App.obj_Pdt.ZFocus) == false)
            {
                HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                            App.obj_Vision.checkPosX,
                                            App.obj_Vision.checkPosY,
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
            if (errStr != "")
            {
                HTUi.PopError(errStr);
                return;
            }
            App.obj_Vision.ShowImage(htWindow, Image, null);
            if (App.obj_Vision.CheckPosModels == null) return;
            HTuple hv_updateSnapMapX; HTuple hv_updateSnapMapY; HTuple hv_iFlag;
            if (App.obj_Vision.CheckPosModels.matchRegion == null || !App.obj_Vision.CheckPosModels.matchRegion.IsInitialized())
                HOperatorSet.GetDomain(Image, out App.obj_Vision.CheckPosModels.matchRegion);
            try
            {
                VisionMethon.update_map_points(Image, App.obj_Vision.CheckPosModels.matchRegion, App.obj_Vision.CheckPosModels.modelType, App.obj_Vision.CheckPosModels.modelID,
                App.obj_Vision.checkPosScoreThresh, App.obj_Vision.clipMapX, App.obj_Vision.clipMapY, App.obj_Vision.snapMapX, App.obj_Vision.snapMapY, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex],
                out App.obj_Vision.hv_updateMapX, out App.obj_Vision.hv_updateMapY, out hv_updateSnapMapX, out hv_updateSnapMapY, out App.obj_Vision.hv_foundU, out App.obj_Vision.hv_foundV, out hv_iFlag);
                if (hv_iFlag.S != "")
                {
                    HTLog.Error("矫正点识别失败！" + hv_iFlag.S);
                    return;
                }
            }
            catch
            {
                HTLog.Error("矫正点识别失败！");
                return;
            }
            if (App.obj_Vision.showRegion != null) App.obj_Vision.showRegion.Dispose();
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, App.obj_Vision.hv_foundU, App.obj_Vision.hv_foundV, 512, 0);
            App.obj_Vision.ShowImage(htWindow, Image, App.obj_Vision.showRegion);
        }
        private void btnCrtCheckPosMdl_Click(object sender, EventArgs e)
        {
            if (toolForm == null || toolForm.IsDisposed)
            {
                toolForm = new Form();
                if (this.mTmpCheckPosMdlFrm != null)
                    if(!this.mTmpCheckPosMdlFrm.IsDisposed) this.mTmpCheckPosMdlFrm.Dispose();
                this.mTmpCheckPosMdlFrm = new MainTemplateForm(ToolKits.TemplateEdit.MainTemplateForm.TemplateScence.Match,
                                 this.htWindow, new MainTemplateForm.TemplateParam());
                toolForm.Controls.Clear();
                this.toolForm.Controls.Add(this.mTmpCheckPosMdlFrm);
                this.mTmpCheckPosMdlFrm.Dock = DockStyle.Fill;
                toolForm.Size = new Size(300, 600);
                toolForm.TopMost = true;
                toolForm.Show();
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                toolForm.Location = new Point(SW - toolForm.Size.Width, SH / 8);

                Task.Run(() =>
                {
                    while (!mTmpCheckPosMdlFrm.WorkOver)
                    {
                        Thread.Sleep(200);
                    }
                    HTUi.TipHint("创建矫正点模板完成!");
                    HTLog.Info("创建矫正点模板完成!");
                    toolForm.Close();
                });
            }
            else
            {
                toolForm.Activate();
            }
        }

        private async void btnSaveCheckPosMdl_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.mTmpCheckPosMdlFrm.tmpResult.createTmpOK)
                {
                    HTUi.PopError("创建矫正点模板失败！");
                    return;
                }
                App.obj_Vision.CheckPosModels = new Model();
                App.obj_Vision.CheckPosModels.Dispose();
                App.obj_Vision.CheckPosModels.showContour = this.mTmpCheckPosMdlFrm.tmpResult.showContour.CopyObj(1, -1);
                App.obj_Vision.CheckPosModels.defRows = this.mTmpCheckPosMdlFrm.tmpResult.defRows;
                App.obj_Vision.CheckPosModels.defCols = this.mTmpCheckPosMdlFrm.tmpResult.defCols;
                App.obj_Vision.CheckPosModels.modelType = this.mTmpCheckPosMdlFrm.tmpResult.modelType;
                App.obj_Vision.CheckPosModels.modelID = Vision.CopyModel(this.mTmpCheckPosMdlFrm.tmpResult.modelID, this.mTmpCheckPosMdlFrm.tmpResult.modelType);
                App.obj_Vision.CheckPosModels.scoreThresh = this.mTmpCheckPosMdlFrm.mte_TmpPrmValues.Score;
                App.obj_Vision.CheckPosModels.angleStart = this.mTmpCheckPosMdlFrm.mte_TmpPrmValues.AngleStart;
                App.obj_Vision.CheckPosModels.angleExtent = this.mTmpCheckPosMdlFrm.mte_TmpPrmValues.AngleExtent;
                //保存至硬盘
                string modelPath = App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\CheckPosModels";
                App.obj_Operations.CalibrUV2XYModelPath = modelPath;

                Form_Wait.ShowForm();
                await Task.Run(new Action(() =>
                {
                    try
                    {
                        if (!App.obj_Vision.CheckPosModels.WriteModel(modelPath))
                        {
                            HTUi.PopError("保存矫正点模板失败！");
                            return;
                        }
                        App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "checkPosX", App.obj_Vision.checkPosX);
                        App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "checkPosY", App.obj_Vision.checkPosY);
                        HOperatorSet.WriteImage(Image, "tiff", 0, modelPath + "\\checkPos.tiff");
                        HTUi.TipHint("保存矫正点模板成功！");
                        HTLog.Info("保存矫正点模板成功！");
                    }
                    catch (Exception ex)
                    {
                        HTUi.PopError("保存矫正点模板出错！\n" + ex.ToString());
                    }
                }));
                Form_Wait.CloseForm();
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存矫正点模板失败！\n" + ex.ToString());
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

        private void btnSetXY2CheckPos_Click(object sender, EventArgs e)
        {
            try
            {
                double nowX = App.obj_Chuck.GetXPos();
                double nowY = App.obj_Chuck.GetYPos();
                numCheckPosX.Value = (decimal)nowX;
                numCheckPosY.Value = (decimal)nowY;
            }
            catch (Exception ex)
            {
                HTUi.PopError("加载矫正点图像失败！\n" + ex.ToString());
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
            App.obj_Vision.ShowImage(htWindow, Image, null);
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
                App.obj_Vision.genMapStartX = (double)numStartX.Value;
                App.obj_Vision.genMapStartY = (double)numStartY.Value;
                App.obj_Vision.genMapEndX = (double)numEndX.Value;
                App.obj_Vision.genMapEndY = (double)numEndY.Value;
                App.obj_Vision.sameSpace = (double)numSameSpace.Value;
                App.obj_Vision.scaleFactor = (double)numScaleFactor.Value;
                App.obj_Vision.lctScoreThresh = (double)numLctScoreThresh.Value;
                App.obj_Vision.checkPosX = (double)numCheckPosX.Value;
                App.obj_Vision.checkPosY = (double)numCheckPosY.Value;
                App.obj_Vision.checkPosScoreThresh = (double)numCheckPosScoreThresh.Value;
                App.obj_Vision.widthFactor = (double)numWidthFactor.Value;
                App.obj_Vision.heightFactor = (double)numHeightFactor.Value;
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "genMapStartX", App.obj_Vision.genMapStartX);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "genMapStartY", App.obj_Vision.genMapStartY);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "genMapEndX", App.obj_Vision.genMapEndX);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "genMapEndY", App.obj_Vision.genMapEndY);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "sameSpace", App.obj_Vision.sameSpace);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "scaleFactor", App.obj_Vision.scaleFactor);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "lctScoreThresh", App.obj_Vision.lctScoreThresh);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "checkPosX", App.obj_Vision.checkPosX);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "checkPosY", App.obj_Vision.checkPosY);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "checkPosScoreThresh", App.obj_Vision.checkPosScoreThresh);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "widthFactor", App.obj_Vision.widthFactor);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "heightFactor", App.obj_Vision.heightFactor);
                App.formIniConfig.SaveObj(this);
                HTUi.TipHint(ProductMagzine.ActivePdt+"产品图谱参数保存成功！");
                HTLog.Info(ProductMagzine.ActivePdt + "产品图谱参数保存成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError(ProductMagzine.ActivePdt + "产品图谱参数保存失败。\n" + ex.ToString());
                HTLog.Error(ProductMagzine.ActivePdt + "产品图谱参数保存失败。\n" + ex.ToString());
            }
        }

        private void btnLoadMapImg_Click(object sender, EventArgs e)
        {
            try
            {
                string modelPath = App.ProductDir + "\\" + ProductMagzine.ActivePdt;
                if (!File.Exists(modelPath + "\\frameMapImg.tiff"))
                {
                    HTUi.PopError("未生成过该产品图谱！\n无法找到该产品图谱文件！");
                }
                HOperatorSet.ReadImage(out Image, modelPath + "\\frameMapImg.tiff");
                App.obj_Vision.ShowImage(htWindow, Image, null);
                App.obj_Vision.frameMapImg = Image.CopyObj(1, -1);
                App.obj_Vision.hv_xSnapPosLT = new HTuple();
                App.obj_Vision.hv_ySnapPosLT = new HTuple();
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\hv_xSnapPosLT.tup"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\hv_xSnapPosLT.tup", out App.obj_Vision.hv_xSnapPosLT);
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\hv_ySnapPosLT.tup"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\hv_ySnapPosLT.tup", out App.obj_Vision.hv_ySnapPosLT);
                HTUi.TipHint("加载产品图谱成功！");
                HTLog.Info("加载产品图谱成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError("加载产品图谱失败！\n" + ex.ToString());
            }
        }

        private void btnCcCenterDis_Click(object sender, EventArgs e)
        {
            if (htWindow.RegionType != "Point")
            {
                MessageBox.Show("请先画个点作为绘制中心！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            App.obj_Vision.showRegion = new HObject();
            ToolKits.FunctionModule.Vision.GenROI(htWindow, "contour", ref App.obj_Vision.showRegion);


            HTuple r, c, area,width,height;
            HObject centerPoint = null;

            HOperatorSet.AreaCenterPointsXld(App.obj_Vision.showRegion, out area, out r, out c);
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, r, c, 100, 0);
            App.obj_Vision.ShowImage(htWindow, htWindow.Image, App.obj_Vision.showRegion);

            HOperatorSet.GetImageSize(htWindow.Image, out width, out height);

            HOperatorSet.GenCrossContourXld(out centerPoint, height/2, width/2, 100, 0);
            HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "pink");
            HOperatorSet.DispXld(centerPoint, htWindow.HTWindow.HalconWindow);
            HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "yellow");


            double vdx = (c * 2 - width) / 2;
            double vdy = -(r * 2 - height) / 2;
            tbdx.Text = (vdx * Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D)).ToString();
            tbdy.Text = (vdy * Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][3].D)).ToString();
        }

        private void btnFirstDie_Click(object sender, EventArgs e)
        {
            App.obj_Vision.SnapPos(App.obj_Vision.ClipMapPostions[0].x, App.obj_Vision.ClipMapPostions[0].y, App.obj_Vision.ClipMapPostions[0].z,htWindow, out App.obj_Vision.Image);
        }

        private void button_WriteDxDY_Click(object sender, EventArgs e)
        {
            App.obj_Pdt.RelativeMark_X = Double.Parse(tbdx.Text);
            App.obj_Pdt.RelativeMark_Y = Double.Parse(tbdy.Text);
            App.obj_Pdt.SymmetryMark= cbx_Symmetry.Checked;
            App.obj_Pdt.Save();
        }

        private void btn_MarkFirstDie_Click(object sender, EventArgs e)
        {
            App.obj_Chuck.EC_Printer(App.obj_Vision.ClipMapPostions[0].x + Double.Parse(tbdx.Text) + App.obj_Chuck.Ref_Mark_x,
                App.obj_Vision.ClipMapPostions[0].y + Double.Parse(tbdy.Text) + App.obj_Chuck.Ref_Mark_y,
                App.obj_Pdt.RelativeMark_Z,
                Form_Ec_Jet.IsX, Form_Ec_Jet.RXorY);
            App.obj_Vision.SnapPos(App.obj_Vision.ClipMapPostions[0].x, App.obj_Vision.ClipMapPostions[0].y, App.obj_Vision.ClipMapPostions[0].z, htWindow, out App.obj_Vision.Image);
        }

        private void btn_MarkReview_Click(object sender, EventArgs e)
        {

            App.obj_Chuck.EC_Printer(App.obj_Vision.ClipMapPostions[1].x + Double.Parse(tbdx.Text) - App.obj_Chuck.Ref_Mark_x,
                App.obj_Vision.ClipMapPostions[1].y + Double.Parse(tbdy.Text) + App.obj_Chuck.Ref_Mark_y,
                App.obj_Pdt.RelativeMark_Z,
                Form_Ec_Jet.IsX, Form_Ec_Jet.RXorY);
            App.obj_Vision.SnapPos(App.obj_Vision.ClipMapPostions[1].x, App.obj_Vision.ClipMapPostions[1].y, App.obj_Vision.ClipMapPostions[1].z, htWindow, out App.obj_Vision.Image);
        }

        private void btnLoadCheckPosImg_Click(object sender, EventArgs e)
        {
            try
            {
                string modelPath = App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\CheckPosModels";
                if (!File.Exists(modelPath + "\\checkPos.tiff"))
                {
                    HTUi.PopError("未生成过该产品矫正点！\n无法找到该产品矫正点文件！");
                }
                HOperatorSet.ReadImage(out Image, modelPath + "\\checkPos.tiff");
                HTuple hv_updateSnapMapX; HTuple hv_updateSnapMapY; HTuple hv_iFlag;
                if (App.obj_Vision.CheckPosModels.matchRegion == null || !App.obj_Vision.CheckPosModels.matchRegion.IsInitialized())
                    HOperatorSet.GetDomain(Image, out App.obj_Vision.CheckPosModels.matchRegion);
                VisionMethon.update_map_points(Image, App.obj_Vision.CheckPosModels.matchRegion, App.obj_Vision.CheckPosModels.modelType, App.obj_Vision.CheckPosModels.modelID,
                    App.obj_Vision.checkPosScoreThresh, App.obj_Vision.clipMapX, App.obj_Vision.clipMapY, App.obj_Vision.snapMapX, App.obj_Vision.snapMapY, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex],
                    out App.obj_Vision.hv_updateMapX, out App.obj_Vision.hv_updateMapY, out hv_updateSnapMapX, out hv_updateSnapMapY, out App.obj_Vision.hv_foundU, out App.obj_Vision.hv_foundV, out hv_iFlag);
                if (hv_iFlag.S != "")
                {
                    HTLog.Error("矫正点识别失败！" + hv_iFlag.S);
                    return;
                }
                if (App.obj_Vision.showRegion != null) App.obj_Vision.showRegion.Dispose();
                HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, App.obj_Vision.hv_foundU, App.obj_Vision.hv_foundV, 512, 0);
                App.obj_Vision.ShowImage(htWindow, Image, App.obj_Vision.showRegion);
                HTUi.TipHint("加载矫正点图像成功！");
                HTLog.Info("加载矫正点图像成功！");
            }
            catch (Exception ex)
            {
                HTUi.PopError("加载矫正点图像失败！\n" + ex.ToString());
            }
        }

        private void btnSnapMapEnd_Click(object sender, EventArgs e)
        {
            if (App.obj_Chuck.Move2ImagePosition(App.obj_Vision.genMapEndX, App.obj_Vision.genMapEndY, App.obj_Pdt.ZFocus) == false)
            {
                HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                            App.obj_Vision.genMapEndX,
                                            App.obj_Vision.genMapEndY,
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
            App.obj_Vision.ShowImage(htWindow, Image, null);
            if (errStr != "")
            {
                HTUi.PopError(errStr);
            }
        }

    }
}