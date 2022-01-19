using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTM_interface
{
    public partial class UserControl_Axis : UserControl
    {
        public UserControl_Axis()
        {
            InitializeComponent();
            form_htm = Form_HTM.Get_Form();
            bitmap_green = form_htm.GenColorImage(Color.Green, dataGridView1.Columns[5].Width, dataGridView1.RowTemplate.Height);
            bitmap_window = form_htm.GenColorImage(SystemColors.Window, dataGridView1.Columns[5].Width, dataGridView1.RowTemplate.Height);
            bitmap_red = form_htm.GenColorImage(Color.Red, dataGridView1.Columns[5].Width, dataGridView1.RowTemplate.Height);
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < form_htm.num_axis; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[1].Value = form_htm.axis_data[i].nodeAddr.ToString();
                dataGridView1.Rows[i].Cells[2].Value = form_htm.axis_data[i].axisName;
                dataGridView1.Rows[i].Cells[3].Value = "0.000";
                dataGridView1.Rows[i].Cells[4].Value = "0.000";
                dataGridView1.Rows[i].Cells[5].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[6].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[7].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[8].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[9].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[10].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[11].Value = false;
            } 
            Index_select = 0;
            dataGridView1.Rows[0].Cells[0].Value = true;
        }
        //**********私有变量**********
        Form_HTM form_htm = null;//主窗体
        Form_AxisConfig form_axisConfig;//轴信息配置窗体
        Bitmap bitmap_green;//绿色状态单色图
        Bitmap bitmap_window;//无状态单色图
        Bitmap bitmap_red;//红色状态单色图
        int Index_select=0;//当前被选择记录项序号
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.RowIndex < dataGridView1.RowCount)
                {
                    dataGridView1.Rows[Index_select].Cells[0].Value = false;
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
                    Index_select = e.RowIndex;
                    if (e.ColumnIndex == 11)
                    {
                        bool flag = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
                        dataGridView1.Rows[e.RowIndex].Cells[11].Value = flag;
                    }
                    if (e.ColumnIndex == 12)
                    {
                        form_axisConfig = new Form_AxisConfig(e.RowIndex);
                        form_axisConfig.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }

        private void checkBox_svonAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int rowIndex = 0; rowIndex < form_htm.num_axis; rowIndex++)
                {
                    if (checkBox_svonAll.Checked == true)
                    {
                        form_htm.axis_data[rowIndex].svon = 1;
                        dataGridView1.Rows[rowIndex].Cells[11].Value = true;
                        dataGridView1.Rows[rowIndex].Cells[3].Value = Math.Round(form_htm.axis_data[rowIndex].cmdPos, 3).ToString("f3");
                        dataGridView1.Rows[rowIndex].Cells[4].Value = Math.Round(form_htm.axis_data[rowIndex].fbkPos, 3).ToString("f3");
                        if (form_htm.axis_data[rowIndex].pel == 1) dataGridView1.Rows[rowIndex].Cells[5].Value=bitmap_red;
                        if (form_htm.axis_data[rowIndex].mel == 1) dataGridView1.Rows[rowIndex].Cells[6].Value=bitmap_red;
                        if (form_htm.axis_data[rowIndex].org == 1) dataGridView1.Rows[rowIndex].Cells[7].Value=bitmap_red;
                        if (form_htm.axis_data[rowIndex].alm == 1) dataGridView1.Rows[rowIndex].Cells[8].Value=bitmap_red;
                        if (form_htm.axis_data[rowIndex].atp == 1) dataGridView1.Rows[rowIndex].Cells[9].Value = bitmap_green;
                        if (form_htm.axis_data[rowIndex].svon == 1) dataGridView1.Rows[rowIndex].Cells[10].Value = bitmap_green;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[11].Value = false;
                        form_htm.axis_data[rowIndex].svon = 0;
                        dataGridView1.Rows[rowIndex].Cells[3].Value = "0.000";
                        dataGridView1.Rows[rowIndex].Cells[4].Value = "0.000";
                        dataGridView1.Rows[rowIndex].Cells[5].Value=bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[6].Value=bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[7].Value=bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[8].Value=bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[9].Value=bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[10].Value=bitmap_window;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int rowIndex = 0; rowIndex < form_htm.num_axis; rowIndex++)
                {
                    bool flag = Convert.ToBoolean(dataGridView1.Rows[rowIndex].Cells[11].Value);
                    if (flag == true)
                    {
                        dataGridView1.Rows[rowIndex].Cells[11].Value = true;
                        form_htm.axis_data[rowIndex].svon = 1;
                        dataGridView1.Rows[rowIndex].Cells[3].Value = Math.Round(form_htm.axis_data[rowIndex].cmdPos, 3).ToString("f3");
                        dataGridView1.Rows[rowIndex].Cells[4].Value = Math.Round(form_htm.axis_data[rowIndex].fbkPos, 3).ToString("f3");
                        if (form_htm.axis_data[rowIndex].pel == 1) dataGridView1.Rows[rowIndex].Cells[5].Value = bitmap_red;
                        if (form_htm.axis_data[rowIndex].mel == 1) dataGridView1.Rows[rowIndex].Cells[6].Value = bitmap_red;
                        if (form_htm.axis_data[rowIndex].org == 1) dataGridView1.Rows[rowIndex].Cells[7].Value = bitmap_red;
                        if (form_htm.axis_data[rowIndex].alm == 1) dataGridView1.Rows[rowIndex].Cells[8].Value = bitmap_red;
                        if (form_htm.axis_data[rowIndex].atp == 1) dataGridView1.Rows[rowIndex].Cells[9].Value = bitmap_green;
                        if (form_htm.axis_data[rowIndex].svon == 1) dataGridView1.Rows[rowIndex].Cells[10].Value = bitmap_green;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[11].Value = false;
                        form_htm.axis_data[rowIndex].svon = 0;
                        dataGridView1.Rows[rowIndex].Cells[3].Value = "0.000";
                        dataGridView1.Rows[rowIndex].Cells[4].Value = "0.000";
                        dataGridView1.Rows[rowIndex].Cells[5].Value = bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[6].Value = bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[7].Value = bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[8].Value = bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[9].Value = bitmap_window;
                        dataGridView1.Rows[rowIndex].Cells[10].Value = bitmap_window;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }
    }
}
