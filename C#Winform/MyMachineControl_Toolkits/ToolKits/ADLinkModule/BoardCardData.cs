namespace ToolKits.ADLinkModule
{
    #region IO抽象定义(名称，所在板卡号，所在端口号，有效电平，简单IO设备，气缸设备)
    /// <summary>
    /// 电平模式枚举（一般规定低电平为0~0.25V，高电平为3.5~5V，一般低电平表示0，高电平表示1。一般规定低电平为0~0.25V，高电平为3.5~5V。
    /// </summary>
    public enum ElectricalLevelMode : byte
    {
        /// <summary>
        /// 正逻辑高电平
        /// </summary>
        HighPos = GeneralElectricalLevelMode.High,
        /// <summary>
        /// 正逻辑低电平
        /// </summary>
        LowPos = GeneralElectricalLevelMode.Low,
        /// <summary>
        /// 负逻辑高电平
        /// </summary>
        HighNeg = GeneralElectricalLevelMode.Low,
        /// <summary>
        /// 负逻辑低电平
        /// </summary>
        LowNeg = GeneralElectricalLevelMode.High,
    }

    /// <summary>
    /// 通用电平高低模式枚举
    /// </summary>
    public enum GeneralElectricalLevelMode : byte
    {
        /// <summary>
        /// 低电平0
        /// </summary>
        Low = 0,
        /// <summary>
        /// 高电平1
        /// </summary>
        High = 1,
    }

    /// <summary>
    /// IO端口号(封装IO所在的板卡号，端口号及是否是DO口)
    /// </summary>
    public struct IOPort
    {
        #region 字段
        private int m_cardNo;
        private int m_portNo;
        private bool m_isOutputIO;
        #endregion

        #region 属性
        /// <summary>
        /// 板卡号
        /// </summary>
        public int CardNo
        {
            get { return m_cardNo; }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public int PortNo
        {
            get { return m_portNo; }
        }

        /// <summary>
        /// true为输出IO，false为输入IO
        /// </summary>
        public bool IsOutputIO
        {
            get { return m_isOutputIO; }
        }
        #endregion

        #region 构造器
        /// <summary>
        /// 输出IO专用构造器
        /// </summary>
        /// <param name="portNo"></param>
        public IOPort(int portNo)
            : this(-1, portNo, true)
        { }

        /// <summary>
        /// 针对只用端口号标识的板卡
        /// </summary>
        /// <param name="portNo"></param>
        public IOPort(int portNo, bool isOutputIO)
            : this(-1, portNo, isOutputIO)
        { }

        /// <summary>
        /// 针对用板卡号和端口来标识的板卡
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="portNo"></param>
        public IOPort(int cardNo, int portNo, bool isOutputIO)
        {
            this.m_cardNo = cardNo;
            this.m_portNo = portNo;
            this.m_isOutputIO = isOutputIO;
        }
        #endregion
    }

    /// <summary>
    /// 只有一个输入输出端口的简单设备（如单个报警灯）
    /// </summary>
    public class SimpleIODevice
    {
        #region 字段
        private IOPort m_outputPort;
        private ElectricalLevelMode m_validELevelMode;
        private string m_deviceName;
        #endregion

        #region 属性
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName
        {
            get { return m_deviceName; }
        }

        /// <summary>
        /// 输入输出端口号（十进制）
        /// </summary>
        public IOPort IOPort
        {
            get { return m_outputPort; }
        }

        /// <summary>
        /// 有效电平类型
        /// </summary>
        public ElectricalLevelMode ValidELevelMode
        {
            get { return m_validELevelMode; }
        }

        /// <summary>
        /// true为输出设备，false为输入设备
        /// </summary>
        public bool IsOutputDevice
        {
            get { return m_outputPort.IsOutputIO; }
        }
        #endregion

        #region 构造器
        /// <summary>
        /// 正逻辑高电平有效（即开启）的输出设备构造器
        /// </summary>
        /// <param name="iOPort"></param>
        public SimpleIODevice(IOPort iOPort)
            : this("", iOPort)
        { }

        /// <summary>
        /// 正逻辑高电平有效（即开启）的输出设备构造器
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="iOPort"></param>
        public SimpleIODevice(string deviceName, IOPort iOPort)
            : this(deviceName, iOPort, ElectricalLevelMode.HighPos)
        { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="iOPort"></param>
        /// <param name="validELevelMode"></param>
        public SimpleIODevice(string deviceName, IOPort iOPort, ElectricalLevelMode validELevelMode)
        {
            this.m_deviceName = deviceName;
            this.m_outputPort = iOPort;
            this.m_validELevelMode = validELevelMode;
        }
        #endregion
    }

    /// <summary>
    /// 气缸当前状态
    /// </summary>
    public enum AirCylinderState : byte
    {
        /// <summary>
        /// 打开
        /// </summary>
        Opened,
        /// <summary>
        /// 关闭
        /// </summary>
        Closed,
        /// <summary>
        /// 未知（气缸可能正在运行至指定位置，或者已经损坏）
        /// </summary>
        Unknown = 255
    }

    /// <summary>
    /// 气缸（气缸收缩为关闭，伸出为打开）
    /// </summary>
    public class AirCylinder
    {
        #region 字段
        private SimpleIODevice m_switch;
        private SimpleIODevice m_openStateIO;
        private SimpleIODevice m_closeStateIO;
        private string m_deviceName;
        #endregion

        #region 属性
        /// <summary>
        /// 开关(输出IO)
        /// </summary>
        public SimpleIODevice SwitchIO
        {
            get { return m_switch; }
        }

        /// <summary>
        /// 开状态IO(输入IO)
        /// </summary>
        public SimpleIODevice OpenStateIO
        {
            get { return m_openStateIO; }
        }

        /// <summary>
        /// 关状态IO(输入IO)
        /// </summary>
        public SimpleIODevice CloseStateIO
        {
            get { return m_closeStateIO; }
        }

        /// <summary>
        /// 气缸名称
        /// </summary>
        public string DeviceName
        {
            get { return m_deviceName; }
        }
        #endregion

        #region 构造器
        /// <summary>
        /// 气缸构造器
        /// </summary>
        /// <param name="deviceName">气缸名称</param>
        /// <param name="switch">开关输出口</param>
        /// <param name="openStateIO">开状态IO</param>
        /// <param name="closeStateIO">关状态IO</param>
        public AirCylinder(string deviceName, SimpleIODevice @switch, SimpleIODevice openStateIO, SimpleIODevice closeStateIO)
        {
            m_deviceName = deviceName;
            m_switch = @switch;
            m_openStateIO = openStateIO;
            m_closeStateIO = closeStateIO;
        }
        #endregion
    }
    #endregion

    #region 运动抽象定义（轴位置信息，轴传动信息，轴速信息）
    #region 限位停止方式枚举
    /// <summary>
    /// 限位停止方式
    /// </summary>
    public enum ELStopType : byte
    {
        /// <summary>
        /// 立即停止
        /// </summary>
        ImmediateStop = 0,
        /// <summary>
        /// 减速停止
        /// </summary>
        DecelStop = 1,
    }
    #endregion

    #region 运动方向枚举
    /// <summary>
    /// 运动方向
    /// </summary>
    public enum MoveDirection
    {
        /// <summary>
        /// 正向
        /// </summary>
        Postive = 1,
        /// <summary>
        /// 反向
        /// </summary>
        Negative = -1,
    }
    #endregion

    #region 速度曲线枚举
    /// <summary>
    /// 速度曲线类型
    /// </summary>
    public enum VelocityCurveType : byte
    {
        /// <summary>
        /// 梯形速度曲线
        /// </summary>
        Trapezoid,
        /// <summary>
        /// S型速度曲线
        /// </summary>
        SCurve,
        /// <summary>
        /// 未指定速度曲线类型
        /// </summary>
        None = 255,
    }
    #endregion

    #region 板卡脉冲输出模式枚举(暂时不用)
    /*
    /// <summary>
    /// 脉冲模式
    /// </summary>
    public enum PulseMode
    {
        /// <summary>
        /// pulse/dir模式，脉冲上升沿有效
        /// </summary>
        PulseDirAsc = 0,
        /// <summary>
        /// pulse/dir模式，脉冲下降沿有效
        /// </summary>
        PulseDirDes = 1,

        /// <summary>
        /// pulse/dir模式，脉冲上升沿有效 低电平正向
        /// </summary>
        PulseDirAscLow = 0,
        /// <summary>
        /// pulse/dir模式，脉冲下降沿有效 低电平正向
        /// </summary>
        PulseDirDesLow = 1,
        /// <summary>
        /// pulse/dir模式，脉冲上升沿有效 高电平正向
        /// </summary>
        PulseDirAscHigh = 2,
        /// <summary>
        /// pulse/dir模式，脉冲下降沿有效 高电平正向
        /// </summary>
        PulseDirDesHigh = 3,

        /// <summary>
        /// CW/CCW模式，脉冲上升沿有效
        /// </summary>
        CWCCWAsc = 4,
        /// <summary>
        /// CW/CCW模式，脉冲下降沿有效
        /// </summary>
        CWCCWDes = 5,
    }
    */
    #endregion

    #region 轴运行状态(暂时不用)
    /*
    /// <summary>
    /// 轴运行状态枚举（非0的都是停止运行）
    /// </summary>
    public enum AxisRunState
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        Running = 0,

        /// <summary>
        /// 已停止运行
        /// </summary>
        Stopped = 1,

        /// <summary>
        /// 脉冲输入完毕
        /// </summary>
        PulseOver = 1,

        /// <summary>
        /// 命令停止（如调用了DecelStop函数）
        /// </summary>
        CommandStop = 2,

        /// <summary>
        /// 限位停止
        /// </summary>
        ELStop = 3,

        /// <summary>
        /// 遇原点停止
        /// </summary>
        ORGStop = 4,
    }
    */
    #endregion

    #region 轴信号状态
    /// <summary>
    /// 轴各信号状态枚举(1为该信号已触发发，0则未触发，不代表正式的高低电平)
    /// </summary>
    public class AxisSignalState
    {
        /// <summary>
        /// 正限位
        /// </summary>
        public GeneralElectricalLevelMode PosEL;

        /// <summary>
        /// 负限位
        /// </summary>
        public GeneralElectricalLevelMode NegEL;

        /// <summary>
        /// 原点信号
        /// </summary>
        public GeneralElectricalLevelMode ORG;

        /******************以下是为伺服准备******************/
        /// <summary>
        /// ALM信号
        /// </summary>
        public GeneralElectricalLevelMode ALM;

        /// <summary>
        /// EMG信号
        /// </summary>
        public GeneralElectricalLevelMode EMG;

        /// <summary>
        /// INP信号
        /// </summary>
        public GeneralElectricalLevelMode INP;

        /// <summary>
        /// 初始化为未触发
        /// </summary>
        public AxisSignalState()
        {
            EMG = INP = ALM = ORG = NegEL = PosEL = GeneralElectricalLevelMode.Low;
        }
    }
    #endregion

    #region 轴位置信息(轴所在卡号，站号，插槽号)
    /// <summary>
    /// 定位轴用的参数
    /// </summary>
    public class AxisLocationInfo
    {
        #region 字段
        private int m_cardNo;
        private int m_nodeNo;
        private int m_slotNo;
        private int m_axisNo;

        /// <summary>
        /// Z轴还是R周(只有景焱的HTnet运动控制板需要该参数)
        /// </summary>
        private int m_zOrr;

        /// <summary>
        /// 是否有编码器接入(步进电机无外部编码器)
        /// </summary>
        private int m_hasExtEncode;
        #endregion

        #region 属性
        /// <summary>
        /// 板卡号
        /// </summary>
        public int CardNo
        {
            get { return m_cardNo; }
            set { m_cardNo = value; }
        }

        /// <summary>
        /// 站号
        /// </summary>
        public int NodeNo
        {
            get { return m_nodeNo; }
            set { m_nodeNo = value; }
        }

        /// <summary>
        /// 插槽号
        /// </summary>
        public int SlotNo
        {
            get { return m_slotNo; }
            set { m_slotNo = value; }
        }

        /// <summary>
        /// 轴号
        /// </summary>
        public int AxisNo
        {
            get { return m_axisNo; }
            set { m_axisNo = value; }
        }

        /// <summary>
        /// Z轴还是R周(只有景焱的HTnet运动控制板需要该参数)
        /// </summary>
        public int ZOrR
        {
            get { return m_zOrr; }
            set { m_zOrr = value; }
        }

        /// <summary>
        /// 是否有编码器接入(步进电机无外部编码器)
        /// </summary>
        public int HasExtEncode
        {
            get { return m_hasExtEncode; }
            set { m_hasExtEncode = value; }
        }
        #endregion

        #region 构造器
        /// <summary>
        /// 针对用板卡号,站号,插槽号标识的板卡(针对台达板卡)
        /// </summary>
        /// <param name="cardNo">板卡号</param>
        /// <param name="nodeNo">站号</param>
        /// <param name="slotNo">插槽号</param>
        /// <param name="hasExtEncode">是否有外部编码器</param>
        public AxisLocationInfo(int cardNo, int nodeNo, int slotNo, int hasExtEncode)
            : this(cardNo, nodeNo, slotNo, -1, hasExtEncode, -1)
        {
        }

        /// <summary>
        /// 针对有板卡号,站号的板卡(景焱HTNet板卡，默认有编码器信号)
        /// </summary>
        /// <param name="cardNo">板卡号</param>
        /// <param name="nodeNo">站号</param>
        /// <param name="zOrR">是否是R轴</param>
        public AxisLocationInfo(int cardNo, int nodeNo, int zOrR)
            : this(cardNo, nodeNo, -1, -1, 1, zOrR)
        {
        }

        /// <summary>
        /// 针对只用轴号标示的板卡(凌华板卡)
        /// </summary>
        /// <param name="axisNo"></param>
        public AxisLocationInfo(int axisNo, int hasExtEncode)
            : this(-1, -1, -1, axisNo, hasExtEncode, -1)
        {
        }

        public AxisLocationInfo(int cardNo, int nodeNo, int slotNo, int axisNo, int hasExtEncode, int zOrR)
        {
            this.m_cardNo = cardNo;
            this.m_nodeNo = nodeNo;
            this.m_slotNo = slotNo;
            this.m_axisNo = axisNo;
            this.m_hasExtEncode = hasExtEncode;
            this.m_zOrr = zOrR;
        }
        #endregion
    }
    #endregion

    #region 轴传动信息(步距角，导程，脉冲当量)
    /// <summary>
    /// 传动参数
    /// </summary>
    public class TransmissionParams
    {
        #region 字段
        /// <summary>
        /// 步距角
        /// </summary>
        private double m_stepAngle;

        /// <summary>
        /// 导程
        /// </summary>
        private double m_lead;

        /// <summary>
        /// 细分数(几倍细分)
        /// </summary>
        private int m_subDivisionNum;

        /// <summary>
        /// 当量脉冲 = 一圈的脉冲数/导程
        /// </summary>
        private double m_equivalentPulse;


        #endregion

        #region 属性
        /// <summary>
        /// 电机步距角
        /// </summary>
        public double StepAngle
        {
            get { return m_stepAngle; }
        }

        /// <summary>
        /// 导程(mm/周，度/周)
        /// </summary>
        public double Lead
        {
            get { return m_lead; }
        }

        /// <summary>
        /// 细分数
        /// </summary>
        public int SubDivisionNum
        {
            get { return m_subDivisionNum; }
        }

        /// <summary>
        /// 当量脉冲(如：p/mm，p/度)
        /// </summary>
        public double EquivalentPulse
        {
            get { return m_equivalentPulse; }
        }

        /// <summary>
        /// 脉冲当量(如：mm/p，度/p)
        /// </summary>
        public double PulseEquivalent
        {
            get
            {
                if (m_equivalentPulse == 0)
                    throw new System.Exception("脉冲当量为0");
                return 1 / m_equivalentPulse;
            }
        }


        #endregion

        #region 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="stepAngle">步距角</param>
        /// <param name="lead">导程</param>
        /// <param name="subDivisionNum">细分数</param>
        public TransmissionParams(double stepAngle, double lead, int subDivisionNum)
        {
            this.m_stepAngle = stepAngle;
            this.m_lead = lead;
            this.m_subDivisionNum = subDivisionNum;

            this.m_equivalentPulse = GetPulseEquivalent(stepAngle, subDivisionNum, lead);
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="pulsePerRound">旋转一圈所需脉冲数</param>
        /// <param name="lead">一圈所走的距离</param>
        public TransmissionParams(double pulsePerRound, double lead)
        {
            this.m_stepAngle = -1;
            this.m_subDivisionNum = -1;
            if (lead == 0)
                this.m_equivalentPulse = -1;
            else
                this.m_equivalentPulse = pulsePerRound / lead;
            this.m_lead = lead;
        }

        /// <summary>
        /// 只关心脉冲当量
        /// </summary>
        /// <param name="pulseEquivalent">脉冲当量，1个脉冲代表的实际物理位置</param>
        //public TransmissionParams(double pulsePerRound)
        //{
        //    this.m_stepAngle = -1;
        //    this.m_subDivisionNum = -1;

        //    this.m_equivalentPulse = pulsePerRound / 360d;
        //    this.m_lead = -1;
        //}
        public TransmissionParams(double pulseEquivalent)
        {
            this.m_stepAngle = -1;
            this.m_subDivisionNum = -1;

            this.m_equivalentPulse = 1 / pulseEquivalent;
            this.m_lead = -1;
        }

        public TransmissionParams()
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取脉冲当量
        /// </summary>
        /// <param name="stepAngle">电机的步距角</param>
        /// <param name="subDivisionNum">细分数</param>
        /// <param name="lead">导程(mm/周，度/周)</param>
        /// <returns>返回设置好的当量值</returns>
        public static double GetPulseEquivalent(double stepAngle, double subDivisionNum, double lead)
        {
            return (360 * subDivisionNum) / (stepAngle * lead);
        }

        /// <summary>
        /// 通过脉冲计数来获取具体距离，角度值(如：p/mm，p/度)
        /// </summary>
        public double GetTansferValue(int pulseNum)
        {
            return pulseNum / m_equivalentPulse;
        }
        #endregion
    }
    #endregion

    #region 轴卡有效电平配置
    /// <summary>
    /// 轴卡有效电平配置
    /// </summary>
    public class AxisSignalParams
    {
        #region 字段
        private int m_elLogic;	      //限位信号电平 0-not inverse, 1-inverse
        private int m_ogrLogic;	      //ORG信号 0-not inverse, 1-inverse
        private int m_almLogic;       //ALM信号 0-low active,1-high active
        private int m_ezLogic;        //EZ信号 0-low active,1-high active
        private int m_inpLogic;       //INP信号 0-low active,1-high active
        private int m_servoLogic;     //SERVO信号 0-low active,1-high active
        private int m_plsOutMode;     //脉冲输出模式：
        private int m_plsInMode;      //脉冲输入模式：
        private int m_encodeDir;      //脉冲方向
        #endregion

        #region 属性
        /// <summary>
        /// 限位信号电平 0-not inverse, 1-inverse
        /// </summary>
        public int ElLogic
        {
            get { return m_elLogic; }
            set { m_elLogic = value; }
        }

        /// <summary>
        /// ORG信号 0-not inverse, 1-inverse
        /// </summary>
        public int OrgLogic
        {
            get { return m_ogrLogic; }
            set { m_ogrLogic = value; }
        }

        /// <summary>
        /// ALM信号 0-low active,1-high active
        /// </summary>
        public int AlmLogic
        {
            get { return m_almLogic; }
            set { m_almLogic = value; }
        }

        /// <summary>
        /// EZ信号 0-low active,1-high active
        /// </summary>
        public int EzLogic
        {
            get { return m_ezLogic; }
            set { m_ezLogic = value; }
        }

        /// <summary>
        /// INP信号 0-low active,1-high active
        /// </summary>
        public int InpLogic
        {
            get { return m_inpLogic; }
            set { m_inpLogic = value; }
        }

        /// <summary>
        /// SERVO信号 0-low active,1-high active
        /// </summary>
        public int ServoLogic
        {
            get { return m_servoLogic; }
            set { m_servoLogic = value; }
        }

        /// <summary>
        /// 脉冲输出模式：
        /// </summary>
        public int PlsOutMode
        {
            get { return m_plsOutMode; }
            set { m_plsOutMode = value; }
        }

        /// <summary>
        /// 脉冲输入模式：
        /// </summary>
        public int PlsInMode
        {
            get { return m_plsInMode; }
            set { m_plsInMode = value; }
        }

        /// <summary>
        /// 脉冲方向 0-positive, 1-negative
        /// </summary>
        public int EncodeDir
        {
            get { return m_encodeDir; }
            set { m_encodeDir = value; }
        }
        #endregion

        #region 构造器
        public AxisSignalParams()
        {
        }

        public AxisSignalParams(int elLogic, int orgLogic, int almLogic, int ezLogic, int inpLogic, int servoLogic, int plsOutMode, int plsInMode, int encodeDir)
        {
            this.m_elLogic = elLogic ;	      //限位信号电平 0-not inverse, 1-inverse
            this.m_ogrLogic = orgLogic;	      //ORG信号 0-not inverse, 1-inverse
            this.m_almLogic = almLogic;       //ALM信号 0-low active,1-high active
            this.m_ezLogic = ezLogic;        //EZ信号 0-low active,1-high active
            this.m_inpLogic = inpLogic;       //INP信号 0-low active,1-high active
            this.m_servoLogic = servoLogic;     //SERVO信号 0-low active,1-high active
            this.m_plsOutMode = plsOutMode;     //脉冲输出模式：
            this.m_plsInMode = plsInMode;      //脉冲输入模式：
            this.m_encodeDir = encodeDir;      //脉冲方向

        }        
        #endregion
    }
    #endregion

    #region 轴速度配置
    /// <summary>
    /// 速度曲线参数
    /// </summary>
    public class VelocityCurveParams
    {
        #region 字段
        private double m_strvel;	  //Starting velocity  (mm/s)
        private double m_maxvel;	  //max velocity	   (mm/s)
        private double m_tacc;	      //acceleration time  (m/s2)
        private double m_tdec;	      //deceleration time  (m/s2)

        private double m_svacc;       //S-curve acc disition (mm) 
        private double m_svdec;       //S-curve dec disition (mm) 

        private double m_sfac;        //S-Factor(0表示T形曲线)

        private VelocityCurveType m_velocityCurveType;

        private double m_timeout;     //运动超时时间
        #endregion

        #region 属性
        /// <summary>
        /// 开始速度pps
        /// </summary>
        public double Strvel
        {
            get { return m_strvel; }
            set { m_strvel = value; }
        }

        /// <summary>
        /// 最大速度pps（即稳定运行的速度）
        /// </summary>
        public double Maxvel
        {
            get { return m_maxvel; }
            set { m_maxvel = value; }
        }

        /// <summary>
        /// 加速时间s
        /// </summary>
        public double Tacc
        {
            get { return m_tacc; }
            set { m_tacc = value; }
        }

        /// <summary>
        /// 减速时间s
        /// </summary>
        public double Tdec
        {
            get { return m_tdec; }
            set { m_tdec = value; }
        }

        /// <summary>
        /// 加速度圆角（S型运动曲线专用，其他的类型时为零）
        /// </summary>
        public double Svacc
        {
            get { return m_svacc; }
            set { m_svacc = value; }
        }

        /// <summary>
        /// 减速度圆角（S型运动曲线专用，其他的类型时为零）
        /// </summary>
        public double Svdec
        {
            get { return m_svdec; }
            set { m_svdec = value; }
        }

        /// <summary>
        /// S曲线因子
        /// </summary>
        public double Sfac
        {
            get { return m_sfac; }
            set { m_sfac = value; }
        }

        /// <summary>
        /// 速度曲线类型
        /// </summary>
        public VelocityCurveType VelocityCurveType
        {
            get { return m_velocityCurveType; }
            set { m_velocityCurveType = value; }
        }

        /// <summary>
        /// 运动超时时间
        /// </summary>
        public double Timeout
        {
            get { return m_timeout; }
            set { m_timeout = value; }
        }
        #endregion

        #region 构造器
        public VelocityCurveParams()
        {

        }

        /// <summary>
        /// 等腰梯形的梯形速度曲线
        /// </summary>
        /// <param name="strvel">起始速度，pps</param>
        /// <param name="maxvel">正常速度，pps</param>
        /// <param name="tacc">加速时间，单位：s</param>
        public VelocityCurveParams(double strvel, double maxvel, double tacc)
            : this(strvel, maxvel, tacc, tacc, 0, 0, VelocityCurveType.Trapezoid)
        {
        }

        /// <summary>
        /// 加减速不对称的梯形速度曲线
        /// </summary>
        /// <param name="strvel"></param>
        /// <param name="maxvel"></param>
        /// <param name="tacc"></param>
        /// <param name="tdec">减速时间</param>
        public VelocityCurveParams(double strvel, double maxvel, double tacc, double tdec)
            : this(strvel, maxvel, tacc, tdec, 0, 0, VelocityCurveType.Trapezoid)
        {
        }

        /// <summary>
        ///  加减速不对称的S形速度曲线
        /// </summary>
        /// <param name="strvel">pps</param>
        /// <param name="maxvel"></param>
        /// <param name="tacc"></param>
        /// <param name="tdec"></param>
        /// <param name="svacc">加速圆角半径</param>
        /// <param name="svdec">减速圆角半径</param>
        public VelocityCurveParams(double strvel, double maxvel, double tacc, double tdec, double svacc, double svdec)
            : this(strvel, maxvel, tacc, tdec, svacc, svdec, VelocityCurveType.SCurve)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strvel">开始速度pps</param>
        /// <param name="maxvel">最大速度pps（即稳定运行的速度）</param>
        /// <param name="tacc"> 加速时间s</param>
        /// <param name="tdec">减速时间s</param>
        /// <param name="svacc">加速度圆角（S型运动曲线专用，其他的类型时为零）</param>
        /// <param name="svdec">减速度圆角（S型运动曲线专用，其他的类型时为零）</param>
        /// <param name="velocityCurveType">速度曲线类型</param>
        public VelocityCurveParams(double strvel, double maxvel, double tacc, double tdec, double svacc, double svdec, VelocityCurveType velocityCurveType)
        {
            this.m_strvel = strvel;
            this.m_maxvel = maxvel;
            this.m_tacc = tacc;
            this.m_tdec = tdec;
            this.m_svacc = svacc;
            this.m_svdec = svdec;

            this.m_velocityCurveType = velocityCurveType;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 复制个完整的副本
        /// </summary>
        /// <returns></returns>
        public VelocityCurveParams Clone()
        {
            return new VelocityCurveParams(this.m_strvel, this.m_maxvel, this.m_tacc, this.m_tdec, this.m_svacc, this.m_svdec, this.m_velocityCurveType);
        }
        #endregion
    }
    #endregion

    #region 软限位配置
    /// <summary>
    /// 软限位参数
    /// </summary>
    public class SoftLimitParams
    {
        #region 字段
        private bool m_sen;	          //软限位使能
        private double m_spel;	      //正限位
        private double m_smel;	      //负限位
        #endregion

        #region 属性
        /// <summary>
        /// 使能软件位
        /// </summary>
        public bool Enable
        {
            get { return m_sen; }
            set { m_sen = value; }
        }

        /// <summary>
        /// 正限位位置
        /// </summary>
        public double SPelPosition
        {
            get { return m_spel; }
            set { m_spel = value; }
        }

        /// <summary>
        /// 负限位位置
        /// </summary>
        public double SMelPosition
        {
            get { return m_smel; }
            set { m_smel = value; }
        }
        #endregion

        #region 构造器
        public SoftLimitParams()
        {

        }
        public SoftLimitParams(bool enable, double pPosition, double mPosition)
        {
            this.m_sen = enable;
            this.m_spel = pPosition;
            this.m_smel = mPosition;
        }
        #endregion
    }
    #endregion

    #region 回零配置
    /// <summary>
    /// 回零配置参数
    /// </summary>
    public class HomeConfigParams
    {
        #region 字段
        private int m_mode;	      //回零模式 0：ORG,1：EL,2：EZ
        private int m_dir;	      //回零方向 0：Positive, 1:Negative
        private int m_ez;	      //寻找Z向信号 0：NO，1：YES
        private int m_maxvel;     //回零最大速度
        private int m_orgvel;     //找原点速度
        private int m_shift;      //回零偏移
        private double m_timeout; //回零超时时间
        #endregion

        #region 属性
        /// <summary>
        /// 回零模式 0：ORG,1：EL,2：EZ
        /// </summary>
        public int Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        /// <summary>
        /// 回零方向 0：Positive, 1:Negative
        /// </summary>
        public int Dir
        {
            get { return m_dir; }
            set { m_dir = value; }
        }

        /// <summary>
        /// EZ 0：NO，1：YES
        /// </summary>
        public int EZ
        {
            get { return m_ez; }
            set { m_ez = value; }
        }

        /// <summary>
        /// 最大速度
        /// </summary>
        public int MaxVel
        {
            get { return m_maxvel; }
            set { m_maxvel = value; }
        }

        /// <summary>
        /// 找原点速度
        /// </summary>
        public int OrgVel
        {
            get { return m_orgvel; }
            set { m_orgvel = value; }
        }

        /// <summary>
        /// 回零偏移
        /// </summary>
        public int Shift
        {
            get { return m_shift; }
            set { m_shift = value; }
        }

        /// <summary>
        /// 运动超时时间
        /// </summary>
        public double Timeout
        {
            get { return m_timeout; }
            set { m_timeout = value; }
        }
        #endregion

        #region 构造器
        public HomeConfigParams()
        {

        }

        public HomeConfigParams(int mode, int dir, int ez, int maxVel, int orgVel, int shift)
        {
            this.m_mode = mode;
            this.m_dir = dir;
            this.m_ez = ez;
            this.m_maxvel = maxVel;
            this.m_orgvel = orgVel;
            this.m_shift = shift;
        }
        #endregion
    }
    #endregion

    #region 机构抽象(名称，轴位置信息，轴传动信息，轴有效电平配置，轴速度配置，轴软限位配置，回零配置)
    /// <summary>
    /// 运动执行机构
    /// </summary>
    public class ExecuteBody
    {
        #region 字段
        private string m_partName;
        private AxisLocationInfo m_axisLocationInfo;
        private TransmissionParams m_transmissionParams;
        private AxisSignalParams m_axisSignalParams;
        private VelocityCurveParams m_velocityCurveParams;
        private SoftLimitParams m_softLimitParams;
        private HomeConfigParams m_homeConfigParams;
        #endregion

        #region 属性
        /// <summary>
        /// 机构名称
        /// </summary>
        public string PartName
        {
            get { return m_partName; }
            //set { axisName = value; }
        }

        /// <summary>
        /// 程序定位机构用的参数
        /// </summary>
        public AxisLocationInfo AxisLocationInfo
        {
            get { return m_axisLocationInfo; }
            set { m_axisLocationInfo = value; }
        }

        /// <summary>
        /// 机构传动参数
        /// </summary>
        public TransmissionParams TransmissionParams
        {
            get { return m_transmissionParams; }
            set { m_transmissionParams = value; }
        }

        /// <summary>
        /// 有效电平配置
        /// </summary>
        public AxisSignalParams AxisSignalParams
        {
            get { return this.m_axisSignalParams; }
            set { this.m_axisSignalParams = value; }
        }

        /// <summary>
        /// 轴速配置
        /// </summary>
        public VelocityCurveParams VelocityCurveParams
        {
            get { return this.m_velocityCurveParams; }
            set { this.m_velocityCurveParams = value; }
        }

        /// <summary>
        /// 软限位配置
        /// </summary>
        public SoftLimitParams SoftLimitParams
        {
            get { return this.m_softLimitParams; }
            set { this.m_softLimitParams = value; }
        }

        /// <summary>
        /// 回零配置
        /// </summary>
        public HomeConfigParams HomeConfigParams
        {
            get { return this.m_homeConfigParams; }
            set { this.m_homeConfigParams = value; }
        }
        #endregion

        #region 构造器
        public ExecuteBody(TransmissionParams transmissionParams, AxisLocationInfo axisLocationInfo)
            : this("未命名设备", transmissionParams, axisLocationInfo)
        {
        }

        public ExecuteBody(string partName, TransmissionParams transmissionParams, AxisLocationInfo axisLocationInfo)
        {
            this.m_partName = partName;
            this.m_transmissionParams = transmissionParams;
            this.m_axisLocationInfo = axisLocationInfo;

            this.m_velocityCurveParams = new VelocityCurveParams();      //轴速配置
            this.m_softLimitParams = new SoftLimitParams();              //软限位配置
            this.m_homeConfigParams = new HomeConfigParams();            //回零位置
        }

        public ExecuteBody(string partName)
        {
            this.m_partName = partName;
            this.m_transmissionParams = new TransmissionParams();        //传动信息
            this.m_velocityCurveParams = new VelocityCurveParams();      //轴速配置
            this.m_softLimitParams = new SoftLimitParams();              //软限位配置
            this.m_homeConfigParams = new HomeConfigParams();            //回零位置
        }

        public ExecuteBody()
            :this("未命名设备")
        {
        }
        #endregion
    }
    #endregion
    #endregion
}