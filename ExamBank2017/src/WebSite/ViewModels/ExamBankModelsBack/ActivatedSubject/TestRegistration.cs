﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject
{
    public class TestReigstration
    {
        public string _id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public string ExamLanguage { get; set; }

        public string VoiceLanguage { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime ExpriedDate { get; set; }

        public string SiteId { get; set; }

        public string CenterId { get; set; }

        public bool ForTestSystem { get; set; }

        public bool ForPractice { get; set; }

        public string Status { get; set; }

        public string PID { get; set; }

        public string ExamNumber { get; set; }

        public string ExamPeriod { get; set; }

        public DateTime AppointDate { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public int MaxCount { get; set; }
    }
}