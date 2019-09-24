using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class Question
    {
        [BsonId]
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
