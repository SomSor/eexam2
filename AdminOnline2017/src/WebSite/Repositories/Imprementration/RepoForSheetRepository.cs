using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Repositories.MongoImpl;
using WebSite.ViewModels.AdminOnlineModelsBack;
using MongoDB.Driver;

namespace WebSite.Repositories.Imprementration
{
    public class RepoForSheetRepository : IRepoForSheetRepository
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

        public RepoForSheetRepository(MongoHelper helper)
        {
            this.helper = helper;
        }

        public void CreatePreExamSheet(List<ExamSheet> ExamSheets)
        {
            if (ExamSheets != null && ExamSheets.Count > 0)
            {
                var coltn = this.helper.GetCollection<ExamSheet>(ShareData_ExamSheet);
                coltn.InsertMany(ExamSheets);
            }
        }

        public void UpdateExamSheet(List<ExamSheet> ExamSheets)
        {
            if (ExamSheets.Count() > 0)
            {
                var coltn = helper.GetCollection<ExamSheet>(ShareData_ExamSheet);

                foreach (var item in ExamSheets)
                {
                    coltn.ReplaceOne(it => it._id == item._id, item);
                }
            }
            else
            {
                throw new ArgumentNullException("null input from ExamSheets");
            }
        }

        public IEnumerable<ExamSheet> ListExamSheetByID(List<string> id)
        {
            var coltn = helper.GetCollection<ExamSheet>(ShareData_ExamSheet);

            var result = coltn.Find(x => id.Contains(x._id)).ToList();

            return result ?? new List<ExamSheet>();
        }

        public List<OccupationGroup> ListOccupationGroup(string centerid)
        {
            var centerColtn = this.helper.GetCollection<Center>(ShareData_Center);
            var center = centerColtn.Find(x => x._id == centerid).FirstOrDefault();

            var coltn = helper.GetCollection<WebSite.ViewModels.AdminOnlineModelsBack.OccupationGroup>(ShareData_OccupationGroup);
            var result = coltn.Find(Builders<OccupationGroup>.Filter.Eq(it => it.SiteId, center.SiteId)).ToList();
            return result ?? new List<OccupationGroup>();
        }

        public List<ExamSheet> GetTestResultByDate(string centerid, DateTime testdate)
        {
            var date1 = testdate.Date;
            var date2 = date1.AddDays(1);
            var coltn = helper.GetCollection<ExamSheet>(ShareData_ExamSheet);
            var result = coltn.Find(x => x.CenterId == centerid
            && x.TestReg != null && x.ExamDateTime.HasValue
            && x.ExamDateTime >= date1
            && x.ExamDateTime < date2
            ).ToList();

            return result ?? new List<ExamSheet>();
        }

        public IEnumerable<ExamSheet> ListExamSheetForSendTo3rd(string centerid)
        {
            var coltn = helper.GetCollection<ExamSheet>(ShareData_ExamSheet);
            var result = coltn.Find(x => x.CenterId == centerid && (x.LatestStatus == "PASS" || x.LatestStatus == "FAIL") && x.TestReg.ExamStatus == "UNSEND").ToList();

            return result ?? new List<ExamSheet>();
        }

        public IEnumerable<ExamSheet> ListPassExamSheetForSendTo3rd(string centerid)
        {
            var coltn = helper.GetCollection<ExamSheet>(ShareData_ExamSheet);
            var result = coltn.Find(x => x.CenterId == centerid && x.LatestStatus == "PASS" && x.TestReg.ExamStatus == "UNSEND").ToList();

            return result ?? new List<ExamSheet>();
        }

        public IEnumerable<ExamSheet> ListFailExamSheetForSendTo3rd(string centerid)
        {
            var coltn = helper.GetCollection<ExamSheet>(ShareData_ExamSheet);
            var result = coltn.Find(x => x.CenterId == centerid && x.LatestStatus == "FAIL" && x.TestReg.ExamStatus == "UNSEND").ToList();

            return result ?? new List<ExamSheet>();
        }
    }
}
