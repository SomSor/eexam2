using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack
{
    public class AssetResponse
    {
        public string id { get; set; }
        public string Resource { get; set; }
        public int ApplyTo { get; set; }
        public List<int> Positions { get; set; }
    }
}
