using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeadframeAOI.UI
{
    public partial class About : Form
    {
        bool isShowInnerVersion;

        public About()
        {
            InitializeComponent();
            isShowInnerVersion = false;
            this.Text = String.Format("关于 {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("高软版本 {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region 程序集特性访问器

        /// <summary>
        /// 程序集的标题
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// 高软版本
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;
                string strVer = "V" + ver.Major + ".R" + (ver.Minor > 9 ? ver.Minor.ToString() : "0" + ver.Minor) + ".C" + (ver.Build > 9 ? ver.Build.ToString() : "0" + ver.Build) + (isShowInnerVersion ? (".B" + (ver.Revision > 9 ? ver.Revision.ToString() : "0" + ver.Revision)) : "");
                return strVer;
            }
        }

        /// <summary>
        /// 版权
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }       
        #endregion

        private void labelVersion_Click(object sender, EventArgs e)
        {
            isShowInnerVersion = true;
            this.labelVersion.Text = String.Format("高软版本 {0}", AssemblyVersion);
        }

        private void lblUnderSoftVer_Click(object sender, EventArgs e)
        {
            lblUnderSoftVer.Text = String.Format("底软版本 {0}", "V100.R005"); ;
        }

        private void About_Load(object sender, EventArgs e)
        {

        }
    }
}
