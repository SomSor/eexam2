using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;
using WebSite.ViewModels.ExamBankModelsBack;
using InActiveSubject = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;

namespace WebSite.Repositories
{
    public interface IQuestionImportRepository
    {
        void InsertQuestionSuite(QuestionSuite qsuite);
        void Upsert(QuestionSuite qsuite);
        IEnumerable<QuestionSuite> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> titleCode</param>
        /// <returns></returns>
        QuestionSuiteVM GetQuestionSuite(string id);

        IEnumerable<QuestionSuiteVM> GetAllQuestionSuiteBySubjectId(string SubjectId);

        InActiveSubject.InactiveSubject GetInActiveSubject(string inactiveSubjectId);
        //void CreateInActiveSubject(InActiveSubject.InactiveSubject InactiveSubject);
        void UpsertInActiveSubject(InActiveSubject.InactiveSubject InactiveSubject);
        void InsertInActiveSubject(InActiveSubject.InactiveSubject InactiveSubject);
        void DeleteInActiveSubject(string SubjectId);

        void DeleteExamSuite(string ExamSuiteId);
        void DeleteExamSuiteBySubjectId(string SubjectId);

        void UploadAssets(IEnumerable<TheS.ExamBank.Parsers.AssetFileUtil.Asset> assetFiles, string qid, ICloudStorage cloudStorage);
        void UpdateSubjectNameAndContentLanguage(string subjectId, string subjectName, string contentLanguage);
        void UpdateSubjectCodeAndNameAndContentLanguage(string subjectId, string subjectCode, string subjectName, string contentLanguage);
        void UpdateQuestionCount(string subjectId, int QuestionCount, int ExamSuiteCount);
        void UpdateQuestionCountComment(string subjectId, int ExamSuiteAcceptCount, int ExamSuiteRejectCount);
        void UpdateFIX(QuestionSuiteVM examSuite);
        IEnumerable<QuestionSuiteVM> GetAllDamageQuestionSuite();
        void UpdateQuestionSuiteCodeAndName(string QuestionSuiteId, object Code, object Title);

        void InsertQuestion(MultipleChoiceQuestionWithOneCorrectAnswerWithId question);
        void InsertChoice(SelectableChoiceWithId choice);
        void UpdateQuestion(MultipleChoiceQuestionWithOneCorrectAnswerWithId question);
        void UpdateChoice(SelectableChoiceWithId choice);
    }
}
