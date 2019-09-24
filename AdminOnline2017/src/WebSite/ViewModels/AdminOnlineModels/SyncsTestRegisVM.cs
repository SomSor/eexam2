using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class SyncsTestRegisVM
    {
        public List<TestRegistration> TestRegistrations { get; set; }
        public List<LanguageSource> LanguageSources { get; set; }

        public string json { get; set; }
    }
}
