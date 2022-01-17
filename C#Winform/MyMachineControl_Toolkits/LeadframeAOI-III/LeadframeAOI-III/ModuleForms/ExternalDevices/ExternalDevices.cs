using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using System.Threading;
using HT_Lib;



namespace LeadframeAOI
{
    /// <summary>
    /// 仅保存光源序列号，相机参数保存在ini文件内
    /// </summary>
    class ExternalDevices : Base
    {
        public string LightSN = string.Empty;
        public ExternalDevices(String para_file, String para_table) : base(para_file, para_table) { }

    }
}
