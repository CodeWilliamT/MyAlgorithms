using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolKits.UI.DialogUI
{
    public partial class Confirm : Form
    {
        private static DialogResult result = DialogResult.Cancel;
        private static string psd;
        private static string labelInfo;

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            set { psd = value; }
        }

        /// <summary>
        /// 功能模式0--为普通输入模式，1--为密码框模式
        /// </summary>
        public static int FuncMode
        {
            set;
            get;
        }

        /// <summary>
        /// 文本框提示信息
        /// </summary>
        public static string LabelInfo
        {
            set { labelInfo = value; }
            get { return labelInfo; }
        }

        /// <summary>
        /// 窗体表示
        /// </summary>
        public static string TextTitle
        {
            set;
            get;
        }

        public string ValidInfo
        {
            get
            {
                if (txtPsd.Text.Trim() == "")
                    return "";
                else
                    return txtPsd.Text.Trim(); 
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Confirm()
        {
            InitializeComponent();

            label1.Text = LabelInfo;
            this.Text = TextTitle;

            if(FuncMode == 1)
            {
                txtPsd.UseSystemPasswordChar = true;
            }
            else
            {
                txtPsd.UseSystemPasswordChar = false;
            }
        }

        /// <summary>
        /// 以独占的方式显示自己，如果结果正确返回DialogResult.OK
        /// </summary>
        /// <returns></returns>
        //public static new DialogResult ShowDialog()
        //{
        //    Confirm conf = new Confirm();
        //    ((Form)conf).ShowDialog();
        //    return result;
        //}
        public new System.Windows.Forms.DialogResult ShowDialog()
        {
            this.ShowDialog(null);
            return result;
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (FuncMode == 0)
            {
                if (txtPsd.Text.Trim() == null || txtPsd.Text.Trim() == "")
                {
                    MessageBox.Show("Please Input The Conrrect Text", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = DialogResult.Cancel;
                }
                else
                {
                    if (Regex.IsMatch(txtPsd.Text, "[\\/:*?\"<>|]"))
                    {
                        //toolTip1.SetToolTip(txtPsd, "Product Name Can Not Contain One Of The Following Char:\r\n     \\ / : * ? \" < > |     ");
                        MessageBox.Show("Product Name Can Not Contain One Of The Following Char:\r\n     \\ / : * ? \" < > |     ", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        result = DialogResult.Cancel;
                    }
                    else
                    {
                        result = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            else
            {
                if (txtPsd.Text.Trim() == psd)
                {
                    result = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please Connect The Machine Author", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = DialogResult.Cancel;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 判断输入信息是否正确
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPsd_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtPsd.Text, "[\\/:*?\"<>|]"))
            {
                toolTip1.SetToolTip(txtPsd, "Product Name Can Not Contain One Of The Following Char:\r\n     \\ / : * ? \" < > |     ");
                toolTip1.Active = true;
            }
            else
            {
                toolTip1.SetToolTip(txtPsd, "");
            }

        }
    }
}
