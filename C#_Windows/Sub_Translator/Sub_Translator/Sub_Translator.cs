using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using System.Globalization;
using System.Reflection;
using Sub_Translator.Resources;

namespace Sub_Translator
{
    public partial class Sub_Translator : Form
    {
        private ResourceManager rsm;
        public Sub_Translator()
        {
            InitializeComponent();
            foreach (var e in TranslatorHelper.Language)
            {
                comboBoxFrom.Items.Add(e.Key);
                comboBoxTo.Items.Add(e.Key);
            }

            foreach (var e in Enum.GetNames(typeof(SubHelper.SubType)))
            {
                comboBox_Format.Items.Add(e);
            }
            foreach (var e in TranslatorHelper.TranslateServer)
            {
                comboBox_Server.Items.Add(e);
            }
            foreach (var e in Program.Language)
            { 
                comboBox_language.Items.Add(e.Key);
            }
            comboBox_language.SelectedItem = CultureInfo.CurrentUICulture.EnglishName;
            InitializeDisplayText();
            comboBox_Server.SelectedIndex = 0;
            comboBox_Format.SelectedIndex = 0;
            comboBoxFrom.SelectedItem = "English";
            comboBoxTo.SelectedItem = "Chinese Simplified";
            tbSubfilepath.Text = AppConfigHelper.LoadKey("Subfilepath");
            tbSubfoldername.Text = AppConfigHelper.LoadKey("Subfoldername");
        }

        public string GetString(string str)
        {
            if (rsm == null)
            {
                rsm = new ResourceManager(CommonResources.ResourceManager.BaseName, Assembly.GetExecutingAssembly());
            }
            string val = rsm.GetString(str);
            return val == null ? str : val;
        }
        private void InitializeDisplayText()
        {
            CultureInfo.CurrentUICulture = new CultureInfo(Program.Language[comboBox_language.SelectedItem.ToString()],false);
            lb_path.Text = GetString("Subtitle Path:");
            lb_folder.Text = GetString("Subtitle Folder:");
            lb_server.Text = GetString("Server:");
            lb_from.Text = GetString("From:");
            lb_to.Text = GetString("To:");
            lb_format.Text = GetString("Format:");
            lb_language.Text = GetString("Language:");
            btnBrowse.Text= GetString("Browse");
            btnFolderBrowse.Text = GetString("Browse");
            btnTranslate.Text = GetString("Translate");
            btnFolderTranslate.Text = GetString("Translate");
            toolStripStatusLabel1.Text = GetString("Ready");
        }
        private void SaveConfig()
        {
            AppConfigHelper.UpdateKey("Subfilepath", tbSubfilepath.Text);
            AppConfigHelper.UpdateKey("Subfoldername", tbSubfoldername.Text);
        }
        #region event
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File" + @"|*.txt;*.ass;*.srt;
                        |所有文件|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbSubfilepath.Text = ofd.FileName;
            }
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            if (!File.Exists(tbSubfilepath.Text))
            {
                MessageBox.Show(GetString("No file In this path."));
                return;
            }
            toolStripStatusLabel1.Text = GetString("Operating");
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = 25;
            btnTranslate.Enabled = false;
            btnFolderTranslate.Enabled = false;
            int idx_Format = comboBox_Format.SelectedIndex;
            string str_From = comboBoxFrom.Text;
            string str_To = comboBoxTo.Text;
            int idx_Server = comboBox_Server.SelectedIndex;
            FileInfo fi = new FileInfo(tbSubfilepath.Text);
            string savepath = fi.DirectoryName + @"\" + fi.Name.Replace(fi.Extension, "") + "_Trans" + "."+ ((SubHelper.SubType)idx_Format).ToString();
            Task.Run(new Action(() =>
            {
                try
                {
                    SubHelper.TranslateSubTextFile(tbSubfilepath.Text, savepath, (SubHelper.SubType)idx_Format, str_From, str_To, idx_Server);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                SaveConfig();
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    toolStripProgressBar1.Value = 100;
                    toolStripStatusLabel1.Text = GetString("Complete");
                    btnTranslate.Enabled = true;
                    btnFolderTranslate.Enabled = true;
                }));
                Task.Run(new Action(() =>
                {
                    Thread.Sleep(2000);
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        toolStripProgressBar1.Visible = false;
                        toolStripProgressBar1.Value = 0;
                        toolStripStatusLabel1.Text = GetString("Ready");
                    }));
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
            DirectoryInfo di = new DirectoryInfo(tbSubfoldername.Text);
            FileInfo[] di_FileInfo = di.GetFiles("*", SearchOption.AllDirectories);
            if (di_FileInfo.Count() == 0)
            {
                MessageBox.Show(GetString("No file under this folder."));
                return;
            }
            toolStripStatusLabel1.Text = "Operating";
            toolStripProgressBar1.Visible = true;
            btnTranslate.Enabled = false;
            btnFolderTranslate.Enabled = false;
            int idx_Format = comboBox_Format.SelectedIndex;
            string str_From = comboBoxFrom.Text;
            string str_To = comboBoxTo.Text;
            int idx_Server = comboBox_Server.SelectedIndex;
            if(tbSubfoldername.Text[tbSubfoldername.Text.Length-1] =='\\')
                tbSubfoldername.Text=tbSubfoldername.Text.Remove(tbSubfoldername.Text.Length - 1);
            string savefolder = tbSubfoldername.Text + "_Trans";
            Task.Run(new Action(() =>
            {
                try
                {
                    foreach (FileInfo f in di_FileInfo)
                    {
                        string savepath = savefolder + @"\" +
                            f.FullName.Substring(tbSubfoldername.Text.Length, f.FullName.Length - tbSubfoldername.Text.Length - f.Extension.Length) + "." + ((SubHelper.SubType)idx_Format).ToString();
                        Directory.CreateDirectory(savepath.Substring(0, savepath.Length - f.Name.Length));
                        try
                        {
                            SubHelper.TranslateSubTextFile(f.FullName, savepath, (SubHelper.SubType)idx_Format, str_From, str_To, idx_Server);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(f.FullName+"\n"+ex.Message);
                        }

                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            toolStripProgressBar1.Value += 100 / di_FileInfo.Length;
                        }));
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                SaveConfig();
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    toolStripProgressBar1.Value = 100;
                    toolStripStatusLabel1.Text = GetString("Complete");
                    btnTranslate.Enabled = true;
                    btnFolderTranslate.Enabled = true;
                }));
                Task.Run(new Action(() =>
                {
                    Thread.Sleep(2000);
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        toolStripProgressBar1.Value = 0;
                        toolStripProgressBar1.Visible = false;
                        toolStripStatusLabel1.Text = GetString("Ready");
                    }));
                }));
            }));

        }
        #endregion

        private void Sub_Translator_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            tbSubfilepath.Text = files[0];
        }

        private void Sub_Translator_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None; 
        }

        private void comboBox_language_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeDisplayText();
        }
    }
}
