using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class ExamSuiteGroupMap
    {
        public string id { get; set; }
        public string ExamSuiteId { get; set; }
        public int RandomCount { get; set; }
        public string ExamSuiteGroupId { get; set; }
        public string SubjectId { get; set; }
    }
}
