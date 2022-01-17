using System;
using System.Windows.Forms;
using HT_Lib;

namespace LeadframeAOI
{
    public partial class frmScanner : Form
    {
        private Boolean manualCode;
        public String ManualQRCode;

        public Boolean ManualCode
        {
            get { return manualCode; }
        }
        private Boolean scanCode;
        public Boolean ScanCode
        {
            get { return scanCode; }
        }
        public frmScanner()
        {
            InitializeComponent();
            manualCode = false;
            scanCode = false;

        }
        private void btnManual_Click(object sender, EventArgs e)
        {
            if (txtQRCode.Text == String.Empty)
            {
                HTUi.PopError("Frame产品二维码不能为空");
                return;
            }
            manualCode = true;
            ManualQRCode = txtQRCode.Text;
            this.Close();
        }

        private void butScan_Click(object sender, EventArgs e)
        {
            scanCode = true;
        }
    }
}
