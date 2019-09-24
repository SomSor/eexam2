﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModelsBack
{
    public class Subject
    {
        public string _id { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsEReadiness { get; set; }

        public string ContentLanguage { get; set; }

        public string Version { get; set; }

        public int PassScore { get; set; }

        public int ExamDuration { get; set; }

        //public List<Voice> Voices { get; set; }
    }
}
