using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class ExamSheetRequest
    {
        public string SubjectCode { get; set; }
        public string ExamLanguage { get; set; }
        public string VoiceLanguage { get; set; }
        public int Quantity { get; set; }
        public string CenterId{ get; set; }
    }
}
