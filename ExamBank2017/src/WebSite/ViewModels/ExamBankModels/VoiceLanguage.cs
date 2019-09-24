using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class VoiceLanguage
    {
        public string id { get; set; }
        public string Language { get; set; }
        public string LanguageCode { get; set; }
        public bool IsUsed { get; set; }
    }
}
