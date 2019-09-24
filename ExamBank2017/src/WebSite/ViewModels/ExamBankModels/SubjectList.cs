using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class SubjectList
    {
        public IEnumerable<SubjectDetail> SubjList { get; set; }
        public IEnumerable<Occupation> Occupations { get; set; }
        public IEnumerable<SubjectGroup> SubjectGroups { get; set; }
        public IEnumerable<LanguageSource> LanguageSources { get; set; }
        public IEnumerable<VoiceSource> VoiceSources { get; set; }
        //public string SelectedOccupation { get; set; }
        //public string SelecteSdubjectGroup { get; set; }
    }
}
