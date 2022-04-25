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
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using Utils;

namespace DoubleSubSwicher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbSubfilepath.Text = ConfigurationManager.AppSettings["Subfilepath"];
            tbSubfoldername.Text = ConfigurationManager.AppSettings["Subfoldername"];
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { "只主次交换", "去原主行交换", "去原次行交换" });
            comboBox1.SelectedIndex = 0;
        }
        private void SaveConfig()
        {
            AddUpdateAppSettings("Subfilepath", tbSubfilepath.Text);
            AddUpdateAppSettings("Subfoldername", tbSubfoldername.Text);
        }
        #region event
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"文本文档|*.txt;*.ass;*.srt;
                        |所有文件|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbSubfilepath.Text = ofd.FileName;
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "操作中";
            FileInfo fi = new FileInfo(tbSubfilepath.Text);
            string savepath = tbSubfilepath.Text;
            if (!cbReplace.Checked)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        savepath = fi.DirectoryName + @"\" + fi.Name.Replace(fi.Extension, "") + "_EnCh" + fi.Extension;
                        break;
                    case 1:
                        savepath = fi.DirectoryName + @"\" + fi.Name.Replace(fi.Extension, "") + "_Sub" + fi.Extension; ;
                        break;
                    case 2:
                        savepath = fi.DirectoryName + @"\" + fi.Name.Replace(fi.Extension, "") + "_Main" + fi.Extension;
                        break;
                    default:
                        break;
                }
            }
            SubHelper.switchSubTextFile(tbSubfilepath.Text, savepath);
            SaveConfig();
            toolStripStatusLabel1.Text = "操作完成";
            Task.Run(new Action(() =>
            {
                Thread.Sleep(2000);
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    toolStripStatusLabel1.Text = "准备";
                }));
            }));
        }

        private void btnFolderBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbSubfoldername.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbSubfoldername.Text = fbd.SelectedPath;
            }
        }

        private void btnFolderSwitch_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "操作中";
            string savefolder = tbSubfoldername.Text;

            if (!cbReplace.Checked)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        savefolder = tbSubfoldername.Text + "_EnCh";
                        break;
                    case 1:
                        savefolder = tbSubfoldername.Text + "_Sub";
                        break;
                    case 2:
                        savefolder = tbSubfoldername.Text + "_Main";
                        break;
                    default:
                        break;
                }
            }
            DirectoryInfo di = new DirectoryInfo(tbSubfoldername.Text);
            FileInfo[] di_FileInfo = di.GetFiles("*", SearchOption.AllDirectories);
            if (di_FileInfo.Count() == 0)
            {
                MessageBox.Show("该目录下没有字幕文件");
                return;
            }
            Directory.CreateDirectory(savefolder);
            foreach (FileInfo f in di_FileInfo)
            {
                string savepath = savefolder + @"\" +
                    f.FullName.Substring(tbSubfoldername.Text.Length, f.FullName.Length - tbSubfoldername.Text.Length);
                Directory.CreateDirectory(savepath.Substring(0, savepath.Length - f.Name.Length));
                SubHelper.switchSubTextFile(f.FullName, savepath);
            }
            SaveConfig();
            toolStripStatusLabel1.Text = "操作完成";
            Task.Run(new Action(() =>
            {
                Thread.Sleep(2000);
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    toolStripStatusLabel1.Text = "准备";
                }));
            }));

        }
        #endregion

        #region function
        private void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        #endregion
    }
}
