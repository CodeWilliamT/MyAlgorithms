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
	#define HTM_API							/*!< HTM API��־ */
	#define HTM_LEGACY						/*!< HTM ���ݾɰ�(��3.15)�����Ľӿ� */
#endif
/* ���������� */
#define  HTM_CARD_AMP208C				(0)	/*!< ADLINK 204/208C Dirve Card */
#define  HTM_CARD_HTBOND				(1)	/*!< HT Bond Dirve Card */
#define  HTM_CARD_HTDCMD				(2) /*!< HT DC Motor Dirve Card */
#define  HTM_CARD_HTDHVD				(3) /*!< HT DHVD Dirve Card */
#define  HTM_CARD_HTDLVD				(4)	/*!< HT DLVD Dirve Card */
#define	 HTM_CARD_HT202C				(5) /*!< HT 202C Motion Card */
#define	 HTM_CARD_HTDHVD_PDO			(6) /*!< HT DHVD pdo-type Dirve Card */
#define	 HTM_CARD_HB_DISPENSE			(7) /*!< HT DHVD pdo-type Dirve Card */
#define  HTM_CARD_OTHER					(255)	/*!< Other Dirve Card */ 

/* ������ */
#define  HTM_AXIS_SERVO					(0) /*!< Servo Motor�ŷ� */
#define  HTM_AXIS_STEP					(1) /*!< Step Motor���� */
#define  HTM_AXIS_LINE					(2) /*!< Line Motorֱ�� */ 
#define  HTM_AXIS_VOICE					(3) /*!< Voice Motor��Ȧ */
#define  HTM_AXIS_TORQUE				(4) /*!< Torque Motor���� */
#define  HTM_AXIS_DC					(5) /*!< DC Motorֱ�� */

/* �˶���ʶ���� */
#define  HTM_ABS_MOVE					(1) /*!< �����˶�ģʽ */
#define  HTM_S_MOVE						(2) /*!< S������ģʽ */
#define  HTM_TQ_MOVE					(4) /*!< �����˶�ģʽ */
#define  HTM_BUF_MOVE					(8) /*!< ����ʽ�˶� */
	#define  HTM_AS_MOVE					(3) /*!< ����S�������˶�ģʽ */
	#define  HTM_RS_MOVE					(2) /*!< ���S�������˶�ģʽ */
	#define  HTM_AT_MOVE					(1) /*!< ����R�������˶�ģʽ */
	#define  HTM_RT_MOVE					(0) /*!< ���R�������˶�ģʽ */
/* stop mode */
#define HTM_STOP_EMG					(0) /*!< ����ͣ */
#define HTM_STOP_DEC					(1) /*!< ����ͣ */
#define HTM_STOP_PEL					(2) /*!< ����λ�ź�ͣ */
#define HTM_STOP_MEL					(3) /*!< ����λ�ź�ͣ */
#define HTM_STOP_IO0					(4) /*!< IO0�ź�ͣ */
#define HTM_STOP_IO1					(5) /*!< IO1�ź�ͣ */
#define HTM_STOP_IO2					(6) /*!< IO2�ź�ͣ */
#define HTM_STOP_IO3					(7) /*!< IO3�ź�ͣ */

/* motion io status bit number */
#define HTM_MIO_ALM						(0)	/*!< ����IOλ  */
#define HTM_MIO_PEL						(1)	/*!< ����IOλ  */
#define HTM_MIO_MEL						(2)	/*!< ����IOλ  */
#define HTM_MIO_ORG						(3)	/*!< ԭ��IOλ  */
#define HTM_MIO_IDX						(4)	/*!< Z���ź�IOλ  */
#define HTM_MIO_INP						(6)	/*!< ��λIOλ  */
#define HTM_MIO_SVON					(7)	/*!< ����IOλ  */
#define HTM_MIO_SPEL					(11)/*!< ������IOλ */
#define HTM_MIO_SMEL					(12)/*!< ����IOλ */
#define HTM_MIO_IO0						(13)/*!< ֱ�����IO0 */
#define HTM_MIO_IO1						(14)/*!< ֱ�����IO1*/
#define HTM_MIO_IO2						(15)/*!< ֱ�����IO2*/
#define HTM_MIO_IO3						(16)/*!< ֱ�����IO3*/

/* IO�忨���� */
#define  HTM_DIO_HTNET						(0)	   /*!< HTNET-DIO */
#define	 HTM_AIO_HTNET						(1)	   /*!< HTNET-AIO */

/* �豸���� */
#define  HTM_DEV_POSTRIG				(0) /*!< HT Pos Trigger Card */
#define  HTM_DEV_HTDHVD					(1) /*!< HT DHVD Dirve Card */
#define  HTM_DEV_LTSRC					(2)	/*!< HT Light Source Drive */
#define  HTM_DEV_COM					(3)	/*!< Serial Com */ 		
#define  HTM_DEV_OTHER					(255)	/*!< Other device */ 

#define  HTM_TRIG_SRC0					(1)/*!< ��Դ����Դ0 */
#define  HTM_TRIG_SRC1					(2)/*!< ��Դ����Դ1 */
#define  HTM_TRIG_SRC2					(4)/*!< ��Դ����Դ2 */
#define  HTM_TRIG_SRC3					(8)/*!< ��Դ����Դ3 */

/* Exported typedefs ---------------------------------------------------------*/

/* init paras. */
typedef struct
{
	char para_file[256];	   /*!< ���ò���ȫ·��(htm_para.db�ļ�ȫ·��) */
	U8   use_aps_card;		   /*!< �Ƿ�ʹ���軪�忨(1-use, 0-not use) */
	U8	 use_htnet_card;	   /*!< �Ƿ�ʹ��HTNET�忨(1-use, 0-not use) */
	U8	 offline_mode;		   /*!< �Ƿ�Ϊ�ѻ�ģʽ(1-offline����ʼ���忨, 0-online) */
	U16  max_axis_num;		   /*!< ����(��0) *///
	U16  max_io_num;		   /*!< IO��(��0) */ //
	U16  max_dev_num;		   /*!< �����豸��(��0) */
	U16  max_module_num;	   /*!< ģ����(��0) */
	U8	 language;			   /*!< ����(UI, error��)0-CHN, 1-ENG */
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
	F64    vStart;		/*!< ��ʼ�ٶ�(mm/s) */
	F64    vMax;		/*!< ����ٶ�(mm/s) */
	F64    vEnd;		/*!< �����ٶ�(mm/s) */
	F64    acc; 		/*!< ���ٶ�(mm2/s) */
	F64    dec;			/*!< ���ٶ�(mm2/s) */
	F64    sFactor;		/*!< s��������(0~1.0)*/
	F64    timeout;  	/*!< ��ʱʱ��(s)*/
}HTM_MovePara;

/* axis config */
typedef struct
{
	char	axisName[32]; 	/*!< Axis Name */
	U8		driveType;		/*!< Drive Card Type  */
	U8		axisType;		/*!< Axis type 	*/
	U16		busNo;			/*!< Card/Bus Number ����/���ߺ�  */
	U16		nodeAddr;		/*!< Axis/Node Address ���/�ڵ�� */
	U16		portNo;			/*!< Port Number �˿ں� */
	F64		pulseFactor;	/*!< ���嵱��(pulse/mm) */
	U8		extEncode;		/*!< �Ƿ��б��������� */
	HTM_MovePara   mp;  	/*!< �˶����� */

	U8		enableMEL:1;	/*!< ��������λʹ��*/
	U8		enablePEL:3;	/*!< ��������λʹ��*/
	 
	U8		enableSMEL:1;	/*!< ������λʹ��*/
	U8		enableSPEL:3;  	/*!< ��������λʹ��*/
	F64		sPELPos; 		/*!< ������λλ��(mm/��) */
	F64		sMELPos; 		/*!< ������λλ��(mm/��) */

	I8		homeMode;		/*!< ����ģʽ */
	U8		homeDir;		/*!< ���㷽�� */
	U8		homeEZA;		/*!< EZ�ź�ʹ�� */
//	F64		homeSFactor; 	/*!< ����S����(0~1.0) not use after version 3.10*/
	U32		hdStpCrt;		/*!< DHVDӲ��λ��������޷�����*/
	U32		hdStpTime;		/*!< DHVDӲ��λ����ʱ������*/
	F64		homeAcc;		/*!< ������ٶ�(mm2/s) */
	F64		homeVs;			/*!< ������ʼ�ٶ�(mm/s) */
	F64		homeVm;			/*!< ��������ٶ�(mm/s) */
	F64		homeVo;			/*!< ����ԭ���ٶ�(mm/s) */
	F64		homeShift;   	/*!< ����ƫ����(mm) */
	F64		homeTimeout; 	/*!< ���㳬ʱʱ��(s) */

	/* ����208Cר�� */
	U8		almLogic;		/*!< �����ź��߼� */
	U8		elLogic;		/*!< ��λ�ź��߼� */
	U8		orgLogic;		/*!< ԭ���ź��߼� */
	U8		ezLogic;		/*!< EZ�ź��߼� */
	U8		inpLogic;     	/*!< ��λ�ź��߼� */
	U8		servoLogic;   	/*!< �����ź��߼� */
	U8		optMode;      	/*!< ����ź��߼� */
	U8		iptMode; 		/*!< �����ź��߼� */
	U8		encodeDir;    	/*!< ���������� */

	/* ����htnet-DC���ר�� */
	F64		maxCurrent;		/*!< ������(A) */
	U8		polarity;		/*!< λ�ü��� (0/1) */
	F64		pError;			/*!< λ�����(mm) */
	U16		peTimeout;		/*!< λ����ʱ(ms) */
	F64		inpValue;   	/*!< ��λ����(mm) */
	U16		inpInterval;	/*!< ��λ����ʱ��(ms) */
	F64		stopDec;		/*!< ����ֹͣ�ٶ�(ms) */
} HTM_AxisInfo;

/* axis status */
typedef struct
{
	U8		alm;			/*!< �����ź�(0/1) */
	U8		pel;			/*!< ����λ�ź�(0/1) */
	U8		mel;			/*!< ����λ�ź�(0/1) */
	U8		org;			/*!< ԭ���ź�(0/1) */
	U8		idx;			/*!< index�ź�(0/1) */
	U8		inp;			/*!< INP�ź�(0/1) */
	U8		svon;			/*!< �����ź�(0/1) */
	F64		cmdPos;			/*!< ָ��λ��(mm) */
	F64		fbkPos;			/*!< �����ź�(mm) */
}HTM_AxisStatus;

//io
typedef struct
{
	char	ioName[32]; 	/*!< IO���� */
	U16		busNo;			/*!< ���ߺ� */
	U16		nodeAddr;		/*!< �ڵ�� */
	U16		portNo;			/*!< �˿ں� */
	U8		ioDir;			/*!< IO����(0-����,1-���) */
	U8		polarity;		/*!< ����(0-Ĭ��,1-ȡ��) */
	U8		disable;    	/*!< �Ƿ����(0-������,1-����)�����ú󽫲�������������IO�豸*/
	U8		cardType;		/*!< �忨����*/
} HTM_IoInfo;

//device
/* ����λ����Ϣ*/
typedef struct
{
	F64 pos; 		/*!< Position */
	U8 dir; 		/*!< Direction */
	U8 enable;		/*!< ʹ�� */
}TRIG_PT;	 		/*!< �����ģʽ���� */
typedef struct
{
	F64	startPos;	/*!< ���Դ�����ʼλ��(mm) */
	F64	endPos;		/*!< ���Դ�������λ��(mm) */
	F64	interval;	/*!< ���Դ������(mm) */
}TRIG_LIN;		//���Դ���ģʽ����

/* λ�ô�����information*/
typedef struct
{
	U8	axisIdx;		/*!< ��Ӧ�� */
	U8	trigMode;		/*!< ����ģʽ 0���1���� */
	U8	polarity;		/*!< ȡ�� */
	TRIG_PT trigPt[4];   /*!< ���ģʽ����λ��(mm) */

	U8 lineEnable;		/*!< ���Դ���ʹ�� */
	TRIG_LIN trigLin;	/*!<���Դ�������*/

	F64	ffTime;			/*!< ���������ǰʱ��(us) */
}POS_TIRG;
/* ��Դ��������Ϣ*/
typedef struct
{
	U8	tirgSrc[4];		/*!<����Դ*/
	F64 opTime;			/*!<������ʱ��us*/
}LIGHT_DRIVE;

/* ������Ϣ*/
typedef struct
{
	I32 comNo;			/*!<���ں�*/
	I32 baudRate;		/*!<������*/
	I32 parity;			/*!<У��λ*/
	I32 dataBit;		/*!<����λ*/
	I32 stopBit;		/*!<ֹͣλ*/
}COM_DEV;

typedef union
{
	POS_TIRG posTrig;  /*!<λ�ô�������Ϣ*/
	LIGHT_DRIVE light; /*!<��Դ��������Ϣ*/
	COM_DEV	com;	   /*!<������Ϣ*/
}DEV_LIST;

/* device infomation */
typedef struct
{
	char devName[32];   /*!<�豸����*/
	U16	busNo;			/*!< ���ߺ� */
	U16	nodeAddr;		/*!< �ڵ�� */
	U16	portNo;			/*!< �˿ں� */
	U8	devType;		/*!<�豸����*/
	DEV_LIST devPara;	/*!< �豸��Ϣ*/
}HTM_DeviceInfo;
#ifdef __cplusplus
    }
#endif

#endif  /* ndef __HTM_TYPE_DEF_H__ */
/* End of file ---------------------------------------------------------------*/
