using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class TestResultResponse
    {
        public string id { get; set; }
        public int totalsTest { get; set; }
        public int totalsTestPerson { get; set; }
        public int totalsPassTest { get; set; }
        public int totalsFailTest { get; set; }
        public int percentagePassTest { get; set; }
        public int percentageFailTest { get; set; }
        public int normalTest { get; set; }
        public int retest { get; set; }
        public string centerName { get; set; }
        public List<ExamsheetResult> examsheets { get; set; }
    }
}
