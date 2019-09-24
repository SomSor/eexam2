using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModelsBack
{
    public class Question
    {
        public string _id { get; set; }
        public string ExamCode { get; set; }
        public int QuestionNumber { get; set; }
        public bool IsAllowRandomChoice { get; set; }
        public string Detail { get; set; }
        public List<Choice> Choices { get; set; }
        public int GroupId { get; set; }
        public Choice UseAnswer { get; set; }
        public IEnumerable<Asset> Assets { get; set; }

    }
}
