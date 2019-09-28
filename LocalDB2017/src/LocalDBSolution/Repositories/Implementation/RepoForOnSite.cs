using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalDBSolution.ViewModels;
using LiteDB;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LocalDBSolution.Repositories.Implementation
{
    public class RepoForOnSite : IRepoForOnSite
    {
        private string localdb;

        public RepoForOnSite(localconfig localconfig)
        {
            localdb = localconfig.localdb;
        }

        public void Active(DateTime ActiceDateTime, string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                ActiveData newActive = new ActiveData
                {
                    _id = Guid.NewGuid().ToString(),
                    ActiveFromDateTime = DateTime.Now,
                    ActiveThruDateTime = ActiceDateTime,
                };
                var col = db.GetCollection<ActiveData>("activedata");

                col.Insert(newActive);
            }
        }

        public bool CheckExamEnough(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var colTestRegistration = db.GetCollection<TestRegistration>("testregistration");
                colTestRegistration.EnsureIndex(x => x.CenterId);
                var testRegis = colTestRegistration.Find(x => x.CenterId == centerid && x.IsCloseExam == false);
                var gTestRegis = testRegis.GroupBy(x => x.SubjectCode);


                var colExamSheet = db.GetCollection<ExamSheet>("examsheet");
                colExamSheet.EnsureIndex(x => x.CenterId);

                var examSheet = colExamSheet.Find(x => x.CenterId == centerid && x.IsCloseExam == false);

                var gExamSheet = examSheet.GroupBy(x => x.Subject.SubjectCode);

                bool IsEnough = true;

                foreach (var item in gTestRegis)
                {
                    var sub = gExamSheet.Where(x => x.Key == item.Key);
                    if (sub.Count() < item.Count())
                    {
                        IsEnough = false;
                        return false;
                    }
                }

                return IsEnough;
            }
        }

        public void CloseSheet(List<ExamSheet> sheetList)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");
                foreach (var item in sheetList)
                {
                    col.Update(item);
                }
            }
        }

        public void CloseTestRegis(List<TestRegistration> testRegisList)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");
                foreach (var item in testRegisList)
                {
                    col.Update(item);
                }
            }
        }

        public ActiveData GetActive(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ActiveData>("activedata");

                col.EnsureIndex(x => x._id);
                var data = col.FindAll().OrderByDescending(x=>x.ActiveThruDateTime).FirstOrDefault();
                var result = data ?? new ActiveData();
                return result;
            }
        }

        public Center GetCenter()
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<Center>("center");

                col.EnsureIndex(x => x._id);
                var data = col.FindAll();
                var result = data.FirstOrDefault() ?? null;
                return result;
            }
        }

        public ClientMapResponse GetClientId(string uuid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ClientMap>("ClientMap");
                col.EnsureIndex(x => x._id);

                var data = col.FindAll().OrderByDescending(x => x.UUID == uuid).FirstOrDefault();

                ClientMapResponse response = new ClientMapResponse();
                if (data != null)
                {
                    response._id = data._id;
                    response.UUID = data.UUID;
                    response.ClientId = data.ClientId;
                    response.Message = new MessageRespone { Code = "0", Message = "success" };
                }
                else
                {
                    response.Message = new MessageRespone { Code = "1", Message = string.Format("not found uuid : {0} in system.", uuid) };
                }
                return response;
            }
        }

        public IEnumerable<TestRegistration> GetExamRegisInfo(string pid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.PID);
                var data = col.Find(x => x.PID == pid && x.Status != "PASS" && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<TestRegistration>();
                return result;
            }
        }

        public ActiveData GetLastActive()
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ActiveData>("activedata");

                col.EnsureIndex(x => x._id);
                var data = col.FindAll().OrderByDescending(x => x.ActiveThruDateTime).FirstOrDefault();
                var result = data ?? new ActiveData();
                return result;
            }
        }

        public IEnumerable<ExamSheet> GetMappedSheet(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerid && x.TestRegisID != string.Empty && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<ExamSheet>();
                return result;
            }
        }

        public IEnumerable<ExamSheet> GetResultInfo(string pid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                //col.EnsureIndex(x => x.TestRegistration.PID);
                var data = col.Find(x => x.PID == pid && (x.LatestStatus == "PASS" || x.LatestStatus == "FAIL") && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<ExamSheet>();
                return result;
            }
        }

        public ExamSheet GetSheetByPIDAndSubjectCode(string pid, string subjectCode, string contentlanguage)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                //col.EnsureIndex(x => x.TestRegistration.PID);
                var data = col.Find(x => x.PID == pid && x.Subject.SubjectCode == subjectCode && x.IsCloseExam == false && x.Subject.ContentLanguage == contentlanguage);
                var result = data.FirstOrDefault() ?? new ExamSheet();
                return result;
            }
        }

        public ExamSheet GetSheetBySheetId(string sheetid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x._id);
                var data = col.Find(x => x._id == sheetid && x.IsCloseExam == false);
                var result = data.FirstOrDefault() ?? new ExamSheet();
                return result;
            }
        }

        public ExamSheet GetSheetBySubjectCode(string subjectCode)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                //col.EnsureIndex(x => x.Subject.SubjectCode);
                var data = col.Find(x => x.Subject.SubjectCode == subjectCode && x.IsCloseExam == false);
                var result = data.FirstOrDefault() ?? new ExamSheet();
                return result;
            }
        }

        public ExamSheet GetSheetBySubjectCodeForMap(string subjectCode, string contentlanguge)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                //col.EnsureIndex(x => x.Subject.SubjectCode);
                var data = col.Find(x => x.Subject.SubjectCode == subjectCode && x.IsCloseExam == false && x.TestRegisID == null && x.Subject.ContentLanguage == contentlanguge);
                var result = data.FirstOrDefault() ?? null;
                return result;
            }
        }

        public TestRegistration GetTestRegisById(string id)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x._id);
                var data = col.Find(x => x._id == id);
                var result = data.FirstOrDefault() ?? new TestRegistration();
                return result;
            }
        }

        public IEnumerable<TestRegistration> GetTestRegisByIds(List<string> ids)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                //var data = col.Find(x => ids.Contains(x._id));
                var data = col.FindAll().Where((x => ids.Contains(x._id)));
                

                var result = data.ToList() ?? new List<TestRegistration>();
                return result;

            }
        }

        public int GetTestRegisCount(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerid && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<TestRegistration>();
                return result.Count();
            }
        }

        public TestRegistration GetTestRegisInfo(string pid, string subjectCode)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.PID);
                var data = col.Find(x => x.PID == pid && x.SubjectCode == subjectCode && x.IsCloseExam == false);
                var result = data.FirstOrDefault() ?? new TestRegistration();
                return result;
            }
        }

        public TestRegistration GetTestRegisInfoForPrintQR(string pid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.PID);
                var data = col.Find(x => x.PID == pid && x.IsCloseExam == false);
                var result = data.FirstOrDefault() ?? new TestRegistration();
                return result;
            }
        }

        public ExamSheet GetTestResultInfo(string pid, string sheetId)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x._id);
                var data = col.Find(x => x.PID == pid && x._id == sheetId && (x.LatestStatus == "PASS" || x.LatestStatus == "FAIL") && x.IsCloseExam == false);
                var result = data.FirstOrDefault() ?? new ExamSheet();
                return result;
            }
        }

        public IEnumerable<ExamSheet> ListExamData(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerid && x.IsCloseExam == false );
                var result = data.ToList() ?? new List<ExamSheet>();
                return result;
            }
        }

        public IEnumerable<ExamSheet> ListTestingSheet(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerid && x.LatestStatus == "TESTING" && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<ExamSheet>();
                return result;
            }
        }

        public IEnumerable<TestRegistration> ListTestRegis(string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerid && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<TestRegistration>();
                return result;
            }
        }

        public MessageRespone Login(string user, string pwd, string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<Center>("center");

                col.EnsureIndex(x => x._id);
                var data = col.Find(x => x._id == centerid).FirstOrDefault();

                MessageRespone result = new MessageRespone() { };

                if (data.LatestUser == user && data.LatestPass == pwd)
                {
                    result.Code = "SUCCESS";
                    result.Message = "SUCCESS";
                }
                else
                {
                    result.Code = "NOT MATCH";
                    result.Message = "NOT MATCH";
                }

                return result;
            }
        }

        public IEnumerable<TestRegistration> SearchTestRegis(string txt, string centerid)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerid && (x.PID == txt || x.FirstName.Contains(txt) || x.LastName.Contains(txt)) && x.IsCloseExam == false);
                var result = data.ToList() ?? new List<TestRegistration>();
                return result;
            }
        }

        public void UpdateSheet(ExamSheet sheet)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");
                col.Update(sheet._id, sheet);
            }
        }

        public void UpdateTestRegis(TestRegistration testRegis)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.Update(testRegis._id, testRegis);
            }
        }
    }
}
