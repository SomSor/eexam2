using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class Choice
    {
        public string _id { get; set; }
        public string Detail { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
