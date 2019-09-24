using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModelsBack
{
    public class ActiveData
    {
        public string _id { get; set; }
        public DateTime ActiveFromDateTime { get; set; }
        public DateTime ActiveThruDateTime { get; set; }
    }
}
