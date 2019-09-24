using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ShareData
{
    public class Site
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsAutoSyncScore { get; set; }
        public bool IsAutoSyncResult { get; set; }

    }
}
