using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class PreExamResponse : MVVMBase
    {
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string PID { get; set; }
        public string ExamNumber { get; set; }
        public List<SubjectResponse> SubjectRespones { get; set; }
    }
}
