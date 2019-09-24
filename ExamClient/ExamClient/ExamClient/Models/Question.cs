using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class Question : MVVMBase
    {
        public string _id { get; set; }
        public int QuestionNumber { get; set; }
        public bool IsAllowRandomChoice { get; set; }
        public string Detail { get; set; }
        public List<Choice> Choices { get; set; }
        public string GroupId { get; set; }
        public Choice UserAnswer { get; set; }
    }
}
