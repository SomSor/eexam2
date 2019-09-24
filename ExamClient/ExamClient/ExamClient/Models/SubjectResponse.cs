using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
     public class SubjectResponse : MVVMBase
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
    }
}
