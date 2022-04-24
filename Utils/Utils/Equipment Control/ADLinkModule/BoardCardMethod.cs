namespace ToolKits.ADLinkModule
{
    #region 通用板卡顶级抽象
    /// <summary>
    /// 板卡共有方法(顶层)
    /// </summary>
    public abstract class GeneralBoardCard
    {
        public abstract int InitBoard();
        public abstract int CloseBoard();
        public abstract int GetInputBitState(IOPort port);
        public abstract int GetOutputBitState(IOPort port);
        public abstract int SetOutputBitState(IOPort port, int bitData);
    }
    #endregion

    #region IO板卡次级抽象
    /// <summary>
    /// 通用IO板卡(第二层)
    /// </summary>
    public abstract class GeneralIOCard : GeneralBoardCard
    {
        public abstract uint GetInputPortState(int card, int port);
        public abstract uint GetOutputPortState(int card, int port);
        public abstract int SetOutputPortState(int card, int port, uint val);
    }
    #endregion

    #region 运动板卡次级抽象
    /// <summary>
    /// 通用运动板卡(第二层)
    /// </summary>
    public abstract class GeneralMotionCard : GeneralBoardCard
    {
        #region 电机励磁
        public virtual int SetServo(ExecuteBody executeBody, int off_on)
        {
            return SetServo(executeBody.AxisLocationInfo, off_on);
        }
        public abstract int SetServo(AxisLocationInfo axisLocationInfo, int off_on);

        public virtual int GetServo(ExecuteBody executeBody)
        {
            return GetServo(executeBody.AxisLocationInfo);
        }
        public abstract int GetServo(AxisLocationInfo axisLocationInfo);
        #endregion
        
        #region 运动部分
        #region 单轴相对运动命令
        /// <summary>
        /// 命令指定机构以相对坐标运行一段指定数值（单位为执行结构的指定的单位）
        /// </summary>
        /// <param name="executeBody"></param>
        /// <param name="val">数值的正负代表跑动的方向</param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public virtual int MoveRelValue(ExecuteBody executeBody, double val)
        {
            int pulseNum = (int)(val * executeBody.TransmissionParams.EquivalentPulse);
            return MoveRelPulse(executeBody, pulseNum);
        }

        /// <summary>
        /// 命令指定机构以相对坐标运行一段指定脉冲
        /// </summary>
        /// <param name="executeBody"></param>
        /// <param name="pulseNum">控制卡实际发送的脉冲数</param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public virtual int MoveRelPulse(ExecuteBody executeBody, int pulseNum)
        {
            return MoveRelPulse(executeBody.AxisLocationInfo, pulseNum, executeBody.VelocityCurveParams);
        }
        public abstract int MoveRelPulse(AxisLocationInfo axisLocationInfo, int pulseNum, VelocityCurveParams velocityCurveParams);
        #endregion

        #region 单轴绝对运动命令
        /// <summary>
        /// 命令指定机构以绝对坐标运行一段指定数值（单位为执行结构指定的单位）
        /// </summary>
        public virtual int MoveAbsValue(ExecuteBody executeBody, double val)
        {
            int pulseNum = (int)(val * executeBody.TransmissionParams.EquivalentPulse);
            return MoveAbsPulse(executeBody, pulseNum);
        }

        /// <summary>
        /// 命令指定机构以绝对坐标运行一段指定脉冲
        /// </summary>
        /// <param name="executeBody"></param>
        /// <param name="pulseNum">控制卡实际发送的脉冲数</param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public virtual int MoveAbsPulse(ExecuteBody executeBody, int pulseNum)
        {
            return MoveAbsPulse(executeBody.AxisLocationInfo, pulseNum, executeBody.VelocityCurveParams);
        }
        public abstract int MoveAbsPulse(AxisLocationInfo axisLocationInfo, int pulseNum, VelocityCurveParams velocityCurveParams);
        #endregion

        #region 单轴连续运动
        /// <summary>
        /// 命令指定机构连续运动
        /// </summary>
        /// <param name="executeBody"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public virtual int ContinuousMove(ExecuteBody executeBody, MoveDirection moveDirection)
        {
            return ContinuousMove(executeBody.AxisLocationInfo, moveDirection, executeBody.VelocityCurveParams);
        }
        public abstract int ContinuousMove(AxisLocationInfo axisLocationInfo, MoveDirection moveDirection, VelocityCurveParams velocityCurveParams);
        #endregion

        #region 单轴停止运动
        /// <summary>
        /// 立即停止
        /// </summary>
        /// <param name="executeBody"></param>
        /// <returns></returns>
        public virtual int ImmediateStop(ExecuteBody executeBody)
        {
            return ImmediateStop(executeBody.AxisLocationInfo);
        }
        public abstract int ImmediateStop(AxisLocationInfo axisLocationInfo);

        /// <summary>
        /// 减速停止指定机构轴脉冲输出
        /// </summary>
        /// <returns>正确：返回ERR_NoError； 错误：返回相关错误码</returns>
        public virtual int DecelStop(ExecuteBody executeBody)
        {
            return DecelStop(executeBody.AxisLocationInfo);
        }
        public abstract int DecelStop(AxisLocationInfo axisLocationInfo);
        #endregion

        #region 检查运动状态
        /// <summary>
        /// 检测指定机构的运动状态,0表示正常结束，-1表示异常结束，-2表示超时
        /// </summary>
        /// <param name="executeBody">运动机构</param>
        /// <returns>
        /// </returns>
        public virtual int CheckDone(ExecuteBody executeBody)
        {
            return CheckDone(executeBody.AxisLocationInfo, executeBody.VelocityCurveParams.Timeout);
        }
        public abstract int CheckDone(AxisLocationInfo axisLocationInfo, double timeoutLimit);
        #endregion

        #region 两轴做相对移动
        public virtual int MoveLine2RelPulse(ExecuteBody executeBody1, ExecuteBody executeBody2, double val1, double val2)
        {
            int pulseNum1 = (int)(val1 * executeBody1.TransmissionParams.EquivalentPulse);
            int pulseNum2 = (int)(val2 * executeBody2.TransmissionParams.EquivalentPulse);

            return MoveLine2RelPulse(executeBody1, executeBody2, pulseNum1, pulseNum2);
        }

        public virtual int MoveLine2RelPulse(ExecuteBody executeBody1, ExecuteBody executeBody2, int pulseNum1, int pulseNum2)
        {
            if (System.Math.Abs((decimal)(pulseNum1)) >= System.Math.Abs((decimal)(pulseNum2)))
            {
                return MoveLine2RelPulse(executeBody1.AxisLocationInfo, executeBody2.AxisLocationInfo, pulseNum1, pulseNum2, executeBody1.VelocityCurveParams);
            }
            else
            {
                return MoveLine2RelPulse(executeBody2.AxisLocationInfo, executeBody1.AxisLocationInfo, pulseNum2, pulseNum1, executeBody2.VelocityCurveParams);
            }
        }

        public abstract int MoveLine2RelPulse(AxisLocationInfo axisLocationInfo1, AxisLocationInfo axisLocationInfo2, int pulseNum1, int pulseNum2, VelocityCurveParams velocityCurveParams);
        #endregion

        #region 两轴做绝对移动
        public virtual int MoveLine2AbsPulse(ExecuteBody executeBody1, ExecuteBody executeBody2, double val1, double val2)
        {
            int pulseNum1 = (int)(val1 * executeBody1.TransmissionParams.EquivalentPulse);
            int pulseNum2 = (int)(val2 * executeBody2.TransmissionParams.EquivalentPulse);

            return MoveLine2AbsPulse(executeBody1, executeBody2, pulseNum1, pulseNum2);
        }

        public virtual int MoveLine2AbsPulse(ExecuteBody executeBody1, ExecuteBody executeBody2, int pulseNum1, int pulseNum2)
        {
            int[] dis = { (int)GetFeedbackPos(executeBody1.AxisLocationInfo) - pulseNum1, (int)GetFeedbackPos(executeBody1.AxisLocationInfo) - pulseNum2 };
            if (System.Math.Abs((decimal)(dis[0])) >= System.Math.Abs((decimal)(dis[1])))
            {
                return MoveLine2AbsPulse(executeBody1.AxisLocationInfo, executeBody2.AxisLocationInfo, pulseNum1, pulseNum2, executeBody1.VelocityCurveParams);
            }
            else
            {
                return MoveLine2AbsPulse(executeBody2.AxisLocationInfo, executeBody1.AxisLocationInfo, pulseNum2, pulseNum1, executeBody2.VelocityCurveParams);
            }
        }

        public abstract int MoveLine2AbsPulse(AxisLocationInfo axisLocationInfo1, AxisLocationInfo axisLocationInfo2, int pulseNum1, int pulseNum2, VelocityCurveParams velocityCurveParams);
        #endregion
        #endregion

        #region 回零
        #region 单轴回零
        /// <summary>
        /// 指定某个运动部分回零
        /// </summary>
        /// <returns></returns>
        public virtual int BackHome(ExecuteBody executeBody)
        {
            return BackHome(executeBody.AxisLocationInfo, executeBody.VelocityCurveParams);
        }
        public abstract int BackHome(AxisLocationInfo axisLocationInfo, VelocityCurveParams velocityCurveParams);
        #endregion

        #region 检查回零是否完成
        /// <summary>
        /// 检测指定机构的运动状态,0表示正常结束，-1表示异常结束，-2表示超时
        /// </summary>
        /// <param name="executeBody">运动机构</param>
        /// <returns>
        /// </returns>
        public virtual int CheckHomeDone(ExecuteBody executeBody)
        {
            return CheckHomeDone(executeBody.AxisLocationInfo, executeBody.VelocityCurveParams.Timeout);
        }
        public abstract int CheckHomeDone(AxisLocationInfo axisLocationInfo, double timeoutLimit);
        #endregion
        #endregion

        #region 配置
        #region 轴卡电平信号配置(板卡初始化后必须配置)
        public virtual int SetAxisSignalConfig(ExecuteBody executeBody)
        {
            return SetAxisSignalConfig(executeBody.AxisLocationInfo, executeBody.AxisSignalParams);
        }
        public abstract int SetAxisSignalConfig(AxisLocationInfo axisLocationInfo, AxisSignalParams axisSignalParams);
        #endregion

        #region 回零配置(回零前配置)
        public virtual int SetAxisHomeConfig(ExecuteBody executeBody)
        {
            return SetAxisHomeConfig(executeBody.AxisLocationInfo, executeBody.HomeConfigParams);
        }
        public abstract int SetAxisHomeConfig(AxisLocationInfo axisLocationInfo, HomeConfigParams homeConfigParams);
        #endregion

        #region 限位配置
        public virtual int ClearSoftConfig(ExecuteBody executeBody)
        {
            return ClearSoftConfig(executeBody.AxisLocationInfo);
        }
        public abstract int ClearSoftConfig(AxisLocationInfo axisLocationInfo); 
        public virtual int SetSoftELConfig(ExecuteBody executeBody)
        {
            return SetSoftELConfig(executeBody.AxisLocationInfo, executeBody.SoftLimitParams);
        }
        public abstract int SetSoftELConfig(AxisLocationInfo axisLocationInfo, SoftLimitParams softLimitParams);
        #endregion

        #region 配置输入口使能(部分板卡有此功能)
        /// <summary>
        /// 专用输入口使能(不是所有板卡都有此功能)
        /// </summary>
        /// <param name="cardNo">卡号</param>
        /// <param name="inputEn">专用信号输入口使能，需要将各信号按照二进制的位取值，然后转化为十进制,</param>
        /// <returns></returns>
        public abstract int InputEnable(int cardNo, int inputEn);
        #endregion

        #region 配置限位停止方式(部分板卡有此功能)
        /// <summary>
        /// 配置限位停止方式
        /// </summary>
        public virtual int ConfigELStopType(ExecuteBody executeBody, ELStopType ELStopType)
        {
            return ConfigELStopType(executeBody.AxisLocationInfo, ELStopType);
        }
        /// <summary>
        /// 配置限位停止方式
        /// </summary>
        /// <param name="axisLocationInfo"></param>
        /// <param name="ELStopType"></param>
        /// <returns>返回零无异常</returns>
        public abstract int ConfigELStopType(AxisLocationInfo axisLocationInfo, ELStopType ELStopType);
        #endregion
        #endregion

        #region 信息获取
        #region 脉冲计数器
        /// <summary>
        /// 读取指令位置计数器计数值
        /// </summary>
        /// <returns>指定轴当前指令位置计数器值，单位：Pulse</returns>
        public virtual double GetCommandPos(ExecuteBody executeBody)
        {
            return GetCommandPos(executeBody.AxisLocationInfo);
        }
        public abstract double GetCommandPos(AxisLocationInfo axisLocationInfo);

        /// <summary>
        /// 设置指令位置计数器计数值
        /// </summary>
        /// <param name="executeBody"></param>
        /// <param name="pos">设置指令位置计数器值</param>
        /// <returns>正确：返回0； 错误：返回相关错误码。</returns>
        public virtual int SetCommandPos(ExecuteBody executeBody, double pos)
        {
            return SetCommandPos(executeBody.AxisLocationInfo, pos);
        }
        public abstract int SetCommandPos(AxisLocationInfo axisLocationInfo, double pos);

        /// <summary>
        /// 读取反馈位置计数器计数值
        /// </summary>
        /// <returns>指定轴当前反馈位置计数器值，单位：Pulse</returns>
        public virtual double GetFeedbackPos(ExecuteBody executeBody)
        {
            return GetFeedbackPos(executeBody.AxisLocationInfo);
        }
        public abstract double GetFeedbackPos(AxisLocationInfo axisLocationInfo);

        /// <summary>
        /// 设置反馈位置计数器计数值
        /// </summary>
        /// <param name="executeBody"></param>
        /// <param name="pos">设置反馈位置计数器值</param>
        /// <returns>正确：返回0； 错误：返回相关错误码。</returns>
        public virtual int SetFeedbackPos(ExecuteBody executeBody, double pos)
        {
            return SetFeedbackPos(executeBody.AxisLocationInfo, pos);
        }
        public abstract int SetFeedbackPos(AxisLocationInfo axisLocationInfo, double pos);
        #endregion

        #region 轴IO信息获取
        /// <summary>
        /// 读取指定机构轴的专用接口信号状态，包括EL+、EL-、STP、STA、SD+、SD-等信号状态
        /// </summary>
        /// <returns>指定轴专用信号状态，需要将返回值转化为二进制</returns>
        public virtual AxisSignalState GetAxisState(ExecuteBody executeBody)
        {
            return GetAxisState(executeBody.AxisLocationInfo);
        }
        public abstract AxisSignalState GetAxisState(AxisLocationInfo axisLocationInfo);
        #endregion
        #endregion

        #region 获取错误码信息
        /// <summary>
        /// 根据错误码获取相关信息
        /// </summary>
        /// <param name="errNo"></param>
        /// <returns></returns>
        public abstract string GetErrMessage(int errNo);

        /// <summary>
        /// 获取源指定位的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="bitNo"></param>
        /// <returns></returns>
        public int GetBitValue(int source, int bitNo)
        {
            return (source >> bitNo) & 0x1;
        }

        /// <summary>
        /// 获取源指定位的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="bitNo"></param>
        /// <returns></returns>
        public uint GetBitValue(uint source, int bitNo)
        {
            return (source >> bitNo) & 0x1;
        }
        #endregion
    }
    #endregion
}
