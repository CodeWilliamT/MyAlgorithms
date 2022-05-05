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
        public static readonly int LimitRequestDelay = 3000;

        //Baidu Fanyi Server Info
        private static readonly string baiduAppId = "20220430001197782";
        private static readonly string baiduKey = "TXwFnIYmM5gUY58OTKi8";

        //Azure Bing Server Info
        private static readonly string bingKey = "1c6aac0dcdd04bd19f79ed51109540d4";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        // Add your location, also known as region. The default is global.
        // This is required if using a Cognitive Services resource.
        private static readonly string location = "eastasia";

        public static string TranslateText(string textToTranslate, string from = "en", string to = "zh-Hans", int TransServerUsing = 0)
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
        /// <summary>
        /// 百度翻译
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
        /// Azure Translator Bing翻译并输出翻译后的文本 用的MSDN样例修改
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


    }
}
