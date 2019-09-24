using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ShareData
{
    public class ActivatedSubject
    {
        public string _id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectId { get; set; }
        public string SiteId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public bool IsEReadiness { get; set; }
        public DateTime? DisabledDateTime { get; set; }
        public string ContentLanguage { get; set; }
        public string ContentText { get; set; }
    }
}
