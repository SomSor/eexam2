using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;

namespace WebSite
{
    public class ActivatedSubjectComparer : IEqualityComparer<WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroup>
    {
        public bool Equals(ExamSuiteGroup x, ExamSuiteGroup y)
        {
            return x.ExamSuiteGroupName == y.ExamSuiteGroupName;
        }

        public int GetHashCode(ExamSuiteGroup obj)
        {
            return 99;
        }
    }
}
