using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Collections.Concurrent;
using System.IO;
using System.Windows.Forms;
using ToolKits.HTCamera;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using HTHalControl;
using System.Runtime.Serialization.Formatters.Binary;
using Utils;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using IniDll;
using HT_Lib;
using LFAOIReview;
using System.Threading;
using VisionMethonDll;
using System.ComponentModel;
using System.Reflection;

namespace LeadframeAOI
{
    /// <summary>
    /// 常用检测参数列表单项
    /// </summary>
    public class UsualInspectPara
    {
        [DisplayNameAttribute("参数名")]
        public string ParaName { get; set; }
        [DisplayNameAttribute("配置值")]
        public string ParaConfig { get; set; }
        [DisplayNameAttribute("检测值")]
        public string ParaResult { get; set; }
        public UsualInspectPara(string paraName, string paraConfig, string paraResult)
        {
            ParaName = paraName;
            ParaConfig = paraConfig;
            ParaResult = paraResult;
        }
    }
    #region  单个拍照位图片集合 by M.bing

    /// <summary>
    /// 封装的Halcon image
    /// </summary>
    public class ImageExt
    {
        public HObject image;
        public ImageExt()
        {
            image = new HObject();
            HOperatorSet.GenEmptyObj(out image);
        }
        public void Dispose()
        {
            image.Dispose();
        }
    }

    public struct RC
    {
        public int r;
        public int c;
    }
    /// <summary>
    /// 单个拍照位图片集合
    /// </summary>
    public class ImageCache
    {
        /// <summary>
        /// 2d图片集合
        /// </summary>
        public HObject _2dImage;
        /// <summary>
        /// 2d图片名
        /// </summary>
        public List<string> _2dImgKeys;
        /// <summary>
        /// 3d图片集合
        /// </summary>
        public HObject _3dImage;
        /// <summary>
        /// 3d图片名
        /// </summary>
        public List<string> _3dImgKeys;
        /// <summary>
        /// 当前拍照位包含的所有die的rc
        /// </summary>
        public List<RC> rc;
        /// <summary>
        /// 所在block
        /// </summary>
        public int b { get; set; }
        /// <summary>
        /// 所在拍照位的行
        /// </summary>
        public int r { get; set; }
        /// <summary>
        /// 所在拍照位的列
        /// </summary>
        public int c { get; set; }
        /// <summary>
        /// 当前拍照位X
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 当前拍照位Y
        /// </summary>
        public double Y { get; set; }
        public List<HObject> List_3dImage;
        public List<HTuple> List_Z_TrigPos;

        public ImageCache()
        {
            _2dImage = new HObject();
            HOperatorSet.GenEmptyObj(out _2dImage);
            _2dImgKeys = new List<string>();

            _3dImage = new HObject();
            HOperatorSet.GenEmptyObj(out _3dImage);
            _3dImgKeys = new List<string>();
            rc = new List<RC>();
            b = 0;
            r = 0;
            c = 0;
            List_3dImage = new List<HObject>();
            List_Z_TrigPos = new List<HTuple>();
        }

        public void Dispose()
        {
            if (_2dImage != null) _2dImage.Dispose();
            if (_2dImgKeys != null) _2dImgKeys.Clear();
            if (_3dImage != null) _3dImage.Dispose();
            if (_3dImgKeys != null) _3dImgKeys.Clear();
            if (rc != null) rc.Clear();
            b = 0;
            r = 0;
            c = 0;
            if (List_3dImage != null) List_3dImage.Clear(); ;
            if (List_Z_TrigPos != null) List_Z_TrigPos.Clear(); ;
        }
    }

    #endregion

    #region  单个拍照位检测结果集合 by Weiliang Tong
    /// <summary>
    /// 每个视野的检测结果信息类
    /// </summary>
    public class SnapResult
    {
        /// <summary>相机拍照位行索引</summary>
        public int Snap_r { get; set; }
        /// <summary>相机拍照位列索引</summary>
        public int Snap_c { get; set; }
        /// <summary>相机拍照位横坐标</summary>
        public double Snap_x { get; set; }
        /// <summary>相机拍照位纵坐标</summary>
        public double Snap_y { get; set; }
        /// <summary>2d图片集合</summary>
        public HObject _2dImage;
        /// <summary>3d图片集合</summary>
        public HObject _3dImage;
        /// <summary>金线信息集合</summary>
        public HObjectVector VectorWires = null;
        /// <summary>缺陷信息集合</summary>
        public HObjectVector VectorDefectRegs = null;
        /// <summary>芯片区信息集合</summary>
        public List<HObject> ClipRegs = null;
        /// <summary>检测是否成功</summary>
        public bool InspectFlag;
        /// <summary>视野内是否均为OK芯片</summary>
        public bool IsAllOkSnap;
        /// <summary>检测结果集合</summary>
        public List<StructInspectResult> InspectResults;
        public SnapResult()
        {
            this.Snap_r = -1;
            this.Snap_c = -1;
            this.Snap_x = 0;
            this.Snap_y = 0;
            _2dImage = null;
            _3dImage = null;
            VectorWires = null;
            VectorDefectRegs = null;
            ClipRegs = null;
            InspectFlag = false;
            IsAllOkSnap = false;
            InspectResults = new List<StructInspectResult>();
        }

        public void Dispose()
        {
            if (_2dImage != null) _2dImage.Dispose();
            if (_3dImage != null) _3dImage.Dispose();
            if (VectorWires != null) VectorWires.Dispose();
            if (VectorDefectRegs != null) VectorDefectRegs.Dispose();
            if (ClipRegs != null) ClipRegs = null;
            this.Snap_r = -1;
            this.Snap_c = -1;
            this.Snap_x = 0;
            this.Snap_y = 0;
            _2dImage = null;
            _3dImage = null;
            VectorWires = null;
            VectorDefectRegs = null;
            ClipRegs = null;
            InspectFlag = false;
            IsAllOkSnap = false;
            InspectResults = null;
        }
    }
    public class SnapDataResult
    {
        public Int32 clipRowMinInImg;
        public Int32 clipRowMaxInImg;
        public Int32 clipColMinInImg;
        public Int32 clipColMaxInImg;
        public Int32 clipRowNumInImg;
        public Int32 clipColNumInImg;
        public SnapDataResult()
        {
            clipRowMinInImg = 0;
            clipRowMaxInImg = 0;
            clipColMinInImg = 0;
            clipColMaxInImg = 0;
            clipRowNumInImg = 0;
            clipColNumInImg = 0;
        }
    }
    #endregion
    #region 单芯片数据集合
    public class ClipResult
    {
        public HObject ClipImage;
        public List<HObject> ListImgClipDefect;
        public HObject ClipWire;
        public int realRow;
        public int realCol;
        public HTuple defectType;
        public HTuple defectImgIdx;
        public InspectDetail inspectDetail;
        public ClipResult()
        {
            realRow = 0;
            realCol = 0;
            inspectDetail = null;
            ListImgClipDefect = new List<HObject>();
            HOperatorSet.GenEmptyObj(out ClipImage);
            HOperatorSet.GenEmptyObj(out ClipWire);
        }
    }
    #endregion
    public class Vision : Base
    {
        public Vision(String para_file, String para_table) : base(para_file, para_table) { }
        #region 模块公共变量
        /// <summary>
        /// 主相机号
        /// </summary>
        public String cameraName0 = "";
        /// <summary>
        /// 分相机号
        /// </summary>
        public String cameraName1 = "";
        /// <summary>
        /// 分相机号
        /// </summary>
        public String cameraName2 = "";
        /// <summary>
        /// 图像存储主目录
        /// </summary>
        public String imageFolder = "";
        public String frameName = "";//disk二维码识别出的名字
        public String frameFolder = "";//图像存储位置+disk二维码识别出的名字形成的目录
        public String frameResultFolder = "";
        public Int32 InspectResult = -1;
        public String camIniPath = Application.StartupPath + "\\CameraSet.ini";
        public int scanPosNum;
        public int clipPosNum;
        public Double checkPosX;
        public Double checkPosY;
        public Double checkPosScoreThresh = 0.6;
        public Double widthFactor;
        public Double heightFactor;
        public Double sameSpace = 3;
        public Double scaleFactor = 0.125;
        public Double lctScoreThresh = 0.6;
        public Double genMapStartX;
        public Double genMapStartY;
        public Double genMapEndX;
        public Double genMapEndY;
        public Int32 scanRowNum;
        public Int32 scanColNum;
        public Int32 clipRowMinInImg;
        public Int32 clipRowMaxInImg;
        public Int32 clipColMinInImg;
        public Int32 clipColMaxInImg;
        public Int32 clipRowNumInImg;
        public Int32 clipColNumInImg;
        public int SaveThreadNow = 0;
        public int ImageThreadNow = 0;
        public int DefaultImgNum = 0;
        public int ThreadMax = 3;
        public bool IsDbLocked;

        public List<bool> ListThreadInspectionFlag = new List<bool>() { };
        public bool ThreadInspectionFlag = false;
        public bool saveResultFlag = false;//检测结果存储线程状态标识
        public String TestImagePath = String.Empty;

        protected int runMode; //0-离线 1-demo 2-online 
        public int RunMode { get { return runMode; } set { runMode = value; } }


        public List<Obj_Camera> obj_camera;
        public List<HObject> ListImgRegion;
        public ClipResult[,] clipResults;
        //public HObject ClipImage;
        //public List<HObject> ListImgClipDefect;
        //public HObject ClipWire;
        public HObject Image;
        public HObject showRegion;
        private VisionState visionState = 0;//处理任务标识

        public HTuple hv_dataCodeHandle = null;
        public List<ImagePosition> ScanMapPostions = null;
        public List<ImagePosition> ClipMapPostions = null;
        public List<ImagePosition> ThisClipPostions = null;
        public ConcurrentQueue<SnapResult> qSnapResult;
        public IniFiles scanIniConfig = null;
        public HTuple snapMapX;
        public HTuple snapMapY;
        public HTuple snapMapRow;
        public HTuple snapMapCol;
        public HTuple clipMapX = null;
        public HTuple clipMapY = null;
        public HTuple clipMapU = null;
        public HTuple clipMapV = null;
        public HTuple clipMapRow;
        public HTuple clipMapCol;
        public LeadframeAOI.Model CheckPosModels;
        public HTuple hv_foundU;
        public HTuple hv_foundV;
        public HTuple ImageWidth = 5120;
        public HTuple ImageHeight = 5120;
        public HTuple hv_dieWidth;
        public HTuple hv_dieHeight;
        public HTuple hv_updateMapX;
        public HTuple hv_updateMapY;
        public HTuple hv_xSnapPosLT;
        public HTuple hv_ySnapPosLT;
        HTuple ListChannelSum = null;
        HTuple ListChannelNum = null;
        public ImagePosition[,] dieMatrix;
        public ImagePosition[,] ScanMatrix;

        public List<HTuple> List_UV2XYResult = null;
        public HObject frameMapImg = null;
        ToolKits.FunctionModule.Vision tool_vision;
        private HTupleVector _frameNgImagesRowIndex = new HTupleVector(1); //料片NG图像行索引

        public HTupleVector FrameNgImagesRowIndex
        {
            get { return _frameNgImagesRowIndex; }
            set { _frameNgImagesRowIndex = value; }
        }
        private HTupleVector _frameNgImagesColIndex = new HTupleVector(1); //料片NG图像列索引

        public HTupleVector FrameNgImagesColIndex
        {
            get { return _frameNgImagesColIndex; }
            set { _frameNgImagesColIndex = value; }
        }
        private HTuple _rowNgImagesRowIndex = new HTuple();//行扫描中NG图像行索引

        public HTuple RowNgImagesRowIndex
        {
            get { return _rowNgImagesRowIndex; }
            set { _rowNgImagesRowIndex = value; }
        }
        private HTuple _rowNgImagesColIndex = new HTuple();//行扫描中NG图像列索引
        public Acquisition Acq;
        private Boolean _flagLoadInspModels = false;

        public Boolean FlagLoadInspModels
        {
            get { return _flagLoadInspModels; }
            set { _flagLoadInspModels = value; }
        }

        public VisionState VisionState
        {
            get { return visionState; }
            set { visionState = value; }
        }
        /// <summary>
        /// 相机所拍图像内存对象
        /// </summary>

        public Model ModelOperations = new Model();


        //public HTupleVector FrameNgImagesRowIndex { get => _frameNgImagesRowIndex; set => _frameNgImagesRowIndex = value; }
        //public HTupleVector FrameNgImagesColIndex { get => _frameNgImagesColIndex; set => _frameNgImagesColIndex = value; }
        //public bool FlagLoadInspModels { get => _flagLoadInspModels; set => _flagLoadInspModels = value; }
        #endregion
        #region 模块私有变量
        byte[] scannerStart = { 0X16, 0X54, 0X0D };
        byte[] scannerStop = { 0X16, 0X55, 0X0D };
        #endregion

        #region 变量存储模块"声明变量"

        public void ClearRowNgRecord()
        {
            _rowNgImagesColIndex = new HTuple();
            _rowNgImagesRowIndex = new HTuple();
        }
        public static HTuple CopyModel(HTuple modelId, HTuple modelType)
        {
            HTuple modelID = null;
            switch (modelType.I)
            {
                case 0:
                    modelID = CopyNccModel(modelId);
                    break;
                case 1:
                    modelID = CopyShapeModel(modelId);
                    break;
            }
            return modelID;
        }
        public static HTuple CopyShapeModel(HTuple shapeModelId)
        {
            HTuple copyShapeModeId = new HTuple();
            HTuple serializedItemHandle = new HTuple();
            HOperatorSet.SerializeShapeModel(shapeModelId, out serializedItemHandle);
            HOperatorSet.DeserializeShapeModel(serializedItemHandle, out copyShapeModeId);
            return copyShapeModeId;
        }
        //model类Copy,需要序列化和反序列化的过程
        public static HTuple CopyNccModel(HTuple nccModelId)
        {
            HTuple copyNccModeId = new HTuple();
            HTuple serializedItemHandle = new HTuple();
            HOperatorSet.SerializeNccModel(nccModelId, out serializedItemHandle);
            HOperatorSet.DeserializeNccModel(serializedItemHandle, out copyNccModeId);
            return copyNccModeId;
        }
        /// <summary>
        /// 清除模板信息和模板类型
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool ClearModel(HTuple modelType, HTuple model)
        {
            if (model.Length != modelType.Length) return false;
            for (int i = 0; i < model.Length; i++)
            {
                switch (modelType[i].I)
                {
                    case 0:
                        HOperatorSet.ClearNccModel(model[i]);
                        break;
                    case 1:
                        HOperatorSet.ClearShapeModel(model[i]);
                        break;
                    default:
                        return false;
                }
            }
            model = new HTuple();
            modelType = new HTuple();
            return true;
        }


        public void depth_from_focus_gpu(HObject ho_MultiChannelImage, out HObject ho_Depth,
     out HObject ho_Confidence, out HTuple hv_iFlag)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_DeviceIdentifier = null, hv_DeviceHandle = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Depth);
            HOperatorSet.GenEmptyObj(out ho_Confidence);
            hv_iFlag = 0;
            HOperatorSet.QueryAvailableComputeDevices(out hv_DeviceIdentifier);
            if ((int)(new HTuple((new HTuple(hv_DeviceIdentifier.TupleLength())).TupleEqual(
                0))) != 0)
            {
                hv_iFlag = -1;
            }
            //get_compute_device_info (DeviceIdentifier, 'name', Info)
            HOperatorSet.OpenComputeDevice(hv_DeviceIdentifier, out hv_DeviceHandle);
            HOperatorSet.SetComputeDeviceParam(hv_DeviceHandle, "asynchronous_execution",
                "true");
            HOperatorSet.InitComputeDevice(hv_DeviceHandle, "depth_from_focus");
            HOperatorSet.ActivateComputeDevice(hv_DeviceHandle);
            ho_Depth.Dispose(); ho_Confidence.Dispose();
            HOperatorSet.DepthFromFocus(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
                "bandpass", "local");
            HOperatorSet.DeactivateComputeDevice(hv_DeviceHandle);

            return;
        }

        //ho_depth 不同位置每个像素对应的高度？ho_confidence 高度图的置信度 ? ho_sharpImae 清晰图？ hv_FocusHeigth 两个高度? hv_iFlag？
        public void auto_focus_via_dff(HObject ho_Image,
            out HObject ho_Depth, out HObject ho_Confidence, out HObject ho_SharpImage,
          out HTuple hv_FocusHeigth, out HTuple hv_iFlag)
        {

            //HObject ho_MatchRegion = ModelOperations.matchRegion;
            HObject ho_MatchRegion = ModelOperations.ho_RegionDilation;
            HObject ho_FocusRegion = ModelOperations.FocusRegion;
            HTuple hv_IsGPU = ModelOperations.hv_is_gpu;
            HTuple hv_MeanSize = ModelOperations.hv_mean_size;
            HTuple hv_ModelID = ModelOperations.modelID;
            HTuple hv_ModelType = ModelOperations.hv_model_type;



            HTuple hv_AngleStart = ModelOperations.hv_angle_start;
            HTuple hv_AngleExtent = ModelOperations.hv_angle_extent;
            HTuple hv_MatchThresh = ModelOperations.hv_match_thresh;


            HTuple hv_ConfidenceThresh = ModelOperations.hv_confidence_thresh;

            HTuple hv_IsMean = ModelOperations.hv_is_mean;


            // Local iconic variables 

            HObject ho_MultiChannelImage, ho_DepthMean;
            HObject ho_ImageReduced, ho_RegionAffineTrans = null, ho_ObjectSelected = null;
            HObject ho_Region = null, ho_RegionIntersection = null;

            // Local control variables 

            HTuple hv_Number = null, hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
            HTuple hv_Score = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_Index = new HTuple(), hv_Mean = new HTuple();
            HTuple hv_Deviation = new HTuple(), hv_Min = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Depth);
            HOperatorSet.GenEmptyObj(out ho_Confidence);
            HOperatorSet.GenEmptyObj(out ho_SharpImage);
            HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);
            HOperatorSet.GenEmptyObj(out ho_DepthMean);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            hv_iFlag = 0;
            hv_FocusHeigth = new HTuple();
            ho_Depth.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Depth);
            ho_SharpImage.Dispose();
            HOperatorSet.GenEmptyObj(out ho_SharpImage);
            //*****************************************
            HOperatorSet.CountObj(ho_FocusRegion, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
            {
                hv_iFlag = -1;
                ho_MultiChannelImage.Dispose();
                ho_DepthMean.Dispose();
                ho_ImageReduced.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Region.Dispose();
                ho_RegionIntersection.Dispose();

                return;
            }
            ho_MultiChannelImage.Dispose();
            HOperatorSet.ChannelsToImage(ho_Image, out ho_MultiChannelImage);
            if ((int)(new HTuple(hv_IsGPU.TupleEqual(1))) != 0)
            {
                ho_Depth.Dispose(); ho_Confidence.Dispose();
                depth_from_focus_gpu(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
                    out hv_iFlag);
                if ((int)(new HTuple(hv_iFlag.TupleNotEqual(0))) != 0)
                {
                    hv_iFlag = -2;
                    ho_MultiChannelImage.Dispose();
                    ho_DepthMean.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_Region.Dispose();
                    ho_RegionIntersection.Dispose();

                    return;
                }
            }
            else
            {
                ho_Depth.Dispose(); ho_Confidence.Dispose();
                HOperatorSet.DepthFromFocus(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
                    "bandpass", "local");
            }
            ho_DepthMean.Dispose();
            HOperatorSet.MeanImage(ho_Depth, out ho_DepthMean, hv_MeanSize, hv_MeanSize);
            ho_SharpImage.Dispose();
            HOperatorSet.SelectGrayvaluesFromChannels(ho_MultiChannelImage, ho_DepthMean,
                out ho_SharpImage);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_SharpImage, ho_MatchRegion, out ho_ImageReduced);
            if ((int)(new HTuple(hv_ModelType.TupleEqual(0))) != 0)
            {
                HOperatorSet.FindNccModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
                    0.2, 1, 0.5, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            }
            else
            {
                HOperatorSet.FindShapeModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
                    0.2, 1, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle,
                    out hv_Score);
            }
            if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(1))).TupleAnd(
                new HTuple(hv_Score.TupleGreater(hv_MatchThresh)))) != 0)
            {
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out hv_HomMat2D);
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_FocusRegion, out ho_RegionAffineTrans, hv_HomMat2D,
                    "nearest_neighbor");
                HTuple end_val31 = hv_Number;
                HTuple step_val31 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val31, step_val31); hv_Index = hv_Index.TupleAdd(step_val31))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_RegionAffineTrans, out ho_ObjectSelected, hv_Index);
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_Confidence, out ho_Region, hv_ConfidenceThresh,
                        255);
                    ho_RegionIntersection.Dispose();
                    HOperatorSet.Intersection(ho_Region, ho_ObjectSelected, out ho_RegionIntersection
                        );
                    if ((int)(new HTuple(hv_IsMean.TupleEqual(1))) != 0)
                    {
                        HOperatorSet.Intensity(ho_RegionIntersection, ho_Depth, out hv_Mean, out hv_Deviation);
                        hv_FocusHeigth = hv_FocusHeigth.TupleConcat(hv_Mean);
                    }
                    else
                    {
                        HOperatorSet.MinMaxGray(ho_RegionIntersection, ho_Depth, 50, out hv_Min,
                            out hv_Max, out hv_Range);
                        hv_FocusHeigth = hv_FocusHeigth.TupleConcat(hv_Min);
                    }
                }
            }
            else
            {
                hv_iFlag = -3;
                ho_MultiChannelImage.Dispose();
                ho_DepthMean.Dispose();
                ho_ImageReduced.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Region.Dispose();
                ho_RegionIntersection.Dispose();

                return;
            }
            ho_MultiChannelImage.Dispose();
            ho_DepthMean.Dispose();
            ho_ImageReduced.Dispose();
            ho_RegionAffineTrans.Dispose();
            ho_ObjectSelected.Dispose();
            ho_Region.Dispose();
            ho_RegionIntersection.Dispose();

            return;
        }

        #endregion


        #region 标定相关  by M.Bing
        /// <summary>
        /// block内die与die之间的标定   获得die与die的之间的位置关系
        /// </summary>
        /// <returns></returns>
        public bool CalibDie2Die()
        {
            return true;
        }
        /// <summary>
        /// block与block之间的标定  获得block与block之间的关系  
        /// </summary>
        /// <returns></returns>
        public bool CalibBlock2Block()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CalibProduct()
        {
            if (!CalibDie2Die())
            {
                errCode = -1;
                errString = "block内die与die之间标定失败!";
                return false;
            }

            if (!CalibBlock2Block())
            {
                errCode = -1;
                errString = "block与block之间标定失败!";
                return false;
            }
            return true;
        }
        #endregion
        #region 视觉模块相关方法
        public Boolean SaveImageFolder()
        {
            System.Data.SQLite.SQLiteConnection sqlCon = null;    //连接
            Boolean ret = true;
            try
            {
                sqlCon = new System.Data.SQLite.SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String sql = "CREATE TABLE IF NOT EXISTS " + paraTable + "(Para TEXT PRIMARY KEY NOT NULL, Value TEXT NOT NULL)";
                System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                System.Reflection.FieldInfo[] infos = this.GetType().GetFields();//type.GetField
                cmd.CommandText = String.Format("REPLACE INTO {0}(Para, Value) VALUES(@_Para, @_Value)", paraTable);//1234之类的？
                cmd.Parameters.Add("_Para", System.Data.DbType.String).Value = "imageFolder";
                cmd.Parameters.Add("_Value", System.Data.DbType.Object).Value = imageFolder;
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                errCode = -1;
                errString = exp.ToString();
                if (sqlCon != null)
                    sqlCon.Close();
                ret = false;
            }
            return ret;
        }
        public void SaveCameraData()
        {
            IniFiles config = new IniFiles(camIniPath);
            Obj_Camera.Num_Camera = obj_camera.Count;
            config.WriteInteger("Camera_Number", "Num_Camera", Obj_Camera.Num_Camera);
            config.WriteInteger("Camera_Selected", "SelectedIndex", Obj_Camera.SelectedIndex);
            for (int i = 0; i < Obj_Camera.Num_Camera; i++)
            {
                Obj_Camera obj_cam = obj_camera[i];
                int cameraType_No;
                cameraType_No = Convert.ToInt32(obj_cam.cameraType);
                config.WriteBool("Camera_" + i.ToString(), "isEnable", obj_cam.isEnable);
                config.WriteString("Camera_" + i.ToString(), "cameraName", obj_cam.cameraName);
                config.WriteString("Camera_" + i.ToString(), "camFile", obj_cam.camFile);
                config.WriteInteger("Camera_" + i.ToString(), "cameraType", cameraType_No);
                config.WriteDouble("Camera_" + i.ToString(), "exposure", obj_cam.exposure);
                config.WriteDouble("Camera_" + i.ToString(), "gain", obj_cam.gain);
                config.WriteBool("Camera_" + i.ToString(), "isCameraTrigger", obj_cam.isCameraTrigger);
                config.WriteBool("Camera_" + i.ToString(), "isSoftwareTrigger", obj_cam.isSoftwareTrigger);
                config.WriteBool("Camera_" + i.ToString(), "isMirrorX", obj_cam.isMirrorX);
                config.WriteBool("Camera_" + i.ToString(), "isMirrorY", obj_cam.isMirrorY);
            }
        }

        public void LoadCameraData()
        {
            IniFiles config = new IniFiles(camIniPath);
            //有问题
            config.ReadInteger("Camera_Number", "Num_Camera", out Obj_Camera.Num_Camera);
            config.ReadInteger("Camera_Selected", "SelectedIndex", out Obj_Camera.SelectedIndex);

            for (int i = 0; i < Obj_Camera.Num_Camera; i++)
            {
                Obj_Camera obj_cam = new Obj_Camera();
                int cameraType_No;
                cameraType_No = Convert.ToInt32(obj_cam.cameraType);
                config.ReadBool("Camera_" + i.ToString(), "isEnable", out obj_cam.isEnable);
                config.ReadString("Camera_" + i.ToString(), "cameraName", out obj_cam.cameraName);
                config.ReadString("Camera_" + i.ToString(), "camFile", out obj_cam.camFile);
                config.ReadInteger("Camera_" + i.ToString(), "cameraType", out cameraType_No);
                obj_cam.cameraType = (CameraEnum)cameraType_No;
                config.ReadDouble("Camera_" + i.ToString(), "exposure", out obj_cam.exposure);
                config.ReadDouble("Camera_" + i.ToString(), "gain", out obj_cam.gain);
                config.ReadBool("Camera_" + i.ToString(), "isCameraTrigger", out obj_cam.isCameraTrigger);
                config.ReadBool("Camera_" + i.ToString(), "isSoftwareTrigger", out obj_cam.isSoftwareTrigger);
                config.ReadBool("Camera_" + i.ToString(), "isMirrorX", out obj_cam.isMirrorX);
                config.ReadBool("Camera_" + i.ToString(), "isMirrorY", out obj_cam.isMirrorY);
                obj_camera.Add(obj_cam);
            }
        }

        /// <summary>
        /// 初始化所有相机
        /// </summary>
        public void InitializeAllCamera()
        {
            Obj_Camera.Num_Camera = 1;
            obj_camera = new List<Obj_Camera>();
            LoadCameraData();
            if (runMode != 0) return;
            for (int i = 0; i < Obj_Camera.Num_Camera; i++)
            {
                if (!obj_camera[i].isEnable) continue;
                if (!obj_camera[i].InitCamera())
                {
                    HTUi.PopError("初始化相机失败！");
                    return;
                }

                if (!obj_camera[i].OpenCamera())
                {
                    HTUi.PopError("打开相机失败！");
                    return;
                }
                obj_camera[i].SetExposure(obj_camera[i].exposure);
                obj_camera[i].SetGain(obj_camera[i].gain);
            }
        }

        /// <summary>
        /// 关闭所有相机
        /// </summary>
        public void CloseAllCamera()
        {
            if (obj_camera == null) return;
            if (obj_camera.Count == 0) return;
            for (int i = 0; i < Obj_Camera.Num_Camera; i++)
            {
                if (!obj_camera[i].isEnable) continue;
                if (!obj_camera[i].CloseCamera())
                {
                    MessageBox.Show("关闭相机" + i.ToString() + "失败！");
                    continue;
                }
            }
        }
        /// <summary>
        /// 读取已有的芯片点位
        /// </summary>
        /// <returns></returns>
        public bool ReadClipPoints()
        {
            try
            {
                clipMapX = new HTuple();
                clipMapY = new HTuple();
                clipMapRow = new HTuple();
                clipMapRow = new HTuple();
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapX.dat"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapX.dat", out clipMapX);
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapY.dat"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapY.dat", out clipMapY);
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapRow.dat"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapRow.dat", out clipMapRow);
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapCol.dat"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "clipMapCol.dat", out clipMapCol);
                //清除之前的点位
                if (ClipMapPostions != null)
                {
                    ClipMapPostions.Clear();
                }
                else
                {
                    ClipMapPostions = new List<ImagePosition>();
                }
                ImagePosition imagePosition = new ImagePosition();
                imagePosition.z = App.obj_Pdt.ZFocus;
                clipPosNum = clipMapX.Length;
                //clipIniConfig.ReadInteger("ClipPoints", "clipPosNum", out clipPosNum);
                for (int i = 0; i < clipPosNum; i++)
                {
                    imagePosition.x = clipMapX.TupleSelect(i);
                    imagePosition.y = clipMapY.TupleSelect(i);
                    imagePosition.r = clipMapRow.TupleSelect(i);
                    imagePosition.c = clipMapCol.TupleSelect(i);
                    ClipMapPostions.Add(imagePosition);
                }
                return true;
            }
            catch (Exception err)
            {
                HTUi.PopError(err.Message);
                return false;
            }
        }
        /// <summary>
        /// 读取已有的扫描点位
        /// </summary>
        /// <returns></returns>
        public bool ReadScanPoints()
        {
            try
            {
                if (File.Exists(App.AlgParamsPath))
                {

                    VisionFlow_Interface.Params.config = new IniFiles(App.AlgParamsPath);
                    VisionFlow_Interface.Params.config.ReadInteger(App.obj_Vision.GetType().ToString(), "ImageThreadMax", out ThreadMax);

                    if (ThreadMax == 0) ThreadMax = 3;
                    ListThreadInspectionFlag.Clear();
                    for (int i = 0; i < ThreadMax; i++)
                    {
                        ListThreadInspectionFlag.Add(false);
                    }
                }
                snapMapX = new HTuple();
                snapMapY = new HTuple();
                snapMapRow = new HTuple();
                snapMapCol = new HTuple();
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapX.dat"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapX.dat", out snapMapX);
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapY.dat"))
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapY.dat", out snapMapY);
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapRow.dat"))
                {
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapRow.dat", out snapMapRow);
                    scanRowNum = snapMapRow.TupleMax() + 1;
                }
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapRow.dat"))
                {
                    HOperatorSet.ReadTuple(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "snapMapCol.dat", out snapMapCol);
                    scanColNum = snapMapCol.TupleMax() + 1;
                }
                //清除之前的点位
                if (ScanMapPostions != null)
                {
                    ScanMapPostions.Clear();
                }
                else
                {
                    ScanMapPostions = new List<ImagePosition>();
                }
                ImagePosition imagePosition = new ImagePosition();
                imagePosition.z = App.obj_Pdt.ZFocus;
                scanPosNum = snapMapX.Length;
                double dieWidth = 0, dieHeight = 0;

                scanIniConfig.ReadDouble("ScanPoints", "genMapStartX", out genMapStartX);
                scanIniConfig.ReadDouble("ScanPoints", "genMapStartY", out genMapStartY);
                scanIniConfig.ReadDouble("ScanPoints", "genMapEndX", out genMapEndX);
                scanIniConfig.ReadDouble("ScanPoints", "genMapEndY", out genMapEndY);
                scanIniConfig.ReadDouble("ScanPoints", "sameSpace", out sameSpace);
                scanIniConfig.ReadDouble("ScanPoints", "scaleFactor", out scaleFactor);
                scanIniConfig.ReadDouble("ScanPoints", "checkPosX", out checkPosX);
                scanIniConfig.ReadDouble("ScanPoints", "checkPosY", out checkPosY);
                scanIniConfig.ReadDouble("ScanPoints", "checkPosScoreThresh", out checkPosScoreThresh);
                scanIniConfig.ReadDouble("ScanPoints", "widthFactor", out widthFactor);
                scanIniConfig.ReadDouble("ScanPoints", "heightFactor", out heightFactor);
                scanIniConfig.ReadDouble("ScanPoints", "dieWidth", out dieWidth);
                scanIniConfig.ReadDouble("ScanPoints", "dieHeight", out dieHeight);
                scanIniConfig.ReadInteger("MobileShooting", "LineAxisTrigCount", out App.obj_MobSht.LineAxisTrigCount);
                scanIniConfig.ReadInteger("MobileShooting", "NoLineAxisMoveCount", out App.obj_MobSht.NoLineAxisMoveCount);
                hv_dieWidth = dieWidth;
                hv_dieHeight = dieHeight;
                for (int i = 0; i < scanPosNum; i++)
                {

                    imagePosition.x = snapMapX.TupleSelect(i);
                    imagePosition.y = snapMapY.TupleSelect(i);
                    imagePosition.r = (snapMapRow.Length != 0 ? snapMapRow.TupleSelect(i).I : 0);
                    imagePosition.c = (snapMapCol.Length != 0 ? snapMapCol.TupleSelect(i).I : 0);
                    ScanMapPostions.Add(imagePosition);
                }
                return true;
            }
            catch (Exception err)
            {
                HTUi.PopError(err.Message);
                return false;
            }
        }
        /// <summary>
        /// 初始化视觉模块参数
        /// </summary>
        public void Initialize()
        {
            try
            {
                LFAOIModel.FilePath.ProductDirectory = App.ProductPath;
                DefaultImgNum = 3;
                IsDbLocked = false;
                HOperatorSet.SetSystem("width", 5000);
                HOperatorSet.SetSystem("height", 5000);
                HOperatorSet.SetSystem("clip_region", "false");
                camIniPath = App.programDir + @"\Camfile\CameraSet.ini";
                imageFolder = Application.StartupPath + "\\ImageFolder";
                TestImagePath = App.ProductDir + "\\" + ProductMagzine.ActivePdt + @"\image";
                Read();
                CheckPosModels = new LeadframeAOI.Model();
                frameFolder = imageFolder;
                SaveThreadNow = 0;
                Task.Run(new Action(SaveResult_Db));
                if (Process.focusPara == null)
                {
                    Process.focusPara = new Vision.AutoFocusParam();
                    Process.focusPara.focusRegion = new HObject();
                }
                if (File.Exists(Application.StartupPath + "\\focusRegion.reg"))
                    HOperatorSet.ReadRegion(out Process.focusPara.focusRegion, Application.StartupPath + "\\focusRegion.reg");

                Acq = new Acquisition();
                VisionState = 0;
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }
                InitializeAllCamera();
                try
                {
                    List_UV2XYResult = new List<HTuple>();
                    tool_vision = new ToolKits.FunctionModule.Vision();
                    for (int i = 0; i < Obj_Camera.Num_Camera; i++)
                    {
                        HTuple Item_UV2XYResult = null;
                        if (!obj_camera[i].isEnable)
                        {
                            Item_UV2XYResult = new HTuple(-1);
                            List_UV2XYResult.Add(Item_UV2XYResult);
                            continue;
                        }
                        if (File.Exists(App.SystemUV2XYDir + "\\Camera_" + i + "\\" + "UV2XY" + ".dat"))
                        {
                            tool_vision.read_hom2d(App.SystemUV2XYDir + "\\Camera_" + i + "\\" + "UV2XY" + ".dat", out Item_UV2XYResult);
                            List_UV2XYResult.Add(Item_UV2XYResult);
                        }
                        else
                        {
                            Item_UV2XYResult = new HTuple(-1);
                            List_UV2XYResult.Add(Item_UV2XYResult);
                        }
                    }
                }
                catch
                {
                    HTUi.PopError("无法读取UV-XY结果文件.");
                }
                InitPdtData();
            }
            catch (Exception err)
            {
                HTUi.PopError(err.Message);
            }
        }
        /// <summary>
        /// 加载产品视觉数据
        /// </summary>
        public void InitPdtData()
        {
            //加载二维码训练模板
            if (hv_dataCodeHandle != null)
            {
                HOperatorSet.ClearDataCode2dModel(hv_dataCodeHandle);
                App.obj_Vision.hv_dataCodeHandle = null;
            }
            if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "Code2D.dcm"))
                HOperatorSet.ReadDataCode2dModel(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "Code2D.dcm",
                    out hv_dataCodeHandle);

            scanIniConfig = new IniFiles(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\scanPoint.ini");

            if (!ReadScanPoints())
            {
                throw new Exception("无法读取该产品扫描点位信息.");
            }
            if (!ReadClipPoints())
            {
                throw new Exception("无法读取该产品芯片点位信息.");
            }
            if (!CheckPosModels.ReadModel(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\CheckPosModels"))
            {
                throw new Exception("无法读取该产品矫正点模板信息.");
            }
            try
            {
                App.obj_AlgApp.Initialization(App.obj_Vision.ThreadMax, App.AlgParamsPath, App.ModelPath, App.Fuc_LibPath, App.Json_Path);
            }
            catch (Exception EXP)
            {
                throw new Exception("加载产品模板数据失败!\n" + EXP.ToString());
            }
        }
        /// <summary>
        /// 采集多张图并做3d算法  前提相机已经连续采集到多张图
        /// </summary>
        /// <param name="imageNum">图片张数</param>
        /// <param name="_3dImages">返回的聚焦清晰的图片</param>
        /// <param name="posList">对应的清晰图片的位置</param>
        /// <returns></returns>
        public Boolean CaputreImagesFocus3d(int imageNum, ref HObject _3dImages, out List<double> indexList)
        {
            string errStr = "";
            bool ret = false;
            indexList = new List<double>();
            HObject focusImages = new HObject();
            HOperatorSet.GenEmptyObj(out focusImages);
            errStr = obj_camera[0].CaputreImages(ref focusImages, imageNum, 5);
            if (errStr != "")
            {
                errString = errStr;
                goto _end;
            }
            if (!Focus3DAlgorithm(focusImages, ref _3dImages, ref indexList))
            {
                goto _end;
            }
            ret = true;
        _end:
            indexList.Clear();
            focusImages.Dispose();
            return ret;
        }
        /// <summary>
        /// 3D算法 输出 ho_Depth ho_Confidence  ho_SharpImage 和 hv_FocusHeigth
        /// </summary>
        /// <param name="images"></param>
        /// <param name="_3dImages"></param>
        /// <param name="indexList"></param>
        /// <returns></returns>
        public Boolean Focus3DAlgorithm(HObject images, ref HObject _3dImages, ref List<double> indexList)
        {
            bool ret = false;

            HObject ho_Depth;
            HObject ho_Confidence;
            HObject ho_SharpImage;
            HTuple hv_FocusHeigth;
            HTuple hv_iFlag;
            auto_focus_via_dff(images, out ho_Depth, out ho_Confidence, out ho_SharpImage,
          out hv_FocusHeigth, out hv_iFlag);
            _3dImages = ho_SharpImage.CopyObj(1, -1);
            HOperatorSet.ConcatObj(_3dImages, ho_Depth, out _3dImages);
            HOperatorSet.ConcatObj(_3dImages, ho_Confidence, out _3dImages);
            if (-1 == hv_iFlag)
            {
                errString = String.Format("连续聚焦3D检测失败");
                goto _end;
            }
            foreach (var item in hv_FocusHeigth.DArr)
            {
                indexList.Add(item);
            }
            ret = true;
        _end:
            if (ho_Depth != null) ho_Depth.Dispose();
            if (ho_Confidence != null) ho_Confidence.Dispose();
            if (ho_SharpImage != null) ho_SharpImage.Dispose();
            indexList.Clear();
            return ret;
        }

        /// <summary>
        /// 载入线性触发图片模板
        /// </summary>
        /// <returns>载入模板是否成功</returns>
        public Boolean LoadAllModels()
        {
            string errMsg = String.Empty;

            //if (App.obj_Vision.ModelOperations.ReadModel(App.ProductFile + @"\" + ProductMagzine.ActivePdt + @"\3DModels") == false)
            //{
            //    //throw new Exception("加载3D模版失败");
            //    return false;
            //}
            try
            {
                if (App.obj_AlgApp.ReadAllModel(App.ProductPath + "\\Models\\") == false)
                {
                    return false;
                }
                return true;

            }
            catch
            {
                return false;
            }
            // return App.visionFunction.LoadAllModels(App.ProductFile + "\\" + ProductMagzine.ActivePdt, ref  errMsg);
        }
        /// <summary>
        /// 创建图像文件夹
        /// </summary>
        /// <returns></returns>
        public Boolean ConfigFrameFolder()
        {
            try
            {
                //ascii文件夹不可用符号转化为下划线
                char[] tempstring = frameName.ToCharArray();
                for (int i = 0; i < tempstring.Count(); i++)
                {
                    if (tempstring[i] == '\\' || tempstring[i] == '/' || tempstring[i] == ':' || tempstring[i] == '*' ||
                        tempstring[i] == '?' || tempstring[i] == '"' || tempstring[i] == '<' || tempstring[i] == '>'
                        || tempstring[i] == '|')
                        tempstring[i] = '_';
                }
                frameName = new string(tempstring);
                //回车跟换行转化为空
                frameName = frameName.Replace("\r\n", "");

                frameFolder = imageFolder + "\\" + frameName;
                Directory.CreateDirectory(frameFolder);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean TrainCode2D(HTWindowControl htWindow)
        {
            try
            {
                HObject ho_dataCode2DContour = null;
                HTuple hv_ResultHandles = null, hv_dataCodeString = null, hv_dataCodeType = null, iFlag = null;
                showRegion = null;
                HOperatorSet.GenEmptyRegion(out showRegion);
                ToolKits.FunctionModule.Vision.GenROI(htWindow, "region", ref showRegion);
                if (showRegion == null)
                {
                    HTUi.PopError("请先画出一个包含二维码的训练用区域。");
                    return false;
                }
                Image = htWindow.Image;
                VisionMethon.TrainDataCode2D(Image, showRegion, out ho_dataCode2DContour, out hv_dataCodeHandle,
                               out hv_dataCodeType, out hv_dataCodeString, out iFlag);
                App.obj_Vision.frameName = hv_dataCodeString;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 算法识别二维码
        /// </summary>
        /// <returns></returns>
        public Boolean ScanCode2D()
        {
            try
            {
                HObject ho_dataCode2DContour = null;
                HTuple hv_ResultHandles = null, hv_dataCodeString = null;
                HOperatorSet.FindDataCode2d(Image, out ho_dataCode2DContour, hv_dataCodeHandle,
                              new HTuple(), new HTuple(), out hv_ResultHandles, out hv_dataCodeString);
                App.obj_Vision.frameName = hv_dataCodeString;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检测算法
        /// </summary>
        /// <param name="imageCache">缓存图片结构</param>
        /// <param name="htWindow">视窗</param>
        /// <param name="SaveRecord">是否记录信息到本地</param>
        /// <param name="inspectResults">结果</param>
        /// <returns></returns>
        public Boolean Inspection(ImageCache imageCache, HTWindowControl htWindow, int selectImg, Boolean IsProcess, out List<StructInspectResult> inspectResults, out bool IsAllOkSnap, out SnapDataResult snapDataResult, int netIdx = -1)
        {
            try
            {
                snapDataResult = new SnapDataResult();
                htWindow.Region = null;
                HObject defect_Region = null;
                HObject wire_Region = null;
                HObject die_match_region = null;
                List<HObject> List_die_match_region = new List<HObject>();
                HObject OK_match_region = null;
                HObject NG_match_region = null;
                HObjectVector VectorWires = null;
                HObjectVector VectorDefectRegs = null;
                HOperatorSet.GenEmptyObj(out wire_Region);
                HTuple dieRows = null, dieCols = null, iFlag = null;
                bool flag;
                IsAllOkSnap = false;
                //存视野图
                // 若为存全图模式则在检测前存图
                //DateTime t0 = DateTime.Now;
                if (IsProcess)
                {
                    if (App.obj_SystemConfig.ImageNgSave == 0)
                    {
                        string snapPath = frameFolder + "\\Row" +
                            imageCache.r + "-" + imageCache.c;
                        Directory.CreateDirectory(snapPath);
                        HTuple SnapX = null, SnapY = null;
                        SnapX = imageCache.X;
                        SnapY = imageCache.Y;
                        HOperatorSet.WriteTuple(SnapX, snapPath + "\\" + "SnapX.tup");
                        HOperatorSet.WriteTuple(SnapY, snapPath + "\\" + "SnapY.tup");
                        for (int i = 0; i < App.obj_ImageInformSet.Count; i++)
                        {
                            if (App.obj_ImageInformSet[i].Use2D)
                            {
                                HObject tempHObj = imageCache._2dImage.SelectObj(i + 1);
                                HOperatorSet.WriteImage(tempHObj, "tiff", 0, snapPath + "\\" + i + ".tiff");
                                tempHObj.Dispose();
                            }
                            if (App.obj_ImageInformSet[i].UseAutoFocus)
                            {
                                string _3DPath = snapPath + "\\3DAutoFocus";
                                Directory.CreateDirectory(_3DPath);
                                HOperatorSet.WriteTuple(imageCache.List_Z_TrigPos[i], _3DPath + "\\" + "Snap" + i + "-" + "Z_TrigPoses.dat");
                                int num = imageCache.List_Z_TrigPos[i].Length;
                                for (int j = 0; j < num; j++)
                                {
                                    HObject tempHObj = imageCache.List_3dImage[i].SelectObj(j + 1);
                                    HOperatorSet.WriteImage(tempHObj, "tiff", 0, _3DPath + "\\" + "Snap" + i + "-" + j + ".tiff");
                                    tempHObj.Dispose();
                                }
                            }
                        }

                    }
                }
                //HTLog.Info("存视野全图" + "耗毫秒：" + (DateTime.Now.Subtract(t0).TotalMilliseconds).ToString());
                for (int i = 0; i < imageCache._2dImage.CountObj(); i++)
                {
                    if (!obj_camera[App.obj_ImageInformSet[i].CameraName].isEnable)
                    {
                        die_match_region = new HObject();
                        List_die_match_region.Add(die_match_region);
                        die_match_region = null;
                        continue;
                    }
                    HObject tempHObj = imageCache._2dImage.SelectObj(i + 1);
                    HOperatorSet.GetImageSize(tempHObj, out ImageWidth, out ImageHeight);
                    tempHObj.Dispose();
                    VisionMethon.gen_die_match_region(out die_match_region, imageCache.X, imageCache.Y, hv_dieWidth, hv_dieHeight, hv_updateMapX, hv_updateMapY, List_UV2XYResult[App.obj_ImageInformSet[i].CameraName],
                    ImageWidth, ImageHeight, widthFactor, heightFactor, scaleFactor, App.obj_AlgApp.P.dilationSize, App.obj_Pdt.RowNumber, App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber,
                    out dieRows, out dieCols, out iFlag);
                    List_die_match_region.Add(die_match_region);
                    die_match_region = null;
                }
                int imgIdxDefault = 0;
                for (int i = 0; i < App.obj_ImageInformSet.Count; i++)
                {
                    if (App.obj_ImageInformSet[i].CameraName == Obj_Camera.SelectedIndex)
                    {
                        imgIdxDefault = i;
                        break;
                    }

                }
                HObject tempHObjOutside = imageCache._2dImage.SelectObj(imgIdxDefault + 1);
                HOperatorSet.GetImageSize(tempHObjOutside, out ImageWidth, out ImageHeight);
                tempHObjOutside.Dispose();
                VisionMethon.gen_die_match_region(out die_match_region, imageCache.X, imageCache.Y, hv_dieWidth, hv_dieHeight, hv_updateMapX, hv_updateMapY, List_UV2XYResult[Obj_Camera.SelectedIndex],
            ImageWidth, ImageHeight, widthFactor, heightFactor, scaleFactor, App.obj_AlgApp.P.dilationSize, App.obj_Pdt.RowNumber, App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber,
            out dieRows, out dieCols, out iFlag);
                snapDataResult.clipRowNumInImg = 0;
                snapDataResult.clipColNumInImg = 0;
                if (iFlag != "")
                {
                    inspectResults = null;
                    return false;
                }
                if (dieRows.Length == 0 || dieCols.Length == 0)
                {
                    inspectResults = new List<StructInspectResult>();
                    IsAllOkSnap = true;
                    return true;
                }
                snapDataResult.clipRowMinInImg = dieRows.TupleMin();
                snapDataResult.clipRowMaxInImg = dieRows.TupleMax();
                snapDataResult.clipColMinInImg = dieCols.TupleMin();
                snapDataResult.clipColMaxInImg = dieCols.TupleMax();
                snapDataResult.clipRowNumInImg = snapDataResult.clipRowMaxInImg - snapDataResult.clipRowMinInImg + 1;
                snapDataResult.clipColNumInImg = snapDataResult.clipColMaxInImg - snapDataResult.clipColMinInImg + 1;
                clipRowNumInImg = snapDataResult.clipRowNumInImg;
                clipColNumInImg = snapDataResult.clipColNumInImg;
                clipRowMinInImg = snapDataResult.clipRowMinInImg;
                clipRowMaxInImg = snapDataResult.clipRowMaxInImg;
                clipColMinInImg = snapDataResult.clipColMinInImg;
                clipColMaxInImg = snapDataResult.clipColMaxInImg;
                // algrithm
                flag = App.obj_AlgApp.Inspection(netIdx == -1 ? 0 : netIdx, imageCache._2dImage, List_die_match_region, dieRows, dieCols, out defect_Region, out wire_Region,
                out VectorWires, out VectorDefectRegs, out inspectResults);

                if (!flag)
                {
                    inspectResults = null;
                    return false;
                }
                IsAllOkSnap = inspectResults.Count != 0 ? true : false;//表示视野中是否有NG芯片，用作界面显示

                for (int i = 0; i < inspectResults.Count; i++)
                {
                    StructInspectResult tempResult = inspectResults[i];
                    if (IsProcess)
                    {
                        tempResult.x = dieMatrix[inspectResults[i].realRow, inspectResults[i].realCol].x;
                        tempResult.y = dieMatrix[inspectResults[i].realRow, inspectResults[i].realCol].y;
                    }
                    inspectResults[i] = tempResult;
                    if (!inspectResults[i].OkOrNg)
                    {
                        IsAllOkSnap = false;
                    }
                }
                //string msg = "";
                //for (int i = 0; i < inspectResults.Count; i++)
                //{
                //    msg += String.Format("{0},{1},{2},{3}\r\n", imageCache.X, imageCache.Y, inspectResults[i].deltaX, inspectResults[i].deltaY);
                //}
                //File.AppendAllText("D:\\delta.csv", msg);
                // display
                //显示图、缺陷区域、金线
                ListChannelSum = null;
                ListChannelNum = null;
                PrepareTransImageIndex(imageCache._2dImage, out ListChannelSum, out ListChannelNum);


                int ChannelSum = ListChannelSum.TupleMax().I;
                int ImgNum = imageCache._2dImage.CountObj();
                int ClipNum = inspectResults.Count;
                List<HObject> ListImgRegionTemp = new List<HObject>();
                HObject[,] tempListRegion = new HObject[ImgNum, ClipNum];
                for (int i = 0; i < imageCache._2dImage.CountObj(); i++)
                {
                    HObject ItemRegion;
                    HOperatorSet.GenEmptyObj(out ItemRegion);
                    ListImgRegionTemp.Add(ItemRegion);
                }
                for (int i = 0; i < inspectResults.Count; i++)
                {
                    for (int j = 0; j < App.obj_ImageInformSet.Count; j++)
                    {
                        HObject ItemRegion;
                        HOperatorSet.GenEmptyObj(out ItemRegion);
                        tempListRegion[j, i] = ItemRegion;
                    }
                    if (!inspectResults[i].OkOrNg)
                    {
                        for (int k = 0; k < inspectResults[i].defectType.Length; k++)
                        {
                            int defectImgIdx = TransImageIndex(ListChannelSum, ListChannelNum, inspectResults[i].defectImgIdx[k]);
                            HObject tempRegion = null;
                            HOperatorSet.ConcatObj(tempListRegion[defectImgIdx, i], VectorDefectRegs[i].O.SelectObj(k + 1), out tempRegion);
                            tempListRegion[defectImgIdx, i].Dispose();
                            tempListRegion[defectImgIdx, i] = tempRegion.CopyObj(1, -1);
                            tempRegion.Dispose();
                        }
                        for (int j = 0; j < App.obj_ImageInformSet.Count; j++)
                        {
                            HObject tempRegion = null;
                            HOperatorSet.Union1(tempListRegion[j, i], out tempRegion);
                            HOperatorSet.ConcatObj(ListImgRegionTemp[j], tempRegion, out tempRegion);
                            ListImgRegionTemp[j].Dispose();
                            ListImgRegionTemp[j] = tempRegion.CopyObj(1, -1);
                            tempRegion.Dispose();
                        }
                    }
                    else
                    {
                        for (int j = 0; j < App.obj_ImageInformSet.Count; j++)
                        {
                            HObject tempRegion = null;
                            HOperatorSet.GenEmptyRegion(out tempRegion);
                            HOperatorSet.ConcatObj(ListImgRegionTemp[j], tempRegion, out tempRegion);
                            ListImgRegionTemp[j] = tempRegion.CopyObj(1, -1);
                            tempRegion.Dispose();

                        }
                    }
                }
                ListImgRegion = ListImgRegionTemp;
                htWindow.Region = flag ? ListImgRegion[selectImg] : null;
                HObject tempImg = imageCache._2dImage.SelectObj(selectImg + 1);
                HOperatorSet.ClearWindow(htWindow.HTWindow.HalconWindow);
                ShowImage(htWindow, tempImg, flag ? ListImgRegion[selectImg] : null);
                tempImg.Dispose();
                if (!IsProcess)
                {
                    HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                    HOperatorSet.DispXld(wire_Region, htWindow.HTWindow.HalconWindow);
                    HOperatorSet.GenEmptyObj(out OK_match_region);
                    HOperatorSet.GenEmptyObj(out NG_match_region);
                    for (int i = 0; i < inspectResults.Count; i++)
                    {
                        if (!inspectResults[i].OkOrNg)
                        {
                            HOperatorSet.ConcatObj(NG_match_region, die_match_region.SelectObj(i + 1), out NG_match_region);
                            IsAllOkSnap = false;
                        }
                        else
                        {
                            HOperatorSet.ConcatObj(OK_match_region, die_match_region.SelectObj(i + 1), out OK_match_region);
                        }
                    }
                    HOperatorSet.SetTposition(htWindow.HTWindow.HalconWindow, 0, 0);  //设置字体
                                                                                      //if (!IsAllOkSnap)
                                                                                      //    HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "red");
                    HOperatorSet.WriteString(htWindow.HTWindow.HalconWindow,
                        "Row " + imageCache.r + "-" + imageCache.c + ":" + (flag ? (IsAllOkSnap ? "OK" : "NG") : "NG")); //设置文字

                    if (netIdx == -1)
                    {
                        HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "red");
                        HOperatorSet.DispRegion(NG_match_region, htWindow.HTWindow.HalconWindow);
                        HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                        HOperatorSet.DispRegion(OK_match_region, htWindow.HTWindow.HalconWindow);

                        clipResults = new ClipResult[snapDataResult.clipRowNumInImg, snapDataResult.clipColNumInImg];

                        HObject ImageNew = null;
                        HObject _clipRegion = null;
                        HObject saveClipReg = null, saveDefectReg = null, saveWireDxf = null;
                        HTuple _row1, _col1, _row2, _col2;
                        HTuple hom2D;
                        for (int i = 0; i < inspectResults.Count; i++)
                        {
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg] = new ClipResult();
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].inspectDetail = inspectResults[i].inspectDetail;
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].defectImgIdx = inspectResults[i].defectImgIdx;
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].defectType = inspectResults[i].defectType;
                            HOperatorSet.GenEmptyObj(out clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].ClipImage);
                            HOperatorSet.GenEmptyObj(out clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].ClipWire);
                            for (int j = 0; j < imageCache._2dImage.CountObj(); j++)
                            {
                                try
                                {
                                    _clipRegion = List_die_match_region[j].SelectObj(i + 1);
                                    HOperatorSet.SmallestRectangle1(_clipRegion, out _row1, out _col1, out _row2, out _col2);
                                    //if (_row1 < 0) continue;
                                    //if (_col1 < 0) continue;
                                    HOperatorSet.VectorAngleToRigid(_row1, _col1, 0, 0, 0, 0, out hom2D);
                                    HOperatorSet.AffineTransRegion(_clipRegion, out saveClipReg, hom2D, "nearest_neighbor");
                                    HOperatorSet.AffineTransRegion(tempListRegion[j, i], out saveDefectReg, hom2D, "nearest_neighbor");
                                    HOperatorSet.AffineTransContourXld(VectorWires[i].O, out saveWireDxf, hom2D);
                                    HOperatorSet.CropRectangle1(imageCache._2dImage.SelectObj(j + 1), out ImageNew, _row1, _col1, _row2, _col2);

                                    HOperatorSet.ConcatObj(clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].ClipImage, ImageNew,
                                        out clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].ClipImage);
                                    ImageNew.Dispose();
                                    clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].ListImgClipDefect.Add(saveDefectReg.CopyObj(1, -1));
                                }
                                catch (Exception ex)
                                {
                                    HTLog.Error(ex.ToString());
                                }
                            }
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].ClipWire = saveWireDxf.CopyObj(1, -1);
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].realRow = inspectResults[i].realRow;
                            clipResults[dieRows[i] - snapDataResult.clipRowMinInImg, dieCols[i] - snapDataResult.clipColMinInImg].realCol = inspectResults[i].realCol;
                            saveDefectReg.Dispose();
                            saveWireDxf.Dispose();
                        }
                    }
                }

                else
                {
                    SnapResult itemSnapResult = new SnapResult();
                    itemSnapResult.InspectFlag = flag;
                    itemSnapResult.IsAllOkSnap = IsAllOkSnap;
                    itemSnapResult.Snap_x = imageCache.X;
                    itemSnapResult.Snap_y = imageCache.Y;
                    itemSnapResult.Snap_r = imageCache.r;
                    itemSnapResult.Snap_c = imageCache.c;
                    itemSnapResult._2dImage = imageCache._2dImage.CopyObj(1, -1);
                    itemSnapResult._3dImage = imageCache._3dImage.CopyObj(1, -1);
                    itemSnapResult.VectorDefectRegs = VectorDefectRegs.Clone();
                    itemSnapResult.VectorWires = VectorWires.Clone();
                    itemSnapResult.ClipRegs = List_die_match_region;
                    itemSnapResult.InspectResults = inspectResults;
                    qSnapResult.Enqueue(itemSnapResult);
                    if (!saveResultFlag)
                    {
                        saveResultFlag = true;
                    }
                }
                //bool flag = true;
                //inspectResults = new List<StructInspectResult>();
                //for (int i = 0; i < App.obj_ImageInformSet.Count; i++)
                //{
                //    if (!obj_camera[App.obj_ImageInformSet[i].CameraName].isEnable)
                //    {
                //        continue;
                //    }
                //    HOperatorSet.SetDraw(htWindow.HTWindow.HalconWindow, "margin");
                //    HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                //    HOperatorSet.DispRegion(List_die_match_region[i], htWindow.HTWindow.HalconWindow);
                //}
                return flag;
            }
            catch (Exception ex)
            {
                IsAllOkSnap = false;
                inspectResults = null;
                errString = ex.ToString();
                snapDataResult = new SnapDataResult();
                return false;
            }
        }
        /// <summary>
        /// 将数据写入DB
        /// </summary>
        public void SaveResult_Db()
        {
            DataManager.FileSaveDirectory = imageFolder + "\\" + "Result";
            while (true)
            {
                if (!saveResultFlag)
                {
                    Thread.Sleep(500);
                    continue;
                }
                frameResultFolder = imageFolder + "\\" + "Result\\" + ProductMagzine.ActivePdt + "\\" + App.obj_Process.LotId;
                try
                {
                    List<InspectionData> list_InspectionData = new List<InspectionData>();
                    SnapResult itemSnapResult = null;
                    //DateTime t1 = DateTime.Now;
                    //DateTime t2 = t1;
                    //处理拍照位检测结果队列
                    while (qSnapResult != null && !qSnapResult.IsEmpty)
                    {
                        try
                        {
                            if (itemSnapResult != null)
                            {
                                itemSnapResult.Dispose();
                            }
                            //从队列里取图
                            if (!qSnapResult.TryDequeue(out itemSnapResult))
                            {
                                saveResultFlag = false;
                                return;
                            };
                            //HTLog.Info("从队列里取图" + "耗秒：" + (DateTime.Now.Subtract(t1).TotalMilliseconds / 1000).ToString("F3"));
                            //t1 = DateTime.Now;
                            //存视野图
                            // Save image(只有三个条件都成立才不保存图，所以false的时候存图)
                            if ((itemSnapResult.InspectFlag && itemSnapResult.IsAllOkSnap) == false && App.obj_SystemConfig.ImageNgSave == 1)
                            {
                                string snapPath = frameFolder + "\\Row" +
                                    itemSnapResult.Snap_r + "-" + itemSnapResult.Snap_c;
                                Directory.CreateDirectory(snapPath);
                                HTuple SnapX = null, SnapY = null;
                                SnapX = itemSnapResult.Snap_x;
                                SnapY = itemSnapResult.Snap_y;
                                HOperatorSet.WriteTuple(SnapX, snapPath + "\\" + "SnapX.tup");
                                HOperatorSet.WriteTuple(SnapY, snapPath + "\\" + "SnapY.tup");
                                for (int i = 0; i < itemSnapResult._2dImage.CountObj(); i++)
                                {
                                    HObject tempHObj = itemSnapResult._2dImage.SelectObj(i + 1);
                                    HOperatorSet.WriteImage(tempHObj, "tiff", 0, snapPath + "\\" + i + ".tiff");
                                    tempHObj.Dispose();
                                }
                            }
                            //存芯片图以及检测数据

                            for (int i = 0; i < itemSnapResult.InspectResults.Count; i++)
                            {
                                List<DefectData> list_DefectData = new List<DefectData>();
                                if (!itemSnapResult.InspectResults[i].OkOrNg)
                                {
                                    for (int k = 0; k < itemSnapResult.InspectResults[i].defectType.Length; k++)
                                    {
                                        DefectData defect = new DefectData();
                                        if (k >= itemSnapResult.InspectResults[i].defectImgIdx.Length) defect.ImageIndex = 0;//如果缺陷没有对应图号，则默认第一张图
                                        else
                                            defect.ImageIndex = TransChannelIndex(ListChannelSum, ListChannelNum, itemSnapResult.InspectResults[i].defectImgIdx[k]);
                                        defect.DefectTypeIndex = itemSnapResult.InspectResults[i].defectType[k];
                                        HTuple tuple = new HTuple(itemSnapResult.InspectResults[i].inspectDetail.ToString(itemSnapResult.InspectResults[i].defectType[k]));
                                        defect.ErrorDetail = tuple;
                                        list_DefectData.Add(defect);

                                    }
                                }
                                HObject saveDefectReg = null;
                                HObject _clipRegion = null;
                                HTuple _row1, _col1, _row2, _col2;
                                HTuple hom2D;
                                HObject ImageNew = null;
                                HObject saveClipReg = null, saveWireDxf = null;
                                for (int j = 0; j < itemSnapResult._2dImage.CountObj(); j++)
                                {
                                    InspectionData data = new InspectionData();
                                    data.RowIndex = itemSnapResult.InspectResults[i].realRow;
                                    data.ColumnIndex = itemSnapResult.InspectResults[i].realCol;
                                    data.InspectionResult = itemSnapResult.InspectResults[i].OkOrNg ? InspectionResults.OK : InspectionResults.NG;
                                    try
                                    {

                                        _clipRegion = itemSnapResult.ClipRegs[j].SelectObj(i + 1);
                                        HOperatorSet.SmallestRectangle1(_clipRegion, out _row1, out _col1, out _row2, out _col2);
                                        //if (_row1 < 0) continue;
                                        //if (_col1 < 0) continue;
                                        HOperatorSet.VectorAngleToRigid(_row1, _col1, 0, 0, 0, 0, out hom2D);
                                        HOperatorSet.AffineTransRegion(_clipRegion, out saveClipReg, hom2D, "nearest_neighbor");
                                        HOperatorSet.AffineTransContourXld(itemSnapResult.VectorWires[i].O, out saveWireDxf, hom2D);
                                        HOperatorSet.AffineTransRegion(itemSnapResult.VectorDefectRegs[i].O, out saveDefectReg, hom2D, "nearest_neighbor");
                                        HOperatorSet.CropRectangle1(itemSnapResult._2dImage.SelectObj(j + 1), out ImageNew, _row1, _col1, _row2, _col2);
                                    }
                                    catch (Exception ex)
                                    {
                                        HTLog.Error(ex.ToString());
                                    }
                                    data.List_DefectData = list_DefectData;
                                    data.Image = ImageNew.CopyObj(1, -1);
                                    data.Region = saveDefectReg.CopyObj(1, -1);
                                    data.Wire = saveWireDxf.CopyObj(1, -1);
                                    list_InspectionData.Add(data);
                                    saveDefectReg.Dispose();
                                    ImageNew.Dispose();
                                    saveWireDxf.Dispose();
                                }
                            }
                            IsDbLocked = true;
                            //DateTime t1 = DateTime.Now;
                            DataManager.AddInspectionData(list_InspectionData);
                            //HTLog.Info("存单视野数据" + "耗毫秒：" + (DateTime.Now.Subtract(t1).TotalMilliseconds).ToString());
                            IsDbLocked = false;
                            list_InspectionData.Clear();
                        }
                        catch (Exception ex)
                        {
                            HTLog.Error(ex.ToString());
                        }
                        //HTLog.Info("将检测数据转为指定格式" + "耗秒：" + (DateTime.Now.Subtract(t1).TotalMilliseconds / 1000).ToString("F3"));

                    }
                }
                catch (Exception ex)
                {
                    HTLog.Error(ex.ToString());
                }
                finally
                {
                    saveResultFlag = false;
                }
            }
        }
        /// <summary>
        /// 根据图像数据,得到每张图通道总数列表,每张图通道数列表,索引为图片号
        /// </summary>
        /// <param name="img">图像源</param>
        /// <param name="ListChannelSum">包括当前图以及前面图的通道总数列表,索引为图片号</param>
        /// <param name="ListChannelNum">当前图的通道数列表,索引为图片号</param>
        void PrepareTransImageIndex(HObject img, out HTuple ListChannelSum, out HTuple ListChannelNum)
        {
            HTuple temp = null;
            int ChannelSum = 0;
            ListChannelSum = new HTuple();
            ListChannelNum = new HTuple();
            ListChannelSum.Append(0);
            for (int i = 0; i < img.CountObj(); i++)
            {
                HOperatorSet.CountChannels(img.SelectObj(i + 1), out temp);
                ChannelSum += temp;
                ListChannelNum.Append(temp.I);
                ListChannelSum.Append(ChannelSum);
            }
        }
        /// <summary>
        /// 将总通道号转化为图号
        /// </summary>
        /// <param name="ListChannelSum"></param>
        /// <param name="ListChannelNum"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public int TransChannelIndex(HTuple ListChannelSum, HTuple ListChannelNum, int a)
        {
            for (int i = 0; i < ListChannelNum.Length; i++)
            {
                if (a >= ListChannelSum[i].I && a < ListChannelSum[i + 1].I)
                {
                    if (ListChannelNum[i].I == 3)
                    {
                        return a - ListChannelSum[i].I;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 根据通道总数列表，通道数列表，当前通道号(所有图拆通道得到的通道索引)，得到其对应图号
        /// </summary>
        /// <param name="ListChannelSum">通道总数列表</param>
        /// <param name="ListChannelNum">通道数列表</param>
        /// <param name="ThisChannel">当前通道号(所有图拆通道得到的通道索引)</param>
        /// <returns>对应图号</returns>
        public int TransImageIndex(HTuple ListChannelSum, HTuple ListChannelNum, int ThisChannel)
        {
            for (int i = 0; i < ListChannelNum.Length; i++)
            {
                if (ThisChannel >= ListChannelSum[i].I && ThisChannel < ListChannelSum[i + 1].I)
                {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// 保存检测结果
        /// </summary>
        public void SaveResult()
        {
            while (true)
            {
                if (!saveResultFlag)
                {
                    Thread.Sleep(500);
                    continue;
                }
                frameResultFolder = imageFolder + "\\" + "Result\\" + ProductMagzine.ActivePdt + "\\" + App.obj_Process.LotId + "\\" + App.obj_Process.FrameId;
                Directory.CreateDirectory(frameResultFolder);
                IniFiles config = new IniFiles(frameResultFolder + "\\inspectionResult.ini");
                SnapResult itemSnapResult = null;
                try
                {
                    //处理拍照位检测结果队列
                    while (qSnapResult != null && !qSnapResult.IsEmpty)
                    {
                        if (itemSnapResult != null)
                        {
                            itemSnapResult.Dispose();
                        }
                        if (!qSnapResult.TryDequeue(out itemSnapResult))
                        {
                            continue;
                        };
                        string snapPath = frameFolder + "\\Row" +
                            itemSnapResult.Snap_r + "-" + itemSnapResult.Snap_c;
                        string snapResultPath = frameResultFolder;
                        Directory.CreateDirectory(snapPath);
                        // Save image(只有三个条件都成立才不保存图，所以false的时候存图)
                        if ((itemSnapResult.InspectFlag && itemSnapResult.IsAllOkSnap && App.obj_SystemConfig.ImageNgSave == 1) == false && App.obj_SystemConfig.ImageNgSave != 2)
                        {
                            for (int i = 0; i < itemSnapResult._2dImage.CountObj(); i++)
                            {
                                HOperatorSet.WriteImage(itemSnapResult._2dImage.SelectObj(i + 1), "tiff", 0, snapPath + "\\" + i + ".tiff");
                            }
                        }
                        HTuple SnapX = null, SnapY = null;
                        SnapX = itemSnapResult.Snap_x;
                        SnapY = itemSnapResult.Snap_y;
                        HOperatorSet.WriteTuple(SnapX, snapPath + "\\" + "SnapX.tup");
                        HOperatorSet.WriteTuple(SnapY, snapPath + "\\" + "SnapY.tup");
                        HObject ImageNew = null;
                        HObject _clipRegion = null;
                        HObject saveClipReg = null, saveDefectReg = null, saveWireDxf = null;
                        HTuple _row1, _col1, _row2, _col2;
                        HTuple hom2D;
                        for (int i = 0; i < itemSnapResult.InspectResults.Count; i++)
                        {
                            config.WriteString("inspectionResult", itemSnapResult.InspectResults[i].realRow + "-" + itemSnapResult.InspectResults[i].realCol,
                                itemSnapResult.InspectResults[i].OkOrNg ? "OK" : "NG");
                            if (!itemSnapResult.InspectResults[i].OkOrNg)//只保存NG芯片数据
                            {
                                string clipPath = snapResultPath + "\\" + "Clip" + itemSnapResult.InspectResults[i].realRow + "-" + (itemSnapResult.InspectResults[i].realCol);
                                Directory.CreateDirectory(clipPath);
                                for (int j = 0; j < itemSnapResult._2dImage.CountObj(); j++)
                                {
                                    _clipRegion = itemSnapResult.ClipRegs[j].SelectObj(i + 1);
                                    HOperatorSet.SmallestRectangle1(_clipRegion, out _row1, out _col1, out _row2, out _col2);
                                    HOperatorSet.VectorAngleToRigid(_row1, _col1, 0, 0, 0, 0, out hom2D);
                                    HOperatorSet.CropRectangle1(itemSnapResult._2dImage.SelectObj(j + 1), out ImageNew, _row1, _col1, _row2, _col2);
                                    HOperatorSet.WriteImage(ImageNew, "tiff", 0, clipPath + "\\" + j + ".tiff"); HOperatorSet.AffineTransRegion(_clipRegion, out saveClipReg, hom2D, "nearest_neighbor");
                                    HOperatorSet.AffineTransRegion(itemSnapResult.VectorDefectRegs[i].O, out saveDefectReg, hom2D, "nearest_neighbor");
                                    HOperatorSet.AffineTransContourXld(itemSnapResult.VectorWires[i].O, out saveWireDxf, hom2D);

                                    HOperatorSet.WriteRegion(saveClipReg, clipPath + "\\" + "clip" + ".reg");
                                    HOperatorSet.WriteRegion(saveDefectReg, clipPath + "\\" + "defect" + ".reg");
                                    HOperatorSet.WriteContourXldDxf(saveWireDxf, clipPath + "\\" + "wire" + ".dxf");
                                    HOperatorSet.WriteTuple(itemSnapResult.InspectResults[i].defectType, clipPath + "\\" + "defectType.tup");
                                }

                            }
                        }
                        if (ImageNew != null) ImageNew.Dispose();
                        if (_clipRegion != null) _clipRegion.Dispose();
                        if (saveDefectReg != null) saveDefectReg.Dispose();
                        if (saveWireDxf != null) saveWireDxf.Dispose();
                        if (saveClipReg != null) saveClipReg.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    HTLog.Error(ex.ToString());
                }
                finally
                {
                    saveResultFlag = false;
                }
            }
        }
        public bool GetRCfromXY(ref StructInspectResult inspectResult, ImageCache imageCache)
        {
            double minSumDisPow = 100000000;
            double thisSumDisPow = 0;
            double acceptDis = 2;
            int minIdx = -1;
            for (int i = 0; i < clipPosNum; i++)
            {
                thisSumDisPow =
                    Math.Sqrt(Math.Pow(ThisClipPostions[i].x - (imageCache.X + inspectResult.deltaX), 2) + Math.Pow(ThisClipPostions[i].y - (imageCache.Y + inspectResult.deltaY), 2));
                if (thisSumDisPow < minSumDisPow && thisSumDisPow < acceptDis)
                {
                    minSumDisPow = thisSumDisPow;
                    minIdx = i;
                }
            }
            if (minIdx == -1) return false;
            inspectResult.realRow = ThisClipPostions[minIdx].r;
            inspectResult.realCol = ThisClipPostions[minIdx].c;

            IniFiles configDeltaXYAlg = new IniFiles(Application.StartupPath + "\\DeltaXYAlg.ini");
            configDeltaXYAlg.WriteString("DeltaXYAlg", ",deltaX=" + inspectResult.deltaX.ToString("F2") + ",deltaY=" +
                    inspectResult.deltaY.ToString("F2"), inspectResult.realRow + "-" + inspectResult.realCol);
            IniFiles configDeltaXYMap = new IniFiles(Application.StartupPath + "\\DeltaXYMap.ini");
            configDeltaXYMap.WriteString("DeltaXYMap", ",deltaX=" + (ThisClipPostions[minIdx].x - imageCache.X).ToString("F2") +
                    ",deltaY=" + (ThisClipPostions[minIdx].y - imageCache.Y).ToString("F2"), inspectResult.realRow + "-" + inspectResult.realCol);
            //configXY2RC.WriteString("GetRCfromXY",
            //        "X=" + (imageCache.X + inspectResult.deltaX).ToString("F2") + ",Y=" +
            //        (imageCache.Y - inspectResult.deltaY).ToString("F2")+","+
            //        inspectResult.realRow + "-" + inspectResult.realCol +
            //        ",NowX" , ThisClipPostions[minIdx].x.ToString("F2") +
            //        ",NowY=" + ThisClipPostions[minIdx].y.ToString("F2"));
            return true;
        }
        private delegate void ShowImageDelegate(HTWindowControl htWindow, HObject image, HObject region);
        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="htWindow">图像视窗</param>
        /// <param name="image">图像数据</param>
        /// <param name="region">区域数据</param>
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
                if (htWindow.Image == null || !htWindow.Image.IsInitialized())
                    htWindow.RefreshWindow(image, region, "fit");//适应窗口
                else
                {
                    if (image == null)
                    {
                        htWindow.Image = null;
                        HOperatorSet.ClearWindow(htWindow.HTWindow.HalconWindow);
                    }
                    else
                    {
                        if (!image.IsInitialized())
                        {
                            htWindow.Image = null;
                            HOperatorSet.ClearWindow(htWindow.HTWindow.HalconWindow);
                        }
                        else
                        {
                            HTuple width1, height1, width2, height2;
                            HOperatorSet.GetImageSize(htWindow.Image, out width1, out height1);
                            HOperatorSet.GetImageSize(image, out width2, out height2);
                            if (width1.Type != HTupleType.EMPTY && height1.Type != HTupleType.EMPTY)
                            {
                                if (width1.I != width2.I || height1.I != height2.I)
                                {
                                    htWindow.RefreshWindow(image, region, "fit");//可以不显示区域
                                }
                                else
                                    htWindow.RefreshWindow(image, region, "");//可以不显示区域
                            }
                            else
                                htWindow.RefreshWindow(image, region, "");//可以不显示区域
                        }
                    }
                }
                htWindow.SetInteractive(true);
                //htWindow.ColorName = "green";
            }
        }
        /// <summary>
        /// 到指定位置拍照
        /// </summary>
        /// <param name="x">相机X坐标</param>
        /// <param name="y">相机Y坐标</param>
        /// <param name="z">相机Z坐标</param>
        /// <param name="htWindow">图像视窗</param>
        /// <param name="img">拍到的图像</param>
        public void SnapPos(double x, double y, double z, HTHalControl.HTWindowControl htWindow, out HObject img)
        {
            img = null;
            if (!App.obj_Chuck.XYZ_Move(x, y, z))
            {
                throw new Exception(App.obj_Chuck.GetLastErrorString());
            }

            if (App.obj_Vision.RunMode == 1) return;
            //3. 触发
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();
            //4. 取图
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].acq.Dispose();
            string errStr = "";
            errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref img, 1, 5000);//获取图片
                                                                                                         //FormJobs.Instance.ShowImage(Image, null);
            App.obj_Vision.ShowImage(htWindow, img, null);
            if (errStr != "")
            {
                HTUi.PopError(errStr);
            }
        }

        #endregion

        #region 视觉方法中用到的自定义类

        public struct MinMax
        {
            public Double min;
            public Double max;
        }
        /// <summary>
        /// 自动聚焦参数
        /// </summary>
        [Serializable]
        public class AutoFocusParam
        {
            /// <summary>
            /// 镜头聚焦初始位置
            /// </summary>
            public Double initFocusPos;
            /// <summary>
            /// 自动聚焦范围
            /// </summary>
            public MinMax focusRange;
            /// <summary>
            /// 触发间隔
            /// </summary>
            public Double trgInterval;
            /// <summary>
            /// 镜头Z轴偏移量（主要用于记录切割线相对2D镜头Z轴的高度差）
            /// 单位:um
            /// </summary>
            public Double zOffset;
            /// <summary>
            /// 计算图像梯度的核大小
            /// </summary>
            public int maskSize;
            /// <summary>
            /// 聚焦区域
            /// </summary>
            //[XmlIgnore]
            public HObject focusRegion;
            public AutoFocusParam()
            {
                initFocusPos = 0;
                focusRange = new MinMax();
                trgInterval = 0;
                zOffset = 0;
                maskSize = 5;
                focusRegion = new HObject();
                HOperatorSet.GenEmptyObj(out focusRegion);
            }
            /// <summary>
            /// 释放聚焦信息所有的资源
            /// </summary>
            public void Dispose()
            {
                initFocusPos = 0;
                focusRange.min = 0;
                focusRange.max = 0;
                trgInterval = 0;
                zOffset = 0;
                maskSize = 5;
                focusRegion.Dispose();
            }
        }
        public class Visions
        {
            public struct HalconSystemPrm
            {
                private HTuple imgW;
                private HTuple imgH;
                private HTuple clipRgn;
                public HTuple ImgWidth { get { return this.imgW; } }
                public HTuple ImgHeight { get { return this.imgH; } }
                public HTuple ClipRegion { get { return this.clipRgn; } }
                public HalconSystemPrm(int imgWidth, int imgHeight, string clipRegion)
                {
                    this.imgW = imgWidth;
                    this.imgH = imgHeight;
                    this.clipRgn = clipRegion;
                }
            }
            public static void DrawCross(HTuple rows, HTuple cols, HTuple size, ref HObject cross)
            {
                if (rows == null || cols == null)
                {
                    cross.Dispose();
                    HOperatorSet.GenEmptyObj(out cross);
                    return;
                }
                if (rows.Length != cols.Length)
                {
                    cross.Dispose();
                    HOperatorSet.GenEmptyObj(out cross);
                    return;
                }
                cross.Dispose();
                HOperatorSet.GenCrossContourXld(out cross, rows, cols, size, 0);
            }
            /// <summary>
            /// 读取模板通用函数
            /// </summary>
            /// <param name="ho_show_contour"></param>
            /// <param name="hv_model_path"></param>
            /// <param name="hv_model_type"></param>
            /// <param name="hv_model_id"></param>
            /// <param name="hv_def_row"></param>
            /// <param name="hv_def_col"></param>
            /// <param name="hv_iFlag"></param>
            public static void read_model(out HObject ho_show_contour, HTuple hv_model_path, out HTuple hv_model_type,
                                          out HTuple hv_model_id, out HTuple hv_def_row, out HTuple hv_def_col, out HTuple hv_iFlag)
            {
                // Local control variables 
                HTuple hv_file_exist = null, hv_DxfStatus = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_show_contour);
                hv_model_type = new HTuple();
                hv_model_id = new HTuple();
                hv_def_row = new HTuple();
                hv_def_col = new HTuple();
                hv_iFlag = 0;
                HOperatorSet.FileExists(hv_model_path + "/modelType.tup", out hv_file_exist);
                if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
                {
                    hv_iFlag = -1;

                    return;
                }
                HOperatorSet.ReadTuple(hv_model_path + "/modelType.tup", out hv_model_type);
                HOperatorSet.FileExists(hv_model_path + "/modelID.dat", out hv_file_exist);
                if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
                {
                    hv_iFlag = -1;

                    return;
                }
                if ((int)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
                {
                    HOperatorSet.ReadNccModel(hv_model_path + "/modelID.dat", out hv_model_id);
                }
                else
                {
                    HOperatorSet.ReadShapeModel(hv_model_path + "/modelID.dat", out hv_model_id);
                }
                HOperatorSet.FileExists(hv_model_path + "/showContour.dxf", out hv_file_exist);
                if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
                {
                    hv_iFlag = -1;

                    return;
                }
                ho_show_contour.Dispose();
                HOperatorSet.ReadContourXldDxf(out ho_show_contour, hv_model_path + "/showContour.dxf",
                    new HTuple(), new HTuple(), out hv_DxfStatus);
                HOperatorSet.FileExists(hv_model_path + "/defRow.tup", out hv_file_exist);
                if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
                {
                    hv_iFlag = -1;

                    return;
                }
                HOperatorSet.ReadTuple(hv_model_path + "/defRow.tup", out hv_def_row);
                HOperatorSet.FileExists(hv_model_path + "/defCol.tup", out hv_file_exist);
                if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
                {
                    hv_iFlag = -1;

                    return;
                }
                HOperatorSet.ReadTuple(hv_model_path + "/defCol.tup", out hv_def_col);
                return;
            }

            // 检测区域，聚焦区域，type只有ncc或shape
            public static void read_focus_model(out HObject ho_match_region, out HObject ho_focus_region,
        HTuple hv_model_path, out HTuple hv_model_type, out HTuple hv_modelID, out HTuple hv_iFlag)
            {



                // Local iconic variables 

                HObject ho_show_contour;

                // Local control variables 

                HTuple hv_def_row = null, hv_def_col = null;
                HTuple hv_iFlag1 = null, hv_FileExists = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_match_region);
                HOperatorSet.GenEmptyObj(out ho_focus_region);
                HOperatorSet.GenEmptyObj(out ho_show_contour);
                //***********************************
                hv_iFlag = 0;
                ho_show_contour.Dispose();
                read_model(out ho_show_contour, hv_model_path, out hv_model_type, out hv_modelID,
                    out hv_def_row, out hv_def_col, out hv_iFlag1);
                if ((int)(new HTuple(hv_iFlag1.TupleNotEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_model_path + "/match_region.reg", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_match_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_match_region, hv_model_path + "/match_region.reg");
                }
                else
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_model_path + "/focus_region.reg", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_focus_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_focus_region, hv_model_path + "/focus_region.reg");
                }
                else
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();

                    return;
                }
                ho_show_contour.Dispose();

                return;
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="ho_Image"></param>
            /// <param name="ho_MatchRegion"></param>
            /// <param name="ho_FocusRegion"></param>
            /// <param name="ho_Depth">不同位置每个像素对应的高度</param>
            /// <param name="ho_Confidence"></param>
            /// <param name="ho_SharpImage"></param>
            /// <param name="hv_IsGPU"></param>
            /// <param name="hv_MeanSize"></param>
            /// <param name="hv_ModelID"></param>
            /// <param name="hv_ModelType"></param>
            /// <param name="hv_AngleStart"></param>
            /// <param name="hv_AngleExtent"></param>
            /// <param name="hv_MatchThresh"></param>
            /// <param name="hv_ConfidenceThresh"></param>
            /// <param name="hv_IsMean"></param>
            /// <param name="hv_FocusHeigth"></param>
            /// <param name="hv_iFlag"></param>
            //   public void auto_focus_via_dff(HObject ho_Image, HObject ho_MatchRegion, HObject ho_FocusRegion,
            //out HObject ho_Depth, out HObject ho_Confidence, out HObject ho_SharpImage,
            //HTuple hv_IsGPU, HTuple hv_MeanSize, HTuple hv_ModelID, HTuple hv_ModelType,
            //HTuple hv_AngleStart, HTuple hv_AngleExtent, HTuple hv_MatchThresh, HTuple hv_ConfidenceThresh,
            //HTuple hv_IsMean, out HTuple hv_FocusHeigth, out HTuple hv_iFlag)
            //   {




            //       // Local iconic variables 

            //       HObject ho_MultiChannelImage, ho_DepthMean;
            //       HObject ho_ImageReduced, ho_RegionAffineTrans = null, ho_ObjectSelected = null;
            //       HObject ho_Region = null, ho_RegionIntersection = null;

            //       // Local control variables 

            //       HTuple hv_Number = null, hv_Row = new HTuple();
            //       HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
            //       HTuple hv_Score = new HTuple(), hv_HomMat2D = new HTuple();
            //       HTuple hv_Index = new HTuple(), hv_Mean = new HTuple();
            //       HTuple hv_Deviation = new HTuple(), hv_Min = new HTuple();
            //       HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
            //       // Initialize local and output iconic variables 
            //       HOperatorSet.GenEmptyObj(out ho_Depth);
            //       HOperatorSet.GenEmptyObj(out ho_Confidence);
            //       HOperatorSet.GenEmptyObj(out ho_SharpImage);
            //       HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);
            //       HOperatorSet.GenEmptyObj(out ho_DepthMean);
            //       HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            //       HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            //       HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            //       HOperatorSet.GenEmptyObj(out ho_Region);
            //       HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            //       hv_iFlag = 0;
            //       hv_FocusHeigth = new HTuple();
            //       ho_Depth.Dispose();
            //       HOperatorSet.GenEmptyObj(out ho_Depth);
            //       ho_SharpImage.Dispose();
            //       HOperatorSet.GenEmptyObj(out ho_SharpImage);
            //       //*****************************************
            //       HOperatorSet.CountObj(ho_FocusRegion, out hv_Number);
            //       if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
            //       {
            //           hv_iFlag = -1;
            //           ho_MultiChannelImage.Dispose();
            //           ho_DepthMean.Dispose();
            //           ho_ImageReduced.Dispose();
            //           ho_RegionAffineTrans.Dispose();
            //           ho_ObjectSelected.Dispose();
            //           ho_Region.Dispose();
            //           ho_RegionIntersection.Dispose();

            //           return;
            //       }
            //       ho_MultiChannelImage.Dispose();
            //       HOperatorSet.ChannelsToImage(ho_Image, out ho_MultiChannelImage);
            //       if ((int)(new HTuple(hv_IsGPU.TupleEqual(1))) != 0)
            //       {
            //           ho_Depth.Dispose(); ho_Confidence.Dispose();
            //           depth_from_focus_gpu(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
            //               out hv_iFlag);
            //           if ((int)(new HTuple(hv_iFlag.TupleNotEqual(0))) != 0)
            //           {
            //               hv_iFlag = -2;
            //               ho_MultiChannelImage.Dispose();
            //               ho_DepthMean.Dispose();
            //               ho_ImageReduced.Dispose();
            //               ho_RegionAffineTrans.Dispose();
            //               ho_ObjectSelected.Dispose();
            //               ho_Region.Dispose();
            //               ho_RegionIntersection.Dispose();

            //               return;
            //           }
            //       }
            //       else
            //       {
            //           ho_Depth.Dispose(); ho_Confidence.Dispose();
            //           HOperatorSet.DepthFromFocus(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
            //               "bandpass", "local");
            //       }
            //       ho_DepthMean.Dispose();
            //       HOperatorSet.MeanImage(ho_Depth, out ho_DepthMean, hv_MeanSize, hv_MeanSize);
            //       ho_SharpImage.Dispose();
            //       HOperatorSet.SelectGrayvaluesFromChannels(ho_MultiChannelImage, ho_DepthMean,
            //           out ho_SharpImage);
            //       ho_ImageReduced.Dispose();
            //       HOperatorSet.ReduceDomain(ho_SharpImage, ho_MatchRegion, out ho_ImageReduced);
            //       if ((int)(new HTuple(hv_ModelType.TupleEqual(0))) != 0)
            //       {
            //           HOperatorSet.FindNccModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
            //               0.2, 1, 0.5, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            //       }
            //       else
            //       {
            //           HOperatorSet.FindShapeModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
            //               0.2, 1, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle,
            //               out hv_Score);
            //       }
            //       if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(1))).TupleAnd(
            //           new HTuple(hv_Score.TupleGreater(hv_MatchThresh)))) != 0)
            //       {
            //           HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out hv_HomMat2D);
            //           ho_RegionAffineTrans.Dispose();
            //           HOperatorSet.AffineTransRegion(ho_FocusRegion, out ho_RegionAffineTrans, hv_HomMat2D,
            //               "nearest_neighbor");
            //           HTuple end_val31 = hv_Number;
            //           HTuple step_val31 = 1;
            //           for (hv_Index = 1; hv_Index.Continue(end_val31, step_val31); hv_Index = hv_Index.TupleAdd(step_val31))
            //           {
            //               ho_ObjectSelected.Dispose();
            //               HOperatorSet.SelectObj(ho_RegionAffineTrans, out ho_ObjectSelected, hv_Index);
            //               ho_Region.Dispose();
            //               HOperatorSet.Threshold(ho_Confidence, out ho_Region, hv_ConfidenceThresh,
            //                   255);
            //               ho_RegionIntersection.Dispose();
            //               HOperatorSet.Intersection(ho_Region, ho_ObjectSelected, out ho_RegionIntersection
            //                   );
            //               if ((int)(new HTuple(hv_IsMean.TupleEqual(1))) != 0)
            //               {
            //                   HOperatorSet.Intensity(ho_RegionIntersection, ho_Depth, out hv_Mean, out hv_Deviation);
            //                   hv_FocusHeigth = hv_FocusHeigth.TupleConcat(hv_Mean);
            //               }
            //               else
            //               {
            //                   HOperatorSet.MinMaxGray(ho_RegionIntersection, ho_Depth, 50, out hv_Min,
            //                       out hv_Max, out hv_Range);
            //                   hv_FocusHeigth = hv_FocusHeigth.TupleConcat(hv_Min);
            //               }
            //           }
            //       }
            //       else
            //       {
            //           hv_iFlag = -3;
            //           ho_MultiChannelImage.Dispose();
            //           ho_DepthMean.Dispose();
            //           ho_ImageReduced.Dispose();
            //           ho_RegionAffineTrans.Dispose();
            //           ho_ObjectSelected.Dispose();
            //           ho_Region.Dispose();
            //           ho_RegionIntersection.Dispose();

            //           return;
            //       }
            //       ho_MultiChannelImage.Dispose();
            //       ho_DepthMean.Dispose();
            //       ho_ImageReduced.Dispose();
            //       ho_RegionAffineTrans.Dispose();
            //       ho_ObjectSelected.Dispose();
            //       ho_Region.Dispose();
            //       ho_RegionIntersection.Dispose();

            //       return;
            //   }
            /// <summary>
            /// 保存模板通用函数
            /// </summary>
            /// <param name="ho_show_contour"></param>
            /// <param name="hv_model_path"></param>
            /// <param name="hv_model_type"></param>
            /// <param name="hv_model_id"></param>
            /// <param name="hv_def_row"></param>
            /// <param name="hv_def_col"></param>
            /// <param name="hv_iFlag"></param>
            public static void write_model(HObject ho_show_contour, HTuple hv_model_path, HTuple hv_model_type,
                                           HTuple hv_model_id, HTuple hv_def_row, HTuple hv_def_col, out HTuple hv_iFlag)
            {
                // Initialize local and output iconic variables 
                hv_iFlag = 0;
                if ((int)(new HTuple((new HTuple(hv_model_type.TupleLength())).TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    return;
                }
                HOperatorSet.WriteTuple(hv_model_type, hv_model_path + "/modelType.tup");
                if ((int)(new HTuple((new HTuple(hv_model_id.TupleLength())).TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    return;
                }
                if ((int)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
                {
                    HOperatorSet.WriteNccModel(hv_model_id, hv_model_path + "/modelID.dat");
                }
                else
                {
                    HOperatorSet.WriteShapeModel(hv_model_id, hv_model_path + "/modelID.dat");
                }
                HOperatorSet.WriteContourXldDxf(ho_show_contour, hv_model_path + "/showContour.dxf");
                HOperatorSet.WriteTuple(hv_def_row, hv_model_path + "/defRow.tup");
                HOperatorSet.WriteTuple(hv_def_col, hv_model_path + "/defCol.tup");
                return;
            }
            /// <summary>
            /// 0:Ncc;1:Shape
            /// </summary>
            /// <param name="modelId"></param>
            /// <param name="modelType"></param>
            /// <returns></returns>
            public static HTuple CopyModel(HTuple modelId, HTuple modelType)
            {
                HTuple modelID = null;
                switch (modelType.I)
                {
                    case 0:
                        modelID = CopyNccModel(modelId);
                        break;
                    case 1:
                        modelID = CopyShapeModel(modelId);
                        break;
                }
                return modelID;
            }
            //model类Copy,需要序列化和反序列化的过程
            public static HTuple CopyNccModel(HTuple nccModelId)
            {
                HTuple copyNccModeId = new HTuple();
                HTuple serializedItemHandle = new HTuple();
                HOperatorSet.SerializeNccModel(nccModelId, out serializedItemHandle);
                HOperatorSet.DeserializeNccModel(serializedItemHandle, out copyNccModeId);
                return copyNccModeId;
            }
            public static HTuple CopyShapeModel(HTuple shapeModelId)
            {
                HTuple copyShapeModeId = new HTuple();
                HTuple serializedItemHandle = new HTuple();
                HOperatorSet.SerializeShapeModel(shapeModelId, out serializedItemHandle);
                HOperatorSet.DeserializeShapeModel(serializedItemHandle, out copyShapeModeId);
                return copyShapeModeId;
            }


        }
        public class Model
        {
            /// <summary>
            /// 模板句柄
            /// </summary>
            public HTuple modelID;
            /// <summary>
            /// 模板类型
            /// </summary>
            public HTuple modelType;
            /// <summary>
            /// 映射点行坐标
            /// </summary>
            public HTuple defRows;
            /// <summary>
            /// 映射点列坐标
            /// </summary>
            public HTuple defCols;
            /// <summary>
            /// 匹配分数阈值
            /// </summary>
            public HTuple scoreThresh;
            /// <summary>
            /// 匹配起始角度
            /// </summary>
            public HTuple angleStart;
            /// <summary>
            /// 匹配角度范围
            /// </summary>
            public HTuple angleExtent;
            /// <summary>
            /// 用于显示的轮廓
            /// </summary>
            public HObject showContour;
            /// <summary>
            /// 用于记录做模板时设置的点位信息
            /// </summary>
            public HObject showImage;
            /// <summary>
            /// 匹配区域
            /// </summary>
            public HObject matchRegion;

            public HObject FocusRegion;

            public HObject ho_RegionDilation;

            public HTuple hv_confidence_thresh = null;
            public HTuple hv_is_gpu = null;
            public HTuple hv_mean_size = null;
            public HTuple hv_model_type = null;
            public HTuple hv_is_mean = null;
            public HTuple hv_angle_start = null;
            public HTuple hv_angle_extent = null;
            public HTuple hv_match_thresh = null;


            public Model()
            {
                modelID = new HTuple();
                modelType = new HTuple();
                defRows = new HTuple();
                defCols = new HTuple();
                scoreThresh = new HTuple();
                angleStart = new HTuple();
                angleExtent = new HTuple();
                showContour = new HObject();
                HOperatorSet.GenEmptyObj(out showContour);
                showImage = new HObject();
                HOperatorSet.GenEmptyObj(out showImage);
                matchRegion = new HObject();
                HOperatorSet.GenEmptyObj(out matchRegion);
                HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            }


            // public void auto_focus_via_dff(HObject ho_Image, HObject ho_MatchRegion, HObject ho_FocusRegion,
            //out HObject ho_Depth, out HObject ho_Confidence, out HObject ho_SharpImage,
            //HTuple hv_IsGPU, HTuple hv_MeanSize, HTuple hv_ModelID, HTuple hv_ModelType,
            //HTuple hv_AngleStart, HTuple hv_AngleExtent, HTuple hv_MatchThresh, HTuple hv_ConfidenceThresh,
            //HTuple hv_IsMean, out HTuple hv_FocusHeigth, out HTuple hv_iFlag)
            // {

            //     ho_MatchRegion = ModelOperations.matchRegion;
            //     ModelOperations.FocusRegion;
            //     ModelOperations.hv_is_gpu;
            //     ModelOperations.hv_mean_size;
            //     ModelOperations.modelID;
            //     ModelOperations.hv_model_type;

            //     ModelOperations.hv_angle_start
            //     ModelOperations.angleExtent;
            //     ModelOperations.hv_match_thresh


            //     ModelOperations.hv_ConfidenceThresh;

            //     ModelOperations.hv_IsMean;


            //     // Local iconic variables 

            //     HObject ho_MultiChannelImage, ho_DepthMean;
            //     HObject ho_ImageReduced, ho_RegionAffineTrans = null, ho_ObjectSelected = null;
            //     HObject ho_Region = null, ho_RegionIntersection = null;

            //     // Local control variables 

            //     HTuple hv_Number = null, hv_Row = new HTuple();
            //     HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
            //     HTuple hv_Score = new HTuple(), hv_HomMat2D = new HTuple();
            //     HTuple hv_Index = new HTuple(), hv_Mean = new HTuple();
            //     HTuple hv_Deviation = new HTuple(), hv_Min = new HTuple();
            //     HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
            //     // Initialize local and output iconic variables 
            //     HOperatorSet.GenEmptyObj(out ho_Depth);
            //     HOperatorSet.GenEmptyObj(out ho_Confidence);
            //     HOperatorSet.GenEmptyObj(out ho_SharpImage);
            //     HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);
            //     HOperatorSet.GenEmptyObj(out ho_DepthMean);
            //     HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            //     HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            //     HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            //     HOperatorSet.GenEmptyObj(out ho_Region);
            //     HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            //     hv_iFlag = 0;
            //     hv_FocusHeigth = new HTuple();
            //     ho_Depth.Dispose();
            //     HOperatorSet.GenEmptyObj(out ho_Depth);
            //     ho_SharpImage.Dispose();
            //     HOperatorSet.GenEmptyObj(out ho_SharpImage);
            //     //*****************************************
            //     HOperatorSet.CountObj(ho_FocusRegion, out hv_Number);
            //     if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
            //     {
            //         hv_iFlag = -1;
            //         ho_MultiChannelImage.Dispose();
            //         ho_DepthMean.Dispose();
            //         ho_ImageReduced.Dispose();
            //         ho_RegionAffineTrans.Dispose();
            //         ho_ObjectSelected.Dispose();
            //         ho_Region.Dispose();
            //         ho_RegionIntersection.Dispose();

            //         return;
            //     }
            //     ho_MultiChannelImage.Dispose();
            //     HOperatorSet.ChannelsToImage(ho_Image, out ho_MultiChannelImage);
            //     if ((int)(new HTuple(hv_IsGPU.TupleEqual(1))) != 0)
            //     {
            //         ho_Depth.Dispose(); ho_Confidence.Dispose();
            //         depth_from_focus_gpu(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
            //             out hv_iFlag);
            //         if ((int)(new HTuple(hv_iFlag.TupleNotEqual(0))) != 0)
            //         {
            //             hv_iFlag = -2;
            //             ho_MultiChannelImage.Dispose();
            //             ho_DepthMean.Dispose();
            //             ho_ImageReduced.Dispose();
            //             ho_RegionAffineTrans.Dispose();
            //             ho_ObjectSelected.Dispose();
            //             ho_Region.Dispose();
            //             ho_RegionIntersection.Dispose();

            //             return;
            //         }
            //     }
            //     else
            //     {
            //         ho_Depth.Dispose(); ho_Confidence.Dispose();
            //         HOperatorSet.DepthFromFocus(ho_MultiChannelImage, out ho_Depth, out ho_Confidence,
            //             "bandpass", "local");
            //     }
            //     ho_DepthMean.Dispose();
            //     HOperatorSet.MeanImage(ho_Depth, out ho_DepthMean, hv_MeanSize, hv_MeanSize);
            //     ho_SharpImage.Dispose();
            //     HOperatorSet.SelectGrayvaluesFromChannels(ho_MultiChannelImage, ho_DepthMean,
            //         out ho_SharpImage);
            //     ho_ImageReduced.Dispose();
            //     HOperatorSet.ReduceDomain(ho_SharpImage, ho_MatchRegion, out ho_ImageReduced);
            //     if ((int)(new HTuple(hv_ModelType.TupleEqual(0))) != 0)
            //     {
            //         HOperatorSet.FindNccModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
            //             0.2, 1, 0.5, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            //     }
            //     else
            //     {
            //         HOperatorSet.FindShapeModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
            //             0.2, 1, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle,
            //             out hv_Score);
            //     }
            //     if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(1))).TupleAnd(
            //         new HTuple(hv_Score.TupleGreater(hv_MatchThresh)))) != 0)
            //     {
            //         HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out hv_HomMat2D);
            //         ho_RegionAffineTrans.Dispose();
            //         HOperatorSet.AffineTransRegion(ho_FocusRegion, out ho_RegionAffineTrans, hv_HomMat2D,
            //             "nearest_neighbor");
            //         HTuple end_val31 = hv_Number;
            //         HTuple step_val31 = 1;
            //         for (hv_Index = 1; hv_Index.Continue(end_val31, step_val31); hv_Index = hv_Index.TupleAdd(step_val31))
            //         {
            //             ho_ObjectSelected.Dispose();
            //             HOperatorSet.SelectObj(ho_RegionAffineTrans, out ho_ObjectSelected, hv_Index);
            //             ho_Region.Dispose();
            //             HOperatorSet.Threshold(ho_Confidence, out ho_Region, hv_ConfidenceThresh,
            //                 255);
            //             ho_RegionIntersection.Dispose();
            //             HOperatorSet.Intersection(ho_Region, ho_ObjectSelected, out ho_RegionIntersection
            //                 );
            //             if ((int)(new HTuple(hv_IsMean.TupleEqual(1))) != 0)
            //             {
            //                 HOperatorSet.Intensity(ho_RegionIntersection, ho_Depth, out hv_Mean, out hv_Deviation);
            //                 hv_FocusHeigth = hv_FocusHeigth.TupleConcat(hv_Mean);
            //             }
            //             else
            //             {
            //                 HOperatorSet.MinMaxGray(ho_RegionIntersection, ho_Depth, 50, out hv_Min,
            //                     out hv_Max, out hv_Range);
            //                 hv_FocusHeigth = hv_FocusHeigth.TupleConcat(hv_Min);
            //             }
            //         }
            //     }
            //     else
            //     {
            //         hv_iFlag = -3;
            //         ho_MultiChannelImage.Dispose();
            //         ho_DepthMean.Dispose();
            //         ho_ImageReduced.Dispose();
            //         ho_RegionAffineTrans.Dispose();
            //         ho_ObjectSelected.Dispose();
            //         ho_Region.Dispose();
            //         ho_RegionIntersection.Dispose();

            //         return;
            //     }
            //     ho_MultiChannelImage.Dispose();
            //     ho_DepthMean.Dispose();
            //     ho_ImageReduced.Dispose();
            //     ho_RegionAffineTrans.Dispose();
            //     ho_ObjectSelected.Dispose();
            //     ho_Region.Dispose();
            //     ho_RegionIntersection.Dispose();

            //     return;
            // }

        }
        /// <summary>
        /// 轴参数
        /// </summary>
        [Serializable]
        public class AxisParam
        {
            /// <summary>
            /// 最大速度
            /// </summary>
            public Double maxSpeed;
            /// <summary>
            /// 加减速度
            /// </summary>
            public Double accTime;
            public AxisParam()
            {
                maxSpeed = 0;
                accTime = 0;
            }

            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                maxSpeed = 0;
                accTime = 0;
            }
        }
        /// <summary>
        /// 点集（search/matchOnly/matchFocus/calibCenter）
        /// </summary>
        [Serializable]
        public class Point
        {
            /// <summary>
            /// wafer行
            /// </summary>
            public Double r;
            /// <summary> 
            /// wafer列
            /// </summary>
            public Double c;
            /// <summary>
            /// chuck x
            /// </summary>
            public Double x;
            /// <summary>
            /// chuck y
            /// </summary>
            public Double y;
            /// <summary>
            /// 相机Z轴
            /// </summary>
            public Double z;
            /// <summary>
            /// point type(0表示matchOnly point,1表示search point,2表示matchFocus point)
            /// </summary>
            public int type;
            /// <summary>
            /// 是否匹找到的标志
            /// </summary>
            public bool found;
            public Point()
            {

            }
            public Point(Double r = 0, Double c = 0, Double x = 0, Double y = 0, Double z = 0, int type = 0)
            {
                this.r = r;
                this.c = c;
                this.x = x;
                this.y = y;
                this.z = z;
                this.type = type;
                this.found = false;
            }
            /// <summary>
            /// 浅拷贝
            /// </summary>
            /// <returns></returns>
            public Point Clone()
            {
                return this.MemberwiseClone() as Point;
            }
            /// <summary>
            /// 深拷贝
            /// </summary>
            /// <returns></returns>
            public Point DeepClone()
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as Point;
            }
            /// <summary>
            /// 清除点位，点位归0
            /// </summary>
            public void Dispose()
            {
                r = 0;
                c = 0;
                x = 0;
                z = 0;
                y = 0;
                type = 0;
                found = false;
            }
        }
        public class Show
        {
            public HObject Image;
            public int CamInd;
            /// <summary>
            /// 0表示仅显示图像，1表示自动聚焦,2表示匹配
            /// </summary>
            public int Mode;
            /// <summary>
            /// 映射点中心行坐标
            /// </summary>
            public Double Row;
            /// <summary>
            /// 映射点中心列坐标
            /// </summary>
            public Double Column;
            public Show()
            {
                Row = 0;
                Column = 0;
            }
            public void Dispose()
            {
                if (Image != null) Image.Dispose();
            }
        }
        public class Points
        {
            /// <summary>
            /// Point点集
            /// </summary>
            public List<Point> lPoints;
            public Points()
            {
                lPoints = new List<Point>();
            }
            /// <summary>
            /// 拷贝点位信息
            /// </summary>
            /// <param name="oriPoints"></param>
            /// <returns></returns>
            public bool Copy(Points oriPoints)
            {
                try
                {
                    for (int i = 0; i < oriPoints.lPoints.Count; i++)
                    {
                        Point point = new Point();
                        point.r = oriPoints.lPoints[i].r;
                        point.c = oriPoints.lPoints[i].c;
                        point.x = oriPoints.lPoints[i].x;
                        point.y = oriPoints.lPoints[i].y;
                        point.z = oriPoints.lPoints[i].z;
                        point.type = oriPoints.lPoints[i].type;
                        point.found = oriPoints.lPoints[i].found;
                        this.lPoints.Add(point);
                    }
                    return true;
                }
                catch (Exception)
                {
                    this.Dispose();
                    return false;
                }
            }
            /// <summary>
            /// 浅拷贝
            /// </summary>
            /// <returns></returns>
            public Points Clone()
            {
                return this.MemberwiseClone() as Points;
            }
            /// <summary>
            /// 深拷贝
            /// </summary>
            /// <returns></returns>
            public Points DeepClone()
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as Points;
            }
            public void Dispose()
            {
                if (lPoints == null) return;
                for (int i = 0; i < lPoints.Count; i++)
                {
                    lPoints[i].Dispose();
                }
                lPoints.Clear();
            }
        }


        #endregion
    }
}
