using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPort_Driver;
namespace Demo
{
    public partial class Form1 : Form
    {
        EC_JET ec_Jet;
        public Form1()
        {
            InitializeComponent();
            string[] sPorts=System.IO.Ports.SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach (string s in sPorts)
            {
                comboBox1.Items.Add(s);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            ec_Jet = new EC_JET(comboBox1.SelectedItem.ToString().Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ec_Jet.TriggerPrint();
        }
    }
}
