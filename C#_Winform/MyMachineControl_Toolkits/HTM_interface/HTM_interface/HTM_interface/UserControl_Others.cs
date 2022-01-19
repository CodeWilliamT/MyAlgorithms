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
    public partial class UserControl_Others : UserControl
    {
        public UserControl_Others()
        {
            InitializeComponent();
            form_htm = Form_HTM.Get_Form();
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < form_htm.num_device; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[1].Value = form_htm.device_data[i].index.ToString();
                dataGridView1.Rows[i].Cells[2].Value = form_htm.device_data[i].devName;
                dataGridView1.Rows[i].Cells[0].Value = false;
            }
            Index_select = 0;
            dataGridView1.Rows[0].Cells[0].Value = true;
            textBox_devName.Text = form_htm.device_data[Index_select].devName;
        }
        Form_HTM form_htm = null;//主窗体
        int Index_select = 0;//当前被选择记录项序号

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.RowIndex < dataGridView1.RowCount)
                {
                    dataGridView1.Rows[Index_select].Cells[0].Value = false;
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
                    Index_select = e.RowIndex;
                    textBox_devName.Text = form_htm.device_data[Index_select].devName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            try
            {
                ///保存选中的设备的信息
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }
    }
}
