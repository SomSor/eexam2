using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.ViewModels.AdminOnsiteModels;

namespace WebSite.ViewModels.AdminOnsiteModelsForClient
{
    public class ExamSheetRespone
    {
        public string _id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PID { get; set; }
        public string ExamNumber { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public bool IsEReadiness { get; set; }
        public string ContentLanguage { get; set; }
        public int TestCount { get; set; }
        public string LastedStatus { get; set; }
        public int PassScore { get; set; }
        public int CorrectScore { get; set; }
        public int InCorrectScore { get; set; }
        public int ExamDuration { get; set; }
        public int ReviewDuration { get; set; }
        public DateTime ActiveThruDateTime { get; set; }
        public string ClientId { get; set; }
        public List<Question> Questions { get; set; }

        public MessageRespone Message { get; set; }
    }
}
