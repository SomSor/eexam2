using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject
{
    public class VoiceLanguage
    {
        public string _id { get; set; }
        public string Language { get; set; }
        public string LanguageCode { get; set; }
        public bool IsUsed { get; set; }
    }
}
