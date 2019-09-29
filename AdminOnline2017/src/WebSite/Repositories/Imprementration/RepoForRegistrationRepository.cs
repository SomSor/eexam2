using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Repositories.MongoImpl;
using WebSite.ViewModels.AdminOnlineModelsBack;
using MongoDB.Driver;

namespace WebSite.Repositories.Imprementration
{
    public class RepoForRegistrationRepository : IRepoForRegistrationRepository
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

        public RepoForRegistrationRepository(MongoHelper helper)
        {
            this.helper = helper;
        }

        public ViewModels.ExamBankModelsBack.ActivatedSubject.Subject GetActivatedSubjectByCode(string code)
        {
            var coltn = helper.GetCollection<ViewModels.ExamBankModelsBack.ActivatedSubject.Subject>(ActivatedSubject_Subject);
            var result = coltn.Find(x => x.SubjectCode == code).FirstOrDefault();
            return result;
        }

        public IEnumerable<TestRegistration> ListForAproved(string centerId)
        {
            var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
            var result = coltn.Find(x => x.CenterId == centerId && (x.Status == "APPROVED" || x.Status == "MISS" || x.Status == "FAIL")).ToList();
            return result ?? new List<TestRegistration>();
        }

        //public IEnumerable<TestRegistration> ListResultTestRegis(string centerId)
        //{
        //    var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
        //    var result = coltn.Find(x => x.CenterId == centerId && (x.Status == "PASS" || x.Status == "FAIL")).ToList();
        //    return result ?? new List<TestRegistration>();
        //}

        public IEnumerable<TestRegistration> ListTestRegisByDate(string centerId, DateTime date)
        {
            var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
            //var result = coltn.Find(x => x.CenterId == centerId)?.ToList().Where(x => x.AppointDate.Date == date.Date);
            var result = coltn.Find(x => x.CenterId == centerId && x.AppointDate >= date.Date && x.AppointDate < date.Date.AddDays(1))?.ToList();
            //var result = coltn.Find(x => true).ToList();
            return result ?? new List<TestRegistration>();
        }

        public IEnumerable<TestRegistration> ListTestRegisByID(List<string> id)
        {
            var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
            var result = coltn.Find(x => id.Contains(x._id)).ToList();
            return result ?? new List<TestRegistration>();
        }

        public IEnumerable<TestRegistration> SearchTestRegis(string centerId, string txt)
        {
            var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
            var result = coltn.Find(x => x.CenterId == centerId && (x.FirstName.Contains(txt) || x.LastName.Contains(txt) || x.PID == txt)).ToList();
            return result ?? new List<TestRegistration>();
        }

        public void UpdateAppointStatus(IEnumerable<TestRegistration> appointTestRegis)
        {
            if (appointTestRegis != null)
            {
                foreach (var item in appointTestRegis)
                {
                    var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
                    var opt = new UpdateOptions()
                    {
                        //IsUpsert = true,
                    };
                    //coltn.ReplaceOne(it => it._id == item._id, item, opt);
                    coltn.ReplaceOne(it => it._id == item._id, item);
                }
            }
            else
            {
                throw new ArgumentNullException("null input from appointTestRegis ");
            }
        }

        public List<DateTime> ListAppointDate(string centerId)
        {
            var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);
            var result = coltn.Find(x => x.CenterId == centerId && x.AppointDate.HasValue).ToList().GroupBy(x => x.AppointDate.Value.Date).Select(x => x.Key).ToList();

            return result ?? new List<DateTime>();
        }

        public void UpdateLatestStatus(IEnumerable<TestRegistration> testRegis)
        {
            if (testRegis != null && testRegis.Count() > 0)
            {
                var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);

                foreach (var item in testRegis)
                {
                    coltn.ReplaceOne(it => it._id == item._id, item);
                }
            }
            else
            {
                throw new ArgumentNullException("null input from testRegis");
            }
        }

        public Center GetCenterData(string centerId)
        {
            var coltn = helper.GetCollection<Center>(ShareData_Center);
            var result = coltn.Find(x => x._id == centerId);
            return result?.FirstOrDefault();
        }

        public void CreateTestRegis(IEnumerable<TestRegistration> testRegis)
        {
            if (testRegis.Count() > 0)
            {
                var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);

                coltn.InsertMany(testRegis);
            }
        }

        public Site GetSiteData(string siteId)
        {
            var coltn = helper.GetCollection<Site>(ShareData_Site);
            var result = coltn.Find(x => x._id == siteId);
            return result?.FirstOrDefault();
        }

        public IEnumerable<TestRegistration> ListTestRegisByPID(List<string> PID)
        {
            var coltn = helper.GetCollection<TestRegistration>(ShareData_TestRegistration);

            var result = coltn.Find(x => PID.Contains(x.PID)).ToList();

            return result ?? new List<TestRegistration>();
        }
    }
}
