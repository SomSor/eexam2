﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class SubjectCode
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<LanguageSource> LanguageSources { get; set; }
    }
}
