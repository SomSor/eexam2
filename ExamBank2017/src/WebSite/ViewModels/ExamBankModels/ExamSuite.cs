using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class ExamSuite
    {
        public string id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string TitleCode { get; set; }
        public string TitleName { get; set; }
        public string SubjectId { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        //public IEnumerable<QuestionGroup> QuestionGroups { get; set; }
    }
}
