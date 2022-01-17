/**
 *******************************************************************************
 * @file     htm_type_def.h
 * @author   Tongqing CHEN
 * @version  V4.06
 * @date     06/12/2018 (MM/DD/YYYY)<br>
 * @brief    A header file for ht motion type define.
 *******************************************************************************
 * <h2><center>&copy; Copyright 2018, Join Intelligent Equipement CO., LTD
 * <br><br>All rights reserved.</center></h2>
 */
#ifndef __HTM_TYPE_DEF_H__
#define __HTM_TYPE_DEF_H__

#ifdef __cplusplus
    extern "C" {
#endif
/* Include files -------------------------------------------------------------*/
typedef char           I8;       /*!<signed 8-bit integar*/
typedef unsigned char  U8;       /*!<unsigned 8-bit integar*/
typedef short          I16;      /*!<signed 16-bit integar*/
typedef unsigned short U16;      /*!<unsigned 16-bit integar*/
typedef long           I32;      /*!<signed 32-bit integar*/
typedef unsigned long  U32;      /*!<unsigned 32-bit integar*/
typedef float          F32;      /*!<Single precision float*/
typedef double         F64;      /*!<Double precision float*/

/* Exported macros -----------------------------------------------------------*/
#ifndef HTM_API
	#define HTM_API							/*!< HTM API标志 */
	#define HTM_LEGACY						/*!< HTM 兼容旧版(≤3.15)保留的接口 */
#endif
/* 轴驱动类型 */
#define  HTM_CARD_AMP208C				(0)	/*!< ADLINK 204/208C Dirve Card */
#define  HTM_CARD_HTBOND				(1)	/*!< HT Bond Dirve Card */
#define  HTM_CARD_HTDCMD				(2) /*!< HT DC Motor Dirve Card */
#define  HTM_CARD_HTDHVD				(3) /*!< HT DHVD Dirve Card */
#define  HTM_CARD_HTDLVD				(4)	/*!< HT DLVD Dirve Card */
#define	 HTM_CARD_HT202C				(5) /*!< HT 202C Motion Card */
#define	 HTM_CARD_HTDHVD_PDO			(6) /*!< HT DHVD pdo-type Dirve Card */
#define	 HTM_CARD_HB_DISPENSE			(7) /*!< HT DHVD pdo-type Dirve Card */
#define  HTM_CARD_OTHER					(255)	/*!< Other Dirve Card */ 

/* 轴类型 */
#define  HTM_AXIS_SERVO					(0) /*!< Servo Motor伺服 */
#define  HTM_AXIS_STEP					(1) /*!< Step Motor步进 */
#define  HTM_AXIS_LINE					(2) /*!< Line Motor直线 */ 
#define  HTM_AXIS_VOICE					(3) /*!< Voice Motor音圈 */
#define  HTM_AXIS_TORQUE				(4) /*!< Torque Motor力矩 */
#define  HTM_AXIS_DC					(5) /*!< DC Motor直流 */

/* 运动标识参数 */
#define  HTM_ABS_MOVE					(1) /*!< 绝对运动模式 */
#define  HTM_S_MOVE						(2) /*!< S型曲线模式 */
#define  HTM_TQ_MOVE					(4) /*!< 力矩运动模式 */
#define  HTM_BUF_MOVE					(8) /*!< 缓存式运动 */
	#define  HTM_AS_MOVE					(3) /*!< 绝对S型曲线运动模式 */
	#define  HTM_RS_MOVE					(2) /*!< 相对S型曲线运动模式 */
	#define  HTM_AT_MOVE					(1) /*!< 绝对R型曲线运动模式 */
	#define  HTM_RT_MOVE					(0) /*!< 相对R型曲线运动模式 */
/* stop mode */
#define HTM_STOP_EMG					(0) /*!< 立即停 */
#define HTM_STOP_DEC					(1) /*!< 减速停 */
#define HTM_STOP_PEL					(2) /*!< 正限位信号停 */
#define HTM_STOP_MEL					(3) /*!< 负限位信号停 */
#define HTM_STOP_IO0					(4) /*!< IO0信号停 */
#define HTM_STOP_IO1					(5) /*!< IO1信号停 */
#define HTM_STOP_IO2					(6) /*!< IO2信号停 */
#define HTM_STOP_IO3					(7) /*!< IO3信号停 */

/* motion io status bit number */
#define HTM_MIO_ALM						(0)	/*!< 报警IO位  */
#define HTM_MIO_PEL						(1)	/*!< 正限IO位  */
#define HTM_MIO_MEL						(2)	/*!< 负限IO位  */
#define HTM_MIO_ORG						(3)	/*!< 原点IO位  */
#define HTM_MIO_IDX						(4)	/*!< Z相信号IO位  */
#define HTM_MIO_INP						(6)	/*!< 到位IO位  */
#define HTM_MIO_SVON					(7)	/*!< 励磁IO位  */
#define HTM_MIO_SPEL					(11)/*!< 软正限IO位 */
#define HTM_MIO_SMEL					(12)/*!< 软负限IO位 */
#define HTM_MIO_IO0						(13)/*!< 直流电机IO0 */
#define HTM_MIO_IO1						(14)/*!< 直流电机IO1*/
#define HTM_MIO_IO2						(15)/*!< 直流电机IO2*/
#define HTM_MIO_IO3						(16)/*!< 直流电机IO3*/

/* IO板卡类型 */
#define  HTM_DIO_HTNET						(0)	   /*!< HTNET-DIO */
#define	 HTM_AIO_HTNET						(1)	   /*!< HTNET-AIO */

/* 设备类型 */
#define  HTM_DEV_POSTRIG				(0) /*!< HT Pos Trigger Card */
#define  HTM_DEV_HTDHVD					(1) /*!< HT DHVD Dirve Card */
#define  HTM_DEV_LTSRC					(2)	/*!< HT Light Source Drive */
#define  HTM_DEV_COM					(3)	/*!< Serial Com */ 		
#define  HTM_DEV_OTHER					(255)	/*!< Other device */ 

#define  HTM_TRIG_SRC0					(1)/*!< 光源触发源0 */
#define  HTM_TRIG_SRC1					(2)/*!< 光源触发源1 */
#define  HTM_TRIG_SRC2					(4)/*!< 光源触发源2 */
#define  HTM_TRIG_SRC3					(8)/*!< 光源触发源3 */

/* Exported typedefs ---------------------------------------------------------*/

/* init paras. */
typedef struct
{
	char para_file[256];	   /*!< 配置参数全路径(htm_para.db文件全路径) */
	U8   use_aps_card;		   /*!< 是否使用凌华板卡(1-use, 0-not use) */
	U8	 use_htnet_card;	   /*!< 是否使用HTNET板卡(1-use, 0-not use) */
	U8	 offline_mode;		   /*!< 是否为脱机模式(1-offline不初始化板卡, 0-online) */
	U16  max_axis_num;		   /*!< 轴数(≥0) *///
	U16  max_io_num;		   /*!< IO数(≥0) */ //
	U16  max_dev_num;		   /*!< 其它设备数(≥0) */
	U16  max_module_num;	   /*!< 模块数(≥0) */
	U8	 language;			   /*!< 语言(UI, error等)0-CHN, 1-ENG */
} HTM_INIT_PARA;

/* axis moinitor(Simulation) parameters. */
typedef struct
{
	U8 svon;		/*!< axis servo on flag. */
	F64 fbk;		/*!< axis feedback postion. */
	F64 cmd;		/*!< axis command position. */
	F64 inpTime; /*!< axis in postion (achieve to target) time. */
}AxisMonitor, *pAxisMonitor;

/* axis velocity infos. */
typedef struct
{
	F64    vStart;		/*!< 起始速度(mm/s) */
	F64    vMax;		/*!< 最大速度(mm/s) */
	F64    vEnd;		/*!< 结束速度(mm/s) */
	F64    acc; 		/*!< 加速度(mm2/s) */
	F64    dec;			/*!< 减速度(mm2/s) */
	F64    sFactor;		/*!< s曲线因子(0~1.0)*/
	F64    timeout;  	/*!< 超时时间(s)*/
}HTM_MovePara;

/* axis config */
typedef struct
{
	char	axisName[32]; 	/*!< Axis Name */
	U8		driveType;		/*!< Drive Card Type  */
	U8		axisType;		/*!< Axis type 	*/
	U16		busNo;			/*!< Card/Bus Number 卡号/总线号  */
	U16		nodeAddr;		/*!< Axis/Node Address 轴号/节点号 */
	U16		portNo;			/*!< Port Number 端口号 */
	F64		pulseFactor;	/*!< 脉冲当量(pulse/mm) */
	U8		extEncode;		/*!< 是否有编码器反馈 */
	HTM_MovePara   mp;  	/*!< 运动参数 */

	U8		enableMEL:1;	/*!< 负电子限位使能*/
	U8		enablePEL:3;	/*!< 正电子限位使能*/
	 
	U8		enableSMEL:1;	/*!< 负软限位使能*/
	U8		enableSPEL:3;  	/*!< 正软子限位使能*/
	F64		sPELPos; 		/*!< 正软限位位置(mm/°) */
	F64		sMELPos; 		/*!< 负软限位位置(mm/°) */

	I8		homeMode;		/*!< 回零模式 */
	U8		homeDir;		/*!< 回零方向 */
	U8		homeEZA;		/*!< EZ信号使能 */
//	F64		homeSFactor; 	/*!< 回零S因子(0~1.0) not use after version 3.10*/
	U32		hdStpCrt;		/*!< DHVD硬限位回零电流限幅设置*/
	U32		hdStpTime;		/*!< DHVD硬限位回零时间设置*/
	F64		homeAcc;		/*!< 回零加速度(mm2/s) */
	F64		homeVs;			/*!< 回零起始速度(mm/s) */
	F64		homeVm;			/*!< 回零最大速度(mm/s) */
	F64		homeVo;			/*!< 回零原点速度(mm/s) */
	F64		homeShift;   	/*!< 回零偏移量(mm) */
	F64		homeTimeout; 	/*!< 回零超时时间(s) */

	/* 以下208C专用 */
	U8		almLogic;		/*!< 报警信号逻辑 */
	U8		elLogic;		/*!< 限位信号逻辑 */
	U8		orgLogic;		/*!< 原点信号逻辑 */
	U8		ezLogic;		/*!< EZ信号逻辑 */
	U8		inpLogic;     	/*!< 到位信号逻辑 */
	U8		servoLogic;   	/*!< 励磁信号逻辑 */
	U8		optMode;      	/*!< 输出信号逻辑 */
	U8		iptMode; 		/*!< 输入信号逻辑 */
	U8		encodeDir;    	/*!< 编码器方向 */

	/* 以下htnet-DC电机专用 */
	F64		maxCurrent;		/*!< 最大电流(A) */
	U8		polarity;		/*!< 位置极性 (0/1) */
	F64		pError;			/*!< 位置误差(mm) */
	U16		peTimeout;		/*!< 位置误差超时(ms) */
	F64		inpValue;   	/*!< 到位窗口(mm) */
	U16		inpInterval;	/*!< 到位窗口时间(ms) */
	F64		stopDec;		/*!< 快速停止速度(ms) */
} HTM_AxisInfo;

/* axis status */
typedef struct
{
	U8		alm;			/*!< 报警信号(0/1) */
	U8		pel;			/*!< 正限位信号(0/1) */
	U8		mel;			/*!< 负限位信号(0/1) */
	U8		org;			/*!< 原点信号(0/1) */
	U8		idx;			/*!< index信号(0/1) */
	U8		inp;			/*!< INP信号(0/1) */
	U8		svon;			/*!< 励磁信号(0/1) */
	F64		cmdPos;			/*!< 指令位置(mm) */
	F64		fbkPos;			/*!< 反馈信号(mm) */
}HTM_AxisStatus;

//io
typedef struct
{
	char	ioName[32]; 	/*!< IO名字 */
	U16		busNo;			/*!< 总线号 */
	U16		nodeAddr;		/*!< 节点号 */
	U16		portNo;			/*!< 端口号 */
	U8		ioDir;			/*!< IO方向(0-输入,1-输出) */
	U8		polarity;		/*!< 极性(0-默认,1-取反) */
	U8		disable;    	/*!< 是否禁用(0-不禁用,1-禁用)，禁用后将不能正常操作该IO设备*/
	U8		cardType;		/*!< 板卡类型*/
} HTM_IoInfo;

//device
/* 触发位置信息*/
typedef struct
{
	F64 pos; 		/*!< Position */
	U8 dir; 		/*!< Direction */
	U8 enable;		/*!< 使能 */
}TRIG_PT;	 		/*!< 点表触发模式配置 */
typedef struct
{
	F64	startPos;	/*!< 线性触发起始位置(mm) */
	F64	endPos;		/*!< 线性触发结束位置(mm) */
	F64	interval;	/*!< 线性触发间隔(mm) */
}TRIG_LIN;		//线性触发模式配置

/* 位置触发板information*/
typedef struct
{
	U8	axisIdx;		/*!< 对应轴 */
	U8	trigMode;		/*!< 触发模式 0点表1线性 */
	U8	polarity;		/*!< 取反 */
	TRIG_PT trigPt[4];   /*!< 点表模式触发位置(mm) */

	U8 lineEnable;		/*!< 线性触发使能 */
	TRIG_LIN trigLin;	/*!<线性触发配置*/

	F64	ffTime;			/*!< 相机触发超前时间(us) */
}POS_TIRG;
/* 光源驱动板信息*/
typedef struct
{
	U8	tirgSrc[4];		/*!<触发源*/
	F64 opTime;			/*!<脉冲宽度时间us*/
}LIGHT_DRIVE;

/* 串口信息*/
typedef struct
{
	I32 comNo;			/*!<串口号*/
	I32 baudRate;		/*!<波特率*/
	I32 parity;			/*!<校验位*/
	I32 dataBit;		/*!<数据位*/
	I32 stopBit;		/*!<停止位*/
}COM_DEV;

typedef union
{
	POS_TIRG posTrig;  /*!<位置触发板信息*/
	LIGHT_DRIVE light; /*!<光源驱动板信息*/
	COM_DEV	com;	   /*!<串口信息*/
}DEV_LIST;

/* device infomation */
typedef struct
{
	char devName[32];   /*!<设备名称*/
	U16	busNo;			/*!< 总线号 */
	U16	nodeAddr;		/*!< 节点号 */
	U16	portNo;			/*!< 端口号 */
	U8	devType;		/*!<设备类型*/
	DEV_LIST devPara;	/*!< 设备信息*/
}HTM_DeviceInfo;
#ifdef __cplusplus
    }
#endif

#endif  /* ndef __HTM_TYPE_DEF_H__ */
/* End of file ---------------------------------------------------------------*/
