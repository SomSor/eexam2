using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;
using Share = WebSite.ViewModels.ExamBankModelsBack.ShareData;
using Activated = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;
using MongoDB.Driver;
using TheS.ExamBank.DataFormats;

namespace WebSite.Repositories.MongoImpl
{
    public class ExamForApproveRepository : IExamForApproveRepository
    {
        private MongoHelper helper;

        #region Table name

        public string InactiveSubject_InactiveSubject = "InactiveSubject.InactiveSubject";
        public string InactiveSubject_Consideration = "InactiveSubject.Consideration";
        public string InactiveSubject_ExamSuite = "InactiveSubject.ExamSuite";

        public string ShareData_ActivatedSubject = "ShareData.ActivatedSubject";
        public string ShareData_OccupationGroup = "ShareData.OccupationGroup";
        public string ShareData_Site = "ShareData.Site";
        public string ShareData_Center = "ShareData.Center";
        public string ShareData_TestRegistration = "ShareData.TestRegistration";
        public string ShareData_ExamSheet = "ShareData.ExamSheet";

        public string ActivatedSubject_Subject = "ActivatedSubject.Subject";
        public string ActivatedSubject_Question = "ActivatedSubject.Question";
        public string ActivatedSubject_QuestionGroup = "ActivatedSubject.QuestionGroup";
        public string ActivatedSubject_VoiceSource = "ActivatedSubject.VoiceSource";

        #endregion Table name

        //Constructor
        public ExamForApproveRepository(MongoHelper helper)
        {
            this.helper = helper;
        }

        #region Other method

        public string GetConsiderationStatus(ViewModels.ExamBankModelsBack.QuestionSuiteVM es)
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

        #endregion Other method

        #region InActived

        public IEnumerable<InActive.InactiveSubject> ListInActiveSubject(string siteId)
        {
            var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
            var result = coltn.Find(Builders<InActive.InactiveSubject>.Filter.Eq(x => x.SiteId, siteId)).ToList();
            
            return result ?? new List<InActive.InactiveSubject>();
        }

        public IEnumerable<InActive.Consideration> ListConsiderationByExamSuiteId(string ExamSuiteId)
        {
            var coltn = helper.GetCollection<InActive.Consideration>(InactiveSubject_Consideration);

            var result = coltn.Find(Builders<InActive.Consideration>.Filter.Eq(it => it.ExamSuiteId, ExamSuiteId)).ToList();
            return result ?? new List<InActive.Consideration>();
        }

        public InActive.InactiveSubject GetInActiveSubject(string subjectId)
        {
            var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);

            var result = coltn.Find(Builders<InActive.InactiveSubject>.Filter.Eq(it => it._id, subjectId));
            return result?.FirstOrDefault();
        }

        public QuestionSuite GetInActiveExamsuite(string ExamSuiteId)
        {
            var coltn = helper.GetCollection<QuestionSuite>(InactiveSubject_InactiveSubject);

            var result = coltn.Find(Builders<QuestionSuite>.Filter.Eq(it => it._id, ExamSuiteId));
            return result?.FirstOrDefault();
        }

        public void CreateConsideration(InActive.Consideration Consideration)
        {
            if (Consideration != null)
            {
                var coltn = helper.GetCollection<InActive.Consideration>(InactiveSubject_Consideration);
                coltn.InsertOne(Consideration);
            }
            else
            {
                throw new ArgumentNullException("null input from Consideration");
            }
        }

        public void CreateManyConsideration(IEnumerable<InActive.Consideration> Considerations)
        {
            if (Considerations.Any())
            {
                var coltn = helper.GetCollection<InActive.Consideration>(InactiveSubject_Consideration);
                coltn.InsertMany(Considerations);
            }
            else
            {
                throw new ArgumentNullException("null input from Considerations");
            }
        }

        public void UpsertInactiveSubject(InActive.InactiveSubject inactiveSubject)
        {
            if (inactiveSubject != null)
            {
                var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.ReplaceOne(it => it._id == inactiveSubject._id, inactiveSubject, opt);
            }
            else
            {
                throw new ArgumentNullException("null input from inactiveSubject ");
            }
        }

        public void DeleteAllInSubject(string SubjectId)
        {
            if (!string.IsNullOrEmpty(SubjectId))
            {
                var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
                var filter = Builders<InActive.InactiveSubject>.Filter.Eq("_id", SubjectId);
                var result = coltn.DeleteMany(filter);

                var coltn2 = helper.GetCollection<QuestionSuite>(InactiveSubject_ExamSuite);
                //var filter2 = Builders<QuestionSuite>.Filter.Eq("SubjectCode", SubjectCode);
                var filter2 = Builders<QuestionSuite>.Filter.Eq(it => it.SubjectId, SubjectId);

                var result2 = coltn2.DeleteMany(filter2);
                //coltn.DeleteMany(x => x.SubjectCode == SubjectCode);
            }
            else
            {
                throw new ArgumentNullException("null input from SubjectId");
            }
        }

        public void DeleteAllConsiderration(string ExamSuiteId)
        {
            if (!string.IsNullOrEmpty(ExamSuiteId))
            {
                var coltn = helper.GetCollection<InActive.Consideration>(InactiveSubject_Consideration);
                //var filter = Builders<InActive.Consideration>.Filter.Eq("_id", titleCode);
                //var result = coltn.DeleteMany(filter);
                var result = coltn.DeleteMany(con => con.ExamSuiteId == ExamSuiteId);
            }
            else
            {
                throw new ArgumentNullException("null input from ExamSuiteId");
            }
        }

        public void DeleteAllConsiderration(IEnumerable<string> ExamSuiteIds)
        {
            if (ExamSuiteIds.Any())
            {
                var coltn = helper.GetCollection<InActive.Consideration>(InactiveSubject_Consideration);
                //var filter = Builders<InActive.Consideration>.Filter.Eq("_id", titleCode);
                var result = coltn.DeleteMany(con => ExamSuiteIds.Contains(con.ExamSuiteId));
            }
            else
            {
                throw new ArgumentNullException("null input from ExamSuiteId");
            }
        }

        #endregion InActived

        #region Shared

        public IEnumerable<Share.OccupationGroup> ListOccupationGroup(string siteId)
        {
            var coltn = helper.GetCollection<Share.OccupationGroup>(ShareData_OccupationGroup);
            var result = coltn.Find(Builders<Share.OccupationGroup>.Filter.Eq(it => it.SiteId, siteId)).ToList();

            return result ?? new List<Share.OccupationGroup>();
        }

        public IEnumerable<Share.ActivatedSubject> ListActivatedSubject(string siteId)
        {
            var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);
            var result = coltn.Find(Builders<Share.ActivatedSubject>.Filter.Eq(x => x.SiteId, siteId)).ToList();

            return result ?? new List<Share.ActivatedSubject>();
        }

        public Share.ActivatedSubject GetActivatedSubjectBySubjectId(string subjectId)
        {
            var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);

            var result = coltn.Find(Builders<Share.ActivatedSubject>.Filter.Eq(it => it.SubjectId, subjectId));
            return result?.FirstOrDefault();
        }

        public Share.ActivatedSubject GetActivatedSubjectBySubjectCode(string subjectCode, string Language)
        {
            var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);

            var result = coltn.Find(it => it.SubjectCode == subjectCode && it.ContentLanguage == Language);
            //var result = coltn.Find(Builders<Share.ActivatedSubject>.Filter.Eq(it => it.SubjectCode, subjectCode));
            return result?.FirstOrDefault();

        }

        public void CreateActivatedSubject(Share.ActivatedSubject activatedSubject)
        {
            if (activatedSubject != null)
            {
                var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.InsertOne(activatedSubject);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject  ");
            }
        }

        public void UpdateActivatedSubject(Share.ActivatedSubject activatedSubject)
        {
            if (activatedSubject != null)
            {
                var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.ReplaceOne(it => it._id == activatedSubject._id, activatedSubject, opt);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject  ");
            }
        }

        public void UpsertActivatedSubject(Share.ActivatedSubject activatedSubject)
        {
            if (activatedSubject != null)
            {
                var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.ReplaceOne(it => it._id == activatedSubject._id, activatedSubject, opt);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject  ");
            }
        }

        public void ReplaceActivatedSubject(Share.ActivatedSubject activatedSubject)
        {
            if (activatedSubject != null)
            {
                var coltn = helper.GetCollection<Share.ActivatedSubject>(ShareData_ActivatedSubject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.ReplaceOne(it => it._id == activatedSubject._id, activatedSubject, opt);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject  ");
            }
        }

        #endregion Shared

        #region  Activated

        public IEnumerable<Activated.Subject> ListSubject(IEnumerable<string> subjectId)
        {

            var coltn = helper.GetCollection<Activated.Subject>(ActivatedSubject_Subject);

            var result = coltn.Find(x => subjectId.Contains(x._id)).ToList();

            return result ?? new List<Activated.Subject>();
        }

        public IEnumerable<Activated.Subject> ListSubjectBySubjectCode(string subjectCode, string Language)
        {
            var coltn = helper.GetCollection<Activated.Subject>(ActivatedSubject_Subject);
            var result = coltn.Find(Builders<Activated.Subject>.Filter.Eq(it => it.SubjectCode, subjectCode)).ToList();
            return result ?? new List<Activated.Subject>();
        }

        public IEnumerable<Activated.Question> ListAllQuestionByQID(IEnumerable<string> QID, string examSuiteCode, string Language)
        {
            var coltn = helper.GetCollection<Activated.Question>(ActivatedSubject_Question);

            var result = coltn.Find(x => QID.Contains(x._id)).ToList() ?? new List<Activated.Question>();

            return this.helper.GetQuestion(result, examSuiteCode, Language);
        }

        public IEnumerable<Activated.Question> ListAllQuestionByQID(IEnumerable<string> QID)
        {
            var coltn = helper.GetCollection<Activated.Question>(ActivatedSubject_Question);

            var result = coltn.Find(x => QID.Contains(x._id)).ToList();

            return result ?? new List<Activated.Question>();
        }

        public Activated.Subject GetSubject(string subjectId)
        {
            var coltn = helper.GetCollection<Activated.Subject>(ActivatedSubject_Subject);

            var result = coltn.Find(Builders<Activated.Subject>.Filter.Eq(it => it._id, subjectId));

            return result?.FirstOrDefault();
        }

        public Activated.Question GetQuestionByQID(string QID, string examSuiteCode, string Language)
        {
            var coltn = helper.GetCollection<Activated.Question>(ActivatedSubject_Question);

            var result = coltn.Find(Builders<Activated.Question>.Filter.Eq(it => it._id, QID));
            if (result == null)
            {
                return null;
            }
            return this.helper.GetQuestion(result.ToList(), examSuiteCode, Language)?.FirstOrDefault();
        }

        public Activated.Question GetQuestionByQID(string QID)
        {
            var coltn = helper.GetCollection<Activated.Question>(ActivatedSubject_Question);

            var result = coltn.Find(Builders<Activated.Question>.Filter.Eq(it => it._id, QID));
            if (result == null)
            {
                return null;
            }
            return result?.FirstOrDefault();
        }

        public void CreateSubject(Activated.Subject subject)
        {
            if (subject != null)
            {
                var coltn = helper.GetCollection<Activated.Subject>(ActivatedSubject_Subject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                //coltn.ReplaceOne(it => it._id == subject._id, subject, opt);
                coltn.InsertOne(subject);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject subject ");
            }
        }

        public void CreateQuestion(IEnumerable<Activated.Question> questions)
        {
            if (questions.Count() > 0)
            {
                foreach (var question in questions)
                {
                    if (question != null)
                    {
                        var coltn = helper.GetCollection<Activated.Question>(ActivatedSubject_Question);
                        var opt = new UpdateOptions()
                        {
                            IsUpsert = true,
                        };
                        //coltn.ReplaceOne(it => it._id == question._id, question, opt);
                        coltn.InsertOne(question);
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject subject ");
            }
        }

        public void UpsertSubject(Activated.Subject subject)
        {
            if (subject != null)
            {
                var coltn = helper.GetCollection<Activated.Subject>(ActivatedSubject_Subject);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.ReplaceOne(it => it._id == subject._id, subject, opt);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject subject ");
            }
        }

        public void UpsertActivedQuestion(Activated.Question question)
        {
            if (question != null)
            {
                var coltn = helper.GetCollection<Activated.Question>(ActivatedSubject_Question);
                var opt = new UpdateOptions()
                {
                    IsUpsert = true,
                };
                coltn.ReplaceOne(it => it._id == question._id, question, opt);
            }
            else
            {
                throw new ArgumentNullException("null input from ActivatedSubject Question ");
            }
        }

        //public void UpsertActivatedSubject(Share.ActivatedSubject activatedSubject)
        //{
        //    if (activatedSubject != null)
        //    {
        //        var coltn = helper.GetCollection<Share.ActivatedSubject>(ActivatedSubject_Question);
        //        var opt = new UpdateOptions()
        //        {
        //            IsUpsert = true,
        //        };
        //        coltn.ReplaceOne(it => it._id == activatedSubject._id, activatedSubject, opt);
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException("null input from ActivatedSubject  ");
        //    }
        //}

        #endregion  Activated

    }
}
