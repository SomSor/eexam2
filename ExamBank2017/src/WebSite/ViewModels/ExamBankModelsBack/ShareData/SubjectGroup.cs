using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ShareData
{
    public class SubjectGroup
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public List<string> SubjectCodes { get; set; }

    }
}
