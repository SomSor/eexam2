using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModelsBack
{
    public class StatusExtension
    {
        public string _id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public string ClientId { get; set; }
    }
}
