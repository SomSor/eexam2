using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class ExamSheetOnline
    {        
        public string _id { get; set; }
        public Subject Subject { get; set; }
        public TestRegistrationOnline TestReg{ get; set; }
        public int TestCount { get; set; }
        public string LatestStatus { get; set; }
        public DateTime? ExamDateTime { get; set; }
        public List<StatusExtension> StatusExtensions { get; set; }
        public List<Question> RandomQuestion { get; set; }
        public string CenterId { get; set; }
        public int CorrectScore { get; set; }
        public int InCorrectScore { get; set; }
        public int ReviewDuration { get; set; }     
        public DateTime CreateDate { get; set; }
        public string ClientId { get; set; }
    }
}
