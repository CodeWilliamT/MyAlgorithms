using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolKits.FunctionModule;
using HalconDotNet;

namespace ToolKits.TemplateEdit.MatchTmpEdit
{
    public partial class RotateForm : BaseForm
    {
        public RotateForm()
        {
            InitializeComponent();
        }
        MainTemplateForm mtFrm = MainTemplateForm.GetMainTemplateForm();
        private void btnExRotate_Click(object sender, EventArgs e)
        {
            if (mtFrm.htWindow.RegionType != "Line")
            {
                MessageBox.Show("请选择画直线按钮重新画一条直线！","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            HObject image = new HObject();
            HOperatorSet.GenEmptyObj(out image);
            image.Dispose();
            Vision.RotateImage(mtFrm.htWindow.Image, ref image,
                               mtFrm.htWindow.Row1, mtFrm.htWindow.Column1,
                               mtFrm.htWindow.Row2, mtFrm.htWindow.Column2);
            mtFrm.htWindow.Image = image.CopyObj(1, -1);
            mtFrm.htWindow.ClearHWindow();
            mtFrm.htWindow.RefreshWindow(mtFrm.htWindow.Image, null, "");
            image.Dispose();
        }
    }
}
