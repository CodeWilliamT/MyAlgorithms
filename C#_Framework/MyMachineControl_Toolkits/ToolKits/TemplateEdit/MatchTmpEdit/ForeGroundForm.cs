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
    public partial class ForeGroundForm : BaseForm
    {
        public ForeGroundForm()
        {
            InitializeComponent();
            this.colAcceptMode.Items.AddRange(mtFrm.acceptMode);
        }

        MainTemplateForm mtFrm = MainTemplateForm.GetMainTemplateForm();

        private void btnExAdd_Click(object sender, EventArgs e)
        {
            if (mtFrm.htWindow.RegionType == "")
            {
                MessageBox.Show("未知的区域类型！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowInd = this.dgvxDrawInfo.Rows.Add();
            this.dgvxDrawInfo.Rows[rowInd].Cells[0].Value = mtFrm.drawTypeImg[mtFrm.htWindow.RegionType];
            this.dgvxDrawInfo.Rows[rowInd].Cells[1].Value = mtFrm.acceptMode[0];
            //更新数据
            MainTemplateForm.VaryHobject vCont = new MainTemplateForm.VaryHobject();
            vCont.Dispose();
            try
            {
                Vision.GenROI(mtFrm.htWindow, "contour", ref vCont.obj);
                vCont.type = mtFrm.acceptMode[0];// this.dgvxDrawInfo.Rows[rowInd].Cells[1].FormattedValue.ToString();
                if (mtFrm.mte_fGrdRegValues.Count <= rowInd + 1)
                    mtFrm.mte_fGrdRegValues.Add(vCont);
                else
                {
                    mtFrm.mte_fGrdRegValues[rowInd].Dispose();
                    mtFrm.mte_fGrdRegValues[rowInd] = vCont;
                }
            }
            catch (Exception ex)
            {
                vCont.Dispose();
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvxDrawInfo.RowCount < 1) return;
            int curRowInd = this.dgvxDrawInfo.CurrentCell.RowIndex;
            this.dgvxDrawInfo.Rows.RemoveAt(curRowInd);
            mtFrm.mte_fGrdRegValues[curRowInd].Dispose();
            mtFrm.mte_fGrdRegValues.RemoveAt(curRowInd);
        }

        private void dgvxDrawInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2) return;
            //显示轮廓
            HOperatorSet.DispObj(mtFrm.mte_fGrdRegValues[e.RowIndex].obj, mtFrm.htWindow.HTWindow.HalconWindow);
        }

        private void dgvxDrawInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dgvxDrawInfo.CurrentCell.ColumnIndex != 1) return;
            try
            {
                ComboBox cb = e.Control as ComboBox;
                cb.SelectedIndexChanged -= cb_SelectedIndexChanged;
                cb.SelectedIndexChanged += cb_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ComboBox cb = (ComboBox)sender;
            //更新区域的接收类型
            mtFrm.mte_fGrdRegValues[this.dgvxDrawInfo.CurrentCell.RowIndex].type = mtFrm.acceptMode[cb.SelectedIndex];
        }

        private void ForeGroundForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;
            if (mtFrm.mte_TmpPrmValues.Type != 3)
            {
                this.Enabled = false;
            }
            else if(mtFrm.mte_TmpPrmValues.IsCreateTmp)
            {
                this.Enabled = true;
            }
        }
    }
}
