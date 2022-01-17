using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeadframeAOI
{
    public partial class Form_InputCode2D : Form
    {
        public static Form_InputCode2D Instance = null;
        public static string Code2D = "";
        public Form_InputCode2D()
        {
            InitializeComponent();
            FrmCamAxisMotion frm = FrmCamAxisMotion.Instance;
            if (frm == null || frm.IsDisposed) frm = new FrmCamAxisMotion();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.Visible = true;
            panel2.Controls.Add(frm);
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Instance = this;
        }
        /// <summary>
        /// 显示本窗体的一个模态窗体实例
        /// </summary>
        public static void ShowDialogForm()
        {
            Form_InputCode2D frm = Form_InputCode2D.Instance;

            if (frm != null)
            {
                if (!frm.IsDisposed)
                {
                    frm.Dispose();
                }
            }
            Form_InputCode2D.Instance = new Form_InputCode2D();
            frm = Form_InputCode2D.Instance;
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            frm.Location = new Point((SW - frm.Size.Width) / 2, (SH - frm.Size.Height) / 2);
            frm.TopMost = true;
            frm.StartPosition = FormStartPosition.Manual;
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Code2D = textBox1.Text;
            if (Code2D == "")
            {
                MessageBox.Show("请输入二维码后再确认！");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
