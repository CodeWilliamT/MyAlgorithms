using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ToolKits.HTNET
{
    /// <summary>
    /// HTNet库函数封装
    /// </summary>
    public class HTNet
    {
        #region HTNetLight常量
        /*Node types*/
        public const UInt16 NODE_TYPE_LIGHT_SRC = 2;  //HTNET-I-LightSrcDrv module

        /*Output channel para group*/
        public const UInt16 LIGHT_SRC_OUTPUT_CHANNEL0 = 0; //Channel 0
        public const UInt16 LIGHT_SRC_OUTPUT_CHANNEL1 = 1; //Channel 1
        public const UInt16 LIGHT_SRC_OUTPUT_CHANNEL2 = 2; //Channel 2
        public const UInt16 LIGHT_SRC_OUTPUT_CHANNEL3 = 3; //Channel 3
        public const UInt16 LIGHT_SRC_OUTPUT_CHANNEL_CNT = 4; //Channel 4

        /*Trigger source para group*/
        public const UInt16 LIGHT_SRC_TRIGGER_SOURCE0 = 1;//Source 0
        public const UInt16 LIGHT_SRC_TRIGGER_SOURCE1 = 2;//Source 1
        public const UInt16 LIGHT_SRC_TRIGGER_SOURCE2 = 4;//Source 2
        public const UInt16 LIGHT_SRC_TRIGGER_SOURCE3 = 8;//Source 3

        /*Output pulse width para group*/
        public const double LIGHT_SRC_OUTPUT_PW_MIN = 0.1;      //0.1us
        public const double LIGHT_SRC_OUTPUT_PW_MAX = 20000.0;   //20ms

        //private const string DLLPATH = "//kernel32"; // "kernel32";
        #endregion

        #region HTNet Light结构体定义
        /// <summary>
        /// 结构体示例
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COMMTIMEOUTS
        {
            public int ReadIntervalTimeout;
            public int ReadTotalTimeoutMultiplier;
            public int ReadTotalTimeoutConstant;
            public int WriteTotalTimeoutMultiplier;
            public int WriteTotalTimeoutConstant;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct triggerPt
        {
            public Int32 pos;
            public UInt16 dir;
        }
        #endregion

        #region 外部函数调用申明
        /*
        /// <summary>
        /// 设置串口超时时间
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpCommTimeouts">超时时间</param>
        [DllImport("kernel32.dll")]
        public static extern bool SetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);
         */
        /* Card operations */
        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_card_open", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_card_open(ref UInt16 pciCardCnt, ref UInt16 pciCardInBits, ref UInt16 usbCardCnt, ref UInt16 usbCardInBits);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_card_close", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_card_close();

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_pci_get_first_bus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_pci_get_first_bus(UInt16 cardID, ref UInt16 startBus);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_usb_get_first_bus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_usb_get_first_bus(UInt16 cardID, ref UInt16 startBus);

        /* USB operations */
        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_bus_scan", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_bus_scan(UInt16 busID, ref UInt32 nodeTableInBits);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_start_ring", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_start_ring(UInt16 busID);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_stop_ring", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_stop_ring(UInt16 busID);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_get_node_type", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_get_node_type(UInt16 busID, UInt16 nodeAddr, ref UInt16 nodeType);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_get_node_status", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_get_node_status(UInt16 busID, UInt16 nodeAddr, ref Int32 nodeStatus);

        /* IO operations */
        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_write_do", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_write_do(UInt16 busID, UInt16 nodeAddr, UInt32 dwData);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_read_do", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_read_do(UInt16 busID, UInt16 nodeAddr, ref UInt32 dwData);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_set_do", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_set_do(UInt16 busID, UInt16 nodeAddr, UInt32 dwChannel);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_clear_do", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_clear_do(UInt16 busID, UInt16 nodeAddr, UInt32 dwChannel);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_read_di", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_read_di(UInt16 busID, UInt16 nodeAddr, ref UInt32 dwData);

        /* Memory operations - CAUTION: DO NOT USE DIRECTLY! */
        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_write_memory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_write_memory(UInt16 busID, UInt16 nodeAddr, UInt16 memAddr, UInt16 wData);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_read_memory", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_read_memory(UInt16 busID, UInt16 nodeAddr, UInt16 memAddr, ref UInt16 wData);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_write_block", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_write_block(UInt16 busID, UInt16 nodeAddr, UInt16 memAddr, UInt16 wCnt, ref UInt16 wData);

        [DllImport("htnet_i_lib.dll", EntryPoint = "HTNET_I_read_block", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_read_block(UInt16 busID, UInt16 nodeAddr, UInt16 memAddr, UInt16 wCnt, ref UInt16 wData);

        /* Light Config trigger source */
        [DllImport("ht_light_src.dll", EntryPoint = "HTNET_I_light_source_set_trigger_source", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_light_source_set_trigger_source(UInt16 busID, UInt16 nodeAddr, UInt16 channel, UInt16 src);

        /* Config output pulse width */
        [DllImport("ht_light_src.dll", EntryPoint = "HTNET_I_light_source_set_pulse_width", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_light_source_set_pulse_width(UInt16 busID, UInt16 nodeAddr, UInt16 channel, Double pw);

        /* Software trigger */
        [DllImport("ht_light_src.dll", EntryPoint = "HTNET_I_light_source_sw_trigger", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_light_source_sw_trigger(UInt16 busID, UInt16 nodeAddr, UInt16 channel);

        /*pos_trigger*/
        [DllImport("ht_pos_trigger.dll", EntryPoint = "HTNET_I_pos_trigger_config", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_pos_trigger_config(UInt16 busID, UInt16 nodeAddr, UInt16 cntReverse, Double forwardTime);

        [DllImport("ht_pos_trigger.dll", EntryPoint = "HTNET_I_pos_trigger_get_current_pos", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_pos_trigger_get_current_pos(UInt16 busID, UInt16 nodeAddr, ref Int32 pos);

        [DllImport("ht_pos_trigger.dll", EntryPoint = "HTNET_I_pos_trigger_set_current_pos", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_pos_trigger_set_current_pos(UInt16 busID, UInt16 nodeAddr, Int32 pos);

        [DllImport("ht_pos_trigger.dll", EntryPoint = "HTNET_I_pos_trigger_set_point_table", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_pos_trigger_set_point_table(UInt16 busID, UInt16 nodeAddr, ref triggerPt[] pt, UInt16 ptCnt);

        [DllImport("ht_pos_trigger.dll", EntryPoint = "HTNET_I_pos_trigger_sw_trigger", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_pos_trigger_sw_trigger(UInt16 busID, UInt16 nodeAddr);

        /******IO*************/
        [DllImport("ht_io.dll", EntryPoint = "HTNET_I_dio_set_direction", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_dio_set_direction(UInt16 busID, UInt32 dir);
        
        [DllImport("ht_io.dll", EntryPoint = "HTNET_I_dio_get_direction", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 HTNET_I_dio_get_direction(UInt16 busID, ref UInt32 dir);

        #endregion
    }
}
