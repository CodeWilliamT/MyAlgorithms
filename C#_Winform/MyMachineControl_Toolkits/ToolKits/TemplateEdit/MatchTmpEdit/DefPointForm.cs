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
    public partial class DefPointForm : BaseForm
    {
        public DefPointForm()
        {
            InitializeComponent();
            btnExShowLine1.Enabled = false;
            btnExShowLine2.Enabled = false;
        }
        HTuple line1_row1 = new HTuple(), line1_col1 = new HTuple(), line1_row2 = new HTuple(), line1_col2 = new HTuple();
        HTuple line2_row1 = new HTuple(), line2_col1 = new HTuple(), line2_row2 = new HTuple(), line2_col2 = new HTuple();
        HTuple auto_cal_row = new HTuple(), auto_cal_col = new HTuple();
        
        MainTemplateForm mtFrm = MainTemplateForm.GetMainTemplateForm();

        private void btnExGetLine1_Click(object sender, EventArgs e)
        {
            btnExShowLine1.Enabled = false;
            if (mtFrm.htWindow.RegionType != "Line")
            {
                MessageBox.Show("请选择画直线按钮重新画一条直线！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            line1_row1 = mtFrm.htWindow.Row1;
            line1_col1 = mtFrm.htWindow.Column1;
            line1_row2 = mtFrm.htWindow.Row2;
            line1_col2 = mtFrm.htWindow.Column2;
            btnExShowLine1.Enabled = true;
        }

        private void btnExGetLine2_Click(object sender, EventArgs e)
        {
            btnExShowLine2.Enabled = false;
            if (mtFrm.htWindow.RegionType != "Line")
            {
                MessageBox.Show("请选择画直线按钮重新画一条直线！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            line2_row1 = mtFrm.htWindow.Row1;
            line2_col1 = mtFrm.htWindow.Column1;
            line2_row2 = mtFrm.htWindow.Row2;
            line2_col2 = mtFrm.htWindow.Column2;
            btnExShowLine2.Enabled = true;
        }

        private void btnExShowLine1_Click(object sender, EventArgs e)
        {
            HObject line = new HObject();
            HOperatorSet.GenEmptyObj(out line);
            line.Dispose();
            HOperatorSet.GenContourPolygonXld(out line, line1_row1.TupleConcat(line1_row2), line1_col1.TupleConcat(line1_col2));
            HOperatorSet.DispObj(line, mtFrm.htWindow.HTWindow.HalconWindow);
            line.Dispose();
        }

        private void btnExShowLine2_Click(object sender, EventArgs e)
        {
            HObject line = new HObject();
            HOperatorSet.GenEmptyObj(out line);
            line.Dispose();
            HOperatorSet.GenContourPolygonXld(out line, line2_row1.TupleConcat(line2_row2), line2_col1.TupleConcat(line2_col2));
            HOperatorSet.DispObj(line, mtFrm.htWindow.HTWindow.HalconWindow);
            line.Dispose();
        }
        private void btnExAutoCalculate_Click(object sender, EventArgs e)
        {
            if (mtFrm.mte_shwContValues.Count == 0)
            {
                MessageBox.Show("没有用于自动计算中心点的轮廓信息！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HObject cont = new HObject();
            HOperatorSet.GenEmptyObj(out cont);
            HTuple rows=new HTuple(),cols=new HTuple();
            for (int i = 0; i < mtFrm.mte_shwContValues.Count; i++)
            {
                HTuple row,col;
                HOperatorSet.GetContourXld(mtFrm.mte_shwContValues[i], out row, out col);
                rows.Append(row);
                cols.Append(col);
            }
            cont.Dispose();
            HOperatorSet.GenContourPolygonXld(out cont, rows, cols);
            HTuple row1,col1,row2,col2;
            HOperatorSet.SmallestRectangle1Xld(cont, out row1, out col1, out row2, out col2);
            auto_cal_row = (row1.TupleConcat(row2)).TupleMean();
            auto_cal_col = (col1.TupleConcat(col2)).TupleMean();
            cont.Dispose();
        }
        private void btnExAdd_Click(object sender, EventArgs e)
        {
            if ( auto_cal_row.Length==0 && mtFrm.htWindow.RegionType != "Point" && (line1_row1.Length == 0 || line1_col1.Length == 0 ||
                line1_row2.Length == 0 || line1_col2.Length == 0 || line2_row1.Length == 0 || line2_col1.Length == 0 ||
                line2_row2.Length == 0 || line2_col2.Length == 0))
            {
                MessageBox.Show("未自动计算中心点，或区域类型非点类型，或没有有效的两条直线信息！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowInd = this.dgvxDrawInfo.Rows.Add();
            this.dgvxDrawInfo.Rows[rowInd].Cells[0].Value = mtFrm.drawTypeImg["Point"];
            //更新数据
            try
            {
                HTuple row, col, isParallel;
                if (auto_cal_row.Length != 0 && auto_cal_col.Length != 0)
                {
                    row = auto_cal_row;
                    col = auto_cal_col;
                    auto_cal_row = new HTuple();
                    auto_cal_col = new HTuple();
                }
                else if (mtFrm.htWindow.RegionType == "Point")
                {
                    row = mtFrm.htWindow.Row;
                    col = mtFrm.htWindow.Column;
                }
                else
                    HOperatorSet.IntersectionLl(line1_row1, line1_col1, line1_row2, line1_col2,
                                                line2_row1, line2_col1, line2_row2, line2_col2,
                                                out row, out col, out isParallel);
                if (mtFrm.mte_defPointValues.Count <= rowInd + 1)
                    mtFrm.mte_defPointValues.Add(new HTuple(new double[] { row, col }));
                else
                {
                    mtFrm.mte_defPointValues[rowInd] = new HTuple(new double[] { row, col });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnExDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvxDrawInfo.RowCount < 1) return;
            int curRowInd = this.dgvxDrawInfo.CurrentCell.RowIndex;
            this.dgvxDrawInfo.Rows.RemoveAt(curRowInd);
            mtFrm.mte_defPointValues.RemoveAt(curRowInd);
        }

        private void DefPointForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvxDrawInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            //显示
            HObject cross = new HObject();
            HOperatorSet.GenEmptyObj(out cross);
            cross.Dispose();
            Vision.DrawCross(mtFrm.mte_defPointValues[e.RowIndex][0], mtFrm.mte_defPointValues[e.RowIndex][1], 36, ref cross);
            HOperatorSet.DispObj(cross, mtFrm.htWindow.HTWindow.HalconWindow);
            cross.Dispose();
        }

        private void DefPointForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;
            this.Enabled = mtFrm.mte_TmpPrmValues.IsCreateTmp;
        }
    }
}
