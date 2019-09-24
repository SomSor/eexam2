using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class AnswerRequest
    {
        public string ExamSheetId { get; set; }
        public string PID { get; set; }
        public string ClientId { get; set; }
        public string QID { get; set; }
        public string ChoiceId { get; set; }
    }
}
