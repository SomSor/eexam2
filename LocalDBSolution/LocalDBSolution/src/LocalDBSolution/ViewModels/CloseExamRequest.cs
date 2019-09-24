using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class CloseExamRequest
    {
        public List<ExamSheetOnline> ResultSheet { get; set; }

        public List<TestRegistrationOnline> MissTestReg { get; set; }
    }
}
