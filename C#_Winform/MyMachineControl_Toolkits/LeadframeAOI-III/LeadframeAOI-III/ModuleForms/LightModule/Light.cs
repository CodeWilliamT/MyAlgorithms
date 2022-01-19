using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HT_Lib;
using Utils;
using IniDll;
using System.IO;
using System.Windows.Forms;

namespace LeadframeAOI
{
    public enum LightUseFor
    {
        ScanPoint1st = 1,
        ScanPoint2nd = 2,
        ScanPoint3th = 3
    }
    class Light : BaseModule
    {

        public int _coaxialLightDev1;  //同轴光源设备号在配置界面里其他可以找到
        public int _coaxialLightDev2;  //同轴光源设备号在配置界面里其他可以找到
        public int _ringLightDev;//环形光源设备号
        public int _trigBoardDev;
        public int CoaxialLightDev
        {
            get { return _coaxialLightDev1; }
        }

        public int RingLightDev
        {
            get { return _ringLightDev; }

            set { _ringLightDev = value;}
        }

        public Light(String para_file, String para_table) : base(para_file, para_table) { }

        /// <summary>
        /// 设置同轴光、环形光和位置触发板号
        /// </summary>
        /// <param name="coaxialDev"></param>
        /// <param name="ringDev"></param>
        /// <param name="trigBoard"></param>
        public void initial(int coaxialDev,int ringDev,int trigBoard)
        {
            _coaxialLightDev1 = coaxialDev;
            _ringLightDev = ringDev;
            _trigBoardDev = trigBoard;
        }   
        /// <summary>
        /// 配置光源
        /// </summary>
        /// <param name="lightIdx">设备编号(见轴调试助手的设备列表)</param>
        /// <param name="enable">是否使能</param>
        /// <param name="intensity">触发时长</param>
        private void SetLight(int lightIdx, bool enable, double intensity)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return;
            }
            //将同轴光的触发源关闭
            if (HTM.SetLightTrigSrc(lightIdx, enable ? HTM.LightTrigSrc.ALL: HTM.LightTrigSrc.NONE) < 0)
            {
                throw new Exception("关闭光源失败");
            }
            if (HTM.SetLightTrigTime(lightIdx, intensity) < 0)
            {
                throw new Exception("光源时间写入失败");
            }
        }
        /// <summary>
        /// 配置环形光
        /// </summary>
        /// <param name="enable">是否使能</param>
        /// <param name="intensity">触发时长</param>
        public void SetRingLight(bool enable, double intensity)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return;
            }
            if (App.obj_SystemConfig.UseEnHanceLight == 1)
            {
                App.enHance_Light.Enable(enable);
                if(enable)
                    App.enHance_Light.ChangeTriggerTime((int)intensity);
                return;
            }
            else
            {
                _ringLightDev = 3;
                SetLight(_ringLightDev, enable, intensity);
            }
        }
        /// <summary>
        /// 配置多通道增亮环形光某通道
        /// </summary>
        /// <param name="channel">是否使能</param>
        /// <param name="enable">是否使能</param>
        /// <param name="intensity">触发时长</param>
        public void SetRingLight_Ex(int channel,bool enable, double intensity)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return;
            }
            if (App.obj_SystemConfig.UseEnHanceLight_Ex == 1)
            {
                App.enHance_LightEx.Enable(channel, enable);
                if (enable)
                    App.enHance_LightEx.ChangeTriggerTime(channel, (int)intensity);
                return;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 配置同轴光
        /// </summary>
        /// <param name="enable">是否使能</param>
        /// <param name="intensity">触发时长</param>
        public void SetCoaxLight(bool enable, double intensity)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return;
            }
            _coaxialLightDev1 = 1;
            SetLight(_coaxialLightDev1, enable, intensity);
        }
        /// <summary>
        /// 配置上同轴光
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="intensity"></param>
        public void SetCoaxLight1(bool enable, double intensity)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return;
            }
            _coaxialLightDev1 = 1; 
            SetLight(_coaxialLightDev1, enable, intensity);
            return;
        }
        /// <summary>
        /// 配置下同轴光
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="intensity"></param>
        public void SetCoaxLight2(bool enable, double intensity)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return;
            }
            _coaxialLightDev2 = 4;
            SetLight(_coaxialLightDev2, enable, intensity);
        } 

        /// <summary>
        /// 配置存储目录
        /// </summary>
        /// <param name="para_file">路径</param>
        /// <param name="para_table">表名</param>
        public void ConfigParaFile(String para_file, String para_table)
        {
            paraFile = para_file;
            paraTable = para_table;
        }
    }
}
