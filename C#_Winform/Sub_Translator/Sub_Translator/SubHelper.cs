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
        private static readonly string assHeader = "[Script Info]\n; This is an Advanced Sub Station Alpha v4+ script.\nTitle: \nScriptType: v4.00+\nPlayDepth: 0\nScaledBorderAndShadow: Yes\n\n[V4 + Styles]\nFormat: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding\nStyle: Default,Arial,20,&H00FFFFFF,&H0000FFFF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,1,1,2,10,10,10,1\n\n[Events]\nFormat: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";

        private static readonly string[] ContactMark = { "\n&#", "\r\n" };
        private static readonly string[] SpiltMark = { "&#", "\\r\\n" };

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

        /// <summary>
        /// 翻译字幕文件
        /// </summary>
        /// <param name="filepath">字幕文件路径</param>
        /// <param name="savepath">翻译后保存的文件路径</param>
        public static void TranslateSubTextFile(string filepath, string savepath, SubType subFormat, string from = "English", string to = "Chinese Simplified", int TransServerUsing = 0)
        {
            FileInfo fi = new FileInfo(filepath);
            Encoding ecd = GetFileEncodeType(filepath);
            List<SubLineModel> SubModel;
            switch (fi.Extension.ToLower())
            {
                case ".ass":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            SubModel = GetAssSubModel(sr);
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            SubModel = GetSrtSubModel(sr);
                            break;
                        }
                    }
                default:
                    {
                        return;
                    }
            }
            string[] transedSubLines = GetTranslatedSubLines(SubModel,from, to, TransServerUsing);
            List<StringBuilder> rst= GetTransMergedTxt(SubModel, transedSubLines);
            using (StreamWriter sw = new StreamWriter(savepath, false, ecd))
            {
                sw.Write(rst[((int)subFormat)].ToString());
                sw.Flush();
                sw.Close();
            }
        }
        /// <summary>
        /// 将Srt格式文本流转化为模型
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static List<SubLineModel> GetSrtSubModel(StreamReader sr)
        {
            List<SubLineModel> SubModel = new List<SubLineModel>();
            string substr, beginTimeStr, endTimeStr;
            string line;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                if (substr != "")
                {
                    SubLineModel slm = new SubLineModel();
                    substr = sr.ReadLine();
                    beginTimeStr = substr.Substring(0, substr.IndexOf('-') - 1).Replace(",", ".");
                    endTimeStr = substr.Substring(substr.LastIndexOf('>') + 2, substr.Length - (substr.LastIndexOf('>') + 2)).Replace(",", ".");
                    slm.BeginTime = TimeSpan.Parse(beginTimeStr);
                    slm.EndTime = TimeSpan.Parse(endTimeStr);
                    substr = sr.ReadLine();
                    slm.MainLine = substr;
                    substr = sr.ReadLine();
                    while (substr != "")
                    {
                        slm.MainLine += " " + substr;
                        substr = sr.ReadLine();
                    }
                    line = slm.MainLine;
                    slm.MainLine = line.Replace("<i>", "").Replace("</i>", "");
                    SubModel.Add(slm);
                }
            }
            return SubModel;
        }/// <summary>
        /// 将Ass格式文本流转化为模型
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static List<SubLineModel> GetAssSubModel(StreamReader sr)
        {
            List<SubLineModel> SubModel = new List<SubLineModel>();
            string substr, beginTimeStr, endTimeStr;
            Queue<string> substrs = new Queue<string>();
            string line;
            int a, b, x, c, d, e;
            string maska, maskb;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                maska = @",,";
                maskb = @"}";
                x = substr.Length - 1;
                a = substr.LastIndexOf(maska);
                b = substr.LastIndexOf(maskb);
                if (a != -1 || b != -1)
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
                    slm.MainLine = line.Replace(@"\N", " ").Replace("{\\i1}", "").Replace("{\\i0}", "");
                    SubModel.Add(slm);
                }
            }
            return SubModel;
        }
        //根据模型生成翻译后的每一句字幕的数组
        private static string[] GetTranslatedSubLines(List<SubLineModel> SubModel, string from = "English", string to = "Chinese Simplified", int TransServerUsing = 0)
        {
            StringBuilder subLines=new StringBuilder();
            string transedSubLineAll = "";
            string[] transedSubLines = transedSubLineAll.Split(SpiltMark, StringSplitOptions.RemoveEmptyEntries);
            while (transedSubLines.Length < SubModel.Count())
            {
                if(string.IsNullOrEmpty(transedSubLineAll)) transedSubLineAll.Remove(transedSubLineAll.Length - transedSubLines[transedSubLines.Length - 1].Length, transedSubLines[transedSubLines.Length - 1].Length);
                subLines.Clear();
                for (int i = transedSubLines.Length - 1; i < SubModel.Count(); i++)
                {
                    subLines.Append(SubModel[i].MainLine + ContactMark[TransServerUsing]);
                }
                Thread.Sleep(TranslatorHelper.LimitRequestDelay);
                transedSubLineAll += TranslatorHelper.TranslateText(subLines.ToString(), TranslatorHelper.Language[from], TranslatorHelper.Language[to], TransServerUsing);
                transedSubLines = transedSubLineAll.Split(SpiltMark, StringSplitOptions.RemoveEmptyEntries);
            }
            return transedSubLines;
        }
        private static List<StringBuilder> GetTransMergedTxt(List<SubLineModel> SubModel, string[] transedSubLines)
        {
            StringBuilder srtTxt = new StringBuilder();
            StringBuilder assTxt = new StringBuilder();
            assTxt.Append(assHeader);

            for (int i = 0; i < SubModel.Count(); i++)
            {
                transedSubLines[i].Replace("\n", "");
                transedSubLines[i].Replace(@"\n", "");
                SubModel[i].SecondLine = transedSubLines[i];
                SubModel[i].SecondLine = transedSubLines[i];
                srtTxt.Append((i + 1).ToString() + "\n");
                srtTxt.Append(SubModel[i].SrtBeginTime + " --> " + SubModel[i].SrtEndTime + "\n");
                srtTxt.Append(SubModel[i].MainLine + "\n");
                srtTxt.Append(SubModel[i].SecondLine + "\n\n");
                assTxt.Append("Dialogue: 0," + SubModel[i].AssBeginTime + "," + SubModel[i].AssEndTime + ",Default,,0,0,0,," + SubModel[i].MainLine + @"\N" + SubModel[i].SecondLine + "\n");
            }
            return new List<StringBuilder>() { assTxt, srtTxt };
        }


        /// <summary>
        /// 交换某路径的双字幕文件的字幕，并保存到一路径
        /// </summary>
        /// <param name="filepath">双字幕文件路径</param>
        /// <param name="savepath">转化后保存的文件路径</param>
        public static void switchSubTextFile(string filepath, string savepath)
        {
            FileInfo fi = new FileInfo(filepath);
            Encoding ecd = GetFileEncodeType(filepath);
            StringBuilder newsubtext = new StringBuilder();
            switch (fi.Extension.ToLower())
            {
                case ".ass":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            switchAssSubStr(sr, newsubtext);
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            switchSrtSubStr(sr, newsubtext);
                            break;
                        }
                    }
                default:
                    {
                        break;
                    }
            }
            using (StreamWriter sw = new StreamWriter(savepath, false, ecd))
            {
                sw.Write(newsubtext);
                sw.Flush();
                sw.Close();
            }
        }
        /// <summary>
        /// 交换Srt格式文本流的语言行
        /// </summary>
        /// <param name="sr">Srt格式的文本流</param>
        /// <param name="newsubtext">交换后的字符串</param>
        public static void switchSrtSubStr(StreamReader sr, StringBuilder newsubtext)
        {
            string substr, newsubstr;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                if (substr != "")
                {
                    newsubtext.Append(substr + "\n");
                    substr = sr.ReadLine();
                    newsubtext.Append(substr + "\n");
                    substr = sr.ReadLine();
                    newsubstr = substr;
                    substr = sr.ReadLine();
                    newsubtext.Append(substr + "\n");
                    newsubtext.Append(newsubstr + "\n");
                    substr = sr.ReadLine();
                    while (substr != "")
                    {
                        newsubtext.Append(substr + "\n");
                        substr = sr.ReadLine();
                    }
                    newsubtext.Append(substr + "\n");
                }
            }
        }
        /// <summary>
        /// 交换Ass格式文本流的语言行
        /// </summary>
        /// <param name="sr">Ass格式的文本流</param>
        /// <param name="newsubtext">交换后的字符串</param>
        public static void switchAssSubStr(StreamReader sr, StringBuilder newsubtext)
        {
            string substr, newsubstr;
            string chsstr, enustr;
            int a, b, c, d;
            string maska = @",";
            string maskb = @"\N";
            string maskc = @"}";
            substr = sr.ReadLine();
            while (substr != null)
            {
                newsubstr = substr;
                maska = @",";
                maskb = @"\N";
                maskc = @"}";
                b = substr.IndexOf(maskb);
                if (b == -1)
                    goto _end;
                a = b;
                while (substr.Substring(0, a).LastIndexOf(maska) == substr.Substring(0, a).LastIndexOf(maska + " "))
                {
                    a = substr.Substring(0, a).LastIndexOf(maska + " ") - maska.Length;
                }
                a = substr.Substring(0, a).LastIndexOf(maska) + maska.Length;
                c = substr.LastIndexOf(maskc);
                if (c == -1 || c == substr.Length - 1)
                    c = b + maskb.Length;
                else
                    c = c + maskc.Length;
                d = substr.Length;
                if (a > b || b > c)
                    goto _end;
                chsstr = substr.Substring(a, b - a);
                enustr = substr.Substring(c, d - c);
                newsubstr = substr.Substring(0, a) + enustr + substr.Substring(b, c - b) + chsstr;
            _end:

                newsubtext.Append(newsubstr + "\n");
                substr = sr.ReadLine();

            }
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
