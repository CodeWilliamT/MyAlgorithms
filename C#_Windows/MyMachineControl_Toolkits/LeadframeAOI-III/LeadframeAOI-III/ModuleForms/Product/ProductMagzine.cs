using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using System.IO;
using HalconDotNet;
using System.Data.SQLite;
using IniDll;
using HT_Lib;


namespace LeadframeAOI
{
    public class DieInfo
    {
        public int r;
        public int c;
        public int b;//芯片所在的Block数
        public Double x;
        public Double y;
        public Double z;
        public Double u;
        public Double v;
        public void initial()
        {
            r = 0;
            c = 0;
            x = 0;
            y = 0;
            z = 0;
            u = 0;
            v = 0;
            b = 0;
        }
        public DieInfo()
        {
            r = 0;
            c = 0;
            x = 0;
            y = 0;
            z = 0;
            u = 0;
            v = 0;
            b = 0;
        }
        public DieInfo(int r_index, int c_index, Double x_pos, Double y_pos, Double z_pos, Double u_pix, Double v_pix, int blockNumber = 0)
        {
            r = r_index;
            c = c_index;
            x = x_pos;
            y = y_pos;
            z = z_pos;
            u = u_pix;
            v = v_pix;
            b = blockNumber;
        }
        public void Copy(DieInfo dieInfo)
        {
            r = dieInfo.r;
            c = dieInfo.c;
            x = dieInfo.x;
            y = dieInfo.y;
            z = dieInfo.z;
            u = dieInfo.u;
            v = dieInfo.v;
            b = dieInfo.b;
        }
    }

    public struct ImagePosition
    {
        /// <summary>
        /// 所在block
        /// </summary>
        public int b;
        /// <summary>
        /// 行索引
        /// </summary>
        public int r;
        /// <summary>
        /// 列索引
        /// </summary>
        public int c;
        /// <summary>
        /// 位置X
        /// </summary>
        public double x;
        /// <summary>
        /// Y
        /// </summary>
        public double y;
        /// <summary>
        /// Z
        /// </summary>
        public double z;

       
        public void Dispose()
        {
            b = -1;
            r = -1;
            c = -1;
            x = 0;
            y = 0;
            z = 0;
        }
    }

    public class InteractiveData
    {

        

        public Vision.AutoFocusParam focusPara = new Vision.AutoFocusParam();
        public DieInfo dieInfo = new DieInfo(); //芯片信息：行、列、x位置、y位置、z位置、
        //图像回传量
        public Double z_focus;             //z轴聚焦位置
        public Double result_u;            //匹配结果像素u
        public Double result_v;            //匹配结果像素v
        public string answer;              //二维码识别出的字符串
        public int imgNum;                 //采图数
        public int image_index;            //采集第几张图像
        public void initial()
        {
            focusPara.focusRange.max = 0;
            focusPara.focusRange.min = 0;
            focusPara.initFocusPos = 0;
            focusPara.trgInterval = 0;
            dieInfo.initial();
            z_focus = 0;
            result_u = 0;
            result_v = 0;
            answer = "";
            imgNum = 0;
        }
    }

    class ProductMagzine : Base
    {
        /// <summary>
        /// 产品信息发生改变事件
        /// </summary>
        public event Action ProductInfoChangedEvent;

        public static String ActivePdt = String.Empty;  //当前产品 pdt1
        public static Int32 ImageNum = 1;

        private String _ProductFile; //目录 D:\PDT

        #region 料片字段 12个
        private Int32 _rowNumber = 0;
        private Int32 _columnNumber = 0;
        private Int32 _blockNumber = 0;
        private Double _rowSpace = 0;
        private Double _colomnSpace = 0;
        private Double _difcolomnSpace = 0;
        private Double _blockSpace = 0;
        private Double _topEdge = 0;        //正数
        private Double _rightEdge = 0;       //正数
        private Double _frameWidth = 0;
        private Int32 _framePicNumbM = 0;
        private Int32 _framePicNumbN = 0;
        private Double _frameLength = 0;
        //计算出的坐标
        private Double _xFirstDie = 0;
        private Double _yFirstDie = 0;

        private Double _deltaColumnBlock = 0;//Block间距等价于列数


        #endregion

        #region 料盒字段4个
      
        private Int32 _slotNumber = 0;
        private Int32 _blankNumber_Unload;
        private Double _heightFirstSlot = 0;
        private Double _heightFirstSlot_y = 0;
        private Double _heightLastSlot = 0;
        private Double _heightLastSlot_y = 0;
        private Double _heightFirstSlot_Unload = 0;
        private Double _heightFirstSlot_Unload_y = 0;
        private Double _heightLastSlot_Unload = 0;
        private Double _heightLastSlot_Unload_y = 0;
        private SQLiteConnection sqlCon;    //连接
        #endregion

        private int _equivalentRowNumber = 0;
        private int _equivalentColumnNumber = 0;
     
        private string filePath; //filePath:存储路径+文件名.tup

        IniFiles config = new IniFiles("D:\\Products" + "\\ActiveProd.ini");

        //public List<DieInfo> listCalibInit = new List<DieInfo>();          //一组标定点的行列与对应xyz值 
        //public List<DieInfo> listCalibUpdate = new List<DieInfo>();        //聚焦计算后标定点行列对应的xyz、dv、du的值
        public List<DieInfo> listRCXYZ = new List<DieInfo>();              //所有芯片行列对应的xyz值
        //public HTuple matUV2XY = new HTuple();                             //uv对应xy矩阵
        private HTuple matRC2XYZ = new HTuple();                            //rc对应xyz矩阵
        //public List<DieInfo> listCalibUV2XY = new List<DieInfo>();          //一组包含点位行列和对应定位u、v的值
        //public InteractiveData interactiveData = new InteractiveData();  //图像交互
        private double _zFocus;//不同产品的Z轴聚焦位
        private double _ref_zFocus;//不同产品的Z轴聚焦偏移
        private double x_Code2D;//不同产品的二维码位
        private double y_Code2D;//不同产品的二维码位
        private string magezineBox;//不同产品的料盒名
        private int mgzIdx=-1;//不同产品的料盒索引


        #region 料片属性
        [CategoryAttribute("料片属性"), DisplayNameAttribute("①料片长度（mm）")]
        public Double FrameLength
        {
            get { return _frameLength; }
            set { _frameLength = value; }
        }
        [CategoryAttribute("料片属性"), DisplayNameAttribute("②Block内行数目")]
        public Int32 RowNumber
        {
            get { return _rowNumber; }
            set
            {
                _rowNumber = value;
            }
        }

        [CategoryAttribute("料片属性"), DisplayNameAttribute("③Block内列数目")]
        public Int32 ColumnNumber
        {
            get { return _columnNumber; }
            set
            {
                _columnNumber = value;
            }
        }

        [CategoryAttribute("料片属性"), DisplayNameAttribute("④Block数目")]
        public Int32 BlockNumber
        {
            get { return _blockNumber; }
            set
            {
                _blockNumber = value;
            }
        }
        [CategoryAttribute("料片属性"), DisplayNameAttribute("⑤使用打标器")]
        public Boolean UseMarker { get; set; } 
        [CategoryAttribute("料片属性"), DisplayNameAttribute("⑥双芯片Block标记点x镜像")]
        public Boolean SymmetryMark { get; set; } 
        [CategoryAttribute("料片属性"), DisplayNameAttribute("⑦打标点相对X（mm）"), DescriptionAttribute("打标位置相对芯片位置,偏左为负值,X方向")]
        public Double RelativeMark_X { get; set; }
        [CategoryAttribute("料片属性"), DisplayNameAttribute("⑧打标点相对Y（mm）"), DescriptionAttribute("打标位置相对芯片位置,偏下为负值,Y方向")]
        public Double RelativeMark_Y { get; set; }
        [ReadOnly(true),CategoryAttribute("料片属性"), DisplayNameAttribute("⑨打标高度Z（mm）"), DescriptionAttribute("打标位置高度,相机Z轴,Z方向")]
        public Double RelativeMark_Z
        {
            get
            {
                return App.obj_Chuck.Ref_Mark_z;
                //return App.obj_Chuck.Ref_Mark_z+_zFocus-App.obj_Chuck.Mark_Focus_z;
            }
            set
            {
            }
        }
        //[CategoryAttribute("料片属性"), DisplayNameAttribute("④行间距（mm）")]
        //public Double RowSpace
        //{
        //    get { return _rowSpace; }
        //    set
        //    {
        //        _rowSpace = value;
        //    }
        //}

        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑤列间距（mm）")]
        //public Double ColomnSpace
        //{
        //    get { return _colomnSpace; }
        //    set
        //    {
        //        _colomnSpace = value;
        //    }
        //}


        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑥Block间距（mm）")]

        //public Double BlockSpace
        //{
        //    get { return _blockSpace; }
        //    set
        //    {
        //        _blockSpace = value;
        //    }
        //}

        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑦芯片与料片上边缘距离（mm）")]
        //public Double TopEdge
        //{
        //    get { return _topEdge; }
        //    set
        //    {
        //        _topEdge = value;
        //    }
        //}

        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑧芯片与料片右边缘的距离（mm）")]
        //public Double RightEdge
        //{
        //    get
        //    { return _rightEdge; }
        //    set
        //    { _rightEdge = value; }
        //}


        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑩Block中心差异列间距(无差异列则填列间距)（mm）")]

        //public Double DifColomnSpace
        //{
        //    get { return _difcolomnSpace; }
        //    set
        //    {
        //        _difcolomnSpace = value;
        //    }
        //}

        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑪料片拍照视野内芯片行数")]
        //public Int32 FramePicNumM { get { return _framePicNumbM; } set { _framePicNumbM = value; } }

        //[CategoryAttribute("料片属性"), DisplayNameAttribute("⑫料片拍照视野内芯片列数")]
        //public Int32 FramePicNumN { get { return _framePicNumbN; } set { _framePicNumbN = value; } }

        [CategoryAttribute("产品位置信息"), DisplayNameAttribute("①Z轴聚焦高度（mm）")]
        public double ZFocus
        {
            get { return App.obj_Chuck.Mark_Focus_z+ _ref_zFocus; }
            set { _ref_zFocus = value-App.obj_Chuck.Mark_Focus_z; }
        }
        [CategoryAttribute("产品位置信息"),ReadOnly(true), DisplayNameAttribute("①Z轴聚焦偏移（mm）")]
        public double RefZFocus
        {
            get { return _ref_zFocus; }
            set { _ref_zFocus = value; }
        }
        [CategoryAttribute("产品位置信息"), DisplayNameAttribute("②二维码X坐标（mm）")]
        public double X_Code2D
        {
            get { return x_Code2D; }
            set { x_Code2D = value; }
        }
        [CategoryAttribute("产品位置信息"), DisplayNameAttribute("③二维码Y坐标（mm）")]
        public double Y_Code2D
        {
            get { return y_Code2D; }
            set { y_Code2D = value; }
        }
        #endregion

        #region 料盒属性
        [ CategoryAttribute("料盒属性"), DisplayNameAttribute("①对应料盒"), TypeConverter(typeof(BoxManager))]
        public string MagezineBox
        {
            get
            {
                try
                {
                    return magezineBox = App.boxManager.Dir_Boxes[mgzIdx].Name;
                }
                catch
                {
                    mgzIdx = -1;
                    magezineBox = "";
                    return magezineBox;
                }
            }
            set
            {
                foreach(var pair in App.boxManager.Dir_Boxes)
                {
                    if (value == pair.Value.Name)
                    {
                        mgzIdx = pair.Key;
                        magezineBox = value;
                        return;
                    }
                }
                mgzIdx = -1;
                magezineBox = "";
            }
        }

        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("②料盒索引")]
        public Int32 MgzIdx
        {
            get
            {
                return mgzIdx;
            }
            set
            {
                mgzIdx = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("③上料最后槽上料位Z坐标（mm）")]
        public Double HeightLastSlot
        {
            get
            {
                try
                {
                    return  (mgzIdx!=-1? (_heightLastSlot = App.boxManager.Dir_Boxes[mgzIdx].HeightLastSlot): _heightLastSlot);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightLastSlot = value;
            }
        }
        

        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("③上料最后槽上料位Y坐标（mm）")]
        public Double HeightLastSlot_y
        {
            get
            {
                try
                {
                    return (mgzIdx != -1 ? (_heightLastSlot_y = App.boxManager.Dir_Boxes[mgzIdx].HeightLastSlot_y) : _heightLastSlot_y);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightLastSlot_y = value;
            }
        }
        [ReadOnly(true),CategoryAttribute("料盒属性"), DisplayNameAttribute("④上料第一槽上料位Z坐标（mm）")]
        public Double HeightFirstSlot
        {
            get
            {
                try
                {
                    if(mgzIdx != -1)App.obj_Load.z_LoadUnLoadFramePos = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot;
                    return  (mgzIdx != -1 ? (_heightFirstSlot = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot ): _heightFirstSlot);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightFirstSlot = value;
                App.obj_Load.z_LoadUnLoadFramePos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("④上料第一槽上料位Y坐标（mm）")]
        public Double HeightFirstSlot_y
        {
            get
            {
                try
                {
                    if (mgzIdx != -1) App.obj_Load.y_LoadUnLoadFramePos = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot_y;
                    return (mgzIdx != -1 ? (_heightFirstSlot_y = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot_y) : _heightFirstSlot_y);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightFirstSlot_y = value;
                App.obj_Load.y_LoadUnLoadFramePos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑤下料最后槽上料位Z坐标（mm）")]
        public Double HeightLastSlot_Unload
        {
            get
            {
                try
                {
                    return (mgzIdx != -1 ? (_heightLastSlot_Unload = App.boxManager.Dir_Boxes[mgzIdx].HeightLastSlot_Unload) : _heightLastSlot_Unload);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightLastSlot_Unload = value;
            }
        }

        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑤下料最后槽上料位Y坐标（mm）")]
        public Double HeightLastSlot_Unload_y
        {
            get
            {
                try
                {
                    return (mgzIdx != -1 ? (_heightLastSlot_Unload_y = App.boxManager.Dir_Boxes[mgzIdx].HeightLastSlot_Unload_y) : _heightLastSlot_Unload_y);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightLastSlot_Unload_y = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑥下料第一槽上料位Z坐标（mm）")]
        public Double HeightFirstSlot_Unload
        {
            get
            {
                try
                {
                    if (mgzIdx != -1) App.obj_Unload.z_LoadUnLoadFramePos = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot_Unload;
                    return (mgzIdx != -1 ? (_heightLastSlot_Unload = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot_Unload) : _heightFirstSlot_Unload);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightLastSlot_Unload = value;
                App.obj_Unload.z_LoadUnLoadFramePos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑥下料第一槽上料位Y坐标（mm）")]
        public Double HeightFirstSlot_Unload_y
        {
            get
            {
                try
                {
                    if (mgzIdx != -1) App.obj_Unload.y_LoadUnLoadFramePos = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot_Unload_y;
                    return (mgzIdx != -1 ? (_heightLastSlot_Unload_y = App.boxManager.Dir_Boxes[mgzIdx].HeightFirstSlot_Unload_y) : _heightFirstSlot_Unload_y);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _heightLastSlot_Unload_y = value;
                App.obj_Unload.y_LoadUnLoadFramePos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑦料盒内槽数目")]
        public int SlotNumber
        {

            get
            {
                try
                {
                    return (mgzIdx != -1 ? (_slotNumber = App.boxManager.Dir_Boxes[mgzIdx].SlotNumber): _slotNumber);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _slotNumber = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑧顺序下料间隔槽数目"), DescriptionAttribute("顺序下料间隔槽数目，同层时无效")]
        public int BlankNumber_Unload
        {

            get
            {
                try
                {
                    return (mgzIdx != -1 ? (_blankNumber_Unload = App.boxManager.Dir_Boxes[mgzIdx].BlankNumber_Unload) : _blankNumber_Unload);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                _blankNumber_Unload = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑨导轨Y轴位（mm）")]
        public Double FrameWidth
        {
            get { return (mgzIdx != -1 ? (_frameWidth = App.boxManager.Dir_Boxes[mgzIdx].FrameWidth ): _frameWidth); }
            set { _frameWidth = value; }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑩上料Y轴上片位(mm)"), DescriptionAttribute("上料料盒搬运机构Y轴,Y方向")]
        public Double Load_y_LoadUnLoadFramePos
        {
            get
            {
                return (mgzIdx != -1 ? (App.obj_Load.y_LoadUnLoadFramePos = App.boxManager.Dir_Boxes[mgzIdx].y_LoadFramePos ): App.obj_Load.y_LoadUnLoadFramePos);
            }
            set
            {
                App.obj_Load.y_LoadUnLoadFramePos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑫下料Y轴下片位(mm)"), DescriptionAttribute("下料料盒搬运机构Z轴,Y方向")]
        public Double UnLoad_y_LoadUnLoadFramePos
        {
            get
            {
                return (mgzIdx != -1 ? (App.obj_Unload.y_LoadUnLoadFramePos = App.boxManager.Dir_Boxes[mgzIdx].y_UnloadFramePos ): App.obj_Unload.y_LoadUnLoadFramePos);
            }
            set
            {
                App.obj_Unload.y_LoadUnLoadFramePos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑭上料推杆等待位(mm)"), DescriptionAttribute("上料推杆位置,X方向")]
        public Double Load_x_PushRodWaitPos
        {
            get { return (mgzIdx != -1 ? (App.obj_Load.x_PushRodWaitPos = App.boxManager.Dir_Boxes[mgzIdx].x_LoadPushRodWaitPos) : App.obj_Load.x_PushRodWaitPos); }
            set
            {
                App.obj_Load.x_PushRodWaitPos = value;
            }
        }
        [ReadOnly(true), CategoryAttribute("料盒属性"), DisplayNameAttribute("⑮上料推杆结束位(mm)"), DescriptionAttribute("上料推杆位置,X方向")]
        public Double Load_x_PushRodOverPos
        {
            get { return (mgzIdx != -1 ? (App.obj_Load.x_PushRodOverPos = App.boxManager.Dir_Boxes[mgzIdx].x_LoadPushRodOverPos) : App.obj_Load.x_PushRodOverPos); }
            set
            {
                App.obj_Load.x_PushRodOverPos = value;
            }
        }
        
        [CategoryAttribute("流程属性"), DisplayNameAttribute("①产品检测位(mm)"), DescriptionAttribute("载台左侧上料夹爪的位置, X方向")]
        public Double Load_x_ChuckLoadFramePos { get { return App.obj_Load.x_ChuckLoadFramePos; } set { App.obj_Load.x_ChuckLoadFramePos = value; } }
        [BrowsableAttribute(false)]
        public double XFirstDie
        {
            get { return _xFirstDie; }
            set
            {
                _xFirstDie = value;
            }
        }
        [BrowsableAttribute(false)]
        public double YFirstDie
        {
            get { return _yFirstDie; }
            set
            {
                _yFirstDie = value;
            }
        }
        [BrowsableAttribute(false)]
        public int EquivalentRowNumber
        {
            get { return _equivalentRowNumber; }
            set { _equivalentRowNumber = value; }
        }
        [BrowsableAttribute(false)]
        public int EquivalentColumnNumber
        {
            get { return _equivalentColumnNumber; }
            set { _equivalentColumnNumber = value; }
        }
        [BrowsableAttribute(false)]
        public Double DeltaColumnBlock
        {
            get { return _deltaColumnBlock; }
            set { _deltaColumnBlock = value; }
        }


        //[BrowsableAttribute(false)]
        //public static string ActivePdt
        //{
        //    get { return ActivePdt; }
        //    //set
        //    //{
        //    //    ActivePdt = value;
        //    //}
        //}
        #endregion
        //public static String ActivePdt { get { return ActivePdt; } }

        //public string ActivePdt { get => ActivePdt; }

        /// <summary>
        /// 静态构造函数在实例前读取，ActivePdt和imageNum
        /// </summary>


        public ProductMagzine(String para_file, String para_table) : base(para_file, para_table) { }

        /// <summary>
        /// 功能：配置真正的路径 获取db文件
        /// </summary>
        /// <param name="para_file"></param>
        public void ConfigParaFile(String para_file)
        {
            paraFile = para_file;
        }
        /// <summary>
        /// 加载产品数据
        /// </summary>
        public void LoadPdtData()
        {
            string pdt = ActivePdt;
            MgzIdx = -1;
            string db_Path = App.ProductDir + @"\" + ProductMagzine.ActivePdt + @"\ParaPdt.db";
            App.obj_Pdt.ConfigParaFile(db_Path);//初始化和载入时修正路径
            App.obj_light.ConfigParaFile(db_Path, "light");//初始化和载入时修正路径
            App.obj_Pdt.Read();
            App.obj_Process.LotId = "";
            //将料盒参数写入机构
            App.obj_Load.z_LoadUnLoadFirstPos = App.obj_Pdt.HeightFirstSlot;
            App.obj_Load.slotLayersCount = App.obj_Pdt.SlotNumber;
            App.obj_Load.heightSlotLayer = (App.obj_Pdt.HeightLastSlot - App.obj_Pdt.HeightFirstSlot) / (App.obj_Pdt.SlotNumber - 1);
            App.obj_Load.heightSlotLayer_y = (App.obj_Pdt.HeightLastSlot_y - App.obj_Pdt.HeightFirstSlot_y) / (App.obj_Pdt.SlotNumber - 1);
            App.obj_Load.FrameLength = App.obj_Pdt.FrameLength;
            App.obj_Unload.z_LoadUnLoadFirstPos = App.obj_Pdt.HeightFirstSlot_Unload;
            App.obj_Unload.slotLayersCount = App.obj_Pdt.SlotNumber;
            App.obj_Unload.heightSlotLayer = (App.obj_Pdt.HeightLastSlot_Unload - App.obj_Pdt.HeightFirstSlot_Unload) / (App.obj_Pdt.SlotNumber - 1);
            App.obj_Unload.heightSlotLayer_y = (App.obj_Pdt.HeightLastSlot_Unload_y - App.obj_Pdt.HeightFirstSlot_Unload_y) / (App.obj_Pdt.SlotNumber - 1);
            App.obj_Unload.FrameLength = App.obj_Pdt.FrameLength;

            LFAOIModel.FilePath.ProductDirectory = App.ProductPath;
            App.obj_light.Read();
            App.obj_Vision.TestImagePath = App.ProductDir + @"\" + ProductMagzine.ActivePdt + @"\image";

            int ImgNum = App.obj_ImageInformSet.Count;
            while (App.obj_ImageInformSet.Count > 1)
                App.obj_ImageInformSet.RemoveAt(App.obj_ImageInformSet.Count - 1);
            ReadImageNum();
            for (int i = 0; i < ImageNum; i++)
            {
                ImageInformation imageInform = new ImageInformation(App.ProductDir + @"\" + ActivePdt + @"\" + "Image.db", "Image" + i.ToString());
                if (!imageInform.Read())
                {
                    HTUi.PopError(imageInform.GetLastErrorString());
                    return;
                }
                App.obj_ImageInformSet.Add(imageInform);
            }
            if(ImgNum>0) App.obj_ImageInformSet.RemoveAt(0);
        }
        /// <summary>
        /// 将产品首条视觉配置写入设备
        /// </summary>
        public void ConfigPdtData()
        {
            if (App.obj_SystemConfig.SwichLineWidth != 0)
            {
                Task.Run(new Action(() =>
                {
                    Task.Run(new Action(() =>
                    {
                        if (!App.obj_Chuck.Fix_Track(App.obj_Pdt.FrameWidth))
                        {
                            HTLog.Error(App.obj_Chuck.GetLastErrorString());
                            return;
                        }
                        HTLog.Info("导轨宽度配置成功.");
                    }));
                    //switch (HTUi.PopSelect("请在以下选项中选择配置导轨方式。", "直接配置", "轴调试助手", "取消"))
                    //{
                    //    case 0:
                    //        Task.Run(new Action(() =>
                    //        {
                    //            if (!App.obj_Chuck.Fix_Track(App.obj_Pdt.FrameWidth))
                    //            {
                    //                HTLog.Error(App.obj_Chuck.GetLastErrorString());
                    //                return;
                    //            }
                    //            HTLog.Info("导轨宽度配置成功.");
                    //        }));
                    //        break;
                    //    case 1:
                    //        if (HTM.LoadUI() < 0)
                    //        {
                    //            HTUi.PopError("打开轴调试助手界面失败");
                    //        }
                    //        break;
                    //    default:
                    //        break;
                    //}
                }));
            }
            if (App.obj_Vision.RunMode != 1)
            {
                if (App.obj_ImageInformSet.Count != 0)
                {
                    frmCaptureImage.Instance.dgvWorkImagesInfo.Refresh();
                    int _selectIndex = 0;
                    ImageInformation.ConfigAllData(_selectIndex);
                }
            }
        }
        public void ProductInfoChanged()
        {
            if (App.obj_Pdt.ProductInfoChangedEvent != null)
            {
                App.obj_Pdt.ProductInfoChangedEvent();
            }
        }

        public override bool Save()
        {
            Boolean ret = true;
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String sql = "CREATE TABLE IF NOT EXISTS " + paraTable + "(Para TEXT PRIMARY KEY NOT NULL, Value TEXT NOT NULL)";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                PropertyInfo[] infos = this.GetType().GetProperties();//type.GetField
                foreach (PropertyInfo fi in infos)
                {
                    switch (fi.PropertyType.Name)
                    {
                        case "String":
                        case "Int32":
                        case "Boolean":
                        case "Double":
                            cmd.CommandText = String.Format("REPLACE INTO {0}(Para, Value) VALUES(@_Para, @_Value)", paraTable);//1234之类的？
                            cmd.Parameters.Add("_Para", System.Data.DbType.String).Value = fi.Name;
                            cmd.Parameters.Add("_Value", System.Data.DbType.Object).Value = fi.GetValue(this);
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                errCode = -1;
                errString = exp.ToString();
                sqlCon.Close();
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 读取参数，如果数据库中不含该参数则直接报错退出
        /// </summary>
        /// <returns>返回bool类型表示成功或失败，如果保存失败可以通过GetErrorString获取错误信息</returns>
        public override Boolean Read()
        {
            Boolean ret = true;
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand(sqlCon);
                SQLiteDataReader reader;
                PropertyInfo[] infos = this.GetType().GetProperties();
                foreach (PropertyInfo fi in infos)
                {
                    cmd.CommandText = String.Format("SELECT * FROM {0} WHERE [Para] = '{1}'", paraTable, fi.Name);//懂
                    reader = cmd.ExecuteReader();
                    if (!reader.HasRows)   //确保所有参数被赋值成功
                    {
                        //ret = false;
                        errString = String.Format("数据库中没有参数[{0}]", fi.Name);
                    }
                    else
                    {
                        reader.Read();
                        switch (fi.PropertyType.Name)
                        {
                            case "Int32":
                                fi.SetValue(this, Convert.ToInt32(reader["Value"]));
                                break;
                            case "Double":
                                fi.SetValue(this, Convert.ToDouble(reader["Value"]));
                                break;
                            case "Boolean":
                                fi.SetValue(this, Convert.ToBoolean(Convert.ToInt32(reader["Value"])));
                                break;
                            case "String":
                                fi.SetValue(this, Convert.ToString(reader["Value"]));
                                break;
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception exp)
            {
                errCode = -1;
                errString = exp.ToString();
                ret = false;
            }
            sqlCon.Close();
            return ret;
        }

        public Double CalSlotDistance()
        {
            double x = (_heightLastSlot - _heightFirstSlot) / (SlotNumber - 1);
            //return (_heightLastSlot - _heightFirstSlot) / (SlotNumber - 1);
            return (_heightFirstSlot - _heightLastSlot) / (SlotNumber - 1);

        }
        /// <summary>
        ///功能：产品的初始化（传入产品路径；获取当前产品；读取参数记录在实例化的成员变量内)
        ///时间：2017/8/7
        ///作者：杨小雨
        /// </summary>
        /// <param name="pdt_dir"></param>
        /// <returns></returns>
        public Boolean Initialize(String pdt_dir)
        {
            _ProductFile = pdt_dir;
            //return Read();
            return true;
        }

        /// <summary>
        /// 功能：获取ProductFile下的目录文件添加到一个list结构内
        /// 时间：2017/8/7
        /// 作者：杨小雨
        /// </summary>
        /// <returns>包含目录的指针list</returns>
        public List<String> GetProductList()
        {
            List<String> list = new List<string>();
            DirectoryInfo Path = new DirectoryInfo(_ProductFile);
            DirectoryInfo[] Dir = Path.GetDirectories();
            foreach (DirectoryInfo d in Dir)
            {
                list.Add(d.Name);
            }
            //read product directory to fetch the list
            return list;
        }

        /// <summary>
        /// 功能：复制现有产品文件以新建
        /// 作者：YXY
        /// </summary>
        /// <param name="souceFile">复制源文件夹</param>
        /// <param name="destinationFile">目标文件夹</param>
        /// <returns>true 创建成功；false 创建失败</returns>
        public Boolean CreateProduct(String souceFile, String destinationFile)
        {
            string soucePath = App.ProductDir + "\\" + souceFile;
            string destinationPath = App.ProductDir + "\\" + destinationFile;
            try
            {
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                //子文件夹
                foreach (string sub in Directory.GetDirectories(soucePath))
                {
                    CreateProduct(sub + "\\", destinationPath + Path.GetFileName(sub) + "\\");
                }
                //文件
                foreach (string file in Directory.GetFiles(soucePath))
                {
                    File.Copy(file, destinationPath + "\\" + Path.GetFileName(file), true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 功能：改变产品名
        /// </summary>
        /// <param name="destinationFile"></param>
        /// <param name="souceFile"></param>
        /// <returns></returns>
        public Boolean ChangeProdcutName(String destinationFile, String souceFile)
        {
            string soucePath = App.ProductDir + "\\" + souceFile;
            string destinationPath = App.ProductDir + "\\" + destinationFile;
            try
            {
                if (Directory.Exists(soucePath))
                {
                    Directory.Move(soucePath, destinationPath);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 功能：删除产品
        /// </summary>
        /// <param name="pdt_name">产品名</param>
        /// <returns></returns>
        public Boolean DeleteProduct(String pdt_name)
        {
            if (pdt_name == ActivePdt)
            {
                return false;
            }
            string path = App.ProductDir + "\\" + pdt_name;
            DirectoryInfo Directory = new DirectoryInfo(path);
            Directory.Delete(true);//所有
            return true;
        }

        /// <summary>
        /// 功能：将传入的参数设为当前产品，并写入activepdt.txt
        /// 时间：2017/8/7
        /// 作者：xiaoyu Yang
        /// </summary>
        /// <param name="pdt_name"></param>
        /// <returns></returns>
        public void SaveActivePdtName(String pdt_name)
        {
            if (config.WriteString("Product", "Name", pdt_name) == false)
            {
                throw new Exception("保存当前产品名称失败");
            }

            ActivePdt = pdt_name;

            //Boolean ret = false;
            //if (pdt_name == null) goto _end;
            //ActivePdt = pdt_name;
            //StreamWriter sw = new StreamWriter(_ProductFile + @"\activepdt.txt", false);
            //sw.Write(ActivePdt);
            //sw.Close();
            //ret = true;
            //_end:
            //return ret;
        }

        /// <summary>
        /// 功能：读取本地文件activepdt.txt，获取当前产品名,完善db实际路径
        /// 时间：2017/8/7
        /// 作者：xiaoyu Yang
        /// </summary>
        /// <returns></returns>
        public void ReadActivePdtName(out String active_pdt)
        {
            active_pdt = string.Empty;
            if (config.ReadString("Product", "Name", out active_pdt) == false)
            {
                throw new Exception("读取当前产品名称失败");

            }
            active_pdt= active_pdt.Replace("\0", "");
            ActivePdt = active_pdt;
         
            //Boolean ret = false;
            //active_pdt = String.Empty;
            //String _file = _ProductFile + "\\activepdt.txt";
            //if (!File.Exists(_file))
            //{
            //    HTLog.Error("产品目录下activepdt.txt文件不存在，请先创建!");
            //    goto _end;
            //}
            //StreamReader sr = new StreamReader(_file);
            //string tempPdt = sr.ReadToEnd();
            //sr.Close();
            //if (tempPdt == null) goto _end;//正常再赋值
            //active_pdt = tempPdt;
            //ActivePdt = active_pdt;
            //ret = true;
            //_end:
            //return ret;
        }

        public void ReadImageNum()
        {
            ImageNum = 1;
            if (config.ReadInteger("ImageNumber", "number", "D:\\Products\\" + ActivePdt + "\\ImageNumber.ini", out ImageNum) == false)
            {
                throw new Exception("读取当前产品图像数目信息失败");
            }
        }

        public void SaveImageNum()
        {
            
            if (config.WriteInteger("ImageNumber", "number", "D:\\Products\\" + ActivePdt + "\\ImageNumber.ini", ImageNum) == false)
            {
                throw new Exception("保存当前产品图像数目信息失败");
            }


        }


            /// <summary>
            /// 功能：调用基类中的Save()函数保存对象成员变量的值到数据库
            /// 时间：2017/8/7
            /// 作者：杨小雨
            /// </summary>
            /// <returns></returns>
            public Boolean SavePara()
        {
            Boolean ret = false;
            if (!Save())
            {
                HTLog.Error("产品参数保存失败！");
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        ///// <summary>
        ///// 获取RC2XY后,精确的计算单颗芯片视野中心的位置
        ///// </summary>
        ///// <param name="r">行坐标从零开始</param>
        ///// <param name="c">列坐标从零开始</param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void GetPosByRowCol(int r, int c, out Double x, out Double y)
        //{
        //    int k = c / _columnNumber;
        //    Double xDeltaBlock = 0;
        //    Double yDeltaBlock = 0;
        //    App.obj_Operations.CalculateBlockDistance(_deltaColumnBlock, out xDeltaBlock, out yDeltaBlock);
        //    //Two 
        //    x = (r - 0) * App.obj_Operations.RcHxy[0].D + (c - 0) * App.obj_Operations.RcHxy[1].D + App.obj_Operations.RcHxy[2].D + k * xDeltaBlock;
        //    y = (r - 0) * App.obj_Operations.RcHxy[3].D + (c - 0) * App.obj_Operations.RcHxy[4].D + App.obj_Operations.RcHxy[5].D + k * yDeltaBlock;
        //    //centerXaxisCoords = (((r+m-1) * _rcHxy[0].D + (c + n - 1) * _rcHxy[1].D + _rcHxy[2].D) + (r * _rcHxy[0].D + c * _rcHxy[1].D + _rcHxy[2].D)) /2.0;
        //    //centerYaxisCoords = (((r + m - 1) * _rcHxy[3].D + (c + n - 1) * _rcHxy[4].D + _rcHxy[5].D) + (r  * _rcHxy[3].D + c * _rcHxy[4].D + _rcHxy[5].D)) / 2.0;
        //    //x = (c / _columnNumber) * _blockSpace + (c % _columnNumber) * _colomnSpace + _rightEdge + App.obj_Chuck.ref_x;
        //    //y = -r * _rowSpace - _topEdge + 0.5 * _frameWidth + App.obj_Chuck.ref_y;
        //}

        ///// <summary>
        ///// 根据输入产品参数计算后得到的坐标，估计值用于标定，
        ///// </summary>
        ///// <param name="r">行左上角为0</param>
        ///// <param name="c">最上边为0</param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void GetPosByRowCol(int r, int c, Boolean calculate, out DieInfo die)
        //{
        //    int NumBlock = BlockNumber;//4;
        //    int NumC = ColumnNumber; //2;//每个Block多少列
        //    int NumR = RowNumber;//2;//8;//每个Block多少行
        //    int NumM = FramePicNumM;//一个视野多少行
        //    int NumN = FramePicNumN;//一个视野多少列
        //    double[] start = new double[2] { App.obj_Chuck.ref_x - (BlockNumber * BlockSpace - RightEdge * 2) / 2, App.obj_Chuck.ref_y + (RowNumber - 1) * RowSpace / 2 };//new double[2] { -182.184, -6.053 };//左上角
        //    double[] end = new double[2] { App.obj_Chuck.ref_x + (BlockNumber * BlockSpace - RightEdge * 2) / 2, App.obj_Chuck.ref_y - (RowNumber - 1) * RowSpace / 2 };//new double[2] { -2.183, -81.754 };//右下角
        //    double distanceBlockX = BlockSpace;//(end[0] - start[0]) / NumBlock;
        //    double distanceX = ColomnSpace;//distanceBlockX / ColumnNumber;
        //    double distanceY = RowSpace;//(end[1] - start[1]) / (NumM - 1);
        //    double difdistanceX = DifColomnSpace;

        //    die = new DieInfo();
        //    int equivalentCol = _columnNumber * _blockNumber - 1 - c;//mappimg的右上角为0,0位置，待定
        //    //centerXaxisCoords = (((r+m-1) * _rcHxy[0].D + (c + n - 1) * _rcHxy[1].D + _rcHxy[2].D) + (r * _rcHxy[0].D + c * _rcHxy[1].D + _rcHxy[2].D)) /2.0;
        //    //centerYaxisCoords = (((r + m - 1) * _rcHxy[3].D + (c + n - 1) * _rcHxy[4].D + _rcHxy[5].D) + (r  * _rcHxy[3].D + c * _rcHxy[4].D + _rcHxy[5].D)) / 2.0;
        //    if ((equivalentCol % _columnNumber) >= NumC / 2 && NumC % 2 == 0)//如果是单数列，并且当前列大于1/2列总数则考虑特殊列
        //    {
        //        die.x = start[0] + (equivalentCol / _columnNumber) * distanceBlockX + ((equivalentCol % _columnNumber) - 1) * distanceX + difdistanceX;
        //    }
        //    else
        //    {
        //        die.x = start[0] + (equivalentCol / _columnNumber) * distanceBlockX + (equivalentCol % _columnNumber) * distanceX;
        //    }
        //    die.y = start[1] - r * distanceY;
        //    die.r = r;
        //    die.c = c;
        //    //die = new DieInfo();
        //    //int equivalentCol = _columnNumber * _blockNumber - 1 - c;//mappimg的右上角为0,0位置，待定
        //    ////centerXaxisCoords = (((r+m-1) * _rcHxy[0].D + (c + n - 1) * _rcHxy[1].D + _rcHxy[2].D) + (r * _rcHxy[0].D + c * _rcHxy[1].D + _rcHxy[2].D)) /2.0;
        //    ////centerYaxisCoords = (((r + m - 1) * _rcHxy[3].D + (c + n - 1) * _rcHxy[4].D + _rcHxy[5].D) + (r  * _rcHxy[3].D + c * _rcHxy[4].D + _rcHxy[5].D)) / 2.0;
        //    //die.x = (equivalentCol / _columnNumber) * _blockSpace + (equivalentCol % _columnNumber) * _colomnSpace + _rightEdge;
        //    //die.y = -r * _rowSpace - _topEdge + 0.5 * _frameWidth;
        //    //die.r = r;
        //    //die.c = c;
        //}



        ///// <summary>
        ///// 通过rcHxy或者rcHxyz，从rc计算对应的xy by LPY
        ///// </summary>
        ///// <param name="mat">输入的转换矩阵，可以是rcHxy也可以是rcHxyz，程序会根据维数自己选取</param>
        ///// <param name="r">行，可以为小数</param>
        ///// <param name="c">列，可以为小数</param>
        ///// <param name="x">输出的x，mm</param>
        ///// <param name="y">输出的y，mm</param>
        ///// <returns>计算是否成功</returns>
        ///// <returns></returns>
        //public bool _rc2xy(HTuple mat, Double r, Double c, ref Double x, ref Double y)
        //{
        //    if (!(mat.Length == 6 || mat.Length == 9)) return false;
        //    Double[] H = mat.ToDArr();
        //    x = H[0] * r + H[1] * c + H[2];
        //    y = H[3] * r + H[4] * c + H[5];
        //    return true;
        //}

        ///// <summary>
        ///// 通过rcHxyz，从rc计算出对应的xyz by LPY
        ///// </summary>
        ///// <param name="mat">转换矩阵，1*9，对应3*3矩阵（行堆叠）</param>
        ///// <param name="r">行，可以为小数</param>
        ///// <param name="c">列，可以为小数</param>
        ///// <param name="x">x，mm</param>
        ///// <param name="y">y，mm</param>
        ///// <param name="z">z，mm</param>
        ///// <returns>计算是否成功</returns>
        //public bool _rc2xyz(HTuple mat, Double r, Double c, ref Double x, ref Double y, ref Double z)
        //{
        //    if (mat.Length != 9) return false;
        //    return (_rc2xy(mat, r, c, ref x, ref y) && _rc2z(mat, r, c, ref z));
        //}

        ///// <summary>
        ///// 通过rcHz或者rcHxyz，从rc计算出对应的z by LPY
        ///// </summary>
        ///// <param name="mat">输入的转换矩阵，可以是rcHz（1*3数组）也可以是rcHxyz（1*9）数组，程序会根据维数自己选取</param>
        ///// <param name="r">行，可以为小数</param>
        ///// <param name="c">列，可以为小数</param>
        ///// <param name="z">输出的z，mm</param>
        ///// <returns>计算是否成功</returns>
        //public bool _rc2z(HTuple mat, Double r, Double c, ref Double z)
        //{
        //    if (!(mat.Length == 3 || mat.Length == 9)) return false;
        //    Double[] H = mat.ToDArr();
        //    if (mat.Length == 3) { z = H[0] * r + H[1] * c + H[2]; };//mat = rcHz
        //    if (mat.Length == 9) { z = H[6] * r + H[7] * c + H[8]; };//mat = rcHxyz
        //    return true;
        //}

        ///// <summary>
        ///// 通过rc2xyz的关系矩阵得到产品所有芯片rc对应的xyz信息 by LPY
        ///// </summary>
        ///// <param name="row_count">产品行数</param>
        ///// <param name="column_count">产品列数</param>
        ///// <returns>返回类型为bool,表示成功或者失败</returns>
        //public bool CalRC2XYZ(int row_count, int column_count)
        //{
        //    listRCXYZ.Clear();
        //    //计算得到listRC2XYZ
        //    int r_index = 0; int c_index = 0;
        //    for (r_index = 0; r_index < row_count; r_index++)
        //    {
        //        if (r_index % 2 == 0)//偶数行
        //        {
        //            for (c_index = 0; c_index < column_count; c_index++)
        //            {
        //                DieInfo die = new DieInfo();
        //                die.initial();
        //                die.r = r_index;
        //                die.c = c_index;
        //                _rc2xyz(matRC2XYZ, r_index, c_index, ref die.x, ref die.y, ref die.z);
        //                listRCXYZ.Add(die);
        //            }
        //        }
        //        else
        //        {
        //            for (c_index = column_count - 1; c_index >= 0; c_index--)
        //            {
        //                DieInfo temp_die = new DieInfo();
        //                temp_die.r = r_index;
        //                temp_die.c = c_index;
        //                _rc2xyz(matRC2XYZ, r_index, c_index, ref temp_die.x, ref temp_die.y, ref temp_die.z);
        //                listRCXYZ.Add(temp_die);
        //            }
        //        }
        //    }
        //    return true;
        //}


        /// <summary>
        /// 获取RC2XY后精确计算得出点位
        /// </summary>
        /// <returns></returns>
        public Boolean GetListDie()
        {
            int j = 0;
            listRCXYZ.Clear();
            Double m = 1;
            Double n = 1;
            try
            {
                App.obj_Operations.CalcMN(out m, out n);
                _equivalentRowNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_rowNumber) / m));
                _equivalentColumnNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_columnNumber) / n));
                Double xDeltaBlock = 0;
                Double yDeltaBlock = 0;
                Double x = 0;
                Double y = 0;

                App.obj_Operations.CalculateBlockDistance(_deltaColumnBlock, out xDeltaBlock, out yDeltaBlock);

                for (int i = 0; i < _equivalentRowNumber; i++)
                {
                    if (i % 2 == 0)//偶数行
                    {
                        //for (j = 0; j < equivalentColumnNumber * _blockNumber; j++)
                        //for (int k = 0; k < BlockNumber; k++)
                        //{
                        //    for (j = 0; j < equivalentColumnNumber; j++)
                        for (j = 0; j < _equivalentColumnNumber * BlockNumber; j++)
                        {
                            //for (int k = 0; k < BlockNumber; k++)

                            //{
                            int k = j / _equivalentColumnNumber;
                            //GetPosByRowCol(r: i, c: j, equivalentColumnNumber: equivalentColumnNumber, equivalentTopEdge: equivalentTopEdge,
                            //    equivalentColumnSpace: equivalentColumnSpace,
                            //    equivalentRowSpace: equivalentRowSpace,
                            //    equivalentRightEdge: equivalentRightEdge, x: out x, y: out y);
                            //DieInfo _die = new DieInfo(i, j, x + App.obj_Chuck.ref_x, y + App.obj_Chuck.ref_y, App.obj_Pdt.ZFocus, 0, 0);
                            App.obj_Operations.DieCoords(r: i, c: j % _equivalentColumnNumber, xCenterDie: out x, yCenterDie: out y);
                            DieInfo _die = new DieInfo(i * Convert.ToInt32(m), (j % _equivalentColumnNumber * Convert.ToInt32(n)) % _columnNumber, x + k * xDeltaBlock, y + k * yDeltaBlock, App.obj_Chuck.z_Scan, 0, 0, k);
                            listRCXYZ.Add(_die);
                            //}
                        }
                    }
                    else
                    {
                        //for (int k = 0; k < BlockNumber; k++)
                        //{
                        for (j = _equivalentColumnNumber * BlockNumber - 1; j >= 0; j--)
                        {
                            int k = j / _equivalentColumnNumber;
                            App.obj_Operations.DieCoords(r: i, c: j % _equivalentColumnNumber, xCenterDie: out x, yCenterDie: out y);
                            DieInfo _die = new DieInfo(i * Convert.ToInt32(m), (j % _equivalentColumnNumber * Convert.ToInt32(n)) % _columnNumber, x + k * xDeltaBlock, y + k * yDeltaBlock, App.obj_Chuck.z_Scan, 0, 0, k);
                            listRCXYZ.Add(_die);
                        }
                    }
                }

                return true;
            }
            catch (Exception EXP)
            {
                errString = EXP.Message;
                return false;
            }
        }




    }
}