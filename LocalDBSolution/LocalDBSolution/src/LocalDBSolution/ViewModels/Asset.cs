using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class Asset
    {
        public string id { get; set; }
        public string Resource { get; set; }
        public int ApplyTo { get; set; }
        public List<int> Positions { get; set; }
    }
}
