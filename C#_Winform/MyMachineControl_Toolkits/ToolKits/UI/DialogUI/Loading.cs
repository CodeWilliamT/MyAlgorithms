using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ToolKits.FunctionModule;

namespace ToolKits.UI.DialogUI
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置显示的图像
        /// </summary>
        /// <param name="img"></param>
        public void SetLoadingIMG(Image img)
        {
            CommonMethod.UpdatePicbImg(picbLoading, img);
        }

        /// <summary>
        /// 设置状态信息
        /// </summary>
        /// <param name="str"></param>
        private void SetLoadingStatus(string str)
        {
            CommonMethod.UpdateControlTxt(lbltxt, str);
        }

        /// <summary>
        /// 显示的文字(线程安全)
        /// </summary>
        public new string Text
        {
            get
            {
                string str;
                GetLoadingStatus(out str);
                return str;
            }
            set
            {
                SetLoadingStatus(value);
            }
        }

        private delegate void getTxt(out string str);

        /// <summary>
        /// 设置状态信息
        /// </summary>
        /// <param name="str"></param>
        private void GetLoadingStatus(out string str)
        {

            if (this.InvokeRequired)
            {
                //setTxt s = new setTxt(SetTxt);
                //textBox1.Invoke(s,str);
                str = "";
                this.Invoke(new getTxt(GetLoadingStatus), str);
            }
            else
            {
                str = lbltxt.Text;
            }
        }
    }
}
