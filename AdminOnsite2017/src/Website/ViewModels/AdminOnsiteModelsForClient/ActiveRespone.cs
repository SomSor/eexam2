using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModelsForClient
{
    public class ActiveRespone
    {
        public DateTime ActiveDateTime{ get; set; }
        public DateTime ActiveThruTime { get; set; }
        public bool IsActive { get; set; }
    }
}
