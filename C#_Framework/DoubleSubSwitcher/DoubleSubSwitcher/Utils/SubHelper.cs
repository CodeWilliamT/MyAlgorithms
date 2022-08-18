using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class SubHelper
    {
        public static readonly string assHeader = "[Script Info]\n; This is an Advanced Sub Station Alpha v4+ script.\nTitle: \nScriptType: v4.00+\nPlayDepth: 0\nScaledBorderAndShadow: Yes\n\n[V4 + Styles]\nFormat: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding\nStyle: Default,Arial,20,&H00FFFFFF,&H0000FFFF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,1,1,2,10,10,10,1\n\n[Events]\nFormat: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";


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
            public List<string> Lines { get; set; }

            public List<string> RawLines { get; set; }
        }



        /// <summary>
        /// 交换某路径的双字幕文件的字幕，并保存到某路径
        /// </summary>
        /// <param name="filepath">双字幕文件路径</param>
        /// <param name="savepath">转化后保存的文件路径</param>
        public static void switchSubTextFile(string filepath, string savepath)
        {
            FileInfo fi = new FileInfo(filepath);
            Encoding ecd = GetFileEncodeType(filepath);
            List<SubLineModel> subLineModels;
            SubType subType = SubType.ass;
            switch (fi.Extension.ToLower())
            {
                case ".ass":
                    {
                        subType = SubType.ass;
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            subLineModels = ParseAssFileStream(sr);
                            break;
                        }
                    }
                case ".srt":
                    {
                        subType = SubType.srt;
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            subLineModels = ParseSrtFileStream(sr);
                            break;
                        }
                    }
                default:
                    {
                        throw new Exception("It's not a supported sub type.");
                        break;
                    }
            }
            subLineModels = switchSubLines(subLineModels);
            List<StringBuilder> subTexts = GetTxtsFromSubModel(subLineModels);
            using (StreamWriter sw = new StreamWriter(savepath, false, ecd))
            {
                sw.Write(subTexts[(int)subType]);
                sw.Flush();
                sw.Close();
            }
        }
        /// <summary>
        /// 将srt格式文本流转化为字幕模型
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static List<SubLineModel> ParseSrtFileStream(StreamReader sr)
        {
            List<SubLineModel> rst = new List<SubLineModel>();
            SubLineModel subLineModel = new SubLineModel();
            string substr, beginTimeStr, endTimeStr;
            int num = 1;
            substr = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                if (substr == num.ToString())
                {
                    num++;
                    SubLineModel slm = new SubLineModel();
                    substr = sr.ReadLine();
                    beginTimeStr = substr.Substring(0, substr.IndexOf('-') - 1).Replace(",", ".");
                    endTimeStr = substr.Substring(substr.LastIndexOf('>') + 2, substr.Length - (substr.LastIndexOf('>') + 2)).Replace(",", ".");
                    slm.BeginTime = TimeSpan.Parse(beginTimeStr);
                    slm.EndTime = TimeSpan.Parse(endTimeStr); ;
                    substr = sr.ReadLine();
                    slm.RawLines = new List<string>();
                    slm.Lines = new List<string>();
                    slm.Lines.Add(substr);
                    substr = substr.Replace("<i>", "").Replace(@"</i>", "");
                    slm.RawLines.Add(substr);
                    substr = sr.ReadLine();
                    while (substr != null&&substr != num.ToString())
                    {
                        slm.Lines.Add(substr);
                        substr = substr.Replace("<i>", "").Replace(@"</i>", "");
                        slm.RawLines.Add(substr);
                        substr = sr.ReadLine();
                    }
                    rst.Add(slm);
                }
            }
            return rst;
        }
        /// <summary>
        /// 将ass格式文本流转化为模型
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static List<SubLineModel> ParseAssFileStream(StreamReader sr)
        {
            List<SubLineModel> rst = new List<SubLineModel>();
            string substr, beginTimeStr, endTimeStr;
            Queue<string> substrs = new Queue<string>();
            string line;
            int a, x, c, d, e,formatStart,formatlen;
            string maska, maskFormatL, maskFormatR;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                maska = @",,";
                maskFormatL = @"{\";
                maskFormatR = @"}";
                x = substr.Length - 1;
                a = substr.LastIndexOf(maska);
                if (a != -1)
                {
                    SubLineModel slm = new SubLineModel();
                    c = substr.IndexOf(',') + 1;
                    d = substr.IndexOf(',', c) + 1;
                    e = substr.IndexOf(',', d);
                    beginTimeStr = substr.Substring(c, d - c - 1);
                    endTimeStr = substr.Substring(d, e - d);
                    slm.BeginTime = TimeSpan.Parse(beginTimeStr);
                    slm.EndTime = TimeSpan.Parse(endTimeStr);
                    x = a + 2;
                    line = substr.Substring(x, substr.Length - x);
                    slm.Lines = new List<string>();
                    line = line.Replace("{\\i1}", "").Replace("{\\i0}", "");
                    slm.RawLines = line.Split(new[] { @"\N" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string s in slm.RawLines)
                    {
                        string RealLine = s;
                        formatStart = s.IndexOf(maskFormatL);
                        if (formatStart > -1)
                        {
                            formatlen = s.LastIndexOf(maskFormatR) - formatStart + 1;
                            RealLine = s.Remove(formatStart, formatlen);
                        }
                        slm.Lines.Add(RealLine);
                    }
                    rst.Add(slm);
                }
            }
            return rst;
        }

        /// <summary>
        /// 交换文本模型列表的每个模型的第一行与第二行
        /// </summary>
        /// <param name="sr">Srt格式的文本流</param>
        /// <param name="newsubtext">交换后的字符串</param>
        public static List<SubLineModel> switchSubLines(List<SubLineModel> subLineModels)
        {
            string tmp;
            foreach (var e in subLineModels)
            {
                if (e.Lines.Count() != 2) continue;
                tmp = e.Lines[0];
                e.Lines[0] = e.Lines[1];
                e.Lines[1] = tmp;

                tmp = e.RawLines[0];
                e.RawLines[0] = e.RawLines[1];
                e.RawLines[1] = tmp;
            }
            return subLineModels;
        }
        /// <summary>
        /// 根据字幕模型构建各个格式字幕文本。
        /// </summary>
        /// <param name="SubModel"></param>
        /// <returns></returns>
        public static List<StringBuilder> GetTxtsFromSubModel(List<SubHelper.SubLineModel> SubModel)
        {
            StringBuilder srtTxt = new StringBuilder();
            StringBuilder assTxt = new StringBuilder();
            assTxt.Append(assHeader);

            for (int i = 0; i < SubModel.Count(); i++)
            {
                srtTxt.Append((i + 1).ToString() + "\n");
                srtTxt.Append(SubModel[i].SrtBeginTime + " --> " + SubModel[i].SrtEndTime + "\n");
                srtTxt.Append(SubModel[i].RawLines[0] + "\n");
                assTxt.Append("Dialogue: 0," + SubModel[i].AssBeginTime + "," + SubModel[i].AssEndTime + ",Default,,0,0,0,," + SubModel[i].RawLines[0]);
                for (int j = 1; j < SubModel[i].RawLines.Count(); j++)
                {
                    srtTxt.Append(SubModel[i].RawLines[j] + "\n");
                    assTxt.Append(@"\N" + SubModel[i].RawLines[j]);
                }
                assTxt.Append("\n");
            }
            return new List<StringBuilder>() { assTxt, srtTxt };
        }

        /// <summary>
        /// 将字幕模型转化为单字幕模型
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static List<SubLineModel> GetSingleSubModel(List<SubLineModel> subLineModels)
        {
            StringBuilder tmp = new StringBuilder();
            foreach (var e in subLineModels)
            {
                tmp.Clear();
                tmp.Append(e.Lines[0]);
                for (int i = 1; i < e.Lines.Count(); i++)
                {
                    tmp.Append(" " + e.Lines[i]);
                }
                e.Lines.Clear();
                e.Lines.Add(tmp.ToString());

                tmp.Clear();
                tmp.Append(e.RawLines[0]);
                for (int i = 1; i < e.RawLines.Count(); i++)
                {
                    tmp.Append(" " + e.RawLines[i]);
                }
                e.RawLines.Clear();
                e.RawLines.Add(tmp.ToString());
            }
            return subLineModels;
        }

        /// <summary>
        /// 得到文本的编码格式
        /// </summary>
        /// <param name="filename">文本路径</param>
        /// <returns>编码格式</returns>
        public static System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);

            if (buffer[0] == 0x0A && buffer[1] == 0x0A)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0xEF && buffer[1] == 0xBB)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0x5b && buffer[1] == 0x53)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
            {
                return System.Text.Encoding.BigEndianUnicode;
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
            {
                return System.Text.Encoding.Unicode;
            }
            else
            {
                return System.Text.Encoding.UTF8;
            }
        }

    }
}
