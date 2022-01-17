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
using HTHalControl;
using System.IO;
using IniDll;
using HT_Lib;

namespace LeadframeAOI
{
    public partial class FunctionTest : Form
    {
        public static FunctionTest Instance = null;
        public string filefolder = "";
        public Int32 rowInsepctIndex = 0;
        public Int32 columnInsepctIndex = 0;
        public BindingList<UsualInspectPara> List_UsualInspectPara = new BindingList<UsualInspectPara>();
        
        private HObject ImageShown;
        private HObject ImageOrigin;
        private HObject RegionOrigin;
        int SelectImgIdx = 0;
        int SelectChannelIdx = 0;
        bool workflag = false;
        public FunctionTest()
        {
            InitializeComponent();
            Instance = this;
        }

        public void SetupUI()
        {
            try
            {
                HOperatorSet.SetSystem("width", 5000);
                HOperatorSet.SetSystem("height", 5000);
                HOperatorSet.SetSystem("clip_region", "false");
                App.formIniConfig.LoadObj(this);
                tbImagefolder.Text = filefolder;
                tbImagefolder.Text = App.obj_Vision.frameFolder;
                dgvInspectionPara.DataSource = List_UsualInspectPara;
                foreach (DataGridViewColumn item in this.dgvInspectionPara.Columns)
                {
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                Setup_List_UsualInspectPara();
                this.dgvInspectionPara.Columns[2].ReadOnly = true;
                RefreshLotList();
                mappingControl1.OnSelectedDieChanged += ShowInspectData;
                mappingControl2.OnSelectedDieChanged += ShowClipData_Local;
            }
            catch (Exception ex)
            {
                HTUi.PopError(this.GetType().Name + "UI加载失败！\n" + ex.ToString());
            }
        }
        #region 方法
        /// <summary>
        /// 更新常用检测参数列表
        /// </summary>
        public void Setup_List_UsualInspectPara()
        {
            const int MaxList = 50;
            List_UsualInspectPara.Clear();
            int Cnt = 0;
            UsualInspectPara[] usualInspectParas = new UsualInspectPara[MaxList];
            usualInspectParas[Cnt++] = new UsualInspectPara("主IC检测区0的最小脏污面积阈值", App.obj_AlgApp.P.mainIcPara[0].MinArea.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("主IC芯片行像素偏离阈值", App.obj_AlgApp.P.MainIcRowThr.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("主IC芯片列像素偏离阈值", App.obj_AlgApp.P.MainIcColThr.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("主IC芯片角度偏离阈值", App.obj_AlgApp.P.MainIcAngleThr.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("主IC检测区1的最小脏污面积阈值", App.obj_AlgApp.P.mainIcPara[1].MinArea.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("最小焊球半径", App.obj_AlgApp.P.MinBall1Rad.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("最大焊球半径", App.obj_AlgApp.P.MaxBall1Rad.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("最大断线距离", App.obj_AlgApp.P.MaxWireGap.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶上溢截面芯片比阈值", App.obj_AlgApp.P.EpoxyLenUp.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶下溢截面芯片比阈值", App.obj_AlgApp.P.EpoxyLenDown.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶左溢截面芯片比阈值", App.obj_AlgApp.P.EpoxyLenLeft.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶右溢截面芯片比阈值", App.obj_AlgApp.P.EpoxyLenRight.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶上溢阈值", App.obj_AlgApp.P.EpoxyHeiUp.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶下溢阈值", App.obj_AlgApp.P.EpoxyHeiDown.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶左溢阈值", App.obj_AlgApp.P.EpoxyHeiLeft.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶右溢阈值", App.obj_AlgApp.P.EpoxyHeiRight.ToString(), "");

            usualInspectParas[Cnt++] = new UsualInspectPara("粗匹配区外扩尺寸", App.obj_AlgApp.P.CoarseDilationSize.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("粗匹配区最小匹配分", App.obj_AlgApp.P.CoarseMinScore.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("主IC最小匹配分", App.obj_AlgApp.P.MainIcMinScore.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("锡球最小匹配分", App.obj_AlgApp.P.Bond2MinScore.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("金线检测区域长度", App.obj_AlgApp.P.WireSearchLen.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("金线检测剪切长度", App.obj_AlgApp.P.WireClipLen.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("金线检测宽度", App.obj_AlgApp.P.WireWidth.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("金线检测对比度", App.obj_AlgApp.P.WireContrast.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("金线检测最短长度", App.obj_AlgApp.P.WireMinSegLen.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶对比度", App.obj_AlgApp.P.EpoxyContrast.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶外扩尺寸", App.obj_AlgApp.P.EpoxyDilationSize.ToString(), "");
            usualInspectParas[Cnt++] = new UsualInspectPara("银胶图索引", App.obj_AlgApp.P.EpoxyImgIdx.ToString(), "");
            for (int i = 0; i < Cnt; i++)
            {
                List_UsualInspectPara.Add(usualInspectParas[i]);
            }
        }
        /// <summary>
        /// 保存常用检测参数列表
        /// </summary>
        public void Save_List_UsualInspectPara()
        {
            int Cnt = 0;
            App.obj_AlgApp.P.mainIcPara[0].MinArea = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MainIcRowThr = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MainIcColThr = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MainIcAngleThr = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.mainIcPara[1].MinArea = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MinBall1Rad = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MaxBall1Rad = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MaxWireGap = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyLenUp = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyLenDown = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyLenLeft = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyLenRight = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyHeiUp = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyHeiDown = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyHeiLeft = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyHeiRight = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);

            App.obj_AlgApp.P.CoarseDilationSize = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.CoarseMinScore = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.MainIcMinScore = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.Bond2MinScore = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.WireSearchLen = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.WireClipLen = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.WireWidth = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.WireContrast = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.WireMinSegLen = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyContrast = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyDilationSize = Double.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            App.obj_AlgApp.P.EpoxyImgIdx = Int32.Parse(List_UsualInspectPara[Cnt++].ParaConfig);
            if (!App.obj_AlgApp.P.Save(App.AlgParamsPath))
            {
                HTUi.PopError("保存失败！\n");
                return;
            }
            App.obj_AlgApp.Initialization(App.obj_Vision.ThreadMax, App.AlgParamsPath, App.ModelPath, App.Fuc_LibPath, App.Json_Path);
        }
        /// <summary>
        /// 填写常用检测参数列表
        /// </summary>
        /// <param name="inspectDetail">芯片检测细节</param>
        public void Set_List_InspectResult(InspectDetail inspectDetail)
        {
            int Cnt = 0;
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.mainIcInspects.DirtyArea.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.icLoctionDiffs.RowDiff.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.icLoctionDiffs.ColDiff.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.icLoctionDiffs.AngleDiff.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.mainIcInspects.DirtyArea.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.bondWiresInspects.Radius_FirstBond.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.bondWiresInspects.Radius_FirstBond.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.bondWiresInspects.Distance_WireBreak.ToString();
            if (inspectDetail.epoxyInspect.Lenth_EpoxyOut.Length == 0)
            {
                Cnt += 4;
                goto _A;
            }
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.Lenth_EpoxyOut[0].D.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.Lenth_EpoxyOut[1].D.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.Lenth_EpoxyOut[2].D.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.Lenth_EpoxyOut[3].D.ToString();
            _A:
            if (inspectDetail.epoxyInspect.E2C_Ratio.Length == 0)
            {
                Cnt += 4;
                goto _B;
            }
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.E2C_Ratio[0].D.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.E2C_Ratio[1].D.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.E2C_Ratio[2].D.ToString();
            List_UsualInspectPara[Cnt++].ParaResult = inspectDetail.epoxyInspect.E2C_Ratio[3].D.ToString();
            _B:
            dgvInspectionPara.Refresh();
        }
        /// <summary>
        /// 更新图像ListBox选择项
        /// </summary>
        /// <param name="Img">图像，根据concat一起的数目来</param>
        private void RefreshImageList(HObject Img)
        {
            cbBox_ImgSelect.Invoke(new MethodInvoker(() =>
            {
                HTuple ImgCount = null;
                HOperatorSet.CountObj(Img, out ImgCount);
                int sltIdx = cbBox_ImgSelect.SelectedIndex;
                cbBox_ImgSelect.Items.Clear();
                for (int i = 0; i < ImgCount.I; i++)
                {
                    cbBox_ImgSelect.Items.Add("第" + (i + 1) + "张图");
                }
                if (sltIdx == -1 || sltIdx >= ImgCount.I)
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
        /// 更新通道ListBox选择项
        /// </summary>
        /// <param name="Img"></param>
        private void RefreshChannelList(HObject Img)
        {
            cbBox_ChannelSelect.Invoke(new MethodInvoker(() =>
            {
                HTuple ChannelCount = null;
                HOperatorSet.CountChannels(Img, out ChannelCount);
                int sltIdx = cbBox_ChannelSelect.SelectedIndex;
                cbBox_ChannelSelect.Items.Clear();
                cbBox_ChannelSelect.Items.Add("原图");
                if (ChannelCount.I > 1)
                {
                    for (int i = 0; i < ChannelCount.I; i++)
                    {
                        cbBox_ChannelSelect.Items.Add("通道" + (i + 1));
                    }
                }
                if (sltIdx == -1 || sltIdx >= ChannelCount.I)
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
        /// 更新批次列表
        /// </summary>
        public void RefreshLotList()
        {
            try
            {
                listBox_Lot.Items.Clear();
                listBox_Frame.Items.Clear();
                DirectoryInfo dr = null;
                DirectoryInfo[] drKids = null;
                if (!Directory.Exists(App.obj_Vision.imageFolder + "\\Scan" + "\\" + ProductMagzine.ActivePdt))
                {
                    return;
                }
                dr = new DirectoryInfo(App.obj_Vision.imageFolder + "\\Scan" + "\\" + ProductMagzine.ActivePdt);
                drKids = dr.GetDirectories();
                for (int i = 0; i < drKids.Length; i++)
                {
                    listBox_Lot.Items.Add(drKids[i].ToString());
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("列表刷新失败！\n" + ex.ToString());
            }
        }
        /// <summary>
        /// 更新料片列表
        /// </summary>
        public void RefreshFrameList()
        {
            try
            {
                listBox_Frame.Items.Clear();
                DirectoryInfo dr = null;
                DirectoryInfo[] drKids = null;
                if (!Directory.Exists(App.obj_Vision.imageFolder + "\\Scan" + "\\" + ProductMagzine.ActivePdt + "\\" + listBox_Lot.SelectedItem.ToString()))
                {
                    return;
                }
                dr = new DirectoryInfo(App.obj_Vision.imageFolder + "\\Scan" + "\\" + ProductMagzine.ActivePdt + "\\" + listBox_Lot.SelectedItem.ToString());
                drKids = dr.GetDirectories();
                for (int i = 0; i < drKids.Length; i++)
                {
                    listBox_Frame.Items.Add(drKids[i].ToString());
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("列表刷新失败！\n" + ex.ToString());
            }
        }
        /// <summary>
        /// 显示图片的相应通道
        /// </summary>
        /// <param name="Img">单张的图片，countObj()为1</param>
        public void ShowSelectImageChannel(HObject Img)
        {
            if (SelectChannelIdx == 0)
            {
                App.obj_Vision.ShowImage(htWindow, Img, htWindow.Region);
            }
            else
            {
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
        /// 检测相应料片视野图谱位置的图片，并显示结果
        /// </summary>
        /// <param name="dieInfo">料片图谱位置状态信息</param>
        private void ShowInspectData(HTMappingControl.DieInfo dieInfo)
        {
            try
            {
                rowInsepctIndex = dieInfo.RowIndex;
                columnInsepctIndex = dieInfo.ColumnIndex;
                tbNowRowIdx.Text = rowInsepctIndex.ToString();
                tbNowColIdx.Text = columnInsepctIndex.ToString();
                if (!checkBox_Inspect.Checked &&btnInspect.Enabled) return;
                btnInspect.Enabled = false;
                HOperatorSet.SetFont(htWindow.HTWindow.HalconWindow, "-Courier New-32-*-*-*-*-1-");
                if (tbImagefolder.Text.Trim() != "") filefolder = tbImagefolder.Text.Trim();


                HObject tempImg;
                DirectoryInfo dr = new DirectoryInfo(filefolder);
                DirectoryInfo[] drKids = dr.GetDirectories();
                if (drKids.Length < 1)
                {
                    MessageBox.Show("该目录下没有die");
                    return;
                }

                ImageCache imgCache = new ImageCache();
                imgCache.r = dieInfo.RowIndex;
                imgCache.c = dieInfo.ColumnIndex;
                dr = new DirectoryInfo(filefolder + "\\Row" + imgCache.r + "-" + imgCache.c);
                if (!Directory.Exists(filefolder + "\\Row" + imgCache.r + "-" + imgCache.c))
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
                        HOperatorSet.ReadImage(out tempImg, filefolder + "\\Row"
                            + imgCache.r + "-" + imgCache.c + "\\"
                            + fi[i].ToString());
                        HOperatorSet.ConcatObj(imgCache._2dImage, tempImg, out imgCache._2dImage);
                    }
                }
                if (!haveImage)
                {
                    MessageBox.Show("该视野无图片！");
                    return;
                }
                List<StructInspectResult> inspectResults;
                HTuple SnapX = null, SnapY = null;
                HOperatorSet.ReadTuple(filefolder + "\\Row" + imgCache.r + "-" + imgCache.c + "\\" + "SnapX.tup", out SnapX);
                HOperatorSet.ReadTuple(filefolder + "\\Row" + imgCache.r + "-" + imgCache.c + "\\" + "SnapY.tup", out SnapY);
                HOperatorSet.ReadTuple(filefolder + "\\" + "updateMapX.tup", out App.obj_Vision.hv_updateMapX);
                HOperatorSet.ReadTuple(filefolder + "\\" + "updateMapY.tup", out App.obj_Vision.hv_updateMapY);
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
                RefreshImageList(ImageOrigin);
                RefreshChannelList(ImageOrigin);
                DateTime t = DateTime.Now;
                SnapDataResult sdr;
                if (!App.obj_Vision.Inspection(imgCache, htWindow, SelectImgIdx, false, out inspectResults, out IsAllOkSnap,out sdr))
                {
                    MessageBox.Show("算法检测失败！\n" + App.obj_Vision.GetLastErrorString());
                    return;
                }
                HTLog.Info("检测用时：" + DateTime.Now.Subtract(t).TotalMilliseconds + "ms");
                if (IsAllOkSnap)
                {
                    if (sdr.clipRowNumInImg == 0)
                        ShowPRODState(5, imgCache.r, imgCache.c);
                    else
                        ShowPRODState(1, imgCache.r, imgCache.c);
                }
                else ShowPRODState(2, imgCache.r, imgCache.c);
                mappingControl2.Enabled = true;
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
                imgCache.Dispose();
            }
            catch (Exception ex)
            {
                HTUi.PopError("查看该视野信息失败\n" + ex.ToString());
            }
            finally
            {
                btnInspect.Enabled = true;
            }
        }
        /// <summary>
        /// 显示芯片的检测结果信息
        /// </summary>
        /// <param name="dieInfo"></param>
        private void ShowClipData_Local(HTMappingControl.DieInfo dieInfo)
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
                Set_List_InspectResult(App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].inspectDetail);

                listBox_Defect.Items.Clear();
                HTuple DefectType = App.obj_Vision.clipResults[mappingControl2.SelectedDieInfo.RowIndex, mappingControl2.SelectedDieInfo.ColumnIndex].defectType;

                Dictionary<int,string> Set_DefectType = new Dictionary<int, string>();
                using (StreamReader sr = new StreamReader(App.programDir + "\\DefectTypeNew.txt", Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Dictionary<int, string> defectType = new Dictionary<int, string>();
                        string[] strs = line.Split(' ');
                        Set_DefectType.Add(Int32.Parse(strs[0]), strs[1]);
                    }
                }
                for (int i=0;i< DefectType.Length;i++)
                {
                    listBox_Defect.Items.Add(DefectType[i].I.ToString() +":"+ Set_DefectType[DefectType[i].I]);
                }
                if (ImageOrigin != null) ImageOrigin.Dispose();
                ImageOrigin = clipImg.CopyObj(1,-1);
                RefreshImageList(ImageOrigin);
                RefreshChannelList(ImageOrigin);
                HOperatorSet.SetDraw(htWindow.HTWindow.HalconWindow, "margin");
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "yellow");
                if (htWindow.Image != null)
                {
                    if (htWindow.Image.IsInitialized())
                        htWindow.Image.Dispose();
                }
                HObject tempImg = ImageOrigin.SelectObj(SelectImgIdx + 1);
                RegionOrigin = defectRegion;
                htWindow.Region = RegionOrigin;
                ShowSelectImageChannel(tempImg);
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                HOperatorSet.DispXld(wireRegion, htWindow.HTWindow.HalconWindow);
                HOperatorSet.SetTposition(htWindow.HTWindow.HalconWindow, 0, 0);  //设置字体
                                                                                  //if (!IsAllOkSnap)
                                                                                  //    HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "red");
                HOperatorSet.WriteString(htWindow.HTWindow.HalconWindow,
                    "Clip " +(App.obj_Vision.clipRowMinInImg+ mappingControl2.SelectedDieInfo.RowIndex) + "-" + 
                    (App.obj_Vision.clipColMinInImg+mappingControl2.SelectedDieInfo.ColumnIndex)); //设置文字
            }
            catch (Exception ex)
            {
                HTUi.PopError("查看该视野信息失败\n" + ex.ToString());
            }
        }
        delegate void ShowProdStateDelegate(int ProdState, int row, int column);
        /// <summary>
        /// 更新图谱某点的状态
        /// </summary>
        /// <param name="ProdState">状态</param>
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
                        mappingControl1.SetDieState(row, column, "匹配失败");
                        break;
                    default:
                        break;
                }
            }
        }
        private Dictionary<string, Color> dic_DieState_Color;
        /// <summary>
        /// 显示初始视野mapping
        /// </summary>
        public void ShowProdMap()
        {
            //新建颜色字典
            dic_DieState_Color = new Dictionary<string, Color>();
            dic_DieState_Color.Add("未检测", Color.SkyBlue);
            dic_DieState_Color.Add("合格", Color.Green);
            dic_DieState_Color.Add("不合格", Color.Red);
            ////dic_DieState_Color.Add("复看合格", Color.Yellow);
            //dic_DieState_Color.Add("无芯片", Color.Purple); 
            dic_DieState_Color.Add("匹配失败", Color.Pink);
            //初始化图谱
            mappingControl1.BeginInvoke(
               new MethodInvoker(() =>
               {
                   if (App.obj_SystemConfig.ScanMode == 0)
                   {
                       mappingControl1.Initial(App.obj_Vision.scanRowNum,
     App.obj_Vision.scanColNum, dic_DieState_Color, "未检测");
                   }
                   else if (App.obj_SystemConfig.ScanMode == 1)
                   {
                       mappingControl1.Initial(App.obj_MobSht.NoLineAxisMoveCount,
                           App.obj_MobSht.LineAxisTrigCount, dic_DieState_Color, "未检测");
                   }
               }));
        }

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
        /// <summary>字典颜色</summary>
        private Dictionary<string, Color> dic_DieState_Color2;

        /// <summary>
        /// 显示初始芯片mapping
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
        #endregion

        private void btnInspect_Click(object sender, EventArgs e)
        {
            if (mappingControl1.SelectedDieInfo == null)
            {
                MessageBox.Show("请生成图谱并选中视野");
                return;
            }
            btnInspect.Enabled = false;
            ShowInspectData(mappingControl1.SelectedDieInfo);
        }
        private async void btInspection2_Click(object sender, EventArgs e)
        {
            try
            {
                if (workflag)
                {
                    workflag = false;
                    btInspection2.Text = "连续检测";
                    return;
                }
                HOperatorSet.SetFont(htWindow.HTWindow.HalconWindow, "-Courier New-32-*-*-*-*-1-");

                filefolder = @"E:\ImageFolder";
                if (tbImagefolder.Text.Trim() != "") filefolder = tbImagefolder.Text.Trim();

                DirectoryInfo dr = new DirectoryInfo(filefolder);
                DirectoryInfo[] drKids = dr.GetDirectories();
                if (drKids.Length < 1)
                {
                    MessageBox.Show("该目录下没有die");
                    btnInspect.Enabled = true;
                    return;
                }
                mappingControl2.Enabled = false;
                ShowProdMap();
                btInspection2.Text = "停止检测";
                workflag = true;

                await Task.Run(() =>
                {
                    
                        Parallel.For(0, App.obj_Vision.ThreadMax, tIdx =>
                        {
                            for (int insepctIdx = 0; insepctIdx < App.obj_Vision.scanRowNum * App.obj_Vision.scanColNum; insepctIdx += App.obj_Vision.ThreadMax)
                            {
                                int rowIndex = (insepctIdx + tIdx) / App.obj_Vision.scanColNum;
                                int columnIndex = (insepctIdx + tIdx) % App.obj_Vision.scanColNum;
                                if (rowIndex >= App.obj_Vision.scanRowNum) return;
                                //if (rowIndex == 0 && columnIndex == 0)
                                //{
                                //    rowIndex = rowInsepctIndex;
                                //    columnIndex = columnInsepctIndex;
                                //}
                                tbNowRowIdx.BeginInvoke(new MethodInvoker(() => { tbNowRowIdx.Text = rowIndex.ToString(); }));
                                tbNowColIdx.BeginInvoke(new MethodInvoker(() => { tbNowColIdx.Text = columnIndex.ToString(); }));
                                if (!workflag)
                                {
                                    rowInsepctIndex = rowIndex;
                                    columnInsepctIndex = columnIndex;
                                    return;
                                }
                                dr = new DirectoryInfo(filefolder + "\\Row" + rowIndex + "-" + columnIndex);
                                if (!Directory.Exists(filefolder + "\\Row" + rowIndex + "-" + columnIndex))
                                {
                                    HTLog.Error("无视野信息:\n" + "Row" + rowIndex + "-" + columnIndex);
                                    return;
                                }
                                FileInfo[] fi = dr.GetFiles();
                                bool haveImage = false;
                                ImageCache imgCache = new ImageCache();
                                HObject tempImg;
                                imgCache.r = rowIndex;
                                imgCache.c = columnIndex;
                                for (int i = 0; i < fi.Length; i++)
                                {
                                    if (fi[i].ToString().Contains(".tiff"))
                                    {
                                        haveImage = true;
                                        HOperatorSet.ReadImage(out tempImg, filefolder + "\\Row"
                                        + rowIndex + "-" + columnIndex + "\\"
                                        + fi[i].ToString());
                                        HOperatorSet.ConcatObj(imgCache._2dImage, tempImg, out imgCache._2dImage);
                                        tempImg.Dispose();
                                    }
                                }
                                if (!haveImage)
                                {
                                    HTLog.Error("视野" + "Row" + rowIndex + "-" + columnIndex + "无图片！");
                                    return;
                                }
                                List<StructInspectResult> inspectResults;
                                HTuple SnapX = null, SnapY = null;
                                HOperatorSet.ReadTuple(filefolder + "\\Row" + rowIndex + "-" + columnIndex + "\\" + "SnapX.tup", out SnapX);
                                HOperatorSet.ReadTuple(filefolder + "\\Row" + rowIndex + "-" + columnIndex + "\\" + "SnapY.tup", out SnapY);
                                HOperatorSet.ReadTuple(filefolder + "\\" + "updateMapX.tup", out App.obj_Vision.hv_updateMapX);
                                HOperatorSet.ReadTuple(filefolder + "\\" + "updateMapY.tup", out App.obj_Vision.hv_updateMapY);
                                imgCache.X = SnapX.D;
                                imgCache.Y = SnapY.D;
                                bool IsAllOkSnap = false;
                                ImageOrigin = imgCache._2dImage;
                                RefreshImageList(imgCache._2dImage);
                                RefreshChannelList(imgCache._2dImage);
                                SnapDataResult sdr;
                                if (!App.obj_Vision.Inspection(imgCache, htWindow, SelectImgIdx, false, out inspectResults, out IsAllOkSnap, out sdr, tIdx))
                                {
                                    HTLog.Error("检测失败！\n" + "视野" + "Row" + rowIndex + "-" + columnIndex + "算法检测失败！\n" + App.obj_Vision.GetLastErrorString());
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
                                //ShowProdMap2();
                                //for (int k = 0; k < inspectResults.Count; k++)
                                //{
                                //    if (inspectResults[k].OkOrNg)
                                //    {
                                //        ShowPRODState2(1, inspectResults[k].realRow - sdr.clipRowMinInImg, inspectResults[k].realCol - sdr.clipColMinInImg);
                                //    }
                                //    else
                                //    {
                                //        ShowPRODState2(2, inspectResults[k].realRow - sdr.clipRowMinInImg, inspectResults[k].realCol - sdr.clipColMinInImg);
                                //    }
                                //}
                                imgCache.Dispose();
                            }
                        });
                    rowInsepctIndex = 0;
                    columnInsepctIndex = 0;
                    GC.Collect();
                    workflag = false;
                    btInspection2.BeginInvoke(new Action(() => { btInspection2.Text = "连续检测"; }));
                });
            }
            catch
            {
                btInspection2.Enabled = true;
                MessageBox.Show("检测失败！");
            }
        }


        private void btnConfig_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            Fbd.SelectedPath = tbImagefolder.Text;
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                tbImagefolder.Text = Fbd.SelectedPath;
            }
        }

        private void btnSavePara_Click(object sender, EventArgs e)
        {
            try
            {
                filefolder = tbImagefolder.Text;
                App.formIniConfig.SaveObj(this);
                Save_List_UsualInspectPara();
                HTUi.TipHint("保存成功!");
                HTLog.Info("保存成功!");
            }
            catch(Exception ex)
            {
                HTLog.Error("保存失败!\n"+ex.ToString());
            }
        }

        private void listBox_Frame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_Frame.SelectedIndex == -1) return;
            rowInsepctIndex = 0;
            columnInsepctIndex = 0;
            ShowProdMap();
            tbImagefolder.Text = App.obj_Vision.imageFolder + "\\Scan" + "\\" + ProductMagzine.ActivePdt + "\\" + listBox_Lot.SelectedItem.ToString() + "\\" + listBox_Frame.SelectedItem.ToString();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            RefreshLotList();
        }

        private void btnRefreshMap_Click(object sender, EventArgs e)
        {
            mappingControl1.InitaialDieState();
            mappingControl1.OnSelectedDieChanged -= ShowInspectData;
            mappingControl2.OnSelectedDieChanged -= ShowClipData_Local;
            mappingControl1.OnSelectedDieChanged += ShowInspectData;
            mappingControl2.OnSelectedDieChanged += ShowClipData_Local;
        }

        private void listBox_Lot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_Lot.SelectedIndex == -1) return;
            RefreshFrameList();
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
                    ShowSelectImageChannel(ImageOrigin.SelectObj(cbBox_ImgSelect.SelectedIndex + 1));
                    SelectImgIdx = cbBox_ImgSelect.SelectedIndex;
                }
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
                if(cbBox_ImgSelect.Items.Count> SelectImgIdx) cbBox_ImgSelect.SelectedIndex = SelectImgIdx;
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

        private void FunctionTest_Enter(object sender, EventArgs e)
        {
            Setup_List_UsualInspectPara();
        }

        private void listBox_Defect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_Defect.SelectedIndex == -1) return;
            HObject tempImg = ImageOrigin.SelectObj(SelectImgIdx + 1);
            int CounterDefect = RegionOrigin.CountObj();
            if (listBox_Defect.SelectedIndex >= CounterDefect)
            {
                htWindow.Region = new HObject();
                htWindow.Region.GenEmptyObj();
                ShowSelectImageChannel(tempImg);
                return;
            }
            htWindow.Region = RegionOrigin.SelectObj(listBox_Defect.SelectedIndex+1);
            ShowSelectImageChannel(tempImg);

        }
    }
}
