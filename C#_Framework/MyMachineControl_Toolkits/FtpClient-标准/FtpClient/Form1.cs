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

namespace FtpClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            IPAddress[] ips = Dns.GetHostAddresses("");
            textBox_IP.Text = ips[1].ToString();
            textBox_Port.Text = Convert.ToString(21);
        }
        FtpClient ftpClient = null;
        private string ftpUristring = null;
        private string currentDir = "/";
        private void button_FtpLink_Click(object sender, EventArgs e)
        {
            try
            {
                button_FtpLink.Enabled = false;
                ftpClient = new FtpClient(textBox_IP.Text, "", "admin", "admin", int.Parse(textBox_Port.Text));
                ftpUristring = "ftp://" + textBox_IP.Text;
                ftpClient.Connect();
                List_Refresh();
                button_FtpLink.Enabled = true;
            }
            catch(Exception ex)
            {
                button_FtpLink.Enabled = true;
                MessageBox.Show("创建Ftp链接失败\n"+ex.Message);
            }
        }
        private void List_Refresh()
        {
            lstbxFtpResources.Items.Clear(); string uri = string.Empty;
            if (currentDir == "/")
            {
                uri = ftpUristring;
            }
            else
            {
                uri = ftpUristring + currentDir;
            }

            string[] urifield = uri.Split(' ');
            uri = urifield[0];
            string s="";
            string[] ftpdir = ftpClient.List(currentDir);
            lstbxFtpResources.Items.Add("↑返回上层目录");
            int length = 0;
            for (int i = 0; i < ftpdir.Length; i++)
            {
                if (ftpdir[i].EndsWith("."))
                {
                    length = ftpdir[i].Length - 2;
                    break;
                }
            }

            for (int i = 0; i < ftpdir.Length; i++)
            {
                s = ftpdir[i];
                int index = s.LastIndexOf('\t');
                if (index == -1)
                {
                    if (length < s.Length)
                    {
                        index = length;
                    }
                    else
                    {
                        continue;
                    }
                }

                string name = s.Substring(index + 1);
                if (name == "." || name == "..")
                {
                    continue;
                }

                // 判断是否为目录，在名称前加"目录"来表示
                if (s[0] == 'd' || (s.ToLower()).Contains("<dir>"))
                {
                    string[] namefield = name.Split(' ');
                    int namefieldlength = namefield.Length;
                    string dirname;
                    dirname = namefield[namefieldlength - 1];

                    // 对齐
                    dirname = dirname.PadRight(34, ' ');
                    name = dirname;
                    // 显示目录
                    lstbxFtpResources.Items.Add("[目录]" + name);
                }
            }

            for (int i = 0; i < ftpdir.Length; i++)
            {
                s = ftpdir[i];
                int index = s.LastIndexOf('\t');
                if (index == -1)
                {
                    if (length < s.Length)
                    {
                        index = length;
                    }
                    else
                    {
                        continue;
                    }
                }

                string name = s.Substring(index + 1);
                if (name == "." || name == "..")
                {
                    continue;
                }

                // 判断是否为文件
                if (!(s[0] == 'd' || (s.ToLower()).Contains("<dir>")))
                {
                    string[] namefield = name.Split(' ');
                    int namefieldlength = namefield.Length;
                    string filename;

                    filename = namefield[namefieldlength - 1];

                    // 对齐
                    //filename = filename.PadRight(34, ' ');
                    name = filename;

                    // 显示文件
                    lstbxFtpResources.Items.Add(name);
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ftpClient.Connected) ftpClient.DisConnect();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("关闭Ftp链接失败\n" + ex.Message);
            }
        }

        private void button_UpLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ftpClient.Put(textBox_UpFile.Text); 
                MessageBox.Show("上传成功\n");
                List_Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传失败\n" + ex.Message);
            }
        }

        private void button_DownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = lstbxFtpResources.SelectedItem.ToString();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ftpClient.Get(currentDir+"/"+lstbxFtpResources.SelectedItem.ToString(), sfd.FileName.Substring(0, sfd.FileName.LastIndexOf("\\")), sfd.FileName.Substring(sfd.FileName.LastIndexOf("\\") + 1));
                    MessageBox.Show("下载成功\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("下载文件失败！\n" + ex.Message);
            }
        }

        private void button_SelectUp_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "所有文件|*.*";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox_UpFile.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("选择文件失败！\n" + ex.Message);
            }
        }

        private void lstbxFtpResources_DoubleClick(object sender, EventArgs e)
        {
            // 点击返回上层目录
            if (lstbxFtpResources.SelectedIndex == 0)
            {
                if (currentDir == "/")
                {
                    MessageBox.Show("当前目录已经是顶层目录", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                int index = currentDir.LastIndexOf("/");
                if (index == 0)
                {
                    currentDir = "/";
                }
                else
                {
                    currentDir = currentDir.Substring(0, index);
                }

                // 每次更改目录后立即刷新资源列表
                List_Refresh();
            }
            else
            {
                if (lstbxFtpResources.SelectedIndex > 0 && lstbxFtpResources.SelectedItem.ToString().Contains("[目录]"))
                {
                    if (currentDir == "/")
                    {
                        currentDir = "/" + lstbxFtpResources.SelectedItem.ToString().Substring(4);

                    }
                    else
                    {
                        currentDir = currentDir + "/" + lstbxFtpResources.SelectedItem.ToString().Substring(4);
                    }

                    string[] currentDirfield = currentDir.Split(' ');
                    currentDir = currentDirfield[0];
                    // 每次更改目录后立即刷新资源列表
                    List_Refresh();
                }
            }
        }

    }
}
