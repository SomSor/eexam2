using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject
{
    public class Subject
    {
        public string _id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreateDateTime { get; set; }   
        public string Version { get; set; }      
        public List<ExamSuite> ExamSuites { get; set; }
        public List<ExamSuiteGroup> ExamSuiteGroups { get; set; }
        public List<VoiceLanguage> VoiceLanguages { get; set; }
        public string ContentLanguage { get; set; }
    }
}
