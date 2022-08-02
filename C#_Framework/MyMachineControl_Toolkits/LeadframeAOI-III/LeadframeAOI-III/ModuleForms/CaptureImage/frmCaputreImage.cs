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
using HalconDotNet;
using HTHalControl;
using System.IO;
using Utils;
using HT_Lib;




namespace LeadframeAOI
{
    public partial class frmCaptureImage : Form
    {
        public int _selectIndex = 0;
        public static frmCaptureImage Instance = null;
        public HObject Image = null;
        public HObject showRegion;
        public string frameFolder = "";
        public List<DieInfo> listRCXYZ = new List<DieInfo>();  //所有芯片行列对应的xyz值
        public DieInfo dieInfo = new DieInfo(); //芯片信息：行、列、x位置、y位置、z位置、

        private HObject Defect_region = new HObject();
        private HObject Image0 = new HObject();
        private HObject Image1 = new HObject();
        private HObject Image2 = new HObject();
        private Button selBtns;
        private int _imageNum;
        private ImageInformation _imageSelected;
        private HObject ImageShown;
        private HObject ImageOrigin;
        private int SelectImgIdx = 0;
        private int SelectChannelIdx = 0;

        private bool ScanWork = false;
        private bool MutiScanFlag = false;
        private delegate void ShowImageDelegate(HTWindowControl htWindow, HObject image, HObject region);

        public frmCaptureImage()
        {
            try
            {
                InitializeComponent();
                Instance = this;
                //this.dgvTestImagesInfo.Columns[0].Visible = false;
            }
            catch (Exception EXP)
            {
                HTUi.PopError(EXP.Message);
            }
        }

        public void SetupUI()
        {
            try
            {
                mappingControl1.OnSelectedDieChanged += ShowInspectData;
                mappingControl2.OnSelectedDieChanged += ShowClipData;
                _imageNum = ProductMagzine.ImageNum;
                //App.obj_ImageInfo.CreateTable();
                //App.obj_ImageInfo.FillDataSet();
                this.dgvWorkImagesInfo.DataSource = App.obj_ImageInformSet;
                foreach (DataGridViewColumn item in this.dgvWorkImagesInfo.Columns)
                {
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }


                this.txtImageFolder.Text = App.obj_Vision.imageFolder;
                //txtTestImageFolder.Text = App.obj_Vision.TestImagePath;
                if (App.obj_Vision.obj_camera == null) return;
                foreach (Obj_Camera obj_cam in App.obj_Vision.obj_camera)
                {
                    obj_cam.isGrab = false;
                }
                timer_Grab.Enabled = false;
                btnSnap.Enabled = true;
                btnGrab.Enabled = true;

                for (int i = 0; i < Obj_Camera.Num_Camera; i++)
                {
                    cmbCameraList.Items.Add("Camera_" + i.ToString());
                }
                if (App.obj_Vision.obj_camera.Count != 0)
                {
                    cmbCameraList.SelectedIndex = Obj_Camera.SelectedIndex;
                }
                RefreshImageList();
                RefreshChannelList();
                ConfigLight();
            }
            catch (Exception ex)
            {
                HTUi.PopError("UI初始化失败！\n" + ex.ToString());
            }
        }
        #region 方法
        /// <summary>
        /// 配置光源
        /// </summary>
        private void ConfigLight()
        {
            try
            {
                bool ringEnable = App.obj_ImageInformSet[_selectIndex].RingLightEnable;
                double ringTime = App.obj_ImageInformSet[_selectIndex].RingImageMeans;
                bool coaxEnable1 = App.obj_ImageInformSet[_selectIndex].CoaxialLightEnable1;
                double coaxTime1 = App.obj_ImageInformSet[_selectIndex].CoaxialImageMeans1;
                bool coaxEnable2 = App.obj_ImageInformSet[_selectIndex].CoaxialLightEnable2;
                double coaxTime2 = App.obj_ImageInformSet[_selectIndex].CoaxialImageMeans2;
                //2. 配置
                App.obj_light.SetRingLight(ringEnable, ringTime);
                App.obj_light.SetCoaxLight1(coaxEnable1, coaxTime1);
                App.obj_light.SetCoaxLight2(coaxEnable2, coaxTime2);
            }
            catch (Exception ex)
            {

                throw new Exception("配置光源失败！\n" + ex.ToString());
            }
        }
        /// <summary>
        /// 刷新图片选择列表
        /// </summary>
        private void RefreshImageList()
        {
            cbBox_ImgSelect.Invoke(new MethodInvoker(() =>
            {
                int ImgCount = App.obj_ImageInformSet.Count;
                int sltIdx = cbBox_ImgSelect.SelectedIndex;
                cbBox_ImgSelect.Items.Clear();
                for (int i = 0; i < ImgCount; i++)
                {
                    cbBox_ImgSelect.Items.Add("第" + (i + 1) + "张图");
                }
                if (sltIdx == -1 || sltIdx >= ImgCount)
                {
                    cbBox_ImgSelect.SelectedIndex = 0;
                }
                else
                {
                    cbBox_ImgSelect.SelectedIndex = sltIdx;
                }
            }));
        }

        /// <summary>
        /// 刷新通道选择列表
        /// </summary>
        private void RefreshChannelList()
        {
            cbBox_ChannelSelect.Invoke(new MethodInvoker(() =>
            {
                int ChannelCount = 3;
                int sltIdx = cbBox_ChannelSelect.SelectedIndex;
                cbBox_ChannelSelect.Items.Clear();
                cbBox_ChannelSelect.Items.Add("原图");
                if (ChannelCount > 1)
                {
                    for (int i = 0; i < ChannelCount; i++)
                    {
                        cbBox_ChannelSelect.Items.Add("通道" + (i + 1));
                    }
                }
                if (sltIdx == -1 || sltIdx >= ChannelCount)
                {
                    cbBox_ChannelSelect.SelectedIndex = 0;
                }
                else
                {
                    cbBox_ChannelSelect.SelectedIndex = sltIdx;
                }
            }));
        }

        /// <summary>
        /// 根据原始图片显示相应图像以及通道
        /// </summary>
        /// <param name="Img">图片</param>
        public void ShowSelectImageChannel(HObject Img)
        {
            if (SelectChannelIdx == 0)
            {
                App.obj_Vision.ShowImage(htWindow, Img, htWindow.Region);
            }
            else
            {
                HTuple ImgChannelNum;
                HOperatorSet.CountChannels(Img, out ImgChannelNum);
                if (ImgChannelNum == 1)
                {
                    throw (new Exception("该图只有一个通道！"));
                }
                HObject ImgChannel1, ImgChannel2, ImgChannel3;
                HOperatorSet.GenEmptyObj(out ImgChannel1);
                HOperatorSet.GenEmptyObj(out ImgChannel2);
                HOperatorSet.GenEmptyObj(out ImgChannel3);
                HOperatorSet.GenEmptyObj(out ImageShown);
                ImageShown.Dispose();
                ImgChannel1.Dispose();
                ImgChannel2.Dispose();
                ImgChannel3.Dispose();
                ImageShown = Img;
                HOperatorSet.Decompose3(ImageShown, out ImgChannel1, out ImgChannel2, out ImgChannel3);
                if (SelectChannelIdx == 1)
                {
                    ImageShown = ImgChannel1;
                }
                else if (SelectChannelIdx == 2)
                {
                    ImageShown = ImgChannel2;
                }
                else if (SelectChannelIdx == 3)
                {
                    ImageShown = ImgChannel3;
                }
                App.obj_Vision.ShowImage(htWindow, ImageShown, htWindow.Region);
            }
        }
        /// <summary>
        /// 显示视野图检测结果
        /// </summary>
        /// <param name="dieInfo"></param>
        private void ShowInspectData(HTMappingControl.DieInfo dieInfo)
        {
            try
            {
                HOperatorSet.SetFont(htWindow.HTWindow.HalconWindow, "-Courier New-32-*-*-*-*-1-");

                frameFolder = App.obj_Vision.frameFolder;
                HObject tempImg;
                DirectoryInfo dr = new DirectoryInfo(frameFolder);
                DirectoryInfo[] drKids = dr.GetDirectories();
                if (drKids.Length < 1)
                {
                    HTUi.PopError("该目录下没有die");
                    return;
                }

                ImageCache imgCache = new ImageCache();
                imgCache.r = dieInfo.RowIndex;
                imgCache.c = dieInfo.ColumnIndex;
                dr = new DirectoryInfo(frameFolder + "\\Row" + imgCache.r + "-" + imgCache.c);
                if (!Directory.Exists(frameFolder + "\\Row" + imgCache.r + "-" + imgCache.c))
                {
                    HTLog.Error("无视野信息:\n" + "Row" + imgCache.r + "-" + imgCache.c);
                    return;
                }
                FileInfo[] fi = dr.GetFiles();
                bool haveImage = false;
                for (int i = 0; i < fi.Length; i++)
                {
                    if (fi[i].ToString().Contains(".tiff"))
                    {
                        haveImage = true;
                        HOperatorSet.ReadImage(out tempImg, frameFolder + "\\Row"
                            + imgCache.r + "-" + imgCache.c + "\\"
                            + fi[i].ToString());
                        HOperatorSet.ConcatObj(imgCache._2dImage, tempImg, out imgCache._2dImage);
                    }
                }
                if (!haveImage)
                {
                    HTUi.PopError("该视野无图片！");
                    return;
                }
                List<StructInspectResult> inspectResults;
                HTuple SnapX = null, SnapY = null;
                HOperatorSet.ReadTuple(frameFolder + "\\Row" + imgCache.r + "-" + imgCache.c + "\\" + "SnapX.tup", out SnapX);
                HOperatorSet.ReadTuple(frameFolder + "\\Row" + imgCache.r + "-" + imgCache.c + "\\" + "SnapY.tup", out SnapY);
                HOperatorSet.ReadTuple(frameFolder + "\\" + "updateMapX.tup", out App.obj_Vision.hv_updateMapX);
                HOperatorSet.ReadTuple(frameFolder + "\\" + "updateMapY.tup", out App.obj_Vision.hv_updateMapY);
                imgCache.X = SnapX.D;
                imgCache.Y = SnapY.D;
                bool IsAllOkSnap = false;


                if (ImageOrigin != null)
                {
                    lock (ImageOrigin)
                    {
                        if (ImageOrigin.IsInitialized())
                            ImageOrigin.Dispose();
                    }
                }
                ImageOrigin = imgCache._2dImage.CopyObj(1, -1);
                ShowSelectImageChannel(ImageOrigin.SelectObj(SelectImgIdx + 1));
                if (ckbUseAlg.Checked)
                {
                    SnapDataResult sdr;
                    if (!App.obj_Vision.Inspection(imgCache, htWindow, SelectImgIdx, false, out inspectResults, out IsAllOkSnap, out sdr))
                    {
                        HTUi.PopError("算法检测失败！\n" + App.obj_Vision.GetLastErrorString());
                        return;
                    }
                    if (IsAllOkSnap)
                    {
                        if (sdr.clipRowNumInImg == 0)
                            ShowPRODState(5, imgCache.r, imgCache.c);
                        else
                            ShowPRODState(1, imgCache.r, imgCache.c);
                    }
                    else ShowPRODState(2, imgCache.r, imgCache.c);
                    ShowProdMap2();
                    for (int k = 0; k < inspectResults.Count; k++)
                    {
                        if (inspectResults[k].OkOrNg)
                        {
                            ShowPRODState2(1, inspectResults[k].realRow - sdr.clipRowMinInImg, inspectResults[k].realCol - sdr.clipColMinInImg);
                        }
                        else
                        {
                            ShowPRODState2(2, inspectResults[k].realRow - sdr.clipRowMinInImg, inspectResults[k].realCol - sdr.clipColMinInImg);
                        }
                    }
                }
                imgCache.Dispose();
            }
            catch (Exception ex)
            {
                HTUi.PopError("查看该视野信息失败\n" + ex.ToString());
            }
        }
        /// <summary>
        /// 显示某芯片检测结果
        /// </summary>
        /// <param name="dieInfo"></param>
        private void ShowClipData(HTMappingControl.DieInfo dieInfo)
        {
            try
            {
                if (mappingControl2.GetDieState(dieInfo.RowIndex, dieInfo.ColumnIndex) != "不合格")
                {
                    HTUi.TipHint("只能查看不合格芯片！");
                    return;
                }
                if (htWindow.Image != null)
                {
                    htWindow.Image.Dispose();
                }
                HObject clipImg = null; HObject defectRegion = null; HObject wireRegion = null;
                clipImg = App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].ClipImage;
                defectRegion = App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].ListImgClipDefect[SelectImgIdx];
                wireRegion = App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].ClipWire;

                if (ImageOrigin != null) ImageOrigin.Dispose();
                ImageOrigin = clipImg.CopyObj(1, -1);
                RefreshImageList();
                RefreshChannelList();
                htWindow.Region = defectRegion;
                HOperatorSet.SetDraw(htWindow.HTWindow.HalconWindow, "margin");
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "yellow");
                if (htWindow.Image != null)
                {
                    if (htWindow.Image.IsInitialized())
                        htWindow.Image.Dispose();
                }
                HObject tempImg = ImageOrigin.SelectObj(SelectImgIdx + 1);
                ShowSelectImageChannel(tempImg);
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                HOperatorSet.DispXld(wireRegion, htWindow.HTWindow.HalconWindow);
                HOperatorSet.SetTposition(htWindow.HTWindow.HalconWindow, 0, 0);  //设置字体
                                                                                  //if (!IsAllOkSnap)
                                                                                  //    HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "red");
                HOperatorSet.WriteString(htWindow.HTWindow.HalconWindow,
                    "Clip " + (App.obj_Vision.clipRowMinInImg + mappingControl2.SelectedDieInfo.RowIndex) + "-" +
                    (App.obj_Vision.clipColMinInImg + mappingControl2.SelectedDieInfo.ColumnIndex)); //设置文字
            }
            catch (Exception ex)
            {
                HTUi.PopError("查看该视野信息失败\n" + ex.ToString());
            }
        }
        /// <summary>
        /// 定义刷新图谱的委托
        /// </summary>
        /// <param name="ProdState">状态值</param>
        /// <param name="row">纵坐标</param>
        /// <param name="column">横坐标</param>
        delegate void ShowProdStateDelegate(int ProdState, int row, int column);
        /// <summary>
        /// 刷新图谱的方法
        /// </summary>
        /// <param name="ProdState">状态值</param>
        /// <param name="row">纵坐标</param>
        /// <param name="column">横坐标</param>
        public void ShowPRODState(int ProdState, int row, int column)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowProdStateDelegate(ShowPRODState), new object[] { ProdState, row, column });
            }
            else
            {
                switch (ProdState)
                {
                    case 0://未检测
                        mappingControl1.SetDieState(row, column, "未检测");
                        break;
                    case 1://OK
                        mappingControl1.SetDieState(row, column, "合格");
                        break;
                    case 2://NG
                        mappingControl1.SetDieState(row, column, "不合格");
                        break;
                    //case 3://N2K
                    //    mappingControl1.SetDieState(row, column, "复看合格");
                    //    break;
                    //case 4://无芯片
                    //    mappingControl1.SetDieState(row, column, "无芯片");
                    //    break;
                    case 5://匹配失败
                        mappingControl1.SetDieState(row, column, "无芯片");
                        break;
                    case 6://未拍摄
                        mappingControl1.SetDieState(row, column, "未拍摄");
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 刷新图谱的方法
        /// </summary>
        /// <param name="ProdState">状态值</param>
        /// <param name="row">纵坐标</param>
        /// <param name="column">横坐标</param>
        public void ShowPRODState2(int ProdState, int row, int column)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowProdStateDelegate(ShowPRODState2), new object[] { ProdState, row, column });
            }
            else
            {
                switch (ProdState)
                {
                    case 0://未检测
                        mappingControl2.SetDieState(row, column, "未检测");
                        break;
                    case 1://OK
                        mappingControl2.SetDieState(row, column, "合格");
                        break;
                    case 2://NG
                        mappingControl2.SetDieState(row, column, "不合格");
                        break;
                    //case 3://N2K
                    //    mappingControl2.SetDieState(row, column, "复看合格");
                    //    break;
                    //case 4://无芯片
                    //    mappingControl2.SetDieState(row, column, "无芯片");
                    //    break;
                    case 5://匹配失败
                        mappingControl2.SetDieState(row, column, "匹配失败");
                        break;
                    default:
                        break;
                }
            }
        }
        private Dictionary<string, Color> dic_DieState_Color;
        private Dictionary<string, Color> dic_DieState_Color2;
        /// <summary>
        /// 显示产品图谱
        /// </summary>
        public void ShowProdMap()
        {
            //新建颜色字典
            dic_DieState_Color = new Dictionary<string, Color>();
            dic_DieState_Color.Add("未检测", Color.SkyBlue);
            dic_DieState_Color.Add("未拍摄", Color.White);
            dic_DieState_Color.Add("合格", Color.Green);
            dic_DieState_Color.Add("不合格", Color.Red);
            ////dic_DieState_Color.Add("复看合格", Color.Yellow);
            //dic_DieState_Color.Add("无芯片", Color.Purple); 
            dic_DieState_Color.Add("无芯片", Color.Pink);
            //初始化图谱
            mappingControl1.BeginInvoke(
                new MethodInvoker(() =>
                {
                    if (App.obj_SystemConfig.ScanMode == 0)
                    {
                        mappingControl1.Initial(App.obj_Vision.scanRowNum,
      App.obj_Vision.scanColNum, dic_DieState_Color, "未拍摄");
                    }
                    else if (App.obj_SystemConfig.ScanMode == 1)
                    {
                        mappingControl1.Initial(App.obj_MobSht.NoLineAxisMoveCount,
                            App.obj_MobSht.LineAxisTrigCount, dic_DieState_Color, "未拍摄");
                    }
                }));
        }

        /// <summary>
        /// 显示产品图谱
        /// </summary>
        public void ShowProdMap2()
        {
            //新建颜色字典
            dic_DieState_Color2 = new Dictionary<string, Color>();
            dic_DieState_Color2.Add("未检测", Color.SkyBlue);
            dic_DieState_Color2.Add("合格", Color.Green);
            dic_DieState_Color2.Add("不合格", Color.Red);
            ////dic_DieState_Color.Add("复看合格", Color.Yellow);
            //dic_DieState_Color.Add("无芯片", Color.Purple); 
            dic_DieState_Color2.Add("匹配失败", Color.Pink);
            //初始化图谱
            mappingControl2.Invoke(
                new MethodInvoker(() =>
                {
                    mappingControl2.Initial(App.obj_Vision.clipRowNumInImg,
                        App.obj_Vision.clipColNumInImg, dic_DieState_Color2, "未检测");
                }));
        }

        /// <summary>
        /// 软触发单次拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void softSnap(object sender, EventArgs e)
        {
            try
            {
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
                //lbGrabStatus.Text = App.obj_Vision.Acq.GrabStatus + ":" + (App.obj_Vision.Acq.index + 1).ToString();
                switch (App.obj_Vision.Acq.GrabStatus)
                {
                    case "GrabPass":
                        //lbGrabStatus.BackColor = Color.Green;
                        //HOperatorSet.DispObj(acq.Image, this.hWindowControl1.HalconWindow);
                        this.Image.Dispose();
                        this.Image = App.obj_Vision.Acq.Image.CopyObj(1, -1);
                        this.htWindow.RefreshWindow(this.Image, null, "fit");
                        HTuple imgNum;
                        HOperatorSet.CountChannels(this.Image, out imgNum);
                        //lbGrabStatus.Text = "GrabPass:" + imgNum.TupleSum().I.ToString();
                        App.obj_Vision.Acq.Image.Dispose();
                        break;
                    default:
                        //lbGrabStatus.BackColor = Color.Red;
                        //lbGrabStatus.Text = "GrabFail:0";
                        break;
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("采集图片失败！\n" + ex.ToString());
            }
        }

        /// <summary>
        /// 保存相机配置
        /// </summary>
        /// <returns></returns>
        private bool cameraSave()
        {
            for (int i = 0; i < Obj_Camera.Num_Camera; i++)
            {
                if (App.obj_Vision.obj_camera[i].Camera == null) continue;
                if (!App.obj_Vision.obj_camera[i].isEnable) continue;
                App.obj_Vision.obj_camera[i].SetExposure(App.obj_Vision.obj_camera[i].exposure);
                App.obj_Vision.obj_camera[i].SetGain(App.obj_Vision.obj_camera[i].gain);
            }
            try
            {
                App.obj_Vision.SaveCameraData();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 连续采图
        /// </summary>
        private void Grab()
        {
            do
            {
                try
                {
                    App.obj_Chuck.SWPosTrig();
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].Camera.ClearImageQueue();
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].acq.Dispose();
                    string errStr = "";
                    errStr = App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].CaputreImages(ref App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].acq.Image, 1, 5000);//获取图片
                    ShowImage(htWindow, App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].acq.Image, null);
                    if (errStr != "")
                    {
                        HTUi.PopError(errStr);
                        btnSnap.Enabled = true;
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    //LOG("失败:相机设备" + Obj_Camera.SelectedIndex.ToString() + "采图失败");
                    MessageBox.Show("采集图片失败！\n" + ex.ToString());
                }
            }
            while (App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].isGrab);
        }

        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="htWindow"></param>
        /// <param name="image"></param>
        /// <param name="region"></param>
        public void ShowImage(HTWindowControl htWindow, HObject image, HObject region)
        {
            if (htWindow.InvokeRequired)
            {
                htWindow.Invoke(new ShowImageDelegate(ShowImage), new object[] { htWindow, image, region });
            }
            else
            {
                htWindow.ColorName = "yellow";
                htWindow.SetInteractive(false);
                htWindow.RefreshWindow(image, region, "");//可以不显示区域
                htWindow.SetInteractive(true);
                //htWindow.ColorName = "green";
            }
        }

        /// <summary>
        /// 扫描
        /// </summary>
        private void Scan()
        {
            try
            {
                if (App.obj_SystemConfig.LotIdMode == 0)
                {
                    if (Form_InputLotId.Instance.DialogResult == DialogResult.Cancel)
                    {
                        HTUi.PopError("【Load】" + "未创建批次，扫描停止");
                        goto _end;
                    }
                }
                App.obj_Chuck.CatchFrame();
                if (!App.obj_Process.GetFrameId())
                {
                    HTUi.PopError("获取二维码失败!");
                    goto _end;
                }
                App.obj_Vision.ImageThreadNow = 0;
                if (App.obj_Process.ThisScanPostions == null) App.obj_Process.ThisScanPostions = new List<ImagePosition>();
                string errStr = "";
                if (App.obj_SystemConfig.ScanMode == 0)
                {
                    //Scan_Init
                    if (!App.obj_Process.GetImagePositions_PointAuto(htWindow))
                    {
                        HTUi.PopError("初始化扫描位置信息失败!");
                        goto _end;
                    }
                    if (ckbWaitCheckPos.Checked)
                    {
                        DialogResult result = MessageBox.Show("确认矫正点识别正确吗?", "检查矫正点", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            goto _end;
                        }
                    }
                    ShowProdMap();
                    frameFolder = App.obj_Vision.frameFolder;
                    //启动算法模块
                    App.obj_Process.InitCaches();
                    HTLog.Info("开始扫描...");
                    Task.Run(new Action(Inspection));
                    App.obj_Process.scan_index = 0;
                    while (ScanWork)
                    {
                        if (App.obj_Process.scan_index >= App.obj_Process.ThisScanPostions.Count)
                        {
                            break;
                        }
                        if (App.obj_Chuck.Move2ImagePosition(App.obj_Process.ThisScanPostions[App.obj_Process.scan_index].x, App.obj_Process.ThisScanPostions[App.obj_Process.scan_index].y, App.obj_Process.ThisScanPostions[App.obj_Process.scan_index].z) == false)
                        {
                            HTUi.PopError(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                                        App.obj_Process.ThisScanPostions[App.obj_Process.scan_index].b,
                                                        App.obj_Process.ThisScanPostions[App.obj_Process.scan_index].r,
                                                        App.obj_Process.ThisScanPostions[App.obj_Process.scan_index].c,
                                                        App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                            ScanWork = false;
                            btnScan.BeginInvoke(new MethodInvoker(() => { btnScan.Text = "扫描料片"; }));
                            return;
                        }
                        ImageCache imageCache = new ImageCache();
                        errStr = App.obj_Process.CaputreMultipleImages(ref imageCache);
                        if (errStr != "")//拍照完成
                        {
                            HTLog.Info("拍照失败!");
                            break;
                        }
                        App.obj_Process.scan_index++;
                    }
                }
                else if (App.obj_SystemConfig.ScanMode == 1)
                {
                    if (!App.obj_Process.GetImagePositions_RowAuto(htWindow))
                    {
                        HTUi.PopError("初始化扫描位置信息失败!");
                        goto _end;
                    }
                    if (ckbWaitCheckPos.Checked)
                    {
                        DialogResult result = MessageBox.Show("确认矫正点识别正确吗?", "检查矫正点", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            goto _end;
                        }
                    }
                    ShowProdMap();
                    frameFolder = App.obj_Vision.frameFolder;
                    //启动算法模块
                    App.obj_Process.InitCaches();
                    Task.Run(new Action(Inspection));
                    if (!ScanWork) goto _end;
                    //启动线性扫描
                    HTLog.Info("开始扫描...");
                    if (!App.obj_MobSht.StartMobileShooting_X(App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex],
                        App.obj_Chuck.dev_chuck.a_X, App.obj_Chuck.dev_chuck.a_Y))
                    {
                        HTUi.PopError("线性扫描失败:" + App.obj_MobSht.GetLastErrorString());
                        goto _end;
                    }
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("扫描过程中出现错误:\n" + ex.ToString());
            }
        _end:
            HTLog.Info("扫描结束");
            //移动到安全位
            if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                App.obj_Chuck.ref_y,
                App.obj_Chuck.z_Safe))
            {
                HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
            }
            DateTime t = DateTime.Now;
            while (App.obj_Vision.ImageThreadNow > 0)
            {
                Thread.Sleep(200);
                if (DateTime.Now.Subtract(t).TotalMilliseconds > (int)(numDelayMax.Value * 1000))
                {
                    App.obj_Vision.ImageThreadNow = 0;
                }
                label3.Invoke(new Action(() =>
                {
                    label3.Text = "检测线程数:" + App.obj_Vision.ImageThreadNow.ToString();
                }));
            }
            label3.Invoke(new Action(() =>
            {
                label3.Text = "检测线程数:" + App.obj_Vision.ImageThreadNow.ToString();
            }));
            btnScan.Invoke(new MethodInvoker(() => { btnScan.Enabled = false; }));
            btnScan.Invoke(new MethodInvoker(() => { btnScan.Text = "等待存储数据"; }));
            App.obj_Process.DisposeCaches();
            ScanWork = false;
            App.obj_Process.scan_index = 0;
            btnScan.Invoke(new MethodInvoker(() => { btnScan.Text = "扫描料片"; }));
            btnScan.Invoke(new MethodInvoker(() => { btnScan.Enabled = true; }));
            btn_Mark.Invoke(new MethodInvoker(() => { btn_Mark.Enabled = true; }));
        }
        /// <summary>
        /// 检测
        /// </summary>
        private void Inspection()
        {
            if (App.obj_Process.qAllImageCache != null & App.obj_Vision.RunMode != 1)
            {
                try
                {
                    App.obj_Vision.ThreadInspectionFlag = true;

                    App.obj_Vision.ImageThreadNow = App.obj_Vision.ThreadMax;
                    label3.Invoke(new Action(() =>
                    {
                        label3.Text = "检测线程数:" + App.obj_Vision.ImageThreadNow.ToString();
                    }));
                    Parallel.For(0, App.obj_Vision.ThreadMax, t =>
                    {
                        while (App.obj_Process.qAllImageCache != null || ScanWork)
                        {
                            if (App.obj_Process.qAllImageCache == null) return;
                            if (App.obj_Process.qAllImageCache.Count == 0)
                            {
                                Thread.Sleep(200);
                                App.obj_Vision.ListThreadInspectionFlag[t] = false;
                                bool temp = false;
                                for (int i = 0; i < App.obj_Vision.ThreadMax; i++)
                                {
                                    if (App.obj_Vision.ListThreadInspectionFlag[i])
                                    {
                                        temp = true;
                                        break;
                                    }
                                }
                                App.obj_Vision.ThreadInspectionFlag = temp;
                                continue;
                            }
                            App.obj_Vision.ListThreadInspectionFlag[t] = true;
                            App.obj_Vision.ThreadInspectionFlag = true;
                            try
                            {
                                if (App.obj_Process.qAllImageCache == null)
                                {
                                    return;
                                }
                                if (App.obj_Process.qAllImageCache.IsEmpty)
                                {
                                    Thread.Sleep(100);
                                    continue;
                                }
                                ImageCache imgCache = null;
                                //1.取图
                                if (!App.obj_Process.qAllImageCache.TryDequeue(out imgCache))
                                {
                                    Thread.Sleep(100);
                                    continue;
                                }
                                //2.不检测
                                if (!ckbUseAlg.Checked)
                                {
                                    //if (App.obj_SystemConfig.ScanMode == 0)
                                    //if (ImageOrigin != null)
                                    //{
                                    //    lock (ImageOrigin)
                                    //    {
                                    //        if (ImageOrigin.IsInitialized())
                                    //            ImageOrigin.Dispose();
                                    //    }
                                    //}
                                    ShowPRODState(0, imgCache.r, imgCache.c);
                                    HObject tempHObj = imgCache._2dImage.SelectObj(SelectImgIdx + 1);
                                    ShowImage(htWindow, tempHObj, null);
                                    tempHObj.Dispose();
                                    if (App.obj_SystemConfig.ImageNgSave == 0)
                                    {
                                        string snapPath = frameFolder + "\\Row" +
                                            imgCache.r + "-" + imgCache.c;
                                        Directory.CreateDirectory(snapPath);
                                        for (int i = 0; i < App.obj_ImageInformSet.Count; i++)
                                        {
                                            if (App.obj_ImageInformSet[i].Use2D)
                                            {
                                                HObject tmpHObj = imgCache._2dImage.SelectObj(i + 1);
                                                HOperatorSet.WriteImage(tmpHObj, "tiff", 0, snapPath + "\\" + i + ".tiff");
                                                tmpHObj.Dispose();
                                            }
                                            if (App.obj_ImageInformSet[i].UseAutoFocus)
                                            {
                                                string _3DPath = snapPath + "\\3DAutoFocus";
                                                Directory.CreateDirectory(_3DPath);
                                                HOperatorSet.WriteTuple(imgCache.List_Z_TrigPos[i], _3DPath + "\\" + "Snap" + i + "-" + "Z_TrigPoses.dat");
                                                int num = imgCache.List_Z_TrigPos[i].Length;
                                                for (int j = 0; j < num; j++)
                                                {
                                                    HOperatorSet.WriteImage(imgCache.List_3dImage[i].SelectObj(j + 1), "tiff", 0, _3DPath + "\\" + "Snap" + i + "-" + j + ".tiff");
                                                }
                                            }
                                        }
                                        HTuple SnapX = null, SnapY = null;
                                        SnapX = imgCache.X;
                                        SnapY = imgCache.Y;
                                        HOperatorSet.WriteTuple(SnapX, snapPath + "\\" + "SnapX.tup");
                                        HOperatorSet.WriteTuple(SnapY, snapPath + "\\" + "SnapY.tup");
                                    }
                                }
                                //2.检测
                                else
                                {
                                    List<StructInspectResult> inspectResults;
                                    bool IsAllOkSnap = false;

                                    //if (ImageOrigin != null)
                                    //{
                                    //    lock (ImageOrigin)
                                    //    {
                                    //        if (ImageOrigin.IsInitialized())
                                    //            ImageOrigin.Dispose();
                                    //    }
                                    //}
                                    ShowPRODState(0, imgCache.r, imgCache.c);
                                    //ImageOrigin = imgCache._2dImage.CopyObj(1, -1);
                                    //HObject tmpHObj = imgCache._2dImage.SelectObj(SelectImgIdx + 1);
                                    //ShowImage(htWindow, tmpHObj, null);
                                    //tmpHObj.Dispose();
                                    SnapDataResult sdr;
                                    if (!App.obj_Vision.Inspection(imgCache, htWindow, SelectImgIdx, true, out inspectResults, out IsAllOkSnap, out sdr, t))
                                    {
                                        imgCache.Dispose();
                                        HTLog.Error("检测失败!\n" + App.obj_Vision.GetLastErrorString());
                                        continue;
                                    }
                                    if (inspectResults == null)
                                    {
                                        continue;
                                    }
                                    //if (App.obj_SystemConfig.ScanMode == 0)
                                    {
                                        //DateTime tt = DateTime.Now;
                                        if (IsAllOkSnap)
                                        {
                                            if (sdr.clipRowNumInImg == 0)
                                                ShowPRODState(5, imgCache.r, imgCache.c);
                                            else
                                                ShowPRODState(1, imgCache.r, imgCache.c);
                                        }
                                        else ShowPRODState(2, imgCache.r, imgCache.c);
                                        //HTLog.Info("刷图谱用时：" + DateTime.Now.Subtract(tt).TotalMilliseconds + "ms");
                                    }
                                    for (int k = 0; k < inspectResults.Count; k++)
                                    {
                                        if (!inspectResults[k].OkOrNg)
                                        {
                                            ImagePosition ngPs = new ImagePosition();
                                            ngPs.r = imgCache.r;
                                            ngPs.c = imgCache.c;
                                            ngPs.x = inspectResults[k].x + App.obj_Pdt.RelativeMark_X + App.obj_Chuck.Ref_Mark_x;
                                            ngPs.y = inspectResults[k].y + App.obj_Pdt.RelativeMark_Y + App.obj_Chuck.Ref_Mark_y;
                                            ngPs.z = App.obj_Pdt.RelativeMark_Z;
                                            if (App.obj_Pdt.SymmetryMark && ngPs.c % 2 == 1)
                                            {
                                                ngPs.x = inspectResults[k].x + App.obj_Pdt.RelativeMark_X - App.obj_Chuck.Ref_Mark_x;
                                            }
                                            App.obj_Process.ThisMarkPostions.Add(ngPs);
                                        }
                                    }
                                }
                                //3.销毁
                                imgCache.Dispose();
                            }
                            catch (Exception ex)
                            {
                                HTLog.Error("算法检测失败！" + ex.ToString());
                            }
                        }
                    });
                    App.obj_Vision.ImageThreadNow = 0;
                    GC.Collect();
                    App.obj_Vision.ThreadInspectionFlag = false;
                }
                catch (Exception ex)
                {

                    HTLog.Error("算法检测失败！" + ex.ToString());
                }
                finally
                {
                    label3.Invoke(new Action(() =>
                    {
                        label3.Text = "检测线程数:" + App.obj_Vision.ImageThreadNow.ToString();
                    }));
                }
            }
        }
        /// <summary>
        /// 标记
        /// </summary>
        private void MarkNG()
        {

            for (int i = 0; i < App.obj_Process.ThisMarkPostions.Count; i = i + 1)
            {
                if (!ScanWork) break;
                if (App.obj_Chuck.EC_Printer(App.obj_Process.ThisMarkPostions[i].x, App.obj_Process.ThisMarkPostions[i].y, App.obj_Process.ThisMarkPostions[i].z, Form_Ec_Jet.IsX, Form_Ec_Jet.RXorY) == false)
                {
                    HTLog.Error(String.Format("喷码失败!详细信息:{0}",
                                                App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                    break;
                }

                if (i != 0)
                {
                    if (App.obj_Process.ThisMarkPostions[i].r != App.obj_Process.ThisMarkPostions[i - 1].r || App.obj_Process.ThisMarkPostions[i].c != App.obj_Process.ThisMarkPostions[i - 1].c)
                    {
                        App.obj_Vision.SnapPos(App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i - 1].r, App.obj_Process.ThisMarkPostions[i - 1].c].x,
                            App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i - 1].r, App.obj_Process.ThisMarkPostions[i - 1].c].y, App.obj_Pdt.ZFocus, htWindow, out App.obj_Vision.Image);
                        if (App.obj_SystemConfig.MarkSave == 1)
                        {
                            HOperatorSet.ZoomImageFactor(App.obj_Vision.Image, out App.obj_Vision.Image, App.obj_Vision.scaleFactor, App.obj_Vision.scaleFactor, "constant");
                            string snapPath = App.obj_Vision.frameFolder + "\\MarkImgs";
                            Directory.CreateDirectory(snapPath);
                            HOperatorSet.WriteImage(App.obj_Vision.Image, "tiff", 0, snapPath + "\\" + App.obj_Process.ThisMarkPostions[i - 1].r + " - " + App.obj_Process.ThisMarkPostions[i - 1].c + ".tiff");
                        }
                    }
                }
                if (i == App.obj_Process.ThisMarkPostions.Count - 1)
                {
                    App.obj_Vision.SnapPos(App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i].r, App.obj_Process.ThisMarkPostions[i].c].x,
                        App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i].r, App.obj_Process.ThisMarkPostions[i].c].y, App.obj_Pdt.ZFocus, htWindow, out App.obj_Vision.Image);
                    if (App.obj_SystemConfig.MarkSave == 1)
                    {
                        HOperatorSet.ZoomImageFactor(App.obj_Vision.Image, out App.obj_Vision.Image, App.obj_Vision.scaleFactor, App.obj_Vision.scaleFactor, "constant");
                        string snapPath = App.obj_Vision.frameFolder + "\\MarkImgs";
                        Directory.CreateDirectory(snapPath);
                        HOperatorSet.WriteImage(App.obj_Vision.Image, "tiff", 0, snapPath + "\\" + App.obj_Process.ThisMarkPostions[i].r + "-" + App.obj_Process.ThisMarkPostions[i].c + ".tiff");
                    }
                }
            }
            //移动到安全位
            if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                App.obj_Chuck.ref_y,
                App.obj_Chuck.z_Safe))
            {
                HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
            }
            ScanWork = false;
            btnScan.Invoke(new MethodInvoker(() => { btnScan.Enabled = true; }));
            btn_Mark.Invoke(new MethodInvoker(() => { btn_Mark.Text = "标记芯片"; }));

        }
        #endregion
        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (App.obj_Vision.ScanMapPostions == null)
            {
                HTUi.PopError("无法扫描！该产品扫描点位为空！\n请在图谱生成界面生成扫描点位！");
                return;
            }
            if (ScanWork)
            {
                MobileShootingModule.workFlag = false;
                ScanWork = false;
                btnScan.Text = "扫描料片";
                btn_Mark.Enabled = true;
            }
            else
            {
                HOperatorSet.SetFont(htWindow.HTWindow.HalconWindow, "-Courier New-32-*-*-*-*-1-");
                ScanWork = true;
                MobileShootingModule.workFlag = true;
                btn_Mark.Enabled = false;
                btnScan.Text = "停止扫描";
                if (App.obj_SystemConfig.LotIdMode == 0)
                {
                    Form_InputLotId.ShowDialogForm();
                }
                else if (App.obj_SystemConfig.LotIdMode == 1)
                {
                    App.obj_Process.InitialNewLot();
                }
                await Task.Run(new Action(Scan));
            }
        }
        private void btnConfigImagePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                txtImageFolder.Text = Fbd.SelectedPath;
            }
            App.obj_Vision.imageFolder = txtImageFolder.Text.Trim();
        }



        private void btnCaputreOneDieImages_Click(object sender, EventArgs e)
        {
            try
            {
                if (mappingControl1.SelectedDieInfo == null)
                {
                    HTUi.PopHint("请在扫描后进行该操作。");
                }
                int SelectedRow = mappingControl1.SelectedDieInfo.RowIndex;
                int SelectedColumn = mappingControl1.SelectedDieInfo.ColumnIndex;
                HTuple SnapX = null, SnapY = null;
                HOperatorSet.ReadTuple(App.obj_Vision.frameFolder + "\\Row" + SelectedRow + "-" + SelectedColumn + "\\" + "SnapX.tup", out SnapX);
                HOperatorSet.ReadTuple(App.obj_Vision.frameFolder + "\\Row" + SelectedRow + "-" + SelectedColumn + "\\" + "SnapY.tup", out SnapY);
                App.obj_Vision.SnapPos(SnapX, SnapY, App.obj_Pdt.ZFocus, htWindow, out App.obj_Vision.Image);
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.ToString());
            }
        }


        private void btnGrab_Click(object sender, EventArgs e)
        {
            if (App.obj_Vision.RunMode == 1) return;
            Thread thread_Grab = new Thread(Grab);
            if (!App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].isGrab)
            {
                try
                {
                    if (_selectIndex != dgvWorkImagesInfo.CurrentRow.Index)
                    {
                        _selectIndex = dgvWorkImagesInfo.CurrentRow.Index;

                        ImageInformation.ConfigAllData(_selectIndex);
                    }
                    btnSnap.Enabled = false;
                    btnGrab.Text = "停止采集";
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].isGrab = true;
                    thread_Grab.Start();
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

                    btnGrab.Text = "连续采集";
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].isGrab = false;
                    btnSnap.Enabled = true;
                }
                catch (Exception ex)
                {
                    btnSnap.Enabled = true;
                    HTUi.PopError("停止连续采图失败！\n" + ex.ToString());
                }
            }
        }

        private void timer_Grab_Tick(object sender, EventArgs e)
        {
            softSnap(btnGrab, e);
        }
        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                //int trigNum = 25;
                //double time = 10000.0;
                //int delayTime = (int)Math.Floor(1000.0 / trigNum);
                //DateTime start = DateTime.Now;
                //double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;
                //while (DateTime.Now.Subtract(start).TotalMilliseconds <= time)
                //{
                //    App.obj_Chuck.SWPosTrig();
                //    Thread.Sleep(delayTime);
                //}
                //return;

                if (App.obj_Vision.RunMode == 1) return;
                if (!timer_Grab.Enabled) btnSnap.Enabled = false;

                //1. get value from ui
                if (dgvWorkImagesInfo.CurrentRow.Index + 1 > App.obj_ImageInformSet.Count)
                {
                    throw new Exception("请先保存图像设置");
                }
                if (_selectIndex != dgvWorkImagesInfo.CurrentRow.Index)
                {
                    _selectIndex = dgvWorkImagesInfo.CurrentRow.Index;

                    ImageInformation.ConfigAllData(_selectIndex);
                }
                //3. 触发
                App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].Camera.ClearImageQueue();
                //int trigNum = 25;
                //double time = 1000.0;
                //int delayTime = (int)Math.Floor(1000.0 / trigNum);
                //DateTime start = DateTime.Now;
                //double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;
                //while (DateTime.Now.Subtract(start).TotalMilliseconds <= time)
                //{
                //    App.obj_Chuck.SWPosTrig();
                //    Thread.Sleep(delayTime);
                //}
                App.obj_Chuck.SWPosTrig();
                //4. 取图
                App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].acq.Dispose();
                string errStr = "";
                errStr = App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].CaputreImages(ref App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].acq.Image, 1, 5000);//获取图片
                if (errStr != "")
                {
                    HTUi.PopError(errStr);
                    btnSnap.Enabled = true;
                }
                else
                {
                    App.obj_Vision.ShowImage(htWindow, App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].acq.Image, null);
                }
                if (!timer_Grab.Enabled) btnSnap.Enabled = true;
            }
            catch (Exception EXP)
            {
                HTUi.PopError(EXP.Message);
                btnSnap.Enabled = true;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (App.obj_Vision.RunMode != 1)
                {
                    _selectIndex = dgvWorkImagesInfo.CurrentRow.Index;

                    ImageInformation.ConfigAllData(_selectIndex);
                }
                //DateTime t = DateTime.Now;
                //HTLog.Info(DateTime.Now.Subtract(t).TotalMilliseconds.ToString());
                if (cameraSave() == false)
                {
                    throw new Exception("保存相机参数失败");
                }

                App.obj_Vision.imageFolder = this.txtImageFolder.Text;
                if (App.obj_Vision.SaveImageFolder() == false)
                {
                    throw new Exception("保存视觉路径失败");
                }
                if (App.obj_Chuck.SaveFps() == false)
                {
                    throw new Exception("保存拍照帧率失败");
                }
                App.obj_ImageInformSet = (BindingList<ImageInformation>)this.dgvWorkImagesInfo.DataSource;
                foreach (ImageInformation item in App.obj_ImageInformSet)
                {
                    if (!item.Save())
                    {
                        throw new Exception("保存工作时拍照图像参数失败");
                    }
                }
                ProductMagzine.ImageNum = App.obj_ImageInformSet.Count;
                App.obj_Pdt.SaveImageNum();

                HTUi.TipHint("配置并保存当前参数成功");
                HTLog.Info("配置并保存当前参数成功");
            }
            catch (Exception EXP)
            {
                HTLog.Error(EXP.Message);
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //if (tabImage.SelectedTabIndex == 0)
                //{
                //    App.obj_ImageInfo.CameraName = cmbCameraList.SelectedIndex;
                //    App.obj_ImageInfo.insertData();
                //    App.obj_ImageInfo.FillDataSet();
                //}
                //else
                //{
                String temp;
                App.obj_Pdt.ReadActivePdtName(out temp);
                int i = App.obj_ImageInformSet.Count;
                ImageInformation imageInform = new ImageInformation(App.ProductDir + @"\" + temp + @"\" + "Image.db", "Image" + i.ToString());
                imageInform.CameraName = int.Parse(cmbCameraList.SelectedItem.ToString().Replace("Camera_", ""));
                App.obj_ImageInformSet.Add(imageInform);
                RefreshImageList();
                //}
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_ImageInformSet.Count > 0)
                    App.obj_ImageInformSet.RemoveAt(App.obj_ImageInformSet.Count - 1);
                RefreshImageList();
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
            }
        }
        
        private void brnAutoFocus_Click(object sender, EventArgs e)
        {
            if (!HTUi.PopYesNo("是否启用Z轴线扫？")) return;
            try
            {
                string _3DPath = App.obj_Vision.imageFolder + "\\3DAutoFocus";
                if (Directory.Exists(_3DPath)) Directory.Delete(_3DPath, true);
                if (_selectIndex != dgvWorkImagesInfo.CurrentRow.Index)
                {
                    _selectIndex = dgvWorkImagesInfo.CurrentRow.Index;

                    ImageInformation.ConfigAllData(_selectIndex);
                }
                HTuple Z_TrigPos;
                HObject ho_SharpIamge;
                App.obj_Process.Auto3DFocus(App.obj_ImageInformSet[_selectIndex].CameraName, App.obj_ImageInformSet[_selectIndex].Fps, App.obj_ImageInformSet[_selectIndex].ZFocusStart,
                   App.obj_ImageInformSet[_selectIndex].ZFocusEnd, App.obj_ImageInformSet[_selectIndex].TrigInterval, out Z_TrigPos, out ho_SharpIamge);

                Directory.CreateDirectory(_3DPath);
                int num = Z_TrigPos.Length;
                for (int i = 0; i < num; i++)
                {
                    HOperatorSet.WriteImage(ho_SharpIamge.SelectObj(i + 1), "tiff", 0, _3DPath + "\\" + "Snap" + _selectIndex + "-" + i + ".tiff");
                }
                HOperatorSet.WriteTuple(Z_TrigPos, _3DPath + "\\Z_TrigPoses.dat");
                HTUi.TipHint("自动聚焦成功");
                HTLog.Info("自动聚焦成功");
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
            }
        }
        

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (HTM.LoadUI() < 0)
            {
                HTUi.PopError("打开轴调试助手界面失败");
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

        private void btn_CheckPos_Click(object sender, EventArgs e)
        {
            if (!App.obj_Process.GetImagePositions_PointAuto(htWindow))
            {
                HTUi.PopError("初始化扫描位置信息失败!");
                goto _end;
            }
        _end:
            //移动到安全位
            if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                App.obj_Chuck.ref_y,
                App.obj_Chuck.z_Safe))
            {
                HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
            }
        }



        private void cbBox_ImgSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbBox_ImgSelect.SelectedIndex == -1) return;
                if (ImageOrigin == null) return;
                if (!ImageOrigin.IsInitialized()) return;
                if (ImageOrigin != null && ImageOrigin.IsInitialized())
                {
                    if (App.obj_Vision.ListImgRegion != null)
                        htWindow.Region = App.obj_Vision.ListImgRegion[cbBox_ImgSelect.SelectedIndex];
                    if (App.obj_Vision.ListImgRegion != null)
                        ShowSelectImageChannel(ImageOrigin.SelectObj(cbBox_ImgSelect.SelectedIndex + 1));
                    SelectImgIdx = cbBox_ImgSelect.SelectedIndex;
                }
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
                if (cbBox_ImgSelect.Items.Count > SelectImgIdx) cbBox_ImgSelect.SelectedIndex = SelectImgIdx;
            }
        }

        private void cbBox_ChannelSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbBox_ChannelSelect.SelectedIndex == -1) return;
                SelectChannelIdx = cbBox_ChannelSelect.SelectedIndex;
                if (ImageOrigin == null) return;
                if (!ImageOrigin.IsInitialized()) return;
                if (ImageOrigin != null && ImageOrigin.IsInitialized())
                {
                    if (App.obj_Vision.ListImgRegion != null)
                        htWindow.Region = App.obj_Vision.ListImgRegion[SelectImgIdx];
                    ShowSelectImageChannel(ImageOrigin.SelectObj(SelectImgIdx + 1));
                }
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
                if (cbBox_ChannelSelect.Items.Count != 0) cbBox_ChannelSelect.SelectedIndex = 0;
            }
        }

        private async void btnMoveToCode2D_Click(object sender, EventArgs e)
        {

            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                //1.移动
                if (!App.obj_Chuck.XYZ_Move(App.obj_Pdt.X_Code2D, App.obj_Pdt.Y_Code2D, App.obj_Pdt.ZFocus)) return false;
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
                    ShowImage(htWindow, App.obj_Vision.Image, null);
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

        private void btnScanCode2D_Click(object sender, EventArgs e)
        {

            if (!App.obj_Vision.ScanCode2D())
            {
                HTLog.Error("二维码识别失败!");
                return;
            }
            HTLog.Info("二维码为:" + App.obj_Vision.frameName);
        }

        private void btnTrainCode2D_Click(object sender, EventArgs e)
        {
            if (!App.obj_Vision.TrainCode2D(htWindow))
            {
                HTLog.Error("二维码训练失败!");
                return;
            }
            if (HTUi.PopYesNo("二维码为:" + App.obj_Vision.frameName + "，是否保存该训练文件?"))
            {
                HOperatorSet.WriteDataCode2dModel(App.obj_Vision.hv_dataCodeHandle, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "Code2D.dcm");
            }
        }

        private void btnOpenImageFolder_Click(object sender, EventArgs e)
        {
            if (App.obj_Vision.imageFolder == "") return;
            System.Diagnostics.Process.Start("explorer.exe", App.obj_Vision.imageFolder);
        }

        private void dgvWorkImagesInfo_CellContent_Click(object sender, DataGridViewCellEventArgs e)
        {
            //_selectIndex = dgvWorkImagesInfo.CurrentRow.Index;
        }

        private void btnEnhanLight_Click(object sender, EventArgs e)
        {
            App.enHance_Light.Close();
            System.Diagnostics.Process.Start(App.EnhanLightPath);
        }

        private async void btn_Mark_Click(object sender, EventArgs e)
        {
            if (App.obj_SystemConfig.Marking && App.obj_Pdt.UseMarker)
            {
                if (App.obj_Chuck.IsHaveInk() == 0)
                {
                    HTUi.PopError("喷印器无墨水！");
                    return;
                }
                if (App.obj_Chuck.IsJetRun() == 0)
                {
                    HTUi.PopError("喷印器未启动！");
                    return;
                }
            }
            else
            {
                HTUi.PopError("未启用喷印器！");
                return;
            }
            if (App.obj_Process.ThisMarkPostions == null)
            {
                return;
            }
            if (App.obj_Process.ThisMarkPostions.Count == 0)
            {
                return;
            }
            if (App.obj_Vision.ScanMapPostions == null)
            {
                HTUi.PopError("无法扫描！该产品扫描点位为空！\n请在图谱生成界面生成扫描点位！");
                return;
            }
            if (ScanWork)
            {
                ScanWork = false;
                btnScan.Enabled = true;
                btn_Mark.Text = "标记芯片";
            }
            else
            {
                ScanWork = true;
                btnScan.Enabled = false;
                btn_Mark.Text = "停止标记";
                await Task.Run(new Action(MarkNG));
            }
        }
        private void btnSnapClip_Click(object sender, EventArgs e)
        {
            double targetX = App.obj_Vision.dieMatrix[App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].realRow,
                App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].realCol].x;
            double targetY = App.obj_Vision.dieMatrix[App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].realRow,
                App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].realCol].y;

            App.obj_Vision.SnapPos(targetX, targetY, App.obj_Pdt.ZFocus, htWindow, out App.obj_Vision.Image);
        }
        private async void btnMutiScan_Click(object sender, EventArgs e)
        {
            if (MutiScanFlag)
            {
                MutiScanFlag = false;
                btnMutiScan.Text = "连续扫描";
                return;
            }
            await Task.Run(() =>
            {
                MutiScanFlag = true;
                btnMutiScan.Invoke(new Action(() => btnMutiScan.Text = "停止连扫"));
                while (MutiScanFlag)
                {
                    btnScan.Invoke(new Action(()=>btnScan_Click(null, null)));
                    while(ScanWork)
                    {
                        Thread.Sleep(500);
                    }
                }
            });
        }
    }
}
