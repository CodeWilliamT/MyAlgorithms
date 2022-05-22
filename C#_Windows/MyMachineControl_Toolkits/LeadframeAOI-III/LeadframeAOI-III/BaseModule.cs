using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HT_Lib;
using System.Data.SQLite;
using Utils;
using System.IO;
using System.ComponentModel;
using System.Reflection;


namespace LeadframeAOI
{
    /// <summary>
    /// Base class for modules
    /// </summary>
    public class BaseModule : Base
    {
        protected ModuleState moduleState = 0;
        /// <summary>
        /// 模块回零状态  false--未回零  true--已回零
        /// </summary>
        protected Boolean ready = false;
        protected Double speed = 1.0;
        protected Double timeDelay = 5.0;
        protected int runMode; //0-离线 1-demo 2-online 

        [BrowsableAttribute(false)]
        public int RunMode { get { return runMode; } set { runMode = value; } }
        [BrowsableAttribute(false)]
        public ModuleState CurrentModuleState
        {
            set { moduleState = value; }
            get { return moduleState; }
        }
        public BaseModule(String para_file, String para_table) : base(para_file, para_table) { }
        [BrowsableAttribute(false)]
        public Boolean Ready { get { return ready; } }

        public virtual Boolean Home(Boolean a) { return true; }
        public virtual Boolean Home() { return true; }


        public virtual Boolean ReadPara()
        {
            return true;
        }
        public virtual Boolean SavePara()
        {
            return true;
        }
        public Boolean IsSignalOn(int Senser)
        {
            Boolean ret = false;
            if (HTM.ReadDI(Senser) == 0)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
    }

}
