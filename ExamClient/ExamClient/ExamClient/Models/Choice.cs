using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class Choice : MVVMBase
    {
        public string _id { get; set; }
        public string Detail { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
