using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class ExamSuiteDetail
    {
        public string id { get; set; }
        public string TitleCode { get; set; }
        public string TitleName { get; set; }
        public int QuestionCount { get; set; }
        public string ConsiderationStatus { get; set; }
    }
}
