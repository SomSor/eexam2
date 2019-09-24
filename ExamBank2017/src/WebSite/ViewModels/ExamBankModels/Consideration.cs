using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class Consideration
    {
        public string id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string RejectComment { get; set; }
        public bool IsAccept { get; set; }
        public string UserName { get; set; }
        public string ExamSuiteId { get; set; }
        public string TitleCode { get; set; }
        public int QuestionNumber { get; set; }
    }
}
