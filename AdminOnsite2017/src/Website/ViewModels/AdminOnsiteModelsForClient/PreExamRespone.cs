using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModelsForClient
{
    public class PreExamRespone
    {
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string PID { get; set; }
        public string ExamNumber { get; set; }
        public List<SubjectRespone> SubjectRespones { get; set; }
    }
}
