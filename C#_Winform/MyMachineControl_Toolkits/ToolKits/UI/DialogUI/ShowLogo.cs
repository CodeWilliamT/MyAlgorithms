using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolKits.UI.DialogUI
{
    /// <summary>
    /// 显示Logo并检查是否授权
    /// </summary>
    public partial class ShowLogo : Form
    {
        DialogResult dialogResult;
        public event EventHandler InitSystem;
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

            dialogResult = this.DialogResult;

            return dialogResult;
        }
        /// <summary>
        /// 1s后关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowLogo_Shown(object sender, EventArgs e)
        {
            //#if false
            try
            {
                status_Text.Visible = true;
                status_Text.Text = "系统正在加载产品...";


                /*
                if (!Wayrise.WR800.Tool.LicenseValidate.JudgeLicenseValidity(Wayrise.WR800.UI.Method.Directory_Info.DirLicense + "license.lic"))
                {
                    lblStatus.Text = "许可证文件已过期或无效!";
                    MessageBox.Show("许可证文件已过期或无效!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dialogResult = System.Windows.Forms.DialogResult.Abort;
                    this.Close();
                    //Application.Exit();
                 //System.Environment.Exit(0);
                }
                else
                 */

                {
                    //status_Text.Text = "正在初始化设备...";
                    //初始化设备
                    //ProcessMgr.Instance.Init();
                    //status_Text.Text = "正在执行开机自检...";
                    //用于表示指针或句柄的平台特定类型。
                    //IntPtr formPtr = this.Handle;

                    //开机自检
                    System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        InitSystem(this, e);
                        System.Threading.Thread.Sleep(1000);
                    });
                    var initTreeViewTask = task.ContinueWith((t) =>
                    {
                        this.Close();
                    }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Soft Occur Error!\r\nError Info：\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dialogResult = System.Windows.Forms.DialogResult.Abort;
                this.Close();
            }
            //#endif
        }
    }
}
