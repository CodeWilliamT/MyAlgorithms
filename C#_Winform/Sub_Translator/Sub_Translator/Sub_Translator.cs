using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Sub_Translator
{
    public partial class Sub_Translator : Form
    {
        public Sub_Translator()
        {
            InitializeComponent();
            foreach(var e in MSTranslatorHelper.Language)
            {
                comboBoxFrom.Items.Add(e.Key);
                comboBoxTo.Items.Add(e.Key);
            }
            comboBoxFrom.SelectedItem = "English";
            comboBoxTo.SelectedItem = "Chinese Simplified";
            tbSubfilepath.Text = ConfigurationManager.AppSettings["Subfilepath"];
            tbSubfoldername.Text = ConfigurationManager.AppSettings["Subfoldername"];
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

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "操作中";
            MSTranslatorHelper.From = comboBoxFrom.Text;
            MSTranslatorHelper.To = comboBoxTo.Text;
            FileInfo fi = new FileInfo(tbSubfilepath.Text);
            string savepath = tbSubfilepath.Text;
            if (!cbReplace.Checked)
            {
                savepath = fi.DirectoryName + @"\" + fi.Name.Replace(fi.Extension, "") + "_Trans" + fi.Extension;
            }
            MSTranslatorHelper.TranslateSubTextFile(tbSubfilepath.Text, savepath);
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

        private void btnFolderTranslate_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "操作中";
            MSTranslatorHelper.From = comboBoxFrom.Text;
            MSTranslatorHelper.To = comboBoxTo.Text;
            string savefolder = tbSubfoldername.Text;
            if (!cbReplace.Checked)
            {
                savefolder = tbSubfoldername.Text + "_Trans";
            }
            DirectoryInfo di = new DirectoryInfo(tbSubfoldername.Text);
            FileInfo[] di_FileInfo = di.GetFiles("*", SearchOption.AllDirectories);
            if (di_FileInfo.Count() == 0)
            {
                MessageBox.Show("该目录下没有字幕文件");
                return;
            }
            btnTranslate.Enabled = false;
            btnFolderTranslate.Enabled = false;
            Task.Run(new Action(() =>
            {
                foreach (FileInfo f in di_FileInfo)
                {
                    string savepath = savefolder + @"\" +
                        f.FullName.Substring(tbSubfoldername.Text.Length, f.FullName.Length - tbSubfoldername.Text.Length);
                    Directory.CreateDirectory(savepath.Substring(0, savepath.Length - f.Name.Length));
                    MSTranslatorHelper.TranslateSubTextFile(f.FullName, savepath);
                    Thread.Sleep(1000);
                }
                SaveConfig();
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    toolStripStatusLabel1.Text = "操作完成";
                    btnTranslate.Enabled = true;
                    btnFolderTranslate.Enabled = true;
                }));
                Task.Run(new Action(() =>
                {
                    Thread.Sleep(2000);
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        toolStripStatusLabel1.Text = "准备";
                    }));
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
