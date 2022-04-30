using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Utils
{
    public class TranslatorHelper
    {
        //定义一个StrModel的类，用于接受从srt/ass文件读取的文件格式
        private class SubLineModel
        {
            public string BeginTime { get; set; }
            public string EndTime { get; set; }
            public string MainLine { get; set; }
            public string SecondLine { get; set; }
        }
        
            public class BaiduTransObj
            {
                //注意：就是此类的三个属性名称必须和json数据里面的key一致
                public string from { set; get; }
                public string to { set; get; }
                public List<BaiduTransResult> trans_result { set; get; }
            }
            public class BaiduTransResult
            {
                public string src { set; get; }
                public string dst { set; get; }
            }
        public static readonly Dictionary<string,string> Language=new Dictionary<string, string>(){
        {"Afrikaans","af"},{"Albanian","sq"},{"Amharic","am"},{"Armenian","hy"},{"Assamese","as"},{"Azerbaijani","az"},
        {"Bangla","bn"},{"Bashkir","ba"},{"Basque","eu"},{"Bosnian(Latin)","bs"},{"Bulgarian","bg"},
        {"Cantonese (Traditional)","yue"},{"Catalan","ca"},{"Chinese(Literary)","lzh"},{"Chinese Simplified","zh"},{"Chinese Traditional","zh-Hant"},{"Croatian","hr"},{"Czech","cs"},
        {"Danish","da"},{"Dari","prs"},{"Divehi","dv"},{"Dutch","nl"},
        {"English","en"},{"Estonian","et"},
        {"Fijian","fj"},{"Filipino","fil"},{"Finnish","fi"},{"French","fr"},{"French(Canada)","fr-ca"},
        {"Galician","gl"},{"Georgian","ka"},{"German","de"},{"Greek","el"},{"Gujarati","gu"},
        {"Haitian Creole","ht"},{"Hebrew","he"},{"Hindi","hi"},{"Hmong Daw","mww"},{"Hungarian","hu"},
        {"Icelandic","is"},{"Indonesian","id"},{"Inuinnaqtun","ikt"},{"Inuktitut","iu"},{"Inuktitut (Latin)","iu-Latn"},{"Irish","ga"},{"Italian","it"},
        {"Japanese","ja"},
        {"Kannada","kn"},{"Kazakh","kk"},{"Khmer","km"},{"Klingon","tlh-Latn"},{"Klingon(plqaD)","tlh-Piqd"},{"Korean","ko"},{"Kurdish(Central)","ku"},{"Kurdish(Northern)","kmr"},{"Kyrgyz","ky"},
        {"Lao","lo"},{"Latvian","lv"},{"Lithuanian","lt"},
        {"Macedonian","mk"},{"Malagasy","mg"},{"Malay","ms"},{"Malayalam","ml"},{"Maltese","mt"},{"Maori","mi"},{"Marathi","mr"},{"Mongolian(Cyrillic)","mn-Cyrl"},{"Mongolian(Traditional)","mn-Mong"},{"Myanmar","my"},
        {"Nepali","ne"},{"Norwegian","nb"},
        {"Odia","or"},
        {"Pashto","ps"},{"Persian","fa"},{"Polish","pl"},{"Portuguese(Brazil)","pt"},{"Portuguese(Portugal)","pt-pt"},{"Punjabi","pa"},
        {"Queretaro Otomi","otq"},
        {"Romanian","ro"},{"Russian","ru"},
        {"Samoan","sm"},{"Serbian(Cyrillic)","sr-Cyrl"},{"Serbian(Latin)","sr-Latn"},{"Slovak","sk"},{"Slovenian","sl"},{"Somali","so"},{"Spanish","es"},{"Swahili","sw"},{"Swedish","sv"},
        {"Tahitian","ty"},{"Tamil","ta"},{"Tatar","tt"},{"Telugu","te"},{"Thai","th"},{"Tibetan","bo"},{"Tigrinya","ti"},{"Tongan","to"},{"Turkish","tr"},{"Turkmen","tk"},
        {"Ukrainian","uk"},{"Upper Sorbian","hsb"},{"Urdu","ur"},{"Uyghur","ug"},{"Uzbek(Latin)","uz"},
        {"Vietnamese","vi"},{"Welsh","cy"},{"Yucatec Maya","yua"},{"Zulu","zu"},
        };
        public static readonly string[] TranslateServer = { "Baidu","Bing"};
        public static readonly int TransServerUsing = 0;
        public static readonly string[] SuporttedFormat={ "ass", "srt" };

        //Baidu Fanyi Server Info
        private static readonly string baiduAppId = "20220430001197782";
        private static readonly string baiduKey = "TXwFnIYmM5gUY58OTKi8";
        private static readonly int baiduRequestDelayTime = 3000;

        //Azure Bing Server Info
        private static readonly string bingKey = "1c6aac0dcdd04bd19f79ed51109540d4";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        // Add your location, also known as region. The default is global.
        // This is required if using a Cognitive Services resource.
        private static readonly string location = "eastasia";

        private static readonly string spiltMark = "&#";
        private static readonly string assHeader = "[Script Info]\n; This is an Advanced Sub Station Alpha v4+ script.\nTitle: \nScriptType: v4.00+\nPlayDepth: 0\nScaledBorderAndShadow: Yes\n\n[V4 + Styles]\nFormat: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding\nStyle: Default,Arial,20,&H00FFFFFF,&H0000FFFF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,1,1,2,10,10,10,1\n\n[Events]\nFormat: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";

        public static string TranslateText(string textToTranslate, string from = "en", string to = "zh-Hans")
        {
            switch (TransServerUsing)
            {
                case 0:
                    return BDTranslateText(textToTranslate, from, to);
                case 1:
                    return MSTranslateText(textToTranslate, from, to);
                default:
                    return "";
            }
        }
        public static string BDTranslateText(string textToTranslate, string from = "en", string to = "zh")
        {
            // 原文
            string q = textToTranslate;
            // 改成您的APP ID
            string appId = baiduAppId;
            // 改成您的APP ID
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = baiduKey;
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string gotString = myStreamReader.ReadToEnd();
            BaiduTransObj transObj = JsonConvert.DeserializeObject<BaiduTransObj>(gotString);
            string rst = "";
            foreach (var e in transObj.trans_result)
            {
                rst += e.dst;
            }
            return rst;
        }

        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
        /// <summary>
        /// 翻译并输出翻译后的文本
        /// </summary>
        /// <param name="textToTranslate">需要翻译的文本</param>
        /// <param name="from">从什么语言</param>
        /// <param name="to">到什么语言</param>
        /// <returns>翻译后的文本</returns>
        public static string MSTranslateText(string textToTranslate, string from = "en", string to = "zh-Hans")
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
                request.Headers.Add("Ocp-Apim-Subscription-Key", bingKey);
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
        public static void TranslateSubTextFile(string filepath, string savepath, int subFormat, string from= "English", string to= "Chinese Simplified")
        {
            FileInfo fi = new FileInfo(filepath);
            Encoding ecd = GetFileEncodeType(filepath);
            List<StringBuilder> rst;
            switch (fi.Extension.ToLower())
            {
                case ".ass":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            rst=TranslateAssSubStr(sr,from,to);
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            rst=TranslateSrtSubStr(sr, from, to);
                            break;
                        }
                    }
                default:
                    {
                        return;
                    }
            }
            using (StreamWriter sw = new StreamWriter(savepath, false, ecd))
            {
                sw.Write(rst[subFormat].ToString());
                sw.Flush();
                sw.Close();
            }
        }
        /// <summary>
        /// 翻译Srt格式文本流
        /// </summary>
        /// <param name="sr">Srt格式的文本流</param>
        /// <param name="newsubtext">翻译后的文本</param>
        public static List<StringBuilder> TranslateSrtSubStr(StreamReader sr, string from = "English", string to = "Chinese Simplified")
        {
            List<SubLineModel> SubModel = new List<SubLineModel>();
            string substr;
            StringBuilder subLines=new StringBuilder();
            string newsubstr;
            while (!sr.EndOfStream)
            {
                substr = sr.ReadLine();
                if (substr != "")
                {
                    newsubstr = "";
                    SubLineModel slm = new SubLineModel();
                    substr = sr.ReadLine();
                    slm.BeginTime = substr.Substring(0,substr.IndexOf('-'));
                    slm.EndTime = substr.Substring(substr.LastIndexOf('>')+2, substr.Length-(substr.LastIndexOf('>') +2));
                    substr = sr.ReadLine();
                    slm.MainLine = "";
                    while (substr != "")
                    {
                        slm.MainLine += substr+"\n";
                        newsubstr += substr+"\n";
                        substr = sr.ReadLine();
                    }
                    slm.MainLine += substr;
                    SubModel.Add(slm);
                    subLines.Append(newsubstr + spiltMark);
                }
            }
            string transedSubLineAll = TranslateText(subLines.ToString(), Language[from], Language[to]);
            string[] transedSubLines = transedSubLineAll.Split(new string[] { spiltMark }, StringSplitOptions.RemoveEmptyEntries);
            while (transedSubLines.Length < SubModel.Count())
            {
                subLines.Clear();
                for (int i = transedSubLines.Length; i < SubModel.Count(); i++)
                {
                    subLines.Append(SubModel[i].MainLine + spiltMark);
                }
                Thread.Sleep(baiduRequestDelayTime);
                transedSubLineAll += TranslateText(subLines.ToString(), Language[from], Language[to]);
                transedSubLines = transedSubLineAll.Split(new string[] { spiltMark }, StringSplitOptions.RemoveEmptyEntries);
            }
            return GenerateResultTxt(SubModel, transedSubLines);
        }
        /// <summary>
        /// 翻译Ass格式文本流
        /// </summary>
        /// <param name="sr">Ass格式的文本流</param>
        /// <param name="newsubtext">翻译后的文本</param>
        public static List<StringBuilder> TranslateAssSubStr(StreamReader sr, string from = "English", string to = "Chinese Simplified")
        {
            List<SubLineModel> SubModel = new List<SubLineModel>();
            string substr;
            StringBuilder subLines = new StringBuilder();
            Queue<string> substrs = new Queue<string>();
            string line;
            int a,b,x,c,d,e;
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
                    SubLineModel slm = new SubLineModel();
                    c = substr.IndexOf(',') + 1;
                    d = substr.IndexOf(',', c)+1;
                    e = substr.IndexOf(',', d);
                    slm.BeginTime = substr.Substring(c, d-c-1);
                    slm.EndTime = substr.Substring(d, e-d);
                    x = Math.Max(a+2, b+1);
                    line = substr.Substring(x, substr.Length - x);
                    slm.MainLine = line;
                    SubModel.Add(slm);
                    subLines.Append(line + spiltMark);
                }
            }
            string transedSubLineAll = TranslateText(subLines.ToString(),Language[from], Language[to]);
            string[] transedSubLines = transedSubLineAll.Split(new string[] { spiltMark }, StringSplitOptions.RemoveEmptyEntries);
            while (transedSubLines.Length < SubModel.Count())
            {
                subLines.Clear();
                Thread.Sleep(baiduRequestDelayTime);
                for (int i = transedSubLines.Length; i < SubModel.Count(); i++)
                {
                    subLines.Append(SubModel[i].MainLine + spiltMark);
                }
                transedSubLineAll +=TranslateText(subLines.ToString(), Language[from], Language[to]);
                transedSubLines = transedSubLineAll.Split(new string[] { spiltMark }, StringSplitOptions.RemoveEmptyEntries);
            }
            return GenerateResultTxt(SubModel, transedSubLines);
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

        private static List<StringBuilder> GenerateResultTxt(List<SubLineModel> SubModel, string[] transedSubLines)
        {
            StringBuilder srtTxt = new StringBuilder();
            StringBuilder assTxt = new StringBuilder();
            assTxt.Append(assHeader);

            for (int i = 0; i < SubModel.Count(); i++)
            {
                SubModel[i].SecondLine = transedSubLines[i];
                srtTxt.Append((i + 1).ToString() + "\n");
                srtTxt.Append(SubModel[i].BeginTime + " --> " + SubModel[i].EndTime + "\n");
                srtTxt.Append(SubModel[i].MainLine + "\n");
                srtTxt.Append(SubModel[i].SecondLine + "\n\n");
                assTxt.Append("Dialogue: 0," + SubModel[i].BeginTime + "," + SubModel[i].EndTime + ",Default,,0,0,0,," + SubModel[i].MainLine + @"\N" + SubModel[i].SecondLine + "\n");
            }
            return new List<StringBuilder>() { assTxt,srtTxt};
        }

    }
}
