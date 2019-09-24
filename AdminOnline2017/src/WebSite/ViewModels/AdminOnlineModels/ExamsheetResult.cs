using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class ExamsheetResult
    {
        public string id { get; set; }
        public string LatestStatus { get; set; }
        public int TestCount { get; set; }
        public int Score { get; set; }
        public TestRegistration TestRegis { get; set; }
        public DateTime ExamDateTime { get; set; }
    }
}
