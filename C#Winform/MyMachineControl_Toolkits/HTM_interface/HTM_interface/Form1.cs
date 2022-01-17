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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Form_HTM form_HTM;
        private void button1_Click(object sender, EventArgs e)
        {
            form_HTM = new Form_HTM();
            form_HTM.ShowDialog();
        }
    }
}
