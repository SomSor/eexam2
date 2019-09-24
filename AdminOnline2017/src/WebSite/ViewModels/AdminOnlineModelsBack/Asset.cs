using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModelsBack
{
    public class Asset
    {
        public string id { get; set; }
        public string Resource { get; set; }
        public int ApplyTo { get; set; }
        public IEnumerable<int> Positions { get; set; }
    }
}
