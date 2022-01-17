#define DemoMode  //定义则代表整机离线模式
//#define CameraOnline //定义则表示相机在整机离线模式中可在线

using System;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using HT_Lib;
using MobileShooting;
using IniDll;
using System.Collections.Concurrent;
using HalconDotNet;
using SPort_Driver;
namespace LeadframeAOI
{

    enum SystemRunMode
    {
        MODE_ONLINE = 0,
        MODE_OFFLINE = 1,
    }
    static class App
    {
        #region 路径
        const bool use_current_dir = false;

        public static string currentDir = Directory.GetCurrentDirectory();

        public static string programDir = (use_current_dir ? Environment.CurrentDirectory : "D:\\ht'tech") + "\\AOI_LF";
        public static string LogFile { get { return ("D:\\Log"); } }
        public static string UserFile { get { return (programDir + "\\User\\User.db"); } }
        public static string SystemDir { get { return (programDir + "\\System"); } }
        public static string SystemUV2XYDir { get { return (programDir + "\\System\\UV2XY"); } }
        public static string SystemUV2XYResultFile { get { return (programDir + "\\System\\UV2XY" + "\\" + "UV2XY" + ".dat"); } }
        public static string BspParaFile { get { return (programDir + "\\System\\htm_config.db"); } }
        public static string SystemMatFile { get { return (programDir + "\\System\\matUV2XY.tup"); } }
        public static string SystemConfig { get { return (programDir + "\\SystemConfig"); } }
        public static string ProductDir { get { return ("D:\\Products"); } }
        public static string ProductPath { get { return ProductDir + "\\" + ProductMagzine.ActivePdt; } }
        public static string ModelPath { get { return ProductPath + "\\Models\\"; } }
        public static string AlgParamsPath { get { return ProductPath + "\\AlgParams.ini"; } }
        public static string Fuc_LibPath { get { return Application.StartupPath + "\\VisionFlow_Funclib.dll"; } }
        public static string Json_Path { get { return ProductPath + "\\AlgorithmConfig.json"; } }
        public static string SystemParaFile { get { return (programDir + "\\System\\mechanism_paras.db"); } }
        public static string EnhanLightPath { get { return ("LightCtrl_0310.exe"); } }
        #endregion

        #region 各个功能模块
        public static IniFiles formIniConfig = new IniFiles(Application.StartupPath + "\\FromData.ini");
        public static AlgApp obj_AlgApp = new AlgApp();

        public static MobileShootingModule obj_MobSht = new MobileShootingModule(SystemParaFile, "MobileShoot");
        public static ProductMagzine obj_Pdt = new ProductMagzine("", "pdt");
        public static LoadModule obj_Load = new LoadModule(SystemParaFile, "Load");
        public static UnLoadModule obj_Unload = new UnLoadModule(SystemParaFile, "Unload");
        public static ChuckModule obj_Chuck = new ChuckModule(SystemParaFile, "Chuck");
        public static Process obj_Process = new Process();
        public static Vision obj_Vision = new Vision(SystemParaFile, "Vision");
        public static Light obj_light = new Light("", "light");
        public static SystemConfig obj_SystemConfig = new SystemConfig(SystemConfig + "\\SystemConfig.db", "SystemConfig");
        public static ReadQRCode obj_ReadQRCode;//= new ReadQRCode("COM5");
        public static MainModule obj_Main = new MainModule();
        public static Operations obj_Operations = new Operations(SystemParaFile, "CalibrationPara");
        //   public static JSInspection obj_JSInspection = new JSInspection();
        public static ExternalDevices obj_ExternalDevices = new ExternalDevices(SystemParaFile, "DevicesPara");
        public static BrightnessCalibration obj_BrightnessCalib = new BrightnessCalibration(SystemParaFile, "Brightness");

        //public static htWindow.MainWindow mainWindow = null;

        //public static ImageInformation obj_ImageInfo = new ImageInformation(SystemParaFile, "TestImage");
        public static HTUi.MainUi mainWin = null;
        public static BindingList<ImageInformation> obj_ImageInformSet = new BindingList<ImageInformation>();
        /// <summary>单通道增亮光源</summary>
        public static EnHance_Light enHance_Light = new EnHance_Light("COM6");
        /// <summary>多通道增亮光源</summary>
        public static EnHance_Light enHance_LightEx = new EnHance_Light("COM7");
        public static EC_JET ec_Jet = new EC_JET("COM3");
        public static BoxManager boxManager = new BoxManager();
        #endregion

        /// <summary>
        /// 卸载数据
        /// </summary>
        private static void _UnloadData()
        {
            //窗口关闭

            //1.先关闭各个任务线程
            obj_Process.EndTasks();
            obj_Load.EnableAxisZStoper(true);
            obj_Unload.EnableAxisZStoper(false);
            if (obj_Vision.RunMode == 0) obj_Vision.CloseAllCamera();
            //2.释放资源
            enHance_Light.Close();
            obj_AlgApp.DisposeNet();
            //3.关闭HTM
            HTM.Discard();
            return;
        }

        /// <summary>
        /// 配置模块模式
        /// </summary>
        public static void SetSystemMode()
        {
            obj_Load.RunMode = obj_SystemConfig.systemRunMode;
            obj_Unload.RunMode = obj_SystemConfig.systemRunMode;
            obj_Chuck.RunMode = obj_SystemConfig.systemRunMode;
            obj_Vision.RunMode = obj_SystemConfig.systemRunMode;
#if CameraOnline
            obj_Vision.RunMode = 0;
#endif
            obj_light.RunMode = obj_SystemConfig.systemRunMode;
        }
        /// <summary>
        /// 初始化系统数据
        /// </summary>
        /// <returns></returns>
        private static void _InitSystem()
        {
            try
            {
                //EnHanceLight ehl = new EnHanceLight("Com4");
                //ehl.ChangeTriggerTime(1000);
                int progress = 0;
                bool ret = false;
                int err = -1;

                #region 数据初始化
                mainWin.UpdateProgressBar(progress, "初始化产品路径...");
                obj_Pdt.Initialize(ProductDir);
                string activePdtName = "";
                obj_Pdt.ReadActivePdtName(out activePdtName);
                mainWin.UpdateProgressBar(progress, string.Format("当前使能产品:{0}", activePdtName));

                obj_Pdt.ConfigParaFile(ProductDir + @"\" + activePdtName + @"\ParaPdt.db");
                mainWin.UpdateProgressBar(progress, "初始化产品路径成功!");
                mainWin.UpdateProgressBar(progress, "初始化系统配置...");
                obj_SystemConfig.SystemConfigInit();
                boxManager.Load();
                mainWin.UpdateProgressBar(progress, "初始化系统配置成功！");

                mainWin.UpdateProgressBar(progress, "初始化光源配置...");
                obj_light.ConfigParaFile(ProductDir + @"\" + activePdtName + @"\ParaPdt.db", "light");
                mainWin.UpdateProgressBar(progress, "初始化光源配置成功");
                mainWin.UpdateProgressBar(progress, "初始化主控模块...");
                obj_Main.Initilaize(ltGreen: 30, ltRed: 31, ltYellow: 32, Buzzer: 33, btnStart: 34, btnStop: 35, btnReset: 36, MuteON: false);
                #endregion

                progress = 20;
                mainWin.UpdateProgressBar(progress, "初始化主控模块成功！");
                #region 加载各个模块参数
                mainWin.UpdateProgressBar(progress, "加载各个模块参数...");
                if (!obj_Load.Read())
                {
                    HTUi.PopError(obj_Load.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_Load.GetLastErrorString()));
                    goto _end;
                }
                if (!obj_Unload.Read())
                {
                    HTUi.PopError(obj_Unload.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_Unload.GetLastErrorString()));
                    goto _end;
                }
                if (!obj_Chuck.Read())
                {
                    HTUi.PopError(obj_Chuck.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_Chuck.GetLastErrorString()));
                    goto _end;
                }
                if (!obj_SystemConfig.Read())
                {
                    HTUi.PopError(obj_SystemConfig.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_SystemConfig.GetLastErrorString()));
                    goto _end;
                }
                if (!obj_light.Read())
                {
                    HTUi.PopError(obj_light.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_light.GetLastErrorString()));
                    goto _end;
                }
                if (!obj_ExternalDevices.Read())
                {
                    HTUi.PopError(obj_ExternalDevices.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_ExternalDevices.GetLastErrorString()));
                    goto _end;
                }
                if (!obj_Pdt.Read())
                {
                    HTUi.PopError(obj_Pdt.GetLastErrorString());
                    mainWin.UpdateProgressBar(progress, string.Format(obj_Pdt.GetLastErrorString()));
                    goto _end;
                }

                #region load轴IO初始化
                Device_LoadUnload dev_Load = new Device_LoadUnload()
                {
                    //轴
                    a_Y = 7,
                    a_Z = 6,
                    a_JawZ = 10,
                    a_Y_Btm = 8,
                    a_X_Push = 9,
                    a_JawX = 3,
                    a_JawFrame = 16,

                    //IO
                    i_Jaw_Closed = 37,
                    i_Mgz_Full_TOP = 39,
                    i_Mgz_Full_BTM = 40,
                    o_Z_Stopping = 42,
                    i_Jaw_HaveMgz = 44,
                    i_Transfer_HaveCst = 14,
                    i_Chuck_HaveCst = 5,

                    i_HaveCst = 43,
                    i_PushCst_Err = 38,

                    i_Transfer_JawCylinder_Down = 9,
                    i_Transfer_JawCylinder_Up = 8,

                    o_Transfer_JawCylinder = 21,


                };
                #endregion
                #region Unload轴IO初始化
                Device_LoadUnload dev_Unload = new Device_LoadUnload()
                {
                    //轴
                    a_Y = 11,
                    a_Z = 12,
                    a_JawZ = 13,
                    a_Y_Btm = 14,
                    a_JawX = 4,
                    a_JawFrame = 15,

                    //IO
                    i_Jaw_Closed = 45,
                    i_Mgz_Full_TOP = 46,
                    i_Mgz_Full_BTM = 47,
                    o_Z_Stopping = 49,
                    i_Jaw_HaveMgz = 53,
                    i_Transfer_HaveCst = 15,
                    i_Chuck_HaveCst = 6,
                    i_PushCst_Err = 7,

                    i_Jaw_NgMgz_Closed = 50,
                    i_Jaw_NgMgz_Opened = 51,
                    o_Jaw_NgMgz_Close = 52,
                    i_NgMgz_Side_ON = 54,
                    i_NgMgz_Btm_ON = 55,
                    i_Transfer_JawCylinder_Down = 11,
                    i_Transfer_JawCylinder_Up = 10,
                    o_Transfer_JawCylinder = 22,
                    i_Unload_PushCstLiftCylinderUp = 12,
                    i_Unload_PushCstLiftCylinderDown = 13,
                    o_Unload_PushCstLiftCylinder = 23, ///////////

                    i_MarkCylinderUp = 19,
                    i_MarkCylinderDown = 20,
                    o_MarkCylinder = 26,
                    i_FrameBendChecker = 56
                };

                #endregion
                #region chuck轴IO初始化
                Device_Chuck dev_Chuck = new Device_Chuck()
                {
                    #region 轴
                    /// <summary>
                    /// 相机X轴
                    /// </summary>
                    a_X = 0,
                    /// <summary>
                    /// 相机Y轴
                    /// </summary>
                    a_Y = 1,
                    /// <summary>
                    /// 相机Z轴
                    /// </summary>
                    a_Z = 2,
                    /// <summary>
                    /// 导轨间距Y轴 
                    /// </summary>
                    a_Y_Track = 5,
                    #endregion

                    #region  IO
                    /// <summary>
                    /// 压板前上信号
                    /// </summary>
                    i_PlateFrontUp = 0,
                    /// <summary>
                    /// 压板前下信号
                    /// </summary>
                    i_PlateFrontDown = 1,
                    /// <summary>
                    /// 压板后上信号
                    /// </summary>
                    i_PlateBehindUp = 2,
                    /// <summary>
                    /// 压板后下信号
                    /// </summary>
                    i_PlateBehindDown = 3,
                    /// <summary>
                    /// 压板气缸  
                    /// </summary>
                    o_Plate = 24,
                    /// <summary>
                    /// 真空控制开关
                    /// </summary>
                    o_Vacum = 27,
                    /// <summary>
                    /// 吹气开关
                    /// </summary>
                    o_Blow = 28,
                    /// <summary>
                    /// 真空信号
                    /// </summary>         
                    i_Vacum = 4,
                    /// <summary>
                    /// 基板载台气缸开关
                    /// </summary>
                    o_Base = 25,
                    /// <summary>
                    /// 基板载台气缸上信号
                    /// </summary>
                    i_BaseUp = 16,
                    /// <summary>
                    /// 基板载台气缸下信号
                    /// </summary>
                    i_BaseDown = 17,
                    /// <summary>喷印器墨水信号，1有0无 </summary>
                    i_HaveInk = 58,
                    /// <summary>喷印器运行中信号,1运行，0待机 </summary>
                    i_JetRun = 59
                    #endregion
                };
                #endregion


                obj_Load.Initialize(dev_Load);
                obj_Unload.Initialize(dev_Unload);
                obj_Chuck.Initialize(dev_Chuck);
                obj_light.initial(2, 3, 1);//设置同轴光、环形光和位置触发板号

                //同步外部配置文件的运行模式到系统配置
                //从系统配置中同步系统运行模式到各个模块  //0-离线 1-demo 2-online
                int DemoMode = 0;
#if DemoMode
                DemoMode = 1;
#endif

                App.obj_SystemConfig.systemRunMode = DemoMode;

                SetSystemMode();
                #endregion
                progress = 30;
                mainWin.UpdateProgressBar(progress, "加载各个模块参数成功!");

                #region 采图配置模块
                App.obj_Pdt.LoadPdtData();
                #endregion
                progress = 40;
                #region 设备初始化

                mainWin.UpdateProgressBar(progress, "启动处理流程任务...");
                obj_Process.CreateTasks();
                mainWin.UpdateProgressBar(progress, "启动处理流程任务成功！");

                mainWin.UpdateProgressBar(progress, "初始化运动控制服务...");

                HTM.INIT_PARA _init = new HTM.INIT_PARA();
                _init.para_file = BspParaFile;
                _init.use_htnet_card = 1;
                _init.use_aps_card = 0;
                _init.offline_mode = (byte)(DemoMode == 0 ? 0 : 1);  //工作模式
                _init.max_axis_num = 17;//15;
                if (obj_SystemConfig.LoadManually)//手动上下料模式
                {
                    if (obj_SystemConfig.JawCatchMode == 1)//气缸模式
                        _init.max_axis_num = 6;//15;
                    else//电机模式
                        _init.max_axis_num = 17;
                }
                else//自动上下料模式
                {
                    if (obj_SystemConfig.JawCatchMode == 1)//气缸模式
                        _init.max_axis_num = 15;//15;
                    else//电机模式
                        _init.max_axis_num = 17;
                }
                _init.max_io_num = 60;// 57;
                _init.max_dev_num = 5;// 3;
                _init.language = 0;          //语言 0-简体中文，1-英语（需有语言包） 
                err = HTM.Init(ref _init);
                if (err != 0)
                {
                    HTUi.PopError(string.Format("初始化运动控制服务失败! Code = {0}", err));
                    mainWin.UpdateProgressBar(progress, string.Format("初始化运动控制服务失败! Code = {0}", err));
                    goto _end;
                }

                progress = 80;
                mainWin.UpdateProgressBar(progress, "初始化运动控制服务成功!");
                ///相机初始化
                obj_Vision.Initialize();
                progress = 90;
                mainWin.UpdateProgressBar(progress, "打开相机模板模块成功!");
                #endregion
                #region SetupUI
                FormJobs.Instance.SetupUI();
                frmLoadModules.Instance.SetupUI();
                frmUnloadModules.Instance.SetupUI();
                frmChuckModules.Instance.SetupUI();
                FormExternalDevices.Instance.SetupUI();
                frmProduct.Instance.SetupUI();
                frmCaptureImage.Instance.SetupUI();
                //ParaTest.Instance.SetupUI();
                frmSystemConfig.Instance.SetupUI();
                FormUV2XY.Instance.SetupUI();
                FrmAutoMapping.Instance.SetupUI();
                FrmInspectPara.Instance.SetupUI();
                //FrmScanTest.Instance.SetupUI();
                FunctionTest.Instance.SetupUI();
                #endregion
                progress = 100;
                mainWin.UpdateProgressBar(progress, "UI初始化成功!");
                App.obj_Pdt.ConfigPdtData();

                Thread.Sleep(500);
                #region  记录信息到日志
                HTLog.Debug("*****************************[SoftWare Start]******************************");
                HTLog.Debug(String.Format("当前模式：{0}", DemoMode == 1 ? "离线模式" : "在线模式"));
                HTLog.Debug(String.Format("产品名：{0}", ProductMagzine.ActivePdt));
                FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:待机"; }));
                #endregion

                ret = true;
            _end:
                if (ret == false)
                {
                    HTM.Discard();
                    obj_Vision.CloseAllCamera();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序遇到错误意外终止,原因如下\r\n\r\n" + ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _UnloadData();
            }
        }
        /// <summary>
        /// 意外异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                HTLog.Error(ex.ToString());
            }

            MessageBox.Show(ex.ToString());
        }
        /// <summary>
        /// 意外异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            if (ex != null)
            {
                HTLog.Error(ex.ToString());
            }

            MessageBox.Show(ex.ToString());
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                //初始化系统数据
                mainWin = new HTUi.MainUi();
                //配置HTLog 如果要使用HTLog.Ui，必须在加载Ui之前配置Log文件所在的文件夹
                //HTLog.Configs.Console = false;
                HTLog.Configs.Folder = LogFile;

                //绑定外部事件，即每次使用HTLog记录信息都会调用该函数。
                HTLog.LogMessageEvent += mainWin.AddMessage;
                //启动HTLog服务，服务会异步将信息记录到本地。
                HTLog.StartService();

                //配置界面，可以通过xml文件或者 配置用的类的实例来配置
                mainWin.SetupUi(@"SetupUi.xml");

                //配置用户数据文件，也可以在上面setup.xml中配置
                mainWin.UiConfig.User.DataFile = UserFile;
                //绑定事件
                mainWin.MainUiLoadEvent += _InitSystem;
                mainWin.MainUiCloseEvent += _UnloadData;
                mainWin.Text = "景焱直线AOI控制与检测软件" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                mainWin.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                //mainWin.FormBorderStyle = FormBorderStyle.None;
                mainWin.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
                mainWin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);


                //  for(int i=0; i<2; i++)
                //mainWin.GetTabForm(0, 3).SetupUi();

                //测试UI的消息记录框添加信息
                //mainWin.AddMessage("DD1{0}", 123);
                //mainWin.AddMessage("DD2_{0}", Math.PI);
                //mainWin.AddMessage("DD3");
                //mainWin.AddMessage("DD4");
                //mainWin.AddMessage("DD5");


                Application.Run(mainWin);


                //关闭HTLog服务
                HTLog.StopService();
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序遇到错误意外终止,原因如下\r\n\r\n" + ex.ToString() + "\n" + ex.TargetSite, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}