using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class ExamSuiteGroup
    {
        public string id { get; set; }
        public string ExamSuiteGroupName { get; set; }
        public bool IsUsed { get; set; }
        public int PassScore { get; set; }
        public int ExamDuration { get; set; }
        public int QuestionCount { get; set; }
        public IEnumerable<ExamSuiteGroupMap> ExamSuiteGroupMaps { get; set; }
        public string SubjectId { get; set; }
    }
}
