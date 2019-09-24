using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheS.ExamBank.DataFormats
{
    public class Asset
    {
        public string Resource { get; set; }
        public int ApplyTo { get; set; }
        public IEnumerable<int> Positions { get; set; }
    }
}
