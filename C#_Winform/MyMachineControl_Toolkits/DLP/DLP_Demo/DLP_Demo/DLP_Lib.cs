/****************************************************************************/
/*  File Name   :   DLP_Lib.cs                                          */
/*  Brief       :   Motion APIs(x86/x64)                                    */
/*  Verion      :   1.0                                                    */
/*  Date        :   2017/11/01                                              */
/*  Author      :   Tongqing CHEN	                                        */
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLP_Lib
{
    #region  DLP API接口，原生C语言接口
    public class DLP_API
    {
        private const String _Dll = "DLP_Lib.dll";
        private const CallingConvention _Cvt = CallingConvention.Cdecl;
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_OpenByPath(Int32 id, String path);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_IsConnected(Int32 id);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_Close(Int32 id);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_SetMode(Int32 id, Byte mode);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_GetMode(Int32 id, ref Byte mode);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_SetLedCurrents(Int32 id, Byte RedCurrent, Byte GreenCurrent, Byte BlueCurrent);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_GetLedCurrents(Int32 id, ref Byte RedCurrent, ref Byte GreenCurrent, ref Byte BlueCurrent);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_ClearPatLut(Int32 id);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_AddToPatLut(Int32 id, Int32 TrigType, Int32 PatNum, Int32 BitDepth, Int32 LEDSelect, Byte InvertPat, Byte InsertBlack, Byte BufSwap, Byte trigOutPrev);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_SetPatternConfig(Int32 id, UInt32 numLutEntries, Byte repeat, UInt32 numPatsForTrigOut2, UInt32 numImages);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_GetPatternConfig(Int32 id, ref UInt32 pNumLutEntries, ref Byte pRepeat, ref UInt32 pNumPatsForTrigOut2, ref UInt32 pNumImages);

        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_SendPatLut(Int32 id);

        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_SendImageLut(Int32 id, Byte[] lutEntries, UInt32 numEntries);

        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_SetExposure_FramePeriod(Int32 id, UInt32 exposurePeriod, UInt32 framePeriod);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_GetExposure_FramePeriod(Int32 id, ref UInt32 pExposure, ref UInt32 pFramePeriod);

        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_ValidatePatLutData(Int32 id, ref UInt32 pStatus);

        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_PatternDisplay(Int32 id, UInt32 Action);
        [DllImport(_Dll, CallingConvention = _Cvt)]
        public static extern Int32 DLP_GetPatternDisplay(Int32 id, ref UInt32 pAction);
    }
    #endregion

    #region  C#DLP API定制接口
    public enum LedColorEnum
    {
        NoLED = 0, Red=1, Green=2, Yellow=3, Blue=4, Magenta=5, Cyan=6, White=7
    }
    public enum DisplayAction
    {
        Stop = 0, Pause = 1, Start = 2
    }
    public struct PatternSequenceStruct
    {
        public LedColorEnum ledColor;
        public Byte imageIdx;
        public Int32 patNum;
        public Int32 bitDepth;
    }
    public struct PatternConfigStruct
    {
        public Byte currentRed;   // 红色LED亮度（0-255）
        public Byte currentGreen;// 绿色LED亮度（0-255）
        public Byte currentBlue;// 蓝色LED亮度（0-255）
        public Boolean repeat;
        public UInt32 exposurePeriod;
        public UInt32 framePeriod;
    }
    public class DLP
    {
        public static Int32 OpenByPath(Int32 id, String path) { return DLP_API.DLP_OpenByPath(id, path); }
        public static Boolean IsConnected(Int32 id) { return (DLP_API.DLP_IsConnected(id) == 1); }
        public static Int32 Close(Int32 id) { return DLP_API.DLP_Close(id); }
        public static Int32 SetMode(Int32 id, Byte mode) { return DLP_API.DLP_SetMode(id, mode); }
        public static Int32 GetMode(Int32 id, ref Byte mode) { return DLP_API.DLP_GetMode(id, ref mode); }
        public static Int32 SetLedCurrents(Int32 id, Byte RedCurrent, Byte GreenCurrent, Byte BlueCurrent) { return DLP_API.DLP_SetLedCurrents(id, RedCurrent, GreenCurrent, BlueCurrent); }
        public static Int32 SetLedCurrents(Int32 id, ref Byte RedCurrent, ref Byte GreenCurrent, ref Byte BlueCurrent) { return DLP_API.DLP_GetLedCurrents(id, ref RedCurrent, ref GreenCurrent, ref BlueCurrent); }
        public static Int32 ClearPatLut(Int32 id) { return DLP_API.DLP_ClearPatLut(id); }
        public static Int32 AddToPatLut(Int32 id, Int32 TrigType, Int32 PatNum, Int32 BitDepth, Int32 LEDSelect, Boolean InvertPat, Boolean InsertBlack, Boolean BufSwap, Boolean trigOutPrev) { return DLP_API.DLP_AddToPatLut(id, TrigType, PatNum, BitDepth, LEDSelect, Convert.ToByte(InvertPat), Convert.ToByte(InsertBlack), Convert.ToByte(BufSwap), Convert.ToByte(trigOutPrev)); }
        public static Int32 SetPatternConfig(Int32 id, UInt32 numLutEntries, Byte repeat, UInt32 numPatsForTrigOut2, UInt32 numImages) { return DLP_API.DLP_SetPatternConfig(id, numLutEntries, repeat, numPatsForTrigOut2, numImages); }
        public static Int32 GetPatternConfig(Int32 id, ref UInt32 pNumLutEntries, ref Byte pRepeat, ref UInt32 pNumPatsForTrigOut2, ref UInt32 pNumImages) { return DLP_API.DLP_GetPatternConfig(id, ref pNumLutEntries, ref pRepeat, ref pNumPatsForTrigOut2, ref pNumImages); }
        public static Int32 SendPatLut(Int32 id) { return DLP_API.DLP_SendPatLut(id); }
        public static Int32 SendImageLut(Int32 id, Byte[] lutEntries, UInt32 numEntries) { return DLP_API.DLP_SendImageLut(id, lutEntries, numEntries); }
        public static Int32 SetExposure_FramePeriod(Int32 id, UInt32 exposurePeriod, UInt32 framePeriod) { return DLP_API.DLP_SetExposure_FramePeriod(id, exposurePeriod, framePeriod); }
        public static Int32 GetExposure_FramePeriod(Int32 id, ref UInt32 pExposurePeriod, ref UInt32 pFramePeriod) { return DLP_API.DLP_GetExposure_FramePeriod(id, ref pExposurePeriod, ref pFramePeriod); }
        public static Int32 ValidatePatLutData(Int32 id, ref UInt32 pStatus) { return DLP_API.DLP_ValidatePatLutData(id, ref pStatus); }
        public static Int32 PatternDisplay(Int32 id, DisplayAction Action) { return DLP_API.DLP_PatternDisplay(id, (UInt32)Action); }
        public static Int32 GetPatternDisplay(Int32 id, ref UInt32 pAction) { return DLP_API.DLP_GetPatternDisplay(id, ref pAction); }

#region 整合后接口便于使用
        public static Int32 ConfigPatternSequence(Int32 id, PatternConfigStruct patCfg, List<PatternSequenceStruct> patSeq)
        {
            Int32 ret = 0;
            
            UInt32 count = (UInt32)patSeq.Count;
            Byte[] imageLut = new Byte[count];
            //0.设置模式为Pattern模式 0-video, 1- pattern
            if ((ret = SetMode(id, 1)) < 0) goto _END;
            //1.添加到表格
            for (int i = 0; i < count; i++)
            {
                imageLut[i] = patSeq[i].imageIdx;
                if ((ret = AddToPatLut(id, 0, patSeq[i].patNum, patSeq[i].bitDepth, (Int32)patSeq[i].ledColor, false, false, true, false)) < 0)
                    goto _END;
            }
            //2.设置Patten样式
            if ((ret = SetPatternConfig(id, count, Convert.ToByte(patCfg.repeat), count, count)) < 0) goto _END;
            //3.发送Pattern表格
            if ((ret = SendPatLut(id)) < 0) goto _END;
            //4.发送图像表格
            if ((ret = SendImageLut(id, imageLut, count)) < 0) goto _END;
            //5.Set LED
            if ((ret = SetLedCurrents(id, patCfg.currentRed, patCfg.currentGreen, patCfg.currentBlue)) < 0) goto _END;
            //6.设置曝光时间
            if ((ret = SetExposure_FramePeriod(id, patCfg.exposurePeriod, patCfg.framePeriod)) < 0) goto _END;
            //7.校验
            UInt32 pStatus = 0;
            if ((ret = ValidatePatLutData(id, ref pStatus)) < 0) goto _END;
        _END:
            return ret;
        }
#endregion
    }
    #endregion
}