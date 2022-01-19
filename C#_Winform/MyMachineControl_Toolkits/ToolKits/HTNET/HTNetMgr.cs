using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKits.HTNET
{
    /// <summary>
    /// 光源信息
    /// </summary>
    public class HTLightInfo
    {
        private bool _open;
        private UInt16 _busID;
        private UInt16 _nodeAddr;
        private UInt16 _channel;
        private UInt16 _src;


        public bool open
        {
            get
            {
                return this._open;
            }
            set
            {
                this._open = value;
            }
        }

        public UInt16 busID
        {
            get
            {
                return this._busID;
            }
            set
            {
                this._busID = value;
            }
        }

        public UInt16 nodeAddr
        {
            get
            {
                return this._nodeAddr;
            }
            set
            {
                this._nodeAddr = value;
            }
        }

        public UInt16 channel
        {
            get
            {
                return this._channel;
            }
            set
            {
                this._channel = value;
            }
        }

        public UInt16 src
        {
            get
            {
                return this._src;
            }
            set
            {
                this._src = value;
            }
        }

    }
    public class HTPosInfo
    {
        private bool _open;
        private UInt16 _busID;
        private UInt16 _nodeAddr;
        private UInt16 _cntReverse;
        private UInt16 _forwardTime;

        public bool open
        {
            get
            {
                return this._open;
            }
            set
            {
                this._open = value;
            }
        }
        public UInt16 busID
        {
            get
            {
                return this._busID;
            }
            set
            {
                this._busID = value;
            }
        }
        public UInt16 nodeAddr
        {
            get
            {
                return this._nodeAddr;
            }
            set
            {
                this._nodeAddr = value;
            }
        }
        public UInt16 cntReverse
        {
            get { return this._cntReverse; }
            set { this._cntReverse = value; }
        }
        public UInt16 forwarTime
        {
            get { return this._forwardTime; }
            set { this._forwardTime = value; }
        }
    }
    public class HTIOInfo
    {
        private bool _open;
        private UInt16 _busID;
        private UInt16 _nodeAddr;

        public bool open
        {
            get
            {
                return this._open;
            }
            set
            {
                this._open = value;
            }
        }
        public UInt16 busID
        {
            get
            {
                return this._busID;
            }
            set
            {
                this._busID = value;
            }
        }
        public UInt16 nodeAddr
        {
            get
            {
                return this._nodeAddr;
            }
            set
            {
                this._nodeAddr = value;
            }
        }
    }

    /// <summary>
    /// 板卡信息
    /// </summary>
    public class HTCardInfo
    {
        public bool init;
        public int maxLight;
        public int maxPosTrigger;
        public int maxIO;
        public List<HTLightInfo> lightSet = new List<HTLightInfo>();
        public List<HTPosInfo> posSet = new List<HTPosInfo>();
        public List<HTIOInfo> ioSet = new List<HTIOInfo>();
    }

    /// <summary>
    /// HTNet板卡管理类
    /// </summary>
    public class HTNetMgr
    {
        #region 字段
        static HTCardInfo s_cardInfo = null;

        /// <summary>
        /// 定义板卡变量
        /// </summary>
        private static UInt16 s_pcicardCnt = 0;
        private static UInt16 s_pcicardBit = 0;
        private static UInt16 s_usbcardCnt = 0;
        private static UInt16 s_usbcardBit = 0;
        #endregion

        #region 构造器
        /// <summary>
        /// 初始化HTNET管理类
        /// </summary>
        /// <param name="lightNum">最大光源数量</param>
        /// <param name="posTrigNum">最大位置触发数量</param>
        public HTNetMgr(int lightNum,int posTrigNum,int ioNum)
        {
            if (s_cardInfo == null)
            {
                s_cardInfo = new HTCardInfo();
                s_cardInfo.init = false;
                s_cardInfo.maxLight = lightNum;
                s_cardInfo.maxPosTrigger = posTrigNum;
                s_cardInfo.maxIO = ioNum;
                s_cardInfo.lightSet.Clear();
            }
        }
        #endregion

        #region 板卡操作部分
        /// <summary>
        /// 初始化板卡
        /// </summary>
        /// <returns></returns>
        public static bool card_open()
        {
            if (s_cardInfo.init == false)
            {
                UInt16 busNo = 0;
                UInt32 nodeBit = 0;

                //加载板卡
                if (HTNet.HTNET_I_card_open(ref s_pcicardCnt, ref s_pcicardBit, ref s_usbcardCnt, ref s_usbcardBit) < 0)
                {
                    return false;
                }

                for (UInt16 i = 0; i < 16; i++)
                {
                    //pci板卡
                    if (Convert.ToBoolean(s_pcicardBit & (1 << i)))
                    {
                        busNo = (ushort)(i * 4);
                        for (UInt16 j = 0; j < 4; j++, busNo++)
                        {
                            //扫描总线
                            HTNet.HTNET_I_bus_scan(busNo, ref nodeBit);
                            if (nodeBit != 0)
                            {
                                //开始通信
                                HTNet.HTNET_I_start_ring(busNo);
                            }
                        }
                    }

                    //usb板卡
                    if (Convert.ToBoolean(s_usbcardBit & (1 << i)))
                    {
                        busNo = (ushort)(64 + i * 4);
                        for (UInt16 j = 0; j < 4; j++, busNo++)
                        {
                            //扫描总线
                            HTNet.HTNET_I_bus_scan(busNo, ref nodeBit);
                            if (nodeBit != 0)
                            {
                                //开始通信
                                HTNet.HTNET_I_start_ring(busNo);
                            }
                        }
                    }
                }

                s_cardInfo.init = true;
            }

            return true;
        }

        /// <summary>
        /// 关闭板卡
        /// </summary>
        /// <returns></returns>
        public static bool card_close()
        {
            if (s_cardInfo.init == true)
            {
                UInt16 busNo = 0;

                for (UInt16 i = 0; i < 16; i++)
                {
                    if (Convert.ToBoolean(s_pcicardBit & (1 << i)))
                    {
                        busNo = (ushort)(i * 4);
                        for (UInt16 j = 0; j < 4; j++, busNo++)
                        {
                            HTNet.HTNET_I_stop_ring(busNo);
                        }
                    }

                    if (Convert.ToBoolean(s_usbcardBit & (1 << i)))
                    {
                        busNo = (ushort)(64 + i * 4);
                        for (int j = 0; j < 4; j++, busNo++)
                        {
                            HTNet.HTNET_I_stop_ring(busNo);
                        }
                    }
                }

                Int32 err = HTNet.HTNET_I_card_close();

                s_cardInfo.lightSet.Clear();
                s_cardInfo.posSet.Clear();
                s_cardInfo.init = false;
            }

            return true;
        }
        #endregion

        #region 光源操作部分
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busID">板卡总线号</param>
        /// <param name="nodeAddr">光源板站号</param>
        /// <param name="channel">光源通道</param>
        /// <param name="src">光源触发源(2/4/8/16)</param>
        /// <returns></returns>
        public bool light_init(UInt16 busID, UInt16 nodeAddr, UInt16 channel, UInt16 src)
        {
            if (s_cardInfo.lightSet.Count >=s_cardInfo.maxLight)
                s_cardInfo.lightSet.Clear();

            HTLightInfo tmpLight = new HTLightInfo();
            tmpLight.open = false;
            tmpLight.busID = busID;
            tmpLight.nodeAddr = nodeAddr;
            tmpLight.channel = channel;
            tmpLight.src = src;

            s_cardInfo.lightSet.Add(tmpLight);

            return true;
        }
        /// <summary>
        /// 打开光源
        /// </summary>
        /// <returns></returns>
        public bool light_open(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if (s_cardInfo.lightSet[id].open == false)
            {
                if (HTNet.HTNET_I_light_source_set_trigger_source(s_cardInfo.lightSet[id].busID, s_cardInfo.lightSet[id].nodeAddr, s_cardInfo.lightSet[id].channel, s_cardInfo.lightSet[id].src) < 0)
                {
                    sb.Append("[HTNET_I_light_source_set_trigger_source]调用失败!");
                    strMsg = sb.ToString();
                    return false;
                }

                s_cardInfo.lightSet[id].open = true;
            }

            return true;
        }

        /// <summary>
        /// 关闭光源
        /// </summary>
        /// <returns></returns>
        public bool light_close(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if (s_cardInfo.lightSet[id].open == true)
            {
                if (HTNet.HTNET_I_light_source_set_trigger_source(s_cardInfo.lightSet[id].busID, s_cardInfo.lightSet[id].nodeAddr, s_cardInfo.lightSet[id].channel, 0) < 0)
                {
                    sb.Append("[HTNET_I_light_source_set_trigger_source]调用失败!");
                    strMsg = sb.ToString();
                    return false;
                }

                s_cardInfo.lightSet[id].open = false;
            }

            return true;
        }

        /// <summary>
        /// 设置触发时间
        /// </summary>
        /// <param name="triggerTime"></param>
        /// <returns></returns>
        public bool light_setTriggerTime(int id, double triggerTime, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if ((triggerTime < HTNet.LIGHT_SRC_OUTPUT_PW_MIN) || (triggerTime > HTNet.LIGHT_SRC_OUTPUT_PW_MAX))
            {
                sb.Append("设置范围超限，触发时间不能超过[" + HTNet.LIGHT_SRC_OUTPUT_PW_MIN + "," + HTNet.LIGHT_SRC_OUTPUT_PW_MAX + "]!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_light_source_set_pulse_width(s_cardInfo.lightSet[id].busID, s_cardInfo.lightSet[id].nodeAddr, s_cardInfo.lightSet[id].channel, triggerTime) < 0)
            {
                sb.Append("[HTNET_I_light_source_set_pulse_width]调用失败!");
                strMsg = sb.ToString();
                return false;
            }

            return true;
        }
        /// <summary>
        /// 点亮光源一次
        /// </summary>
        /// <returns></returns>
        public bool light_trigger(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.lightSet[id].open == false)
            {
                sb.Append("光源未打开, 不能触发!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_light_source_sw_trigger(s_cardInfo.lightSet[id].busID, s_cardInfo.lightSet[id].nodeAddr, s_cardInfo.lightSet[id].channel) < 0)
            {
                sb.Append("[HTNET_I_light_source_sw_trigger]调用失败!");
                strMsg = sb.ToString();
                return false;
            }

            return true;
        }
        #endregion

        #region 位置触发操作部分
        public bool pos_init(UInt16 busID, UInt16 nodeAddr, UInt16 cntReverse, UInt16 forwardTime)
        {
            if (s_cardInfo.posSet.Count >= s_cardInfo.maxPosTrigger)
                s_cardInfo.posSet.Clear();

            HTPosInfo _htPosInfo = new HTPosInfo();
            _htPosInfo.open = false;
            _htPosInfo.busID = busID;
            _htPosInfo.nodeAddr = nodeAddr;
            _htPosInfo.cntReverse = cntReverse;
            _htPosInfo.forwarTime = forwardTime;

            s_cardInfo.posSet.Add(_htPosInfo);

            return true;
        }
        public bool pos_open(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if (s_cardInfo.posSet[id].open == false)
            {
                if (HTNet.HTNET_I_pos_trigger_config(s_cardInfo.posSet[id].busID, s_cardInfo.posSet[id].nodeAddr, s_cardInfo.posSet[id].cntReverse, s_cardInfo.posSet[id].forwarTime) < 0)
                {
                    sb.Append("[HTNET_I_pos_trigger_config]调用失败!");
                    strMsg = sb.ToString();
                    return false;
                }

                s_cardInfo.posSet[id].open = true;
            }

            return true;
        }
        public bool pos_close(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if (s_cardInfo.posSet[id].open == true)
            {
                s_cardInfo.posSet[id].open = false;
            }

            return true;
        }
        public bool pos_get_current_pos(int id, ref int pos, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.posSet[id].open == false)
            {
                sb.Append("位置触发板未打开, 不能获取当前位置!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_pos_trigger_get_current_pos(s_cardInfo.posSet[id].busID, s_cardInfo.posSet[id].nodeAddr, ref pos) < 0)
            {
                sb.Append("[HTNET_I_pos_trigger_get_current_pos]调用失败!");
                strMsg = sb.ToString();
                return false;
            }
            return true;
        }
        public bool pos_set_current_pos(int id, int pos, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.posSet[id].open == false)
            {
                sb.Append("位置触发板未打开, 不能设置指定位置!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_pos_trigger_set_current_pos(s_cardInfo.posSet[id].busID, s_cardInfo.posSet[id].nodeAddr, pos) < 0)
            {
                sb.Append("[HTNET_I_pos_trigger_set_current_pos]调用失败!");
                strMsg = sb.ToString();
                return false;
            }
            return true;
        }
        public bool pos_set_point_table(int id, ref HTNet.triggerPt[] pt, UInt16 ptCnt, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.posSet[id].open == false)
            {
                sb.Append("位置触发板未打开, 不能设置指定触发点位!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_pos_trigger_set_point_table(s_cardInfo.posSet[id].busID, s_cardInfo.posSet[id].nodeAddr, ref pt,ptCnt) < 0)
            {
                sb.Append("[HTNET_I_pos_trigger_set_point_table]调用失败!");
                strMsg = sb.ToString();
                return false;
            }
            return true;
        }
        public bool pos_sw_trigger(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.posSet[id].open == false)
            {
                sb.Append("位置触发板未打开, 不能软件触发!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_pos_trigger_sw_trigger(s_cardInfo.posSet[id].busID, s_cardInfo.posSet[id].nodeAddr) < 0)
            {
                sb.Append("[HTNET_I_pos_trigger_sw_trigger]调用失败!");
                strMsg = sb.ToString();
                return false;
            }
            return true;
        }
        #endregion

        #region IO操作部分
        public bool io_init(UInt16 busID, UInt16 nodeAddr)
        {
            if (s_cardInfo.ioSet.Count >= s_cardInfo.maxIO)
                s_cardInfo.ioSet.Clear();

            HTIOInfo _htIOInfo = new HTIOInfo();
            _htIOInfo.open = false;
            _htIOInfo.busID = busID;
            _htIOInfo.nodeAddr = nodeAddr;

            s_cardInfo.ioSet.Add(_htIOInfo);

            return true;
        }
        public bool io_open(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if (s_cardInfo.ioSet[id].open == false)
            {
                s_cardInfo.ioSet[id].open = true;
            }

            return true;
        }
        public bool io_close(int id, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();

            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }

            if (s_cardInfo.ioSet[id].open == true)
            {
                s_cardInfo.ioSet[id].open = false;
            }

            return true;
        }
        public bool io_get_dir(int id,ref UInt32 dir,ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.ioSet[id].open == false)
            {
                sb.Append("IO板未打开, 不能获取信号!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_dio_get_direction(s_cardInfo.ioSet[id].busID, ref dir) < 0)
            {
                sb.Append("[HTNET_I_dio_get_direction]调用失败!");
                strMsg = sb.ToString();
                return false;
            }
            return true;
        }
        public bool io_set_dir(int id, UInt32 dir, ref string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            if (s_cardInfo.init == false)
            {
                if (card_open() == false)
                {
                    sb.Append("HTNet板卡打开失败!");
                    strMsg = sb.ToString();
                    return false;
                }
            }
            if (s_cardInfo.ioSet[id].open == false)
            {
                sb.Append("IO板未打开, 不能设置信号!");
                strMsg = sb.ToString();
                return false;
            }
            if (HTNet.HTNET_I_dio_set_direction(s_cardInfo.ioSet[id].busID, dir) < 0)
            {
                sb.Append("[HTNET_I_dio_set_direction]调用失败!");
                strMsg = sb.ToString();
                return false;
            }
            return true;
        }

        #endregion
    }
}
