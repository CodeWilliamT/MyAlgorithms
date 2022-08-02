using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sub_Translator
{
    public static class Program
    {
        public static readonly Dictionary<string, string> Language = new Dictionary<string, string>() {
            { "English (United States)", "en-US" }, { "Chinese Simplified", "zh-CN" }
        };
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Sub_Translator());
        }
    }
}
