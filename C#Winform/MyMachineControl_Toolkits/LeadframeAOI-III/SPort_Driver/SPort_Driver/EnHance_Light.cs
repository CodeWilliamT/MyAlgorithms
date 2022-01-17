using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPort_Driver
{
    /// <summary>
    /// 增亮频闪光源控制器
    /// </summary>
    public class EnHance_Light : SPortClass
    {
        private System.IO.Ports.SerialPort sPort;
        /// <summary> 头,长度,设备型号，设备编号 </summary>
        byte[] head= {0X64,6,0X10,0X01};
        /// <summary> 尾</summary>
        byte[] tail= {0X84};
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComName">串口号</param>
        public EnHance_Light(string ComName)
        {
            this.sPort = new System.IO.Ports.SerialPort(ComName, 19200, System.IO.Ports.Parity.None, 8,System.IO.Ports.StopBits.One);
        }
        /// <summary>
        /// 更改全通道触发时长
        /// </summary>
        /// <param name="time">触发时长(μS)</param>
        public void ChangeTriggerTime(int time)
        {
            if(!sPort.IsOpen)
            {
                sPort.Open();
            }
            if (time < 0) time = 0;
            if (time > 1000) time = 1000;

            /// <summary> 命令字,通道字 </summary>
            byte[] order= { 0x16, 0x01};
            /// <summary> 数据字 </summary>
            byte[] num = { (byte)(time >> 8), (byte)(time & 0XFF) };
            SendCmd(order, num);
        }
        /// <summary>
        /// 更改某通道触发时长
        /// </summary>
        /// <param name="channel">通道号</param>
        /// <param name="time">触发时长(μS)</param>
        public void ChangeTriggerTime(int channel,int time)
        {
            if (!sPort.IsOpen)
            {
                sPort.Open();
            }
            if (time < 0) time = 0;
            if (time > 1000) time = 1000;
            if (channel < 1) return;
            /// <summary> 命令字,通道字 </summary>
            byte[] order = { 0x16, (byte)channel };
            /// <summary> 数据字 </summary>
            byte[] num = { (byte)(time >> 8), (byte)(time & 0XFF) };
            SendCmd(order, num);
        }
        /// <summary>
        /// 通道1使能
        /// </summary>
        /// <param name="flag"></param>
        public void Enable(bool flag)
        {
            if (!sPort.IsOpen)
            {
                sPort.Open();
            }

            /// <summary> 命令字,通道字 </summary>
            byte[] order = { 0x01, 0x01 };
            /// <summary> 数据字 </summary>
            byte[] num = { 0, (byte)(flag?0x01:0x00)};
            SendCmd(order, num);
        }

        /// <summary>
        /// 某通道使能
        /// </summary>
        /// <param name="channel">通道号</param>
        /// <param name="flag"></param>
        public void Enable(int channel, bool flag)
        {
            if (!sPort.IsOpen)
            {
                sPort.Open();
            }
            if (channel < 1) return;
            /// <summary> 命令字,通道字 </summary>
            byte[] order = { 0x01, (byte)channel };
            /// <summary> 数据字 </summary>
            byte[] num = { 0, (byte)(flag ? 0x01 : 0x00) };
            SendCmd(order, num);
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (sPort == null) return;
            if (sPort.IsOpen)
            {
                sPort.Close();
            }
        }
        #region 引用函数
        /// <summary>
        /// 发送控制指令
        /// </summary>
        /// <param name="order">命令位+通道位</param>
        /// <param name="num">数据位</param>
        public void SendCmd(byte[] order, byte[] num)
        {
            /// <summary> 用于校验的字 </summary>
            byte[] MainCmd = { head[1], head[2], head[3], order[0], order[1], num[0], num[1] }; ;
            /// <summary> 校验后的字 </summary>
            byte[] checkCode = CRC16_MODBUS(MainCmd);
            byte[] Cmd = { head[0], head[1], head[2], head[3], order[0], order[1], num[0], num[1], checkCode[0], checkCode[1], tail[0] };
            //100 6 16 1 21 1 0 100 119 22 132
            //64 06 10 01 16 01 00 64 16 33 84
            if (sPort.IsOpen) sPort.Write(Cmd, 0, 11);
            int a = sPort.ReadByte();
            System.Threading.Thread.Sleep(10);
        }
        #endregion
    }
}
