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
        public class BaiduTransObj
        {
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
        {"Cantonese (Traditional)","yue"},{"Catalan","ca"},{"Chinese(Literary)","lzh"},{"Chinese Simplified","zh-CN"},{"Chinese Traditional","zh-Hant"},{"Croatian","hr"},{"Czech","cs"},
        {"Danish","da"},{"Dari","prs"},{"Divehi","dv"},{"Dutch","nl"},
        {"English","en"},{"English (United States)","en-US"},{"Estonian","et"},
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
        public static readonly string[] TranslateServer = { "Google","Bing","Baidu" };

        //Private constent info
        private static readonly Dictionary<string,string> ContactMark = new Dictionary<string, string> { { "Google", "\r\n" }, { "Bing", "\r\n" }, { "Baidu", "\n" } };
        private static readonly string[] SpiltMark = { "\\r\\n" ,"\\n"};
        private static readonly Dictionary<string, int> LimitRequestDelay = new Dictionary<string, int> { { "Google", 1100 }, { "Bing", 1100 }, { "Baidu", 5500 } };
        private const int TransLinesMax = 9000;
        //Baidu Fanyi Server Info
        private static readonly string baiduAppId = "20220430001197782";//not using.
        private static readonly string baiduKey = "TXwFnIYmM5gUY58OTKi8";//will dispose.

        //Azure Bing Server Info
        private static readonly string bingKey = "1c6aac0dcdd04bd19f79ed51109540d4";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        // Add your location, also known as region. The default is global.
        // This is required if using a Cognitive Services resource.
        private static readonly string location = "eastasia";

        public static string TranslateText(string textToTranslate, string TransServerUsing, string from = "en", string to = "zh-Hans")
        {
            switch (TransServerUsing)
            {
                case "Google":
                    return TranslateerTranslateText(textToTranslate, from, to);
                case "Bing":
                    return MSTranslateText(textToTranslate, from, to);
                case "Baidu":
                    return BDTranslateText(textToTranslate, from, to);
                default:
                    return "";
            }
        }

        /// <summary>
        /// Azure Translator Bing翻译并输出翻译后的文本 用的MSDN样例修改
        /// </summary>
        /// <param name="textToTranslate">需要翻译的文本</param>
        /// <param name="from">从什么语言</param>
        /// <param name="to">到什么语言</param>
        /// <returns>翻译后的文本</returns>
        public static string TranslateerTranslateText(string textToTranslate, string from = "en", string to = "zh-Hans")
        {
            string address = "https://t.song.work/api?"+"text="+textToTranslate+"&from=" + from + "&to=" + to;
            string rstStart = "\"result\":\"";
            string rstEnd = "\"";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(address);

                // Send the request and get response.
                HttpResponseMessage response = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                // Read response as a string.
                string rst = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                int start = rst.IndexOf(rstStart) + rstStart.Length;
                if (start == rstStart.Length-1)
                {
                    throw new Exception(rst);
                }
                int end = rst.IndexOf(rstEnd, start);
                int len = end - start;
                if (len >= 0 && start >= 0)
                    rst = rst.Substring(start, len);
                else
                    throw new Exception(rst);
                return rst;
            }
        }
        /// <summary>
        /// 百度翻译 连续请求延迟需要5秒
        /// </summary>
        /// <param name="textToTranslate"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string BDTranslateText(string textToTranslate,string from = "en", string to = "zh")
        {
            string q = textToTranslate;
            string appId = baiduAppId;
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
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
            if (transObj == null || transObj.trans_result == null)
            {
                throw new Exception(gotString);
            }
            string rst = "";
            foreach (var e in transObj.trans_result)
            {
                rst += e.dst + "\\r\\n";
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
        /// Azure Translator Bing翻译并输出翻译后的文本 用的MSDN样例修改
        /// </summary>
        /// <param name="textToTranslate">需要翻译的文本</param>
        /// <param name="from">从什么语言</param>
        /// <param name="to">到什么语言</param>
        /// <returns>翻译后的文本</returns>
        public static string MSTranslateText(string textToTranslate, string from = "en", string to = "zh-Hans")
        {
            string route = "/translate?api-version=3.0&from=" + from + "&to=" + to;
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
                string rst = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                int start = rst.IndexOf("text") + 7;
                if (start == 6)
                {
                    throw new Exception(rst);
                }
                int len = rst.LastIndexOf("to") - 3 - start;
                if (len >= 0 && start >= 0)
                    rst = rst.Substring(start, len);
                return rst;
            }
        }
        /// <summary>
        /// 翻译字幕文件
        /// </summary>
        /// <param name="filepath">字幕文件路径</param>
        /// <param name="savepath">翻译后保存的文件路径</param>
        /// <param name="subFormat">希望转化为的字幕格式</param>
        /// <param name="from">从什么语言</param>
        /// <param name="to">翻译为什么语言</param>
        /// <param name="TransServerUsing">用哪个翻译服务</param>
        public static void TranslateSubTextFile(string filepath, string savepath, SubHelper.SubType subFormat, string TransServerUsing, string from = "English", string to = "Chinese Simplified")
        {
            FileInfo fi = new FileInfo(filepath);
            Encoding ecd = SubHelper.GetFileEncodeType(filepath);
            List<StringBuilder> rst;
            using (StreamReader sr = new StreamReader(filepath, ecd))
            {
                rst=TranslateSubTextStream(filepath, sr, subFormat, TransServerUsing, from, to);
            }
            using (StreamWriter sw = new StreamWriter(savepath, false, ecd))
            {
                sw.Write(rst[((int)subFormat)].ToString());
                sw.Flush();
                sw.Close();
            }
        }
        /// <summary>
        /// 翻译字幕文件流
        /// </summary>
        /// <param name="sr">字幕文件的文件信息</param>
        /// <param name="subFormat">希望转化为的字幕格式</param>
        /// <param name="from">从什么语言</param>
        /// <param name="to">翻译为什么语言</param>
        /// <param name="TransServerUsing">用哪个翻译服务</param>
        public static List<StringBuilder> TranslateSubTextStream(string fileName, StreamReader sr, SubHelper.SubType subFormat, string TransServerUsing, string from = "English", string to = "Chinese Simplified")
        {
            List<SubHelper.SubLineModel> SubModel;
            int extIdx = fileName.LastIndexOf('.');
            int len = fileName.Length;
            string ext = fileName.Substring(extIdx, len - extIdx).ToLower();
            switch (ext)
            {
                case ".ass":
                    {
                        SubModel = SubHelper.ParseAssFileStream(sr);
                        break;
                    }
                case ".srt":
                    {
                        SubModel = SubHelper.ParseSrtFileStream(sr);
                        break;
                    }
                default:
                    {
                        throw new Exception("It's not a supported sub type.");
                    }
            }
            SubModel = SubHelper.GetSingleSubModel(SubModel);
            string[] transedSubLines = GetTranslatedSubLines(SubModel, TransServerUsing, from, to);
            SubModel=AddTransedSubLines(SubModel, transedSubLines);
            List<StringBuilder> rst = SubHelper.GetTxtsFromSubModel(SubModel);
            return rst;
        }

        /// <summary>
        /// 根据模型生成翻译后的每一句字幕的数组
        /// </summary>
        /// <param name="SubModel"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="TransServerUsing"></param>
        /// <returns></returns>
        private static string[] GetTranslatedSubLines(List<SubHelper.SubLineModel> SubModel, string TransServerUsing, string from = "English", string to = "Chinese Simplified")
        {
            StringBuilder subLines = new StringBuilder();
            string transedSubLineAll = "";
            string[] transedSubLines = transedSubLineAll.Split(SpiltMark, StringSplitOptions.RemoveEmptyEntries);
            while (transedSubLines.Length < SubModel.Count())
            {
                if (!string.IsNullOrEmpty(transedSubLineAll))
                    transedSubLineAll = transedSubLineAll.Remove(transedSubLineAll.Length - transedSubLines[transedSubLines.Length - 1].Length, transedSubLines[transedSubLines.Length - 1].Length);
                subLines.Clear();
                for (int i = Math.Max(transedSubLines.Length - 1, 0), cnt = 0; i < SubModel.Count() && subLines.Length < TransLinesMax; i++, cnt++)
                {
                    subLines.Append(SubModel[i].RawLines[0] + ContactMark[TransServerUsing]);
                }
                Thread.Sleep(TranslatorHelper.LimitRequestDelay[TransServerUsing]);
                transedSubLineAll += TranslatorHelper.TranslateText(subLines.ToString(), TransServerUsing, TranslatorHelper.Language[from], TranslatorHelper.Language[to]);
                transedSubLines = transedSubLineAll.Split(SpiltMark, StringSplitOptions.RemoveEmptyEntries);
            }
            return transedSubLines;
        }
        private static List<SubHelper.SubLineModel> AddTransedSubLines(List<SubHelper.SubLineModel> SubModel, string[] transedSubLines)
        {
            for (int i = 0; i < SubModel.Count(); i++)
            {
                SubModel[i].RawLines.Add(transedSubLines[i]);
            }
            return SubModel;
        }


    }
}
