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
namespace WindowsFormsApp1
{
    public partial class Motion : Form
    {
        public Motion()
        {
            InitializeComponent();
        }

        private void button_Init_Click(object sender, EventArgs e)
        {
            var init = new HTM.INIT_PARA()
            {
                 offline_mode=1,
                 max_axis_num = 8,
                 max_io_num = 18,
                 max_dev_num = 2,
                 para_file = "paras.db"              
            };
            if (HTM.Init(ref init) < 0)
            {
                HTUi.TipError("初始化失败!");
            }
        }

        private void button_LoadUI_Click(object sender, EventArgs e)
        {
            HTM.LoadUI(this.panel1);
        }

        private void button_Discard_Click(object sender, EventArgs e)
        {
            HTM.Discard();
        }

        private void button_Pop_Click(object sender, EventArgs e)
        {
            HTM.LoadUI(null);
        }

        private void button_LoadToolUi_Click(object sender, EventArgs e)
        {
            HTM.LoadToolUI(null);
        }
    }
}
