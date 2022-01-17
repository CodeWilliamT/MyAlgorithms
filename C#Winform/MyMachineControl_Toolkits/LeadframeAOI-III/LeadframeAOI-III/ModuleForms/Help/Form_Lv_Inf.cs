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
    public partial class Form_Lv_Inf : Form
    {
        public Form_Lv_Inf()
        {
            InitializeComponent();
            textBox_Lv_program.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
