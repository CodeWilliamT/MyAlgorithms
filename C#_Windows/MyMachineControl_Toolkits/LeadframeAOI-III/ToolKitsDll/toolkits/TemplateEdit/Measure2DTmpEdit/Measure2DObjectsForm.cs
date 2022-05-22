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
using ToolKits.FunctionModule;

namespace ToolKits.TemplateEdit.Measure2DTmpEdit
{
    public partial class Measure2DObjectsForm : BaseForm
    {
        public Measure2DObjectsForm()
        {
            InitializeComponent();
        }
        MainTemplateForm mtFrm = MainTemplateForm.GetMainTemplateForm();
        private void Measure2DObjectsForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;
        }

        private void btnExAdd_Click(object sender, EventArgs e)
        {
            if (mtFrm.htWindow.RegionType == "")
            {
                MessageBox.Show("未知的区域类型！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (mtFrm.htWindow.RegionType != "Line" &&
                mtFrm.htWindow.RegionType != "Circle" &&
                mtFrm.htWindow.RegionType != "Ellipse" &&
                mtFrm.htWindow.RegionType != "Rectangle1" &&
                mtFrm.htWindow.RegionType != "Rectangle2"
                )
            {
                MessageBox.Show("选择测量区域类型不正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowInd = this.dgvMsr2DObjects.Rows.Add();
            this.dgvMsr2DObjects.Rows[rowInd].Cells[0].Value = mtFrm.drawTypeImg[mtFrm.htWindow.RegionType];
            this.dgvMsr2DObjects.Rows[rowInd].Cells[1].Value = rowInd.ToString();
            //更新数据
            MainTemplateForm.VaryHobject vCont = new MainTemplateForm.VaryHobject();
            vCont.Dispose();
            try
            {
                vCont.contType = mtFrm.htWindow.RegionType;
                Vision.GenROI(mtFrm.htWindow, "contour", ref vCont.obj);
                if (mtFrm.mte_Msr2DRegValues.Count <= rowInd + 1)
                    mtFrm.mte_Msr2DRegValues.Add(vCont);
                else
                {
                    mtFrm.mte_Msr2DRegValues[rowInd].Dispose();
                    mtFrm.mte_Msr2DRegValues[rowInd] = vCont;
                }
                //初始化检测参数
                MainTemplateForm.Measure2DParam msr2DPrm = new MainTemplateForm.Measure2DParam();
                if (mtFrm.mte_Msr2DPrmValues.Count <= rowInd + 1)
                    mtFrm.mte_Msr2DPrmValues.Add(msr2DPrm);
                else
                {
                    mtFrm.mte_Msr2DPrmValues[rowInd] = msr2DPrm;
                }
            }
            catch (Exception ex)
            {
                vCont.Dispose();
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvMsr2DObjects.RowCount < 1) return;
            int curRowInd = this.dgvMsr2DObjects.CurrentCell.RowIndex;
            this.dgvMsr2DObjects.Rows.RemoveAt(curRowInd);
            mtFrm.mte_Msr2DRegValues[curRowInd].Dispose();
            mtFrm.mte_Msr2DRegValues.RemoveAt(curRowInd);
            mtFrm.mte_Msr2DPrmValues.RemoveAt(curRowInd);
            for (int i = 0; i < this.dgvMsr2DObjects.RowCount; i++)
            {
                this.dgvMsr2DObjects.Rows[i].Cells[1].Value = i.ToString();
            }
        }

        private void dgvMsr2DObjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 2) return;
            if (e.ColumnIndex == 2)//显示
            {
                HOperatorSet.DispObj(mtFrm.mte_Msr2DRegValues[e.RowIndex].obj, mtFrm.htWindow.HTWindow.HalconWindow);
            }
            else //设置参数
            {
                Measure2DParamForm msr2DPrmFrm = new Measure2DParamForm();
                if (!msr2DPrmFrm.SetValue(mtFrm.mte_Msr2DPrmValues[e.RowIndex]))
                {
                    MessageBox.Show("加载2D测量参数失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    msr2DPrmFrm.Dispose();
                    return;
                }
                if (msr2DPrmFrm.ShowDialog() != DialogResult.OK) return;
                bool applyOther = msr2DPrmFrm.ApplyOther;
                MainTemplateForm.Measure2DParam msr2DPrm = new MainTemplateForm.Measure2DParam();
                if (!msr2DPrmFrm.GetValue(ref msr2DPrm))
                {
                    MessageBox.Show("获取2D测量参数失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    msr2DPrmFrm.Dispose();
                    return;
                }
                mtFrm.mte_Msr2DPrmValues[e.RowIndex] = msr2DPrm;
                msr2DPrmFrm.Dispose();
                //给其它同类测量模型设置测量参数
                string contType = mtFrm.mte_Msr2DRegValues[e.RowIndex].contType;
                for (int i = 0; i < mtFrm.mte_Msr2DRegValues.Count; i++)
                {
                    if (mtFrm.mte_Msr2DRegValues[i].contType == contType)
                        mtFrm.mte_Msr2DPrmValues[i] = msr2DPrm.DeepClone();
                }
            }
        }

        private void Measure2DObjectsForm_Load(object sender, EventArgs e)
        {
            if (mtFrm.mte_Msr2DRegValues.Count < 1) return;
            for (int i = 0; i < mtFrm.mte_Msr2DRegValues.Count; i++)
            {
                int rowInd = this.dgvMsr2DObjects.Rows.Add();
                this.dgvMsr2DObjects.Rows[rowInd].Cells[0].Value = mtFrm.drawTypeImg[mtFrm.mte_Msr2DRegValues[i].contType];
                this.dgvMsr2DObjects.Rows[rowInd].Cells[1].Value = rowInd.ToString();
                try
                {
                    //初始化检测参数
                    MainTemplateForm.Measure2DParam msr2DPrm = new MainTemplateForm.Measure2DParam();
                    if (mtFrm.mte_Msr2DPrmValues.Count <= rowInd + 1)
                        mtFrm.mte_Msr2DPrmValues.Add(msr2DPrm);
                    else
                    {
                        mtFrm.mte_Msr2DPrmValues[rowInd] = msr2DPrm;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
