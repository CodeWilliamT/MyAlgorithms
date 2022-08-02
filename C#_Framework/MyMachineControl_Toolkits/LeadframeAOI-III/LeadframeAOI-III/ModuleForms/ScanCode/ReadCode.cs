using System;
using System.Collections.Generic;
using HT_Lib;
using System.IO.Ports;

namespace LeadframeAOI
{
    class ReadQRCode
    {
        #region 成员变量
        private SerialPort scannerSP;
        private DateTime sendStartTime;
        /// <summary>
        /// 接收完成
        /// </summary>
        private Boolean m_readOver = false;
        public Boolean ReadOver
        {
            get { return m_readOver; }
            set { m_readOver = value; }
        }
        /// <summary>
        /// 定义接收缓冲区数据
        /// </summary>
        private List<byte> m_buffer = null;
        public List<byte> Buffer
        {
            get { return m_buffer; }
            set { m_buffer = value; }
        }

        #endregion
        #region 构造函数初始化串口
        public ReadQRCode(String COMName)
        {
            scannerSP = new SerialPort();
            scannerSP.PortName = COMName;
            scannerSP.Parity = Parity.None;
            //scannerSP.StopBits = StopBits.None;
            scannerSP.Handshake = Handshake.None;
            scannerSP.DataBits = 8;
            scannerSP.ReadBufferSize = 100;
            scannerSP.ReceivedBytesThreshold = 1; //设置 DataReceived 事件发生前内部输入缓冲区中的字节数
            scannerSP.WriteBufferSize = 2048;//writebuffersize忽略任何小于2018的值
            scannerSP.BaudRate = 115200;
            m_buffer = new List<byte>();
            scannerSP.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            scannerSP.Close();
            try
            {
                scannerSP.Open();
            }
            catch (Exception EXP)
            {
                HTLog.Error(EXP.Message);
            }
            if (scannerSP.IsOpen)
            {
                HTLog.Debug("读码器成功打开串口");
            }
            else
            {
                HTLog.Error("读码器打开串口失败");
            }
        }
        #endregion

        #region  发送指令
        public Boolean SendMes(byte[] data)
        {
            if (scannerSP.IsOpen)
            {
                scannerSP.Write(data, 0, data.Length);
                sendStartTime = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public Boolean GetQrCode()
        {
            if (!m_readOver)
                return false;
            m_readOver = false;
            return true;
        }
        #region  接收等待
        /// <summary>
        /// 发送完指令到接收到之间的时间
        /// </summary>
        /// <returns></returns>
        public Boolean ScannerWaitTime(int m_waitTimeout)//完成true 不完成false
        {
            while (!m_readOver)
            {
                if (DateTime.Now.Subtract(sendStartTime).TotalMilliseconds >= m_waitTimeout)
                {
                    m_readOver = false;
                    return false;
                }
            }
            m_readOver = false;
            return true;
        }
        #endregion
        #region 关闭串口
        public void Stop()
        {
            scannerSP.Close();
            m_buffer.Clear();
            GC.Collect();//能就是强制对所有代进行垃圾回收
        }
        #endregion
        #region  接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SerialPort_DataReceived(object sender, EventArgs e)
        {
            m_readOver = false;
            int availCount = scannerSP.BytesToRead;
            m_buffer.Clear();
            for (int i = 0; i < availCount; i++)
            {
                //单个字节读取
                m_buffer.Add((byte)scannerSP.ReadByte());
            }
            m_readOver = true;
        }
        #endregion
    }
}
