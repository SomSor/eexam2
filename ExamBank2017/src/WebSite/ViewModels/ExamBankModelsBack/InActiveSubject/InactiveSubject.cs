using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.InActiveSubject

{
    public class InactiveSubject
    {
        public string _id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsEReadiness { get; set; }
        public string ContentLanguage { get; set; }
        public string SiteId { get; set; }
        public int ExamSuiteCount { get; set; }
        public int QuestionCount { get; set; }
        public int ExamSuiteAcceptCount { get; set; }
        public int ExamSuiteRejectCount { get; set; }
        public List<ExamSuiteGroup> ExamSuiteGroups { get; set; }
    }
}
