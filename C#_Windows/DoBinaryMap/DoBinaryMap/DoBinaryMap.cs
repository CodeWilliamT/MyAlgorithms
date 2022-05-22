using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using System.Drawing;
using System.Drawing.Imaging; 

namespace DoBinaryMap
{
    class DoBinaryMap
    {
        /// <summary>
        /// 将文本转化为二维码
        /// </summary>
        /// <param name="text">文本</param>
        public static Bitmap Generate(string text)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = 500;
            options.Height = 500;
            //设置二维码的边距,单位不是固定像素
            options.Margin = 1;
            writer.Options = options;
            Bitmap map = writer.Write(text);
            return map;
        }
        /// <summary>
        /// 识别二维码图片，输出文本
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string Read(string filename)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            Bitmap map = new Bitmap(filename);
            Result result = reader.Decode(map);
            map.Dispose();
            return result == null ? "" : result.Text;
        }
    }
}
