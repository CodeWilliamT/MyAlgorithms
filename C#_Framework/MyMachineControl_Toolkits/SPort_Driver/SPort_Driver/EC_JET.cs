using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPort_Driver
{
    /// <summary>
    /// 广州易达包装设备喷码器，型号EC-2000系列
    /// </summary>
    public class EC_JET: SPortClass
    {
        private System.IO.Ports.SerialPort sPort;
        /// <summary> 头 </summary>
        byte[] head = { 0X7E};
        /// <summary> 尾</summary>
        byte[] tail = { 0X7F };
        public EC_JET(string ComName)
        {
            this.sPort = new System.IO.Ports.SerialPort(ComName, 38400, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
        }
        public void TriggerPrint()
        {
            if (!sPort.IsOpen)
            {
                sPort.Open();
            }

            /// <summary> 命令字俩字节 </summary>
            byte[] order = { 0x1A,0x00};
            /// <summary> 数据字 </summary>
            byte[] num = { };
            SendCmd(order, num);
        }
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
            /// <summary> 用于校验的字(不包含数据位) </summary>
            byte[] OrginCmd = { 0x00, order[0], order[1],0x0C,0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            /// <summary> 用于校验的字(不包含数据位) </summary>
            byte[] MainCmd = OrginCmd.Concat(num).ToArray();
            /// <summary> 校验后的字 </summary>
            byte[] checkCode = CRC16_X25(MainCmd);
            byte[] Cmd = head.Concat(MainCmd).Concat(checkCode).Concat(tail).ToArray();
            if (sPort.IsOpen) sPort.Write(Cmd, 0, Cmd.Length);
            int a = sPort.ReadByte();
            System.Threading.Thread.Sleep(10);
        }

        #endregion
    }
}
