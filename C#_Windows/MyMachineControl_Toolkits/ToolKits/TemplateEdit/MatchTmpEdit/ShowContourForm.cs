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
    public partial class ShowContourForm : BaseForm
    {
        public ShowContourForm()
        {
            InitializeComponent();
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
            //更新数据
            HObject cont = new HObject();
            HOperatorSet.GenEmptyObj(out cont);
            try
            {
                cont.Dispose();
                Vision.GenROI(mtFrm.htWindow, "contour", ref cont);
                if (mtFrm.mte_shwContValues.Count <= rowInd + 1)
                    mtFrm.mte_shwContValues.Add(cont);
                else
                {
                    mtFrm.mte_shwContValues[rowInd].Dispose();
                    mtFrm.mte_shwContValues[rowInd] = cont;
                }
            }
            catch (Exception ex)
            {
                cont.Dispose();
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvxDrawInfo.RowCount < 1) return;
            int curRowInd = this.dgvxDrawInfo.CurrentCell.RowIndex;
            this.dgvxDrawInfo.Rows.RemoveAt(curRowInd);
            mtFrm.mte_shwContValues[curRowInd].Dispose();
            mtFrm.mte_shwContValues.RemoveAt(curRowInd);
        }

        private void dgvxDrawInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            //显示轮廓
            HOperatorSet.DispObj(mtFrm.mte_shwContValues[e.RowIndex], mtFrm.htWindow.HTWindow.HalconWindow);
        }

        private void ShowContourForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;
            this.Enabled = mtFrm.mte_TmpPrmValues.IsCreateTmp;
        }

    }
}
