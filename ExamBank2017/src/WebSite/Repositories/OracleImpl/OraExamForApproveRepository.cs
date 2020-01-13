using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;
using WebSite.ViewModels.ExamBankModelsBack;
using WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;
using WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;
using WebSite.ViewModels.ExamBankModelsBack.ShareData;

namespace WebSite.Repositories.OracleImpl
{
    public class OraExamForApproveRepository : IExamForApproveRepository
    {

        public void CreateActivatedSubject(ActivatedSubject activatedSubject)
        {
            throw new NotImplementedException();
        }

        public void CreateConsideration(Consideration Consideration)
        {
            throw new NotImplementedException();
        }

        public void CreateManyConsideration(IEnumerable<Consideration> Considerations)
        {
            throw new NotImplementedException();
        }

        public void CreateQuestion(IEnumerable<ViewModels.ExamBankModelsBack.ActivatedSubject.Question> questions)
        {
            throw new NotImplementedException();
        }

        public void CreateSubject(ViewModels.ExamBankModelsBack.ActivatedSubject.Subject subject)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllConsiderration(string ExamSuiteId)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllConsiderration(IEnumerable<string> ExamSuiteIds)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllInSubject(string SubjectId)
        {
            throw new NotImplementedException();
        }

        public void DeleteConsiderration(string Id)
        {
            throw new NotImplementedException();
        }

        public ActivatedSubject GetActivatedSubjectBySubjectCode(string subjectCode, string Language)
        {
            throw new NotImplementedException();
        }

        public ActivatedSubject GetActivatedSubjectBySubjectId(string subjectId)
        {
            throw new NotImplementedException();
        }

        public string GetConsiderationStatus(QuestionSuiteVM es)
        {
            var _questionCount = es.Questions?.Count() ?? 0;
            var _considerations = this.ListConsiderationByExamSuiteId(es._id);

            var _lastConsiderationForQuestions = _considerations
                ?.GroupBy(con => con.QuestionNumber)
                ?.Select(conG => conG?.OrderByDescending(con => con.CreateDateTime).FirstOrDefault())?.ToList();
            var _acceptCount = _lastConsiderationForQuestions?.Count(con => con.IsAccept) ?? 0;
            var _rejectCount = _lastConsiderationForQuestions?.Count(con => !con.IsAccept) ?? 0;

            var _ConsiderationStatus = string.Empty;
            if (_rejectCount > 0) _ConsiderationStatus = "Rejected";
            else if (_questionCount == _acceptCount) _ConsiderationStatus = "Accepted";
            else _ConsiderationStatus = "Wait";

            return _ConsiderationStatus;
        }

        public QuestionSuite GetInActiveExamsuite(string ExamSuiteId)
        {
            throw new NotImplementedException();
        }

        public InactiveSubject GetInActiveSubject(string subjectId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {OracleDbTableName.InactiveSubject} WHERE Id=:Id";
                cmd.Parameters.Add("Id", subjectId);
                cmd.CommandType = CommandType.Text;

                var da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                var ds = new DataSet();
                da.Fill(ds);
                var dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return OracleUtility.GetInactiveSubject(dt.Rows[0]);
                }

                return null;
            }
        }

        public Guid GetNewGuid(string x)
        {
            return Guid.NewGuid();
        }

        public Guid GetNewQId(string x)
        {
            return Guid.NewGuid();
        }

        public Guid GetNewSubId(string x)
        {
            return Guid.NewGuid();
        }

        public ViewModels.ExamBankModelsBack.ActivatedSubject.Question GetQuestionByQID(string QID, string examSuiteCode, string Language)
        {
            throw new NotImplementedException();
        }

        public ViewModels.ExamBankModelsBack.ActivatedSubject.Question GetQuestionByQID(string QID)
        {
            throw new NotImplementedException();
        }

        public ViewModels.ExamBankModelsBack.ActivatedSubject.Subject GetSubject(string subjectId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ActivatedSubject> ListActivatedSubject(string siteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewModels.ExamBankModelsBack.ActivatedSubject.Question> ListAllQuestionByQID(IEnumerable<string> QID, string examSuiteCode, string Language)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Consideration> ListConsiderationByExamSuiteId(string ExamSuiteId)
        {
            return Enumerable.Empty<Consideration>();
        }

        public IEnumerable<InactiveSubject> ListInActiveSubject(string siteId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {OracleDbTableName.InactiveSubject} WHERE SiteId=:SiteId";
                cmd.Parameters.Add("SiteId", siteId);
                cmd.CommandType = CommandType.Text;

                var da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                var ds = new DataSet();
                da.Fill(ds);
                var dt = ds.Tables[0];

                var subjects = Enumerable.Empty<InactiveSubject>().ToList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    subjects.Add(OracleUtility.GetInactiveSubject(dt.Rows[i]));
                }
                return subjects;
            }
        }

        public IEnumerable<OccupationGroup> ListOccupationGroup(string siteId)
        {
            return Enumerable.Empty<OccupationGroup>();
        }

        public IEnumerable<ViewModels.ExamBankModelsBack.ActivatedSubject.Subject> ListSubject(IEnumerable<string> subjectId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewModels.ExamBankModelsBack.ActivatedSubject.Subject> ListSubjectBySubjectCode(string subjectCode, string Language)
        {
            throw new NotImplementedException();
        }

        public void ReplaceActivatedSubject(ActivatedSubject activatedSubject)
        {
            throw new NotImplementedException();
        }

        public void UpdateActivatedSubject(ActivatedSubject activatedSubject)
        {
            throw new NotImplementedException();
        }

        public void UpdateConsideration(Consideration Consideration)
        {
            throw new NotImplementedException();
        }

        public void UpsertActivatedSubject(ActivatedSubject activatedSubject)
        {
            throw new NotImplementedException();
        }

        public void UpsertActivedQuestion(ViewModels.ExamBankModelsBack.ActivatedSubject.Question question)
        {
            throw new NotImplementedException();
        }

        public void UpsertInactiveSubject(InactiveSubject inactiveSubject)
        {
            throw new NotImplementedException();
        }

        public void UpsertSubject(ViewModels.ExamBankModelsBack.ActivatedSubject.Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
