using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ToolKits.DataManage.Text
{
    class textManipulate
    {
        /// <summary>
        /// 从文件中读取数据
        /// </summary>
        /// <param name="path_filename"></param>
        /// <param name="strLst"></param>
        /// <returns>共读取了几行数据</returns>
        public static int ReadData(string path_filename, List<string> strLst)
        {
            using (StreamReader streamReader = new StreamReader(path_filename, Encoding.Default))
            {
                string str = null;
                int i = 0;
                while ((str = streamReader.ReadLine()) != null)
                {
                    if (str.Replace(" ", "") == "")
                        continue;
                    strLst.Add(str);
                    i++;
                }
                streamReader.Close();
                return i;
            }
        }
        //public static int ReadDataByStrContent(string content, Dictionary<string, string> dics)
        //{
        //    string[] strlst =content.Split()

        //    return i;
        //}
        /// <summary>
        /// 把内容保存到文件
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="path_filename"></param>
        public static void Save(string strContent, string path_filename)
        {
            Save(strContent, path_filename, FileMode.Create);
        }
        public static void Save(string strContent, string path_filename, FileMode fileMode)
        {

            try
            {
                //实例化一个文件流--->与写入文件相关联
                FileStream fs = new FileStream(path_filename, fileMode);
                //实例化一个StreamWriter-->与fs相关联
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                //开始写入
                sw.Write(strContent);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();


                //FileInfo finfo = new FileInfo(path_filename);
                //StreamWriter text = finfo.CreateText();
                //text.WriteLine(strContent);
                //text.Close();
            }
            catch (Exception)
            {
                //MsgModule.LogException(ex);

            }
        }

    }
}
