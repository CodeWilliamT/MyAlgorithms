using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ToolKits.FunctionModule;
using HalconDotNet;
using HTHalControl;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ToolKits.TemplateEdit
{
    public partial class MainTemplateForm : UserControl
    {
        /// <summary>
        /// 模板场景枚举
        /// </summary>
        public enum TemplateScence
        {
            /// <summary>
            /// 定位匹配模板
            /// </summary>
            Match = 0,
            /// <summary>
            /// 1D测量模板
            /// </summary>
            Measure1D = 1,
            /// <summary>
            /// 2D测量模板
            /// </summary>
            Measure2D = 2
        }
        /// <summary>
        /// 交互操作类型
        /// </summary>
        public class DrawType
        {
            public static string Point = "Point";
            public static string Line = "Line";
            public static string Rectangle1 = "Rectangle1";
            public static string Rectangle2 = "Rectangle2";
            public static string Circle = "Circle";
            public static string Ellipse = "Ellipse";
            public static string Region = "Region";
            public static string Xld = "Xld";
        }
        /// <summary>
        /// 声明Hobject类型的类【接受区/拒绝区】
        /// </summary>
        public class VaryHobject
        {
            /// <summary>
            /// Hobject
            /// </summary>
            public HObject obj;
            /// <summary>
            /// "接受区"/"拒绝区"
            /// </summary>
            public string type;
            /// <summary>
            /// 轮廓类型
            /// </summary>
            public string contType;
            public VaryHobject()
            {
                obj = new HObject();
                HOperatorSet.GenEmptyObj(out obj);
                type = "";
                contType = "";
            }
            public void Dispose()
            {
                obj.Dispose();
                type = "";
                contType = "";
            }
        }
        /// <summary>
        /// 定位模板参数类
        /// </summary>
        public class TemplateParam
        {
            private double angleStart;
            private double angleExtent;
            private int numLevel;
            private double score;
            private int type;
            private bool isCreateTmp;
            /// <summary>
            /// 模板的起始角度/°
            /// </summary>
            public double AngleStart
            {
                get { return this.angleStart; }
                set { this.angleStart = value; }
            }
            /// <summary>
            /// 模板的角度范围/°
            /// </summary>
            public double AngleExtent
            {
                get { return this.angleExtent; }
                set { this.angleExtent = value; }
            }
            /// <summary>
            /// 模板的金字塔层数
            /// </summary>
            public int NumLevel
            {
                get { return this.numLevel; }
                set { this.numLevel = value; }
            }
            /// <summary>
            /// 模板的匹配分数
            /// </summary>
            public double Score
            {
                get { return this.score; }
                set { this.score = value; }
            }
            /// <summary>
            /// 模板的类型【0表示原图Ncc;1表示原图Shape;2表示Shape Xld;3表示二值图Ncc】
            /// </summary>
            public int Type
            {
                get { return this.type; }
                set { this.type = value; }
            }
            /// <summary>
            /// 是否创建模板的标志
            /// </summary>
            public bool IsCreateTmp
            {
                get { return this.isCreateTmp; }
                set { this.isCreateTmp = value; }
            }
            /// <summary>
            /// 使用默认参数初始化
            /// </summary>
            public TemplateParam()
            {
                this.angleStart = -10;
                this.angleExtent = 20;
                this.numLevel = 0;
                this.score = 0.8;
                this.type = 0;
                this.isCreateTmp = true;
            }
            /// <summary>
            /// 使用指定参数初始化
            /// </summary>
            /// <param name="angleStart">起始角度，单位角度</param>
            /// <param name="angleExtent">终止角度，单位角度</param>
            /// <param name="numLevel">金字塔层数</param>
            /// <param name="score">匹配分数</param>
            /// <param name="type">模板类型</param>
            /// <param name="isCreateTmp">是否创建模板</param>
            public TemplateParam(double angleStart, double angleExtent, int numLevel, double score, int type, bool isCreateTmp)
            {
                this.angleStart = angleStart;
                this.angleExtent = angleExtent;
                this.numLevel = numLevel;
                this.score = score;
                this.type = type;
                this.isCreateTmp = isCreateTmp;
            }
        }
        /// <summary>
        /// 与模板信息相关的类（modelID,modelType,showContour,defRows,defCols）
        /// </summary>
        public class TemplateResult
        {
            /// <summary>
            /// 显示区
            /// </summary>
            public HObject showContour;
            /// <summary>
            /// 映射点行坐标
            /// </summary>
            public HTuple defRows;
            /// <summary>
            /// 映射点列坐标
            /// </summary>
            public HTuple defCols;
            /// <summary>
            /// 模板句柄
            /// </summary>
            public HTuple modelID;
            /// <summary>
            /// 模板类型
            /// </summary>
            public HTuple modelType;
            /// <summary>
            /// 是否成功创建模板
            /// </summary>
            public bool createTmpOK;
            public TemplateResult()
            {
                showContour = new HObject();
                HOperatorSet.GenEmptyObj(out showContour);
                defRows = new HTuple();
                defCols = new HTuple();
                modelID = new HTuple();
                modelType = new HTuple();
                createTmpOK = false;
            }
            /// <summary>
            /// 拷贝所有信息
            /// </summary>
            /// <param name="tmpResult"></param>
            /// <returns></returns>
            public bool Copy(TemplateResult tmpResult)
            {
                try
                {
                    if (tmpResult.showContour.IsInitialized())
                        showContour = tmpResult.showContour.CopyObj(1, -1);
                    defRows = tmpResult.defRows;
                    defCols = tmpResult.defCols;
                    modelType = tmpResult.modelType;
                    if (modelType.Length < 1) return false;
                    modelID = Vision.CopyModel(tmpResult.modelID, tmpResult.modelType);
                    createTmpOK = tmpResult.createTmpOK;

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            /// <summary>
            /// 释放模板的所有资源
            /// </summary>
            /// <returns></returns>
            public bool Dispose()
            {
                try
                {
                    showContour.Dispose();
                    defRows = new HTuple();
                    defCols = new HTuple();
                    HTuple iFlag = null;
                    Vision.clear_model(modelType, modelID, out iFlag);
                    modelType = new HTuple();
                    modelID = new HTuple();
                    if (iFlag.I != 0) return false;
                    createTmpOK = false;

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 2D测量参数
        /// </summary>
        [Serializable]
        public class Measure2DParam
        {
            /// <summary>
            /// 拟合个数
            /// </summary>
            private int instanceNums;
            /// <summary>
            /// 测量矩形个数
            /// </summary>
            private int measureNums;
            /// <summary>
            /// 边缘选择
            /// </summary>
            private string measureSelect;
            /// <summary>
            /// 边缘变化
            /// </summary>
            private string measureTransition;
            /// <summary>
            /// 滤波系数
            /// </summary>
            private double measureSigma;
            /// <summary>
            /// 匹配分数
            /// </summary>
            private double minScore;
            /// <summary>
            /// 2D测量矩形长轴一半
            /// </summary>
            private double measureLen1;
            /// <summary>
            /// 2D测量矩形短轴一半
            /// </summary>
            private double measureLen2;
            /// <summary>
            /// 边缘梯度阈值
            /// </summary>
            private double measureThreshold;
            /// <summary>
            /// 圆或椭圆的起始角度
            /// </summary>
            private double startAngle;
            /// <summary>
            /// 圆或椭圆的终止角度
            /// </summary>
            private double endAngle;
            /// <summary>
            /// 拟合个数
            /// </summary>
            public int InstanceNums
            {
                get { return this.instanceNums; }
                set { this.instanceNums = value; }
            }
            /// <summary>
            /// 测量矩形个数
            /// </summary>
            public int MeasureNums
            {
                get { return this.measureNums; }
                set { this.measureNums = value; }
            }
            /// <summary>
            /// 测量点选择
            /// </summary>
            public string MeausreSelect
            {
                get { return this.measureSelect; }
                set { this.measureSelect = value; }
            }
            /// <summary>
            /// 测量过渡带变化
            /// </summary>
            public string MeasureTransition
            {
                get { return this.measureTransition; }
                set { this.measureTransition = value; }
            }
            /// <summary>
            /// 滤波系数
            /// </summary>
            public double MeasureSigma
            {
                get { return this.measureSigma; }
                set { this.measureSigma = value; }
            }
            /// <summary>
            /// 匹配分数
            /// </summary>
            public double MinScore
            {
                get { return this.minScore; }
                set { this.minScore = value; }
            }
            /// <summary>
            /// 2D测量矩形长轴一半
            /// </summary>
            public double MeasureLen1
            {
                get { return this.measureLen1; }
                set { this.measureLen1 = value; }
            }
            /// <summary>
            /// 2D测量矩形短轴一半
            /// </summary>
            public double MeasureLen2
            {
                get { return this.measureLen2; }
                set { this.measureLen2 = value; }
            }
            /// <summary>
            /// 边缘梯度阈值
            /// </summary>
            public double MeasureThreshold
            {
                get { return this.measureThreshold; }
                set { this.measureThreshold = value; }
            }
            /// <summary>
            /// 圆或椭圆的起始角度
            /// </summary>
            public double StartAngle
            {
                get { return this.startAngle; }
                set { this.startAngle = value; }
            }
            /// <summary>
            /// 圆或椭圆的终止角度
            /// </summary>
            public double EndAngle
            {
                get { return this.endAngle; }
                set { this.endAngle = value; }
            }
            public Measure2DParam()
            {
                instanceNums = 1;
                measureNums = -1;
                measureSelect = "all";
                measureTransition = "all";
                measureSigma = 1.0;
                minScore = 0.5;
                measureLen1 = 20.0;
                measureLen2 = 5.0;
                measureThreshold = 30.0;
                startAngle = 0;
                endAngle = 360;
            }
            public Measure2DParam(int instanceNums, int measureNums, string measureSelect, string measureTransition,
                                  double measureSigma, double minScore, double measureLen1, double measureLen2,
                                  double measureThreshold, double startAngle, double endAngle)
            {
                this.instanceNums = instanceNums;
                this.measureNums = measureNums;
                this.measureSelect = measureSelect;
                this.measureTransition = measureTransition;
                this.measureSigma = measureSigma;
                this.minScore = minScore;
                this.measureLen1 = measureLen1;
                this.measureLen2 = measureLen2;
                this.measureThreshold = measureThreshold;
                this.startAngle = startAngle;
                this.endAngle = endAngle;
            }
            /// <summary>
            /// 深拷贝
            /// </summary>
            /// <returns></returns>
            public Measure2DParam DeepClone()
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as Measure2DParam;
            }
        }
        /// <summary>
        /// 2D测量结果信息
        /// </summary>
        public class Measure2DResult
        {
            public HTuple msrHandle;
            public HTuple distPoint1;
            public HTuple distPoint2;
            /// <summary>
            /// 测量区域类型集合【0:直线;1:矩形;2:圆;3:椭圆】
            /// </summary>
            public HTuple msrObjType;
            /// <summary>
            /// 2D测量是否完成
            /// </summary>
            public bool createMsr2DOK;
            public Measure2DResult()
            {
                msrHandle = new HTuple();
                distPoint1 = new HTuple();
                distPoint2 = new HTuple();
                msrObjType = new HTuple();
                createMsr2DOK = false;
            }
            public bool Copy(Measure2DResult msr2DResult)
            {
                try
                {
                    distPoint1 = msr2DResult.distPoint1;
                    distPoint2 = msr2DResult.distPoint2;
                    msrObjType = msr2DResult.msrObjType;
                    if (msr2DResult.msrHandle != null && msr2DResult.msrHandle.Length == 1)
                        HOperatorSet.CopyMetrologyModel(msr2DResult.msrHandle, "all", out msrHandle);
                    createMsr2DOK = msr2DResult.createMsr2DOK;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool Dispose()
            {
                try
                {
                    distPoint1 = new HTuple();
                    distPoint2 = new HTuple();
                    msrObjType = new HTuple();
                    if (msrHandle != null && msrHandle.Length == 1)
                        HOperatorSet.ClearMetrologyModel(msrHandle);
                    msrHandle = new HTuple();
                    createMsr2DOK = false;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public MainTemplateForm(TemplateScence tmpScence, HTWindowControl htWindow, object obj)
        {
            InitializeComponent();
            WorkOver = false;
            CheckForIllegalCrossThreadCalls = false;
            _instance = this;

            this.tmpScence = tmpScence;
            this.htWindow = htWindow;
            //没有图像资源
            if (this.htWindow.Image == null)
            {
                return;
            }
            if (!this.htWindow.Image.IsInitialized())
            {
                return;
            }
            if (!InitResources(tmpScence, obj))
            {
                return;
            }
            this.btnLast.Visible = false;
            UpdateForm(this.nodeFrm.Value);
        }
        static MainTemplateForm _instance;
        public static MainTemplateForm GetMainTemplateForm()
        {
            return _instance;
        }

        LinkedList<BaseForm> llFrm;
        LinkedListNode<BaseForm> nodeFrm;
        TemplateScence tmpScence;
        public HTWindowControl htWindow;
        public Dictionary<string, Image> drawTypeImg;
        public string[] acceptMode = { "接受区", "拒绝区" };

        public string[] tmpMode = { "原图Ncc", "原图Shape", "Shape Xld", "二值图Ncc" };
        /// <summary>
        /// 模板区域
        /// </summary>
        public HObject modelRegion;
        /// <summary>
        /// 匹配区
        /// </summary>
        public HObject matchRegion;
        /// <summary>
        /// 拒绝区
        /// </summary>
        public HObject rejectRegion;
        /// <summary>
        /// 控件是否完成工作
        /// </summary>
        public bool WorkOver;

        #region 创建模板窗口
        /// <summary>
        /// 模板参数窗口
        /// </summary>
        BaseForm mte_TmpPrmFrm;
        /// <summary>
        /// 图像旋转窗口
        /// </summary>
        BaseForm mte_rotateFrm;
        /// <summary>
        /// 画前景区窗口（为二值图Ncc服务）
        /// </summary>
        BaseForm mte_foreGrdFrm;
        /// <summary>
        /// 画模板区窗口
        /// </summary>
        BaseForm mte_mdlRegFrm;
        /// <summary>
        /// 画模板去窗口
        /// </summary>
        BaseForm mte_shwContFrm;
        /// <summary>
        /// 画映射点窗口
        /// </summary>
        BaseForm mte_defPointFrm;
        /// <summary>
        /// 画匹配区窗口
        /// </summary>
        BaseForm mte_mtchRegFrm;
        /// <summary>
        /// 画拒绝区窗口
        /// </summary>
        BaseForm mte_rejRegFrm;
        #endregion

        #region 测量窗口
        /// <summary>
        /// 画测量区域面板
        /// </summary>
        BaseForm mte_Msr2DObjFrm;
        /// <summary>
        /// 设置距离区面板
        /// </summary>
        BaseForm mte_Msr2DDistPtFrm;
        #endregion

        #region 数据类型
        #region 模板数据类型
        /// <summary>
        /// 创建模板的数据
        /// </summary>
        public TemplateParam mte_TmpPrmValues;
        /// <summary>
        /// 画前景区的数据
        /// </summary>
        public List<VaryHobject> mte_fGrdRegValues;
        /// <summary>
        /// 画模板区的数据
        /// </summary>
        public List<VaryHobject> mte_mdlRegValues;
        /// <summary>
        /// 画显示区的数据
        /// </summary>
        public List<HObject> mte_shwContValues;
        /// <summary>
        /// 画映射点的数据
        /// </summary>
        public List<HTuple> mte_defPointValues;
        /// <summary>
        /// 画匹配区的数据
        /// </summary>
        public List<VaryHobject> mte_mtchRegValues;
        /// <summary>
        /// 画拒绝区的数据
        /// </summary>
        public List<VaryHobject> mte_rejRegValues;
        /// <summary>
        /// 创建模板的结果
        /// </summary>
        public TemplateResult tmpResult;
        #endregion

        #region 2D测量数据类型
        /// <summary>
        /// 2D测量区域信息
        /// </summary>
        public List<VaryHobject> mte_Msr2DRegValues;
        /// <summary>
        /// 测距第一点索引集合
        /// </summary>
        public HTuple mte_Msr2DDistPt1;
        /// <summary>
        /// 测距第二点索引集合
        /// </summary>
        public HTuple mte_Msr2DDistPt2;
        /// <summary>
        /// 2D测量参数
        /// </summary>
        public List<Measure2DParam> mte_Msr2DPrmValues;
        /// <summary>
        /// 2D测量结果
        /// </summary>
        public Measure2DResult msr2DResult;
        #endregion
        #endregion

        #region 窗体事件
        //上一步
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (this.nodeFrm == null) return;
            if (this.nodeFrm.Equals(this.llFrm.Last))
                this.btnNext.Text = "下一步";
            this.nodeFrm = this.nodeFrm.Previous;
            UpdateForm(this.nodeFrm.Value);
            if (this.nodeFrm.Equals(this.llFrm.First))
                this.btnLast.Visible = false;
        }
        //下一步
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.nodeFrm == null) return;
            if (this.nodeFrm.Equals(this.llFrm.Last))
            {
                //单击完成按钮时，开始进行创建模板
                ToolKits.UI.DialogUI.Loading loadFrm = new UI.DialogUI.Loading();
                loadFrm.Text = "正在创建模板，请稍等...";
                loadFrm.Shown += loadFrm_Shown;
                loadFrm.ShowDialog();
                WorkOver = true;
                return;
            }
            this.btnLast.Visible = true;
            this.nodeFrm = this.nodeFrm.Next;
            #region 首次加载窗口时
            InitForms(this.tmpScence, this.nodeFrm);
            #endregion
            UpdateForm(this.nodeFrm.Value);
            if (this.nodeFrm.Equals(this.llFrm.Last))
                this.btnNext.Text = "完成";
        }
        void loadFrm_Shown(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Form frm = (Form)sender;
            //this.Enabled = false;
            Task<bool> load = Task.Factory.StartNew<bool>(() =>
                {
                    return this.CreateResult(this.tmpScence);
                });
            Task task = load.ContinueWith((tResult) =>
                {

                    frm.Close();
                    if (!tResult.Result)
                    {
                        string err = "";
                        switch (this.tmpScence)
                        {
                            case TemplateScence.Match:
                                err = "创建模板失败！";
                                break;
                            case TemplateScence.Measure1D:
                                err = "创建1D测量模板信息失败！";
                                break;
                            case TemplateScence.Measure2D:
                                err = "创建2D测量模板信息失败！";
                                break;
                            default:
                                break;
                        }
                        MessageBox.Show(err, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //this.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region 方法
        private bool CreateResult(TemplateScence tmpScence)
        {
            switch (tmpScence)
            {
                case TemplateScence.Match:
                    return CreateTemplate();
                case TemplateScence.Measure1D:
                    return false;
                case TemplateScence.Measure2D:
                    return CreateMeasure2DModel();
                default:
                    return false;
            }
        }
        /// <summary>
        /// 初始化缓存变量与首个窗体资源
        /// </summary>
        /// <param name="tmpScence"></param>
        /// <returns></returns>
        private bool InitResources(TemplateScence tmpScence, object obj)
        {
            switch (tmpScence)
            {
                case TemplateScence.Match:
                    mte_TmpPrmValues = (TemplateParam)obj;
                    mte_fGrdRegValues = new List<VaryHobject>();
                    mte_mdlRegValues = new List<VaryHobject>();
                    mte_shwContValues = new List<HObject>();
                    mte_defPointValues = new List<HTuple>();
                    mte_mtchRegValues = new List<VaryHobject>();
                    mte_rejRegValues = new List<VaryHobject>();
                    tmpResult = new TemplateResult();

                    llFrm = new LinkedList<BaseForm>();
                    mte_TmpPrmFrm = new MatchTmpEdit.TmpPrmForm();
                    this.nodeFrm = new LinkedListNode<BaseForm>(mte_TmpPrmFrm);
                    llFrm.AddFirst(this.nodeFrm);
                    llFrm.AddLast(mte_rotateFrm);
                    llFrm.AddLast(mte_foreGrdFrm);
                    llFrm.AddLast(mte_mdlRegFrm);
                    llFrm.AddLast(mte_shwContFrm);
                    llFrm.AddLast(mte_defPointFrm);
                    llFrm.AddLast(mte_mtchRegFrm);
                    llFrm.AddLast(mte_rejRegFrm);
                    break;
                case TemplateScence.Measure1D:
                    break;
                case TemplateScence.Measure2D:
                    mte_Msr2DRegValues = (List<VaryHobject>)obj;
                    mte_Msr2DDistPt1 = new HTuple();
                    mte_Msr2DDistPt2 = new HTuple();
                    mte_Msr2DPrmValues = new List<Measure2DParam>();
                    msr2DResult = new Measure2DResult();

                    llFrm = new LinkedList<BaseForm>();
                    mte_Msr2DObjFrm = new Measure2DTmpEdit.Measure2DObjectsForm();
                    this.nodeFrm = new LinkedListNode<BaseForm>(mte_Msr2DObjFrm);
                    llFrm.AddFirst(this.nodeFrm);
                    llFrm.AddLast(mte_Msr2DDistPtFrm);
                    break;
                default:
                    MessageBox.Show("未知的创建类型！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }
            #region 添加图像资源
            drawTypeImg = new Dictionary<string, Image>();
            drawTypeImg.Add(DrawType.Point, ToolKitsDll.Properties.Resources.point);
            drawTypeImg.Add(DrawType.Line, ToolKitsDll.Properties.Resources.line);
            drawTypeImg.Add(DrawType.Rectangle1, ToolKitsDll.Properties.Resources.rect1);
            drawTypeImg.Add(DrawType.Rectangle2, ToolKitsDll.Properties.Resources.rect2);
            drawTypeImg.Add(DrawType.Circle, ToolKitsDll.Properties.Resources.circle);
            drawTypeImg.Add(DrawType.Ellipse, ToolKitsDll.Properties.Resources.ellipse);
            drawTypeImg.Add(DrawType.Region, ToolKitsDll.Properties.Resources.region);
            drawTypeImg.Add(DrawType.Xld, ToolKitsDll.Properties.Resources.xld);
            #endregion
            return true;
        }
        /// <summary>
        /// 首次加载窗体
        /// </summary>
        /// <param name="tmpScence"></param>
        /// <param name="nodeFrm"></param>
        /// <returns></returns>
        private bool InitForms(TemplateScence tmpScence, LinkedListNode<BaseForm> nodeFrm)
        {
            try
            {
                if (nodeFrm.Value == null)
                {
                    switch (tmpScence)
                    {
                        case TemplateScence.Match:
                            if (nodeFrm.Previous.Value.Equals(mte_TmpPrmFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.RotateForm();
                                mte_rotateFrm = nodeFrm.Value;
                            }
                            else if (nodeFrm.Previous.Value.Equals(mte_rotateFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.ForeGroundForm();
                                mte_foreGrdFrm = nodeFrm.Value;
                            }
                            else if (nodeFrm.Previous.Value.Equals(mte_foreGrdFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.ModelRegionForm();
                                mte_mdlRegFrm = nodeFrm.Value;
                            }
                            else if (nodeFrm.Previous.Value.Equals(mte_mdlRegFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.ShowContourForm();
                                mte_shwContFrm = nodeFrm.Value;
                            }
                            else if (nodeFrm.Previous.Value.Equals(mte_shwContFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.DefPointForm();
                                mte_defPointFrm = nodeFrm.Value;
                            }
                            else if (nodeFrm.Previous.Value.Equals(mte_defPointFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.MatchRegionForm();
                                mte_mtchRegFrm = nodeFrm.Value;
                            }
                            else if (nodeFrm.Previous.Value.Equals(mte_mtchRegFrm))
                            {
                                nodeFrm.Value = new MatchTmpEdit.RejectRegionForm();
                                mte_rejRegFrm = nodeFrm.Value;
                            }
                            else
                            {
                                MessageBox.Show("本级窗口类型未知！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            break;
                        case TemplateScence.Measure1D:
                            return false;
                        case TemplateScence.Measure2D:
                            if (nodeFrm.Previous.Value.Equals(mte_Msr2DObjFrm))
                            {
                                nodeFrm.Value = new Measure2DTmpEdit.Measure2DDistPointForm();
                                mte_Msr2DDistPtFrm = nodeFrm.Value;
                            }
                            else
                            {
                                MessageBox.Show("本级窗口类型未知！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            break;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 更新窗口
        /// </summary>
        /// <param name="baseForm"></param>
        private void UpdateForm(BaseForm baseForm)
        {
            this.splitContainer1.Panel1.Controls.Clear();
            this.splitContainer1.Panel1.Controls.Add(baseForm);
            this.nodeFrm.Value.Dock = DockStyle.Fill;
            //更新数据使用
            baseForm.Visible = false;
            baseForm.Visible = true;
        }
        /// <summary>
        /// 释放所有占用资源
        /// </summary>
        public new void Dispose()
        {
            switch (this.tmpScence)
            {
                case TemplateScence.Match:
                    if (this.mte_fGrdRegValues != null)
                    {
                        for (int i = 0; i < this.mte_fGrdRegValues.Count; i++)
                            this.mte_fGrdRegValues[i].Dispose();
                        this.mte_fGrdRegValues.Clear();
                    }
                    if (this.mte_mdlRegValues != null)
                    {
                        for (int i = 0; i < this.mte_mdlRegValues.Count; i++)
                            this.mte_mdlRegValues[i].Dispose();
                        this.mte_mdlRegValues.Clear();
                    }
                    if (this.mte_shwContValues != null)
                    {
                        for (int i = 0; i < this.mte_shwContValues.Count; i++)
                            this.mte_shwContValues[i].Dispose();
                        this.mte_shwContValues.Clear();
                    }
                    if (this.mte_defPointValues != null)
                    {
                        this.mte_shwContValues.Clear();
                    }
                    if (this.mte_mtchRegValues != null)
                    {
                        for (int i = 0; i < this.mte_mtchRegValues.Count; i++)
                            this.mte_mtchRegValues[i].Dispose();
                        this.mte_mtchRegValues.Clear();
                    }
                    if (this.mte_rejRegValues != null)
                    {
                        for (int i = 0; i < this.mte_rejRegValues.Count; i++)
                            this.mte_rejRegValues[i].Dispose();
                        this.mte_rejRegValues.Clear();
                    }
                    if (this.tmpResult != null) this.tmpResult.Dispose();
                    if (this.modelRegion != null) this.modelRegion.Dispose();
                    if (this.matchRegion != null) this.matchRegion.Dispose();
                    if (this.rejectRegion != null) this.rejectRegion.Dispose();

                    if (this.mte_TmpPrmFrm != null) this.mte_TmpPrmFrm.Dispose();
                    if (this.mte_rotateFrm != null) this.mte_rotateFrm.Dispose();
                    if (this.mte_foreGrdFrm != null) this.mte_foreGrdFrm.Dispose();
                    if (this.mte_mdlRegFrm != null) this.mte_mdlRegFrm.Dispose();
                    if (this.mte_shwContFrm != null) this.mte_shwContFrm.Dispose();
                    if (this.mte_defPointFrm != null) this.mte_defPointFrm.Dispose();
                    if (this.mte_mtchRegFrm != null) this.mte_mtchRegFrm.Dispose();
                    if (this.mte_rejRegFrm != null) this.mte_rejRegFrm.Dispose();
                    break;
                case TemplateScence.Measure1D:
                    break;
                case TemplateScence.Measure2D:
                    if (this.mte_Msr2DRegValues != null)
                    {
                        for (int i = 0; i < this.mte_Msr2DRegValues.Count; i++)
                        {
                            this.mte_Msr2DRegValues[i].Dispose();
                        }
                        this.mte_Msr2DRegValues.Clear();
                    }
                    this.mte_Msr2DDistPt1 = null;
                    this.mte_Msr2DDistPt2 = null;
                    this.msr2DResult = null;
                    if (this.mte_Msr2DPrmValues != null) this.mte_Msr2DPrmValues.Clear();

                    if (this.mte_Msr2DObjFrm != null) this.mte_Msr2DObjFrm.Dispose();
                    if (this.mte_Msr2DDistPtFrm != null) this.mte_Msr2DObjFrm.Dispose();
                    break;
            }
            if (llFrm != null)
            {
                foreach (var item in llFrm)
                {
                    if (item != null) item.Dispose();
                }
                llFrm.Clear();
            }
            base.Dispose();
        }
        /// <summary>
        /// 收集接受区与拒绝区的差
        /// </summary>
        /// <param name="values"></param>
        /// <param name="region"></param>
        private string AllocAcceptRejRegion(List<VaryHobject> values, ref HObject region)
        {
            string msg = "";
            if (values.Count == 0) return msg;
            HObject accept_region = new HObject(), reject_region = new HObject();
            try
            {
                HOperatorSet.GenEmptyObj(out accept_region);
                HOperatorSet.GenEmptyObj(out reject_region);
                for (int i = 0; i < values.Count; i++)
                {
                    HObject local_region = new HObject();
                    HOperatorSet.GenEmptyObj(out local_region);
                    local_region.Dispose();
                    HOperatorSet.GenRegionContourXld(values[i].obj, out local_region, "filled");
                    //接受区
                    if (values[i].type == acceptMode[0])
                        HOperatorSet.Union2(accept_region, local_region, out accept_region);
                    //拒绝区
                    else HOperatorSet.Union2(reject_region, local_region, out reject_region);
                }
                region.Dispose();
                HOperatorSet.Difference(accept_region, reject_region, out region);
                accept_region.Dispose();
                reject_region.Dispose();

                return msg;
            }
            catch (Exception ex)
            {
                accept_region.Dispose();
                reject_region.Dispose();
                region.Dispose();
                return ex.ToString();
            }
        }
        /// <summary>
        /// 创建模板主函数
        /// </summary>
        /// <returns></returns>
        private bool CreateTemplate()
        {
            HObject fore_region = null;
            try
            {
                tmpResult.Dispose();

                #region 收集匹配区信息，集中在match_region
                if (matchRegion == null)
                {
                    matchRegion = new HObject();
                    HOperatorSet.GenEmptyObj(out matchRegion);
                }
                matchRegion.Dispose();
                string match_msg = AllocAcceptRejRegion(mte_mtchRegValues, ref matchRegion);
                if (match_msg != "")
                {
                    matchRegion.Dispose();
                    MessageBox.Show("收集匹配区信息失败！\r\n详细错误信息：" + match_msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                #region 收集拒绝区信息，集中在reject_region
                if (rejectRegion == null)
                {
                    rejectRegion = new HObject();
                    HOperatorSet.GenEmptyObj(out rejectRegion);
                }
                rejectRegion.Dispose();
                string reject_msg = AllocAcceptRejRegion(mte_rejRegValues, ref rejectRegion);
                if (reject_msg != "")
                {
                    rejectRegion.Dispose();
                    MessageBox.Show("收集拒绝区信息失败！\r\n详细错误信息：" + reject_msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                #region 如果不需要创建模板，则收集匹配区和拒绝区。然后函数退出
                if (!mte_TmpPrmValues.IsCreateTmp)
                    return true;
                #endregion

                #region 二次处理模板区域，如果模板是Shape Xld，则模板区不能出现拒绝区类型，否则报错，修改
                if (mte_TmpPrmValues.Type == 2)
                {
                    for (int i = 0; i < mte_mdlRegValues.Count; i++)
                    {
                        if (mte_mdlRegValues[i].type == acceptMode[1])
                        {
                            MessageBox.Show("当前创建模板类型不支持在模板区出现拒绝区类型！请返回修改。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                #endregion

                #region 如果模板类型为二值图Ncc，则需要收集前景区信息，集中在fore_region
                fore_region = new HObject();
                HOperatorSet.GenEmptyObj(out fore_region);
                fore_region.Dispose();
                if (mte_TmpPrmValues.Type == 3)
                {
                    string fore_msg = AllocAcceptRejRegion(mte_fGrdRegValues, ref fore_region);
                    if (fore_msg != "")
                    {
                        fore_region.Dispose();
                        MessageBox.Show("收集前景区信息失败！\r\n详细错误信息：" + fore_msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                #endregion

                #region 收集模板区信息，集中在model_region
                if (mte_mdlRegValues.Count == 0)
                {
                    MessageBox.Show("模板区不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                modelRegion = new HObject();
                HObject accept_model_region = new HObject(), reject_model_region = new HObject();
                try
                {
                    HOperatorSet.GenEmptyObj(out accept_model_region);
                    HOperatorSet.GenEmptyObj(out reject_model_region);
                    HOperatorSet.GenEmptyObj(out modelRegion);
                    for (int i = 0; i < mte_mdlRegValues.Count; i++)
                    {
                        //如果为Shape Xld,则只会出现接受区，且都为轮廓
                        if (mte_TmpPrmValues.Type == 2)
                            HOperatorSet.ConcatObj(modelRegion, mte_mdlRegValues[i].obj.CopyObj(1, -1), out modelRegion);
                        else
                        {
                            HObject local_region = new HObject();
                            HOperatorSet.GenEmptyObj(out local_region);
                            local_region.Dispose();
                            HOperatorSet.GenRegionContourXld(mte_mdlRegValues[i].obj, out local_region, "filled");
                            //接受区
                            if (mte_mdlRegValues[i].type == acceptMode[0])
                                HOperatorSet.Union2(accept_model_region, local_region, out accept_model_region);
                            //拒绝区
                            else HOperatorSet.Union2(reject_model_region, local_region, out reject_model_region);
                        }
                    }

                    if (mte_TmpPrmValues.Type != 2)
                    {
                        modelRegion.Dispose();
                        HOperatorSet.Difference(accept_model_region, reject_model_region, out modelRegion);
                    }
                    accept_model_region.Dispose();
                    reject_model_region.Dispose();
                }
                catch (Exception ex)
                {
                    accept_model_region.Dispose();
                    reject_model_region.Dispose();
                    modelRegion.Dispose();
                    MessageBox.Show("收集模板区信息失败！\r\n详细错误信息：" + ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                #region 收集显示区信息，集中在showContour
                if (mte_shwContValues.Count == 0)
                {
                    MessageBox.Show("显示区不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                HOperatorSet.GenEmptyObj(out tmpResult.showContour);
                for (int i = 0; i < mte_shwContValues.Count; i++)
                    HOperatorSet.ConcatObj(tmpResult.showContour, mte_shwContValues[i].CopyObj(1, -1), out tmpResult.showContour);
                #endregion

                #region 收集映射点信息，集中在defRows,defCols
                if (mte_defPointValues.Count == 0)
                {
                    MessageBox.Show("映射点不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                for (int i = 0; i < mte_defPointValues.Count; i++)
                {
                    tmpResult.defRows.Append(mte_defPointValues[i][0]);
                    tmpResult.defCols.Append(mte_defPointValues[i][1]);
                }
                #endregion

                #region 开始创建模板
                //使用通用模板创建函数（通用模板函数包括模板创建，模板匹配，模板保存，模板加载）
                if (mte_TmpPrmValues.Type == 0 || mte_TmpPrmValues.Type == 3)
                    tmpResult.modelType = 0;
                else tmpResult.modelType = 1;

                HTuple iFlag;
                HTuple numLevel = null;
                if (mte_TmpPrmValues.NumLevel == 0)
                    numLevel = "auto";
                else numLevel = mte_TmpPrmValues.NumLevel;
                HObject byteImg=new HObject();
                byteImg.Dispose();
                HOperatorSet.ConvertImageType(this.htWindow.Image, out byteImg, "byte");
                HTuple channel;
                HOperatorSet.CountChannels(byteImg, out channel);
                if (channel != 1)
                    HOperatorSet.Rgb1ToGray(byteImg, out byteImg);
                Vision.create_model(byteImg, modelRegion, fore_region, mte_TmpPrmValues.AngleStart,
                                    mte_TmpPrmValues.AngleExtent, numLevel, mte_TmpPrmValues.Type, out tmpResult.modelID, out iFlag);
                byteImg.Dispose();
                fore_region.Dispose();

                if (iFlag.I != 0)
                {
                    tmpResult.Dispose();
                    MessageBox.Show("创建模板失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                tmpResult.createTmpOK = true;

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                if (fore_region != null) fore_region.Dispose();
                tmpResult.Dispose();
                MessageBox.Show("创建模板失败！\r\n详细错误信息：" + ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// 创建2D测量模板主函数
        /// </summary>
        /// <returns></returns>
        private bool CreateMeasure2DModel()
        {
            #region 局部变量定义
            HTuple width, height;
            HTuple row, col, row1, col1, row2, col2, phi, len1, len2, radius1, radius2, startPhi, endPhi, pointOrder, nr, nc, dist;
            //HTuple lineIndices =new HTuple(),rectMsrIndices = new HTuple(), circleMsrIndices = new HTuple(), ellipseMsrIndices = new HTuple();
            HTuple index;
            HObject fitContours = new HObject();
            HObject conts = new HObject();
            HObject cross = new HObject();
            HOperatorSet.GenEmptyObj(out fitContours);
            HOperatorSet.GenEmptyObj(out conts);
            HOperatorSet.GenEmptyObj(out cross);
            #endregion
            try
            {
                msr2DResult.Dispose();

                if (this.mte_Msr2DRegValues.Count != this.mte_Msr2DPrmValues.Count)
                {
                    MessageBox.Show("参数个数与测量区域个数不匹配！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (this.mte_Msr2DDistPt1.TupleFind(-1) > -1 || this.mte_Msr2DDistPt2.TupleFind(-1) > -1)
                {
                    MessageBox.Show("测距点索引不能包含小于零！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                HOperatorSet.CreateMetrologyModel(out msr2DResult.msrHandle);
                HOperatorSet.GetImageSize(this.htWindow.Image, out width, out height);
                HOperatorSet.SetMetrologyModelImageSize(msr2DResult.msrHandle, width, height);
                int len = this.mte_Msr2DRegValues.Count;

                #region 创建测量模型索引
                for (int i = 0; i < len; i++)
                {
                    switch (this.mte_Msr2DRegValues[i].contType)
                    {
                        case "Line":
                            HOperatorSet.FitLineContourXld(this.mte_Msr2DRegValues[i].obj, "tukey", -1, 0, 5, 2.0,
                                                           out row1, out col1, out row2, out col2, out nr, out nc, out dist);
                            HOperatorSet.AddMetrologyObjectLineMeasure(msr2DResult.msrHandle, row1, col1, row2, col2, this.mte_Msr2DPrmValues[i].MeasureLen1,
                                                                       this.mte_Msr2DPrmValues[i].MeasureLen2, this.mte_Msr2DPrmValues[i].MeasureSigma, this.mte_Msr2DPrmValues[i].MeasureThreshold,
                                                                       new HTuple(), new HTuple(), out index);
                            msr2DResult.msrObjType.Append(0);
                            //lineIndices.Append(index);
                            break;
                        case "Rectangle1":
                        case "Rectangle2":
                            HOperatorSet.SmallestRectangle2Xld(this.mte_Msr2DRegValues[i].obj, out row, out col, out phi, out len1, out len2);
                            HOperatorSet.AddMetrologyObjectRectangle2Measure(msr2DResult.msrHandle, row, col, phi, len1, len2, this.mte_Msr2DPrmValues[i].MeasureLen1,
                                                                             this.mte_Msr2DPrmValues[i].MeasureLen2, this.mte_Msr2DPrmValues[i].MeasureSigma, this.mte_Msr2DPrmValues[i].MeasureThreshold,
                                                                             new HTuple(), new HTuple(), out index);
                            msr2DResult.msrObjType.Append(1);
                            //rectMsrIndices.Append(index);
                            break;
                        case "Circle":
                            HOperatorSet.FitCircleContourXld(this.mte_Msr2DRegValues[i].obj, "algebraic", -1, 0, 0, 3, 2.0, out row, out col,
                                                             out radius1, out startPhi, out endPhi, out pointOrder);
                            HOperatorSet.AddMetrologyObjectCircleMeasure(msr2DResult.msrHandle, row, col, radius1, this.mte_Msr2DPrmValues[i].MeasureLen1,
                                                                        this.mte_Msr2DPrmValues[i].MeasureLen2, this.mte_Msr2DPrmValues[i].MeasureSigma, this.mte_Msr2DPrmValues[i].MeasureThreshold,
                                                                        (new HTuple("start_phi")).TupleConcat("end_phi"), (new HTuple(this.mte_Msr2DPrmValues[i].StartAngle).TupleRad()).TupleConcat(
                                                                        new HTuple(this.mte_Msr2DPrmValues[i].EndAngle).TupleRad()), out index);
                            msr2DResult.msrObjType.Append(2);
                            //circleMsrIndices.Append(index);
                            break;
                        case "Ellipse":
                            HOperatorSet.FitEllipseContourXld(this.mte_Msr2DRegValues[i].obj, "fitzgibbon", -1, 0, 0, 200, 3, 2.0, out row, out col, out phi,
                                                              out radius1, out radius2, out startPhi, out endPhi, out pointOrder);
                            HOperatorSet.AddMetrologyObjectEllipseMeasure(this.msr2DResult.msrHandle, row, col, phi, radius1, radius2, this.mte_Msr2DPrmValues[i].MeasureLen1,
                                                                          this.mte_Msr2DPrmValues[i].MeasureLen2, this.mte_Msr2DPrmValues[i].MeasureSigma, this.mte_Msr2DPrmValues[i].MeasureThreshold,
                                                                          (new HTuple("start_phi")).TupleConcat("end_phi"), (new HTuple(this.mte_Msr2DPrmValues[i].StartAngle).TupleRad()).TupleConcat(
                                                                          new HTuple(this.mte_Msr2DPrmValues[i].EndAngle).TupleRad()), out index);
                            msr2DResult.msrObjType.Append(3);
                            //ellipseMsrIndices.Append(index);
                            break;
                        default:
                            return false;
                    }
                    //设置测量模型参数
                    HOperatorSet.SetMetrologyObjectParam(msr2DResult.msrHandle, index, "num_instances", this.mte_Msr2DPrmValues[i].InstanceNums);
                    if (this.mte_Msr2DPrmValues[i].MeasureNums != -1)
                        HOperatorSet.SetMetrologyObjectParam(msr2DResult.msrHandle, index, "num_measures", this.mte_Msr2DPrmValues[i].MeasureNums);
                    HOperatorSet.SetMetrologyObjectParam(msr2DResult.msrHandle, index, "measure_select", this.mte_Msr2DPrmValues[i].MeausreSelect);
                    HOperatorSet.SetMetrologyObjectParam(msr2DResult.msrHandle, index, "measure_transition", this.mte_Msr2DPrmValues[i].MeasureTransition);
                    HOperatorSet.SetMetrologyObjectParam(msr2DResult.msrHandle, index, "min_score", this.mte_Msr2DPrmValues[i].MinScore);
                }
                #endregion

                HOperatorSet.ApplyMetrologyModel(this.htWindow.Image, msr2DResult.msrHandle);

                #region 显示测量结果
                fitContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out fitContours, msr2DResult.msrHandle, "all", "all", 1.5);
                HOperatorSet.GetMetrologyObjectIndices(msr2DResult.msrHandle, out index);
                conts.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out conts, msr2DResult.msrHandle, index, "all", out row, out col);
                cross.Dispose();
                HOperatorSet.GenCrossContourXld(out cross, row, col, 5, 0);
                conts = conts.ConcatObj(cross);
                this.htWindow.RefreshWindow(this.htWindow.Image, null, "");
                DialogResult dr = MessageBox.Show("是否需要显示测量细节？", "疑问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "red");
                    HOperatorSet.DispObj(conts, htWindow.HTWindow.HalconWindow);
                }
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                HOperatorSet.DispObj(fitContours, htWindow.HTWindow.HalconWindow);
                #endregion

                msr2DResult.distPoint1 = this.mte_Msr2DDistPt1;
                msr2DResult.distPoint2 = this.mte_Msr2DDistPt2;
                //判断每一个测量目标是否正确测量
                HTuple fitNum = 0;
                bool status = true;
                for (int i = 0; i < index.Length; i++)
                {
                    HOperatorSet.GetMetrologyObjectNumInstances(msr2DResult.msrHandle, index[i], out fitNum);
                    if (fitNum.I != this.mte_Msr2DPrmValues[i].InstanceNums)
                    {
                        MessageBox.Show(String.Format("第{0}个测量目标未获取正确的测量信息！", i));
                        status = false;
                    }
                }
                if (status)
                    msr2DResult.createMsr2DOK = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fitContours != null) fitContours.Dispose();
                if (conts != null) conts.Dispose();
                if (cross != null) cross.Dispose();
            }
        }

        #endregion


    }
}
