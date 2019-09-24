using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class Question
    {
        public string id { get; set; }
        public int QuestionNumber { get; set; }
        public bool IsAllowRandomChoice { get; set; }
        public string Detail { get; set; }
        public string GroupId { get; set; }
        public IEnumerable<Choice> Choices { get; set; }
        public IEnumerable<Consideration> Considerations { get; set; }
        public IEnumerable<VoiceSource> Voices { get; set; }
        public string ExamSuiteId { get; set; }
    }
}
