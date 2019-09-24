using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.InActiveSubject
{
    public class ExamSuiteGroup
    {
        public string _id { get; set; }
        public string ExamSuiteGroupName { get; set; }
        public int? PassScore { get; set; }
        public int? ExamDuration { get; set; }
        public bool IsUsed { get; set; }
        public List<ExamSuiteGroupMap> ExamSuiteGroupMaps { get; set; }

    }
}
