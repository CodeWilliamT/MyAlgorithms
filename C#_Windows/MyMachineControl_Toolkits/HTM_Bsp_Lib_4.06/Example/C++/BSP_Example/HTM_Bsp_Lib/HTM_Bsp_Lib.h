/**
 *******************************************************************************
 * @file     HTM_Bsp_Lib.h
 * @author   Tongqing CHEN
 * @version  V4.06
 * @date     06/12/2018 (MM/DD/YYYY)<br>
 * @brief    A header file for all of htm bsp APIs(x86/x64).
 *******************************************************************************
 * @copy
 *
 * <h2><center>&copy; Copyright 2018, Join Intelligent Equipement CO., LTD
 * <br><br>All rights reserved.</center></h2>
 */
#ifndef __HTM_BSP_LIB_H__
#define __HTM_BSP_LIB_H__

#ifdef __cplusplus
    extern "C" {
#endif

/* Include files -------------------------------------------------------------*/		
#include "htm_type_def.h"

/* Exported function prototypes-----------------------------------------------*/
		
/* HTM BSP(HT Motion Board support package) configuration functions */
HTM_API I32 HTM_Bsp_GetVersionNo(void);	
HTM_API char* HTM_Bsp_GetVersionInfo(void);
HTM_API I32 HTM_Bsp_ShowVersion(void);
HTM_API I32 HTM_Bsp_Init(HTM_INIT_PARA *init_para);
HTM_API I32 HTM_Bsp_InitFromFile(char xmlFile[]);
HTM_API I32 HTM_Bsp_Discard(void);
HTM_API I32 HTM_Bsp_LoadInitParaFromFile(char xmlFile[], HTM_INIT_PARA *init_para);
HTM_API I32 HTM_Bsp_SaveInitParaToFile(HTM_INIT_PARA *init_para, char xmlFile[]);
HTM_API I32 HTM_Bsp_GetInitPara(HTM_INIT_PARA *init_para);
HTM_API I32 HTM_Bsp_GetMode(void);
HTM_API char *HTM_Bsp_GetConfigFilePath(void);
HTM_API I32 HTM_Bsp_GetLastErrCode(void);
HTM_API char *HTM_Bsp_GetLastErrStr(void);
HTM_API I32 HTM_Bsp_LoadUI(void);
HTM_API I32 HTM_Bsp_DiscardUI(void);
HTM_API I32 HTM_Bsp_LoadToolUI(void);
HTM_API I32 HTM_Bsp_DiscardToolUI(void); 
	/* Functions below only available in CVIRTE(CVI Run-time environment) */
	HTM_API I32 HTM_Bsp_LoadUI_CVI(int panel, int frameCtrl);
	HTM_API I32 HTM_Bsp_DisplayUI_CVI(void);
	HTM_API I32 HTM_Bsp_HideUI_CVI(void); 
	HTM_API I32 HTM_Bsp_DiscardUI_CVI(void);

/* HTM_EventLog APIs*/
HTM_API I32 HTM_EventLog_LoadUI(void); 
HTM_API I32 HTM_EventLog_DiscardUI(void);
HTM_API I32 HTM_EventLog_Start(int task_id, char event_name[], char *info_fmt, ...); 
HTM_API I32 HTM_EventLog_End(int task_id); 
	/* Functions below only available in CVIRTE(CVI Run-time environment) */
	HTM_API I32 HTM_EventLog_LoadUI_CVI(int panel, int frameCtrl);
	HTM_API I32 HTM_EventLog_DisplayUI_CVI(void);
	HTM_API I32 HTM_EventLog_HideUI_CVI(void); 
	HTM_API I32 HTM_EventLog_DiscardUI_CVI(void);

/* Moudle APIs*/	
HTM_API I32 HTM_GetModuleInitState(U8 mdl_idx);
HTM_API I32 HTM_SetModuleInitState(U8 mdl_idx, U8 inited);
HTM_API I32 HTM_GetModuleInitCmd(U8 mdl_idx);
HTM_API I32 HTM_SetModuleInitCmd(U8 mdl_idx, U8 cmd);
	
/* Axis operation APIs */
HTM_API I32 HTM_LoadAxisInfo(I32 axisIdx);
HTM_API I32 HTM_SaveAxisInfo(I32 axisIdx);
HTM_API I32 HTM_GetAxisInfo(I32 axisIdx, HTM_AxisInfo *axisInfo);
HTM_API I32 HTM_SetAxisInfo(I32 axisIdx, const HTM_AxisInfo *axisInfo);
HTM_API I32 HTM_GetAxisNum(void);
HTM_API I32 HTM_CheckAxisIdx(I32 axisIdx);

/*------------ get(set) axis parameter functions---------------------------*/
HTM_API HTM_AxisStatus* HTM_GetAxisStatus(I32 axisIdx);
HTM_API I32 HTM_ConfigAxisInfo(I32 axisIdx);
HTM_API char*  HTM_GetAxisName(I32 axisIdx);
HTM_API F64 HTM_GetMaxVel(I32 axisIdx);
HTM_API F64 HTM_GetTimeOut(I32 axisIdx);
HTM_API F64 HTM_GetPulseFactor(I32 axisIdx);
HTM_API HTM_MovePara *HTM_GetMovePara(I32 axisIdx);
/*--------------get(set) motion position(velocity) command(feedback)-------*/
HTM_API I32 HTM_SetCurHomePos(I32 axisIdx);
HTM_API I32 HTM_SetCmdPos(I32 axisIdx, F64 cmdpos_in_unit);
HTM_API I32 HTM_SetFbkPos(I32 axisIdx, F64 fbkpos_in_unit);
HTM_API I32 HTM_ReadCmdPos(I32 axisIdx, F64 *pos);
HTM_API I32 HTM_ReadFbkPos(I32 axisIdx, F64 *pos);
HTM_API I32 HTM_ReadCmdVel(I32 axisIdx, F64 *vel);
HTM_API I32 HTM_ReadFbkVel(I32 axisIdx, F64 *vel);
	/* 兼容版本<=4.05的接口，建议使用新版接口*/
	HTM_LEGACY F64 HTM_GetCmdPos(I32 axisIdx);
	HTM_LEGACY F64 HTM_GetFbkPos(I32 axisIdx);
	HTM_LEGACY F64 HTM_GetCmdVel(I32 axisIdx);
	HTM_LEGACY F64 HTM_GetFbkVel(I32 axisIdx);
	
/*--------------------get motion IO status---------------------------------*/
HTM_API I32 HTM_GetMotionIOBit(I32 axisIdx, I32 bit);
HTM_API I32 HTM_GetINP(I32 axisIdx);
HTM_API I32 HTM_GetPEL(I32 axisIdx);
HTM_API I32 HTM_GetMEL(I32 axisIdx);
HTM_API I32 HTM_GetORG(I32 axisIdx);
HTM_API I32 HTM_GetAlarm(I32 axisIdx);
HTM_API I32 HTM_GetSVON(I32 axisIdx);

HTM_API I32 HTM_SetSVON (I32 axisIdx, I32 on_off);
HTM_API I32 HTM_SetSVON2 (I32 axis1, I32 axis2, I32 on_off);
HTM_API I32 HTM_SetSVON3 (I32 axis1, I32 axis2, I32 axis3, I32 on_off);
HTM_API I32 HTM_SetSVON4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, I32 on_off);
HTM_API I32 HTM_SetSVONN (I32 axisList[], I32 axisNum, I32 on_off);
HTM_API I32 HTM_SetSVONAll(I32 on_off);

HTM_API I32 HTM_SVDone(I32 axisIdx);
HTM_API I32 HTM_SVDone2 (I32 axis1, I32 axis2);
HTM_API I32 HTM_SVDone3 (I32 axis1, I32 axis2, I32 axis3);
HTM_API I32 HTM_SVDone4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4);
HTM_API I32 HTM_SVDoneN (I32 axisList[], I32 axisNum);
HTM_API I32 HTM_SVDoneAll (void);

HTM_API I32 HTM_SetSVOver(I32 axisIdx);
HTM_API I32 HTM_SetSVOver2 (I32 axis1, I32 axis2);
HTM_API I32 HTM_SetSVOver3 (I32 axis1, I32 axis2, I32 axis3);
HTM_API I32 HTM_SetSVOver4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4);
HTM_API I32 HTM_SetSVOverN (I32 axisList[], I32 axisNum);
HTM_API I32 HTM_SetSVOverAll (void); 

HTM_API I32 HTM_EnableEL (I32 axisIdx, I32 pel_enable, I32 mel_enable);
HTM_API I32 HTM_EnableSEL (I32 axisIdx, I32 spel_enable, I32 smel_enable);
/*------------------------ axis motion functions -----------------------------*/
HTM_API I32 HTM_Home (I32 axisIdx);
HTM_API I32 HTM_Home2(I32 axis1, I32 axis2);
HTM_API I32 HTM_Home3(I32 axis1, I32 axis2, I32 axis3);
HTM_API I32 HTM_Home4(I32 axis1, I32 axis2, I32 axis3, I32 axis4);
HTM_API I32 HTM_HomeN(I32 axisList[], I32 axisNum);

HTM_API I32 HTM_HomeOver (I32 axisIdx);
HTM_API I32 HTM_HomeOver2(I32 axis1, I32 axis2);
HTM_API I32 HTM_HomeOver3(I32 axis1, I32 axis2, I32 axis3);
HTM_API I32 HTM_HomeOver4(I32 axis1, I32 axis2, I32 axis3, I32 axis4);
HTM_API I32 HTM_HomeOverN(I32 axisList[], I32 axisNum);

HTM_API I32 HTM_CalcMotionTimeEx(HTM_MovePara *mp, F64 distance, F64* motion_time);
HTM_API I32 HTM_CalcMotionTime(I32 axisIdx, F64 distance, F64* motion_time);

HTM_API I32 HTM_MovePreload(I32 axisIdx);
HTM_API I32 HTM_MoveTrigger(I32 axisIdx);
HTM_API I32 HTM_MoveEx(I32 axisIdx, F64 pos_in_unit, F64 vel_ratio, I32 mode, const HTM_MovePara *mp);
HTM_API I32 HTM_Move(I32 axisIdx, F64 pos_in_unit, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_Move2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_Move3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_Move4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_MoveN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio, I32 mode);

HTM_API I32 HTM_ASMove (I32 axisIdx, F64 pos_in_unit, F64 vel_ratio);
HTM_API I32 HTM_ASMove2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_ASMove3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_ASMove4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_ASMoveN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_ATMove (I32 axisIdx, F64 pos_in_unit, F64 vel_ratio);
HTM_API I32 HTM_ATMove2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_ATMove3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_ATMove4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_ATMoveN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_RSMove (I32 axisIdx, F64 pos_in_unit, F64 vel_ratio);
HTM_API I32 HTM_RSMove2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_RSMove3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_RSMove4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_RSMoveN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_RTMove (I32 axisIdx, F64 pos_in_unit, F64 vel_ratio);
HTM_API I32 HTM_RTMove2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_RTMove3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_RTMove4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_RTMoveN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_MoveOver(I32 axisIdx, F64 pos_in_unit, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_MoveOver2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_MoveOver3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_MoveOver4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_MoveOverN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio, I32 mode);

HTM_API I32 HTM_ASMoveOver (I32 axis, F64 pos, F64 vel_ratio);
HTM_API I32 HTM_ASMoveOver2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_ASMoveOver3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_ASMoveOver4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_ASMoveOverN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_ATMoveOver (I32 axis, F64 pos, F64 vel_ratio);
HTM_API I32 HTM_ATMoveOver2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_ATMoveOver3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio) ;
HTM_API I32 HTM_ATMoveOver4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_ATMoveOverN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_RSMoveOver (I32 axis, F64 pos, F64 vel_ratio);
HTM_API I32 HTM_RSMoveOver2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_RSMoveOver3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_RSMoveOver4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_RSMoveOverN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_RTMoveOver (I32 axis, F64 pos, F64 vel_ratio);
HTM_API I32 HTM_RTMoveOver2 (I32 axis1, I32 axis2, F64 pos1, F64 pos2, F64 vel_ratio);
HTM_API I32 HTM_RTMoveOver3 (I32 axis1, I32 axis2, I32 axis3, F64 pos1, F64 pos2, F64 pos3, F64 vel_ratio);
HTM_API I32 HTM_RTMoveOver4 (I32 axis1, I32 axis2, I32 axis3, I32 axis4, F64 pos1, F64 pos2, F64 pos3, F64 pos4, F64 vel_ratio);
HTM_API I32 HTM_RTMoveOverN (I32 axisList[], F64 posList[], I32 axisNum, F64 vel_ratio);

HTM_API I32 HTM_TQMove(I32 axisIdx, F64 force, F64 vel);
HTM_API I32 HTM_BondMove (I32 axisIdx, F64 pos, F64 force, F64 vel);
/*-------------two axes linear interpolation motion functions-------------*/
HTM_API I32 HTM_LineEx(I32 axisList[], I32 axisNum, F64 posList[], F64 vel_ratio, I32 mode, const HTM_MovePara *mp);
HTM_API I32 HTM_Line(I32 axisList[], I32 axisNum, F64 posList[], F64 vel_ratio, I32 mode);
HTM_API I32 HTM_Arc2_CeEx(I32 axisIdx1, I32 axisIdx2, F64 center1, F64 center2, F64 pos1, F64 pos2, I32 direction, F64 vel_ratio, I32 mode, const HTM_MovePara *mp);
HTM_API I32 HTM_Arc2_Ce(I32 axisIdx1, I32 axisIdx2, F64 center1, F64 center2, F64 pos1, F64 pos2, I32 direction, F64 vel_ratio, I32 mode);
HTM_API I32 HTM_Arc2_CaEx(I32 axisIdx1, I32 axisIdx2, F64 center1, F64 center2, F64 angle, F64 vel_ratio, I32 mode, const HTM_MovePara *mp);
HTM_API I32 HTM_Arc2_Ca(I32 axisIdx1, I32 axisIdx2, F64 center1, F64 center2, F64 angle, F64 vel_ratio, I32 mode);
/*---------------single axis speed move functions-------------------------*/
HTM_API I32 HTM_SpeedEx(I32 axisIdx, F64 vel_ratio, const HTM_MovePara *mp);
HTM_API I32 HTM_Speed(I32 axisIdx, F64 vel_ratio);
/*---------------single axis stop motion functions------------------------*/
HTM_API I32 HTM_Stop(I32 axisIdx, I32 mode);
HTM_API I32 HTM_DStop(I32 axisIdx);
HTM_API I32 HTM_EStop(I32 axisIdx);
/*---------------clear axis alarm functions-------------------------------*/
HTM_API I32 HTM_ClearAlarm(I32 axisIdx);
/*---------------axis motion done functions-------------------------------*/
HTM_API I32 HTM_HomeDone (I32 axisIdx);
HTM_API I32 HTM_HomeDone2(I32 axis1, I32 axis2);
HTM_API I32 HTM_HomeDone3(I32 axis1, I32 axis2, I32 axis3);
HTM_API I32 HTM_HomeDone4(I32 axis1, I32 axis2, I32 axis3, I32 axis4);
HTM_API I32 HTM_HomeDoneN(I32 axisList[], I32 axisNum);

HTM_API I32 HTM_OnceDone (I32 axisIdx);
HTM_API I32 HTM_TQDone(I32 axisIdx);
HTM_API I32 HTM_Done (I32 axisIdx); 
HTM_API I32 HTM_Done2 (I32 axisIdx1, I32 axisIdx2);
HTM_API I32 HTM_Done3 (I32 axisIdx1, I32 axisIdx2, I32 axisIdx3);
HTM_API I32 HTM_Done4 (I32 axisIdx1, I32 axisIdx2, I32 axisIdx3, I32 axisIdx4);
HTM_API I32 HTM_DoneN(I32 axisList[], I32 axisNum);
/*--------------------get motion err code---------------------------------*/
HTM_API char *HTM_GetErr(I32 axisIdx, I32 errNo);
/* IO operation APIs */
HTM_API I32 HTM_LoadIoInfo(I32 ioIdx);
HTM_API I32 HTM_SaveIoInfo(I32 ioIdx);
HTM_API I32 HTM_ConfigIoInfo(I32 ioIdx);
HTM_API char* HTM_GetIoName(I32 ioIdx);
HTM_API I32 HTM_GetIoNum(void);
HTM_API I32 HTM_CheckIoIdx(I32 ioIdx);
HTM_API I32 HTM_GetIoCardType(I32 ioIdx);
HTM_API I32 HTM_GetIoDir(I32 ioIdx);

HTM_API I32 HTM_WriteDO(I32 ioIdx, I32 on_off);
HTM_API I32 HTM_ReadDI(I32 ioIdx);
HTM_API I32 HTM_ReadDO(I32 ioIdx);
HTM_API I32 HTM_GetDI(I32 ioIdx);
HTM_API I32 HTM_GetDO(I32 ioIdx);

HTM_API I32 HTM_WriteAO(I32 ioIdx, F64 volt);
HTM_API I32 HTM_ReadAI(I32 ioIdx, F64* volt);
HTM_API I32 HTM_ReadAO(I32 ioIdx, F64* volt);
HTM_API I32 HTM_GetAI(I32 ioIdx, F64 *ai_val);
HTM_API I32 HTM_GetAO(I32 ioIdx, F64 *ao_val);

HTM_API I32 HTM_WaitDI(I32 ioIdx, I32 on_off, F64 timeout);
HTM_API I32 HTM_WaitDI2(I32 ioIdx1, I32 ioIdx2, I32 on_off, F64 timeout);
HTM_API I32 HTM_WaitDI3(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, I32 on_off, F64 timeout);
HTM_API I32 HTM_WaitDI4(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, I32 ioIdx4, I32 on_off, F64 timeout);
HTM_API I32 HTM_WaitDIN(I32 ioList[], I32 io_num, I32 on_off, F64 timeout);

HTM_API I32 HTM_WaitAI(I32 ioIdx, F64 range_min, F64 range_max, F64 timeout);
HTM_API I32 HTM_WaitAI2(I32 ioIdx1, I32 ioIdx2, F64 range_min, F64 range_max, F64 timeout);
HTM_API I32 HTM_WaitAI3(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, F64 range_min, F64 range_max, F64 timeout);
HTM_API I32 HTM_WaitAI4(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, I32 ioIdx4, F64 range_min, F64 range_max, F64 timeout);
HTM_API I32 HTM_WaitAIN(I32 ioList[], I32 io_num, F64 range_min, F64 range_max, F64 timeout);

	/* 兼容版本<=3.15的接口，建议使用新版接口*/
	HTM_LEGACY I32 HTM_GetIoStatus(I32 ioIdx);
	HTM_LEGACY I32 HTM_SetBit(I32 ioIdx, I32 value);
	HTM_LEGACY I32 HTM_GetBit(I32 ioIdx);

	HTM_LEGACY I32 HTM_CheckIO(I32 ioIdx, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIO2(I32 ioIdx1, I32 ioIdx2, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIO3(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIO4(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, I32 ioIdx4, F64 timeout);
	HTM_LEGACY I32 HTM_CheckION(I32 ioList[], I32 io_num, F64 timeout);

	HTM_LEGACY I32 HTM_CheckIOff(I32 ioIdx, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIOff2(I32 ioIdx1, I32 ioIdx2, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIOff3(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIOff4(I32 ioIdx1, I32 ioIdx2, I32 ioIdx3, I32 ioIdx4, F64 timeout);
	HTM_LEGACY I32 HTM_CheckIOffN(I32 ioList[], I32 io_num, F64 timeout);

/* Other Device Operation APIs */
HTM_API I32 HTM_LoadDeviceInfo(I32 devIdx);
HTM_API I32 HTM_SaveDeviceInfo(I32 devIdx);
HTM_API I32 HTM_ConfigDeviceInfo(I32 devIdx);
HTM_API char *HTM_GetDeviceName(I32 devIdx);
HTM_API I32 HTM_GetDeviceType(I32 devIdx);
HTM_API I32 HTM_GetDeviceNum(void);
HTM_API I32 HTM_CheckDeviceIdx(I32 devIdx);

HTM_API I32 HTM_GetDeviceInfo(I32 devIdx, HTM_DeviceInfo *dev);
HTM_API I32 HTM_SetDeviceInfo(I32 devIdx, HTM_DeviceInfo *dev);

/* POSITIOIN TRIGGER */
HTM_API I32 HTM_SetPtTrigEnable(I32 devIdx, I32 ptIdx, I32 enable);
HTM_API I32 HTM_SetPtTrigPos(I32 devIdx, I32 ptIdx, F64 pos);
HTM_API I32 HTM_SWPosTrig(I32 devIdx);
HTM_API F64 HTM_GetPtTrigPos(I32 devIdx, I32 ptIdx);

HTM_API I32 HTM_SetLinTrigPos(I32 devIdx, TRIG_LIN *lin);
HTM_API I32 HTM_SetLinTrigEnable(I32 devIdx, I32 enable);
HTM_API I32 HTM_GetTrigCnt(I32 devIdx);
HTM_API I32 HTM_ResetTrigCnt(I32 devIdx);

HTM_API I32 HTM_GetTrigCurPos(I32 devIdx, F64 *pos);
HTM_API I32 HTM_SyncTrigCurPos(I32 devIdx);

/* Light Source Dirve */
HTM_API I32 HTM_SWLightTrig(I32 devIdx);
HTM_API I32 HTM_SetLightTrigSrc(I32 devIdx, U16 src_group);
HTM_API I32 HTM_SetLightTrigTime(I32 devIdx, F64 time_in_us);

/* Com Dirve */
HTM_API I32 HTM_GetComNo(I32 devIdx);
HTM_API I32 HTM_ComOpen(I32 devIdx);
HTM_API I32 HTM_ComClose(I32 devIdx);
HTM_API I32 HTM_ComWrite(I32 devIdx, char writeStr[], I32 count);
HTM_API I32 HTM_ComRead(I32 devIdx, char readStr[], I32 count);

/*---------------------------analysis APIs------------------------------------*/
HTM_API void HTM_CalcRotatePos (const F64 former_x, const F64 former_y, const F64 rot_center_x, const F64 rot_center_y,
		F64 delta_deg, F64 *new_x, F64 *new_y);
HTM_API I32 HTM_Mean(F64 input_array[], I32 total_num, I32 delete_max_num, I32 delete_min_num, F64 *mean_val);
HTM_API I32 HTM_Sort(F64 input_array[], I32 total_num, I32 descending, F64 *output_array);
HTM_API I32 HTM_MaxMin(F64 input_array[], I32 total_num, F64 *max, I32 *max_id, F64 *min, I32 *min_id);
HTM_API I32 HTM_Sum(F64 input_array[], I32 total_num, F64 *sum);
HTM_API I32 HTM_MatrixMul(void *A_aXn, void *B_nXb, I32 rowOfA_a, I32 row_colOfAB_n, I32 colOfB_b, void *AB_aXb);
HTM_API I32 HTM_MatrixInv(void *input_matrix, I32 dimensionNum, void* output_matrix);
HTM_API I32 HTM_MatrixTranspose(void *input_matrix, I32 row_num, I32 col_num, void* output_matrix);
HTM_API I32 HTM_LineFit(const F64 x[], const F64 y[], I32 numberOfPoints, F64 *k, F64 *b);
HTM_API I32 HTM_PolyFit(const F64 x[], const F64 y[], I32 numberOfPoints, I32 order, F64* k);
HTM_API I32 HTM_CircleFit(const F64 x[], const F64 y[], I32 numberOfPoints, F64 *centerX, F64 *centerY, F64 *r);
HTM_API I32 HTM_LineInterp(F64 x[], F64 y[], I32 numberOfElements, F64 xValue, F64 *interpolatedYValue);
HTM_API I32 HTM_SplineInterp(F64 x[], F64 y[], I32 numberOfElements, F64 xValue, F64 *interpolatedYValue);
HTM_API I32	HTM_CalcTransMtx(void *A_nXa, void *B_nXb, I32 numberOfRows_n, I32 dimensionOfA_a, I32 dimensionOfB_b, void *A2B_aplus1Xb);
HTM_API I32 HTM_TransPos(void *A_1Xa, void *A2B_aplus1Xb, I32 dimensionOfA_a, I32 dimensionOfB_b, void *B_1Xb);

/*--------------------------common APIs---------------------------------------*/
HTM_API F64 HTM_GetTimeSec(void);
HTM_API U32 HTM_GetTimeMs(void);
HTM_API char* HTM_GetTimeStr(void);
HTM_API char* HTM_GetDateStr(void);
HTM_API I32 HTM_GetTime(I32 *hour, I32 *minute, I32 *second);
HTM_API I32 HTM_GetDate(I32 *year, I32 *month, I32 *day);
#ifdef __cplusplus
    }
#endif

#endif  /* ndef __htm_bsp_lib_H__ */
/* End of file ---------------------------------------------------------------*/  
 
