using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack
{
    public class QuestionResponse
    {
        //[BsonId]
        public string _id { get; set; }
        public string ExamCode { get; set; }
        public int QuestionNumber { get; set; }
        public bool IsAllowRandomChoice { get; set; }
        public string Detail { get; set; }
        public List<ChoiceResponse> Choices { get; set; }
        public string GroupId { get; set; }
        public ChoiceResponse UserAnswer { get; set; }
        public List<AssetResponse> Assets { get; set; }
    }
}
