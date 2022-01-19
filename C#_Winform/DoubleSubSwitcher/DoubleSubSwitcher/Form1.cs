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

namespace DoubleSubSwicher
{
    public partial class Form1 : Form
    {
        Configuration config;
        public Form1()
        {
            InitializeComponent();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
            switchSubText(tbSubfilepath.Text, savepath);
            SaveConfig();
            MessageBox.Show("操作完成");

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
            FileInfo[] di_FileInfo = di.GetFiles();
            if (di_FileInfo.Count() == 0)
            {
                MessageBox.Show("该目录下没有字幕文件");
                return;
            }
            Directory.CreateDirectory(savefolder);
            foreach (FileInfo f in di_FileInfo)
            {
                string savepath = savefolder + @"\" + f.Name;
                switchSubText(f.FullName, savepath);
            }
            SaveConfig();
            MessageBox.Show("操作完成");
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

        private void switchSubText(string filepath, string savepath)
        {
            FileInfo fi = new FileInfo(filepath);
            StringBuilder newsubtext = new StringBuilder();
            Encoding ecd= GetFileEncodeType(filepath);
            string substr, newsubstr;
            switch (fi.Extension.ToLower())
            {
                case ".ass":
                    {
                        using (StreamReader sr = new StreamReader(filepath,ecd))
                        {
                            substr = sr.ReadLine();
                            while (substr != null)
                            {
                                newsubstr = switchAssSubStr(substr);
                                newsubtext.Append(newsubstr + "\n");
                                substr = sr.ReadLine();
                            }
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(filepath, ecd))
                        {
                            substr = sr.ReadLine();
                            while (substr != null)
                            {
                                if (substr == "")
                                {
                                    newsubtext.Append(substr + "\n");
                                    substr = sr.ReadLine();
                                    newsubtext.Append(substr + "\n");
                                    substr = sr.ReadLine();
                                    newsubtext.Append(substr + "\n");
                                    substr = sr.ReadLine();
                                    newsubstr = substr;
                                    substr = sr.ReadLine();
                                    if (comboBox1.SelectedIndex != 2)
                                    {
                                        newsubtext.Append(substr + "\n");
                                    }
                                    if (comboBox1.SelectedIndex != 1)
                                    {
                                        newsubtext.Append(newsubstr + "\n");
                                    }
                                    substr = sr.ReadLine();
                                }
                                else
                                {
                                    newsubtext.Append(substr + "\n");
                                    substr = sr.ReadLine();
                                }
                            }
                            break;
                        }
                    }
                default:
                    {
                        break;
                    }
            }
            using (StreamWriter sw = new StreamWriter(savepath, false, ecd))
            {
                sw.Write(newsubtext);
                sw.Flush();
                sw.Close();
            }
        }
        private string switchAssSubStr(string substr)
        {
            string resultstr = substr;
            string chsstr, enustr;
            int a, b, c, d;
            string maska = @",";
            string maskb = @"\N";
            string maskc = @"}";
            b = substr.IndexOf(maskb);
            if (b == -1)
                goto _end;
            a = b;
            while (substr.Substring(0, a).LastIndexOf(maska) == substr.Substring(0, a).LastIndexOf(maska + " "))
            {
                a = substr.Substring(0, a).LastIndexOf(maska + " ") - maska.Length;
            }
            a = substr.Substring(0, a).LastIndexOf(maska) + maska.Length;
            c = substr.LastIndexOf(maskc);
            if (c == -1 || c == substr.Length - 1)
                c = b+ maskb.Length;
            else
                c = c + maskc.Length;
            d = substr.Length;
            if (a > b || b > c)
                goto _end;
            chsstr = substr.Substring(a, b - a);
            enustr = substr.Substring(c, d - c);
            if (comboBox1.SelectedIndex == 1 && enustr != "")
            {
                chsstr = "";
                resultstr = substr.Substring(0, a) + enustr + substr.Substring(b, c - b).Replace(@"\N", "") + chsstr;
                goto _end;
            }
            if (comboBox1.SelectedIndex == 2 && chsstr != "")
            {
                enustr = chsstr;
                chsstr = "";
                b = b + 2;
                resultstr = substr.Substring(0, a) + enustr + substr.Substring(b, c - b).Replace(@"\N", "") + chsstr;
                goto _end;
            }
            resultstr = substr.Substring(0, a) + enustr + substr.Substring(b, c - b) + chsstr;
        _end:
            return resultstr;
        }

        public System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            
            if (buffer[0] == 0x0A && buffer[1] == 0x0A)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0xEF && buffer[1] == 0xBB)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0x5b && buffer[1] == 0x53)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
            {
                return System.Text.Encoding.BigEndianUnicode;
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
            {
                return System.Text.Encoding.Unicode;
            }
            else
            {
                return System.Text.Encoding.UTF8;
            }
        }
        #endregion
    }
}
