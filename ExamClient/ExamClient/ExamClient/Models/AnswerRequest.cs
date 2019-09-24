using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class AnswerRequest : MVVMBase
    {
        public string ExamSheetId { get; set; }
        public string PID { get; set; }
        public string ClientId { get; set; }
        public string Qid { get; set; }
        public string ChoiceId { get; set; }
    }
}
