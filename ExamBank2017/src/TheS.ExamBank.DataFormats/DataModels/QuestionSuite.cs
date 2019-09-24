using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats.Attributes;

namespace TheS.ExamBank.DataFormats.DataModels
{
    [DataModel]
    public class QuestionSuite
    {
        public string _id { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Level { get; set; }
        public string LayoutCode { get; set; }
        public string Description { get; set; }
        public int? PickingUpCount { get; set; }

        public IEnumerable<QuestionRef> Questions { get; set; }
    }
}
