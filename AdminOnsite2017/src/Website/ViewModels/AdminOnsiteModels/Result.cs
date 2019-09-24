using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModels
{
    public class Result
    {
        public string _id { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string Status { get; set; }
        public int TestCount { get; set; }
        public string PID { get; set; }
        public string ExamNumber { get; set; }
        public int CorrectCount { get; set; }
        public int InCorrectCount { get; set; }
        public DateTime ExamDateTime { get; set; }
        public string CenterNameTH { get; set; }
    }
}
