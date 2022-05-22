using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utils;

namespace LeadframeAOI
{
    class Log
    {
        public string logPath;
        public string diry;
        private string fileFormat;
        public string fileContent = "";
        private string[] files;
        /// <summary>
        /// 初始化文件的路径和类型,by LYH
        /// </summary>
        /// <param name="paraDiry"></param>
        /// <param name="paraFilePatern"></param>
        public Log(string diry, string fileFormat)
        {
            this.diry = diry;
            this.fileFormat = fileFormat;
        }
        /// <summary>
        /// 获取指定文件路径下所有特定文件类型的一组文件名，保存在数组中，by LYH
        /// </summary>
        /// <returns></returns>
        public List<String> Read_FilName()
        {
            List<String> FilName = new List<String>();
            try
            {
                files = Directory.GetFiles(diry, fileFormat);
                files.OrderBy(fl => new FileInfo(fl).CreationTime);
                FileInfo file = null;
                for (int i = 0; i < files.Length; i++)
                {
                    file = new FileInfo(files[i]);
                    FilName.Add(file.Name);
                }
            }
            catch (Exception exp)
            {
                LOG.Debug(exp);
            }
            return FilName;
        }

        /// <summary>
        /// 读取特定路径下特定文本格式文件的内容，读取所有，by LYH
        /// </summary>
        public void DisplayLog()
        {
            try
            {
                using (FileStream fS = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sR = new StreamReader(fS, System.Text.Encoding.Default))
                    {
                        fileContent = sR.ReadToEnd();
                    }
                }
            }
            catch (Exception exp)
            {
                LOG.Debug(exp);
            }
        }
    }
}
