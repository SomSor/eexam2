using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareData = WebSite.ViewModels.ExamBankModelsBack.ShareData;
using ActivatedSubject = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;
using MongoDB.Driver;

namespace WebSite.Repositories.MongoImpl
{
    public class ExamForRandomRepository : IExamForRandomRepository
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
        public ExamForRandomRepository(MongoHelper helper)
        {
            this.helper = helper;
        }

        public ShareData.ActivatedSubject GetActivatedSubjectBySubjectCode(string subjectCode, string contentLanguage)
        {
            var coltn = helper.GetCollection<ShareData.ActivatedSubject>(ShareData_ActivatedSubject);
            var result = coltn.Find(x => x.SubjectCode == subjectCode && x.ContentLanguage == contentLanguage).FirstOrDefault();
            return result;
        }

        public ActivatedSubject.Subject GetInUseSubjectBySubjectId(string subjectId)
        {
            var coltn = helper.GetCollection<ActivatedSubject.Subject>(ActivatedSubject_Subject);
            var result = coltn.Find(x => x._id == subjectId).FirstOrDefault();
            return result;
        }

        public List<ActivatedSubject.ExamSuiteWithQuestion> MapQuestion(List<ActivatedSubject.ExamSuite> examSuiteList)
        {
            var coltn = helper.GetCollection<ActivatedSubject.Question>(ActivatedSubject_Question);
            List<ActivatedSubject.ExamSuiteWithQuestion> examQuestion = new List<ActivatedSubject.ExamSuiteWithQuestion>();
            foreach (var item in examSuiteList)
            {
                ActivatedSubject.ExamSuiteWithQuestion examQ = new ActivatedSubject.ExamSuiteWithQuestion
                {
                    _id = item._id,
                    TitleName = item.TitleName,
                    TitleCode = item.TitleCode,
                    CreateDateTime = item.CreateDateTime
                };

                List<ActivatedSubject.QuestionPool> qlist = new List<ActivatedSubject.QuestionPool>();
                //foreach (var qid in item.QuestionIds)
                //{
                //    var coltn = helper.GetCollection<ActivatedSubject.Question>(ActivatedSubject_Question);
                //    var qResult = coltn.Find(x => x._id == qid).FirstOrDefault();
                //    if (qResult != null)
                //    {
                //        //var qResult = this.helper.GetQuestion(result.ToList(), item.TitleCode).FirstOrDefault();

                //        //mapping 
                //        ActivatedSubject.QuestionPool qpool = new ActivatedSubject.QuestionPool
                //        {
                //            QuestionNumber = qResult.QuestionNumber,
                //            isAllowRandomChoice = qResult.isAllowRandomChoice,
                //            Detail = qResult.Detail,
                //            Choices = qResult.Choices,
                //            GroupId = qResult.GroupId,
                //        };

                //        qlist.Add(qpool);
                //    }
                //}

                var qResult = coltn.Find(x => item.QuestionIds.Contains(x._id)).ToList().Select(x=> new ActivatedSubject.QuestionPool
                {
                    _id = x._id,
                    Assets = x.Assets,
                    CreateDateTime = x.CreateDateTime,
                    QuestionNumber = x.QuestionNumber,
                    IsAllowRandomChoice = !x.IsAllowRandomChoice, //HACK : meaning of word is wrong
                    Detail = x.Detail,
                    Choices = x.Choices,
                    GroupId = x.GroupId,
                    //ExamCode = item.TitleCode,
                    ExamCode = item._id,
                });
                examQ.Questions = qResult.ToList();

                //map qGroup
                //var grouped = examQ.Questions.GroupBy(x => x.GroupId);
                foreach (var group in examQ.Questions.GroupBy(x => x.GroupId))
                {
                    if (group.Key == null)
                    {
                        foreach (var q in group)
                        {
                            q.GroupCount = 1;
                        }
                    }
                    else
                    {
                        int count = group.Count();
                        foreach (var q in group)
                        {
                            q.GroupCount = count;
                        }
                    }
                }
                examQuestion.Add(examQ);
            }


            return examQuestion;
        }

        public void CreatePreExamSheet(List<ActivatedSubject.ExamSheet> ExamSheets)
        {
            if (ExamSheets != null && ExamSheets.Count > 0)
            {
                var coltn = this.helper.GetCollection<ActivatedSubject.ExamSheet>(ShareData_ExamSheet);
                coltn.InsertMany(ExamSheets);
            }
        }
    }
}
