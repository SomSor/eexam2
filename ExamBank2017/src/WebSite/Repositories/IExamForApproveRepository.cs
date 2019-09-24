using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;
using Share = WebSite.ViewModels.ExamBankModelsBack.ShareData;
using Activated = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;
using TheS.ExamBank.DataFormats;

namespace WebSite.Repositories
{
    public interface IExamForApproveRepository
    {
        //Other Method
        string GetConsiderationStatus(ViewModels.ExamBankModelsBack.QuestionSuiteVM es);
        Guid GetNewGuid(string x);
        Guid GetNewQId(string x);
        Guid GetNewSubId(string x);

        //InActive
        IEnumerable<InActive.InactiveSubject> ListInActiveSubject(string siteId);
        IEnumerable<InActive.Consideration> ListConsiderationByExamSuiteId(string ExamSuiteId);
        InActive.InactiveSubject GetInActiveSubject(string subjectId);
        QuestionSuite GetInActiveExamsuite(string ExamSuiteId);
        void CreateConsideration(InActive.Consideration Consideration);
        void CreateManyConsideration(IEnumerable<InActive.Consideration> Considerations);
        void UpsertInactiveSubject(InActive.InactiveSubject inactiveSubject);
        void DeleteAllInSubject(string SubjectId);
        void DeleteAllConsiderration(string ExamSuiteId);
        void DeleteAllConsiderration(IEnumerable<string> ExamSuiteIds);

        //Share
        IEnumerable<Share.OccupationGroup> ListOccupationGroup(string siteId);
        IEnumerable<Share.ActivatedSubject> ListActivatedSubject(string siteId);
        Share.ActivatedSubject GetActivatedSubjectBySubjectId(string subjectId);
        Share.ActivatedSubject GetActivatedSubjectBySubjectCode(string subjectCode, string Language);
        void CreateActivatedSubject(Share.ActivatedSubject activatedSubject);
        void UpdateActivatedSubject(Share.ActivatedSubject activatedSubject);
        void UpsertActivatedSubject(Share.ActivatedSubject activatedSubject);
        void ReplaceActivatedSubject(Share.ActivatedSubject activatedSubject);

        //Activated
        IEnumerable<Activated.Subject> ListSubject(IEnumerable<string> subjectId);
        IEnumerable<Activated.Subject> ListSubjectBySubjectCode(string subjectCode, string Language);
        IEnumerable<Activated.Question> ListAllQuestionByQID(IEnumerable<string> QID, string examSuiteCode, string Language);
        Activated.Subject GetSubject(string subjectId);
        Activated.Question GetQuestionByQID(string QID, string examSuiteCode, string Language);
        Activated.Question GetQuestionByQID(string QID);
        void CreateSubject(Activated.Subject subject);
        void CreateQuestion(IEnumerable<Activated.Question> questions);
        void UpsertSubject(Activated.Subject subject);
        void UpsertActivedQuestion(Activated.Question question);
        //void UpsertQuestion(IEnumerable<Activated.Question> questions);
    }
}
