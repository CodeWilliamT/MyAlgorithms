using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using HT_Lib;
using System.Threading;

namespace LeadframeAOI
{
    class MainModule
    {
        private int i_Start;      // start button
        private int i_Stop;     // stop button
        private int i_Reset;      // clear button 复位按钮
        private int o_ltRed;        // red lamp
        private int o_ltYellow;     // yellow lamp
        private int o_ltGreen;     // greem lamp
        private int o_Buzzer;     // beep
        public Boolean MuteON;     //静音

        public void Initilaize(int ltGreen, int ltRed, int ltYellow, int Buzzer, int btnStart, int btnStop, int btnReset, Boolean MuteON)
        {
            i_Start = btnStart;       // start button
            i_Stop = btnStop;     // stop button
            i_Reset = btnReset;      // clear button 复位按钮
            o_ltRed = ltRed;        // red lamp
            o_ltYellow = ltYellow;     // yellow lamp
            o_ltGreen = ltGreen;     // greem lamp
            o_Buzzer = Buzzer;     // beep
            this.MuteON = MuteON;
        }
        public Boolean GetStartSignal()
        {
            return HTM.GetDI(i_Start) == 1;
        }

        public Boolean GetStopSignal()
        {
            return HTM.GetDI(i_Stop) == 1;
        }
         
        public Boolean GetResetSignal()
        {
            return HTM.GetDI(i_Reset) == 1;
        }

        public Boolean SetSingnalDevice(Boolean RedON = true, Boolean GreenON = true, Boolean YellowON = true, Boolean BuzzerON = true)
        {
            if (HTM.WriteDO(o_ltRed, Convert.ToInt32(RedON)) == -1)
            {
                HTLog.Error("红灯设置失败");
                return false;
            }
            if (HTM.WriteDO(o_ltGreen, Convert.ToInt32(GreenON)) == -1)
            {
                HTLog.Error("绿灯设置失败");
                return false;
            }
            if (HTM.WriteDO(o_ltYellow, Convert.ToInt32(YellowON)) == -1)
            {
                HTLog.Error("黄灯设置失败");
                return false;
            }
            if (MuteON == true)
            {
                BuzzerON = false;
            }
            if (HTM.WriteDO(o_Buzzer, Convert.ToInt32(BuzzerON)) == -1)
            {
                HTLog.Error("蜂鸣器设置失败");
                return false;
            }
            return true;
        }
    }
}
