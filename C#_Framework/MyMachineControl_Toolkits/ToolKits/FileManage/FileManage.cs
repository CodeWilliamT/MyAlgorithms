using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ToolKits.FileManage
{
    public class FileManage
    {
        /// <summary>
        /// 将源目录下所有文件及文件夹拷贝到指定目录下
        /// </summary>
        /// <param name="sourceDirectoryPath">源目录</param>
        /// <param name="destDirectoryPath">目标目录</param>
        public static bool CopyDirectory(string sourceDirectoryPath, string destDirectoryPath)
        {
            try
            {
                if (!Directory.Exists(sourceDirectoryPath)) return false;

                DirectoryInfo dInfo = new DirectoryInfo(sourceDirectoryPath);
                FileInfo[] fInfo = dInfo.GetFiles();
                if (!Directory.Exists(destDirectoryPath)) Directory.CreateDirectory(destDirectoryPath);
                //if (fInfo.Length < 1) return;
                foreach (var item in fInfo)
                {
                    item.CopyTo(destDirectoryPath + "\\" + item.Name, true);
                }
                foreach (var item in dInfo.GetDirectories())
                {
                    //迭代操作
                    CopyDirectory(item.FullName, destDirectoryPath + "\\" + item.Name);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 对文件夹重新排序（该函数只针对源目录下文件夹均为0，1，2...n的情况）
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool SortDirectory(string directoryPath)
        {
            try
            {
                string[] subDirs = Directory.GetDirectories(directoryPath);
                for (int i = 0; i < subDirs.Length; i++)
                {
                    if (!Directory.Exists(directoryPath + "\\" + i.ToString()))
                    {
                        int ind = i+1;
                        while (true)
                        {
                            if (Directory.Exists(directoryPath + "\\" + ind.ToString()))
                            {
                                Directory.Move(directoryPath + "\\" + ind.ToString(), directoryPath + "\\" + i.ToString());
                                break;
                            }
                            if (ind == subDirs.Length - 1)
                            {
                                break;
                            }
                            ind++;
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取指定目录及其子目录下的所有文件信息
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFileInfo(string directoryPath)
        {
            try
            {
                List<FileInfo> lFileInfo = new List<FileInfo>();
                if (!Directory.Exists(directoryPath)) return lFileInfo;
                DirectoryInfo dInfo = new DirectoryInfo(directoryPath);

                lFileInfo.AddRange(dInfo.GetFiles());

                foreach (var item in dInfo.GetDirectories())
                {
                    lFileInfo.AddRange(GetFileInfo(item.FullName));
                }

                return lFileInfo;
            }
            catch (Exception)
            {
                return new List<FileInfo>();
            }
        }
    }
    /// <summary>
    /// 按时间顺序排序文件
    /// </summary>
    public class FileOrderTimeCompare : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return x.LastWriteTime.CompareTo(y.LastWriteTime);
        }
    }
    /// <summary>
    /// 按时间反序排序文件
    /// </summary>
    public class FileRevOrderTimeCompare : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.LastWriteTime.CompareTo(x.LastWriteTime);
        }
    }
}
