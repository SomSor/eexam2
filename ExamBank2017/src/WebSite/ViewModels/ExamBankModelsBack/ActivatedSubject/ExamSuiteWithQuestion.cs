using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject
{
    public class ExamSuiteWithQuestion
    {
        public string _id { get; set; }
        public string TitleName { get; set; }
        public string TitleCode { get; set; }
        public DateTime CreateDateTime { get; set; }
        public List<QuestionPool> Questions { get; set; }
        //public List<int> QuestionGroupIds { get; set; }
    }
}
