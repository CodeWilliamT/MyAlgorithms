
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HT_Lib;


namespace LeadframeAOI
{
    public partial class ParaTest : Form
    {
        public static ParaTest Instance = null;
        public ParaTest()
        {
            InitializeComponent();
            Instance = this;
        }
        public void SetupUI()
        {
            try
            {
                //tabControl1.TabPages.Add("复看");
                //API.LoadUI_Review(tabControl1.TabPages[tabControl1.TabPages.Count-1]);
                //ConfigUI frmConfigUI = new ConfigUI();
                //AddForm2Tab(frmConfigUI, tabControl1, "模板模块配置");
                //ModelCreateUI frmModelCreateUI = new ModelCreateUI();
                //AddForm2Tab(frmModelCreateUI, tabControl1, "模板制作");
                //HTV_Algorithm.ParaUI frmParaUI = new ParaUI();
                //AddForm2Tab(frmParaUI, tabControl1, "参数配置");

                //FrmInspectPara frmInspectPara = new FrmInspectPara();
                //AddForm2Tab(frmInspectPara, tabControl1, "检测参数设置");
            }
            catch (Exception ex)
            {
                HTUi.PopError("模板制作模块加载失败\n" + ex.ToString());
            }
        }
        private void AddForm2Tab(Form frm, TabControl tabControl, string tabTest)
        {
            TabPage tbpg = new TabPage();
            tbpg.Text = tabTest;
            frm.TopLevel = false;
            tbpg.Controls.Add(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.Visible = true;
            tabControl.TabPages.Add(tbpg);
        }
    }
}
