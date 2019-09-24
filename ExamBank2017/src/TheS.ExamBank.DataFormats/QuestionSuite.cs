using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheS.ExamBank.DataFormats
{
    /// <summary>
    /// A set of question for a specific random condition
    /// </summary>
    /// <remarks>It's equivalent to a markdown file.</remarks>
    public class QuestionSuite
    {
        public string _id { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }
        public string SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Level { get; set; }
        public string LayoutCode { get; set; }
        public string Description { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}
