using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolKits.TemplateEdit.Measure2DTmpEdit
{
    public partial class Measure2DParamForm : Form
    {
        public Measure2DParamForm()
        {
            InitializeComponent();
        }
        public bool ApplyOther { get; private set; }
        public bool SetValue(MainTemplateForm.Measure2DParam msr2DPrm)
        {
            try
            {
                this.nudInstanceNums.Value = Convert.ToDecimal(msr2DPrm.InstanceNums);
                this.nudMeasureNums.Value = Convert.ToDecimal(msr2DPrm.MeasureNums);
                this.nudMeasureSigma.Value = Convert.ToDecimal(msr2DPrm.MeasureSigma);
                this.nudMinScore.Value = Convert.ToDecimal(msr2DPrm.MinScore);
                this.nudMeasureLen1.Value = Convert.ToDecimal(msr2DPrm.MeasureLen1);
                this.nudMeasureLen2.Value = Convert.ToDecimal(msr2DPrm.MeasureLen2);
                this.nudMeasureThreshold.Value = Convert.ToDecimal(msr2DPrm.MeasureThreshold);
                this.cbMeasureSelect.Text = msr2DPrm.MeausreSelect;
                this.cbMeasureTransition.Text = msr2DPrm.MeasureTransition;
                this.nudStartAngle.Value = Convert.ToDecimal(msr2DPrm.StartAngle);
                this.nudEndAngle.Value = Convert.ToDecimal(msr2DPrm.EndAngle);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool GetValue(ref MainTemplateForm.Measure2DParam msr2DPrm)
        {
            try
            {
                msr2DPrm.InstanceNums = Convert.ToInt32(this.nudInstanceNums.Value);
                msr2DPrm.MeasureNums = Convert.ToInt32(this.nudMeasureNums.Value);
                msr2DPrm.MeasureSigma = Convert.ToDouble(this.nudMeasureSigma.Value);
                msr2DPrm.MinScore = Convert.ToDouble(this.nudMinScore.Value);
                msr2DPrm.MeasureLen1 = Convert.ToDouble(this.nudMeasureLen1.Value);
                msr2DPrm.MeasureLen2 = Convert.ToDouble(this.nudMeasureLen2.Value);
                msr2DPrm.MeasureThreshold = Convert.ToDouble(this.nudMeasureThreshold.Value);
                msr2DPrm.MeausreSelect = this.cbMeasureSelect.SelectedItem.ToString();
                msr2DPrm.MeasureTransition = this.cbMeasureTransition.SelectedItem.ToString();
                msr2DPrm.StartAngle = Convert.ToDouble(this.nudStartAngle.Value);
                msr2DPrm.EndAngle = Convert.ToDouble(this.nudEndAngle.Value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ApplyOther = cbApplyOther.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Measure2DParamForm_Load(object sender, EventArgs e)
        {

        }
    }
}
