using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LensDriverController.Logic
{
    public static class FileReader
    {
        public static string GetTextFileContents(string path)
        {
            string readText = String.Empty;
            try
            {
                readText = System.IO.File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return readText;
        }
    }
}
