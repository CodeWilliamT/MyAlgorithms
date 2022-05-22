using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public BinaryReader br;
        public BinaryWriter bw;
        public Form1()
        {
            InitializeComponent(); 
            IPAddress[] ips = Dns.GetHostAddresses("");
            this.textBox_ServerIP.Text = ips[1].ToString();
            this.textBox_IdPort.Text = "3011";
        }
        private void ClientA()
        {
            //通过服务器的ip和端口号，创建TcpClient实例
            client = new TcpClient(this.textBox_ServerIP.Text, Convert.ToInt32(this.textBox_IdPort.Text));
            label1.Invoke(new MethodInvoker(() =>
                {
                    label1.Text = "与服务器连接成功";
                    button_cntServer.Text = "已链接";
                }));
            while (true)
            {
                try
                {
                    NetworkStream clientStream = client.GetStream();
                    br = new BinaryReader(clientStream);
                    string receive = null;

                    receive = br.ReadString();
                    textBox1.Invoke(new MethodInvoker(() => textBox1.Text += receive + "\r\n"));
                }
                catch
                {
                    MessageBox.Show("接收失败！");
                    try
                    {
                        label1.Invoke(new MethodInvoker(() =>
                        {
                            label1.Text = "未链接";
                            button_cntServer.Text = "未链接";
                        }));
                    }
                    catch
                    {
                        return;
                    }
                    return;
                }
            }
        }
        private void button_cntServer_Click(object sender, EventArgs e)
        {
            button_cntServer.Text = "尝试建立连接中";
            Thread myThread = new Thread(ClientA);
            myThread.Start();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            NetworkStream clientStream = client.GetStream();
            bw = new BinaryWriter(clientStream);
            bw.Write(tbmessage.Text);
            textBox1.Text += tbmessage.Text + "\r\n";
        }
    }
}
