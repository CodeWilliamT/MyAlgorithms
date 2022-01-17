using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTM_interface
{
    public partial class Form_AxisConfig : Form
    {
        public Form_AxisConfig()
        {
            InitializeComponent();
        }
        Form_HTM form_htm = null;
        /// <summary>
        /// 根据选中的轴记录编号构建窗体
        /// </summary>
        /// <param name="index_axis">选中的轴记录编号</param>
        public Form_AxisConfig(int index_axis)
        {
            InitializeComponent();
            form_htm = Form_HTM.Get_Form();
            this.textBox_axisName.Text = form_htm.axis_data[index_axis].axisName;
            foreach(Control ctl_main in this.Controls)
            {
                string Type_ctl_main = ctl_main.GetType().ToString();
                if (Type_ctl_main == "System.Windows.Forms.Panel")
                {
                    foreach (Control ctl_part in ((Panel)ctl_main).Controls)
                    {
                        string Type_ctl_part = ctl_part.GetType().ToString();
                        if (Type_ctl_part == "System.Windows.Forms.ComboBox")
                        {
                            ComboBox cbbox = (ComboBox)ctl_part;
                            cbbox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                ///保存当前轴信息
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }
    }
}
