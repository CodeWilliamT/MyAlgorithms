using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Configuration;
using System.Collections.ObjectModel;
using Utils;
using System.Collections;
using System.Text.RegularExpressions;
using HT_Lib;

namespace LeadframeAOI
{
    public partial class frmUser : Form
    {
        public static string workNumber;
        frmUser Instance;
        public frmUser()
        {
            InitializeComponent();
            Instance = this;
            comboBox1.Items.AddRange(new object[] { "操作员", "技术员", "工程师" });
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void UserUI_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            ListViewDisplay();
        }
        /// <summary>
        /// 初始化listview列表，并将其绑定某数据表 by LYH
        /// </summary>
        private void ListViewDisplay()
        {
            listView1.Visible = true;
            listView1.Scrollable = true;//显示滚动条
            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("工号", -2, HorizontalAlignment.Center); // Width of -2 indicates auto-size.
            List<String> workNumList = App.obj_User.DataToList();
            for (int i = 0; i < workNumList.Count; i++)
            {
                ListViewItem li = new ListViewItem();
                li.Checked = true;
                li.SubItems.Clear();
                li.SubItems[0].Text = workNumList[i];
                listView1.Items.Add(li);
            }
        }
        /// <summary>
        /// 点击listview中的某条信息，将其所包含的全部信息显示到textbox中 by LYH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textWorkNumber.Text = listView1.SelectedItems[0].Text;
                App.obj_User.workNumber = listView1.SelectedItems[0].Text;
                App.obj_User.Query();
                textPassWord.Text = App.obj_User.passWord;
                switch (App.obj_User.userLevel)
                {
                    case 0:
                        comboBox1.SelectedIndex = 0;
                        break;
                    case 1:
                        comboBox1.SelectedIndex = 1;
                        break;
                    case 2:
                        comboBox1.SelectedIndex = 2;
                        break;
                }
            }
        }
        /// <summary>
        /// 在数据库中添加一条信息 by LYH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if ((textPassWord.Text != string.Empty && textWorkNumber.Text != string.Empty) == false)
            {
                HTUi.PopError("工号、密码不能为空!");
                return;
            }
            if (textPassWord.Text.IndexOf(" ") != -1 || textWorkNumber.Text.IndexOf(" ") != -1)
            {
                HTUi.PopError("工号、密码不能含有空格!");
                return;
            }
            cboxSelection();
            App.obj_User.passWord = textPassWord.Text;
            App.obj_User.workNumber = textWorkNumber.Text;
            if (App.obj_User.Query() == true)
            {
                HTUi.TipHint("工号已存在！");
                return;
            }
            if (App.obj_User.InsertData() == true)
            {
                ListViewDisplay();
                HTUi.TipHint("添加成功");
            }
            else
            {
                HTUi.PopError("添加失败");
            }
        }
        /// <summary>
        /// 编辑单击获取的数据   by LYH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if ((textPassWord.Text != string.Empty && textWorkNumber.Text != string.Empty) == false)
            {
                HTUi.PopError("密码、工号不能为空!");
                return;
            }
            if (textPassWord.Text.IndexOf(" ") != -1 || textWorkNumber.Text.IndexOf(" ") != -1)
            {
                HTUi.PopError("工号、密码不能含有空格!");
                //clearTextBox();
                return;
            }

            cboxSelection();
            App.obj_User.passWord = textPassWord.Text;
            App.obj_User.workNumber = textWorkNumber.Text;
            if (App.obj_User.Query() == false)
            {
                HTUi.PopError("只能修改现有用户信息");
                return;
            }
            if (App.obj_User.UpdateData() == false)
            {
                HTUi.PopError("用户修改失败");
                return;
            }
            ListViewDisplay();
            HTUi.TipHint("用户修改成功");
            clearTextBox();
        }
        /// <summary>
        /// 将数据库中的所有信息清空    by LYH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (HTUi.PopYesNo("是否") == true)
            {
                try
                {
                    App.obj_User.DeleteData(textWorkNumber.Text);
                    ListViewDisplay();
                }
                catch (Exception)
                {
                    HTUi.PopError("只能删除已有账户！");
                }
            }
            clearTextBox();
        }
        /// <summary>
        /// 清空每个textbox by LYH
        /// </summary>
        void clearTextBox()
        {
            textWorkNumber.Text = string.Empty;
            textPassWord.Text = string.Empty;
        }
        private void cboxSelection()
        {
            string userLevelStr = comboBox1.SelectedItem.ToString();
            switch (userLevelStr)
            {
                case "操作员":
                    App.obj_User.userLevel = 0;
                    break;
                case "技术员":
                    App.obj_User.userLevel = 1;
                    break;
                case "工程师":
                    App.obj_User.userLevel = 2;
                    break;
            }
        }
    }
}
