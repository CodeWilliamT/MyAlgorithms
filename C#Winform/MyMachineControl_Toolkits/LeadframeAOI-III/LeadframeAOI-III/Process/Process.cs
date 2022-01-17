//#define NoInspection
//#define NoCatch
/****************************************************************************/
/*  File Name   :   Process.cs                                              */
/*  Brief       :   Process functions                                       */
/*  Date        :   2017/7/13                                               */
/*  Author      :   Tongqing CHEN	                                        */
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;
using HalconDotNet;
using System.IO;
using HT_Lib;
using LFAOIReview;
using VisionMethonDll;


namespace LeadframeAOI
{

    /// <summary>
    /// 各模块状态
    /// ******************
    /// Chuck模块 by LPY
    /// ******************
    /// Chuck_Load：        到达上料位且准备完成
    /// Chuck_WaitLoadOver：等待上料完成
    /// Chuck_Code：        扫描二维码
    /// Chuck_Calib_Init：  初始化标定数据信息
    /// Chuck_Calib_Move：  移动至标定点位
    /// Chuck_AutoFocus：   自动聚焦
    /// Chuck_Scan_Init：   初始化扫描数据信息
    /// Chuck_Scan：        扫描单颗芯片
    /// Chuck_ToUnload：    到达下料位
    /// Chuck_Unload：      下料准备
    /// </summary>
    public enum ModuleState
    {
        /// <summary>上料盒</summary>
        Genoral_LoadMgz = 101,
        /// <summary>下料盒</summary>
        Genoral_UnloadMgz = 102,
        /// <summary>下料机构更换NG料盒</summary>
        UnLoad_SwitchMgzNG = 103,
        /// <summary>推料片上导轨</summary>
        Load_LoadOn = 201,
        /// <summary>推料片至等待位</summary>
        Load_WaitPos = 202,
        /// <summary>推料片至检测位</summary>
        Load_DetectPos = 203,
        /// <summary>检查下料料盒是否满</summary>
        Unload_CheckFull = 301,
        /// <summary>移动至下料等待位</summary>
        Unload_WaitPos = 302,
        /// <summary>移动至下料右极限位</summary>
        Unload_UnloadFrame = 303,
        /// <summary>将料片推入料盒</summary>
        Unload_FrameToMgz = 304,
        /// <summary>等待上料机构推料片到检测位</summary>
        Chuck_LoadReady = 401,
        /// <summary>移动至二维码</summary>
        Chuck_Code = 402,
        /// <summary>通过视觉相机拍照通过二维码算法识别二维码</summary>
        READ_CODE = 403,
        /// <summary>通过二维码识别器识别二维码</summary>
        READ_CODE_EX = 404,
        /// <summary>手动输入二维码</summary>
        MANUAL_INPUT = 405,
        /// <summary>获取扫描点位</summary>
        Chuck_Scan_Init = 406,
        /// <summary>点扫</summary>
        Chuck_ScanPoint = 407,
        /// <summary>按行线扫</summary>
        Chuck_ScanRow = 408,
        /// <summary>按列线扫</summary>
        Chuck_ScanCol = 409,
        /// <summary>开始视觉检测</summary>
        Chuck_Inspect_Point = 410,
        /// <summary>开始NG芯片喷印标记</summary>
        NG_MARKING = 411,
        /// <summary>检测台下料,松开料片固定部件</summary>
        Chuck_Unload = 412,
    }
    /// <summary>
    /// 图像交互指令 by TWL
    /// ScanCode:   扫描二维码获得批次号
    /// CalibRC2XYZ：定位返回像素坐标与图像中心的差值
    /// AutoFocus：自动聚焦计算返回聚焦位
    /// ScanPoint： 扫描采集一颗芯片图像
    /// ScanRow：扫描采集一行芯片图像
    /// CalibUV2XY：定位返回像素坐标
    /// </summary>
    //视觉线程任务启动信号
    public enum VisionState
    {
        NoTask = 0,
        InitCamera = 100, OpenCamera = 101, CloseCamera = 102,
        ScanPoint = 300, ScanRow = 301, AutoFocus = 302, ScanCode = 303, CalibRC2XYZ = 304, CalibUV2XY = 305,
        WaitCmd = 306, Inspection = 307
    }
    /// <summary>
    /// 状态信号 by LPY
    /// </summary>
    class StateFlag
    {
        /// <summary>交互信号 上料能否正常  by Task.load发送</summary>
        public Boolean Load_CanLoad;
        /// <summary>交互信号 上料完成  by Task.load发送</summary>
        public Boolean Load_LoadOver;
        /// <summary>检测完成信号</summary>
        public Boolean Chuck_InspectOver;
        /// <summary>检测结果信号,该料片是否存在ng芯片</summary>
        public Boolean Chuck_InspectResult;
        /// <summary>下料是否已到等待位</summary>
        public Boolean Unload_WaitReady;
        /// <summary>NO料盒就位信号</summary>
        public Boolean NGBoxReady;
        /// <summary>图像完成返回信号 true 完成 false 未完成 by LPY</summary>
        public Boolean visionOver;
        /// <summary>图像返回结果状态 true 执行成功 false 执行失败 by LPY</summary>
        public Boolean visionSuccess;

        public StateFlag()
        {
            Load_CanLoad = true;
            Load_LoadOver = false;
            Chuck_InspectOver = false;
            Chuck_InspectResult = false;
            Unload_WaitReady = true;
            NGBoxReady = false;
            visionOver = false;
            visionSuccess = false;
        }
    }

    public struct NGLocation
    {
        public Double NGx;
        public Double NGy;
    }
    public class ProcessData
    {
        private const int Default_Num = 100;
        public int LotIdx;
        public int Idx_Load;
        public int Idx_OK;
        public int Idx_NG;
        public int visionRow = 0;
        public int visionColumn = 0;
        public int Counter_NG=0;
        public int Counter_Total = 0;
        public Double _visionX = 0;
        public Double _visionY = 0;
        /// <summary>
        /// X,Y分别表示料盒批次，当前检测层数
        /// </summary>
        public System.Drawing.Point FrameDetect;
        /// <summary>
        /// X,Y分别表示料盒批次，上一检测层数
        /// </summary>
        public System.Drawing.Point FrameLast;
        /// <summary>
        /// 实例化，层数上限用默认测试值
        /// </summary>
        public ProcessData()
        {
            LotIdx = 0;
            Idx_Load = 1;
            Idx_OK = 1;
            Idx_NG = 1;
            visionRow = 0;
            visionColumn = 0;
            _visionX = 0;
            _visionY = 0;
            Counter_NG = 0;
            Counter_Total = 0;
            FrameDetect = new System.Drawing.Point(0, 0);
            FrameLast = new System.Drawing.Point(0, 0);
        }
    }
    class Process
    {

        #region 事件
        public delegate void MchineStateChangedDel(MachineState state);
        public event MchineStateChangedDel MchineStateChangedHandle;
        #endregion

        //0-手工上下料；1-自动上下料。
        private int _loadMode = 0;
        private Boolean flagWork = true;
        private MachineState currentState = MachineState.STATE_IDLE;

        //stop singal different pause
        Boolean _stopSingal = false;


        /// <summary>
        /// 结束当前批次
        /// </summary>
        private Boolean _endCurrentLot = false;
        private int _piecesNumber = 0;
        private int _currentFrameNumber;
        /// <summary>
        /// 当结批信号发生在传片前，预先阻止传片和接片
        /// </summary>
        private Boolean _forestallLoadFrame = false;
        private Boolean flagRun = false;
        private Boolean flagAlarm = false;
        private int delayTime = 100;
        private int Idx_Unload;
        private String codeStr = String.Empty;
        byte[] scannerStart = { 0X16, 0X54, 0X0D };
        byte[] scannerStop = { 0X16, 0X55, 0X0D };
        private List<NGLocation> _ngLocation = new List<NGLocation>();
        private ProcessData processData = new ProcessData();
        #region 任务模块间交互变量
        public StateFlag stateFlag = new StateFlag();
        private List<Task> taskList = new List<Task>();
        public InteractiveData interactiveData = new InteractiveData();  //图像交互
        private int calib_index = 0;
        public int scan_index = 0;
        private int snap_index = 0;
        private int _equivalentRowShowed = 0; //显示mapping中用到的行数
        public static Vision.AutoFocusParam focusPara = null;        //by TWL
        bool moduletest = false;
        #endregion
        public ConcurrentQueue<System.Drawing.Point> qLoadIdx;
        public string LotId;
        public string FrameId;
        public string NowProduct = "";

        public int LoadMode { set { _loadMode = value; } }

        public Boolean StopSingal
        {
            get { return _stopSingal; }
            set { _stopSingal = value; }
        }
        public Boolean EndCurrentLot
        {
            get { return _endCurrentLot; }
            set { _endCurrentLot = value; }
        }


        #region 图像交互变量  by M.bing
        /// <summary>
        /// 单个拍照位图片队列
        /// </summary>
        public ConcurrentQueue<ImageCache> qAllImageCache;
        /// <summary>
        /// 拍照位位置信息
        /// </summary>
        public List<ImagePosition> ThisScanPostions,ThisMarkPostions;
        #endregion


        public Process()
        {
            App.obj_Pdt.ProductInfoChangedEvent += ProductInfoChangedHandle;
        }

        private void ProductInfoChangedHandle()
        {
            //HTUi.TipHint("产品数据发生改变!");

            //更新load模块的产品参数
            //更新unLoad模块产品参数
            //更新chuck模块产品参数
            //更新process模块产品参数
        }

        /// <summary>
        /// 初始化队列 by M.Bing
        /// </summary>
        public void InitCaches()
        {
            if (qAllImageCache == null) qAllImageCache = new ConcurrentQueue<ImageCache>();
            if (App.obj_Vision.qSnapResult == null) App.obj_Vision.qSnapResult = new ConcurrentQueue<SnapResult>();
            if (ThisScanPostions == null) ThisScanPostions = new List<ImagePosition>();
            ThisMarkPostions = new List<ImagePosition>();
            if (qLoadIdx == null) qLoadIdx = new ConcurrentQueue<System.Drawing.Point>();
            App.obj_Vision.IsDbLocked = false;
            App.obj_Vision.saveResultFlag = false;
        }
        /// <summary>
        /// 销毁队列 by M.Bing
        /// </summary>
        public void DisposeCaches()
        {
            DateTime t = DateTime.Now;
            if (qAllImageCache != null)
            {
                while (qAllImageCache.Count != 0 || App.obj_Vision.ThreadInspectionFlag)
                {
                    if (DateTime.Now.Subtract(t).TotalMilliseconds > 5000) break;
                    Thread.Sleep(200);
                }
                foreach (var item in qAllImageCache)
                {
                    item.Dispose();
                }
                qAllImageCache = null;
            }

            if (App.obj_Vision.qSnapResult != null)
            {
                t = DateTime.Now;
                while (App.obj_Vision.qSnapResult.Count != 0 && App.obj_Vision.saveResultFlag)
                {
                    if (DateTime.Now.Subtract(t).TotalMilliseconds > 5000) break;
                    Thread.Sleep(200);
                }

                foreach (var item in App.obj_Vision.qSnapResult)
                {
                    item.Dispose();
                }
                App.obj_Vision.qSnapResult = null;

            }
            if (ThisScanPostions != null)
            {
                ThisScanPostions.Clear();
                ThisScanPostions = null;
            }
            if (qLoadIdx != null)
            {
                System.Drawing.Point ItemLoadIdx;
                while (qLoadIdx.Count!=0)
                {
                    qLoadIdx.TryDequeue(out ItemLoadIdx);
                }
            }
            try
            {
                while (App.obj_Vision.IsDbLocked)
                {
                    t = DateTime.Now;
                    if (DateTime.Now.Subtract(t).TotalMilliseconds > 10000) break;
                    Thread.Sleep(200);
                }
                if (DataManager.IsWritingLot)
                {
                    App.obj_Vision.IsDbLocked = true;
                    if (LFAOIReview.DataManager.CurrentLot != null && LFAOIReview.DataManager.CurrentLot != "") LFAOIReview.DataManager.EndLot();

                    App.obj_Vision.IsDbLocked = false;
                }
                HTLog.Info("线程资源已释放");
            }
            catch (Exception ex)
            {
                HTLog.Error("数据库关闭过程中出现错误:\n" + ex.ToString());
                //try
                //{
                //    DataManager.UnlockDb();
                //}
                //catch
                //{
                //    HTLog.Error("数据库解锁过程中出现错误:\n" + ex.ToString());
                //}
            }
        }

        public MachineState CurrentState { get { return currentState; } }



        //public bool StopSingal { get => _stopSingal; set => _stopSingal = value; }

        public void EndLot()
        {
            _endCurrentLot = true;
            HTLog.Debug("【流程】结批开始...");
        }

        public void CreateTasks()
        {
            taskList.Add(Task.Factory.StartNew(TaskMain));
            taskList.Add(Task.Factory.StartNew(TaskLoad));
            taskList.Add(Task.Factory.StartNew(TaskUnload));
            taskList.Add(Task.Factory.StartNew(TaskChuck));
            taskList.Add(Task.Factory.StartNew(TaskVision));
        }
        public void EndTasks()
        {
            flagWork = false;
            foreach (Task t in taskList)
            {
                t.Wait();
            }
        }
        public void ProcessStart()
        {

            App.obj_Main.SetSingnalDevice(YellowON: false, GreenON: true, RedON: false, BuzzerON: false);
            InitProcess();
            if (App.obj_SystemConfig.LotMode == 1&& LotId=="")
            {
                if (App.obj_SystemConfig.LotIdMode == 0)
                {
                    Form_InputLotId.ShowDialogForm();
                    if (Form_InputLotId.Instance.DialogResult == DialogResult.Cancel)
                    {
                        ProcessAlarm("【Load】" + "未创建批次，流程报警");
                        ProcessStop();
                        return;
                    }
                }
                else if (App.obj_SystemConfig.LotIdMode == 1)
                {
                    App.obj_Process.InitialNewLot();
                }
            }
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:运行中  批次号:" + LotId; }));
            HTLog.Debug("【流程】开始...");
            flagRun = true;
            currentState = MachineState.STATE_RUN;
        }
        public void InitialNewLot()
        {
            LotId = DateTime.Now.ToString("yyyyMMdd-HH-mm-ss");
            NowProduct = ProductMagzine.ActivePdt;
            LotInfo lotInfo = new LotInfo();
            lotInfo.ProductCode = ProductMagzine.ActivePdt;
            lotInfo.LotName = App.obj_Process.LotId;
            lotInfo.Operator = App.mainWin.ActiveUser.Name;
            lotInfo.Machine = "AI-8231";
            lotInfo.TotalFrameCount = Convert.ToInt32("10");
            lotInfo.RowCount = App.obj_Pdt.RowNumber;
            lotInfo.ColumnCount = App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber;
            DataManager.DataBaseDirectory = App.ProductDir;

            List<DefectTypeInfo> list_DefectType = new List<DefectTypeInfo>();
            using (StreamReader sr = new StreamReader(App.programDir + "\\DefectTypeNew.txt", Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    DefectTypeInfo defectType = new DefectTypeInfo();
                    string[] strs = line.Split(' ');
                    defectType.DefectType = strs[1];
                    defectType.Index = Convert.ToInt32(strs[0]);
                    list_DefectType.Add(defectType);
                }
            }
            while (App.obj_Vision.IsDbLocked)
            {
                Thread.Sleep(200);
            }
            App.obj_Vision.IsDbLocked = true;
            //DateTime t = DateTime.Now;
            DataManager.InitialNewLot(lotInfo, list_DefectType);
            DataManager.StartLot();
            App.obj_Vision.IsDbLocked = false;
            HTLog.Debug(String.Format("【新批次】:产品:{0};批次号:{1};操作员:{2};机器型号:{3};料片数:{4};芯片行数:{5};芯片列数:{6};",
                lotInfo.ProductCode, lotInfo.LotName, lotInfo.Operator, lotInfo.Machine, lotInfo.TotalFrameCount, lotInfo.RowCount, lotInfo.ColumnCount));
            //double a = DateTime.Now.Subtract(t).TotalMilliseconds;
        }
        public void InitialNewLot(string LotId)
        {
            App.obj_Process.LotId = LotId;
            //DateTime t = DateTime.Now;
            if (LotId == "")
                App.obj_Process.LotId = DateTime.Now.ToString("yyyyMMdd-HH-mm-ss");
            NowProduct = ProductMagzine.ActivePdt;
            LotInfo lotInfo = new LotInfo();
            lotInfo.ProductCode = ProductMagzine.ActivePdt;
            lotInfo.LotName = App.obj_Process.LotId;
            lotInfo.Operator = App.mainWin.ActiveUser.Name;
            lotInfo.Machine = "AI-8231";
            lotInfo.TotalFrameCount = Convert.ToInt32("10");
            lotInfo.RowCount = App.obj_Pdt.RowNumber;
            lotInfo.ColumnCount = App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber;
            DataManager.DataBaseDirectory = App.ProductDir;

            List<DefectTypeInfo> list_DefectType = new List<DefectTypeInfo>();
            using (StreamReader sr = new StreamReader(App.programDir + "\\DefectTypeNew.txt", Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    DefectTypeInfo defectType = new DefectTypeInfo();
                    string[] strs = line.Split(' ');
                    defectType.DefectType = strs[1];
                    defectType.Index = Convert.ToInt32(strs[0]);
                    list_DefectType.Add(defectType);
                }
            }
            while (App.obj_Vision.IsDbLocked)
            {
                Thread.Sleep(200);
            }
            App.obj_Vision.IsDbLocked = true;
            DataManager.InitialNewLot(lotInfo, list_DefectType);
            DataManager.StartLot();
            App.obj_Vision.IsDbLocked = false;
            HTLog.Debug(String.Format("【新批次】:产品:{0};批次号:{1};操作员:{2};机器型号:{3};料片数:{4};芯片行数:{5};芯片列数:{6};",
                lotInfo.ProductCode, lotInfo.LotName, lotInfo.Operator, lotInfo.Machine, lotInfo.TotalFrameCount, lotInfo.RowCount, lotInfo.ColumnCount));
            //double a = DateTime.Now.Subtract(t).TotalMilliseconds;
            //HTLog.Info(DateTime.Now.Subtract(t).TotalMilliseconds.ToString());
        }
        public void ProcessPause()
        {
            App.obj_Main.SetSingnalDevice(YellowON: true, GreenON: false, RedON: false, BuzzerON: false);
            flagRun = false;
            currentState = MachineState.STATE_PAUSE;
            HTLog.Debug("【流程】暂停...");
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:暂停  批次号:" + LotId; }));
        }
        public void ProcessResume()
        {
            flagRun = true;
            stateFlag.Load_CanLoad = true;
            App.obj_Main.SetSingnalDevice(YellowON: false, GreenON: true, RedON: false, BuzzerON: false);
            currentState = MachineState.STATE_RUN;
            HTLog.Debug("【流程】继续...");
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:运行中  批次号:" + LotId; }));
        }
        public Boolean ProcessHome()
        {
            HTLog.Debug("【流程】回零...");
            flagRun = false;
            if (App.obj_Main.SetSingnalDevice(YellowON: false, GreenON: false, RedON: false, BuzzerON: false) == false)
            {
                return false;
            }
            currentState = MachineState.STATE_IDLE;
            return true;
        }
        #region 将process里面的LogDebugShow、HTLog.ERROR、HTLog.Inf显示在mainwindow下
        #endregion

        public void ProcessAlarm(String strAlarm)
        {
            if (currentState == MachineState.STATE_IDLE)
            {
                HTLog.Error("流程停止后，本应有的警告...");
                HTLog.Error(strAlarm);
                FormJobs.Instance.Invoke(new Action(() =>
                {
                    FormJobs.Instance.SetOtherFrm(true);
                }));
                return;
            }
            App.obj_Main.SetSingnalDevice(YellowON: false, GreenON: false, RedON: true, BuzzerON: true);
            flagAlarm = true;
            currentState = MachineState.STATE_ALARM;
            HTLog.Error("流程警告...");
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:报警  批次号:" + LotId; }));
            FormJobs.Instance.lbxAlarm.Invoke(
                (MethodInvoker)delegate
                {
                    FormJobs.Instance.lbxAlarm.Items.Add(String.Format("{0}  {1}", DateTime.Now.ToShortTimeString().ToString(), strAlarm));
                });
            if (MchineStateChangedHandle != null)
            {
                MchineStateChangedHandle(App.obj_Process.CurrentState);
            }
        }
        public void LogNgInfShow(String date, String ngNum)
        {
            try
            {
                if (FormJobs.Instance.lbxNgInf.IsDisposed || !FormJobs.Instance.lbxNgInf.Parent.IsHandleCreated)
                {
                    return;
                }
                FormJobs.Instance.lbxNgInf.Invoke(
                  (MethodInvoker)delegate
                  {
                      FormJobs.Instance.lbxNgInf.Items.Add(String.Format("{0}  {1}", date, ngNum));
                      if (FormJobs.Instance.lbxNgInf.Items.Count > 0)
                      {
                          FormJobs.Instance.lbxNgInf.SelectedItem = FormJobs.Instance.lbxNgInf.Items[FormJobs.Instance.lbxNgInf.Items.Count - 1];
                      }
                      FormJobs.Instance.lbxNgInf.ForeColor = System.Drawing.Color.Black;
                  });
            }
            catch (Exception EXP)
            {
                HTLog.Error(EXP.Message);
            }
            //HTLog.Debug(ngNum);
        }

        public void ClearAlarm()
        {
            flagRun = false;
            flagAlarm = false;
            App.obj_Main.SetSingnalDevice(YellowON: true, GreenON: false, RedON: false, BuzzerON: false);
            currentState = MachineState.STATE_PAUSE;
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:暂停  批次号:" + LotId; }));
        }
        public void ProcessStop()
        {
            flagRun = false;
            flagAlarm = false;
            _stopSingal = true;

            App.obj_Main.SetSingnalDevice(YellowON: false, GreenON: false, RedON: false, BuzzerON: false);
            currentState = MachineState.STATE_IDLE;

            DisposeCaches(); //by M.Bing
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:停止"; }));
            FormJobs.Instance.label6.Invoke(new Action(() => { FormJobs.Instance.label6.Text = ""; }));
        }
        private void InitProcess()
        {
            processData = new ProcessData();
            App.obj_Load.CurrentModuleState = ModuleState.Genoral_LoadMgz;
            App.obj_Unload.CurrentModuleState = ModuleState.Genoral_LoadMgz;
            if (App.obj_SystemConfig.LoadManually)
            {
                App.obj_Load.CurrentModuleState = ModuleState.Load_LoadOn;
                App.obj_Unload.CurrentModuleState = ModuleState.Unload_WaitPos;
            }
            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_LoadReady;
            App.obj_Vision.VisionState = 0;
            stateFlag.Load_CanLoad = true;
            stateFlag.Load_LoadOver = false;
            stateFlag.Chuck_InspectOver = false;
            stateFlag.Chuck_InspectResult = true;
            stateFlag.Unload_WaitReady = true;
            stateFlag.NGBoxReady = false;
            _endCurrentLot = false;

            _forestallLoadFrame = false;//当结批信号发生在传片前，预先阻止传片和接片
            _equivalentRowShowed = 0;

            InitCaches();  // by M.Bing

        }
        /// <summary>
        /// 在一定时间内等待图像返回结果，否则为超时
        /// </summary>
        /// <param name="wait_time">等待时间</param>
        /// <returns>返回bool类型值，超时为false</returns>
        public bool waitVisionDone(Double wait_time)
        {
            DateTime t_end;
            Double timeout = 0;
            DateTime t_start = DateTime.Now;
            while (timeout < wait_time)
            {
                if (stateFlag.visionOver)
                {
                    stateFlag.visionOver = false;
                    break;
                }
                t_end = DateTime.Now;
                timeout = t_end.Subtract(t_start).TotalSeconds;
            }
            if (timeout >= wait_time)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 用来处理外部输入或消息等
        /// </summary>
        private void TaskMain()
        {
            while (flagWork)
            {
                Thread.Sleep(delayTime);
                try
                {
                    if (App.obj_Main.GetStartSignal())
                    {
                        switch (App.obj_Process.CurrentState)
                        {
                            case MachineState.STATE_IDLE:
                                ProcessStart();
                                break;
                            case MachineState.STATE_PAUSE:
                                ProcessResume();
                                break;
                            default:
                                break;
                        }
                    }
                    if (App.obj_Main.GetStopSignal())//修改替换
                    {
                        ProcessStop();
                    }
                    if (App.obj_Main.GetResetSignal())
                    {
                        if (FormJobs.Instance.lbxAlarm.IsDisposed || !FormJobs.Instance.lbxAlarm.Parent.IsHandleCreated)
                        {
                            continue;
                        }
                        FormJobs.Instance.lbxAlarm.Invoke(
                          (MethodInvoker)delegate
                          {
                              if (FormJobs.Instance.lbxAlarm.Items.Count != 0)
                              {
                                  FormJobs.Instance.lbxAlarm.Items.Remove(FormJobs.Instance.lbxAlarm.Items[0]);
                              }
                          });
                        FormJobs.Instance.lbxAlarm.Invoke(
                         (MethodInvoker)delegate
                         {
                             if (FormJobs.Instance.lbxAlarm.Items.Count == 0)
                             {
                                 App.obj_Process.ClearAlarm();
                             }
                         });
                    }
                    if (MchineStateChangedHandle != null)
                    {
                        MchineStateChangedHandle(App.obj_Process.CurrentState);
                    }
                }
                catch (Exception EXP)
                {
                    HTLog.Error(EXP.Message);
                }
            }
            //HTLog.Debug("task MAIN ending...");
        }
        /// <summary>
        /// 上料线程
        /// </summary>
        private void TaskLoad()
        {
            while (flagWork)
            {
                if (!flagRun || flagAlarm)
                {
                    Thread.Sleep(delayTime);
                    continue;
                }
                switch (App.obj_Load.CurrentModuleState)
                {
                    //task 0:下料盒（一盘回到这里）
                    case ModuleState.Genoral_UnloadMgz:
                        if (!stateFlag.Load_CanLoad)
                        {
                            Thread.Sleep(delayTime);
                            continue;
                        }
                        HTLog.Debug("【Load】下Load料盒...");
                        if (!App.obj_SystemConfig.LoadManually)
                        {
                            if (App.obj_Load.UnloadMagezine(Left: true) < 0)
                            {
                                HTLog.Error("【Load】下Load料盒失败");
                                stateFlag.Load_CanLoad = false;
                                break;
                            }
                        }
                        HTLog.Debug("【Load】下料盒成功");
                        if (qLoadIdx.Count < 1)
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Genoral_LoadMgz;
                        }
                        else
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Load_DetectPos;
                        }
                        break;
                    //task 1:上料盒,I_L置零
                    case ModuleState.Genoral_LoadMgz:
                        if (_endCurrentLot)
                        {
                            Thread.Sleep(delayTime);
                            continue;
                        }
                        if (!stateFlag.Load_CanLoad)
                        {
                            Thread.Sleep(delayTime);
                            continue;
                        }
                        if (App.obj_SystemConfig.LoadManually)
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Load_LoadOn;
                            break;
                        }
                        HTLog.Debug("【Load】上Load料盒...");
                        int temp = 0;
                        temp = App.obj_Load.LoadMagezine(Left: true);
                        if (temp == -1)
                        {
                            ProcessAlarm(App.obj_Load.GetLastErrorString());
                            break;
                        }
                        else if (temp == -2)
                        {
                            //if (snap_index == 0
                            //&& App.obj_Chuck.CurrentModuleState == ModuleState.Chuck_LoadReady)
                            //{
                            //ProcessAlarm("【Load】无备用料盒可上，请上新的Load料盒");
                            // }
                            // else
                            // {
                            //HTLog.Error("【Load】无备用料盒可上，请上新的Load料盒");
                            //stateFlag.Load_CanLoad = false;
                            //}
                            HTLog.Error("【Load】无备用料盒可上，开始结批");
                            EndLot();
                            break;
                        }
                        processData.Idx_Load = 1;
                        HTLog.Debug("【Load】上Load料盒成功");
                        if(qLoadIdx==null)
                        {
                            HTLog.Error("【Load】流程已停止，不继续上料盒");
                            break;
                        }
                        if (qLoadIdx.Count < 1)
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Load_LoadOn;
                        }
                        else
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Load_DetectPos;
                        }
                        break;
                    //task 2:检测台夹爪移动至上料位，运动到I_L层,推出料片,收回推片,夹爪夹紧
                    case ModuleState.Load_LoadOn:
                        if (_endCurrentLot)//判断结批信号有无
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                            break;
                        }
                        if (App.obj_SystemConfig.Marking&&App.obj_Pdt.UseMarker)
                        {
                            if (App.obj_Chuck.IsHaveInk() == 0)
                            {
                                ProcessAlarm("喷印器无墨水！");
                                break;
                            }
                            if (App.obj_Chuck.IsJetRun() == 0)
                            {
                                ProcessAlarm("喷印器未启动！");
                            }
                        }
                        if (!App.obj_SystemConfig.LoadManually)
                        {
                            if (processData.Idx_Load == 1)
                            {
                                processData.LotIdx += 1;
                            }
                            HTLog.Debug(String.Format("【Load】上第{0}片料片...", processData.Idx_Load));
                            int ret = App.obj_Load.LoadFrameFromMgz(processData.Idx_Load);
                            if (ret == 0)
                            {
                                if (qLoadIdx != null) qLoadIdx.Enqueue(new System.Drawing.Point(processData.LotIdx, processData.Idx_Load));
                                HTLog.Debug(String.Format("【Load】推片成功，第{0}片", processData.Idx_Load));
                                processData.Idx_Load++;
                            }
                            else if (ret == -1) //上片失败
                            {
                                ProcessAlarm("【Load】" + App.obj_Load.GetLastErrorString());
                                break;
                            }
                            else if (ret == -2) //无料片
                            {
                                HTLog.Debug(String.Format("【Load】第{0}层无料片", processData.Idx_Load));
                                processData.Idx_Load++;
                                if (processData.Idx_Load > App.obj_Pdt.SlotNumber)
                                {
                                    HTLog.Debug("【Load】料盒已空下料盒...");
                                    App.obj_Load.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (!App.obj_Load.CheckLeftHaveFrame())
                                break;
                            HTLog.Debug(String.Format("【Load】上第{0}片料片...", processData.Idx_Load));
                            if (processData.Idx_Load == 1)
                            {
                                processData.LotIdx += 1;
                            }
                            processData.Idx_Load++;
                        }
                        App.obj_Load.CurrentModuleState = ModuleState.Load_WaitPos;
                        break;
                    //task 3:检测台移动至左等待位
                    case ModuleState.Load_WaitPos:
                        HTLog.Debug("【Load】移动料片至等待位...");
                        if (!App.obj_Load.TransferFrame2WaitPos())
                        {
                            ProcessAlarm("【Load】" + App.obj_Load.GetLastErrorString());
                            break;
                        }
                        if (processData.Idx_Load > App.obj_Pdt.SlotNumber && !stateFlag.Unload_WaitReady)
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                        }
                        else
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Load_DetectPos;
                        }
                        break;

                    //task 4:等待下料机构可上料信号为true，置false，移动至检测位，松开夹爪，上料机构上料完成信号true，I_L++，判定I_L<N_L,true则返回第二步，false则下料盒
                    case ModuleState.Load_DetectPos:
                        if (!stateFlag.Unload_WaitReady)
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        processData.FrameLast = new System.Drawing.Point(0, 0);
                        stateFlag.Unload_WaitReady = false;
                        HTLog.Debug("【Load】移动料片至检测位...");
                        if (!App.obj_Load.TransferFrame2LoadPos())
                        {
                            ProcessAlarm(App.obj_Load.GetLastErrorString());
                            break;
                        }
                        HTLog.Debug("【Load】至检测位完成");
                        processData.FrameLast = processData.FrameDetect;
                        if (qLoadIdx != null)
                            qLoadIdx.TryDequeue(out processData.FrameDetect);
                        stateFlag.Load_LoadOver = true;
                        if (processData.Idx_Load > App.obj_Pdt.SlotNumber&& !App.obj_SystemConfig.LoadManually)
                        {
                            if (App.obj_Load.IsHaveMgz())
                                App.obj_Load.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                            else
                                App.obj_Load.CurrentModuleState = ModuleState.Genoral_LoadMgz;
                        }
                        else
                        {
                            App.obj_Load.CurrentModuleState = ModuleState.Load_LoadOn;
                        }
                        break;
                }
            }
            //HTLog.Debug("task Load ending...");
        }

        /// <summary>
        /// 下料线程
        /// </summary>
        private void TaskUnload()
        {
            while (flagWork)
            {
                if (!flagRun || flagAlarm)
                {
                    Thread.Sleep(delayTime);
                    continue;
                }
                switch (App.obj_Unload.CurrentModuleState)
                {
                    //task 1 ：上空良品料盒，良品料盒Idx为零
                    case ModuleState.Genoral_LoadMgz:
                        if (_endCurrentLot)
                        {
                            ProcessStop();
                            HTLog.Debug("【流程】已停止");
                            break;
                        }
                        if (!stateFlag.Load_CanLoad)
                        {
                            ProcessAlarm("【UnLoad】上料端无法提供新料片，下料端不上新料盒，流程报警停止");
                        }
                        HTLog.Debug("【UnLoad】上OK料盒...");
                        if (!App.obj_SystemConfig.LoadManually)
                        {
                            int temp = 0;
                            temp = App.obj_Unload.LoadMagezine(Left: false);
                            if (temp == 0)
                            {
                                processData.Idx_OK = 1;
                                processData.Idx_NG = 1;
                            }
                            else if (temp == -1)
                            {
                                ProcessAlarm(App.obj_Unload.GetLastErrorString());
                                break;
                            }
                            else if (temp == -2)
                            {
                                ProcessAlarm("【UnLoad】下料传输平台无新料盒，流程停止");
                                break;
                            }
                        }
                        HTLog.Debug("【UnLoad】上OK料盒成功");
                        App.obj_Unload.CurrentModuleState = ModuleState.Unload_WaitPos;
                        break;
                    //task 2 ：等待检测台检测完成信号为true，则置false，移动至右等待位，可上料信号置true
                    case ModuleState.Unload_WaitPos:
                        if (_endCurrentLot)//判断结批信号有无
                        {
                            if (snap_index == 0
                                && App.obj_Load.CurrentModuleState == ModuleState.Genoral_LoadMgz
                                && App.obj_Chuck.CurrentModuleState == ModuleState.Chuck_LoadReady && !stateFlag.Chuck_InspectOver)
                            {
                                HTLog.Debug("【流程】结批完成");
                                while (App.obj_Vision.IsDbLocked)
                                {
                                    Thread.Sleep(200);
                                }
                                App.obj_Vision.IsDbLocked = true;
                                if (LFAOIReview.DataManager.CurrentLot != null && LFAOIReview.DataManager.CurrentLot != "") LFAOIReview.DataManager.EndLot();
                                App.obj_Vision.IsDbLocked = false;
                                App.obj_Unload.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                break;
                            }
                        }
                        if (App.obj_SystemConfig.LoadSame)
                        {
                            if (qLoadIdx.Count == 0 && processData.FrameLast.X == 0&& processData.Idx_Load > App.obj_Pdt.SlotNumber)
                            {
                                App.obj_Unload.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                break;
                            }
                        }
                        if (!stateFlag.Chuck_InspectOver)
                        {
                            Thread.Sleep(delayTime);
                            continue;
                        }
                        stateFlag.Chuck_InspectOver = false;
                        //**********因无感应器暂时跳到移动到右极限位
                        App.obj_Unload.CurrentModuleState = ModuleState.Unload_UnloadFrame;
                        break;
                        //************
                        HTLog.Debug("【UnLoad】移动料片至等待位...");
                        if (!App.obj_Unload.UnloadMove2Wait())
                        {
                            HTLog.Error("【UnLoad】移动料片等待位失败！");
                            ProcessAlarm(App.obj_Unload.GetLastErrorString());
                            break;
                        }
                        stateFlag.Unload_WaitReady = true;
                        App.obj_Unload.CurrentModuleState = ModuleState.Unload_UnloadFrame;
                        break;
                    //task 3 ：根据检测结果信号移动至良品或次品料盒上料位，I_OK或I_NG层，并使料片处于最右端，推片
                    case ModuleState.Unload_UnloadFrame:
                        if (!App.obj_SystemConfig.LoadManually)
                        {
                            if (App.obj_SystemConfig.LoadSame)
                            {
                                processData.Idx_OK = processData.FrameDetect.Y;
                            }
                            else
                            {
                                if(processData.Idx_OK!=1)
                                    processData.Idx_OK += App.obj_Pdt.BlankNumber_Unload;
                            }
                            HTLog.Debug("【UnLoad】将" + (stateFlag.Chuck_InspectResult ? "良品盒" + processData.Idx_OK + "层" : "不良品盒" + processData.Idx_NG + "层") + "对准...");

                            if (!App.obj_Unload.UnlaodMgzMove2ReceiveFrame(stateFlag.Chuck_InspectResult ? processData.Idx_OK : processData.Idx_NG, OkCst: stateFlag.Chuck_InspectResult
                                ))
                            {
                                HTLog.Error("【UnLoad】料盒对准失败！");
                                ProcessAlarm(App.obj_Unload.GetLastErrorString());
                                break;
                            }
                            if (stateFlag.Chuck_InspectResult) processData.Idx_OK++;
                            else processData.Idx_NG++;
                            stateFlag.Chuck_InspectResult = true;
                            HTLog.Debug("【UnLoad】移动料片至右边缘...");
                            if (!App.obj_Unload.TransferFrameToMgz())
                            {
                                HTLog.Error("【UnLoad】移动料片至右边缘失败！");
                                ProcessAlarm(App.obj_Unload.GetLastErrorString());
                                break;
                            }
                            App.obj_Unload.CurrentModuleState = ModuleState.Unload_FrameToMgz;
                            stateFlag.Unload_WaitReady = true;
                            break;
                        }
                        else
                        {
                            HTLog.Debug("【UnLoad】移动料片至右边缘...");
                            if (!App.obj_Unload.TransferFrameToRight())
                            {
                                HTLog.Error("【UnLoad】推片至右边缘失败！");
                                ProcessAlarm(App.obj_Unload.GetLastErrorString());
                                break;
                            }
                            App.obj_Unload.CurrentModuleState = ModuleState.Unload_CheckFull;
                            stateFlag.Unload_WaitReady = true;
                            HTLog.Debug("【UnLoad】下料已完成...");
                            break;
                        }

                    case ModuleState.Unload_FrameToMgz:
                        HTLog.Debug("【UnLoad】推片入盒...");
                        if (!App.obj_Unload.UnloadFrameToMgz())
                        {
                            HTLog.Error("【UnLoad】推片入盒失败");
                            ProcessAlarm(App.obj_Unload.GetLastErrorString());
                            break;
                        }
                        //if (!App.obj_Unload.UnloadFrameToMgz())
                        //{
                        //    HTLog.Error("【UnLoad】推片入盒失败");
                        //    ProcessAlarm(App.obj_Unload.GetLastErrorString());
                        //    break;
                        //}
                        App.obj_Unload.CurrentModuleState = ModuleState.Unload_CheckFull;
                        HTLog.Debug("【UnLoad】下料已完成...");
                        break;
                    //task 4 ：根据检测结果信号，判定I_OK++或I_NG++,检测结果信号置true，判定I_OK<N_OK或I_NG<N_NG，true则，下料机构就位信号true，回到第二步，false则下一步
                    case ModuleState.Unload_CheckFull:

                        if (App.obj_SystemConfig.LoadSame)
                        {
                            if (qLoadIdx.Count != 0)
                            {
                                if (qLoadIdx.First().X > processData.FrameDetect.X && App.obj_Pdt.HeightFirstSlot < App.obj_Pdt.HeightLastSlot)
                                {
                                    processData.Idx_OK = App.obj_Pdt.SlotNumber + 1;
                                }
                                if (qLoadIdx.First().X < processData.FrameDetect.X && App.obj_Pdt.HeightFirstSlot > App.obj_Pdt.HeightLastSlot)
                                {
                                    processData.Idx_OK = App.obj_Pdt.SlotNumber + 1;
                                }
                            }
                            //查看是否满料
                            if (processData.Idx_OK > App.obj_Pdt.SlotNumber)
                            {
                                HTLog.Debug("【UnLoad】OK料盒已满换料盒...");
                                App.obj_Unload.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                break;
                            }
                            else if (processData.Idx_NG > App.obj_Pdt.SlotNumber)
                            {
                                HTLog.Debug("【UnLoad】NG料盒已满换料盒...");
                                App.obj_Unload.CurrentModuleState = ModuleState.UnLoad_SwitchMgzNG;
                                break;
                            }
                            else if (!stateFlag.Load_CanLoad)
                            {
                                HTLog.Debug("【Chuck】上料机构无法提供可检测料片");
                                App.obj_Unload.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                break;
                            }
                            else
                            {
                                App.obj_Unload.CurrentModuleState = ModuleState.Unload_WaitPos;
                                break;
                            }
                        }
                        else
                        {

                            //查看是否满料
                            if (processData.Idx_OK+App.obj_Pdt.BlankNumber_Unload > App.obj_Pdt.SlotNumber)
                            {
                                HTLog.Debug("【UnLoad】OK料盒已满换料盒...");
                                App.obj_Unload.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                break;
                            }
                            else if (processData.Idx_NG > App.obj_Pdt.SlotNumber)
                            {
                                HTLog.Debug("【UnLoad】NG料盒已满换料盒...");
                                App.obj_Unload.CurrentModuleState = ModuleState.UnLoad_SwitchMgzNG;
                                break;
                            }
                            else if (!stateFlag.Load_CanLoad)
                            {
                                HTLog.Debug("【Chuck】上料机构无法提供可检测料片");
                                App.obj_Unload.CurrentModuleState = ModuleState.Genoral_UnloadMgz;
                                break;
                            }
                            else
                            {
                                App.obj_Unload.CurrentModuleState = ModuleState.Unload_WaitPos;
                                break;
                            }
                        }
                    //task 5 ：等待次品置换料盒成功信号，收到后置false，下料机构就位信号true，回第二步
                    case ModuleState.UnLoad_SwitchMgzNG:
                        if (_endCurrentLot)
                        {
                            Thread.Sleep(delayTime);
                            continue;
                        }
                        if (!stateFlag.NGBoxReady)
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        stateFlag.NGBoxReady = false;
                        processData.Idx_NG = 0;
                        HTLog.Debug("【UnLoad】换NG料盒成功");
                        App.obj_Unload.CurrentModuleState = ModuleState.Unload_WaitPos;
                        break;
                    //task 5 ：良品下料盒
                    case ModuleState.Genoral_UnloadMgz:
                        if (App.obj_SystemConfig.LoadManually)
                        {
                            App.obj_Unload.CurrentModuleState = ModuleState.Genoral_LoadMgz;
                            break;
                        }
                        HTLog.Debug("【UnLoad】下OK料盒成功");
                        if (!App.obj_SystemConfig.LoadManually && App.obj_Unload.UnloadMagezine(Left: false) < 0)
                        {
                            HTLog.Error("【UnLoad】下OK料盒失败");
                            ProcessAlarm(App.obj_Unload.GetLastErrorString());
                            break;
                        }
                        App.obj_Unload.CurrentModuleState = ModuleState.Genoral_LoadMgz;
                        break;

                }
            }
            //HTLog.Debug("task Unload ending...");
        }

        /// <summary>
        /// Chuck线程
        /// </summary>
        private void TaskChuck()
        {
            string errStr = "";
            while (flagWork)
            {
                if (!flagRun || flagAlarm)
                {
                    Thread.Sleep(delayTime);
                    continue;
                }
                switch (App.obj_Chuck.CurrentModuleState)
                {
                    //等待上料机构上料完成信号为true，则置false，压下压板
                    case ModuleState.Chuck_LoadReady:
                        if (!stateFlag.Load_LoadOver||App.obj_Unload.CurrentModuleState==ModuleState.Unload_FrameToMgz)
                        {
                            Thread.Sleep(delayTime);
                            continue;
                        }
                        stateFlag.Load_LoadOver = false;
                        HTLog.Debug("【Chuck】固定料片...");
#if NoCatch
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Scan_Init;
                        break;
#endif
                        if (!App.obj_Chuck.CatchFrame())
                        {
                            ProcessAlarm(App.obj_Chuck.GetLastErrorString());
                            break;
                        }
                        if (processData.FrameLast.X != processData.FrameDetect.X)
                        {
                            if (App.obj_SystemConfig.LotMode == 0)
                            {
                                if (App.obj_SystemConfig.LotIdMode == 0)
                                {
                                    Form_InputLotId.ShowDialogForm();
                                    if (Form_InputLotId.Instance.DialogResult == DialogResult.Cancel)
                                    {
                                        ProcessAlarm("【Load】" + "未创建批次，流程报警");
                                        break;
                                    }
                                }
                                else if (App.obj_SystemConfig.LotIdMode == 1)
                                {
                                    App.obj_Process.InitialNewLot();
                                }
                                FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:运行中  批次号:" + LotId; }));
                            }
                        }

                        App.obj_Chuck.CurrentModuleState = ModuleState.READ_CODE;
                        break;
                    case ModuleState.Chuck_Code: //移动到二维码
                        HTLog.Debug("【Chuck】移动到二维码扫描位...");
                        if (!App.obj_Chuck.XYZ_Move(App.obj_Pdt.X_Code2D, App.obj_Pdt.Y_Code2D, App.obj_Pdt.ZFocus))
                        {
                            ProcessAlarm(App.obj_Chuck.GetLastErrorString());//报警 停止动作
                            break;
                        }
                        App.obj_Chuck.CurrentModuleState = ModuleState.READ_CODE_EX;
                        break;
                    case ModuleState.READ_CODE_EX:
                        HTLog.Debug("启动二维码识别...");
                        if (App.obj_ReadQRCode.SendMes(scannerStart) == false)
                        {
                            ProcessAlarm("打开二维码扫描器串口失败！");
                            break;
                        }
                        if (App.obj_Chuck.X_RSMove(5, 0.3) == false)//微移扫描二维码
                        {
                            ProcessAlarm("移动到扫描二维码位置失败！");//报警 停止动作
                            break;
                        }
                        if (App.obj_ReadQRCode.ScannerWaitTime(1000) == false)
                        {
                            App.obj_ReadQRCode.SendMes(scannerStop);
                            App.obj_Chuck.CurrentModuleState = ModuleState.MANUAL_INPUT;
                            break;
                        }
                        App.obj_ReadQRCode.SendMes(scannerStop);
                        App.obj_Vision.frameName = Encoding.ASCII.GetString(App.obj_ReadQRCode.Buffer.ToArray());
                        if (App.obj_Vision.ConfigFrameFolder() == false)
                        {
                            ProcessAlarm("料片文件夹创建失败！");
                            break;
                        }
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Scan_Init;
                        break;
                    case ModuleState.READ_CODE:
                        if (!GetFrameId())
                        {
                            ProcessAlarm("识别二维码失败！");
                            break;
                        }
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Scan_Init;
                        break;
                    case ModuleState.MANUAL_INPUT: //手工输入二维码
                        HTLog.Debug("创建料片文件夹...");
                        App.obj_Vision.frameName = DateTime.Now.ToLongTimeString().ToString();
                        if (App.obj_Vision.ConfigFrameFolder() == false)
                        {
                            ProcessAlarm("料片文件夹创建失败！");
                            break;
                        }
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Scan_Init;
                        break;

                    //case ModuleState.Chuck_Calib_Init: //标定数据初始化
                    //    HTLog.Debug("标定...");
                    //    if (!App.obj_Vision.CalibProduct())
                    //    {
                    //        ProcessAlarm(string.Format("产品标定失败!,详细信息:{0}", App.obj_Vision.GetLastErrorString()));
                    //        break;
                    //    }
                    //    scan_index = 0;
                    //    HTLog.Debug("产品标定成功");
                    //    App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanPoint;
                    //    HTLog.Debug("开始扫描...");
                    //    break;
                    case ModuleState.Chuck_Scan_Init://扫描信息初始化
                        HTLog.Debug("拍摄矫正点比对，初始化扫描信息...");
                        FormJobs.Instance.ShowProdMap();
                        App.obj_Vision.ImageThreadNow = 0;
                        if (App.obj_SystemConfig.ScanMode == 0)
                        {
                            if (!GetImagePositions_PointAuto(FormJobs.Instance.htWindow))
                            {
                                ProcessAlarm("初始化扫描位置信息失败!");
                                break;
                            }
                            FormJobs.Instance.frameFolder = App.obj_Vision.frameFolder;
                            scan_index = 0;
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanPoint;
                        }
                        else if (App.obj_SystemConfig.ScanMode == 1)
                        {
                            if (!GetImagePositions_RowAuto(FormJobs.Instance.htWindow))
                            {
                                ProcessAlarm("初始化扫描位置信息失败!");
                                break;
                            }
                            FormJobs.Instance.frameFolder = App.obj_Vision.frameFolder;
                            scan_index = 0;
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanRow;
                        }
                        else if (App.obj_SystemConfig.ScanMode == 2)
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanCol;
                        }
                        else
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanPoint;
                        }
                        HTLog.Debug("扫描...");
                        break;
                    #region chuck_Snap
                    case ModuleState.Chuck_ScanPoint://移动到指定位置
                        if (ThisScanPostions == null)
                        {
                            if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                        App.obj_Chuck.ref_y,
                        App.obj_Chuck.z_Safe))
                            {
                                ProcessAlarm("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                            }
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Unload;
                            break;
                        }
                        if (ThisScanPostions.Count == 0)
                        {
                            if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                        App.obj_Chuck.ref_y,
                        App.obj_Chuck.z_Safe))
                            {
                                ProcessAlarm("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                            }
                            App.obj_Chuck.CurrentModuleState = ModuleState.NG_MARKING;
                            break;
                        }
                        if (App.obj_Chuck.Move2ImagePosition(ThisScanPostions[scan_index].x, ThisScanPostions[scan_index].y, ThisScanPostions[scan_index].z) == false)
                        {
                            ProcessAlarm(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                                        ThisScanPostions[scan_index].b,
                                                        ThisScanPostions[scan_index].r,
                                                        ThisScanPostions[scan_index].c,
                                                        App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                            break;
                        }
                        ImageCache imageCache = new ImageCache();
                        errStr = CaputreMultipleImages(ref imageCache);
                        if (errStr == "")//拍照完成
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Inspect_Point;
                            break;
                        }
                        else
                        {
                            //imageCache.Dispose();
                            ProcessAlarm(String.Format("移动的拍照位{0}_{1}_{2}失败!详细信息:{3}",
                                                        ThisScanPostions[scan_index].b,
                                                        ThisScanPostions[scan_index].r,
                                                        ThisScanPostions[scan_index].c,
                                                        errStr));//报警 停止动作
                        }
                        break;
                    case ModuleState.Chuck_ScanRow:
                        //启动算法模块
                        if (App.obj_Vision.VisionState != 0 & App.obj_Vision.VisionState != VisionState.Inspection)
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        App.obj_Vision.VisionState = VisionState.Inspection;
                        //启动线性扫描
                        if (!App.obj_MobSht.StartMobileShooting_X(App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex],
                            App.obj_Chuck.dev_chuck.a_X, App.obj_Chuck.dev_chuck.a_Y))
                        {
                            ProcessAlarm("线性扫描失败:" + App.obj_MobSht.GetLastErrorString());
                        }
                        //扫描结束，移动至避让位
                        if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                        App.obj_Chuck.ref_y,
                        App.obj_Chuck.z_Safe))
                        {
                            ProcessAlarm("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                        }
                        App.obj_Chuck.CurrentModuleState = ModuleState.NG_MARKING;
                        break;
                    case ModuleState.Chuck_ScanCol:

                        break;
                    //开启视觉检测
                    case ModuleState.Chuck_Inspect_Point:
                        if (App.obj_Vision.VisionState != 0 & App.obj_Vision.VisionState != VisionState.Inspection)
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        App.obj_Vision.VisionState = VisionState.Inspection;
                        scan_index++;
                        if (scan_index >= ThisScanPostions.Count)
                        {
                            if (!App.obj_Chuck.XYZ_Move(App.obj_Chuck.ref_x,
                        App.obj_Chuck.ref_y,
                        App.obj_Chuck.z_Safe))
                            {
                                ProcessAlarm("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                            }
                            App.obj_Chuck.CurrentModuleState = ModuleState.NG_MARKING;
                            break;
                        }
                        if (App.obj_SystemConfig.ScanMode == 0)
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanPoint;
                        }
                        else if (App.obj_SystemConfig.ScanMode == 1)
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanRow;
                        }
                        else if (App.obj_SystemConfig.ScanMode == 2)
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanCol;
                        }
                        else
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanPoint;
                        break;
                    #endregion Chuck_Snap
                    //标记NG 芯片
                    case ModuleState.NG_MARKING:
                        if (!(App.obj_SystemConfig.Marking && App.obj_Pdt.UseMarker))
                        {
                            App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Unload;
                            break;
                        }
                        if (!stateFlag.visionOver )
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        if (App.obj_SystemConfig.Marking && App.obj_Pdt.UseMarker)
                        {
                            if (App.obj_Chuck.IsHaveInk() == 0)
                            {
                                ProcessAlarm("喷印器无墨水！");
                                break;
                            }
                            if (App.obj_Chuck.IsJetRun() == 0)
                            {
                                ProcessAlarm("喷印器未启动！");
                            }
                        }
                        if (!stateFlag.visionOver )
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        if (App.obj_Vision.VisionState != 0)
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        //添加Chuck模块里的打点函数,for循环
                        for (int i = 0; i < ThisMarkPostions.Count; i = i + 1)
                        {
                            if (App.obj_Chuck.EC_Printer(ThisMarkPostions[i].x, ThisMarkPostions[i].y, ThisMarkPostions[i].z,Form_Ec_Jet.IsX, Form_Ec_Jet.RXorY) == false)
                            {
                                ProcessAlarm(String.Format("喷码失败!详细信息:{3}",
                                                            ThisMarkPostions[i].b,
                                                            ThisMarkPostions[i].r,
                                                            ThisMarkPostions[i].c,
                                                            App.obj_Chuck.GetLastErrorString()));//报警 停止动作
                                break;
                            }
                            if (i != 0)
                            {
                                if (App.obj_Process.ThisMarkPostions[i].r != App.obj_Process.ThisMarkPostions[i - 1].r || App.obj_Process.ThisMarkPostions[i].c != App.obj_Process.ThisMarkPostions[i - 1].c)
                                {
                                    App.obj_Vision.SnapPos(App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i - 1].r, App.obj_Process.ThisMarkPostions[i - 1].c].x,
                                        App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i - 1].r, App.obj_Process.ThisMarkPostions[i - 1].c].y, App.obj_Pdt.ZFocus,FormJobs.Instance.htWindow, out App.obj_Vision.Image);
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
                                    App.obj_Vision.ScanMatrix[App.obj_Process.ThisMarkPostions[i].r, App.obj_Process.ThisMarkPostions[i].c].y, App.obj_Pdt.ZFocus, FormJobs.Instance.htWindow, out App.obj_Vision.Image);
                                if (App.obj_SystemConfig.MarkSave == 1)
                                {
                                    HOperatorSet.ZoomImageFactor(App.obj_Vision.Image, out App.obj_Vision.Image, App.obj_Vision.scaleFactor, App.obj_Vision.scaleFactor, "constant");
                                    string snapPath = App.obj_Vision.frameFolder + "\\MarkImgs";
                                    Directory.CreateDirectory(snapPath);
                                    HOperatorSet.WriteImage(App.obj_Vision.Image, "tiff", 0, snapPath + "\\"+App.obj_Process.ThisMarkPostions[i].r + "-" + App.obj_Process.ThisMarkPostions[i].c + ".tiff");
                                }
                            }
                        }
                        ThisMarkPostions.Clear();
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Unload;
                        HTLog.Debug("等待下料端料片放入料盒...");
                        break;
                    //下料准备.压板抬升，夹爪收紧
                    case ModuleState.Chuck_Unload:
#if NoCatch
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_LoadReady;
                        stateFlag.Chuck_InspectOver = true;
                        break;
#endif
                        if(!stateFlag.visionOver&& App.obj_Vision.RunMode == Convert.ToInt16(SystemRunMode.MODE_ONLINE))
                        {
                            Thread.Sleep(delayTime);
                            break;
                        }
                        HTLog.Debug("【Chuck】松开料片...");
                        if (!App.obj_Chuck.LooseFrame())
                        {
                            ProcessAlarm(App.obj_Chuck.GetLastErrorString());
                            break;
                        }

                        if (!stateFlag.Chuck_InspectResult)
                        {
                            if(App.obj_SystemConfig.LoadSame)
                                LogNgInfShow(DateTime.Now.ToShortTimeString().ToString(), "料盒内第" + (processData.FrameDetect.Y) + "层存在NG芯片");
                            else
                                LogNgInfShow(DateTime.Now.ToShortTimeString().ToString(), "料盒内第" + (processData.Idx_OK% App.obj_Pdt.SlotNumber + 1) + "层存在NG芯片");
                            stateFlag.Chuck_InspectResult = true;
                        }
                        App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_LoadReady;
                        stateFlag.Chuck_InspectOver = true;
                        scan_index = 0;
                        //for (int i = 0; i < App.obj_Pdt.RowNumber; i++)
                        //{
                        //    for (int j = 0; j < App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber; j++)
                        //    {
                        //        if (FormJobs.Instance.mappingControl1.GetDieState(i, j) == "未检测")
                        //        {
                        //            FormJobs.Instance.ShowPRODState(5, i, j);
                        //        }
                        //    }
                        //}
                        break;
                }
            }
            //HTLog.Debug("task CHUCK ending...");
        }
        /// <summary>
        /// 图像线程
        /// </summary>
        private void TaskVision()
        {
            while (flagWork)
            {
                if (!flagRun || flagAlarm)
                {
                    Thread.Sleep(delayTime);
                    continue;
                }
                switch (App.obj_Vision.VisionState)
                {
                    case VisionState.Inspection:
                        if (qAllImageCache != null && App.obj_Vision.RunMode != 1)
                        {
                            try
                            {

                                stateFlag.visionOver = false;
                                App.obj_Vision.ThreadInspectionFlag = true;

                                App.obj_Vision.ImageThreadNow = App.obj_Vision.ThreadMax;
                                Parallel.For(0, App.obj_Vision.ThreadMax, t =>
                                {
                                    while (qAllImageCache != null)
                                    {
                                        try
                                        {
                                            if (qAllImageCache == null)
                                            {
                                                return;
                                            }
                                            if (qAllImageCache.IsEmpty)
                                            {
                                                if(App.obj_Chuck.CurrentModuleState == ModuleState.Chuck_Unload)
                                                {
                                                    return;
                                                }
                                                Thread.Sleep(delayTime);
                                                continue;
                                            }
                                            ImageCache imgCache = null;
                                            //1.取图
                                            if (!qAllImageCache.TryDequeue(out imgCache))
                                            {
                                                Thread.Sleep(delayTime);
                                                continue;
                                            }
                                            //2.检测
                                            List<StructInspectResult> inspectResults;
                                            bool IsAllOkSnap = false;
                                            //if (FormJobs.Instance.ImageOrigin != null)
                                            //{
                                            //lock (FormJobs.Instance.ImageOrigin)
                                            //{
                                            //    if (FormJobs.Instance.ImageOrigin.IsInitialized())
                                            //        FormJobs.Instance.ImageOrigin.Dispose();
                                            //}
                                            //}
                                            //FormJobs.Instance.ImageOrigin = imgCache._2dImage.CopyObj(1, -1);
                                            //App.obj_Vision.ShowImage(FormJobs.Instance.htWindow, imgCache._2dImage.SelectObj(FormJobs.Instance.SelectImgIdx + 1), null);
                                            //FormJobs.Instance.ShowSelectImageChannel(FormJobs.Instance.ImageOrigin.SelectObj(FormJobs.Instance.SelectImgIdx + 1));
                                            SnapDataResult sdr;
                                            if (!App.obj_Vision.Inspection(imgCache, FormJobs.Instance.htWindow, FormJobs.Instance.SelectImgIdx, true, out inspectResults, out IsAllOkSnap,out sdr, t))
                                            {
                                                HTLog.Error("检测失败!\n" + App.obj_Vision.GetLastErrorString());
                                            }
                                            if (inspectResults == null)
                                            {
                                                continue;
                                            }
                                            if (!IsAllOkSnap&&App.obj_SystemConfig.UseNGBox)
                                            {
                                                stateFlag.Chuck_InspectResult = false;
                                            }
                                            for (int k = 0; k < inspectResults.Count; k++)
                                            {
                                                processData.Counter_Total++;
                                                if (inspectResults[k].OkOrNg)
                                                {
                                                    FormJobs.Instance.ShowPRODState(1, inspectResults[k].realRow, inspectResults[k].realCol);
                                                }
                                                else
                                                {
                                                    FormJobs.Instance.ShowPRODState(2, inspectResults[k].realRow, inspectResults[k].realCol);
                                                    ImagePosition ngPs = new ImagePosition();
                                                    ngPs.r = imgCache.r;
                                                    ngPs.c = imgCache.c;
                                                    ngPs.x = inspectResults[k].x + App.obj_Pdt.RelativeMark_X + App.obj_Chuck.Ref_Mark_x;
                                                    ngPs.y = inspectResults[k].y+App.obj_Pdt.RelativeMark_Y + App.obj_Chuck.Ref_Mark_y;
                                                    ngPs.z = App.obj_Pdt.RelativeMark_Z;
                                                    if (App.obj_Pdt.SymmetryMark && ngPs.c % 2 == 1)
                                                    {
                                                        ngPs.x = inspectResults[k].x + App.obj_Pdt.RelativeMark_X - App.obj_Chuck.Ref_Mark_x;
                                                    }
                                                    ++processData.Counter_NG;
                                                    ThisMarkPostions.Add(ngPs);
                                                }
                                            }
                                            FormJobs.Instance.label6.Invoke(new Action(() => { FormJobs.Instance.label6.Text = "总芯片数:" + (processData.Counter_Total) + "  缺陷芯片数:" + (processData.Counter_NG); }));
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
                            }
                            catch (Exception ex)
                            {

                                ProcessAlarm("算法检测失败！" + ex.ToString());
                            }
                        }
                        //HTLog.Debug("图像检测完成");
                        //GC.Collect();
                        App.obj_Vision.ThreadInspectionFlag = false;
                        stateFlag.visionOver = true;
                        if (stateFlag.Chuck_InspectOver) App.obj_Vision.VisionState = 0;
                        break;
                    default:
                        break;
                }
                Thread.Sleep(delayTime);
            }
            //HTLog.Debug("task Vision ending...");
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
            bool ringEnable = false;
            double ringTime = 0;
            bool ringEnable1 = false;
            double ringTime1 = 0;
            bool ringEnable2 = false;
            double ringTime2 = 0;
            bool ringEnable3 = false;
            double ringTime3 = 0;
            bool ringEnable4 = false;
            double ringTime4 = 0;
            bool coaxEnable1 = false;
            double coaxTime1 = 0;
            bool coaxEnable2 = false;
            double coaxTime2 = 0;
            double exposure = 0;
            double gain = 0;
            int camId = -1;
            if (App.obj_ImageInformSet.Count > 1) Thread.Sleep(100);
            for (int i = 0; i < App.obj_ImageInformSet.Count; i++)
            {
                if (!App.obj_ImageInformSet[i].Use2D && !App.obj_ImageInformSet[i].UseAutoFocus) continue;
                if (i > 0) Thread.Sleep(30);
                //4. 取图 
                //imageCache.b = ThisScanPostions[scan_index].b;
                imageCache.r = ThisScanPostions[scan_index].r;
                imageCache.c = ThisScanPostions[scan_index].c;
                if (App.obj_Vision.RunMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE)) continue;
                if (App.obj_ImageInformSet.Count < 2) goto _A;
                if (ringEnable != App.obj_ImageInformSet[i].RingLightEnable
                    || ringTime != App.obj_ImageInformSet[i].RingImageMeans)
                {
                    ringEnable = App.obj_ImageInformSet[i].RingLightEnable;
                    ringTime = App.obj_ImageInformSet[i].RingImageMeans;
                    App.obj_light.SetRingLight(ringEnable, ringTime);
                }
                if (ringEnable1 != App.obj_ImageInformSet[i].RingLightEnable1
                   || ringTime1 != App.obj_ImageInformSet[i].RingImageMeans1)
                {
                    ringEnable1 = App.obj_ImageInformSet[i].RingLightEnable1;
                    ringTime1 = App.obj_ImageInformSet[i].RingImageMeans1;
                    App.obj_light.SetRingLight_Ex(1,ringEnable1, ringTime1);
                }
                if (ringEnable2 != App.obj_ImageInformSet[i].RingLightEnable2
                   || ringTime2 != App.obj_ImageInformSet[i].RingImageMeans2)
                {
                    ringEnable2 = App.obj_ImageInformSet[i].RingLightEnable2;
                    ringTime2 = App.obj_ImageInformSet[i].RingImageMeans2;
                    App.obj_light.SetRingLight_Ex(2, ringEnable2, ringTime2);
                }
                if (ringEnable3 != App.obj_ImageInformSet[i].RingLightEnable3
                   || ringTime3 != App.obj_ImageInformSet[i].RingImageMeans3)
                {
                    ringEnable3 = App.obj_ImageInformSet[i].RingLightEnable3;
                    ringTime3 = App.obj_ImageInformSet[i].RingImageMeans3;
                    App.obj_light.SetRingLight_Ex(3, ringEnable3, ringTime3);
                }
                if (ringEnable4 != App.obj_ImageInformSet[i].RingLightEnable4
                   || ringTime4 != App.obj_ImageInformSet[i].RingImageMeans4)
                {
                    ringEnable4 = App.obj_ImageInformSet[i].RingLightEnable4;
                    ringTime4 = App.obj_ImageInformSet[i].RingImageMeans4;
                    App.obj_light.SetRingLight_Ex(4, ringEnable4, ringTime4);
                }
                if (coaxEnable1 != App.obj_ImageInformSet[i].CoaxialLightEnable1
                || coaxTime1 != App.obj_ImageInformSet[i].CoaxialImageMeans1)
                {
                    coaxEnable1 = App.obj_ImageInformSet[i].CoaxialLightEnable1;
                    coaxTime1 = App.obj_ImageInformSet[i].CoaxialImageMeans1;
                    App.obj_light.SetCoaxLight1(coaxEnable1, coaxTime1);
                }
                if (coaxEnable2 != App.obj_ImageInformSet[i].CoaxialLightEnable2
                || coaxTime2 != App.obj_ImageInformSet[i].CoaxialImageMeans2)
                {
                    coaxEnable2 = App.obj_ImageInformSet[i].CoaxialLightEnable2;
                    coaxTime2 = App.obj_ImageInformSet[i].CoaxialImageMeans2;
                    App.obj_light.SetCoaxLight2(coaxEnable2, coaxTime2);
                }
                if (camId != App.obj_ImageInformSet[i].CameraName)
                {
                    exposure = App.obj_ImageInformSet[i].Exposure;
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[i].CameraName].SetExposure(exposure);
                    gain = App.obj_ImageInformSet[i].Gain;
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[i].CameraName].SetGain(gain);
                    camId = App.obj_ImageInformSet[i].CameraName;
                }
                else
                {
                    if (exposure != App.obj_ImageInformSet[i].Exposure)
                    {
                        exposure = App.obj_ImageInformSet[i].Exposure;
                        App.obj_Vision.obj_camera[App.obj_ImageInformSet[i].CameraName].SetExposure(exposure);
                    }
                    if (gain != App.obj_ImageInformSet[i].Gain)
                    {
                        gain = App.obj_ImageInformSet[i].Gain;
                        App.obj_Vision.obj_camera[App.obj_ImageInformSet[i].CameraName].SetGain(gain);
                    }
                }
            _A:
                if (App.obj_ImageInformSet[i].Use2D)
                {
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[i].CameraName].Camera.ClearImageQueue();
                    App.obj_Chuck.SWPosTrig();//触发
                                              //3.拍图
                    errStr = App.obj_Vision.obj_camera[App.obj_ImageInformSet[i].CameraName].CaputreImages(ref imageCache._2dImage, 1, 1000);
                    if (errStr != "")
                    {
                        imageCache.Dispose();
                        return errStr;
                    }
                }
                //HT_Lib.HTLog.Info(DateTime.Now.Subtract(t).TotalMilliseconds.ToString());
                imageCache.X = App.obj_Chuck.GetXPos();
                imageCache.Y = App.obj_Chuck.GetYPos();
                if (App.obj_ImageInformSet[i].UseAutoFocus)
                {
                    HTuple Z_TrigPos;
                    HObject Image3D;
                    App.obj_Process.Auto3DFocus(App.obj_ImageInformSet[i].CameraName, App.obj_ImageInformSet[i].Fps, App.obj_ImageInformSet[i].ZFocusStart,
                    App.obj_ImageInformSet[i].ZFocusEnd, App.obj_ImageInformSet[i].TrigInterval, out Z_TrigPos, out Image3D);
                    imageCache.List_3dImage.Add(Image3D);
                    imageCache.List_Z_TrigPos.Add(Z_TrigPos);
                }
                //string snapPath = App.obj_Vision.frameFolder + "\\Row" +
                //    imageCache.r + "-" + imageCache.c;
                //Directory.CreateDirectory(snapPath);
                //HOperatorSet.WriteImage(imageCache._2dImage.SelectObj(i + 1), "tiff", 0, snapPath + "\\" + i + ".tiff");
                //HTuple SnapX = null, SnapY = null;
                //SnapX = imageCache.X;
                //SnapY = imageCache.Y;
                //HOperatorSet.WriteTuple(SnapX, snapPath + "\\" + "SnapX.tup");
                //HOperatorSet.WriteTuple(SnapY, snapPath + "\\" + "SnapY.tup");
            }
            if (qAllImageCache != null) qAllImageCache.Enqueue(imageCache);
            return errStr;
            //************
            //#region 3D拍摄Line图
            //int ringIntensity = (int)App.obj_ImageInformSet[0].RingImageMeans;
            //int coaxialIntensity = (int)App.obj_ImageInformSet[0].CoaxialImageMeans;
            ////1.设置环形光亮度
            //App.obj_light.SetRingLight(true, ringIntensity);
            //App.obj_light.SetCoaxLight(false, coaxialIntensity);
            ////2.Z轴运动触发
            //if(!App.obj_Chuck.Z_LineTrigger())
            //{
            //    return App.obj_Chuck.GetLastErrorString();
            //}
            //int trigNum = 0;
            //if(!App.obj_Chuck.GetZTrigCnt(out trigNum))
            //{
            //    return "获取Z触发个数失败!";
            //}
            ////3.采集图片并调用算法得出结果
            //List<double> indexList;
            //if(!App.obj_Vision.CaputreImagesFocus3d(trigNum,ref imageCache._3dImage, out indexList))
            //{
            //    return App.obj_Vision.GetLastErrorString();
            //}
            //imageCache._3dImgKeys.Add("SharpImage");
            //imageCache._3dImgKeys.Add("Depth");
            //imageCache._3dImgKeys.Add("SharpImage");
            ////string path = App.obj_Vision.visionPath + "\\" + imageCache.m.ToString() + "-" + imageCache.n.ToString();
            //#endregion

            //#region 2D拍摄剩下两张图
            //for (int i = 1; i < 3; i++)
            //{
            //    //利用3d算法得出的清晰度高度拍其它两张图
            //    Double posFoucs = indexList[i - 1] * (App.obj_Chuck.z_TrigEnd - App.obj_Chuck.z_TrigStart) / trigNum;
            //    if(!App.obj_Chuck.Z_Move((posFoucs + App.obj_Chuck.z_TrigStart)))
            //    {
            //        return App.obj_Chuck.GetLastErrorString();
            //    } 

            //    //1.设置在第一个光源下 相机的增益值和曝光值             
            //    double exposure = App.obj_ImageInformSet[i].Exposure;
            //    double gain = App.obj_ImageInformSet[i].Gain;
            //    ringIntensity = (int)App.obj_ImageInformSet[i].RingImageMeans;
            //    coaxialIntensity = (int)App.obj_ImageInformSet[i].CoaxialImageMeans;
            //    App.obj_Vision.obj_camera[0].SetExposure(exposure);
            //    App.obj_Vision.obj_camera[0].SetGain(gain);
            //    App.obj_light.SetRingLight(false, ringIntensity);
            //    App.obj_light.SetCoaxLight(true, coaxialIntensity);
            //    App.obj_Chuck.SWPosTrig();//触发
            //    Thread.Sleep(10);
            //    //3.拍图
            //    App.obj_Chuck.SWPosTrig();
            //    errStr =  App.obj_Vision.obj_camera[0].CaputreImages(ref imageCache._2dImage, 1, 20);
            //    if(errStr != "")
            //    {
            //        imageCache.Dispose();
            //        return errStr;
            //    }
            //    //b_m_n_Pi  = [ 所在block _ 拍照位行 _ 拍照位列 _ 第几张图 ]
            //    imageCache._2dImgKeys.Add(String.Format("{0}_{1}_{2}_P{3}", imageCache.b, imageCache.m, imageCache.n, i));
            //}
            //#endregion
            //return errStr;
        }
        /// <summary>
        /// 获取料片名，内含使用二维码的预操作
        /// </summary>
        /// <returns></returns>
        public Boolean GetFrameId()
        {
            try
            {
                FrameId = DateTime.Now.ToString("yyyyMMdd-HH-mm-ss");
                if (App.obj_SystemConfig.PrdQr)
                {
                    //移动
                    if (!App.obj_Chuck.XYZ_Move(App.obj_Pdt.X_Code2D, App.obj_Pdt.Y_Code2D, App.obj_Pdt.ZFocus)) return false;
                    //拍照
                    App.obj_Vision.obj_camera[App.obj_ImageInformSet[0].CameraName].Camera.ClearImageQueue();
                    App.obj_Chuck.SWPosTrig();//触发
                    string errStr = App.obj_Vision.obj_camera[App.obj_ImageInformSet[0].CameraName].CaputreImages(ref App.obj_Vision.Image, 1, 1000);
                    if (errStr != "")
                    {
                        App.obj_Vision.Image.Dispose();
                        return false;
                    }
                    //回安全位
                    if (!App.obj_Chuck.MoveToSafeZPos())
                    {
                        return false;
                    }
                    if (App.obj_Vision.ScanCode2D() == false)
                    {
                        Form_InputCode2D.ShowDialogForm();
                        if (Form_InputCode2D.Instance.DialogResult == DialogResult.OK)
                        {
                            App.obj_Vision.frameName = Form_InputCode2D.Code2D;
                            Form_InputCode2D.Instance.Dispose();
                            goto _A;
                        }
                        else
                        {
                            Form_InputCode2D.Instance.Dispose();
                            return false;
                        }
                    }
                _A:
                    FrameId = App.obj_Vision.frameName;
                }
                try
                {
                    while (App.obj_Vision.IsDbLocked)
                    {
                        Thread.Sleep(200);
                    }
                    App.obj_Vision.IsDbLocked = true;
                    DataManager.InitialNewFrame(FrameId);
                    App.obj_Vision.IsDbLocked = false;
                }
                catch (Exception ex)
                {
                }
                App.obj_Vision.frameFolder = App.obj_Vision.imageFolder + "\\Scan\\" + ProductMagzine.ActivePdt + "\\" + App.obj_Process.LotId + "\\" + App.obj_Process.FrameId;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        ///// <summary>
        ///// 获取所有拍照位的拍摄位置信息   by TWL
        ///// </summary>
        ///// <returns></returns>
        //public bool GetImagePositions_Point()
        //{
        //    try
        //    {
        //        if (App.obj_Vision.RunMode == 1) return true;

        //        //清除之前的点位
        //        if (ThisScanPostions != null)
        //        {
        //            ThisScanPostions.Clear();
        //        }
        //        //1.根据die与die之间的关系计算得出在一个block里行方向需要几个拍照位、列方向需要几个拍照位
        //        //2.跟距标定后的结果，准确获取第一个block里所有die的位置信息
        //        //3.计算第一个block内的拍照位
        //        //4.根据block标定结果计算出所有的block拍照位信息
        //        //5.将所有拍照位按照以下顺序排列
        //        /*
        //            --> --> -->
        //                        |
        //                        |
        //            <-- <-- <--
        //            |
        //            |
        //            --> --> -->
        //          */
        //        //6.将所有拍照位所有的位置添加到list里 B=16 R=8 C=2 M=1 N=2
        //        int NumBlock = App.obj_Pdt.BlockNumber;//4;
        //        int NumC = App.obj_Pdt.ColumnNumber; //2;//每个Block多少列
        //        int NumR = App.obj_Pdt.RowNumber;//2;//8;//每个Block多少行
        //        int NumM = App.obj_Pdt.FramePicNumM;//一个视野多少行
        //        int NumN = App.obj_Pdt.FramePicNumN;//一个视野多少列
        //        double[] start = new double[2] { App.obj_Chuck.ref_x + App.obj_Pdt.RightEdge + (((double)(NumN - 1)) / 2) * App.obj_Pdt.ColomnSpace, App.obj_Chuck.ref_y + App.obj_Pdt.TopEdge + (NumR - 1) * App.obj_Pdt.RowSpace - (((double)(NumM - 1)) / 2 + 1) * App.obj_Pdt.RowSpace };//new double[2] { -182.184, -6.053 };//左上角视野中心
        //        //double[] end = new double[2] { App.obj_Chuck.ref_x + (App.obj_Pdt.RightEdge + (NumBlock - 1) * App.obj_Pdt.BlockSpace + ((int)((NumC - 1) / NumN) * NumN + ((double)NumN - 1) / 2) * App.obj_Pdt.ColomnSpace), App.obj_Chuck.ref_y + App.obj_Pdt.TopEdge };//new double[2] { -2.183, -81.754 };//右下角
        //        //double endX = start[0] + (NumBlock - 1) * App.obj_Pdt.BlockSpace + ((NumC - 1) / NumN) * NumN * App.obj_Pdt.ColomnSpace;
        //        double distanceBlockX = App.obj_Pdt.BlockSpace;//(end[0] - start[0]) / NumBlock;
        //        double distanceX = App.obj_Pdt.ColomnSpace;//distanceBlockX / App.obj_Pdt.ColumnNumber;
        //        double distanceY = App.obj_Pdt.RowSpace;//(end[1] - start[1]) / (NumM - 1);
        //        double difdistanceX = App.obj_Pdt.DifColomnSpace;
        //        ImagePosition imagePosition = new ImagePosition();
        //        imagePosition.z = App.obj_Pdt.ZFocus;

        //        string sPath = App.obj_Vision.imageFolder + "\\Scan\\" + ProductMagzine.ActivePdt + "\\" + App.obj_Process.LotId + "\\" + App.obj_Process.FrameId;
        //        Directory.CreateDirectory(sPath);
        //        IniDll.IniFiles config = new IniDll.IniFiles(sPath + "\\point.ini");
        //        int step = 0;
        //        for (int i = 0; i < NumBlock; i++)
        //        {
        //            imagePosition.b = i;
        //            for (int j = 0; j < NumC; j += NumN)
        //            {
        //                imagePosition.c = j;
        //                if (j >= NumC / 2 && NumC % 2 == 0)//如果是单数列，并且当前列大于1/2列总数则考虑特殊列
        //                {
        //                    imagePosition.x = start[0] + i * distanceBlockX + (j - 1) * distanceX + difdistanceX;
        //                }
        //                else
        //                {
        //                    imagePosition.x = start[0] + i * distanceBlockX + j * distanceX;
        //                }
        //                if ((i * ((NumC - 1) / NumN + 1) + j / NumN) % 2 == 0)
        //                {
        //                    for (int k = 0; k < NumR; k += NumM)
        //                    {
        //                        imagePosition.r = k;
        //                        imagePosition.y = start[1] - k * distanceY;
        //                        ThisScanPostions.Add(imagePosition);
        //                        config.WriteString("ScanPoint", "step" + step.ToString(), imagePosition.b + "-" + imagePosition.r + "-" + imagePosition.c + "(" + imagePosition.x.ToString() + "," + imagePosition.y + ")");
        //                        step++;
        //                    }
        //                }
        //                else
        //                {
        //                    for (int k = NumR - ((NumR % NumM == 0) ? NumM : (NumR % NumM)); k > -1; k -= NumM)
        //                    {
        //                        imagePosition.r = k;
        //                        imagePosition.y = start[1] - k * distanceY;//+ (NumR - 1 - k) * distanceY;
        //                        ThisScanPostions.Add(imagePosition);
        //                        config.WriteString("ScanPoint", "step" + step.ToString(), imagePosition.b + "-" + imagePosition.r + "-" + imagePosition.c + "(" + imagePosition.x.ToString() + "," + imagePosition.y + ")");
        //                        step++;
        //                    }
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// 初始化点扫拍照点位
        /// </summary>
        /// <param name="hTWindow"></param>
        /// <returns></returns>
        public bool GetImagePositions_PointAuto(HTHalControl.HTWindowControl hTWindow)
        {
            try
            {
                if (App.obj_Vision.RunMode == 1) return true;

                if (App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex].Type == HTupleType.INTEGER)
                {
                    HTUi.PopError("请先标定该相机！");
                    return false;
                }

                if (!App.obj_Chuck.XYZ_Move(App.obj_Vision.checkPosX, App.obj_Vision.checkPosY, App.obj_Pdt.ZFocus))
                {
                    HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                    return false;
                }
                //配置
                ImageInformation.ConfigAllData(frmCaptureImage.Instance._selectIndex);
                Thread.Sleep(500);

                App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
                App.obj_Chuck.SWPosTrig();
                //4. 取图
                if (App.obj_Vision.Image != null) App.obj_Vision.Image.Dispose();
                HOperatorSet.GenEmptyObj(out App.obj_Vision.Image);
                string errStr = "";
                errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref App.obj_Vision.Image, 1, 5000);//获取图片
                //HOperatorSet.WriteImage(App.obj_Vision.Image, "tiff", 0, "D:\\alignImage.tiff");
                //FormJobs.Instance.ShowImage(Image,null);
                if (errStr != "")
                {
                    return false;
                }
                if (App.obj_Vision.ScanMapPostions == null)
                {
                    return false;
                }
                HTuple hv_updateSnapMapX; HTuple hv_updateSnapMapY; HTuple hv_iFlag;
                if (App.obj_Vision.CheckPosModels.matchRegion == null || !App.obj_Vision.CheckPosModels.matchRegion.IsInitialized())
                    HOperatorSet.GetDomain(App.obj_Vision.Image, out App.obj_Vision.CheckPosModels.matchRegion);
                VisionMethon.update_map_points(App.obj_Vision.Image, App.obj_Vision.CheckPosModels.matchRegion, App.obj_Vision.CheckPosModels.modelType, App.obj_Vision.CheckPosModels.modelID,
                    App.obj_Vision.checkPosScoreThresh, App.obj_Vision.clipMapX, App.obj_Vision.clipMapY, App.obj_Vision.snapMapX, App.obj_Vision.snapMapY, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex],
                    out App.obj_Vision.hv_updateMapX, out App.obj_Vision.hv_updateMapY, out hv_updateSnapMapX, out hv_updateSnapMapY, out App.obj_Vision.hv_foundU, out App.obj_Vision.hv_foundV, out hv_iFlag);
                if (hv_iFlag.S != "")
                {
                    HTLog.Error("矫正点识别失败！" + hv_iFlag.S);
                    return false;
                }
                //清除之前的点位
                if (ThisScanPostions != null)
                {
                    ThisScanPostions.Clear();
                }
                else
                {
                    ThisScanPostions = new List<ImagePosition>();
                }
                ImagePosition imagePosition = new ImagePosition();
                imagePosition.z = App.obj_Pdt.ZFocus;
                for (int i = 0; i < App.obj_Vision.scanPosNum; i++)
                {
                    imagePosition.x = hv_updateSnapMapX.TupleSelect(i);
                    imagePosition.y = hv_updateSnapMapY.TupleSelect(i);
                    imagePosition.r = App.obj_Vision.snapMapRow[i];
                    imagePosition.c = App.obj_Vision.snapMapCol[i];
                    ThisScanPostions.Add(imagePosition);
                }
                //清除之前的点位
                if (App.obj_Vision.ThisClipPostions != null)
                {
                    App.obj_Vision.ThisClipPostions.Clear();
                }
                else
                {
                    App.obj_Vision.ThisClipPostions = new List<ImagePosition>();
                }
                for (int i = 0; i < App.obj_Vision.clipPosNum; i++)
                {
                    imagePosition.x = App.obj_Vision.hv_updateMapX.TupleSelect(i);
                    imagePosition.y = App.obj_Vision.hv_updateMapY.TupleSelect(i);
                    imagePosition.r = App.obj_Vision.ClipMapPostions[i].r;
                    imagePosition.c = App.obj_Vision.ClipMapPostions[i].c;
                    App.obj_Vision.ThisClipPostions.Add(imagePosition);
                }
                Directory.CreateDirectory(App.obj_Vision.frameFolder);
                HOperatorSet.WriteTuple(App.obj_Vision.hv_updateMapX, App.obj_Vision.frameFolder + "\\updateMapX.tup");
                HOperatorSet.WriteTuple(App.obj_Vision.hv_updateMapY, App.obj_Vision.frameFolder + "\\updateMapY.tup");
                HOperatorSet.GetImageSize(App.obj_Vision.Image, out App.obj_Vision.ImageWidth, out App.obj_Vision.ImageHeight);
                //double viewWidth = Math.Abs(App.obj_Vision.UV2XYResult[1].D * App.obj_Vision.ImageWidth.D);
                //double viewHeight = Math.Abs(App.obj_Vision.UV2XYResult[3].D * App.obj_Vision.ImageHeight.D);

                App.obj_Vision.dieMatrix = new ImagePosition[App.obj_Pdt.RowNumber, App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber];
                foreach (ImagePosition die in App.obj_Vision.ThisClipPostions)
                {
                    App.obj_Vision.dieMatrix[die.r, die.c] = die;
                }
                App.obj_Vision.ScanMatrix = new ImagePosition[App.obj_Vision.snapMapRow.TupleMax().I+1, App.obj_Vision.snapMapCol.TupleMax().I + 1];
                foreach (ImagePosition die in ThisScanPostions)
                {
                    App.obj_Vision.ScanMatrix[die.r, die.c] = die;
                }
                if (App.obj_Vision.showRegion != null) App.obj_Vision.showRegion.Dispose();
                HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, App.obj_Vision.hv_foundU, App.obj_Vision.hv_foundV, 512, 0);
                App.obj_Vision.ShowImage(hTWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
                //Thread.Sleep(1000);
                //App.obj_Vision.Image.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化线扫拍照点位
        /// </summary>
        public bool GetImagePositions_RowAuto(HTHalControl.HTWindowControl hTWindow)
        {
            if (App.obj_Vision.RunMode == 1) return true;

            if (App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex].Type == HTupleType.INTEGER)
            {
                HTUi.PopError("请先标定该相机！");
                return false;
            }
            if (!App.obj_Chuck.XYZ_Move(App.obj_Vision.checkPosX, App.obj_Vision.checkPosY, App.obj_Pdt.ZFocus))
            {
                HTUi.PopError("【检测台模块】" + App.obj_Chuck.GetLastErrorString());
                return false;
            }
            ////配置
            //bool ringEnable = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].RingLightEnable;
            //double ringTime = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].RingImageMeans;
            //bool coaxEnable1 = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].CoaxialLightEnable1;
            //double coaxTime1 = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].CoaxialImageMeans1;
            //bool coaxEnable2 = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].CoaxialLightEnable2;
            //double coaxTime2 = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].CoaxialImageMeans2;
            //double exposure = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].Exposure;
            //double gain = App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].Gain;
            ////配置
            //App.obj_Vision.obj_camera[App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].CameraName].SetExposure(exposure);
            //App.obj_Vision.obj_camera[App.obj_ImageInformSet[frmCaptureImage.Instance._selectIndex].CameraName].SetGain(gain);
            //App.obj_light.SetRingLight(ringEnable, ringTime);
            //App.obj_light.SetCoaxLight1(coaxEnable1, coaxTime1);
            //App.obj_light.SetCoaxLight2(coaxEnable2, coaxTime2);
            //触发
            App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();
            //4. 取图
            if (App.obj_Vision.Image != null) App.obj_Vision.Image.Dispose();
            HOperatorSet.GenEmptyObj(out App.obj_Vision.Image);
            string errStr = "";
            errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref App.obj_Vision.Image, 1, 5000);//获取图片
            //HOperatorSet.WriteImage(App.obj_Vision.Image, "tiff", 0, "D:\\alignImage.tiff");
            //FormJobs.Instance.ShowImage(Image,null);
            if (errStr != "")
            {
                return false;
            }
            if (App.obj_Vision.ScanMapPostions == null)
            {
                return false;
            }
            HTuple hv_updateSnapMapX; HTuple hv_updateSnapMapY; HTuple hv_iFlag;
            if (App.obj_Vision.CheckPosModels.matchRegion == null || !App.obj_Vision.CheckPosModels.matchRegion.IsInitialized())
                HOperatorSet.GetDomain(App.obj_Vision.Image, out App.obj_Vision.CheckPosModels.matchRegion);
            VisionMethon.update_map_points(App.obj_Vision.Image, App.obj_Vision.CheckPosModels.matchRegion, App.obj_Vision.CheckPosModels.modelType, App.obj_Vision.CheckPosModels.modelID,
                    App.obj_Vision.checkPosScoreThresh, App.obj_Vision.clipMapX, App.obj_Vision.clipMapY, App.obj_Vision.snapMapX, App.obj_Vision.snapMapY, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex],
                    out App.obj_Vision.hv_updateMapX, out App.obj_Vision.hv_updateMapY, out hv_updateSnapMapX, out hv_updateSnapMapY, out App.obj_Vision.hv_foundU, out App.obj_Vision.hv_foundV, out hv_iFlag);
            if (hv_iFlag.S != "")
            {
                HTLog.Error("矫正点矫正失败！" + hv_iFlag.S);
                return false;
            }
            //清除之前的点位
            if (ThisScanPostions != null)
            {
                ThisScanPostions.Clear();
                ThisMarkPostions.Clear();
            }
            else
            {
                ThisScanPostions = new List<ImagePosition>();
            }
            ImagePosition imagePosition = new ImagePosition();
            imagePosition.z = App.obj_Pdt.ZFocus;
            for (int i = 0; i < App.obj_Vision.scanPosNum; i++)
            {
                imagePosition.x = hv_updateSnapMapX.TupleSelect(i);
                imagePosition.y = hv_updateSnapMapY.TupleSelect(i);
                imagePosition.r = App.obj_Vision.snapMapRow[i];
                imagePosition.c = App.obj_Vision.snapMapCol[i];
                ThisScanPostions.Add(imagePosition);
            }
            //清除之前的点位
            if (App.obj_Vision.ThisClipPostions != null)
            {
                App.obj_Vision.ThisClipPostions.Clear();
            }
            else
            {
                App.obj_Vision.ThisClipPostions = new List<ImagePosition>();
            }
            for (int i = 0; i < App.obj_Vision.clipPosNum; i++)
            {
                imagePosition.x = App.obj_Vision.hv_updateMapX.TupleSelect(i);
                imagePosition.y = App.obj_Vision.hv_updateMapY.TupleSelect(i);
                imagePosition.r = App.obj_Vision.ClipMapPostions[i].r;
                imagePosition.c = App.obj_Vision.ClipMapPostions[i].c;
                App.obj_Vision.ThisClipPostions.Add(imagePosition);
            }
            Directory.CreateDirectory(App.obj_Vision.frameFolder);
            HOperatorSet.WriteTuple(App.obj_Vision.hv_updateMapX, App.obj_Vision.frameFolder + "\\updateMapX.tup");
            HOperatorSet.WriteTuple(App.obj_Vision.hv_updateMapY, App.obj_Vision.frameFolder + "\\updateMapY.tup");
            HOperatorSet.GetImageSize(App.obj_Vision.Image, out App.obj_Vision.ImageWidth, out App.obj_Vision.ImageHeight);
            double viewWidth = Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D * App.obj_Vision.ImageWidth.D);
            double viewHeight = Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][3].D * App.obj_Vision.ImageHeight.D);

            App.obj_Vision.dieMatrix = new ImagePosition[App.obj_Pdt.RowNumber, App.obj_Pdt.BlockNumber*App.obj_Pdt.ColumnNumber];
            foreach (ImagePosition die in App.obj_Vision.ThisClipPostions)
            {
                App.obj_Vision.dieMatrix[die.r, die.c] = die;
            }
            App.obj_Vision.ScanMatrix = new ImagePosition[App.obj_Vision.snapMapRow.TupleMax().I + 1, App.obj_Vision.snapMapCol.TupleMax().I + 1];
            foreach (ImagePosition die in ThisScanPostions)
            {
                App.obj_Vision.ScanMatrix[die.r, die.c] = die;
            }
            App.obj_MobSht.InitialScanRowParameters(ThisScanPostions, viewWidth,
                viewHeight, App.obj_Vision.hv_dieWidth.D, App.obj_Vision.hv_dieHeight.D, App.obj_Vision.scanRowNum, App.obj_Vision.scanColNum);
            App.obj_Vision.scanIniConfig.WriteInteger("MobileShooting", "LineAxisTrigCount", App.obj_MobSht.LineAxisTrigCount);
            App.obj_Vision.scanIniConfig.WriteInteger("MobileShooting", "NoLineAxisMoveCount", App.obj_MobSht.NoLineAxisMoveCount);
            if (App.obj_Vision.showRegion != null) App.obj_Vision.showRegion.Dispose();
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, App.obj_Vision.hv_foundU, App.obj_Vision.hv_foundV, 512, 0);
            App.obj_Vision.ShowImage(hTWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
            //Thread.Sleep(2000);
            App.obj_Vision.Image.Dispose();
            return true;
        }
        /// <summary>
        /// 3D线扫
        /// </summary>
        /// <param name="CamIdx">相机</param>
        /// <param name="Fps">帧率</param>
        /// <param name="Start">触发起点</param>
        /// <param name="End">触发终点</param>
        /// <param name="Interval">触发间隔</param>
        /// <param name="Z_TrigPos">所有触发点位</param>
        /// <param name="ho_SharpIamge">所有获取到的图</param>
        public void Auto3DFocus(int CamIdx, int Fps, double Start, double End, double Interval, out HTuple Z_TrigPos, out HObject ho_SharpIamge)
        {
            try
            {
                string errstr = "-1";
                App.obj_Chuck.z_TrigStart = Start;
                App.obj_Chuck.z_TrigEnd = End;
                App.obj_Chuck.z_TrigStep = Interval;
                App.obj_Chuck.Fps = Fps;
                Z_TrigPos = null;
                ho_SharpIamge = null;

                //if (_selectIndex + 1 > App.obj_ImageInformSet.Count)
                //{
                //    throw new Exception("请先保存图像设置");
                //}

                //App.obj_light.SetRingLight(true, App.obj_ImageInformSet[0].RingImageMeans);
                //App.obj_light.SetCoaxLight1(false, 0);

                for (int i = 0; i < 1; i++)
                {
                    if (errstr == "")
                    {
                        break;
                    }
                    Z_TrigPos = null;
                    if (ho_SharpIamge != null)
                    {
                        if (ho_SharpIamge.IsInitialized())
                        {
                            ho_SharpIamge.Dispose();
                        }
                    }
                    errstr = App.obj_Operations.Z_LineTrigger(CamIdx, out Z_TrigPos, out ho_SharpIamge);
                }
                if (errstr != "")
                {
                    throw new Exception("3D自动聚焦失败\n" + errstr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("3D自动聚焦出错\n" + ex.ToString());
            }
        }
    }
}