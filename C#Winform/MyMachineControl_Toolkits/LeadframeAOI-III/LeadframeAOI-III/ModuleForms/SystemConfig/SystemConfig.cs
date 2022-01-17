using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using System.IO;
using System.Runtime.InteropServices;
using IniDll;
using System.Windows.Forms;
using HT_Lib;

namespace LeadframeAOI
{
    class SystemConfig : Base
    {
        public Int32 systemRunMode;
        public Boolean LoadManually = true;
        public Int32 CatchMode = 0;
        public Boolean QrChuckLocation = true;
        public Boolean Marking = false;
        public Boolean PrdQr = false;
        public Boolean InspectionMultiDies = false;
        public Int32 ImageNgSave = 0;
        public Boolean test = false;
        public Int32 ScanMode = 0;
        public Boolean UseNGBox = false;
        public Boolean LoadSame = false;
        public Int32 LotIdMode = 0;
        public Int32 UseEnHanceLight = 0;
        public Int32 MarkSave = 0;
        public Int32 ScanLot = 0;
        public Int32 ScanRecipe = 0;
        public Int32 LotMode = 0;
        public Int32 SwichLineWidth = 0;
        public Int32 JawCatchMode = 0;
        public Int32 UseEnHanceLight_Ex = 0;


        /// <summary>
        /// 机器版本 1-第一版(信利)，2-第二版(嘉盛)
        /// </summary>
        private static int machineVersion = 2;
        public static int MachineVersion
        {
            get { return machineVersion; }
        }

        //public int ChuckBaseTime = 0;
        public SystemConfig(String para_file, String para_table) : base(para_file, para_table) { }

        IniFiles config = new IniFiles(Application.StartupPath + "\\machineVersion.ini");
        
        /// <summary>
        /// 系统模式配置方法 by LYH
        /// </summary>
        /// 
        public Boolean SystemConfigInit()
        {
            Boolean ret = false;
            config.ReadInteger("Version","machineVersion",out machineVersion);

                if (Read() == false)
                {
                    HTLog.Error(GetLastErrorString());
                    goto _end;
                }
                ret = true;
        _end:
            return ret;
        }
        /// <summary>
        /// 存储配置信息
        /// </summary>
        /// <returns></returns>
        public Boolean SaveSystemConfig()
        {
            Boolean ret = false;
            if (Save() == false)
            {
                HTLog.Error(GetLastErrorString());
                throw new Exception("是否保存NG图像设置失败");
                goto _end;
            }
            ret = true;
            HTLog.Debug("系统保存参数成功");
            _end:
            return ret;
        }
        
    }
}


