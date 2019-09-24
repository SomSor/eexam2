using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class Subject
    {
        public string id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string ContentLanguage { get; set; }
        public string Version { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<ExamSuiteDetail> ExamSuites { get; set; }
        public IEnumerable<ExamSuiteGroup> ExamSuiteGroups { get; set; }
        public IEnumerable<SubjectVersion> VersionList { get; set; }
        public IEnumerable<VoiceLanguage> VoiceLanguageList { get; set; }      
    }
}
