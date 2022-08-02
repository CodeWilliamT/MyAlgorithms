using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HT_Lib;

namespace LeadframeAOI
{
    public partial class Form_BoxManage : Form
    {
        public static Form_BoxManage Instance;
        int selectIdx;
        public Form_BoxManage()
        {
            InitializeComponent();
            RefreshList();
            Instance = this;
        }
        /// <summary>
        /// 刷新料盒列表
        /// </summary>
        public void RefreshList()
        {
            listBox_Box.Items.Clear();
            foreach(var pair in App.boxManager.Dir_Boxes)
            {
                listBox_Box.Items.Add(pair.Value.Name);
            }
            if (App.boxManager.Dir_Boxes.Count > 0) listBox_Box.SelectedIndex = 0;
        }
        private void listBox_Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach(var item in App.boxManager.Dir_Boxes)
            {
                if (i== listBox_Box.SelectedIndex)
                {
                    propertyGrid_Box.SelectedObject = item.Value;
                    return;
                }
                i++;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Box ItemBox = new Box("Box_" + (App.boxManager.Dir_Boxes.Count),
                    App.boxManager.Dir_Boxes.Count == 0 ? 0 : App.boxManager.Dir_Boxes.Last().Value.Idx + 1);
                App.boxManager.Dir_Boxes.TryAdd(ItemBox.Idx, ItemBox);
                listBox_Box.Items.Add(App.boxManager.Dir_Boxes.Last().Value.Name);
                if (App.boxManager.Dir_Boxes.Count > 0) listBox_Box.SelectedIndex = App.boxManager.Dir_Boxes.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            try
            {
                if (App.boxManager.Dir_Boxes.Count > 0)
                {
                    Box selectBox = (Box)propertyGrid_Box.SelectedObject;
                    int selectBoxIdx = selectBox.Idx;
                    int selectIdx = listBox_Box.SelectedIndex;
                    listBox_Box.SelectedIndex = 0;
                    Box Item;
                    if (!App.boxManager.Dir_Boxes.TryRemove(App.boxManager.Dir_Boxes[selectBoxIdx].Idx, out Item))
                    {
                        HTUi.PopError("删除失败！");
                        return;
                    }
                    listBox_Box.Items.RemoveAt(selectIdx);
                }
                else
                {
                    HTUi.PopError("没有元素可供删除！\n");
                }
                if (App.boxManager.Dir_Boxes.Count > 0) listBox_Box.SelectedIndex = App.boxManager.Dir_Boxes.Count - 1;
            }
            catch (Exception ex)
            {
                HTUi.PopError("删除失败！\n" + ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            App.boxManager.Save();
            frmProduct.Instance.prgProductMagzine.Refresh();
            HTUi.TipHint("保存成功！");
        }

        private void btn_AddCopy_Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid_Box.Refresh();
                if (App.boxManager.Dir_Boxes.Count == 0)
                {

                    HTLog.Error("增加失败！\n");
                    return;
                }
                Box selectBox = (Box)propertyGrid_Box.SelectedObject;
                int selectBoxIdx = selectBox.Idx;
                int selectIdx = listBox_Box.SelectedIndex;
                Box ItemBox = App.boxManager.Dir_Boxes[selectBoxIdx].Clone();
                ItemBox.Idx = App.boxManager.Dir_Boxes.Last().Value.Idx + 1;
                ItemBox.Name = ItemBox.Name + "'s Copy";
                App.boxManager.Dir_Boxes.TryAdd(ItemBox.Idx, ItemBox);
                listBox_Box.Items.Add(ItemBox.Name);
                if (App.boxManager.Dir_Boxes.Count > 0) listBox_Box.SelectedIndex = App.boxManager.Dir_Boxes.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }
    }
}
