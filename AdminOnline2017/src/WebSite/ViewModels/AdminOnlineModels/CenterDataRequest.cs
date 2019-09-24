using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.ViewModels.AdminOnlineModelsBack;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class CenterDataRequest
    {
        public string _id { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public List<CertData> CertDatas { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string LatestUser { get; set; }
        public string LatestPass { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SiteName { get; set; }
        public string SiteId { get; set; }
        public bool IsAutoSyncScore { get; set; }
        public bool IsAutoSyncResult { get; set; }
        public bool IsShowAnswer { get; set; }
    }
}
