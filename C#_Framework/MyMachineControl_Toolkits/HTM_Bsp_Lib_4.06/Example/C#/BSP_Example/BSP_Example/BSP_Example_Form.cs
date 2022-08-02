using System;
using System.Windows.Forms;
using HTM_BSP;

namespace BSP_Example
{
    public partial class BSP_Example_Form : Form
    {
        public BSP_Example_Form()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Button_Init_Click(object sender, EventArgs e)
        {
            Int32 err = 0;
            if (button_Init.Text == "初始化")
            {
                if(checkBox_Remote.Checked)
                {
                    HTM.Connect(textBox_IP.Text);
                }
                else
                {
                    HTM.LocalMode = true;
                }

                // 初始化的参数配置

                /* //方法1：利用结构体配置初始化
                INIT_PARA init = new INIT_PARA();
                init.para_file = "htm_paras.db";  //需要放在程序启动目录（如果文件在其它路径，可以配置全路径）
                init.use_aps_card = 1;
                init.use_htnet_card = 1;
                init.offline_mode = 1;      //离线模式
                init.max_axis_num = 10;     //轴数
                init.max_io_num = 50;
                init.max_dev_num = 2;
                init.language = 0;          //语言 0-简体中文，1-英语（需有语言包）

                listBox_DebugInfo.Items.Add("Para File: " + init.para_file);
                listBox_DebugInfo.Items.Add("Axis num: " + Convert.ToString(init.max_axis_num));
                listBox_DebugInfo.Items.Add("I/O num: " + Convert.ToString(init.max_io_num));
                listBox_DebugInfo.Items.Add("Mode: " + ((init.offline_mode != 0) ? "离线" : "在线"));
                
                err = HTM.Init(ref init); 
                */

                // 方法2：利用配置文件初始化
                err = HTM.InitFromFile("config.ini");
                if (err == 0)
                {
                    listBox_DebugInfo.Items.Add("初始化成功");
                    button_Init.Text = "卸载";
                    button_LoadUI.Enabled = true;
                    button_LoadTool.Enabled = true;
                }
                else
                {
                    listBox_DebugInfo.Items.Add(String.Format("初始化失败, err = {0}", err));
                }
            }
            else
            {
                HTM.Discard();
                button_Init.Text = "初始化";
                button_LoadUI.Enabled = false;
                button_LoadTool.Enabled = false;
            }
        }

        private void button_LoadUI_Click(object sender, EventArgs e)
        {
            Int32 err = HTM.LoadUI(null, false);
            if (err < 0)
            {
                listBox_DebugInfo.Items.Add(String.Format("加载面板失败, err = {0}", err));
            }
        }

        private void button_LoadTool_Click(object sender, EventArgs e)
        {
            Int32 err = HTM.LoadToolUI();
            if (err < 0)
            {
                listBox_DebugInfo.Items.Add(String.Format("加载工具面板失败, err = {0}", err));
            }
        }

        private void button_Time_Click(object sender, EventArgs e)
        {
            Int32 err = HTM.EventLog_LoadUI();
            if (err < 0)
            {
                listBox_DebugInfo.Items.Add(String.Format("加载时效面板失败, err = {0}", err));
            }
        }
    }
}
