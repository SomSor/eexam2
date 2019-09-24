using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.LocalDBModels
{
    public class Voice
    {
        public string _id { get; set; }
        public string Language { get; set; }
        public string URL { get; set; }
        public string LanguageCode { get; set; }
    }
}
