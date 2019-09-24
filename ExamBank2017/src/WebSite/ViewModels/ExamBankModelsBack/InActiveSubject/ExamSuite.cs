using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.InActiveSubject
{
    public class ExamSuite
    {

        public string _id { get; set; }
        public string TitleCode { get; set; }
        public string TitleName { get; set; }
        public DateTime CreateTime { get; set; }
        public int SubjectId { get; set; }
        public List<Question> Questions { get; set; }
    }
}
