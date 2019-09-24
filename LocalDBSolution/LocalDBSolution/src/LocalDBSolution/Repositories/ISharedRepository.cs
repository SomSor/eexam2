using LocalDBSolution.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDBSolution.Repositories
{
    public interface ISharedRepository
    {
        List<ExamSheet> GetLocalExamSheetBySubjectCode(string subjectCode, string contentlanguage, string voicelanguage, string centerId, string version);
        List<ExamSheet> GetAllLocalExamSheetBySubjectCode(string centerId);


        //AdminOnsite job
        void CreateExamSheet(List<ExamSheet> examSheets);
        void CreateTestRegistration(List<TestRegistration> testregists);

        //Get available exam sheet
        List<ExamSheet> GetAvailableExamSheet(string centerId);
        //Get local testregistration
        List<TestRegistration> GetAvailableTestRegis(string centerId);

        List<ExamSheet> GetDifferentAvailableSheet(string centerId, string subjectcode, string version);
        void CloseDifSheets(List<ExamSheet> sheets);


        List<TestRegistration> GetLocalTestRegis(string centerId);

        void CreateCenter(Center center);
    }
}
