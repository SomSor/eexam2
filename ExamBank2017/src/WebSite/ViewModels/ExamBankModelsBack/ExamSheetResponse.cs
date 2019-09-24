using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModelsBack
{
    public class ExamSheetResponse
    {
        //[BsonId]
        public string _id { get; set; }
        public SubjectResponse Subject { get; set; }
        public string TestRegisID { get; set; }
        public string PID { get; set; }
        public int TestCount { get; set; }
        public string LatestStatus { get; set; }
        public DateTime? ExamDateTime { get; set; }
        public List<StatusExtensionResponse> StatusExtensions { get; set; }
        public List<QuestionResponse> RandomQuestions { get; set; }
        public string CenterId { get; set; }
        public DateTime? VertifiedDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? DoneDateTime { get; set; }
        public DateTime? ActiveThruDateTime { get; set; }
        public int CorrectScore { get; set; }
        public int InCorrectScore { get; set; }
        public int ReviewDuration { get; set; }
        public bool IsSync { get; set; }
        public bool IsCloseExam { get; set; }
        public DateTime CreateDate { get; set; }
        public string ClientId { get; set; }
    }
}
