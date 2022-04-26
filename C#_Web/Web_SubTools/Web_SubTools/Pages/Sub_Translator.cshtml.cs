using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utils;
using Web_SubTools.Models;
using System.Globalization;

namespace Web_SubTools.Pages
{
    public class Sub_TranslatorModel : PageModel
    {
        [BindProperty]
        public Model_Sub_Translator Model_Sub_Translator { get; set; }
        public SelectList SListLanguages { get; set; }
        List<string> listLanguages;
        public string switchedResult { get; set; }
        public string fileWarning { get; set; }
        public void OnGet()
        {
            listLanguages = new List<string>();
            foreach (var e in MSTranslatorHelper.Language)
            {
                listLanguages.Add(e.Key);
            }
            SListLanguages = new SelectList(listLanguages);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            MSTranslatorHelper.From = Model_Sub_Translator.From;
            MSTranslatorHelper.To = Model_Sub_Translator.To;
            int extIdx = Model_Sub_Translator.SubFormFile.FileName.LastIndexOf('.');
            int len = Model_Sub_Translator.SubFormFile.FileName.Length;
            string ext = Model_Sub_Translator.SubFormFile.FileName.Substring(extIdx, len - extIdx).ToLower();
            StringBuilder newsubtext = new StringBuilder();
            await Task.Run(new Action(() =>
            {
                switch (ext)
                {
                    case ".ass":
                        {
                            using (StreamReader sr = new StreamReader(Model_Sub_Translator.SubFormFile.OpenReadStream()))
                            {
                                MSTranslatorHelper.TranslateAssSubStr(sr, newsubtext);
                                break;
                            }
                        }
                    case ".srt":
                        {
                            using (StreamReader sr = new StreamReader(Model_Sub_Translator.SubFormFile.OpenReadStream()))
                            {
                                MSTranslatorHelper.TranslateSrtSubStr(sr, newsubtext);
                                break;
                            }
                        }
                    default:
                        {
                            fileWarning = string.Format(CultureInfo.CurrentCulture, "The input file must be .ass or .srt file!");
                            break;
                        }
                }
                switchedResult = newsubtext.ToString();
            }));
            return Page();
        }

    }
}
