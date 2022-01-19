using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolKits.TemplateEdit.MatchTmpEdit
{
    public partial class TmpPrmForm : BaseForm
    {
        public TmpPrmForm()
        {
            InitializeComponent();
            this.cbModelType.Items.Clear();
            this.cbModelType.Items.AddRange(mtFrm.tmpMode);
            this.cbCreateTmp.Checked = mtFrm.mte_TmpPrmValues.IsCreateTmp;
            this.cbModelType.SelectedIndex = 0;
            this.numAngleStart.Value = Convert.ToDecimal(mtFrm.mte_TmpPrmValues.AngleStart);
            this.numAngleExtent.Value = Convert.ToDecimal(mtFrm.mte_TmpPrmValues.AngleExtent);
            this.numScore.Value = Convert.ToDecimal(mtFrm.mte_TmpPrmValues.Score);
        }
        MainTemplateForm mtFrm = MainTemplateForm.GetMainTemplateForm();
        private void cbModelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mtFrm.mte_TmpPrmValues.Type = this.cbModelType.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numAngleStart_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                mtFrm.mte_TmpPrmValues.AngleStart = Convert.ToDouble(numAngleStart.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numAngleExtent_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                mtFrm.mte_TmpPrmValues.AngleExtent = Convert.ToDouble(numAngleExtent.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numScore_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                mtFrm.mte_TmpPrmValues.Score = Convert.ToDouble(numScore.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void nudNumLevel_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                mtFrm.mte_TmpPrmValues.NumLevel = Convert.ToInt32(nudNumLevel.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cbCreateTmp_CheckedChanged(object sender, EventArgs e)
        {
            mtFrm.mte_TmpPrmValues.IsCreateTmp = cbCreateTmp.Checked;
            gbTmpPrm.Enabled = cbCreateTmp.Checked;
        }

    }
}
