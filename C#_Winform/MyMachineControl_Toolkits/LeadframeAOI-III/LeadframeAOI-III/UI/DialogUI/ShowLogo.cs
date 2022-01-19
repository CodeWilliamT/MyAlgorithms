using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using LeadframeAOI_III;
using LeadframeAOI.UI.ControlWrap;

namespace LeadframeAOI.UI
{
    /// <summary>
    /// 显示Logo并检查是否授权
    /// </summary>
    public partial class ShowLogo : Form
    {
        DialogResult dialogResult;

        public delegate bool SystemInitDel(ShowLogo frm);
        public event SystemInitDel SystemInitEvent;
       
        public ShowLogo()
        {
            InitializeComponent();
            dialogResult = System.Windows.Forms.DialogResult.None;
            //绑定事件，只要窗体是首次显示就发生。异步执行
            this.Shown += new EventHandler(ShowLogo_Shown);
        }
        
        public new DialogResult ShowDialog()
        {
            //将窗体显示为模式对话框，并将当前活动窗口设置为它的所有者。
            base.ShowDialog();
            return dialogResult;
        }
        private delegate void ShowInfoDel(string info);
        public void ShowInfo(string info)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowInfoDel(ShowInfo), new object[] { info });
            }
            else
            {
                rtbInfoView.AppendText(info + "\r\n");
                rtbInfoView.Select(rtbInfoView.Text.Length, 0);
                rtbInfoView.ScrollToCaret();
            }
        }
        /// <summary>
        /// 1s后关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        protected void ShowLogo_Shown(object sender, EventArgs e)
        {
            try
            {
                //开机初始化
                Task<bool> load = Task.Factory.StartNew<bool>(() =>
                {
                    if (SystemInitEvent != null)
                    {
                        return SystemInitEvent(this);
                    }
                    else
                    {
                        return false;
                    }
                });
                Task task = load.ContinueWith((tResult) =>
                {
                    dialogResult = tResult.Result ? System.Windows.Forms.DialogResult.OK : System.Windows.Forms.DialogResult.Abort;
                    this.Close();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Soft Occur Error!\r\nError Info：\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dialogResult = System.Windows.Forms.DialogResult.Abort;
                this.Close();
            }
        }
    }
}
