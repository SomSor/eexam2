using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class Occupation
    {
        public string id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SubjectGroup> SubjectGroups { get; set; }

    }
}
