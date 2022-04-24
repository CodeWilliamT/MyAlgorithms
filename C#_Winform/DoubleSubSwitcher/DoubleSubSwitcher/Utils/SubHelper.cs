using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class SubHelper
    {
        /// <summary>
        /// 交换某路径的双字幕文件的字幕，并保存到一路径
        /// </summary>
        /// <param name="filepath">双字幕文件路径</param>
        /// <param name="savepath">转化后保存的文件路径</param>
        public static void switchSubText(string filepath, string savepath)
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
            substr = sr.ReadLine();
            while (substr != null)
            {
                if (substr == "")
                {
                    newsubtext.Append(substr + "\n");
                    substr = sr.ReadLine();
                    newsubtext.Append(substr + "\n");
                    substr = sr.ReadLine();
                    newsubtext.Append(substr + "\n");
                    substr = sr.ReadLine();
                    newsubstr = substr;
                    substr = sr.ReadLine();
                    substr = sr.ReadLine();
                }
                else
                {
                    newsubtext.Append(substr + "\n");
                    substr = sr.ReadLine();
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
