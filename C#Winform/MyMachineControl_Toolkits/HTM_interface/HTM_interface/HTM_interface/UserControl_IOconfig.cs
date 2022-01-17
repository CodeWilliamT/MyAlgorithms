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
    public partial class UserControl_IOconfig : UserControl
    {
        public UserControl_IOconfig()
        {
            InitializeComponent();
            form_htm = Form_HTM.Get_Form();
            bitmap_green = form_htm.GenColorImage(Color.Green, dataGridView1.Columns[8].Width, dataGridView1.RowTemplate.Height);
            bitmap_window = form_htm.GenColorImage(SystemColors.Window, dataGridView1.Columns[8].Width, dataGridView1.RowTemplate.Height);
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < form_htm.num_axis; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[1].Value = form_htm.io_data[i].index.ToString();
                dataGridView1.Rows[i].Cells[2].Value = form_htm.io_data[i].busNo.ToString();
                dataGridView1.Rows[i].Cells[3].Value = form_htm.io_data[i].nodeAddr.ToString();
                dataGridView1.Rows[i].Cells[4].Value = form_htm.io_data[i].portNo.ToString();
                dataGridView1.Rows[i].Cells[5].Value = (bool)(form_htm.io_data[i].ioSrc == 1);
                dataGridView1.Rows[i].Cells[6].Value = (bool)(form_htm.io_data[i].polarity == 1);
                dataGridView1.Rows[i].Cells[7].Value = false;
                dataGridView1.Rows[i].Cells[8].Value = bitmap_window;
                dataGridView1.Rows[i].Cells[9].Value = form_htm.io_data[i].ioName;
            }
        }

        Form_HTM form_htm = null;//主窗体
        Bitmap bitmap_green;//绿色状态单色图
        Bitmap bitmap_window;//无状态单色图

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.RowIndex < dataGridView1.RowCount)
                {
                    if (e.ColumnIndex == 7)
                    {
                        int rowIndex = e.RowIndex;
                        bool flag = !Convert.ToBoolean(dataGridView1.Rows[rowIndex].Cells[7].Value);
                        dataGridView1.Rows[rowIndex].Cells[7].Value = flag;
                    }
                    if (e.ColumnIndex == 10)
                    {
                        Function_SaveSelectRow(e.RowIndex);
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
                    bool flag = Convert.ToBoolean(dataGridView1.Rows[rowIndex].Cells[7].Value);
                    if (flag == true)
                    {
                        dataGridView1.Rows[rowIndex].Cells[7].Value = true;
                        form_htm.io_data[rowIndex].state = 1;
                        if (form_htm.io_data[rowIndex].state == 1) dataGridView1.Rows[rowIndex].Cells[8].Value = bitmap_green;
                        //if (form_htm.io_data[rowIndex].state == 1) dataGridView1.Rows[rowIndex].Cells[8].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[7].Value = false;
                        form_htm.io_data[rowIndex].state = 0;
                        dataGridView1.Rows[rowIndex].Cells[8].Value = bitmap_window;
                        //dataGridView1.Rows[rowIndex].Cells[8].Style.BackColor = System.Drawing.SystemColors.Window;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }

        private void button_SaveAll_Click(object sender, EventArgs e)
        {
            Function_SaveAll();
        }
        private void Function_SaveAll()
        {
            for(int rowIndex=0;rowIndex<form_htm.num_io;rowIndex++)
            {
                Function_SaveSelectRow(rowIndex);
            }
        }
        private void Function_SaveSelectRow(int rowIndex)
        {
            form_htm.io_data[rowIndex].busNo = Convert.ToUInt16(dataGridView1.Rows[rowIndex].Cells[2].Value.ToString());
            form_htm.io_data[rowIndex].nodeAddr = Convert.ToUInt16(dataGridView1.Rows[rowIndex].Cells[3].Value.ToString());
            form_htm.io_data[rowIndex].portNo = Convert.ToUInt16(dataGridView1.Rows[rowIndex].Cells[4].Value.ToString());
            form_htm.io_data[rowIndex].ioSrc = Convert.ToByte(dataGridView1.Rows[rowIndex].Cells[5].Value);
            form_htm.io_data[rowIndex].polarity = Convert.ToByte(dataGridView1.Rows[rowIndex].Cells[6].Value);
            form_htm.io_data[rowIndex].ioName = dataGridView1.Rows[rowIndex].Cells[9].Value.ToString();
        }
    }
}
