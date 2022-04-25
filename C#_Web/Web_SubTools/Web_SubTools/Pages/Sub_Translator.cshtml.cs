using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utils;
using Web_SubTools.Models;

namespace Web_SubTools.Pages
{
    public class Sub_TranslatorModel : PageModel
    {
        [BindProperty]
        public Model_Sub_Translator Model_Sub_Translator { get; set; }

        public string switchedResult { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            int extIdx = Model_Sub_Translator.SubFormFile.FileName.LastIndexOf('.');
            int len = Model_Sub_Translator.SubFormFile.FileName.Length;
            string ext = Model_Sub_Translator.SubFormFile.FileName.Substring(extIdx, len - extIdx).ToLower();
            StringBuilder newsubtext = new StringBuilder();
            switch (ext)
            {
                case ".ass":
                    {
                        using (StreamReader sr = new StreamReader(Model_Sub_Translator.SubFormFile.OpenReadStream()))
                        {
                            SubHelper.switchAssSubStr(sr, newsubtext);
                            break;
                        }
                    }
                case ".srt":
                    {
                        using (StreamReader sr = new StreamReader(Model_Sub_Translator.SubFormFile.OpenReadStream()))
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
            switchedResult = MSTranslatorHelper.TranslateText("Hello");
            return Page();
        }

    }
}
