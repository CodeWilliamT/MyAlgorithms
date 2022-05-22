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
        public List<string> listLanguages { get; set; }
        public string switchedResult { get; set; }
        public string fileWarning { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await Task.Run(new Action(() =>
            {
                List<StringBuilder> rst;
                try
                {
                    using (StreamReader sr = new StreamReader(Model_Sub_Translator.SubFormFile.OpenReadStream()))
                    {
                        rst = SubHelper.TranslateSubTextStream(Model_Sub_Translator.SubFormFile.FileName, sr, (SubHelper.SubType)0);
                    }
                }
                catch(Exception ex)
                {
                    switchedResult = ex.Message;
                    return;
                }  
                switchedResult = rst[0].ToString();
            }));
            return Page();
        }

    }
}
