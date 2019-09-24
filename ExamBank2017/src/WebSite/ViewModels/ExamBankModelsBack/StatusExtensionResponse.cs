using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack
{
    public class StatusExtensionResponse
    {
        public string _id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Status { get; set; }
        public string ClientId { get; set; }
    }
}
