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

namespace TCPServer
{
    public partial class MainWindow : Form
    {
        private TcpListener myListener;
        private TcpClient newClient;
        public BinaryReader br;
        public BinaryWriter bw;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ServerA()
        {
            IPAddress ip = IPAddress.Parse("192.168.0.114");//服务器端ip
            myListener = new TcpListener(ip, 3011);//创建TcpListener实例
            myListener.Start();//start
            newClient = myListener.AcceptTcpClient();//等待客户端连接
            label1.Invoke(new MethodInvoker(() => label1.Text = "连接成功"));
            //label1.Invoke(new delegate
            //{
            //    ()=> label1.Text = "连接成功";
            //    });
            while (true)
            {
                try
                {
                    NetworkStream clientStream = newClient.GetStream();//利用TcpClient对象GetStream方法得到网络流
                    br = new BinaryReader(clientStream);
                    string receive = null;
                    receive = br.ReadString();//读取
                    textBox1.Invoke(new MethodInvoker(() => textBox1.Text += receive + "\r\n"));
                    
                }
                catch
                {
                    MessageBox.Show("接收失败！");
                }
            }
        }

        private void button_crtServer_Click(object sender, EventArgs e)
        {
            Thread myThread = new Thread(ServerA);
            myThread.Start();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            //利用TcpClient对象GetStream方法得到网络流
            NetworkStream clientStream = newClient.GetStream();
            bw = new BinaryWriter(clientStream);
            //写入
            bw.Write(tbmessage.Text);
            textBox1.Text += tbmessage.Text + "\r\n";
        }

    }
}
