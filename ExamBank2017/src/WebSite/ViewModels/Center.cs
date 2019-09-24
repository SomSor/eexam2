using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels
{
    public class Center
    {
        public string _id { get; set; }
        public string NameTH { get; set; }
        public string NameEN { get; set; }
        public string SiteId { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public List<CertData> CertDatas { get; set; }
    }
    public class CertData
    {
        public string UserCode { get; set; }
        public string CertNo { get; set; }
        public string CertYear { get; set; }
    }
}
