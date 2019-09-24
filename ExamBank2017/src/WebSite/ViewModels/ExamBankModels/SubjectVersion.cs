using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class SubjectVersion
    {
        public string id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string VersionText { get; set; }
        public bool IsUsed { get; set; }
        public string SubjectId { get; set; }
    }
}
