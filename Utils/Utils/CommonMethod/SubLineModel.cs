using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public enum SubType { ass, srt }
    //用于接受从srt/ass文件读取的文件格式
    public class SubLineModel
    {
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AssBeginTime
        {
            get
            {
                return BeginTime.ToString(@"h\:mm\:ss\.ff");
            }
        }
        public string AssEndTime
        {
            get
            {
                return EndTime.ToString(@"h\:mm\:ss\.ff");
            }
        }
        public string SrtBeginTime
        {
            get
            {
                return BeginTime.ToString(@"hh\:mm\:ss\,fff");
            }
        }
        public string SrtEndTime
        {
            get
            {
                return EndTime.ToString(@"hh\:mm\:ss\,fff");
            }
        }
        public string MainLine { get; set; }
        public string SecondLine { get; set; }
    }

}
