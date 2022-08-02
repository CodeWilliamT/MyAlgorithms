using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace ToolKits.TemplateEdit.Measure2DTmpEdit
{
    public partial class Measure2DDistPointForm : BaseForm
    {
        public Measure2DDistPointForm()
        {
            InitializeComponent();
        }
        ComboBox cb;
        MainTemplateForm mtFrm = MainTemplateForm.GetMainTemplateForm();
        private void Measure2DDistPointForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;
            //根据上一步测量区域数量更新点对表格
            DataGridViewComboBoxColumn[] dgvCbCol = { Msr2DDistPoint1, Msr2DDistPoint2 };
            for (int i = 0; i < dgvCbCol.Length; i++)
            {
                dgvCbCol[i].Items.Clear();
                for (int j = 0; j < mtFrm.mte_Msr2DRegValues.Count; j++)
                {
                    dgvCbCol[i].Items.Add(j.ToString());
                }
            }
            for (int i = 0; i < this.dgvMsr2DDistPoints.RowCount; i++)
            {
                if (mtFrm.mte_Msr2DDistPt1[i].I >= Msr2DDistPoint1.Items.Count)
                {
                    mtFrm.mte_Msr2DDistPt1[i] = -1;
                }
                else
                    this.dgvMsr2DDistPoints.Rows[i].Cells[1].Value = (mtFrm.mte_Msr2DDistPt1[i] < 0) ? "" : mtFrm.mte_Msr2DDistPt1[i].I.ToString();
                if (mtFrm.mte_Msr2DDistPt2[i].I >= Msr2DDistPoint2.Items.Count)
                {
                    mtFrm.mte_Msr2DDistPt2[i] = -1;
                }
                else
                    this.dgvMsr2DDistPoints.Rows[i].Cells[2].Value = (mtFrm.mte_Msr2DDistPt2[i] < 0) ? "" : mtFrm.mte_Msr2DDistPt2[i].I.ToString();
            }
        }

        private void btnExAdd_Click(object sender, EventArgs e)
        {
            int rowInd = this.dgvMsr2DDistPoints.Rows.Add();
            this.dgvMsr2DDistPoints.Rows[rowInd].Cells[0].Value = rowInd.ToString();
            //this.dgvMsr2DDistPoints.Rows[rowInd].Cells[1].Value = -1;
            //this.dgvMsr2DDistPoints.Rows[rowInd].Cells[2].Value = -1;
            mtFrm.mte_Msr2DDistPt1.Append(new HTuple(-1));
            mtFrm.mte_Msr2DDistPt2.Append(new HTuple(-1));
        }

        private void btnExDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvMsr2DDistPoints.RowCount < 1) return;
            int curRowInd = this.dgvMsr2DDistPoints.CurrentCell.RowIndex;
            this.dgvMsr2DDistPoints.Rows.RemoveAt(curRowInd);
            mtFrm.mte_Msr2DDistPt1.TupleRemove(curRowInd);
            mtFrm.mte_Msr2DDistPt2.TupleRemove(curRowInd);
        }
        private void dgvMsr2DDistPoints_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int colInd = this.dgvMsr2DDistPoints.CurrentCell.ColumnIndex;
            if (colInd == 0) return;
            try
            {
                cb = e.Control as ComboBox;
                if (colInd == 1)
                {
                    cb.SelectedIndexChanged += cb1_SelectedIndexChanged;
                }
                else
                {
                    cb.SelectedIndexChanged += cb2_SelectedIndexChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ComboBox cb = (ComboBox)sender;
            //更新测距第一点索引
            mtFrm.mte_Msr2DDistPt1[this.dgvMsr2DDistPoints.CurrentCell.RowIndex] = Convert.ToInt32(cb.SelectedItem.ToString());
            cb.SelectedIndexChanged -= cb1_SelectedIndexChanged;
        }
        void cb2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ComboBox cb = (ComboBox)sender;
            //更新测距第二点索引
            mtFrm.mte_Msr2DDistPt2[this.dgvMsr2DDistPoints.CurrentCell.RowIndex] = Convert.ToInt32(cb.SelectedItem.ToString());
            cb.SelectedIndexChanged -= cb2_SelectedIndexChanged;
        }
    }
}
