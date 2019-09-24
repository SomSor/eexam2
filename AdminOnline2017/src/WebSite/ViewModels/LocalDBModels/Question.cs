using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.LocalDBModels
{
    public class Question
    {
        public string _id { get; set; }
        public string ExamCode { get; set; }
        public int QuestionNumber { get; set; }
        public bool IsAllowRandomChoice { get; set; }
        public string Detail { get; set; }
        public List<Choice> Choices { get; set; }
        public string GroupId { get; set; }
        public Choice UserAnswer { get; set; }
        public List<Asset> Assets { get; set; }
    }
}
