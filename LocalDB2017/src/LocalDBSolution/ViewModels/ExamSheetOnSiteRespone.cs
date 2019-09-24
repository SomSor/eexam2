using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class ExamSheetOnSiteRespone
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string ExamLanguage { get; set; }
        public string VoiceLanguage { get; set; }
        public int Quantity { get; set; }
        public int Book { get; set; }
        public string Version { get; set; }
        public bool IsExamEnough { get; set; }

        public string _id { get; set; }
        public string OccupationName { get; set; }
    }
}
