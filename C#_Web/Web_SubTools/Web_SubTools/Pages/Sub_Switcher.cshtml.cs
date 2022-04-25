using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_SubTools.Models;
using Utils;

namespace Web_SubTools.Pages
{
    public class Sub_SwitcherModel : PageModel
    {
        [BindProperty]
        public Model_Sub_Switcher Model_Sub_Switcher { get; set; }

        public string switchedResult { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            int extIdx = Model_Sub_Switcher.SubFormFile.FileName.LastIndexOf('.');
            int len = Model_Sub_Switcher.SubFormFile.FileName.Length;
            string ext = Model_Sub_Switcher.SubFormFile.FileName.Substring(extIdx, len- extIdx).ToLower();
            StringBuilder newsubtext = new StringBuilder();
            switch (ext)
            {
                case ".ass":
                    {
                        using (StreamReader sr = new StreamReader(Model_Sub_Switcher.SubFormFile.OpenReadStream()))
                        {
                            SubHelper.switchAssSubStr(sr, newsubtext);
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(Model_Sub_Switcher.SubFormFile.OpenReadStream()))
                        {
                            SubHelper.switchSrtSubStr(sr, newsubtext);
                            break;
                        }
                    }
                default:
                    {
                        break;
                    }
            }
            switchedResult = newsubtext.ToString();
            return Page();
        }

    }
}
