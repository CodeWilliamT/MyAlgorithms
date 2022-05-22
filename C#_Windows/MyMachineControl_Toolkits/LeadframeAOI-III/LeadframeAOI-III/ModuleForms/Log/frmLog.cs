using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using HT_Lib;

namespace LeadframeAOI
{
    public partial class frmLog : Form
    {

        public frmLog()
        {
            InitializeComponent();
            HTLog.Ui ui = new HTLog.Ui(this);
        }
    }
}
