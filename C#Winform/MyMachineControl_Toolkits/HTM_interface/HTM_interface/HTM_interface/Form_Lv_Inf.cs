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
    public partial class Form_Lv_Inf : Form
    {
        Form_HTM form_htm = null;

        public Form_Lv_Inf()
        {
            InitializeComponent();
            form_htm = Form_HTM.Get_Form();
            textBox_Lv_program.Text = form_htm.Lv_program;
        }
    }
}
