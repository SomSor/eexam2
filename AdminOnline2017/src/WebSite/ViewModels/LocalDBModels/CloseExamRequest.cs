using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.LocalDBModels
{
    public class CloseExamRequest
    {
        public List<ExamSheet> ResultSheet { get; set; }

        public List<TestRegistration> MissTestReg { get; set; }
    }
}
