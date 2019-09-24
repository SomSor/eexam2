using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.ViewModels
{
    public class TestBankVMResponse
    {
        public List<ExamSheetOnSiteRespone> ExamSheets { get; set; }
        public string json { get; set; }  
        //public List<OccupationGroup> OccupationGroups { get; set; }
    }
}
