using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolKits.LightChart
{
    public partial class LightChart : UserControl
    {
        private List<LightParam> lLightPrm;
        private int row;
        private int col;

        public LightChart()
        {
            InitializeComponent();
            this.dgvLightChart.ColumnHeadersVisible = false;
            this.dgvLightChart.RowHeadersVisible = false;
            this.dgvLightChart.RowPrePaint += dgvLightChart_RowPrePaint;
            this.lLightPrm = new List<LightParam>();
        }

        public event EventHandler<LightEventArgs> savePrmEventHandler;
        public event EventHandler<int> rowClickEventHandler;

        void dgvLightChart_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //throw new NotImplementedException();
            //1,4,7,10...软触发行
            if ((e.RowIndex - 1) % 3 == 0)
            {
                for (int i = 0; i < this.lLightPrm.Count; i++)
                {
                    this.dgvLightChart[i, e.RowIndex] = new DataGridViewButtonCell();
                    this.dgvLightChart[i, e.RowIndex].Value = "软触发";
                }
            }
            //2，5，8，11...状态使能行
            else if ((e.RowIndex - 1) % 3 == 1)
            {
                for (int i = 0; i < this.lLightPrm.Count; i++)
                {
                    this.dgvLightChart[i, e.RowIndex] = new DataGridViewCheckBoxCell();
                    this.dgvLightChart[i, e.RowIndex].Value = false;
                }
            }
        }
        private void SetDGVValue()
        {
            this.dgvLightChart.RowCount = this.row;
            this.dgvLightChart.ColumnCount = this.col;
            //设置行标题
            this.dgvLightChart.Rows[0].HeaderCell.Value = "触发操作";
            for (int i = 0; i < (this.row - 1) / 3; i++)
            {
                this.dgvLightChart.Rows[1 + i * 3].HeaderCell.Value = "状态使能[" + this.lLightPrm[i].camName + "]";
                this.dgvLightChart.Rows[2 + i * 3].HeaderCell.Value = "触发时长[" + this.lLightPrm[i].camName + "]/us";
                this.dgvLightChart.Rows[3 + i * 3].HeaderCell.Value = "触发延迟[" + this.lLightPrm[i].camName + "]/ms";
            }
            for (int i = 0; i < this.col; i++)
            {
                for (int j = 0; j < (this.row - 1) / 3; j++)
                {
                    this.dgvLightChart.Columns[i].HeaderCell.Value = this.lLightPrm[i * 3 + j].lightName;
                    this.dgvLightChart.Rows[1 + j * 3].Cells[i].Value = this.lLightPrm[i * 3 + j].lightEnable;
                    this.dgvLightChart.Rows[2 + j * 3].Cells[i].Value = this.lLightPrm[i * 3 + j].lightTime;
                    this.dgvLightChart.Rows[3 + j * 3].Cells[i].Value = this.lLightPrm[i * 3 + j].delayTime;
                }
            }
        }
        private void GetDGVValue()
        {
            for (int i = 0; i < this.col; i++)
            {
                for (int j = 0; j < (this.row - 1) / 3; j++)
                {
                    this.lLightPrm[i * 3 + j].lightEnable = Convert.ToBoolean(this.dgvLightChart.Rows[1 + j * 3].Cells[i].EditedFormattedValue);
                    this.lLightPrm[i * 3 + j].lightTime = Convert.ToInt32(this.dgvLightChart.Rows[2 + j * 3].Cells[i].Value);
                    this.lLightPrm[i * 3 + j].delayTime = Convert.ToInt32(this.dgvLightChart.Rows[3 + j * 3].Cells[i].Value);
                }
            }
        }
        /// <summary>
        /// 向光源表格中添加数据
        /// </summary>
        /// <param name="LLightPrm">光源参数</param>
        /// <param name="camNum">相机个数</param>
        /// <returns></returns>
        public bool SetValue(List<LightParam> LLightPrm, int camNum)
        {
            try
            {
                this.lLightPrm.Clear();
                this.lLightPrm = null;
                this.lLightPrm = new List<LightParam>(LLightPrm);
                this.row = camNum * 3 + 1;
                this.col = this.lLightPrm.Count;

                if (this.dgvLightChart.RowCount > 0)
                    this.dgvLightChart.Rows.Clear();
                SetDGVValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void btnSavePrm_Click(object sender, EventArgs e)
        {
            GetDGVValue();
            if (this.savePrmEventHandler != null)
                this.savePrmEventHandler(this, new LightEventArgs(new List<LightParam>(this.lLightPrm)));
        }

        private void dgvLightChart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                if (this.rowClickEventHandler != null)
                    this.rowClickEventHandler(this, e.ColumnIndex);
            }
        }

    }
    /// <summary>
    /// 光源参数基类，二次开发可以在此基类上继承
    /// </summary>
    public class LightParam
    {
        /// <summary>
        /// 光源名称
        /// </summary>
        public string lightName;
        /// <summary>
        /// 光源索引
        /// </summary>
        public int lightInd;
        /// <summary>
        /// 相机名称
        /// </summary>
        public string camName;
        /// <summary>
        /// 相机索引
        /// </summary>
        public int camInd;
        /// <summary>
        /// 光源使能状态
        /// </summary>
        public bool lightEnable;
        /// <summary>
        /// 点亮时间，单位us
        /// </summary>
        public int lightTime;
        /// <summary>
        /// 延迟时间，单位ms
        /// </summary>
        public int delayTime;
        public LightParam()
        {
            lightName = "";
            camName = "";
            lightEnable = false;
            lightTime = 0;
            delayTime = 0;
        }
        public void Dispose()
        {
            lightName = "";
            camName = "";
            lightEnable = false;
            lightTime = 0;
            delayTime = 0;
        }
    }
    public class LightEventArgs : EventArgs
    {
        public List<LightParam> LLightPrm;
        public LightEventArgs(List<LightParam> LLightPrm)
        {
            this.LLightPrm = LLightPrm;
        }
    }
}
