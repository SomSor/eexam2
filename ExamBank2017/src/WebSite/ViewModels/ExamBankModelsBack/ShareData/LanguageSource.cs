using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ShareData
{
    public class LanguageSource
    {
        public string ExamLanguage { get; set; }
        public string ExamCode { get; set; }
        public List<VoiceSource> VoiceSources { get; set; }
    }
}
