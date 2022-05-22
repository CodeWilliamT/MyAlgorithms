using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    public class MediaHelper
    {
        /// <summary>
        /// 执行一条command命令
        /// </summary>
        /// <param name="command">需要执行的Command</param>
        /// <param name="output">输出</param>
        /// <param name="error">错误</param>
        public static void ExecuteCommand(string command, out string output, out string error)
        {
            try
            {
                //创建一个进程
                Process pc = new Process();
                pc.StartInfo.FileName = command;
                pc.StartInfo.UseShellExecute = false;
                pc.StartInfo.RedirectStandardOutput = true;
                pc.StartInfo.RedirectStandardError = true;
                pc.StartInfo.CreateNoWindow = true;

                //启动进程
                pc.Start();

                //准备读出输出流和错误流
                string outputData = string.Empty;
                string errorData = string.Empty;
                pc.BeginOutputReadLine();
                pc.BeginErrorReadLine();

                pc.OutputDataReceived += (ss, ee) =>
                {
                    outputData += ee.Data;
                };

                pc.ErrorDataReceived += (ss, ee) =>
                {
                    errorData += ee.Data;
                };

                //等待退出
                pc.WaitForExit();

                //关闭进程
                pc.Close();

                //返回流结果
                output = outputData;
                error = errorData;
            }
            catch (Exception)
            {
                output = null;
                error = null;
            }
        }
        /// 获取视频的帧宽度和帧高度
        /// </summary>
        /// <param name="videoFilePath">mov文件的路径</param>
        /// <returns>null表示获取宽度或高度失败</returns>
        public static void GetMovWidthAndHeight(string videoFilePath, out int width, out int height)
        {
            try
            {
                //判断文件是否存在
                if (!File.Exists(videoFilePath))
                {
                    width = -1;
                    height = -1;
                }

                //执行命令获取该文件的一些信息 
                string ffmpegPath = new FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName + @"\ffmpeg.exe";

                string output;
                string error;
                ExecuteCommand("\"" + ffmpegPath + "\"" + " -i " + "\"" + videoFilePath + "\"", out output, out error);
                if (string.IsNullOrEmpty(error))
                {
                    width = -1;
                    height = -1;
                }

                //通过正则表达式获取信息里面的宽度信息
                Regex regex = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
                Match m = regex.Match(error);
                if (m.Success)
                {
                    width = int.Parse(m.Groups[1].Value);
                    height = int.Parse(m.Groups[2].Value);
                }
                else
                {
                    width = -1;
                    height = -1;
                }
            }
            catch (Exception)
            {
                width = -1;
                height = -1;
            }
        }
    }
}
