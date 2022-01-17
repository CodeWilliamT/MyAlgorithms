using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ToolKits.FunctionModule
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public class CommonMethod
    {
        #region 窗体操作
        /// <summary>
        /// 在控件中显示Form
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="pal"></param>
        /// <param name="Disposed"></param>
        public static void ShowFormInPanel(Form frm, Panel pal, bool Disposed)
        {
            ShowFormInPanel(frm, pal, false, Disposed);
        }
        /// <summary>
        /// 在控件中显示Form
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="pal"></param>
        /// <param name="refresh">是否强制重新载入frm到pal</param>
        /// <param name="Disposed">是否需要释放资源</param>
        public static void ShowFormInPanel(Form frm, Panel pal, bool refresh, bool Disposed)
        {
            //如果控件中含有该窗体，直接返回
            if (!refresh && (pal.Controls.Contains(frm) || pal.HasChildren ? frm.Text == pal.Controls[0].Text : false))
                return;

            ClearPanel(pal, Disposed);
            if (frm != null && !frm.IsDisposed)
            {
                frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.TopLevel = false;
                frm.Parent = pal;
                frm.Show();
            }
        }
        /// <summary>
        /// 在控件中显示Form
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="pal"></param>
        public static void ShowFormInPanel(Form frm, Panel pal)
        {
            ShowFormInPanel(frm, pal, true);
        }

        /// <summary>
        /// 移除控件中的子控件
        /// </summary>
        /// <param name="pal"></param>
        /// <param name="Disposed"></param>
        public static void ClearPanel(Panel pal, bool Disposed)
        {
            if (pal.HasChildren)
            {
                if (Disposed)
                {
                    //foreach (Control ctl in pal.Controls)
                    //    if (ctl != null && (ctl is Form) && !((ctl is TestMainFrm) || (ctl is ChipMgrDialog)))
                    //        ((Form)ctl).Close();
                    //    else
                    //        pal.Controls.Clear();
                }
                else
                    pal.Controls.Clear();
            }
        }
        #endregion

        #region 控件居中

        /// <summary>
        /// 将一控件相对于另一个参考控件在垂直方向上居中
        /// </summary>
        /// <param name="referenceCtrl"></param>
        /// <param name="destCtrl"></param>
        public static void VerticalCenterControl(Control referenceCtrl, Control destCtrl)
        {
            int offsetY = (referenceCtrl.Height - destCtrl.Height) / 2;
            destCtrl.Location = new System.Drawing.Point(destCtrl.Location.X, referenceCtrl.Location.Y + offsetY);
        }
        /// <summary>
        /// 将一控件相对于另一个参考控件在水平方向上居中
        /// </summary>
        /// <param name="referenceCtrl"></param>
        /// <param name="destCtrl"></param>
        public static void HorizonCenterControl(Control referenceCtrl, Control destCtrl)
        {
            int offsetX = (referenceCtrl.Width - destCtrl.Width) / 2;
            destCtrl.Location = new System.Drawing.Point(referenceCtrl.Location.X + offsetX, destCtrl.Location.Y);
        }
        #endregion

        #region 控件处理
        /// <summary>
        /// 释放图像占用的资源
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static bool ReleaseImageResource(ref System.Drawing.Image image)
        {
            try
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ReleasePictureBox(ref PictureBox ctrl)
        {
            return ReleasePictureBox(ref ctrl, true);
        }
        public static bool ReleasePictureBox(ref PictureBox ctrl, bool isDisposedCtrl)
        {
            try
            {
                if (ctrl != null && !ctrl.IsDisposed)
                {
                    if (ctrl.Image != null)
                    {
                        ctrl.Image.Dispose();
                        ctrl.Image = null;
                    }
                    if (isDisposedCtrl)
                    {
                        ctrl.Dispose();
                        ctrl = null;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }                            
        /// <summary>
        /// 控件本身不会释放
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static bool DisposeControl(Control ctrl)
        {
            try
            {
                DisposeControl(ctrl, false);
            }
            catch
            {
                return false;

            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="DisposeContainer">是否销毁容器（图片及其占用的图片资源会一起释放）</param>
        public static void DisposeControl(Control ctrl, bool DisposeContainer)
        {
            if (ctrl == null)
                return;
            if (ctrl.HasChildren)
            {
                foreach (Control tmp in ctrl.Controls)
                {
                    DisposeControl(tmp, true);
                }
            }
            else if (ctrl is PictureBox)
            {
                PictureBox it = (PictureBox)ctrl;
                ReleasePictureBox(ref it, DisposeContainer);
            }
            else
            {
                if (DisposeContainer && !ctrl.IsDisposed)
                    ctrl.Dispose();
            }
            if (ctrl != null && !ctrl.IsDisposed && ctrl.HasChildren)
                ctrl.Controls.Clear();
        }
        /// <summary>
        /// 更新控件信息用
        /// </summary>
        private delegate void UpdateEnableDelegate(Control ctrl, bool enable);
        /// <summary>
        /// 设置控制使能状态
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="enable"></param>
        public static void Dis_EnableBtn(Control ctrl, bool enable)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new UpdateEnableDelegate(Dis_EnableBtn), ctrl, enable);
            }
            else
            {
                ctrl.Enabled = enable;
            }
        }
        public static void Dis_EnableVisible(Control ctrl, bool enable)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new UpdateEnableDelegate(Dis_EnableVisible), ctrl, enable);
            }
            else
            {
                ctrl.Visible = enable;
            }
        }
        private delegate void UpdatePicbDelegate(PictureBox picb, System.Drawing.Image img);
        /// <summary>
        /// 设置PictureBox的Image属性
        /// </summary>
        /// <param name="picb"></param>
        /// <param name="img"></param>
        public static void UpdatePicbImg(PictureBox picb, System.Drawing.Image img)
        {
            if (picb.InvokeRequired)
            {
                picb.Invoke(new UpdatePicbDelegate(UpdatePicbImg), picb, img);
            }
            else
            {
                ReleasePictureBox(ref picb, false);

                picb.Image = img;
            }
        }
        private delegate void UpdateTxtDelegate(Control ctrl, string str);
        /// <summary>
        /// 设置状态信息
        /// </summary>
        public static void UpdateControlTxt(Control ctrl, string str)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new UpdateTxtDelegate(UpdateControlTxt), ctrl, str);
            }
            else
            {
                ctrl.Text = str;
            }
        }
        public static void SetNumericUpDown(NumericUpDown numCtrl, decimal numValue)
        {
            numCtrl.Value = numValue > numCtrl.Maximum ? numCtrl.Maximum : (numValue < numCtrl.Minimum ? numCtrl.Minimum : numValue);
        }
        #endregion

        #region 字符串处理
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
            string msg = new string(chars);
            str = msg.Substring(0, len);
            return str;
        }

        /// <summary>
        /// 将列表中的数据通过-号连接，并返回连接后的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static string GetString<T>(List<T> lst)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lst.Count; i++)
            {
                sb.Append(lst[i]);
                if (i < lst.Count - 1)
                    sb.Append("-");
            }
            return sb.ToString();
        }
        #endregion

        #region InitDllDirectory

        public static void InitDllDirectory()
        {
            if (_initDllDirectoryDone) return;

            SetDllDirectory(IntPtr.Size == 8 ? "x64" : "x86"); // In .NET 4 you can use Environment.Is64BitProcess

            _initDllDirectoryDone = true;
        }

        private static bool _initDllDirectoryDone = false;

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        #endregion
    }
}

