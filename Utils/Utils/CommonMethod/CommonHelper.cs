using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class CommonHelper
    {
        /// <summary>
        /// 计算耗时
        /// </summary>
        /// <param name="previousTime">起始时间</param>
        /// <returns>返回时间（单位：毫秒）</returns>
        public static double timeSpan(DateTime startTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = now.Subtract(startTime);
            return ts.TotalMilliseconds;
        }
        /// <summary>
        /// 字符数组转字符串
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string CharToString(char[] chars)
        {
            string str = "";
            int len = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\0')
                {
                    len = i;
                    break;
                }
            }
            //len = (len == 0) ? 0 : len;

            string msg = new string(chars);
            str = msg.Substring(0, len);
            return str;
        }
        /// <summary>
        /// 十进制转二进制，这个主要用于错误码解析，解析出的二进制顺序与正常顺序恰好相反
        /// </summary>
        /// <param name="value"></param>
        /// <param name="binValue"></param>
        public static void IntToBinary(int value, ref int[] binValue)
        {
            string bin = Convert.ToString(value, 2);
            int len = bin.Length;
            if (len < 1) return;
            for (int i = 0; i < len; i++)
            {
                binValue[i] = Convert.ToInt32(bin[len - 1 - i].ToString());
            }
        }
        /// <summary>
        /// 根据图像大小等比例缩放图像控件
        /// </summary>
        /// <param name="parentCtrl">父级控件</param>
        /// <param name="imageCtrl">图像控件</param>
        /// <param name="imageWidth">图像宽</param>
        /// <param name="imageHeight">图像高</param>
        /// <param name="widthBlank">父级控件右侧至少保留的宽度</param>
        /// <param name="heightBlank">父级控件下侧至少保留的高度</param>
        public static void ControlResize(Control parentCtrl, Control imageCtrl, int imageWidth, int imageHeight, int widthBlank, int heightBlank)
        {
            if (imageWidth > imageHeight)
            {
                double factor = imageWidth / (imageHeight * 1.0);
                imageCtrl.Width = ((parentCtrl.Height - heightBlank) * factor > parentCtrl.Width - widthBlank) ? (parentCtrl.Width - widthBlank) : (Convert.ToInt32((parentCtrl.Height - heightBlank) * factor));
                imageCtrl.Height = (parentCtrl.Height - heightBlank > imageCtrl.Width / factor) ? (Convert.ToInt32(imageCtrl.Width / factor)) : (parentCtrl.Height - heightBlank);
            }
            else
            {
                double factor = imageHeight / (imageWidth * 1.0);
                imageCtrl.Height = ((parentCtrl.Width - widthBlank) * factor > parentCtrl.Height - heightBlank) ? (parentCtrl.Height - heightBlank) : (Convert.ToInt32((parentCtrl.Width - widthBlank) * factor));
                imageCtrl.Width = (parentCtrl.Width - widthBlank > imageCtrl.Height / factor) ? (Convert.ToInt32(imageCtrl.Height / factor)) : (parentCtrl.Width - widthBlank);
            }
        }
    }
}
