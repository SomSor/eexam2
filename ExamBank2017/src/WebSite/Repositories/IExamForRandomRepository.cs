using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareData = WebSite.ViewModels.ExamBankModelsBack.ShareData;
using ActivatedSubject = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;

namespace WebSite.Repositories
{
    public interface IExamForRandomRepository
    {
        ShareData.ActivatedSubject GetActivatedSubjectBySubjectCode(string subjectCode, string contentLanguage);
        ActivatedSubject.Subject GetInUseSubjectBySubjectId(string subjectId);
        List<ActivatedSubject.ExamSuiteWithQuestion> MapQuestion(List<ActivatedSubject.ExamSuite> examSuiteList);
        void CreatePreExamSheet(List<ActivatedSubject.ExamSheet> ExamSheets);
    }
}
