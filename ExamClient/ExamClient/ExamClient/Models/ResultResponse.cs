using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class ResultResponse : MVVMBase
    {
        public ExamSheetResponse ExamSheetRespones { get; set; }

        public bool IsShowAnswer { get; set; }
    }
}
