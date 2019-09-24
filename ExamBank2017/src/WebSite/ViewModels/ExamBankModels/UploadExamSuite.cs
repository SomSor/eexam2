using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class UploadExamSuite
    {
        public string TempFileName { get; set; }
        public string OccupationGroupName { get; set; }
        public string SubjectGroupName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string TitleCode { get; set; }
        public string TitleName { get; set; }
        public int QuestionCount { get; set; }
        public string Content { get; set; }
    }
}
