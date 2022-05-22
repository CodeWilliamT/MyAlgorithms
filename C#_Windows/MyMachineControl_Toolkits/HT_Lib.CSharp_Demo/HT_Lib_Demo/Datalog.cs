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
    public partial class Datalog : Form
    {
        
        public Datalog()
        {
            InitializeComponent();
            HTLog.Ui ui = new HTLog.Ui(this);
        }
    }
}
