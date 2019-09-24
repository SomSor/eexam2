using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModels
{
    public class Center
    {
        public string _id { get; set; }
        public string NameTh { get; set; }
        public string NameEn { get; set; }
        public List<CertData> CertDatas { get; set; }   
        public DateTime UpdateDateTime { get; set; }
        public string SiteName { get; set; }
        public string SiteCode { get; set; }
       
    }
}
