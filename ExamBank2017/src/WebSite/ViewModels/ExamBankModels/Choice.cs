using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class Choice
    {
        public string id { get; set; }
        public string Detail { get; set; }
        public bool IsCorrect { get; set; }
        public IEnumerable<VoiceSource> Voices { get; set; }
    }
}
