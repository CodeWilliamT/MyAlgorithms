using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTM_BSP;

namespace CameraZaxisScanModel
{
    static class Program
    {
        public static MainWindow mainWindow = null;
        public static Vision obj_Vision = new Vision();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainWindow = new MainWindow();
            Application.Run(mainWindow);
            AppDomain.CurrentDomain.UnhandledException += delegate
            {
                HTM.Discard();
                obj_Vision.CloseAllCamera();
            };
        }
    }
}
