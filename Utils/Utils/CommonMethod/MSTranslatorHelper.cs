using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Utils
{
    public class MSTranslatorHelper
    {

        private static readonly string key = "1c6aac0dcdd04bd19f79ed51109540d4";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";

        // Add your location, also known as region. The default is global.
        // This is required if using a Cognitive Services resource.
        private static readonly string location = "eastasia";
        /// <summary>
        /// 翻译并输出翻译后的文本
        /// </summary>
        /// <param name="textToTranslate">需要翻译的文本</param>
        /// <param name="from">从什么语言</param>
        /// <param name="to">到什么语言</param>
        /// <returns>翻译后的文本</returns>
        public static string TranslateText(string textToTranslate, string from="en", string to="zh")
        {
            string route = "/translate?api-version=3.0&from="+from+"&to="+to;
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                // Read response as a string.
                string rst= response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                int start = rst.IndexOf("text") + 7;
                int len = rst.LastIndexOf("to") - 3 - start;
                if(len>=0&& start>=0)
                    rst = rst.Substring(start, len);
                return rst;
            }
        }


        /// <summary>
        /// 翻译字幕文件
        /// </summary>
        /// <param name="filepath">字幕文件路径</param>
        /// <param name="savepath">翻译后保存的文件路径</param>
        public static void TranslateSubTextFile(string filepath, string savepath)
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
                            TranslateAssSubStr(sr, newsubtext);
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            TranslateSrtSubStr(sr, newsubtext);
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
        /// 翻译Srt格式文本流
        /// </summary>
        /// <param name="sr">Srt格式的文本流</param>
        /// <param name="newsubtext">翻译后的文本</param>
        public static void TranslateSrtSubStr(StreamReader sr, StringBuilder newsubtext)
        {
            Queue<string> substrs=new Queue<string>();
            string substr;
            StringBuilder subLines=new StringBuilder();
            string newsubstr;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                substrs.Enqueue(substr);
                if (substr != "")
                {
                    newsubstr = "";
                    substr = sr.ReadLine();
                    substrs.Enqueue(substr);
                    substr = sr.ReadLine();
                    while (substr != "")
                    {
                        substrs.Enqueue(substr);
                        newsubstr += substr;
                        substr = sr.ReadLine();
                    }
                    subLines.Append(newsubstr + "\r\n");
                    substrs.Enqueue(substr);
                }
            }
            string transedSubLineAll = TranslateText(subLines.ToString());
            string[] transedSubLines = transedSubLineAll.Split(new string[] { "\\r\\n" }, StringSplitOptions.RemoveEmptyEntries);
            Queue<string> qs = new Queue<string>(transedSubLines);
            while (substrs.Count()>0)
            {
                substr = substrs.Dequeue();
                if (substr != "")
                {
                    newsubtext.Append(substr + "\n");
                    substr = substrs.Dequeue();
                    newsubtext.Append(substr + "\n");
                    substr = substrs.Dequeue();
                    while (substr != "")
                    {
                        newsubtext.Append(substr + "\n");
                        substr = substrs.Dequeue();
                    }
                    if (qs.Count() > 0)
                    {
                        newsubstr = qs.Dequeue();
                        newsubtext.Append(newsubstr + "\n");
                    }
                    newsubtext.Append(substr + "\n");
                }
            }
        }
        /// <summary>
        /// 翻译Ass格式文本流
        /// </summary>
        /// <param name="sr">Ass格式的文本流</param>
        /// <param name="newsubtext">翻译后的文本</param>
        public static void TranslateAssSubStr(StreamReader sr, StringBuilder newsubtext)
        {
            string substr;
            StringBuilder subLines = new StringBuilder();
            Queue<string> substrs = new Queue<string>();
            string line;
            int a,b,x;
            string maska,maskb;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                maska = @",,"; 
                maskb = @"}";
                x = substr.Length-1;
                a = substr.LastIndexOf(maska);
                b = substr.LastIndexOf(maskb);
                if (a != -1 || b != -1)
                {
                    x = Math.Max(a+2, b+1);
                    line = substr.Substring(x, substr.Length - x);
                    subLines.Append(line + "\r\n");
                    substrs.Enqueue(substr+@"\N");
                }
                else
                {
                    substrs.Enqueue(substr);
                }
            }
            string transedSubLineAll = TranslateText(subLines.ToString());
            string[] transedSubLines = transedSubLineAll.Split(new string[] { "\\r\\n" }, StringSplitOptions.RemoveEmptyEntries);
            Queue<string> qs = new Queue<string>(transedSubLines);
            string newsubstr;
            while (substrs.Count() > 0)
            {
                substr = substrs.Dequeue();
                if (substr.LastIndexOf(@"\N")==substr.Length-2&&qs.Count() > 0)
                {
                    newsubstr = qs.Dequeue();
                    newsubtext.Append(substr + newsubstr + "\n");
                }
                else
                    newsubtext.Append(substr + "\n");
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
