# -*- coding: utf-8 -*-
"""
@file	 HTM_Bsp_Lib.py
@author  Tongqing CHEN
@version 4.06
@data    2018-06-12
@brief   htm_bsp module including APIs and configs(x86/x64)
"""
from ctypes import *
import os


# BSP所有枚举类型数据列表，与C语言lib中的宏定义一一对应

# 板卡类型 #
class DriveType(object):
	AMP208C = (0)     # ADLINK 204/208C Dirve Card #
	HTBOND = (1)      # HT Bond Dirve Card #
	HTDCMD = (2)      # HT DC Motor Dirve Card #
	HTDHVD = (3)      # HT DHVD Dirve Card #
	HTDLVD = (4)      # HT DLVD Dirve Card #
	HT202C = (5)      # HT 202C Motion Card #
	HTDHVD_PDO = (6)  # HT DHVD pdo-type Dirve Card #
	HB_DISPENSE = (7) # HT DHVD pdo-type Dirve Card #
	OTHER = (255)	    # Other Dirve Card #

# 轴类型 #	
class AxisType(object):
	SERVO = (0)       # Servo Motor伺服 #
	STEP = (1)        # Step Motor步进 #
	LINE = (2)        # Line Motor直线 #
	VOICE = (3)       # Voice Motor音圈 #
	TORQUE = (4)      # Torque Motor力矩 #
	DC = (5)          # DC Motor直流 #

# 运动标识参数 #
class MotionMode(object):
	ABS = (1)         # 绝对运动模式 #
	S = (2)           # S型曲线模式 #
	TQ = (4)          # 力矩运动模式 #
	BUF = (8)         # 缓存式运动 #

	AS = ABS_MOVE | S_MOVE# 绝对运动S曲线模式 #
	RS = S_MOVE               # 相对运动S曲线模式 #
	AT = ABS_MOVE             # 绝对运动T曲线模式 #
	RT = 0                        # 相对对运动T曲线模式 #

# stop mode #
class StopMode(object):
	EMG = (0)         # 立即停 #
	DEC = (1)         # 减速停 #
	PEL = (2)         # 正限位信号停 #
	MEL = (3)         # 负限位信号停 #
	IO0 = (4)         # IO0信号停 #
	IO1 = (5)         # IO1信号停 #
	IO2 = (6)         # IO2信号停 #
	IO3 = (7)         # IO3信号停 #

# motion io status bit number #
class MotionIO(object):
	ALM = (0)	        # 报警IO位  #
	PEL = (1)	        # 正限IO位  #
	MEL = (2)	        # 负限IO位  #
	ORG = (3)	        # 原点IO位  #
	IDX = (4)	        # Z相信号IO位  #
	INP = (6)	        # 到位IO位  #
	SVON = (7)	        # 励磁IO位  #
	SPEL = (11)        # 软正限IO位 #
	SMEL = (12)        # 软负限IO位 #
	IO0 = (13)         # 直流电机IO0 #
	IO1 = (14)         # 直流电机IO1#
	IO2 = (15)         # 直流电机IO2#
	IO3 = (16)         # 直流电机IO3#
	
# IO card type #
class IoCardType(object):
	DIO_HTNET = (0)
	AIO_HTNET = (1)
	DIO_208C = (2)
	AIO_208C = (3)

# 设备类型 #
class DeviceType(object):
	POSTRIG = (0)      # HT Pos Trigger Card #
	HTDHVD = (1)       # HT DHVD Dirve Card #
	LTSRC = (2)        # HT Light Source Drive #
	COM = (3)          # Serial Com #
	OTHER = (255)      # Other device #

#用于初始化BSP的结构信息，包含参数文件路径，板卡类型，轴/io/设备数，语言等信息
class INIT_PARA(Structure):
	_fields_ = [
		('para_file', c_char*256),    	# 参数文件路径，如该项为空则默认为工程目录下的htm_paras.db文件#
		('use_aps_card', c_ubyte), 		# 是否使用凌华板卡, 1-使用，0-不使用#
		('use_htnet_card', c_ubyte),    # 是否使用HTNET板卡#
		('offline_mode', c_ubyte),		# 模式，1-脱机模式，0-在线模式(初始化板卡+配置)，2-仅初始化板卡不配置参数#
		('max_axis_num', c_uint16),		# 轴数#
		('max_io_num', c_uint16),		# IO数#
		('max_dev_num', c_uint16),		# 其它设备数#
		('max_module_num', c_uint16),   # 模块数#
		('language', c_ubyte)]			# 0-CHN, 1-ENG#


# 运动参数结构，包含速度加速度等信息
class MOTION_PARA(Structure):
	_fields_ = [
		('vStart', c_double),   # 起始速度(mm/s) #
		('vMax', c_double),	    # 最大速度(mm/s) #
		('vEnd', c_double),		# 结束速度(mm/s) #
		('acc', c_double), 		# 加速度(mm/s2) #
		('dec', c_double),	    # 减速度(mm/s2) #
		('sFactor', c_double),  # s曲线因子(0~1.0)#
		('timeout', c_double)]  # 超时时间(s)#


# 轴信息结构体，包含轴类型、地址、驱动信息、运动信息等。
class AXIS_INFO(Structure):
	_fields_ = [
		('axisName', c_char*32),    # Axis Name #
		('driveType', c_byte),	    # Drive Card Type  #
		('axisType', c_byte),		# Axis type 	#
		('busNo', c_uint16),	    # Card/Bus Number 卡号/总线号  #
		('nodeAddr', c_uint16),	    # Axis/Node Address 轴号/节点号 #
		('portNo', c_uint16),		# Port Number 端口号 #
		('pulseFactor', c_double),  # 脉冲当量(pulse/mm) #
		('extEncode', c_ubyte),	    # 是否有编码器反馈 #
		('mp', MOTION_PARA),        # 运动参数 #
	
		('enableMEL', c_ubyte),  	# 负电子限位使能, 0-不使能，1-使能 #	
		('enablePEL', c_ubyte),  	# 正电子限位使能, 0-不使能，1-使能 #
		('enableSMEL', c_ubyte),  	# 负软限位使能, 0-不使能，1-使能 #
		('enableSPEL', c_ubyte),  	# 正软限位使能, 0-不使能，1-使能 #
		('sPELPos', c_double), 		# 正限位位置(mm) #
		('sMELPos', c_double), 		# 负限位(mm) #
	
		('homeMode', c_byte),		# 回零模式 #
		('homeDir', c_ubyte),		# 回零方向 #
		('homeEZA', c_ubyte),		# EZ信号使能 #
	#	('homeSFactor', c_double),  # 回零S因子(0~1.0) no longer use after version 3.10 #
		('hdStpCrt', c_uint32),		# DHVD硬限位回零电流限幅#
		('hdStpTime', c_uint32),    # DHVD硬限位回零时间#
		('homeAcc', c_double),		# 回零加速度(mm/s2) #
		('homeVs', c_double),		# 回零起始速度(mm/s) #
		('homeVm', c_double),		# 回零最大速度(mm/s) #
		('homeVo', c_double),		# 回零原点速度(mm/s) #
		('homeShift', c_double),    # 回零偏移量(mm) #
		('homeTimeout', c_double),  # 回零超时时间(s) #

		('almLogic', c_ubyte),		# 报警信号逻辑 #
		('elLogic', c_ubyte),		# 限位信号逻辑 #
		('orgLogic', c_ubyte),		# 原点信号逻辑 #
		('ezLogic', c_ubyte),		# EZ信号逻辑 #
		('inpLogic', c_ubyte),       # 到位信号逻辑 #
		('servoLogic', c_ubyte),     # 励磁信号逻辑 #
		('optMode', c_ubyte),        # 输出信号逻辑 #
		('iptMode', c_ubyte), 		# 输入信号逻辑 #
		('encodeDir', c_ubyte),      # 编码器方向 #
	
		('maxCurrent', c_double),   # 最大电流(A)仅对htnet-DCMC驱动板有效#
		('polarity', c_ubyte),		# 位置极性(0/1)#
		('pError', c_double),		# 位置误差(mm)#
		('peTimeout', c_uint16),    # 位置误差超时(ms)#
		('inpValue', c_double),   	# 到位窗口(mm)#
		('inpInterval', c_uint16),  # 到位窗口时间(ms)#
	
		('stopDec', c_double)]		# 快速停止减速度(m/s2), 该参数对HTM_EStop急停接口有效#


# 轴状态结构，包含轴的状态信息，如报警、限位、位置等。
class AXIS_STATUS(Structure):
	_fields_ = [
		('alm', c_ubyte),			# 报警信号(0/1) #
		('pel', c_ubyte),			# 正限位信号(0/1) #
		('mel', c_ubyte),			# 负限位信号(0/1) #
		('org', c_ubyte),			# 原点信号(0/1) #
		('idx', c_ubyte),			# index信号(0/1) #
		('inp', c_ubyte),			# INP信号(0/1) #
		('svon', c_ubyte),		    # 励磁信号(0/1)#
		('cmdPos', c_double),		# 指令位置(mm) #
		('fbkPos', c_double)]		# 反馈信号(mm) #


# IO信息结构，包含IO设备的信息，如地址、方向、极性等
class IO_INFO(Structure):
	_fields_ = [
		('ioName', c_char*32),      # IO名称(不超过32个字节) #
		('busNo', c_uint16),		# 总线号 #
		('nodeAddr', c_uint16),	    # 节点号 #
		('portNo', c_uint16),		# 端口号 #
		('ioSrc', c_ubyte),		    # IO方向(0-输入,1-输出) #
		('polarity', c_ubyte),	    # 极性(0-默认,1-取反) #
		('disable', c_ubyte)]	    # 是否禁用(0-不禁用,1-禁用)，禁用后将不能正常操作该IO设备 #

# 点表触发模式配置，包含触发点的位置、方向、使能等信息
class TRIG_POINT(Structure):
	_fields_ = [('pos', c_double),          # Position 触发位置#
	('dir', c_ubyte),            # Direction触发方向#
	('enable', c_ubyte)]	        # 使能0-不使能，1-使能#


# 线性触发模式配置, 包含触发的起点、终点、间隔等信息
class TRIG_LINEAR(Structure):
	_fields_ = [
		('startPos', c_double),     # 线性触发起始位置(mm) #
		('endPos', c_double),	    # 线性触发结束位置(mm) #
		('interval', c_double)]     # 线性触发间隔(mm) #


# 位置触发板信息，包含对应的轴、触发模式等
class POS_TRIG_PARA(Structure):
	_fields_ = [
		('axisIdx', c_uint16),		# 对应轴 #
		('trigMode', c_ubyte),		# 触发模式 0点表1线性 #
		('polarity', c_ubyte),		# 取反 #
		('trigPt', c_double), # 点表模式触发位置(mm) #
		('lineEnable', c_ubyte),		# 线性触发使能 #
		('trigLin', TRIG_LINEAR),	# 线性触发配置#
		('ffTime', c_double)]		# 相机触发超前时间(us) #


# 光源驱动板信息，包含触发源、触发时间等信息
class LIGHT_DIRVE_PARA(Structure):
	_fields_ = [
		('tirgSrc', c_ubyte*4),		# 触发源#
		('opTime', c_double)]		# 脉冲宽度时间us#


# 串口信息，包含串口号、波特率等信息
class COM_PARA(Structure):
	_fields_ = [
		('comNo', c_int32),         # 串口号#
		('baudRate', c_int32),      # 波特率#
		('parity', c_int32),        # 校验位#
		('dataBit', c_int32),       # 数据位#
		('stopBit', c_int32)]       # 停止位#


# 设备参数结构，可以配置为不同的设备信息，类似于C语言中的联合体
class DEVICE_PARA(Structure):
	_fields_ = [
		('posTrig', POS_TRIG_PARA), # 位置触发板信息#
		('light', LIGHT_DIRVE_PARA), # 光源驱动板信息#
		('com', COM_PARA)]        # 串口信息#


# 设备信息，包含设备地址、设备参数等信息
class DEVICE_INFO(Structure):
	_fields_ = [
		('devName', c_char*32),      # 设备名称#
		('busNo', c_uint16),        # 总线号 #
		('nodeAddr', c_uint16),		# 节点号 #
		('portNo', c_uint16),	    # 端口号 #
		('devType', c_ubyte),	    # 设备类型#
		('devPara', DEVICE_PARA)]   # 设备信息#

# HTM_API 原生C接口
os.environ['path'] += ';' + os.path.dirname(os.path.abspath(__file__)) + '\\HTM_Bsp_Lib'
HTM_API = CDLL('HTM_Bsp_Lib.dll')


class HTM(object):
	# ----------------------------------- HTM_Bsp Operation------------------------------------ #
	@staticmethod
	def GetVersionNo():
		return HTM_API.HTM_Bsp_GetVersionNo()

	@staticmethod
	def GetVersionInfo():
		_ver = HTM_API.HTM_Bsp_GetVersionInfo()
		return string_at(_ver).decode()

	@staticmethod
	def ShowVersion():
		return HTM_API.HTM_Bsp_ShowVersion()
		
	@staticmethod
	def Init(para):
		return HTM_API.HTM_Bsp_Init(byref(para))
	
	@staticmethod
	def InitFromFile(ini_or_xml_file):
		return HTM_API.HTM_Bsp_InitFromFile(ini_or_xml_file)
	
	@staticmethod 
	def GetConfigFile():
		_file = HTM_API.HTM_Bsp_GetConfigFilePath()
		return string_at(_file).decode()
	
	@staticmethod 
	def GetLastErrStr():
		_err = HTM_API.HTM_Bsp_GetLastErrStr()
		return string_at(_err).decode()
	
	@staticmethod 
	def GetInitPara(para):
		_get_para = HTM_API.HTM_Bsp_GetInitPara
		_get_para.argtypes = [POINTER(INIT_PARA)]
		return _get_para(byref(para))
	
	@staticmethod 
	def Discard():
		HTM_API.HTM_Bsp_Discard()
		
	@staticmethod 
	def LoadUI():
		return HTM_API.HTM_Bsp_LoadUI()

	@staticmethod
	def LoadToolUI():
		return HTM_API.HTM_Bsp_LoadToolUI()

	# ---------------------------------- HTM_EventLog APIs------------------------------------#
	@staticmethod
	def EventLog_LoadUI():
		return HTM_API.HTM_EventLog_LoadUI()
	
	@staticmethod 
	def EventLog_Start(task_id, name, msg):
		return HTM_API.HTM_EventLog_Start(task_id, name, msg)
	
	@staticmethod 
	def EventLog_End(task_id):
		return HTM_API.HTM_EventLog_End(task_id)

	# ------------------------------- Axis operation APIs -----------------------------------#
	@staticmethod 
	def InitAxisInfo(axisNum, offline_mode):
		return HTM_API.HTM_InitAxisInfo(axisNum, offline_mode)

	@staticmethod 
	def DiscardAxisInfo():
		return HTM_API.HTM_DiscardAxisInfo()

	@staticmethod
	def LoadAxisInfo(axisIdx):
		return HTM_API.HTM_LoadAxisInfo(axisIdx)

	@staticmethod
	def	SaveAxisInfo(axisIdx):
		return HTM_API.HTM_SaveAxisInfo(axisIdx)

	# Get(set) axis parameter functions #
	@staticmethod
	def	GetAxisInfo(axisIdx, axisInfo):
		_get_axis = HTM_API.HTM_GetAxisInfo
		_get_axis.argtypes = [c_int32, POINTER(AXIS_INFO)]
		return _get_axis(axisIdx, byref(axisInfo))

	@staticmethod
	def SetAxisInfo(axisIdx, axisInfo):
		_set_axis = HTM_API.HTM_SetAxisInfo
		_set_axis.argtypes = [c_int32, POINTER(AXIS_INFO)]
		return _set_axis(axisIdx, byref(axisInfo))

	@staticmethod
	def UpdateAxisStatus():
		return HTM_API.HTM_UpdateAxisStatus()

	@staticmethod
	def ConfigAxisInfo(axisIdx):
		return HTM_API.HTM_ConfigAxisInfo(axisIdx)

	@staticmethod
	def GetAxisName(axisIdx):
		_ret = HTM_API.HTM_GetAxisName(axisIdx)
		return string_at(_ret).decode()


	@staticmethod
	def	GetMaxVel(axisIdx):
		_func = HTM_API.HTM_GetMaxVel
		_func.restype = c_double
		return _func(axisIdx)

	@staticmethod
	def GetTimeOut(axisIdx):
		_func = HTM_API.HTM_GetTimeOut
		_func.restype = c_double
		return _func(axisIdx)

	@staticmethod
	def GetPulseFactor(axisIdx):
		_func = HTM_API.HTM_GetPulseFactor
		_func.restype = c_double
		return _func(axisIdx)

	@staticmethod
	def GetINP(axisIdx):
		return HTM_API.HTM_GetINP(axisIdx)

	@staticmethod
	def GetPEL(axisIdx):
		return HTM_API.HTM_GetPEL(axisIdx)

	@staticmethod
	def GetMEL(axisIdx):
		return HTM_API.HTM_GetMEL(axisIdx)

	@staticmethod
	def GetAlarm(axisIdx):
		return HTM_API.HTM_GetAlarm(axisIdx)

	@staticmethod
	def SetSoftLimit(axisIdx, spel_en, spel, smel_en, smel):
		_set_limit = HTM_API.HTM_SetSoftLimitEx
		_set_limit.argtypes = [c_int32, c_int32, c_double, c_double]
		return _set_limit(axisIdx, (spel_en) * 2 + smel_en, spel, smel)

	@staticmethod
	def GetMotionIO(axisIdx, bit):
		return HTM_API.HTM_GetMotionIOBit(axisIdx, bit)

	@staticmethod
	def GetMovePara(axisIdx):
		_func = HTM_API.HTM_GetMovePara
		_func.restype = POINTER(MOTION_PARA)
		return _func(axisIdx).contents

	# Get(set) motion position(velocity) command(feedback) #
	@staticmethod
	def SetCurHomePos(axisIdx):
		return HTM_API.HTM_SetCurHomePos(axisIdx)

	@staticmethod
	def SetCmdPos(axisIdx, cmdpos_in_unit):
		_func =  HTM_API.HTM_SetCmdPos
		_func.argtypes =[c_int32, c_double]
		return _func(axisIdx, cmdpos_in_unit)

	@staticmethod
	def SetFbkPos(axisIdx, cmdpos_in_unit):
		_func =  HTM_API.HTM_SetFbkPos
		_func.argtypes =[c_int32, c_double]
		return _func(axisIdx, cmdpos_in_unit)

	@staticmethod
	def GetCmdPos(axis_index):
		_get_cmd = HTM_API.HTM_GetFbkPos
		_get_cmd.restype = c_double
		return _get_cmd(axis_index)

	@staticmethod
	def GetFbkPos(self, axis_index):
		_get_fbk = HTM_API.HTM_GetFbkPos
		_get_fbk.restype = c_double
		return _get_fbk(axis_index)

	@staticmethod
	def GetAxisStatus(axisIdx):
		_func = HTM_API.HTM_GetAxisStatus
		_func.restype = POINTER(AXIS_STATUS)
		return _func(axisIdx).contents

	@staticmethod
	def GetCmdVel(axis_index):
		_get_cmd = HTM_API.HTM_GetCmdVel
		_get_cmd.restype = c_double
		return _get_cmd(axis_index)

	@staticmethod
	def GetFbkVel(axis_index):
		_get_fbk = HTM_API.HTM_GetFbkVel
		_get_fbk.restype = c_double
		return _get_fbk(axis_index)
	
	# Single axis move
	@staticmethod 
	def Home(axisIdx):
		return HTM_API.HTM_Home(axisIdx)

	@staticmethod
	def CalcMotionTime(axisIdx, distance, motion_time):
		_calc_time = HTM_API.HTM_CalcMotionTime
		_calc_time.argtypes = [c_int32, c_double, POINTER(c_double)]
		return _calc_time(axisIdx, distance, byref(motion_time))

	@staticmethod
	def CalcMotionTimeEx(motion_para, distance, motion_time):
		_calc_time = HTM_API.HTM_CalcMotionTimeEx
		_calc_time.argtypes = [POINTER(MOTION_PARA), c_double, POINTER(c_double)]
		return _calc_time(byref(motion_para), distance, byref(motion_time))

	@staticmethod 
	def MoveEx(axisIdx, pos, vel, mode, mp):
		_move = HTM_API.HTM_MoveEx
		_move.argtypes = [c_int32, c_double, c_double, c_int32, POINTER(MOTION_PARA)]
		return _move(axisIdx, pos, vel, mode, byref(mp))
	
	@staticmethod 
	def Move(axisIdx, pos, vel, mode):
		_move = HTM_API.HTM_Move
		_move.argtypes = [c_int32, c_double, c_double, c_int32]
		return _move(axisIdx, pos, vel, mode)
	
	@staticmethod 
	def Move2(axis1, axis2, pos1, pos2, vel, mode):
		_move = HTM_API.HTM_Move2
		_move.argtypes = [c_int32, c_double, c_int32, c_double, c_double, c_int32]
		return _move(axis1, axis2, pos1, pos2, vel, mode)
	
	@staticmethod
	def ASMove(axis_index, pos, speed_percentage=1.0):
		_as_move = HTM_API.HTM_ASMove
		_as_move.argtypes = [c_int32, c_double, c_double]
		return _as_move(axis_index, pos, speed_percentage)
	
	@staticmethod
	def RSMove(axis_index, pos, speed_percentage=1.0):
		_rs_move = HTM_API.HTM_ASMove
		_rs_move.argtypes = [c_int32, c_double, c_double]
		return _rs_move(axis_index, pos, speed_percentage)
	
	@staticmethod 
	def Speed(axisIdx, vel = 1.0):
		_speed = HTM_API.HTM_Speed
		_speed.argtypes = [c_int32, c_double]
		return _speed(axisIdx, vel)
	
	@staticmethod 
	def SpeedEx(axisIdx, vel, mp):
		_speed = HTM_API.HTM_SpeedEx
		_speed.argtypes = [c_int32, c_double, POINTER(MOTION_PARA)]
		return _speed(axisIdx, vel, byref(mp))
	
	@staticmethod
	def EStop(axisIdx):
		return HTM_API.HTM_EStop(axisIdx)
	
	@staticmethod 
	def Stop(axisIdx, mode):
		return HTM_API.HTM_Stop(axisIdx, mode)
	
	@staticmethod 
	def SetSVON(axisIdx, ON = 1):
		return HTM_API.HTM_SetSVON(axisIdx, ON)
	
	@staticmethod 
	def GetSVON(axisIdx):
		return HTM_API.HTM_GetSVON(axisIdx)

	# Motion done #
	@staticmethod 
	def OnceDone(axisIdx):
		return HTM_API.HTM_OnceDone(axisIdx)
	
	@staticmethod 
	def Done(axisIdx):
		return HTM_API.HTM_Done(axisIdx)
	
	@staticmethod 
	def Done2(axisIdx1, axisIdx2):
		return HTM_API.HTM_Done2(axisIdx1, axisIdx2)
	    
	@staticmethod 
	def Done3(axisIdx1, axisIdx2, axisIdx3):
		return HTM_API.HTM_Done3(axisIdx1, axisIdx2, axisIdx3)
	
	@staticmethod 
	def Done4(axisIdx1, axisIdx2, axisIdx3, axisIdx4):
		return HTM_API.HTM_Done4(axisIdx1, axisIdx2, axisIdx3, axisIdx4)

	@staticmethod
	def DoneEx(axesList, axisNum):
		_int32_array = (c_int32 * axisNum)()
		for _i in range(axisNum):
			_int32_array[_i] = axesList[_i]
		return HTM_API.HTM_DoneEx(byref(_int32_array), axisNum)

	@staticmethod
	def TQDone(axisIdx):
		return HTM_API.HTM_TQDone(axisIdx)
	
	@staticmethod 
	def HomeDone(axisIdx):
		return HTM_API.HTM_HomeDone(axisIdx)

	# Two axes linear interpolation motion functions #
	@staticmethod 
	def LineEx(axisList, axisNum, posList, vel_ratio, mode, mp):
		_line = HTM_API.HTM_LineEx
		_line.argtypes = [POINTER(c_int32), c_int32, POINTER(c_double), c_double, c_int32, POINTER(MOTION_PARA)]
		return _line(byref(axisList), axisNum, byref(posList), vel_ratio, mode, byref(mp))
	
	@staticmethod 
	def Line(axisList, axisNum, posList, vel_ratio, mode):
		_line = HTM_API.HTM_Line
		_line.argtypes = [POINTER(c_int32), c_int32, POINTER(c_double), c_double, c_int32]
		return _line(byref(axisList), axisNum, byref(posList), vel_ratio, mode)
	
	@staticmethod 
	def Line(axis1, axis2, pos1, pos2, vel_ratio, mode, mp):
		pass
	
	@staticmethod 
	def Line(axis1, axis2, pos1, pos2, vel_ratio, mode):
		pass
	
	@staticmethod 
	def ArcCe(axisIdx1, axisIdx2, center1, center2, pos1, pos2, direction, vel_ratio, mode, mp):
		pass
	
	@staticmethod 
	def ArcCe(axisIdx1, axisIdx2, center1, center2, pos1, pos2, direction, vel_ratio, mode):
		pass
	
	@staticmethod 
	def ArcCa(axisIdx1, axisIdx2, center1, center2, angle, vel_ratio, mode, mp):
		pass
	
	@staticmethod 
	def ArcCa(axisIdx1, axisIdx2, center1, center2, angle, vel_ratio, mode):
		pass
	
	# ---------------------------------------- IO operation APIs --------------------------------------------#	
	@staticmethod 
	def GetIoName(ioIdx):
		_name = HTM_API.HTM_GetIoName(ioIdx)
		return string_at(_name).decode()
	
	@staticmethod 
	def GetIoInfo(ioIdx, io):
		pass
	
	@staticmethod
	def SetIoInfo(ioIdx, io):
		pass
	
	@staticmethod
	def WriteDO(ioIdx, ON):
		return HTM_API.HTM_WriteDO(ioIdx, ON)
		
	@staticmethod
	def ReadDI(ioIdx):
		return HTM_API.HTM_ReadDI(ioIdx)
	
	@staticmethod
	def ReadDO(ioIdx):
		return HTM_API.HTM_ReadDO(ioIdx)

	@staticmethod
	def GetDI(ioIdx):
		return HTM_API.HTM_GetDI(ioIdx)
	
	@staticmethod
	def GetDO(ioIdx):
		return HTM_API.HTM_GetDO(ioIdx)
	
	@staticmethod
	def WriteAO(ioIdx, volt):
		_func = HTM_API.HTM_WriteAO
		_func.argtypes = [c_int32, c_double]
		return _func(ioIdx, volt)
		
	@staticmethod
	def ReadAI(ioIdx, volt):
		_func = HTM_API.HTM_ReadAI
		_func.argtypes = [c_int32, POINTER(c_double)]
		return _func(ioIdx, bref(volt))
	
	@staticmethod
	def ReadAO(ioIdx, volt):
		_func = HTM_API.HTM_ReadAO
		_func.argtypes = [c_int32, POINTER(c_double)]
		return _func(ioIdx, bref(volt))

	@staticmethod
	def GetAI(ioIdx, volt):
		_func = HTM_API.HTM_GetAI
		_func.argtypes = [c_int32, POINTER(c_double)]
		return _func(ioIdx, bref(volt))
	
	@staticmethod
	def GetAO(ioIdx, volt):
		_func = HTM_API.HTM_GetAO
		_func.argtypes = [c_int32, POINTER(c_double)]
		return _func(ioIdx, bref(volt))

	@staticmethod
	def WaitDI(ioIdx, ON, timeout_second):
		_func = HTM_API.HTM_WaitDI
		_func.argtypes = [c_int32, c_int32, c_double]
		return _func(ioIdx, ON, timeout_second)

	@staticmethod
	def WaitDI2(ioIdx1, ioIdx2, ON, timeout_second):
		_func = HTM_API.HTM_WaitDI2
		_func.argtypes = [c_int32, c_int32, c_int32, c_double]
		return _func(ioIdx1, ioIdx2, ON, timeout_second)

	@staticmethod
	def WaitDI3(ioIdx1, ioIdx2, ioIdx3, ON, timeout_second):
		_func = HTM_API.HTM_WaitDI3
		_func.argtypes = [c_int32, c_int32, c_int32, c_int32, c_double]
		return _func(ioIdx1, ioIdx2, ioIdx3, ON, timeout_second)

	@staticmethod
	def WaitDI4(ioIdx1, ioIdx2, ioIdx3, ioIdx4, ON, timeout_second):
		_func = HTM_API.WaitDI4
		_func.argtypes = [c_int32, c_int32, c_int32, c_int32, c_int32, c_double]
		return _func(ioIdx1, ioIdx2, ioIdx3, ioIdx4, ON, timeout_second)

	@staticmethod
	def WaitDIN(ioList, ioNum, ON, timeout_second):
		_func = HTM_API.HTM_WaitDIN
		_func.argtypes = [POINTER(c_int32), c_int32, c_int32, c_double]
		return _func(byref(ioList), ioNum, ON, timeout_second)
	
	@staticmethod 
	def WaitAI(ioIdx, range_min, range_max, timeout_second):
		_func = HTM_API.HTM_WaitAI
		_func.argtypes = [c_int32, c_double, c_double, c_double]
		return _func(ioIdx, range_min, range_max, timeout_second)
	
	@staticmethod 
	def WaitAI2( ioIdx1, ioIdx2, range_min, range_max, timeout_second):
		_func = HTM_API.HTM_WaitAI2
		_func.argtypes = [c_int32, c_int32, c_double, c_double, c_double]
		return _func(ioIdx1, ioIdx2, range_min, range_max, timeout_second)
	
	@staticmethod 
	def WaitAI( ioIdx1, ioIdx2, ioIdx3, range_min, range_max, timeout_second):
		_func = HTM_API.HTM_WaitAI3
		_func.argtypes = [c_int32, c_int32, c_int32, c_double, c_double, c_double]
		return _func(ioIdx1, ioIdx2, ioIdx3, range_min, range_max, timeout_second)
	
	@staticmethod 
	def WaitAI4( ioIdx1, ioIdx2, ioIdx3, ioIdx4, range_min, range_max, timeout_second):
		_func = HTM_API.HTM_WaitAI4
		_func.argtypes = [c_int32, c_int32, c_int32, c_int32, c_double, c_double, c_double]
		return _func(ioIdx1, ioIdx2, ioIdx3, ioIdx4, range_min, range_max, timeout_second)
	
	@staticmethod 
	def WaitAIN(ioList, ioNum, range_min, range_max, timeout_second):
		_func = HTM_API.HTM_WaitAIN
		_func.argtypes = [POINTER(c_int32), c_int32, c_double, c_double, c_double]
		return _func(byref(ioList), ioNum, range_min, range_max, timeout_second)
	
	# ---------------------------- Other Device Operation APIs -----------------------------#
	@staticmethod 
	def GetDeviceName( devIdx):
		_name = HTM_API.HTM_GetDeviceName(devIdx)
		return string_at(_name).decode()
	
	@staticmethod 
	def SetPtTrigEnable(devIdx, ptIdx = -1, enable = 1):
		return HTM_API.HTM_SetPtTrigEnable(devIdx, ptIdx, enable)
	
	@staticmethod 
	def SetPtTrigPos(devIdx, ptIdx, pos):
		return HTM_API.HTM_SetPtTrigPos(devIdx, ptIdx, pos)
	
	@staticmethod 
	def GetPtTrigPos(devIdx, ptIdx):
		return HTM_API.HTM_GetPtTrigPos(devIdx, ptIdx)
	
	@staticmethod 
	def SetLinTrigEnable(devIdx, enable = 1):
		return HTM_API.HTM_SetLinTrigEnable(devIdx, enable)
	
	@staticmethod 
	def SWPosTrig(devIdx):
		return HTM_API.HTM_SWPosTrig(devIdx)
	
	@staticmethod 
	def GetTrigCnt(devIdx):
		return HTM_API.HTM_GetTrigCnt(devIdx)
	
	@staticmethod 
	def ClearTrigCnt(devIdx):
		return HTM_API.HTM_GetTrigCnt(devIdx)
	
	@staticmethod 
	def SyncTrigCurPos(devIdx):
		return HTM_API.HTM_SyncTrigCurPos(devIdx)

	# Light Source Dirve #
	@staticmethod 
	def SWLightTrig(devIdx):
		return HTM_API.HTM_SWLightTrig(devIdx)
	
	@staticmethod 
	def SetLightTrigSrc(devIdx, src_group):
		return HTM_API.HTM_SetLightTrigSrc(devIdx, src_group)
	
	@staticmethod 
	def SetLightTrigTime(devIdx, time_us):
		return HTM_API.HTM_SetLightTrigTime(devIdx, time_us)
	
	# Com Dirve #
	@staticmethod 
	def GetComNo(devIdx):
		return HTM_API.HTM_GetComNo(devIdx)
	
	@staticmethod 
	def ComOpen(devIdx):
		return HTM_API.HTM_ComOpen(devIdx)
	
	@staticmethod 
	def ComClose(devIdx):
		return HTM_API.HTM_ComClose(devIdx)