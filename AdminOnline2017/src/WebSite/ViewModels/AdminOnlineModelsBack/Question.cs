using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModelsBack
{
    public class Question
    {
        public string _id { get; set; }

        public int QuestionNumber { get; set; }

        public bool IsAllowRandomChoice { get; set; }

        public string Detail { get; set; }

        public List<Choice> Choices { get; set; }

        public string GroupId { get; set; }

        public Choice UserAnswer { get; set; }

        public IEnumerable<Asset> Assets { get; set; }
        public string ExamCode { get; set; }
    }
}
