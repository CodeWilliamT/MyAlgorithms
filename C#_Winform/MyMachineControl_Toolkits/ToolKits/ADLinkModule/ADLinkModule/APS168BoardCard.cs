using APS_Define_W32;
using APS168_W32;
using Module_Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AdLinkModule.Module_Hardware
{
    public class APS168BoardCard : GeneralMotionCard
    {
        private static Int32 boardID_InBits = 0;

        #region 板卡初始化
        /// <summary>
        /// 初始化运动板卡并配置各个轴
        /// </summary>
        /// <returns></returns>
        public override int InitBoard()
        {
            Int32 mode = 1;
            int i = 0;

            //设备初始化
            if (APS168.APS_initial(ref boardID_InBits, mode) < 0)
            {
                return -1;
            }
            for (i = 0; i < 32; i++)
            {
                if (((boardID_InBits >> i) & 0x01) == 1)
                {
                    APS168.APS_set_board_param(i, (int)(APS_Define.PRB_EMG_LOGIC), 1);
                }
            }

            return 0;
        }

        /// <summary>
        /// 关闭运动板卡
        /// </summary>
        /// <returns></returns>
        public override int CloseBoard()
        {
            Int32 err = APS168.APS_close();

            return err;
        }
        #endregion

        #region 输入输出获取
        /// <summary>
        /// 读取DI值
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public override int GetInputBitState(IOPort port)
        {
            int diVal = 0;

            APS168.APS_read_d_input(boardID_InBits, 0, ref diVal);
            diVal = diVal & (1 << (port.PortNo));

            return diVal;
        }

        /// <summary>
        /// 读取DO值
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public override int GetOutputBitState(IOPort port)
        {
            int value = 0;

            APS168.APS_read_d_channel_output(boardID_InBits, 0, port.PortNo, ref value);

            return value;
        }

        /// <summary>
        /// 设置DO值
        /// </summary>
        /// <param name="bit"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override int SetOutputBitState(IOPort port, int bitData)
        {
            APS168.APS_write_d_channel_output(boardID_InBits, 0, port.PortNo, bitData);

            return 0;
        }
        #endregion

        #region 电机励磁
        /// <summary>
        /// 配置电机励磁
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="off_on"></param>
        /// <returns></returns>
        public override int SetServo(AxisLocationInfo axisLocationInfo, int off_on)
        {
            return APS168.APS_set_servo_on(axisLocationInfo.AxisNo, off_on);
        }

        /// <summary>
        /// 获取电机励磁状态
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <returns></returns>
        public override int GetServo(AxisLocationInfo axisLocationInfo)
        {
            return (APS168.APS_motion_io_status(axisLocationInfo.AxisNo) >> (int)(APS_Define.MIO_SVON)) & 1;
        }
        #endregion

        #region 运动部分
        #region 单轴相对运动
        /// <summary>
        /// 单轴相对运动
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="pulseNum"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public override int MoveRelPulse(AxisLocationInfo axisLocationInfo, int pulseNum, VelocityCurveParams velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisLocationInfo, velocityCurveParams);

            //启动运动
            APS168.APS_relative_move(axisLocationInfo.AxisNo, pulseNum, (int)(velocityCurveParams.Maxvel));

            return 0;
        }
        #endregion

        #region 单轴绝对运动
        /// <summary>
        /// 单轴绝对运动
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="pulseNum"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public override int MoveAbsPulse(AxisLocationInfo axisLocationInfo, int pulseNum, VelocityCurveParams velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisLocationInfo, velocityCurveParams);

            //启动运动
            APS168.APS_absolute_move(axisLocationInfo.AxisNo, pulseNum, (int)(velocityCurveParams.Maxvel));

            return 0;
        }
        #endregion

        #region 单轴连续运动
        /// <summary>
        /// 连续运动
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="moveDirection"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public override int ContinuousMove(AxisLocationInfo axisLocationInfo, MoveDirection moveDirection, VelocityCurveParams velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisLocationInfo, velocityCurveParams);

            //启动运动
            APS168.APS_velocity_move(axisLocationInfo.AxisNo, (int)(velocityCurveParams.Maxvel * (int)moveDirection));

            return 0;
        }
        #endregion

        #region 单轴停止运动
        /// <summary>
        /// 立即停止
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <returns></returns>
        public override int ImmediateStop(AxisLocationInfo axisLocationInfo)
        {
            APS168.APS_emg_stop(axisLocationInfo.AxisNo);

            return 0;
        }

        /// <summary>
        /// 减速停止
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <returns></returns>
        public override int DecelStop(AxisLocationInfo axisLocationInfo)
        {
            APS168.APS_stop_move(axisLocationInfo.AxisNo);

            return 0;
        }
        #endregion

        #region 检查运动状态
        /// <summary>
        /// 检查运动状态
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <returns></returns>
        public override int CheckDone(AxisLocationInfo axisLocationInfo, double timeoutLimit)
        {
            int status = 0;
            int inp = 1;

            System.Diagnostics.Stopwatch strtime = new System.Diagnostics.Stopwatch();
            strtime.Start();

            do
            {
                //判断是否正常停止
                status = APS168.APS_motion_status(axisLocationInfo.AxisNo);
                if (((status >> (int)(APS_Define.MTS_ASTP)) & 1) == 1)
                {
                    //sCMCHomeStatus[axisIndex] = 0;
                    //return GetStopCode(sAxisInfo[axisIndex].aid);
                    return -1;
                }
                status = (status >> (int)(APS_Define.MTS_NSTP)) & 1;

                //判断INP鑫海
                if (axisLocationInfo.HasExtEncode == 1)
                {
                    inp = APS168.APS_motion_io_status(axisLocationInfo.AxisNo);
                    inp = (inp >> (int)(APS_Define.MIO_INP)) & 1;
                }

                //break条件是否满足
                if (status == 1 && inp == 1)
                    break;

                //检查是否超时
                strtime.Stop();
                if (strtime.ElapsedMilliseconds / 1000.0 > timeoutLimit)
                {
                    APS168.APS_emg_stop(axisLocationInfo.AxisNo);
                    return -2;
                }
                strtime.Start();

                //延时
                System.Threading.Thread.Sleep(20);
            } while (true);

            return 0;
        }
        #endregion

        #region 两轴做相对运动
        /// <summary>
        /// 两个轴同步做相对运动
        /// </summary>
        /// <param name="axisLocationInfo1"></param>
        /// <param name="pulseNum1"></param>
        /// <param name="axisLocationInfo2"></param>
        /// <param name="pulseNum2"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public override int MoveLine2RelPulse(AxisLocationInfo axisLocationInfo1, AxisLocationInfo axisLocationInfo2, int pulseNum1, int pulseNum2, VelocityCurveParams velocityCurveParams)
        {
            int[] axis = new int[2];
            int[] pos = new int[2];

            axis[0] = axisLocationInfo1.AxisNo;
            axis[1] = axisLocationInfo2.AxisNo;
            pos[0] = pulseNum1;
            pos[1] = pulseNum2;

            //设置速度
            SetAxisVelocity(axisLocationInfo1, velocityCurveParams);

            //启动运动
            //unsafe
            //{
            //    int[] buff = new int[size];
            //    fixed (int* pArray = &buff[0])
            //    {
            //        int array = *pArray;
            //        callmethod(ref array);
            //    }

            //}
            APS168.APS_relative_linear_move(2, ref axis, ref pos, (int)(velocityCurveParams.Maxvel));

            //IntPtr[] ptArray = new IntPtr[1];
            //ptArray[0] = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 65535);
            //IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * 1);
            //Marshal.Copy(ptArray, 0, pt, 1);

            //callmethod(ref pt);

            //for (int i = 0; i < 65535; i++)
            //{
            //    int n = (int)Marshal.PtrToStructure((IntPtr)((UInt32)ptArray[0] + i * sizeof(int)), typeof(int));
            //}
            //Marshal.FreeHGlobal(ptArray[0]);
            //Marshal.FreeHGlobal(pt); 

            return 0;
        }
        #endregion

        #region  两轴做绝对运动
        /// <summary>
        /// 两个轴同步做绝对运动
        /// </summary>
        /// <param name="axisLocationInfo1"></param>
        /// <param name="pulseNum1"></param>
        /// <param name="axisLocationInfo2"></param>
        /// <param name="pulseNum2"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public override int MoveLine2AbsPulse(AxisLocationInfo axisLocationInfo1, AxisLocationInfo axisLocationInfo2, int pulseNum1, int pulseNum2, VelocityCurveParams velocityCurveParams)
        {
            int[] axis = new int[2];
            int[] pos = new int[2];

            axis[0] = axisLocationInfo1.AxisNo;
            axis[1] = axisLocationInfo2.AxisNo;
            pos[0] = pulseNum1;
            pos[1] = pulseNum2;

            //设置速度
            SetAxisVelocity(axisLocationInfo1, velocityCurveParams);

            //启动运动
            //unsafe
            //{
            //    int[] buff = new int[size];
            //    fixed (int* pArray = &buff[0])
            //    {
            //        int array = *pArray;
            //        callmethod(ref array);
            //    }

            //}
            APS168.APS_absolute_linear_move(2, ref axis, ref pos, (int)(velocityCurveParams.Maxvel));

            //IntPtr[] ptArray = new IntPtr[1];
            //ptArray[0] = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 65535);
            //IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * 1);
            //Marshal.Copy(ptArray, 0, pt, 1);

            //callmethod(ref pt);

            //for (int i = 0; i < 65535; i++)
            //{
            //    int n = (int)Marshal.PtrToStructure((IntPtr)((UInt32)ptArray[0] + i * sizeof(int)), typeof(int));
            //}
            //Marshal.FreeHGlobal(ptArray[0]);
            //Marshal.FreeHGlobal(pt); 

            return 0;
        }
        #endregion
        #endregion

        #region 回零
        /// <summary>
        /// 单轴回零
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public override int BackHome(AxisLocationInfo axisLocationInfo, VelocityCurveParams velocityCurveParams)
        {
            return APS168.APS_home_move(axisLocationInfo.AxisNo);
        }

        /// <summary>
        /// 检查回零是否完成
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="extEncodes"></param>
        /// <param name="timeoutLimit"></param>
        /// <returns></returns>
        public override int CheckHomeDone(AxisLocationInfo axisLocationInfo, double timeoutLimit)
        {
            int status = 0;

            System.Diagnostics.Stopwatch strtime = new System.Diagnostics.Stopwatch();
            strtime.Start();

            do
            {
                //检查运动是否完成
                status = APS168.APS_motion_status(axisLocationInfo.AxisNo);
                if (((status >> (int)(APS_Define.MTS_ASTP)) & 1) == 1)
                {
                    //sCMCHomeStatus[axisIndex] = 0;
                    //return GetStopCode(sAxisInfo[axisIndex].aid);
                    return -1;
                }
                status = (status >> (int)(APS_Define.MTS_HMV)) & 1;
                if (status == 0)
                {
                    break;
                }

                //检查是否超时
                strtime.Stop();
                if (strtime.ElapsedMilliseconds / 1000.0 > timeoutLimit)
                {
                    APS168.APS_emg_stop(axisLocationInfo.AxisNo);
                    return -2;
                }
                strtime.Start();

                //延时
                System.Threading.Thread.Sleep(20);
            } while (true);

            return 0;
        }
        #endregion

        #region 配置
        #region 轴卡电平信号配置(板卡初始化后必须配置)
        public override int SetAxisSignalConfig(AxisLocationInfo axisLocationInfo, AxisSignalParams axisSignalParams)
        {
            //配置各轴
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_EL_LOGIC), axisSignalParams.ElLogic);     // 限位信号: 0-not inverse, 1-inverse
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_ORG_LOGIC), axisSignalParams.OrgLogic);    // ORG信号: 0-not inverse, 1-inverse    
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_ALM_LOGIC), axisSignalParams.AlmLogic);    // ALM信号：0-low active, 1-high active    
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_EZ_LOGIC), axisSignalParams.EzLogic);     // EZ信号： 0-low active, 1-high active 
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_INP_LOGIC), axisSignalParams.InpLogic);    // INP信号：0-low active, 1-high active 
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SERVO_LOGIC), axisSignalParams.ServoLogic);  // SERVO信号： 0-low logic, 1-high active
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_PLS_OPT_MODE), axisSignalParams.PlsOutMode);  //PLS Output Mode: pulse/dir
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_PLS_IPT_MODE), axisSignalParams.PlsInMode);  //PLS Input Mode: 1xAB
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_ENCODER_DIR), axisSignalParams.EncodeDir);   //encoder dir: positive

            return 0;
        }
        #endregion

        #region 回零配置(回零前配置)
        public override int SetAxisHomeConfig(AxisLocationInfo axisLocationInfo, HomeConfigParams homeConfigParams)
        {
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_MODE), homeConfigParams.Mode); // home mode  0:ORG  1:EL   2:EZ
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_DIR), homeConfigParams.Dir); // Set home direction   0:p-dir   1:n-dir
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_EZA), homeConfigParams.EZ);	// EZ alignment enable 0-no 1-yes    
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_CURVE), 0.5); 							// homing curve parten(T or s curve)  0-T 0~1.0-S       	      
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_ACC), homeConfigParams.MaxVel * 5); // Acceleration deceleration rate    
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_VS), 0);							   // homing start velocity          
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_VM), homeConfigParams.MaxVel); // homing max velocity              
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_VO), homeConfigParams.OrgVel); // Homing leave ORG velocity     
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_HOME_SHIFT), homeConfigParams.Shift); // The shift from ORG   

            return 0;
        }
        #endregion

        #region 限位配置
        public override int ClearSoftConfig(AxisLocationInfo axisLocationInfo)
        {
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SPEL_EN), 0);
            APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SMEL_EN), 0);

            return 0;
        }

        public override int SetSoftELConfig(AxisLocationInfo axisLocationInfo, SoftLimitParams softLimitParams)
        {
            if (softLimitParams.Enable == true)
            {
                APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SPEL_POS), softLimitParams.SPelPosition);
                APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SMEL_POS), softLimitParams.SMelPosition);
                APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SPEL_EN), 2);
                APS168.APS_set_axis_param(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SMEL_EN), 2);
            }
            else
            {
                ClearSoftConfig(axisLocationInfo);

            }

            return 0;
        }
        #endregion

        #region 配置输入口使能(部分板卡有此功能)
        public override int InputEnable(int cardNo, int inputEn)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 配置限位停止方式(部分板卡有此功能)
        public override int ConfigELStopType(AxisLocationInfo axisLocationInfo, ELStopType ELStopType)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 速度配置
        private int SetAxisVelocity(AxisLocationInfo axisLocationInfo, VelocityCurveParams velocityCurveParams)
        {
            //设置速度
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_SF), velocityCurveParams.Sfac);
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_ACC), velocityCurveParams.Tacc);
            APS168.APS_set_axis_param_f(axisLocationInfo.AxisNo, (int)(APS_Define.PRA_DEC), velocityCurveParams.Tdec);

            return 0;
        }
        #endregion
        #endregion

        #region 信息获取
        #region 脉冲计数器
        /// <summary>
        /// 获取轴的命令位置
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <returns></returns>
        public override double GetCommandPos(AxisLocationInfo axisLocationInfo)
        {
            double tmp = 0.0;
            APS168.APS_get_command_f(axisLocationInfo.AxisNo, ref tmp);

            return tmp;
        }

        /// <summary>
        /// 设置轴的命令位置
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override int SetCommandPos(AxisLocationInfo axisLocationInfo, double pos)
        {
            APS168.APS_set_command_f(axisLocationInfo.AxisNo, pos);
            return 0;
        }

        /// <summary>
        /// 获取轴的反馈位置
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <returns></returns>
        public override double GetFeedbackPos(AxisLocationInfo axisLocationInfo)
        {
            double tmp = 0.0;
            APS168.APS_get_position_f(axisLocationInfo.AxisNo, ref tmp);

            return tmp;
        }

        /// <summary>
        /// 设置轴的反馈位置
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override int SetFeedbackPos(AxisLocationInfo axisLocationInfo, double pos)
        {
            APS168.APS_set_position_f(axisLocationInfo.AxisNo, pos);
            return 0;
        }
        #endregion

        #region 轴IO信息获取
        public override AxisSignalState GetAxisState(AxisLocationInfo axisLocationInfo)
        {
            AxisSignalState axisState = new AxisSignalState();

            //正限位
            axisState.PosEL = ((APS168.APS_motion_io_status(axisLocationInfo.AxisNo) >> (int)(APS_Define.MIO_PEL)) & 1) == 1 ? GeneralElectricalLevelMode.High : GeneralElectricalLevelMode.Low;
            //负限位
            axisState.NegEL = ((APS168.APS_motion_io_status(axisLocationInfo.AxisNo) >> (int)(APS_Define.MIO_MEL)) & 1) == 1 ? GeneralElectricalLevelMode.High : GeneralElectricalLevelMode.Low;
            //ORG
            axisState.ORG = ((APS168.APS_motion_io_status(axisLocationInfo.AxisNo) >> (int)(APS_Define.MIO_ORG)) & 1) == 1 ? GeneralElectricalLevelMode.High : GeneralElectricalLevelMode.Low;
            //ALM信号
            axisState.ALM = ((APS168.APS_motion_io_status(axisLocationInfo.AxisNo) >> (int)(APS_Define.MIO_ALM)) & 1) == 1 ? GeneralElectricalLevelMode.High : GeneralElectricalLevelMode.Low;

            return axisState;
        }
        #endregion
        #endregion

        #region 获取错误码信息
        public override string GetErrMessage(int errNo)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
