using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class SubjectDetail
    {
        public string id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string ContentLanguage { get; set; }
        public int ExamSuiteCount { get; set; }
        public int QuestionCount { get; set; }
        public int ExamSuiteAcceptCount { get; set; }
        public int ExamSuiteRejectCount { get; set; }
        public string Version { get; set; }
        public bool IsDisabled { get; set; }
        public string SubjectGroupId { get; set; }
    }
}
