using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Globalization;

namespace FTPServer
{
    public partial class Form1 : Form
    {
        FtpServer ftpServer;
        public Form1()
        {
            InitializeComponent();
            // 设置默认的主目录
            tbxFtpRoot.Text = "E:/MyFtpServerRoot/";
            IPAddress[] ips = Dns.GetHostAddresses("");
            tbxFtpServerIp.Text = ips[1].ToString();
            tbxFtpServerPort.Text = "21";
            lstboxStatus.Enabled = false;
            ftpServer = new FtpServer(tbxFtpServerIp.Text, int.Parse(tbxFtpServerPort.Text), tbxFtpRoot.Text, AddInfo);
        }

        // 启动服务器
        private void btnFtpServerStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(tbxFtpRoot.Text))
                    Directory.CreateDirectory(tbxFtpRoot.Text);
                if (ftpServer.StartStopFtpServer())
                {
                    lstboxStatus.Enabled = true;
                    lstboxStatus.Items.Clear();
                    lstboxStatus.Items.Add("启动Ftp服务...");
                    btnFtpServerStartStop.Text = "停止";
                }
                else
                {
                    lstboxStatus.Items.Add("Ftp服务已停止！");
                    lstboxStatus.TopIndex = lstboxStatus.Items.Count - 1;
                    btnFtpServerStartStop.Text = "启动";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动Ftp服务失败！"+ex.Message);
            }
        }

        // 向屏幕输出显示状态信息（这里使用了委托机制）
        public delegate void AddInfoDelegate(string str);

        public void AddInfo(string str)
        {
            // 如果调用AddInfo()方法的线程与创建ListView控件的线程不在一个线程时
            // 此时利用委托在创建ListView的线程上调用
            if (lstboxStatus.InvokeRequired == true)
            {
                AddInfoDelegate d = new AddInfoDelegate(AddInfo);
                this.Invoke(d, str);
            }
            else
            {
                lstboxStatus.Items.Add(str);
                lstboxStatus.TopIndex = lstboxStatus.Items.Count - 1;
                lstboxStatus.ClearSelected();
            }
        }
        
    }

}
