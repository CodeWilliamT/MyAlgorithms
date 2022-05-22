/****************************************************************************/
/*  File Name   :   HTM_Bsp_Lib.cs                                          */
/*  Brief       :   Motion APIs(x86/x64)                                    */
/*  Verion      :   3.15                                                    */
/*  Date        :   2017/12/25                                              */
/*  Author      :   Tongqing CHEN	                                        */
/****************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace HTM_BSP
{
    #region 枚举类型
    /// <summary>BSP所有枚举类型数据列表，与C语言lib中的宏定义一一对应</summary>
    public enum HTM_Define
    {
        #region 板卡类型
        /// <summary>ADLINK 204/208C Dirve Card</summary>
        HTM_CARD_AMP208C = (0),
        /// <summary>HT Bond Dirve Card</summary>
        HTM_CARD_HTBOND = (1),
        /// <summary>HT DC Motor Dirve Card</summary>
        HTM_CARD_HTDCMD = (2),
        /// <summary>HT DHVD Dirve Card</summary>
        HTM_CARD_HTDHVD = (3),
        /// <summary>HT DLVD Dirve Card</summary>
        HTM_CARD_HTDLVD = (4),
        /// <summary>HT 202C Motion Card</summary>
        HTM_CARD_HT202C = (5),
        /// <summary>HT DHVD pdo-type Dirve Card</summary>
        HTM_CARD_HTDHVD_PDO = (6),
        /// <summary>HT DHVD pdo-type Dirve Card</summary>
        HTM_CARD_HB_DISPENSE = (7),
        /// <summary>Other Dirve Card</summary>
        HTM_CARD_OTHER = (255),
        #endregion

        #region 轴类型 
        /// <summary>Servo Motor伺服</summary>
        HTM_AXIS_SERVO = (0),
        /// <summary>Step Motor步进</summary>
        HTM_AXIS_STEP = (1),
        /// <summary>Line Motor直线</summary>
        HTM_AXIS_LINE = (2),
        /// <summary> Voice Motor音圈 </summary>
        HTM_AXIS_VOICE = (3),
        /// <summary>Torque Motor力矩</summary>
        HTM_AXIS_TORQUE = (4),
        /// <summary>DC Motor直流</summary>
        HTM_AXIS_DC = (5),
        #endregion

        #region  运动标识参数 
        /// <summary>绝对运动模式</summary>
        HTM_ABS_MOVE = (1),
        /// <summary>S型曲线模式</summary>
        HTM_S_MOVE = (2),
        /// <summary>力矩运动模式</summary>
        HTM_TQ_MOVE = (4),
        /// <summary>缓存式运动</summary>
        HTM_BUF_MOVE = (8),
        /// <summary>绝对运动S曲线模式</summary>
        HTM_AS_MOVE = HTM_ABS_MOVE | HTM_S_MOVE,
        /// <summary>相对运动S曲线模式</summary>
        HTM_RS_MOVE = HTM_S_MOVE,
        /// <summary>绝对运动T曲线模式</summary>
        HTM_AT_MOVE = HTM_ABS_MOVE,
        /// <summary>相对对运动T曲线模式</summary>
        HTM_RT_MOVE = 0,
        #endregion

        #region  stop mode 
        /// <summary>立即停</summary>
        HTM_STOP_EMG = (0),
        /// <summary>减速停</summary>
        HTM_STOP_DEC = (1),
        /// <summary>正限位信号停</summary>
        HTM_STOP_PEL = (2),
        /// <summary>负限位信号停</summary>
        HTM_STOP_MEL = (3),
        /// <summary>IO0信号停</summary>
        HTM_STOP_IO0 = (4),
        /// <summary>IO1信号停</summary>
        HTM_STOP_IO1 = (5),
        /// <summary>IO2信号停</summary>
        HTM_STOP_IO2 = (6),
        /// <summary>IO3信号停</summary>
        HTM_STOP_IO3 = (7),
        #endregion

        #region  motion io status bit number 
        /// <summary>报警IO位</summary>
        HTM_MIO_ALM = (0),
        /// <summary>正限IO位</summary>
        HTM_MIO_PEL = (1),
        /// <summary>负限IO位</summary>
        HTM_MIO_MEL = (2),
        /// <summary>原点IO位</summary>
        HTM_MIO_ORG = (3),
        /// <summary>Z相信号IO位</summary>
        HTM_MIO_IDX = (4),
        /// <summary>到位IO位</summary>
        HTM_MIO_INP = (6),
        /// <summary>励磁IO位</summary>
        HTM_MIO_SVON = (7),
        /// <summary>软正限IO位</summary>
        HTM_MIO_SPEL = (11),
        /// <summary>软负限IO位</summary>
        HTM_MIO_SMEL = (12),
        /// <summary>直流电机IO0</summary>
        HTM_MIO_IO0 = (13),
        /// <summary>直流电机IO1</summary>
        HTM_MIO_IO1 = (14),
        /// <summary>直流电机IO2</summary>
        HTM_MIO_IO2 = (15),
        /// <summary>直流电机IO3</summary>
        HTM_MIO_IO3 = (16),
        #endregion

        #region  设备类型 
        /// <summary>HT Pos Trigger Card</summary>
        HTM_DEV_POSTRIG = (0),
        /// <summary>HT DHVD Dirve Card</summary>
        HTM_DEV_HTDHVD = (1),
        /// <summary>HT Light Source Drive</summary>
        HTM_DEV_LTSRC = (2),
        /// <summary>Serial Com</summary>
        HTM_DEV_COM = (3),
        /// <summary>Other device</summary>
        HTM_DEV_OTHER = (255),
        #endregion

        #region 其它
        /// <summary>点表触发模式最大点个数</summary>
        MAX_TRIGPT_NUM = (4)
        #endregion

    }
    #endregion

    #region BSP中所有结构定义  
    /// <summary>用于初始化BSP的结构信息，包含参数文件路径，板卡类型，轴/io/设备数，语言等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct INIT_PARA
    {
        /// <summary>参数文件路径，如该项为空则默认为工程目录下的htm_config.db文件</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String config_file;
        /// <summary>是否使用凌华板卡, 1-使用，0-不使用</summary>
        public Byte use_aps_card;
        /// <summary>是否使用HTNET板卡</summary>
        public Byte use_htnet_card;
        /// <summary>模式，1-脱机模式，0-在线模式(初始化板卡+配置)，2-仅初始化板卡不配置参数</summary>
        public Byte offline_mode;
        /// <summary>最大轴数</summary>
        public UInt16 max_axis_num;
        /// <summary>最大IO数</summary>
        public UInt16 max_io_num;
        /// <summary>其它设备数</summary>
        public UInt16 max_dev_num;
        /// <summary>模块数</summary>
        public UInt16 max_module_num;
        /// <summary>语言(UI相关)0-CHN, 1-ENG</summary>
        public Byte language;
    }
    /// <summary>运动参数结构，包含速度加速度等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MOTION_PARA
    {
        /// <summary>起始速度(mm/s)</summary>
        public Double vStart;
        /// <summary>最大速度(mm/s)</summary>
        public Double vMax;
        /// <summary>结束速度(mm/s)</summary>
        public Double vEnd;
        /// <summary>加速度(mm/s2)</summary>
        public Double acc;
        /// <summary>减速度(mm/s2)</summary>
        public Double dec;
        /// <summary>s曲线因子(0~1.0)</summary>
        public Double sFactor;
        /// <summary>超时时间(s)</summary>
        public Double timeout;
    }
    /// <summary>轴信息结构，包含轴类型、地址、驱动信息、运动信息等。</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AXIS_INFO
    {
        /// <summary>Axis Name</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String axisName;
        /// <summary>Drive Card Type</summary>
        public Byte driveType;
        /// <summary>Axis type</summary>
        public Byte axisType;
        /// <summary>Card/Bus Number 卡号/总线号</summary>
        public UInt16 busNo;
        /// <summary>Axis/Node Address 轴号/节点号</summary>
        public UInt16 nodeAddr;
        /// <summary>Port Number 端口号</summary>
        public UInt16 portNo;
        /// <summary>脉冲当量(pulse/mm)</summary>
        public Double pulseFactor;
        /// <summary>是否有编码器反馈</summary>
        public Byte extEncode;
        /// <summary>运动参数</summary>
        public MOTION_PARA mp;
        /// <summary>负电子限位使能，0-不使能, 1-使能</summary>
        public Byte enableMEL;
        /// <summary>正电子限位使能，0-不使能, 1-使能</summary>
        public Byte enablePEL;
        /// <summary>负软限位使能，0-不使能, 1-使能 </summary>
        public Byte enableSMEL;
        /// <summary>正软限位使能，0-不使能, 1-使能</summary>
        public Byte enableSPEL;
        /// <summary>正软限位位置(mm)</summary>
        public Double sPELPos;
        /// <summary>负软限位位置(mm)</summary>
        public Double sMELPos;

        /// <summary>回零模式</summary>
        public SByte homeMode;
        /// <summary>回零方向</summary>
        public Byte homeDir;
        /// <summary>EZ信号使能</summary>
        public Byte homeEZA;
        //  public Double homeSFactor;  /*!< 回零S因子(0~1.0) no use after version 3.10 */
        /// <summary>DHVD硬限位回零电流限幅设置</summary>
        public UInt32 hdStpCrt;
        /// <summary>DHVD硬限位回零时间设置</summary>
        public UInt32 hdStpTime;
        /// <summary>回零加速度(mm/s2)</summary>
        public Double homeAcc;
        /// <summary>回零起始速度(mm/s)</summary>
        public Double homeVs;
        /// <summary>回零最大速度(mm/s)</summary>
        public Double homeVm;
        /// <summary>回零原点速度(mm/s)</summary>
        public Double homeVo;
        /// <summary>回零偏移量(mm)回零后显示的位置</summary>
        public Double homeShift;
        /// <summary>回零超时时间(s)</summary>
        public float homeTimeout;

        #region Paras only for 208C
        /// <summary>报警信号逻辑</summary>
        public Byte almLogic;
        /// <summary>限位信号逻辑</summary>
        public Byte elLogic;
        /// <summary>原点信号逻辑</summary>
        public Byte orgLogic;
        /// <summary>EZ信号逻辑</summary>
        public Byte ezLogic;
        /// <summary>到位信号逻辑</summary>
        public Byte inpLogic;
        /// <summary>励磁信号逻辑</summary>
        public Byte servoLogic;
        /// <summary>输出信号逻辑</summary>
        public Byte optMode;
        /// <summary>输入信号逻辑</summary>
        public Byte iptMode;
        /// <summary>编码器方向</summary>
        public Byte encodeDir;
        #endregion

        /// <summary>最大电流(A)仅对htnet-DCMC驱动板有效</summary>
        public Double maxCurrent;
        /// <summary>位置极性(0-默认,1-取反)</summary>
        public Byte polarity;
        /// <summary>位置误差(mm)</summary>
        public Double pError;
        /// <summary>位置误差超时(ms)</summary>
        public UInt16 peTimeout;
        /// <summary>到位窗口(mm)</summary>
        public Double inpValue;
        /// <summary>到位窗口时间(ms)</summary>
        public UInt16 inpInterval;

        /// <summary>快速停止减速度(m/s2), 该参数对HTM_EStop急停接口有效</summary>
        public Double stopDec;
    }
    /// <summary>轴状态结构，包含轴的状态信息，如报警、限位、位置等。</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AXIS_STATUS
    {
        /// <summary>报警信号(0/1)</summary>
        public Byte alm;
        /// <summary>正限位信号(0/1)</summary>
        public Byte pel;
        /// <summary>负限位信号(0/1)</summary>
        public Byte mel;
        /// <summary>原点信号(0/1)</summary>
        public Byte org;
        /// <summary>index信号(0/1)</summary>
        public Byte idx;
        /// <summary>INP信号(0/1)</summary>
        public Byte inp;
        /// <summary>励磁信号(0/1)</summary>
        public Byte svon;
        /// <summary>指令位置(mm)</summary>
        public Double cmdPos;
        /// <summary>反馈信号(mm)</summary>
        public Double fbkPos;
    }
    /// <summary>IO信息结构，包含IO设备的信息，如地址、方向、极性等</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IO_INFO
    {
        /// <summary>IO名称(不超过32个字节)</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String ioName;
        /// <summary>总线号</summary>
        public UInt16 busNo;
        /// <summary>节点号</summary>
        public UInt16 nodeAddr;
        /// <summary>端口号</summary>
        public UInt16 portNo;
        /// <summary>IO方向(0-输入,1-输出)</summary>
        public Byte ioSrc;
        /// <summary>极性(0-默认,1-取反)</summary>
        public Byte polarity;
        /// <summary>禁用(0-不禁用,1-禁用)禁用后将不能正常操作该设备</summary>
        public Byte disable;
    }
    /// <summary>点表触发模式配置，包含触发点的位置、方向、使能等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TRIG_POINT
    {
        /// <summary>Position 触发位置</summary>
        public Double pos;
        /// <summary>Direction触发方向</summary>
        public Byte dir;
        /// <summary>使能0-不使能，1-使能</summary>
        public Byte enable;
    }
    /// <summary>线性触发模式配置, 包含触发的起点、终点、间隔等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TRIG_LINEAR
    {
        /// <summary>线性触发起始位置(mm)</summary>
        public Double startPos;
        /// <summary>线性触发结束位置(mm)</summary>
        public Double endPos;
        /// <summary>线性触发间隔(mm)</summary>
        public Double interval;
    }
    /// <summary>位置触发板信息，包含对应的轴、触发模式等</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POS_TRIG_PARA
    {
        /// <summary>对应轴</summary>
        public Byte axisIdx;
        /// <summary>触发模式 0点表1线性</summary>
        public Byte trigMode;
        /// <summary>取反(0-默认，1-取反)</summary>
        public Byte polarity;
        /// <summary>点表模式触发位置(mm)</summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = (int)HTM_Define.MAX_TRIGPT_NUM, ArraySubType = UnmanagedType.Struct)]
        public TRIG_POINT[] trigPt;
        /// <summary>线性触发使能</summary>
        public Byte lineEnable;
        /// <summary>线性触发配置</summary>
        public TRIG_LINEAR trigLin;
        /// <summary>相机触发超前时间(us)</summary>
        public Double ffTime;
    }
    /// <summary>光源驱动板信息，包含触发源、触发时间等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LIGHT_DIRVE_PARA
    {
        /// <summary>触发源</summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte[] tirgSrc;
        /// <summary>脉冲宽度时间us</summary>
        public Double opTime;
    }
    /// <summary>串口信息，包含串口号、波特率等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COM_PARA
    {
        /// <summary>串口号</summary>
        public Int32 comNo;
        /// <summary>波特率</summary>
        public Int32 baudRate;
        /// <summary>校验位</summary>
        public Int32 parity;
        /// <summary>数据位</summary>
        public Int32 dataBit;
        /// <summary>停止位</summary>
        public Int32 stopBit;
    }
    /// <summary>设备参数结构，可以配置为不同的设备信息，类似于C语言中的联合体</summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct DEVICE_PARA
    {
        /// <summary>位置触发板信息</summary>
        [FieldOffset(0)]
        public POS_TRIG_PARA posTrig;
        /// <summary>光源驱动板信息</summary>
        [FieldOffset(0)]
        public LIGHT_DIRVE_PARA light;
        /// <summary>串口信息</summary>
        [FieldOffset(0)]
        public COM_PARA com;
    }
    /// <summary>设备信息，包含设备地址、设备参数等信息</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVICE_INFO
    {
        /// <summary>设备名称</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String devName;
        /// <summary>总线号</summary>
        public UInt16 busNo;
        /// <summary>节点号</summary>
        public UInt16 nodeAddr;
        /// <summary>端口号</summary>
        public UInt16 portNo;
        /// <summary>设备类型</summary>
        public Byte devType;
        /// <summary>设备信息</summary>
        public DEVICE_PARA devPara;
    }
    #endregion

    #region  HT motion API接口，包含所有与C语言对应的原生接口
    /// <summary>HT motion API接口，包含所有与C语言对应的原生接口</summary>
    public class HTM_API
    {
        private const String _Dll = "HTM_Bsp_Lib.dll";
        private const CallingConvention _Cvt = CallingConvention.Cdecl;
        #region  HTM BSP(HT Motion Board support package) configuration functions
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_GetVersionNo();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_Bsp_GetVersionInfo();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_ShowVersion();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_Init(ref INIT_PARA init);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_InitFromFile(String xmlFile);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_Discard();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_LoadInitParaFromFile(String xmlFile, ref INIT_PARA init_para);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_SaveInitParaToFile(ref INIT_PARA init_para, String xmlFile);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_GetInitPara(out INIT_PARA init_para);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_GetMode();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_Bsp_GetConfigFilePath();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_GetLastErrCode();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_Bsp_GetLastErrStr();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_LoadUI();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_LoadToolUI();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_DiscardUI();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Bsp_DiscardToolUI();
        #endregion

        #region  HTM_EventLog APIs
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EventLog_LoadUI();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EventLog_DiscardUI();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EventLog_Start(Int32 task_id, string name, string info_fmt);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EventLog_End(Int32 task_id);
        #endregion

        #region  Axis operation APIs
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_InitAxisInfo(String file, UInt16 axisNum, Byte offline_mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern void HTM_DiscardAxisInfo();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_LoadAxisInfo(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SaveAxisInfo(Int32 axisIdx);
        #endregion

        #region  Get(set) axis parameter functions
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetAxisInfo(Int32 axisIdx, out AXIS_INFO axisInfo);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetAxisInfo(Int32 axisIdx, ref AXIS_INFO axisInfo);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetAxisStatus(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ConfigAxisInfo(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetAxisName(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetMaxVel(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetTimeOut(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetPulseFactor(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetMovePara(Int32 axisIdx);
        #endregion

        #region  Get(set) motion position(velocity) command(feedback)
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetCurHomePos(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetCmdPos(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetCmdPos(Int32 axisIdx, Double cmdpos_in_unit);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetFbkPos(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetFbkPos(Int32 axisIdx, Double fbkpos_in_unit);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetCmdVel(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetFbkVel(Int32 axisIdx);
        #endregion

        #region  Get motion IO status
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetMotionIOBit(Int32 axisIdx, Int32 bit);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetINP(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetPEL(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetMEL(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetORG(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetAlarm(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetSVON(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVON(Int32 axisIdx, Int32 on_off);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVON2(Int32 axis1, Int32 axis2, Int32 on_off);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVON3(Int32 axis1, Int32 axis2, Int32 axis3, Int32 on_off);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVON4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Int32 on_off);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVONN(Int32[] axisList, Int32 axisNum, Int32 on_off);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVONAll(Int32 on_off);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SVDone(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SVDone2(Int32 axis1, Int32 axis2);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SVDone3(Int32 axis1, Int32 axis2, Int32 axis3);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SVDone4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SVDoneN(Int32[] axisList, Int32 axisNum);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SVDoneAll();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVOver(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVOver2(Int32 axis1, Int32 axis2);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVOver3(Int32 axis1, Int32 axis2, Int32 axis3);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVOverN(Int32[] axisList, Int32 axisNum);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetSVOverAll();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EnableEL(Int32 axisIdx, Int32 pel_enable, Int32 mel_enable);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EnableSEL(Int32 axisIdx, Int32 spel_enable, Int32 smel_enable);
        #endregion

        #region  Single axis motion functions
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Home(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Home2(Int32 axis1, Int32 axis2);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Home3(Int32 axis1, Int32 axis2, Int32 axis3);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Home4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeN(Int32[] axisList, Int32 axisNum);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeOver(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeOver2(Int32 axis1, Int32 axis2);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeOver3(Int32 axis1, Int32 axis2, Int32 axis3);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeOverN(Int32[] axisList, Int32 axisNum);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CalcMotionTime(Int32 axisIdx, Double distance, out Double motion_time);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CalcMotionTimeEx(ref MOTION_PARA mp, Double distance, out Double motion_time);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MovePreload(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveTrigger(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveEx(Int32 axisIdx, Double pos_in_unit, Double vel_ratio, Int32 mode, ref MOTION_PARA mp);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Move(Int32 axisIdx, Double pos_in_unit, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Move2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Move3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Move4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMove(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMove2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMove3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMove4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMoveN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMove(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMove2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMove3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMove4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMoveN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMove(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMove2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMove3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMove4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMoveN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMove(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMove2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMove3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMove4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMoveN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_TQMove(Int32 axisIdx, Double force, Double vel);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_BondMove(Int32 axisIdx, Double pos, Double force, Double vel);

        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveOver(Int32 axisIdx, Double pos_in_unit, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveOver2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveOver3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MoveOverN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMoveOver(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMoveOver2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMoveOver3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMoveOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ASMoveOverN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMoveOver(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMoveOver2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMoveOver3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMoveOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ATMoveOverN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMoveOver(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMoveOver2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMoveOver3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMoveOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RSMoveOverN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMoveOver(Int32 axisIdx, Double pos_in_unit, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMoveOver2(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMoveOver3(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMoveOver4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_RTMoveOverN(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel_ratio);

        /* Two axes linear interpolation motion functions */
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_LineEx(Int32[] axisList, Int32 axisNum, Double[] posList, Double vel_ratio, Int32 mode, ref MOTION_PARA mp);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Arc2_CeEx(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double pos1, Double pos2, Int32 direction, Double vel_ratio, Int32 mode, ref MOTION_PARA mp);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Arc2_CaEx(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double angle, Double vel_ratio, Int32 mode, ref MOTION_PARA mp);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Line(Int32[] axisList, Int32 axisNum, Double[] posList, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Arc2_Ce(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double pos1, Double pos2, Int32 direction, Double vel_ratio, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Arc2_Ca(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double angle, Double vel_ratio, Int32 mode);

        /* Single axis speed move functions */
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Speed(Int32 axisIdx, Double vel_ratio);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SpeedEx(Int32 axisIdx, Double vel_ratio, ref MOTION_PARA mp);

        /* Single axis stop motion functions */
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Stop(Int32 axisIdx, Int32 mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_DStop(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_EStop(Int32 axisIdx);

        /* Clear axis alarm functions */
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ClearAlarm(Int32 axisIdx);

        /* axis motion done functions */
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeDone(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeDone2(Int32 axis1, Int32 axis2);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeDone3(Int32 axis1, Int32 axis2, Int32 axis3);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeDone4(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_HomeDoneN(Int32[] axisList, Int32 axisNum);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_TQDone(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_OnceDone(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Done(Int32 axisIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Done2(Int32 axisIdx1, Int32 axisIdx2);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Done3(Int32 axisIdx1, Int32 axisIdx2, Int32 axisIdx3);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Done4(Int32 axisIdx1, Int32 axisIdx2, Int32 axisIdx3, Int32 axisIdx4);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_DoneN(Int32[] axisIdx, Int32 num);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetErr(Int32 axisIdx, Int32 errNo);
        #endregion

        #region  IO operation APIs
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_InitIoInfo(String file, UInt16 ioNum, Byte offline_mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern void HTM_DiscardIoInfo();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_LoadIoInfo(Int32 ioIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SaveIoInfo(Int32 ioIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ConfigIoInfo(Int32 ioIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetIoStatus(Int32 ioIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetIoName(Int32 ioIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetIoInfo(Int32 ioIdx, out IO_INFO io);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetIoInfo(Int32 ioIdx, ref IO_INFO io);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetBit(Int32 ioIdx, Int32 value);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetBit(Int32 ioIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckION(Int32[] ioList, Int32 io_num, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIO(Int32 ioIdx, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIO2(Int32 ioIdx1, Int32 ioIdx2, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIO3(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIO4(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Int32 ioIdx4, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIOffN(Int32[] ioList, Int32 io_num, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIOff(Int32 ioIdx, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIOff2(Int32 ioIdx1, Int32 ioIdx2, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIOff3(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Double timeout);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CheckIOff4(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Int32 ioIdx4, Double timeout);
        #endregion

        #region  Other Device Operation APIs
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_InitDeviceInfo(String file, UInt16 devNum, Byte offline_mode);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern void HTM_DiscardDeviceInfo();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_LoadDeviceInfo(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SaveDeviceInfo(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ConfigDeviceInfo(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetDeviceName(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetDeviceType(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetDeviceInfo(Int32 devIdx, out DEVICE_INFO dev);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetDeviceInfo(Int32 devIdx, DEVICE_INFO[] dev);
        #endregion

        #region  Postion Trigger board
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetPtTrigEnable(Int32 devIdx, Int32 ptIdx, Int32 enable);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetPtTrigPos(Int32 devIdx, Int32 ptIdx, Double pos);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SWPosTrig(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetPtTrigPos(Int32 devIdx, Int32 ptIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetLinTrigPos(Int32 devIdx, TRIG_LINEAR lin);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetLinTrigEnable(Int32 devIdx, Int32 enable);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetTrigCnt(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_ResetTrigCnt(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetTrigCurPos(Int32 devIdx, out Double pos);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SyncTrigCurPos(Int32 devIdx);
        #endregion

        #region  Light Source Dirve
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SWLightTrig(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetLightTrigSrc(Int32 devIdx, UInt16 src_group);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_SetLightTrigTime(Int32 devIdx, Double time_in_us);
        #endregion

        #region Com Dirve
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetComNo(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_OpenCom(Int32 devIdx);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CloseCom(Int32 devIdx);
        #endregion

        #region  Utils lib
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Double HTM_GetTimeSec();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern UInt32 HTM_GetTimeMs();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetTimeStr();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern IntPtr HTM_GetDateStr();
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetTime(out Int32 hour, out Int32 minute, out Int32 second);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_GetDate(out Int32 year, out Int32 month, out Int32 day);
        #endregion

        #region  Analysis lib
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern void HTM_CalcRotatePos(Double former_x, Double former_y, Double rot_center_x, Double rot_center_y, Double delta_deg, out Double new_x, out Double new_y);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Mean(Double[] input_array, Int32 total_num, Int32 delete_max_num, Int32 delete_min_num, out Double mean_val);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Sort(Double[] input_array, Int32 total_num, Int32 descending, Double[] output_array);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MaxMin(Double[] input_array, Int32 total_num, out Double max, out Int32 max_id, out Double min, out Int32 min_id);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_Sum(Double[] input_array, Int32 total_num, out Double sum);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MatrixMul(Double[] A_aXn, Double[] B_nXb, Int32 rowOfA_a, Int32 row_colOfAB_n, Int32 colOfB_b, Double[] AB_aXb);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MatrixInv(Double[] input_matrix, Int32 dimensionNum, Double[] output_matrix);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_MatrixTranspose(Double[] input_matrix, Int32 row_num, Int32 col_num, Double[] output_matrix);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_LineFit(Double[] x, Double[] y, Int32 numberOfPoints, out Double k, out Double b);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_PolyFit(Double[] x, Double[] y, Int32 numberOfPoints, Int32 order, out Double k);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CircleFit(Double[] x, Double[] y, Int32 numberOfPoints, out Double centerX, out Double centerY, out Double r);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_LineInterp(Double[] x, Double[] y, Int32 numberOfElements, Double xValue, out Double interpolatedYValue);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_CalcTransMtx(Double[] A_nXa, Double[] B_nXb, Int32 numberOfRows_n, Int32 dimensionOfA_a, Int32 dimensionOfB_b, Double[] A2B_aplus1Xb);
        [DllImport(_Dll, CallingConvention = _Cvt)] public static extern Int32 HTM_TransPos(Double[] A_1Xa, Double[] A2B_aplus1Xb, Int32 dimensionOfA_a, Int32 dimensionOfB_b, Double[] B_1Xb);
        #endregion
    }
    #endregion

    #region HTM API接口，针对HTM_API原生C语言接口进行二次封装，适合C#开发
    /// <summary>HTM API接口，针对HTM_API原生C语言接口进行二次封装，适合C#开发</summary>
    public class HTM
    {
        #region BSP Operation Apis
        /// <summary>获取BSP版本号。</summary>
        /// <returns>返回版本号Int32类型.</returns>
        public static Int32 GetVersionNo() { return HTM_API.HTM_Bsp_GetVersionNo(); }
        /// <summary>获取BSP版本信息，例如x64_3.07_2017.0806。</summary>
        /// <returns>返回版本信息.</returns>
        public static String GetVersionInfo()
        {
            IntPtr pRet = HTM_API.HTM_Bsp_GetVersionInfo();
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>显示BSP版本信息(弹窗)。</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 ShowVersion() { return HTM_API.HTM_Bsp_ShowVersion(); }
        /// <summary>初始化BSP(申请内存，板卡通讯建立等)。</summary>
        /// <param name="init">输入，初始化配置信息, 如果config_file参数为空则默认取工程目录下的htm_config.db文件</param>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 Init(ref INIT_PARA init) { return HTM_API.HTM_Bsp_Init(ref init); }
        /// <summary>通过配置文件，初始化BSP(申请内存，板卡通讯建立等)。</summary>
        /// <param name="str">配置文件全名(xml格式，扩展名任意))</param>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 InitFromFile(String str) { return HTM_API.HTM_Bsp_InitFromFile(str); }
        /// <summary>获取初始化时所用配置文件路径。</summary>
        /// <returns>返回全路径.</returns>
        public static String GetConfigFile()
        {
            IntPtr pRet = HTM_API.HTM_Bsp_GetConfigFilePath();
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>获取当前初始化所用配置信息。</summary>
        /// <param name="init">输出，初始化配置信息</param>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 GetInitPara(out INIT_PARA init) { return HTM_API.HTM_Bsp_GetInitPara(out init); }
        /// <summary>获取BSP运行模式。</summary>
        /// <returns>返回运行模式，0-在线模式，1-脱机模式，2-调试模式(初始化时不检测硬件设备是否匹配，仅打开板卡).</returns>
        public static Int32 GetMode() { return HTM_API.HTM_Bsp_GetMode(); }
        /// <summary>BSP卸载，板卡通讯断开，清除内存等。</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 Discard() { return HTM_API.HTM_Bsp_Discard(); }
        /// <summary>加载轴、I/O等设备配置和调试面板（仅在正确初始化之后有效）</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 LoadUI() { return HTM_API.HTM_Bsp_LoadUI(); }
        /// <summary>加载轴调试小工具界面（仅在正确初始化之后有效）</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 LoadToolUI() { return HTM_API.HTM_Bsp_LoadToolUI(); }
        /// <summary>卸载时间轴、I/O等设备配置和调试窗口</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 DiscardUI() { return HTM_API.HTM_Bsp_DiscardUI(); }
        /// <summary>卸载轴、I/O工具窗口</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 DiscardToolUI() { return HTM_API.HTM_Bsp_DiscardToolUI(); }
        #endregion

        #region HTM_EventLog APIs        
        /// <summary>加载事件记录监控窗口（可以单独使用，运行BSP不初始化）</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 EventLog_LoadUI() { return HTM_API.HTM_EventLog_LoadUI(); }
        /// <summary>卸载时间记录监控窗口</summary>
        /// <returns>返回0代表成功，负值表示失败.</returns>
        public static Int32 EventLog_DiscardUI() { return HTM_API.HTM_EventLog_DiscardUI(); }
        /// <summary>用于记录开始时的信息。多线程重入：安全。</summary>
        /// <param name="task_id">用来标识任务的ID(正整数)。</param>
        /// <param name="name">待记录事件的名字。</param>
        /// <param name="msg">待记录事件信息的格式，用法和printf函数完全一样。如果不需要可以传入0</param>
        /// <returns>whether the module is loaded successfully. -1-failed, 0-success.</returns>
        public static Int32 EventLog_Start(Int32 task_id, string name, string msg) { return HTM_API.HTM_EventLog_Start(task_id, name, msg); }
        /// <summary>用于记录结束时的信息。多线程重入：安全。</summary>
        /// <param name="task_id">用来标识任务的ID(正整数)。</param>
        /// <returns>whether the module is loaded successfully. -1-failed, 0-success.</returns>
        public static Int32 EventLog_End(Int32 task_id) { return HTM_API.HTM_EventLog_End(task_id); }
        #endregion

        #region Axis operation APIs        
        /// <summary>初始化轴信息(申请所需内存)。</summary>
        /// <param name="file">数据库全路径</param>
        /// <param name="axisNum">轴个数</param>
        /// <param name="offline_mode">离线模式(1-离线，不初始化板卡，0-在线)</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 InitAxisInfo(String file, UInt16 axisNum, Byte offline_mode) { return HTM_API.HTM_InitAxisInfo(file, axisNum, offline_mode); }
        /// <summary>释放轴信息所占用内存空间</summary>
        public static void DiscardAxisInfo() { HTM_API.HTM_DiscardAxisInfo(); }
        /// <summary>从数据库中加载轴信息，加载到内存中(可通过HTM_API.HTM_GetAxisInfo获得)。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 LoadAxisInfo(Int32 axisIdx) { return HTM_API.HTM_LoadAxisInfo(axisIdx); }
        /// <summary>保存内存中的轴信息到数据库中。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 SaveAxisInfo(Int32 axisIdx) { return HTM_API.HTM_SaveAxisInfo(axisIdx); }
        #endregion

        #region Get(set) axis parameter functions        
        /// <summary>获取单轴信息</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="axisInfo">获取到的轴信息首地址</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 GetAxisInfo(Int32 axisIdx, out AXIS_INFO axisInfo) { return HTM_API.HTM_GetAxisInfo(axisIdx, out axisInfo); }
        /// <summary>设置单轴信息</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="axisInfo">设置的轴信息首地址 </param>
        /// <returns>返回0代表成功，-1表示失败</returns>
        public static Int32 SetAxisInfo(Int32 axisIdx, ref AXIS_INFO axisInfo) { return HTM_API.HTM_SetAxisInfo(axisIdx, ref axisInfo); }
        /// <summary>配置轴信息到板卡或驱动器。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回0代表成功，-1表示失败</returns>
        public static Int32 ConfigAxisInfo(Int32 axisIdx) { return HTM_API.HTM_ConfigAxisInfo(axisIdx); }
        /// <summary>获取轴名称</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回获取到的轴名称.</returns>
        public static String GetAxisName(Int32 axisIdx)
        {
            IntPtr pRet = HTM_API.HTM_GetAxisName(axisIdx);
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>
        /// 获取单轴的最大速度(mm/s)。 
        /// </summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回速度值.</returns>
        public static Double GetMaxVel(Int32 axisIdx) { return HTM_API.HTM_GetMaxVel(axisIdx); }
        /// <summary>
        /// 获取轴所配置的超时时间(s)。 
        /// </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回时间(s).</returns>
        public static Double GetTimeOut(Int32 axisIdx) { return HTM_API.HTM_GetTimeOut(axisIdx); }
        /// <summary>
        /// 获取轴所配置的脉冲当量(mm/pulse)。 
        /// </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回脉冲当量.</returns>
        public static Double GetPulseFactor(Int32 axisIdx) { return HTM_API.HTM_GetPulseFactor(axisIdx); }
        /// <summary>
        /// 获取轴所配置运动参数。 
        /// </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回运动参数,如果返回NULL，表示输入axisIdx有误.</returns>
        public static IntPtr GetMovePara(Int32 axisIdx) { return HTM_API.HTM_GetMovePara(axisIdx); }
        #endregion

        #region Get(set) motion position(velocity) command(feedback) 
        /// <summary>设置单轴基准位置，首先更新homeShift值，其次当前位置设置为0。</summary>
        /// <param name="axisIdx">axisIdx 轴号(0-based)</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 SetCurHomePos(Int32 axisIdx) { return HTM_API.HTM_SetCurHomePos(axisIdx); }
        /// <summary>获取单轴指令位置</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回单轴的指令位置(mm).</returns>
        public static Double GetCmdPos(Int32 axisIdx) { return HTM_API.HTM_GetCmdPos(axisIdx); }
        /// <summary>获取单轴反馈位置。</summary>
        /// <param name="axisIdx">axisIdx 轴号(0-based)</param>
        /// <returns>返回单轴的反馈位置(mm).</returns>
        public static Double GetFbkPos(Int32 axisIdx) { return HTM_API.HTM_GetFbkPos(axisIdx); }
        /// <summary>获取单轴状态。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="axisStatus">返回轴状态对应首地址，此API从内存(由背景线程中获得)中获取，始于实时性要求不高的场合，如UI定时刷新等.</param>
        public static void GetAxisStatus(Int32 axisIdx, out AXIS_STATUS axisStatus)
        {
            var intptr = HTM_API.HTM_GetAxisStatus(axisIdx);
            axisStatus = (AXIS_STATUS)Marshal.PtrToStructure(intptr, typeof(AXIS_STATUS));
        }
        /// <summary>设置单轴指令位置。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="cmdpos_in_unit">返回单轴的指令位置(mm). </param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 SetCmdPos(Int32 axisIdx, Double cmdpos_in_unit) { return HTM_API.HTM_SetCmdPos(axisIdx, cmdpos_in_unit); }
        /// <summary>设置单轴反馈位置</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="fbkpos_in_unit">返回单轴的反馈位置(mm)</param>
        /// <returns></returns>
        public static Int32 SetFbkPos(Int32 axisIdx, Double fbkpos_in_unit) { return HTM_API.HTM_SetFbkPos(axisIdx, fbkpos_in_unit); }
        /// <summary>获取单轴指令速度(mm/s)。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回单轴的指令速度(mm/s).</returns>
        public static Double GetCmdVel(Int32 axisIdx) { return HTM_API.HTM_GetCmdVel(axisIdx); }
        /// <summary> 获取单轴反馈速度(mm/s)。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回单轴的反馈速度(mm/s).</returns>
        public static Double GetFbkVel(Int32 axisIdx) { return HTM_API.HTM_GetFbkVel(axisIdx); }
        #endregion

        #region Get motion IO status
        /// <summary> 获取单轴INP信号</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回INP信号状态(0/1).</returns>
        public static Int32 GetINP(Int32 axisIdx) { return HTM_API.HTM_GetINP(axisIdx); }
        /// <summary>获取单轴正限位信号</summary>
        /// <param name="axisIdx">轴号(0-based)  </param>
        /// <returns>返回正限位信号状态(0/1).</returns>
        public static Int32 GetPEL(Int32 axisIdx) { return HTM_API.HTM_GetPEL(axisIdx); }
        /// <summary>获取单轴负限位信号。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回负限位信号状态(0/1).</returns>
        public static Int32 GetMEL(Int32 axisIdx) { return HTM_API.HTM_GetMEL(axisIdx); }
        /// <summary>获取单轴报警信号 </summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回报警信号状态(0/1).</returns>
        public static Int32 GetAlarm(Int32 axisIdx) { return HTM_API.HTM_GetAlarm(axisIdx); }
        /// <summary>获取单轴原点信号。  </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>返回原点信号状态(0/1).</returns>
        public static Int32 GetORG(Int32 axisIdx) { return HTM_API.HTM_GetORG(axisIdx); }
        /// <summary>设置轴电子限位使能，改变内存中数据并配置到板卡，但不保存到本地数据库。 </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="pel_enable">正电子限位使能标志(0-不使能,1-使能) </param>
        /// <param name="mel_enable">负电子限位使能标志(0-不使能,1-使能) </param>
        /// <returns>0代表成功,-1表示失败.</returns>
        public static Int32 EnableEL(Int32 axisIdx, Int32 pel_enable, Int32 mel_enable) { return HTM_API.HTM_EnableEL(axisIdx, pel_enable, mel_enable); }
        /// <summary>设置轴软限位使能，改变内存中数据并配置到板卡，但不保存到本地数据库。 </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="spel_enable">正软限位使能标志(0-不使能,1-使能) </param>
        /// <param name="smel_enable">负软限位使能标志(0-不使能,1-使能) </param>
        /// <returns>0代表成功,-1表示失败.</returns>
        public static Int32 EnableSEL(Int32 axisIdx, Int32 spel_enable, Int32 smel_enable) { return HTM_API.HTM_EnableEL(axisIdx, spel_enable, smel_enable); }
        /// <summary>获取单轴运动IO信息。 </summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <param name="bit">IO Bit(0-based). MIO_ALM(0)：报警; MIO_PEL(1)：正限; MIO_MEL(2)：负限; MIO_ORG(3)：原点; MIO_INP(6)：到位; MIO_SVON(7)：励磁; MIO_SPEL(11)：软正限; MIO_SMEL(12)：软负限 </param>
        /// <returns>Return motion IO status</returns>
        public static Int32 GetMotionIO(Int32 axisIdx, Int32 bit) { return HTM_API.HTM_GetMotionIOBit(axisIdx, bit); }
        #endregion

        #region Axis Motion Apis 
        /// <summary>单轴回零函数，不检测完成</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Home(Int32 axisIdx) { return HTM_API.HTM_Home(axisIdx); }
        /// <summary>两轴回零函数，不检测完成</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Home(Int32 axis1, Int32 axis2) { return HTM_API.HTM_Home2(axis1, axis2); }
        /// <summary>三轴回零函数，不检测完成</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="axis3">轴3号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Home(Int32 axis1, Int32 axis2, Int32 axis3) { return HTM_API.HTM_Home3(axis1, axis2, axis3); }
        /// <summary>四轴回零函数，不检测完成</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="axis3">轴3号(0-based)</param>
        /// <param name="axis4">轴4号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Home(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4) { return HTM_API.HTM_Home4(axis1, axis2, axis3, axis4); }
        /// <summary>多轴回零函数，不检测完成</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="axisNum">轴数</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Home(Int32[] axisList, Int32 axisNum) { return HTM_API.HTM_HomeN(axisList, axisNum); }
        /// <summary>单轴回零完成检测，阻塞检测直到回零完成或错误退出。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 HomeOver(Int32 axisIdx) { return HTM_API.HTM_HomeOver(axisIdx); }
        /// <summary>两轴回零完成检测，阻塞检测直到所有轴回零完成或超时退出.</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 HomeOver(Int32 axis1, Int32 axis2) { return HTM_API.HTM_HomeOver2(axis1, axis2); }
        /// <summary>三轴回零完成检测，阻塞检测直到所有轴回零完成或超时退出.</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="axis3">轴3号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 HomeOver(Int32 axis1, Int32 axis2, Int32 axis3) { return HTM_API.HTM_HomeOver3(axis1, axis2, axis3); }
        /// <summary>四轴回零完成检测，阻塞检测直到所有轴回零完成或超时退出.</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="axis3">轴3号(0-based)</param>
        /// <param name="axis4">轴4号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 HomeOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4) { return HTM_API.HTM_HomeOver4(axis1, axis2, axis3, axis4); }
        /// <summary>多轴回零完成检测，阻塞检测直到所有轴回零完成或某超时退出.</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="axisNum">轴个数</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 HomeOver(Int32[] axisList, Int32 axisNum) { return HTM_API.HTM_HomeOverN(axisList, axisNum); }
        /// <summary>用于根据运动参数计算理论运动时间（仅计算，不操作硬件）。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="distance">运动距离</param>
        /// <param name="motion_time">输出参数。计算所得的理论运动时间</param>
        /// <returns>0代表成功, 负值表示传入参数有误.</returns>
        public static Int32 CalcMotionTime(Int32 axisIdx, Double distance, out Double motion_time) { return HTM_API.HTM_CalcMotionTime(axisIdx, distance, out motion_time); }
        /// <summary>用于计算单轴的理论运动时间（仅计算，不操作硬件）。</summary>
        /// <param name="mp">MOTION_PARA结构体，传入运动参数(加速度/速度/减速度等)</param>
        /// <param name="distance">运动距离</param>
        /// <param name="motion_time">输出参数。计算所得的理论运动时间</param>
        /// <returns>0代表成功, 负值表示传入参数有误.</returns>
        public static Int32 CalcMotionTime(ref MOTION_PARA mp, Double distance, out Double motion_time) { return HTM_API.HTM_CalcMotionTimeEx(ref mp, distance, out motion_time); }
        /// <summary>单轴设置预加载模式(设置预加载模式后，轴运动将不会立即执行直到MoveTrigger)。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MovePreload(Int32 axisIdx) { return HTM_API.HTM_MovePreload(axisIdx); }
        /// <summary>单轴运动触发(仅在已Preload预加载过运动之后有效)。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MoveTrigger(Int32 axisIdx) { return HTM_API.HTM_MoveTrigger(axisIdx); }
        /// <summary>单轴点到点运动函数, 不检测到位(可以定制运动参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式;</param>
        /// <param name="mp">运动参数(加速度/速度/S因子等)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Move(Int32 axisIdx, Double pos, Double vel, Int32 mode, ref MOTION_PARA mp) { return HTM_API.HTM_MoveEx(axisIdx, pos, vel, mode, ref mp); }
        /// <summary>单轴点到点运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Move(Int32 axisIdx, Double pos, Double vel, Int32 mode) { return HTM_API.HTM_Move(axisIdx, pos, vel, mode); }
        /// <summary>两轴点到点运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="pos1">轴1目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">轴2目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Move(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel, Int32 mode) { return HTM_API.HTM_Move2(axis1, axis2, pos1, pos2, vel, mode); }
        /// <summary>三轴点到点运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="axis3">轴3号(0-based)</param>
        /// <param name="pos1">轴1目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">轴2目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos3">轴3目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Move(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel, Int32 mode) { return HTM_API.HTM_Move3(axis1, axis2, axis3, pos1, pos2, pos3, vel, mode); }
        /// <summary>四轴点到点运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1号(0-based)</param>
        /// <param name="axis2">轴2号(0-based)</param>
        /// <param name="axis3">轴3号(0-based)</param>
        /// <param name="axis4">轴4号(0-based)</param>
        /// <param name="pos1">轴1目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">轴2目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos3">轴3目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos4">轴4目标位置(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Move(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel, Int32 mode) { return HTM_API.HTM_Move4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel, mode); }
        /// <summary>多轴点到点运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">与轴一一对应，目标位置列表(mm或deg) 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="axisNum">轴个数</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Move(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel, Int32 mode) { return HTM_API.HTM_MoveN(axisList, posList, axisNum, vel, mode); }
        /// <summary>单轴点到点绝对S速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ASMove(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_ASMove(axisIdx, pos, vel); }
        /// <summary> 两轴点到点绝对S曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 ASMove(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_ASMove2(axis1, axis2, pos1, pos2, vel); }
        /// <summary> 三轴点到点绝对S曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">轴3绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功,负值表示失败</returns>
        public static Int32 ASMove(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_ASMove3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary> 四轴点到点绝对S曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">轴3绝对目标位置, 单位mm或deg</param>
        /// <param name="pos4">轴4绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功,负值表示失败</returns>
        public static Int32 ASMove(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_ASMove4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary> 多轴点到点绝对S曲线运动, 不检测到位 </summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">与各轴一一对应，各轴的绝对目标位置列表, 单位mm或deg</param>
        /// <param name="axisNum">轴数目</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 ASMove(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel) { return HTM_API.HTM_ASMoveN(axisList, posList, axisNum, vel); }
        /// <summary>单轴点到点相对S速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMove(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_RSMove(axisIdx, pos, vel); }
        /// <summary>  两轴点到点相对S曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMove(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_RSMove2(axis1, axis2, pos1, pos2, vel); }
        /// <summary> 三轴点到点相对S曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败</returns>
        public static Int32 RSMove(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_RSMove3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary> 四轴点到点相对S曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="pos4">轴4相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败</returns>
        public static Int32 RSMove(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_RSMove4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点相对S曲线运动, 不检测到位。</summary>
        /// <param name="axisList">轴号列表</param>
        /// <param name="posList">各轴对应的相对运动距离, 单位mm或deg</param>
        /// <param name="axisNum">轴数目</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败</returns>
        public static Int32 RSMove(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel) { return HTM_API.HTM_RSMoveN(axisList, posList, axisNum, vel); }
        /// <summary>单轴点到点绝对T速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMove(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_ATMove(axisIdx, pos, vel); }
        /// <summary> 两轴点到点绝对T曲线运动, 不检测到位。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMove(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_ATMove2(axis1, axis2, pos1, pos2, vel); }
        /// <summary>三轴点到点绝对T曲线运动, 不检测到位</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">轴3绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMove(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_ATMove3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary>四轴点到点绝对T曲线运动, 不检测到位</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">轴3绝对目标位置, 单位mm或deg</param>
        /// <param name="pos4">轴4绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMove(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_ATMove4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点绝对T曲线运动, 不检测到位。</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">各轴绝对目标位置列表, 单位mm或deg</param>
        /// <param name="axisNum">轴数目</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMove(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel) { return HTM_API.HTM_ATMoveN(axisList, posList, axisNum, vel); }
        /// <summary>单轴点到点相对T速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMove(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_RTMove(axisIdx, pos, vel); }
        /// <summary>两轴点到点相对T速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMove(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_RTMove2(axis1, axis2, pos1, pos2, vel); }
        /// <summary>三轴点到点相对T速度曲线运动函数, 不检测到位.(使用配置的默认参数) </summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMove(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_RTMove3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary>四轴点到点相对T速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="pos4">轴4相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMove(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_RTMove4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点相对T速度曲线运动函数, 不检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisList">多轴轴号列表(0-based)</param>
        /// <param name="posLis">各轴相对运动距离列表，单位mm或deg</param>
        /// <param name="axisNum">轴数</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMove(Int32[] axisList, Double[] posLis, Int32 axisNum, Double vel) { return HTM_API.HTM_RTMoveN(axisList, posLis, axisNum, vel); }
        /// <summary>轴点到点运动函数, 并阻塞检测到位（采用默认已配置的运动参数）。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MoveOver(Int32 axisIdx, Double pos, Double vel, Int32 mode) { return HTM_API.HTM_MoveOver(axisIdx, pos, vel, mode); }
        /// <summary> 两轴点到点运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出（采用默认已配置的运动参数）</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MoveOver(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel, Int32 mode) { return HTM_API.HTM_MoveOver2(axis1, axis2, pos1, pos2, vel, mode); }
        /// <summary>三轴点到点运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出（采用默认已配置的运动参数）。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos3">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel, Int32 mode) { return HTM_API.HTM_MoveOver3(axis1, axis2, axis3, pos1, pos2, pos3, vel, mode); }
        /// <summary>四轴点到点运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出（采用默认已配置的运动参数）。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos3">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos4">单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel, Int32 mode) { return HTM_API.HTM_MoveOver4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel, mode); }
        /// <summary>多轴点到点运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出（采用默认已配置的运动参数）。</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">各轴对应的运动位置列表，单位mm或deg, 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="axisNum">轴总数目</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mode">模式定义在HTM_API.HTM_Define中。绝对/相对运动模式, S(Scurve)/T(Trapezoid)形速度模式, 力模式, 缓冲/覆盖当前运动模式</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 MoveOver(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel, Int32 mode) { return HTM_API.HTM_MoveOverN(axisList, posList, axisNum, vel, mode); }
        /// <summary>单轴点到点绝对S速度曲线运动函数, 并阻塞检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ASMoveOver(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_ASMoveOver(axisIdx, pos, vel); }
        /// <summary>两轴点到点绝对S速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ASMoveOver(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_ASMoveOver2(axis1, axis2, pos1, pos2, vel); }
        /// <summary>三轴点到点绝对S速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ASMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_ASMoveOver3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary>四轴点到点绝对S速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出(使用配置的默认参数) </summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">绝对目标位置, 单位mm或deg</param>
        /// <param name="pos4">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ASMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_ASMoveOver4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点绝对S速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出(使用配置的默认参数)</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">各轴一一对应的运动位置列表，绝对目标位置, 单位mm或deg</param>
        /// <param name="axisNum">轴数</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ASMoveOver(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel) { return HTM_API.HTM_ASMoveOverN(axisList, posList, axisNum, vel); }
        /// <summary>单轴点到点相对S速度曲线运动函数, 并阻塞检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMoveOver(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_RSMoveOver(axisIdx, pos, vel); }
        /// <summary>两轴点到点相对S速度曲线运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出(使用配置的默认参数).</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴1轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMoveOver(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_RSMoveOver2(axis1, axis2, pos1, pos2, vel); }
        /// <summary>三轴点到点相对S速度曲线运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出(使用配置的默认参数). </summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_RSMoveOver3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary>四轴点到点相对S速度曲线运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出(使用配置的默认参数).</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="pos4">轴4相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_RSMoveOver4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点相对S速度曲线运动函数, 并阻塞检测到位，直到所有轴运动到位或超时退出(使用配置的默认参数).</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">与轴号一一对应，相对运动距离列表，单位mm或deg</param>
        /// <param name="axisNum">轴数</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RSMoveOver(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel) { return HTM_API.HTM_RSMoveOverN(axisList, posList, axisNum, vel); }
        /// <summary>单轴点到点绝对T速度曲线运动函数, 并阻塞检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMoveOver(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_ATMoveOver(axisIdx, pos, vel); }
        /// <summary>两轴点到点绝对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMoveOver(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_ATMoveOver2(axis1, axis2, pos1, pos2, vel); }
        /// <summary>三轴点到点绝对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">轴3绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_ATMoveOver3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary>四轴点到点绝对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出.(使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1绝对目标位置, 单位mm或deg</param>
        /// <param name="pos2">轴2绝对目标位置, 单位mm或deg</param>
        /// <param name="pos3">轴3绝对目标位置, 单位mm或deg</param>
        /// <param name="pos4">轴4绝对目标位置, 单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_ATMoveOver4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点绝对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出.(使用配置的默认参数)</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posList">各轴一一对应，绝对目标位置列表, 单位mm或deg</param>
        /// <param name="axisNum">轴数</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 ATMoveOver(Int32[] axisList, Double[] posList, Int32 axisNum, Double vel) { return HTM_API.HTM_ATMoveOverN(axisList, posList, axisNum, vel); }
        /// <summary>单轴点到点相对T速度曲线运动函数, 并阻塞检测到位.(使用配置的默认参数)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMoveOver(Int32 axisIdx, Double pos, Double vel) { return HTM_API.HTM_RTMoveOver(axisIdx, pos, vel); }
        /// <summary>两轴点到点相对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出 (使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMoveOver(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel) { return HTM_API.HTM_RTMoveOver2(axis1, axis2, pos1, pos2, vel); }
        /// <summary>三轴点到点相对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出 (使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Double pos1, Double pos2, Double pos3, Double vel) { return HTM_API.HTM_RTMoveOver3(axis1, axis2, axis3, pos1, pos2, pos3, vel); }
        /// <summary>四轴点到点相对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出 (使用配置的默认参数)</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="pos1">轴1相对运动距离，单位mm或deg</param>
        /// <param name="pos2">轴2相对运动距离，单位mm或deg</param>
        /// <param name="pos3">轴3相对运动距离，单位mm或deg</param>
        /// <param name="pos4">轴4相对运动距离，单位mm或deg</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMoveOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Double pos1, Double pos2, Double pos3, Double pos4, Double vel) { return HTM_API.HTM_RTMoveOver4(axis1, axis2, axis3, axis4, pos1, pos2, pos3, pos4, vel); }
        /// <summary>多轴点到点相对T速度曲线运动函数, 并阻塞检测到位, 直到所有轴运动到位或超时退出 (使用配置的默认参数)</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="posLis">各轴一一对应，相对运动距离列表，单位mm或deg</param>
        /// <param name="axisNum">轴数</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 RTMoveOver(Int32[] axisList, Double[] posLis, Int32 axisNum, Double vel) { return HTM_API.HTM_RTMoveOverN(axisList, posLis, axisNum, vel); }
        /// <summary>Bond模式运动（带位置限幅的力模式）</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="pos">目标位置(mm)</param>
        /// <param name="force">力(g)</param>
        /// <param name="vel">速度(mm/s)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 BondMove(Int32 axisIdx, Double pos, Double force, Double vel) { return HTM_API.HTM_BondMove(axisIdx, pos, force, vel); }
        /// <summary>单轴速度运动指令，以默认运动参数。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Speed(Int32 axisIdx, Double vel) { return HTM_API.HTM_Speed(axisIdx, vel); }
        /// <summary>单轴速度运动指令，可输入运动参数。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="vel">运动速率(-1.0~1.0)</param>
        /// <param name="mp">运动参数MOTION_PARA结构</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Speed(Int32 axisIdx, Double vel, ref MOTION_PARA mp) { return HTM_API.HTM_SpeedEx(axisIdx, vel, ref mp); }
        /// <summary>轴紧急停止运动函数。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Stop(Int32 axisIdx) { return HTM_API.HTM_EStop(axisIdx); }
        /// <summary>轴停止运动函数。(带模式选择)</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="mode">停止模式，HTM_API.HTM_STOP_EMG-立即以最大减速度急停, HTM_API.HTM_STOP_DEC-立即以默认减速度停止, 其它参数仅针对直流电机驱动板: HTM_API.HTM_STOP_PEL-仍保持运动直到检测到正限位后急停, HTM_API.HTM_STOP_MEL-仍保持运动直到检测到负限位后急停, HTM_API.HTM_STOP_IO0 ~3-仍保持运动直到检测到对应信号后急停</param>
        /// <returns>0代表成功, 负值表示失败.</returns>
        public static Int32 Stop(Int32 axisIdx, Int32 mode) { return HTM_API.HTM_Stop(axisIdx, mode); }
        /// <summary>获取单轴励磁信号。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>返回励磁信号状态, 0-未励磁，1-已励磁，负值表示失败.</returns>
        public static Int32 GetSVON(Int32 axisIdx) { return HTM_API.HTM_GetSVON(axisIdx); }
        /// <summary>单轴励磁操作，不检测励磁成功。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <param name="ON">励磁指令(0/1)</param>
        /// <returns>0代表指令发送成功, 负值表示失败.</returns>
        public static Int32 SetSVON(Int32 axisIdx, Int32 ON) { return HTM_API.HTM_SetSVON(axisIdx, ON); }
        /// <summary>两轴励磁操作，不检测励磁成功。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="ON">励磁指令(0/1)</param>
        /// <returns>0代表指令发送成功, 负值表示失败.</returns>
        public static Int32 SetSVON(Int32 axis1, Int32 axis2, Int32 ON) { return HTM_API.HTM_SetSVON2(axis1, axis2, ON); }
        /// <summary>三轴励磁操作，不检测励磁成功。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="ON">励磁指令(0/1)</param>
        /// <returns>0代表指令发送成功, 负值表示失败.</returns>
        public static Int32 SetSVON(Int32 axis1, Int32 axis2, Int32 axis3, Int32 ON) { return HTM_API.HTM_SetSVON3(axis1, axis2, axis3, ON); }
        /// <summary>四轴励磁操作，不检测励磁成功。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <param name="ON">励磁指令(0/1)</param>
        /// <returns>0代表指令发送成功, 负值表示失败.</returns>
        public static Int32 SetSVON(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4, Int32 ON) { return HTM_API.HTM_SetSVON4(axis1, axis2, axis3, axis4, ON); }
        /// <summary>多轴励磁操作，不检测励磁成功。</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="axisNum">轴数</param>
        /// <param name="ON">励磁指令(0/1)</param>
        /// <returns>0代表指令发送成功, 负值表示失败.</returns>
        public static Int32 SetSVON(Int32[] axisList, Int32 axisNum, Int32 ON) { return HTM_API.HTM_SetSVONN(axisList, axisNum, ON); }
        /// <summary>所有轴励磁操作，不检测励磁成功。</summary>
        /// <param name="ON">励磁指令(0/1)</param>
        /// <returns>0代表指令发送成功, 负值表示失败.</returns>
        public static Int32 SetSVONAll(Int32 ON) { return HTM_API.HTM_SetSVONAll(ON); }
        /// <summary>单轴励磁操作，并阻塞检测，直到励磁成功或超时。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0代表励磁指令发送成功并完成励磁, 负值表示失败.</returns>
        public static Int32 SetSVOver(Int32 axisIdx) { return HTM_API.HTM_SetSVOver(axisIdx); }
        /// <summary>两轴励磁操作，并阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <returns>0代表励磁指令发送成功并完成励磁, 负值表示失败.</returns>
        /// <returns></returns>
        public static Int32 SetSVOver(Int32 axis1, Int32 axis2) { return HTM_API.HTM_SetSVOver2(axis1, axis2); }
        /// <summary>三轴励磁操作，并阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <returns>0代表励磁指令发送成功并完成励磁, 负值表示失败.</returns>
        public static Int32 SetSVOver(Int32 axis1, Int32 axis2, Int32 axis3) { return HTM_API.HTM_SetSVOver3(axis1, axis2, axis3); }
        /// <summary>四轴励磁操作，并阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <returns>0代表励磁指令发送成功并完成励磁, 负值表示失败.</returns>
        public static Int32 SetSVOver(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4) { return HTM_API.HTM_SetSVOver4(axis1, axis2, axis3, axis4); }
        /// <summary>多轴励磁操作，并阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="axisNum">轴数</param>
        /// <returns>0代表励磁指令发送成功并完成励磁, 负值表示失败.</returns>
        public static Int32 SetSVOver(Int32[] axisList, Int32 axisNum) { return HTM_API.HTM_SetSVOverN(axisList, axisNum); }
        /// <summary>所有轴励磁操作，并阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <returns>0代表励磁指令发送成功并完成励磁, 负值表示失败.</returns>
        public static Int32 SetSVOverAll() { return HTM_API.HTM_SetSVOverAll(); }
        /// <summary>检测单轴励磁是否成功，阻塞检测，直到单轴励磁成功或某轴超时退出。</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示超时，默认超时时间5秒.</returns>
        public static Int32 SVDone(Int32 axisIdx) { return HTM_API.HTM_SVDone(axisIdx); }
        /// <summary>检测两轴励磁是否成功，阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示超时，默认超时时间5秒.</returns>
        public static Int32 SVDone(Int32 axis1, Int32 axis2) { return HTM_API.HTM_SVDone2(axis1, axis2); }
        /// <summary>检测三轴励磁是否成功，阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示超时，默认超时时间5秒.</returns>
        public static Int32 SVDone(Int32 axis1, Int32 axis2, Int32 axis3) { return HTM_API.HTM_SVDone3(axis1, axis2, axis3); }
        /// <summary>检测四轴励磁是否成功，阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示超时，默认超时时间5秒.</returns>
        public static Int32 SVDone(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4) { return HTM_API.HTM_SVDone4(axis1, axis2, axis3, axis4); }
        /// <summary>检测多轴励磁是否成功，阻塞检测，直到单轴励磁成功或超时退出。</summary>
        /// <param name="axisList">轴号列表(0-based)</param>
        /// <param name="axisNum">轴数</param>
        /// <returns>0表示检测成功，负值表示超时，默认超时时间5秒.</returns>
        public static Int32 SVDone(Int32[] axisList, Int32 axisNum) { return HTM_API.HTM_SVDoneN(axisList, axisNum); }
        /// <summary>检测所有轴励磁是否成功，阻塞检测，直到所有轴励磁成功或某轴超时退出。</summary>
        /// <returns>0表示检测成功，负值表示超时，默认超时时间5秒.</returns>
        public static Int32 SVDoneAll() { return HTM_API.HTM_SVDoneAll(); }
        #endregion

        #region Motion done
        /// <summary>单次检测Motion Done信号函数(同时检测报警)，非阻塞检测</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>0代表未检测到MotionDone信号,1表示检测到.</returns>
        public static Int32 OnceDone(Int32 axisIdx) { return HTM_API.HTM_OnceDone(axisIdx); }
        /// <summary>检测单轴Motion Done信号函数，(同时检测正负限、报警)，阻塞检测，直到有信号或报警退出。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>0表示检测成功，负值表示报警.</returns>
        public static Int32 Done(Int32 axisIdx) { return HTM_API.HTM_Done(axisIdx); }
        /// <summary>检测两轴Motion Done信号函数(同时检测报警)，阻塞检测，直到所有轴运动完成或报警退出。</summary>
        /// <param name="axisIdx1">轴1轴号(0-based) </param>
        /// <param name="axisIdx2">轴2轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示报警.</returns>
        public static Int32 Done(Int32 axisIdx1, Int32 axisIdx2) { return HTM_API.HTM_Done2(axisIdx1, axisIdx2); }
        /// <summary>检测三轴Motion Done信号函数(同时检测报警)，阻塞检测，直到所有轴运动完成或报警退出。</summary>
        /// <param name="axisIdx1">轴1轴号(0-based)</param>
        /// <param name="axisIdx2">轴2轴号(0-based)</param>
        /// <param name="axisIdx3">轴3轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示报警.</returns>
        public static Int32 Done(Int32 axisIdx1, Int32 axisIdx2, Int32 axisIdx3) { return HTM_API.HTM_Done3(axisIdx1, axisIdx2, axisIdx3); }
        /// <summary>
        /// 检测四轴Motion Done信号函数(同时检测报警)，阻塞检测，直到所有轴运动完成或报警退出。
        /// </summary>
        /// <param name="axisIdx1">轴1轴号(0-based)</param>
        /// <param name="axisIdx2">轴2轴号(0-based)</param>
        /// <param name="axisIdx3">轴3轴号(0-based)</param>
        /// <param name="axisIdx4">轴4轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示报警.</returns>
        public static Int32 Done(Int32 axisIdx1, Int32 axisIdx2, Int32 axisIdx3, Int32 axisIdx4) { return HTM_API.HTM_Done4(axisIdx1, axisIdx2, axisIdx3, axisIdx4); }
        /// <summary> 检测多轴Motion Done信号函数(同时检测报警)，阻塞检测，直到所有轴运动完成或报警退出</summary>
        /// <param name="axesList">轴号列表(0-based) </param>
        /// <param name="axisNum">轴个数, 最多32个 </param>
        /// <returns>0表示检测成功，负值表示报警.</returns>
        public static Int32 Done(Int32[] axesList, Int32 axisNum) { return HTM_API.HTM_DoneN(axesList, axisNum); }
        /// <summary>检测力矩模式是否达到设定力，阻塞检测</summary>
        /// <param name="axisIdx">轴号(0-based)</param>
        /// <returns>0表示检测成功，负值表示失败.</returns>
        public static Int32 TQDone(Int32 axisIdx) { return HTM_API.HTM_TQDone(axisIdx); }
        /// <summary>单轴回零完成检测，阻塞检测直到回零完成或错误退出。</summary>
        /// <param name="axisIdx">轴号(0-based) </param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 HomeDone(Int32 axisIdx) { return HTM_API.HTM_HomeDone(axisIdx); }
        /// <summary>两轴回零完成检测，阻塞检测直到所有轴回零完成或某个轴错误</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 HomeDone(Int32 axis1, Int32 axis2) { return HTM_API.HTM_HomeDone2(axis1, axis2); }
        /// <summary>三轴回零完成检测，阻塞检测直到所有轴回零完成或某个轴错误</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 HomeDone(Int32 axis1, Int32 axis2, Int32 axis3) { return HTM_API.HTM_HomeDone3(axis1, axis2, axis3); }
        /// <summary>四轴回零完成检测，阻塞检测直到所有轴回零完成或某个轴错误</summary>
        /// <param name="axis1">轴1轴号(0-based)</param>
        /// <param name="axis2">轴2轴号(0-based)</param>
        /// <param name="axis3">轴3轴号(0-based)</param>
        /// <param name="axis4">轴4轴号(0-based)</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 HomeDone(Int32 axis1, Int32 axis2, Int32 axis3, Int32 axis4) { return HTM_API.HTM_HomeDone4(axis1, axis2, axis3, axis4); }
        /// <summary>多轴回零完成检测，阻塞检测直到所有轴回零完成或某个轴错误</summary>
        /// <param name="axesList">各轴轴号列表(0-based)</param>
        /// <param name="axisNum">轴数</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 HomeDone(Int32[] axesList, Int32 axisNum) { return HTM_API.HTM_HomeDoneN(axesList, axisNum); }
        #endregion

        #region Two axes linear interpolation motion functions
        /// <summary>两轴直线插补运动, 不检测到位</summary>
        /// <param name="axisList">轴号列表(0-based) </param>
        /// <param name="axisNum">轴个数(0-based) </param>
        /// <param name="posList">轴目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position </param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0) </param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <param name="mp">运动参数(向量值) </param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 Line(Int32[] axisList, Int32 axisNum, Double[] posList, Double vel_ratio, Int32 mode, ref MOTION_PARA mp) { return HTM_API.HTM_LineEx(axisList, axisNum, posList, vel_ratio, mode, ref mp); }
        /// <summary>轴直线插补运动, 不检测到位（使用轴1的运动参数作为向量参数)</summary>
        /// <param name="axisList">轴号列表(0-based) </param>
        /// <param name="axisNum">轴个数(0-based) </param>
        /// <param name="posList">轴目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position </param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0) </param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 Line(Int32[] axisList, Int32 axisNum, Double[] posList, Double vel_ratio, Int32 mode) { return HTM_API.HTM_Line(axisList, axisNum, posList, vel_ratio, mode); }
        /// <summary>两轴直线插补运动, 不检测到位（使用轴1的运动参数作为向量参数)</summary>
        /// <param name="axis1">轴1</param>
        /// <param name="axis2">轴2</param>
        /// <param name="pos1">轴1目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">轴1目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0)</param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <param name="mp">运动参数(向量值)</param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 Line(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio, Int32 mode, ref MOTION_PARA mp)
        {
            Int32[] axisList = new Int32[2] { axis1, axis2 };
            Double[] posList = new Double[2] { pos1, pos2 };
            return HTM_API.HTM_LineEx(axisList, 2, posList, vel_ratio, mode, ref mp);
        }
        /// <summary>两轴直线插补运动, 不检测到位（使用轴1的运动参数作为向量参数)</summary>
        /// <param name="axis1">轴1</param>
        /// <param name="axis2">轴2</param>
        /// <param name="pos1">轴1目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="pos2">轴1目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position</param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0)</param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 Line(Int32 axis1, Int32 axis2, Double pos1, Double pos2, Double vel_ratio, Int32 mode)
        {
            Int32[] axisList = new Int32[2] { axis1, axis2 };
            Double[] posList = new Double[2] { pos1, pos2 };
            return HTM_API.HTM_Line(axisList, 2, posList, vel_ratio, mode);
        }
        /// <summary>两轴以圆心、结束位置模式圆弧插补运动, 不检测到位。</summary>
        /// <param name="axisIdx1">轴号1(0-based) </param>
        /// <param name="axisIdx2">轴号2(0-based) </param>
        /// <param name="center1">圆心位置轴1坐标(mm) </param>
        /// <param name="center2">圆心位置轴2坐标(mm) </param>
        /// <param name="pos1">轴1目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position </param>
        /// <param name="pos2">轴2目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position </param>
        /// <param name="direction">方向(0/1) </param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0) </param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <param name="mp">运动参数(加速度/速度/S因子等) </param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 ArcCe(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double pos1, Double pos2, Int32 direction, Double vel_ratio, Int32 mode, ref MOTION_PARA mp) { return HTM_API.HTM_Arc2_CeEx(axisIdx1, axisIdx2, center1, center2, pos1, pos2, direction, vel_ratio, mode, ref mp); }
        /// <summary>两轴以圆心、结束位置模式圆弧插补运动, 不检测到位。</summary>
        /// <param name="axisIdx1">轴号1(0-based) </param>
        /// <param name="axisIdx2">轴号2(0-based) </param>
        /// <param name="center1">圆心位置轴1坐标(mm) </param>
        /// <param name="center2">圆心位置轴2坐标(mm) </param>
        /// <param name="pos1">轴1目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position </param>
        /// <param name="pos2">轴2目标位置或距离，用户单位(mm或deg), 如果是相对运动，该值为distance，如果是绝对运动，该值为position </param>
        /// <param name="direction">方向(0/1) </param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0) </param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <returns>0代表成功,负值表示失败.</returns>
        public static Int32 ArcCe(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double pos1, Double pos2, Int32 direction, Double vel_ratio, Int32 mode) { return HTM_API.HTM_Arc2_Ce(axisIdx1, axisIdx2, center1, center2, pos1, pos2, direction, vel_ratio, mode); }
        /// <summary>两轴以圆心、角度模式圆弧插补运动, 不检测到位</summary>
        /// <param name="axisIdx1">轴号1(0-based) </param>
        /// <param name="axisIdx2">轴号2(0-based) </param>
        /// <param name="center1">圆心位置轴1坐标(mm) </param>
        /// <param name="center2">圆心位置轴2坐标(mm) </param>
        /// <param name="angle">旋转角度(degree) </param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0) </param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        /// <param name="mp">运动参数(加速度/速度/S因子等) </param>
        /// <returns></returns>
        public static Int32 ArcCa(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double angle, Double vel_ratio, Int32 mode, ref MOTION_PARA mp) { return HTM_API.HTM_Arc2_CaEx(axisIdx1, axisIdx2, center1, center2, angle, vel_ratio, mode, ref mp); }
        /// <summary>两轴以圆心、角度模式圆弧插补运动, 不检测到位。</summary>
        /// <param name="axisIdx1">轴号1(0-based) </param>
        /// <param name="axisIdx2">轴号2(0-based) </param>
        /// <param name="center1">圆心位置轴1坐标(mm) </param>
        /// <param name="center2">圆心位置轴2坐标(mm) </param>
        /// <param name="angle">旋转角度(degree) </param>
        /// <param name="vel_ratio">运动速率(-1.0~1.0) </param>
        /// <param name="mode">HTM_API.HTM_ABS_MOVE: 绝对运动，否则为相对运动; HTM_API.HTM_S_MOVE: S(Scurve)形速度，否则为T(Trapezoid)形速度 </param>
        public static Int32 ArcCa(Int32 axisIdx1, Int32 axisIdx2, Double center1, Double center2, Double angle, Double vel_ratio, Int32 mode) { return HTM_API.HTM_Arc2_Ca(axisIdx1, axisIdx2, center1, center2, angle, vel_ratio, mode); }
        #endregion

        #region IO operation APIs
        /// <summary>获取IO状态。与GetBit的区别在于GetBit为直接从板卡中读取，而GetIoStatus为从程序内存(由背景线程负责刷新)中读取.</summary>
        /// <param name="ioIdx">IO号(0-based)</param>
        /// <returns>返回IO状态(0/1)</returns>
        public static Int32 GetIoStatus(Int32 ioIdx) { return HTM_API.HTM_GetIoStatus(ioIdx); }
        /// <summary>获取IO名称。</summary>
        /// <param name="ioIdx">IO号(0-based)  </param>
        /// <returns>返回IO的名字.</returns>
        public static String GetIoName(Int32 ioIdx)
        {
            IntPtr pRet = HTM_API.HTM_GetIoName(ioIdx);
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>获取IO信息。</summary>
        /// <param name="ioIdx">IO号(0-based) </param>
        /// <param name="io">获取到的IO信息首地址 </param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 GetIoInfo(Int32 ioIdx, out IO_INFO io) { return HTM_API.HTM_GetIoInfo(ioIdx, out io); }
        /// <summary>设置IO信息。</summary>
        /// <param name="ioIdx"> ioIdx IO号(0-based)  </param>
        /// <param name="io">设置IO信息的首地址  </param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 SetIoInfo(Int32 ioIdx, ref IO_INFO io) { return HTM_API.HTM_SetIoInfo(ioIdx, ref io); }
        /// <summary>从板卡中读取IO状态.与GetIoStatus的区别在于GetBit为直接从板卡中读取，而此GetIoStatus为从程序内存(由背景线程负责刷新)中读取.</summary>
        /// <param name="ioIdx">IO号(0-based)</param>
        /// <returns>返回IO状态(0/1).</returns>
        public static Int32 GetBit(Int32 ioIdx) { return HTM_API.HTM_GetBit(ioIdx); }
        /// <summary>IO置位操作。</summary>
        /// <param name="ioIdx">IO号(0-based)</param>
        /// <param name="ON">待置位的状态(0/1)</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 SetBit(Int32 ioIdx, Int32 ON) { return HTM_API.HTM_SetBit(ioIdx, ON); }
        /// <summary>检测单个IO信号变为高电平1，阻塞检测直到信号产生或超时。</summary>
        /// <param name="ioIdx">IO号(0-based)</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到，负值表示超时.</returns>
        public static Int32 CheckIO(Int32 ioIdx, Double timeout_second) { return HTM_API.HTM_CheckIO(ioIdx, timeout_second); }
        /// <summary> 检测两个IO信号变为高电平1，阻塞检测直到所有IO信号产生或超时</summary>
        /// <param name="ioIdx1">IO号1(0-based) </param>
        /// <param name="ioIdx2">IO号2(0-based)</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到，负值表示超时.</returns>
        public static Int32 CheckIO(Int32 ioIdx1, Int32 ioIdx2, Double timeout_second) { return HTM_API.HTM_CheckIO2(ioIdx1, ioIdx2, timeout_second); }
        /// <summary>检测三个IO信号变为高电平1，阻塞检测直到所有IO信号产生或超时</summary>
        /// <param name="ioIdx1">IO号1(0-based) </param>
        /// <param name="ioIdx2">IO号2(0-based) </param>
        /// <param name="ioIdx3">IO号3(0-based) </param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到，负值表示超时.</returns>
        public static Int32 CheckIO(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Double timeout_second) { return HTM_API.HTM_CheckIO3(ioIdx1, ioIdx2, ioIdx3, timeout_second); }
        /// <summary>检测四个IO信号变为高电平1，阻塞检测直到所有IO信号产生或超时</summary>
        /// <param name="ioIdx1">IO号1(0-based) </param>
        /// <param name="ioIdx2">IO号2(0-based) </param>
        /// <param name="ioIdx3">IO号3(0-based) </param>
        /// <param name="ioIdx4">IO号4(0-based) </param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到，负值表示超时.</returns>
        public static Int32 CheckIO(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Int32 ioIdx4, Double timeout_second) { return HTM_API.HTM_CheckIO4(ioIdx1, ioIdx2, ioIdx3, ioIdx4, timeout_second); }
        /// <summary>检测多个IO信号变为高电平1，阻塞检测直到所有IO信号产生或超时 </summary>
        /// <param name="ioList">IO号列表(0-based) </param>
        /// <param name="ioNum">IO数</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到，负值表示超时.</returns>
        public static Int32 CheckIO(Int32[] ioList, Int32 ioNum, Double timeout_second) { return HTM_API.HTM_CheckION(ioList, ioNum, timeout_second); }
        /// <summary>检测单个IO信号变为低电平0，阻塞检测直到信号消失或超时。</summary>
        /// <param name="ioIdx">IO号(0-based)</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到IO信号变为0，负值表示超时.</returns>
        public static Int32 CheckIOff(Int32 ioIdx, Double timeout_second) { return HTM_API.HTM_CheckIOff(ioIdx, timeout_second); }
        /// <summary>检测两个IO信号变为低电平0，阻塞检测直到所有IO信号消失或超时。 </summary>
        /// <param name="ioIdx1">IO号1(0-based)</param>
        /// <param name="ioIdx2">IO号2(0-based)</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到IO信号变为0，负值表示超时.</returns>
        public static Int32 CheckIOff(Int32 ioIdx1, Int32 ioIdx2, Double timeout_second) { return HTM_API.HTM_CheckIOff2(ioIdx1, ioIdx2, timeout_second); }
        /// <summary>检测三个IO信号变为低电平0，阻塞检测直到所有IO信号消失或超时。 </summary>
        /// <param name="ioIdx1">IO号1(0-based)</param>
        /// <param name="ioIdx2">IO号2(0-based)</param>
        /// <param name="ioIdx3">IO号3(0-based)</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到IO信号变为0，负值表示超时.</returns>
        public static Int32 CheckIOff(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Double timeout_second) { return HTM_API.HTM_CheckIOff3(ioIdx1, ioIdx2, ioIdx3, timeout_second); }
        /// <summary>检测四个IO信号变为低电平0，阻塞检测直到所有IO信号消失或超时。 </summary>
        /// <param name="ioIdx1">IO号1(0-based)</param>
        /// <param name="ioIdx2">IO号2(0-based)</param>
        /// <param name="ioIdx3">IO号3(0-based)</param>
        /// <param name="ioIdx4">IO号4(0-based)</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到IO信号变为0，负值表示超时.</returns>
        public static Int32 CheckIOff(Int32 ioIdx1, Int32 ioIdx2, Int32 ioIdx3, Int32 ioIdx4, Double timeout_second) { return HTM_API.HTM_CheckIOff4(ioIdx1, ioIdx2, ioIdx3, ioIdx4, timeout_second); }
        /// <summary>检测多个IO信号变为低电平0，阻塞检测直到所有IO信号消失或超时。 </summary>
        /// <param name="ioList">多个IO号列表</param>
        /// <param name="ioNum">IO数目</param>
        /// <param name="timeout_second">超时时间(单位：秒)</param>
        /// <returns>0表示检测到IO信号变为0，负值表示超时.</returns>
        public static Int32 CheckIOff(Int32[] ioList, Int32 ioNum, Double timeout_second) { return HTM_API.HTM_CheckIOffN(ioList, ioNum, timeout_second); }
        #endregion

        #region Other Device Operation APIs
        /// <summary>获取设备名称</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <returns>返回设备名字.</returns>
        public static String GetDeviceName(Int32 devIdx)
        {
            IntPtr pRet = HTM_API.HTM_GetDeviceName(devIdx);
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>设置点表模式位置触发使能</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <param name="ptIdx">点表序号(0-based)，-1表示对全部点操作 </param>
        /// <param name="enable">1-使能，0-不使能 </param>
        /// <returns>返回0代表成功，负数表示失败的错误码.</returns>
        public static Int32 SetPtTrigEnable(Int32 devIdx, Int32 ptIdx, Int32 enable) { return HTM_API.HTM_SetPtTrigEnable(devIdx, ptIdx, enable); }
        /// <summary>设置点表模式位置触发点位</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <param name="ptIdx">点表序号(0-based) </param>
        /// <param name="pos">触发位置 </param>
        /// <returns>返回0代表成功，负数表示失败的错误码.</returns>
        public static Int32 SetPtTrigPos(Int32 devIdx, Int32 ptIdx, Double pos) { return HTM_API.HTM_SetPtTrigPos(devIdx, ptIdx, pos); }
        /// <summary>获取点表模式位置触发点位 </summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <param name="ptIdx">点表序号(0-based) </param>
        /// <returns>返回点位(mm).</returns>
        public static Double GetPtTrigPos(Int32 devIdx, Int32 ptIdx) { return HTM_API.HTM_GetPtTrigPos(devIdx, ptIdx); }
        /// <summary>设置线性模式位置触发使能。</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <param name="enable">1-使能，0-不使能 </param>
        /// <returns>返回0代表成功，负数表示失败的错误码.</returns>
        public static Int32 SetLinTrigEnable(Int32 devIdx, Int32 enable) { return HTM_API.HTM_SetLinTrigEnable(devIdx, enable); }
        /// <summary>位置触发板软件触发信号输出。</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <returns>返回0代表成功，负数表示失败的错误码.</returns>
        public static Int32 SWPosTrig(Int32 devIdx) { return HTM_API.HTM_SWPosTrig(devIdx); }

        /// <summary>获取位置触发板已经触发的次数。</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <returns>返回次数.</returns>
        public static Int32 GetTrigCnt(Int32 devIdx) { return HTM_API.HTM_GetTrigCnt(devIdx); }
        /// <summary>  位置触发板已经触发的次数清零。 </summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <returns>返回0表示成功，负数表示错误码.</returns>
        public static Int32 ResetTrigCnt(Int32 devIdx) { return HTM_API.HTM_ResetTrigCnt(devIdx); }
        /// <summary> 同步触发板的位置和所绑定轴的位置。 </summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <returns>返回0表示成功，负数表示错误码.</returns>
        public static Int32 SyncTrigCurPos(Int32 devIdx) { return HTM_API.HTM_SyncTrigCurPos(devIdx); }
        #endregion

        #region Light Source Dirve
        /// <summary> 光源驱动板软件触发信号。</summary>
        /// <param name="devIdx">设备号(0-based)</param>
        /// <returns>返回0表示成功，负数表示错误码.</returns>
        public static Int32 SWLightTrig(Int32 devIdx) { return HTM_API.HTM_SWLightTrig(devIdx); }
        /// <summary>设置光源驱动板单通道的触发源。</summary>
        /// <param name="devIdx">设备号(0-based) </param>
        /// <param name="src_group">触发源组合(1, 2, 4, 8分别表示四个通道，按位与可组合触发信息) </param>
        /// <returns>返回0表示成功，负数表示错误码.</returns>
        public static Int32 SetLightTrigSrc(Int32 devIdx, UInt16 src_group) { return HTM_API.HTM_SetLightTrigSrc(devIdx, src_group); }
        /// <summary>设置光源驱动板单通道的触发时间.</summary>
        /// <param name="devIdx">设备号(0-based)</param>
        /// <param name="time_us">触发时间(微秒)</param>
        /// <returns>返回0表示成功，负数表示错误码.</returns>
        public static Int32 SetLightTrigTime(Int32 devIdx, Double time_us) { return HTM_API.HTM_SetLightTrigTime(devIdx, time_us); }
        #endregion

        #region Com Dirve
        /// <summary>获取串口号。</summary>
        /// <param name="devIdx">devIdx 设备号(0-based)</param>
        /// <returns>返回串口号.</returns>
        public static Int32 GetComNo(Int32 devIdx) { return HTM_API.HTM_GetComNo(devIdx); }
        /// <summary>打开串口。</summary>
        /// <param name="devIdx">设备号(0-based)</param>
        /// <returns>返回0表示成功，负数表示失败.</returns>
        public static Int32 OpenCome(Int32 devIdx) { return HTM_API.HTM_OpenCom(devIdx); }
        /// <summary> 关闭串口。</summary>
        /// <param name="devIdx">设备号(0-based)</param>
        /// <returns>返回0表示成功，负数表示失败.</returns>
        public static Int32 CloseCom(Int32 devIdx) { return HTM_API.HTM_CloseCom(devIdx); }
        #endregion

        #region  Utils lib
        /// <summary>获取从进程启动到函数调用时的秒数(s)</summary>
        /// <returns>返回从进程启动到调用该函数时的秒数（双精度double类型）.</returns>
        public static Double GetTimeSec() { return HTM_API.HTM_GetTimeSec(); }
        /// <summary>获取从进程启动到函数调用时的毫秒数(ms)</summary>
        /// <returns>返回从进程启动到调用该函数时的毫秒数（无浮整型unsigned int）.</returns>
        public static UInt32 GetTimeMs() { return HTM_API.HTM_GetTimeMs(); }
        /// <summary>获取系统当前的时间信息，格式HH:MM:SS(如10:58:38)</summary>
        /// <returns>返回系统时间信息字符串.</returns>
        public static String HTM_GetTimeStr()
        {
            IntPtr pRet = HTM_API.HTM_GetTimeStr();
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>获取系统当前的日期信息，格式DD-MM-YYYY(如12-18-2017)</summary>
        /// <returns>返回系统日期信息字符串.</returns>
        public static String HTM_GetDateStr()
        {
            IntPtr pRet = HTM_API.HTM_GetDateStr();
            return Marshal.PtrToStringAnsi(pRet);
        }
        /// <summary>获取系统当前的时分秒信息。</summary>
        /// <param name="hour">当前系统的时位，0~23(如当前时间10:58:38，则该值为10)</param>
        /// <param name="minute">当前系统的分位，0~59(如当前时间10:58:38，则该值为58)</param>
        /// <param name="second">当前系统的秒位，0~59(如当前时间10:58:38，则该值为38)</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 GetTime(out Int32 hour, out Int32 minute, out Int32 second) { return HTM_API.HTM_GetTime(out hour, out minute, out second); }
        /// <summary>获取系统当前的年月日信息。</summary>
        /// <param name="year">当前系统的年位(如当前时间2017年12月18日，则该值为2017)。</param>
        /// <param name="month">当前系统的月位，1~12(如当前时间2017年12月18日，则该值为12)。</param>
        /// <param name="day">当前系统的日位，1~31(如当前时间2017年12月18日，则该值为18)。</param>
        /// <returns>返回0代表成功，-1表示失败.</returns>
        public static Int32 GetDate(out Int32 year, out Int32 month, out Int32 day) { return HTM_API.HTM_GetDate(out year, out month, out day); }
        #endregion

        #region Analysis lib
        /// <summary>Calculate position after rotating an angle. 计算绕旋转中心转动一个角度后的坐标</summary>
        /// <param name="former_x">x value before rotaion 转动前x坐标</param>
        /// <param name="former_y">y value before rotaion 转动前y坐标</param>
        /// <param name="rot_center_x">x value of rotation center 旋转中心x坐标</param>
        /// <param name="rot_center_y">y value of rotation center 旋转中心y坐标</param>
        /// <param name="delta_deg">degree to rotate 待旋转的角度(度为单位)</param>
        /// <param name="new_x">pointer to new x value after rotatioin 旋转后的新x坐标</param>
        /// <param name="new_y">pointer to new y value after rotatioin 旋转后的新y坐标</param>
        public static void CalcRotatePos(Double former_x, Double former_y, Double rot_center_x, Double rot_center_y, Double delta_deg, out Double new_x, out Double new_y) { HTM_API.HTM_CalcRotatePos(former_x, former_y, rot_center_x, rot_center_y, delta_deg, out new_x, out new_y); }
        /// <summary>求一组数的算术平均值，可以选择去除最大和最小的若干个值后计算</summary>
        /// <param name="input_array">待求数组</param>
        /// <param name="total_num">数组总长度</param>
        /// <param name="delete_max_num">待删除的最大数数量(可以为0)</param>
        /// <param name="delete_min_num">待删除的最小数数量(可以为0)</param>
        /// <param name="mean_val">输出，去掉若干最大值和最小值后这组数的平均值</param>
        /// <returns>返回0表示正确，负数表示传入数据错误.</returns>
        public static Int32 Mean(Double[] input_array, Int32 total_num, Int32 delete_max_num, Int32 delete_min_num, out Double mean_val) { return HTM_API.HTM_Mean(input_array, total_num, delete_max_num, delete_min_num, out mean_val); }
        /// <summary>一位数组排序，输入输出可以为同一个数组 </summary>
        /// <param name="input_array">输入的一位数组首地址 </param>
        /// <param name="total_num">数组的长度 </param>
        /// <param name="descending">1-表示降序排列，0-表示升序排列 </param>
        /// <param name="output_array">输出排列好的数组 </param>
        /// <returns>返回0表示成功，返回负值表示失败</returns>
        public static Int32 Sort(Double[] input_array, Int32 total_num, Boolean descending, Double[] output_array) { return HTM_API.HTM_Sort(input_array, total_num, Convert.ToInt32(descending), output_array); }
        /// <summary>求一位数组最大值和最小值</summary>
        /// <param name="input_array">输入的一位数组首地址</param>
        /// <param name="total_num">数组的长度</param>
        /// <param name="max">最大值</param>
        /// <param name="max_id">第一个最大值在数组中的位置(0-based)</param>
        /// <param name="min">最小值</param>
        /// <param name="min_id">第一个最小值在数组中的位置(0-based)</param>
        /// <returns>返回0表示成功，返回负值表示失败</returns>
        public static Int32 MaxMin(Double[] input_array, Int32 total_num, out Double max, out Int32 max_id, out Double min, out Int32 min_id) { return HTM_API.HTM_MaxMin(input_array, total_num, out max, out max_id, out min, out min_id); }
        /// <summary>求一位数组之和</summary>
        /// <param name="input_array">输入的一位数组首地址</param>
        /// <param name="total_num">数组的长度</param>
        /// <param name="sum">数组之和</param>
        /// <returns>返回0表示成功，返回负值表示失败</returns>
        public static Int32 Sum(Double[] input_array, Int32 total_num, out Double sum) { return HTM_API.HTM_Sum(input_array, total_num, out sum); }
        /// <summary>矩阵相乘</summary>
        /// <param name="A_aXn">A矩阵首地址，a行n列 </param>
        /// <param name="B_nXb">B矩阵首地址，n行b列 </param>
        /// <param name="rowOfA_a">A矩阵行数 </param>
        /// <param name="row_colOfAB_n">A矩阵列数和B矩阵行数 </param>
        /// <param name="colOfB_b">B矩阵列数 </param>
        /// <param name="AB_aXb">所得矩阵，a行b列 </param>
        /// <returns>返回0表示成功，返回负值表示失败</returns>
        public static Int32 MatrixMul(Double[] A_aXn, Double[] B_nXb, Int32 rowOfA_a, Int32 row_colOfAB_n, Int32 colOfB_b, Double[] AB_aXb) { return HTM_API.HTM_MatrixMul(A_aXn, B_nXb, rowOfA_a, row_colOfAB_n, colOfB_b, AB_aXb); }
        /// <summary>矩阵求逆</summary>
        /// <param name="input_matrix">输入矩阵首地址 </param>
        /// <param name="dimensionNum">方阵的阶数 </param>
        /// <param name="output_matrix">逆矩阵的首地址 </param>
        /// <returns>返回0表示成功，返回负值表示失败</returns>
        public static Int32 MatrixInv(Double[] input_matrix, Int32 dimensionNum, Double[] output_matrix) { return HTM_API.HTM_MatrixInv(input_matrix, dimensionNum, output_matrix); }
        /// <summary>矩阵转置</summary>
        /// <param name="input_matrix">输入矩阵首地址 </param>
        /// <param name="row_num">矩阵的行数 </param>
        /// <param name="col_num">矩阵的列数 </param>
        /// <param name="output_matrix">所求转置矩阵的首地址 </param>
        /// <returns></returns>
        public static Int32 MatrixTranspose(Double[] input_matrix, Int32 row_num, Int32 col_num, Double[] output_matrix) { return HTM_API.HTM_MatrixTranspose(input_matrix, row_num, col_num, output_matrix); }
        /// <summary>最小二乘法拟合直线，输出斜率和截距 y = k*x + b</summary>
        /// <param name="x">X坐标数组 </param>
        /// <param name="y">Y坐标数组 </param>
        /// <param name="numberOfPoints">坐标点个数 </param>
        /// <param name="k">拟合直线的斜率 </param>
        /// <param name="b">拟合直线的截距 </param>
        /// <returns>返回0表示拟合成功，返回负值表示失败</returns>
        public static Int32 LineFit(Double[] x, Double[] y, Int32 numberOfPoints, out Double k, out Double b) { return HTM_API.HTM_LineFit(x, y, numberOfPoints, out k, out b); }
        /// <summary>多项式拟合曲线，输出系数 y = k0 + k1x + k2*x^2 + ... + kn*x^n</summary>
        /// <param name="x">X坐标数组 </param>
        /// <param name="y">Y坐标数组 </param>
        /// <param name="numberOfPoints">坐标点个数 </param>
        /// <param name="order">多项式次数 </param>
        /// <param name="k">拟合出多项式的系数，系数的个数=order+1 </param>
        /// <returns>返回0表示拟合成功，返回负值表示失败</returns>
        public static Int32 PolyFit(Double[] x, Double[] y, Int32 numberOfPoints, Int32 order, out Double k) { return HTM_API.HTM_PolyFit(x, y, numberOfPoints, order, out k); }
        /// <summary>最小二乘法拟合圆，输出圆心和半径</summary>
        /// <param name="x">X坐标数组 </param>
        /// <param name="y">Y坐标数组 </param>
        /// <param name="numberOfPoints">坐标点个数 </param>
        /// <param name="centerX">拟合出的圆心X坐标 </param>
        /// <param name="centerY">拟合出的圆心Y坐标 </param>
        /// <param name="r">拟合出的圆半径 </param>
        /// <returns>返回0表示拟合成功，返回-999表示输入参数错误，返回-2表示拟合失败</returns>
        public static Int32 CircleFit(Double[] x, Double[] y, Int32 numberOfPoints, out Double centerX, out Double centerY, out Double r) { return HTM_API.HTM_CircleFit(x, y, numberOfPoints, out centerX, out centerY, out r); }
        /// <summary>直线插值计算中间值(x数组必须是递增的，即x[i]小于x[i+1])</summary>
        /// <param name="x">X坐标数组 </param>
        /// <param name="y">Y坐标数组 </param>
        /// <param name="numberOfElements">坐标点个数 </param>
        /// <param name="xValue">待插值的x位置 </param>
        /// <param name="interpolatedYValue">插出的数值 </param>
        /// <returns>返回0表示拟合成功，返回-999表示输入参数错误</returns>
        public static Int32 LineInterp(Double[] x, Double[] y, Int32 numberOfElements, Double xValue, out Double interpolatedYValue) { return HTM_API.HTM_LineInterp(x, y, numberOfElements, xValue, out interpolatedYValue); }
        /// <summary>计算两组矩阵的转换矩阵</summary>
        /// <param name="A_nXa">A矩阵n行a列 </param>
        /// <param name="B_nXb">B矩阵n行b列 </param>
        /// <param name="numberOfRows_n">两个矩阵的行数 </param>
        /// <param name="dimensionOfA_a">  A矩阵的列数 </param>
        /// <param name="dimensionOfB_b"> B矩阵的列数 </param>
        /// <param name="A2B_aplus1Xb"> 计算出的矩阵(a+1)行b列 </param>
        /// <returns>返回0表示成功,返回负值表示失败。</returns>
        public static Int32 CalcTransMtx(Double[] A_nXa, Double[] B_nXb, Int32 numberOfRows_n, Int32 dimensionOfA_a, Int32 dimensionOfB_b, Double[] A2B_aplus1Xb) { return HTM_API.HTM_CalcTransMtx(A_nXa, B_nXb, numberOfRows_n, dimensionOfA_a, dimensionOfB_b, A2B_aplus1Xb); }
        /// <summary>根据转换关系和给定矩阵计算另一个矩阵</summary>
        /// <param name="A_1Xa">A矩阵1行a列</param>
        /// <param name="A2B_aplus1Xb">A到B的转换关系</param>
        /// <param name="dimensionOfA_a">A矩阵的列数</param>
        /// <param name="dimensionOfB_b">B矩阵的列数</param>
        /// <param name="B_1Xb">输出的坐标</param>
        /// <returns>返回0表示成功,返回负值表示失败。</returns>
        public static Int32 TransPos(Double[] A_1Xa, Double[] A2B_aplus1Xb, Int32 dimensionOfA_a, Int32 dimensionOfB_b, Double[] B_1Xb) { return HTM_API.HTM_TransPos(A_1Xa, A2B_aplus1Xb, dimensionOfA_a, dimensionOfB_b, B_1Xb); }
        #endregion
    }
    #endregion
}