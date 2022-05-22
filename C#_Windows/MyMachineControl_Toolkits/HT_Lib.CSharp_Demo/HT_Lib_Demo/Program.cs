using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HT_Lib;
namespace WindowsFormsApp1
{
    static class Program
    {
        static HTUi.MainUi mainWin = null;        
        private static void _LoadData()
        {
            Console.WriteLine("_LoadData called!");
            int step = 20;
            int progress = 0;

            //模拟加载数据过程中的进度条
            while (progress < 100)
            {
                System.Threading.Thread.Sleep(1000);
                mainWin.UpdateProgressBar(progress, progress.ToString());
                progress += step;
            }            
        }
        private static bool userLogin(string u, string p)
        {
            Console.WriteLine("Login called!");
            HTUi.TipHint("DSFD");
            System.Threading.Thread.Sleep(1000);
            //配置记录中的用户
            HTLog.Configs.User = u;
            return true;
        }

        private static void _UnloadData()
        {
            Console.WriteLine("_UnloadData called!");
            System.Threading.Thread.Sleep(2000);
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            mainWin = new HTUi.MainUi();        
            //配置HTLog 如果要使用HTLog.Ui，必须在加载Ui之前配置Log文件所在的文件夹
            //HTLog.Configs.Console = true;
            //HTLog.Configs.Folder = "D://Logs";

            //绑定外部事件，即每次使用HTLog记录信息都会调用该函数。
            HTLog.LogMessageEvent += mainWin.AddMessage;
            //启动HTLog服务，服务会异步将信息记录到本地。
            HTLog.StartService();


            //配置界面，可以通过xml文件或者 配置用的类的实例来配置
            mainWin.SetupUi(@"t1.xml");
            //绑定事件
            mainWin.MainUiLoadEvent += _LoadData;
            mainWin.MainUiLoginEvent += userLogin;
            mainWin.MainUiCloseEvent += _UnloadData;

            //测试UI的消息记录框添加信息
            mainWin.AddMessage("DD1{0}", 123);
            mainWin.AddMessage("DD2_{0}", Math.PI);
            mainWin.AddMessage("DD3");
            mainWin.AddMessage("DD4");
            mainWin.AddMessage("DD5");

                       
            Application.EnableVisualStyles();
            Application.Run(mainWin);
            //关闭HTLog服务
            HTLog.StopService();
        }
    }
}
