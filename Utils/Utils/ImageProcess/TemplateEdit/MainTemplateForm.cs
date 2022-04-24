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
            /// 测量模板
            /// </summary>
            Measure = 1
        }
        /// <summary>
        /// 交互操作类型
        /// </summary>
        class DrawType
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
            public VaryHobject()
            {
                obj = new HObject();
                HOperatorSet.GenEmptyObj(out obj);
                type = "";
            }
            public void Dispose()
            {
                obj.Dispose();
                type = "";
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
            public TemplateParam(double angleStart, double angleExtent,int numLevel, double score, int type, bool isCreateTmp)
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
        public MainTemplateForm(TemplateScence tmpScence, HTWindowControl htWindow,object obj)
        {
            InitializeComponent();
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
            if (!InitResources(tmpScence,obj))
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

        #region 测量数据类型
        #endregion
        #endregion

        #region 窗体事件
        //上一步
        private void btnLast_Click(object sender, EventArgs e)
        {
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
            if (this.nodeFrm.Equals(this.llFrm.Last))
            {
                //单击完成按钮时，开始进行创建模板
                ToolKits.UI.DialogUI.Loading loadFrm = new UI.DialogUI.Loading();
                loadFrm.Text = "正在创建模板，请稍等...";
                loadFrm.Shown += loadFrm_Shown;
                loadFrm.ShowDialog();

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
                        MessageBox.Show("创建模板失败！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                case TemplateScence.Measure:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 初始化缓存变量与首个窗体资源
        /// </summary>
        /// <param name="tmpScence"></param>
        /// <returns></returns>
        private bool InitResources(TemplateScence tmpScence,object obj)
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
                    //mte_rotateFrm = new MatchTmpEdit.RotateForm();
                    llFrm.AddLast(mte_rotateFrm);
                    //mte_foreGrdFrm = new MatchTmpEdit.ForeGroundForm();
                    llFrm.AddLast(mte_foreGrdFrm);
                    //mte_mdlRegFrm = new MatchTmpEdit.ModelRegionForm();
                    llFrm.AddLast(mte_mdlRegFrm);
                    //mte_shwContFrm = new MatchTmpEdit.ShowContourForm();
                    llFrm.AddLast(mte_shwContFrm);
                    //mte_defPointFrm = new MatchTmpEdit.DefPointForm();
                    llFrm.AddLast(mte_defPointFrm);
                    //mte_mtchRegFrm = new MatchTmpEdit.MatchRegionForm();
                    llFrm.AddLast(mte_mtchRegFrm);
                    //mte_rejRegFrm = new MatchTmpEdit.RejectRegionForm();
                    llFrm.AddLast(mte_rejRegFrm);
                    break;
                case TemplateScence.Measure:
                    break;
                default:
                    MessageBox.Show("未知的创建类型！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }
            #region 添加图像资源
            drawTypeImg = new Dictionary<string, Image>();
            drawTypeImg.Add(DrawType.Point, AutomaticAOI.Properties.Resources.point);
            drawTypeImg.Add(DrawType.Line, AutomaticAOI.Properties.Resources.line);
            drawTypeImg.Add(DrawType.Rectangle1, AutomaticAOI.Properties.Resources.rect1);
            drawTypeImg.Add(DrawType.Rectangle2, AutomaticAOI.Properties.Resources.rect2);
            drawTypeImg.Add(DrawType.Circle, AutomaticAOI.Properties.Resources.circle);
            drawTypeImg.Add(DrawType.Ellipse, AutomaticAOI.Properties.Resources.ellipse);
            drawTypeImg.Add(DrawType.Region, AutomaticAOI.Properties.Resources.region);
            drawTypeImg.Add(DrawType.Xld, AutomaticAOI.Properties.Resources.xld);
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
                        case TemplateScence.Measure:
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
                    break;
                case TemplateScence.Measure:
                    break;
            }
            if(llFrm!=null)
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
                return ex.Message;
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
                    MessageBox.Show("收集模板区信息失败！\r\n详细错误信息：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                HTuple numLevel=null;
                if(mte_TmpPrmValues.NumLevel==0)
                    numLevel="auto";
                else numLevel=mte_TmpPrmValues.NumLevel;
                Vision.create_model(this.htWindow.Image, modelRegion, fore_region, mte_TmpPrmValues.AngleStart,
                                    mte_TmpPrmValues.AngleExtent, numLevel, mte_TmpPrmValues.Type, out tmpResult.modelID, out iFlag);

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
                MessageBox.Show("创建模板失败！\r\n详细错误信息：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion


    }
}
