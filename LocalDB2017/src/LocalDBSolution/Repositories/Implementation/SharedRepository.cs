using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalDBSolution.ViewModels;
using LiteDB;

namespace LocalDBSolution.Repositories.Implementation
{
    public class SharedRepository : ISharedRepository
    {
        private string localdb;

        public SharedRepository()
        {
            localdb = System.Configuration.ConfigurationSettings.AppSettings.Get("localdb");
        }

        public void CloseDifSheets(List<ExamSheet> sheets)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x._id);

                foreach (var item in sheets)
                {
                    item.IsCloseExam = true;
                    col.Update(item._id, item);
                }
            }
        }

        public void CreateCenter(Center center)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<Center>("Center");

                //if (col.Exists(x=>x._id == center._id))
                //{
                //    col.Update(center);
                //}
                //else
                //{
                    db.DropCollection("Center");
                    col.Insert(center);
                //}
            }
        }

        public void CreateExamSheet(List<ExamSheet> examSheets)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.Insert(examSheets);
            }
        }

        public void CreateTestRegistration(List<TestRegistration> testregists)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.Insert(testregists);
            }
        }

        public List<ExamSheet> GetAllLocalExamSheetBySubjectCode(string centerId)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerId);
                var result = data ?? new List<ExamSheet>();
                return result.ToList();
            }
        }

        public List<ExamSheet> GetAvailableExamSheet(string centerId)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerId && x.TestRegisID == string.Empty);
                var result = data ?? new List<ExamSheet>();
                return result.ToList();
            }
        }

        public List<TestRegistration> GetAvailableTestRegis(string centerId)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerId && x.IsCloseExam == false);
                var result = data ?? new List<TestRegistration>();
                return result.ToList();
            }
        }

        public List<ExamSheet> GetDifferentAvailableSheet(string centerId, string subjectcode, string version)
        {
            var result =  GetAvailableExamSheet(centerId)?.Where(x => x.Subject.SubjectCode == subjectcode && x.Subject.Version != version).ToList();
            return result;
        }

        public List<ExamSheet> GetLocalExamSheetBySubjectCode(string subjectCode, string contentlanguage, string voicelanguage, string centerId, string version)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<ExamSheet>("examsheet");

                col.EnsureIndex(x => x.Subject.SubjectCode);
                col.EnsureIndex(x => x.CenterId);
                col.EnsureIndex(x => x.Subject.ContentLanguage);
                var data = col.Find(x => x.Subject.SubjectCode == subjectCode && x.CenterId == centerId && x.Subject.ContentLanguage == contentlanguage && x.Subject.Version != version);
                var result = data ?? new List<ExamSheet>();
                return result.ToList();
            }
        }

        public List<TestRegistration> GetLocalTestRegis(string centerId)
        {
            using (var db = new LiteDatabase(localdb))
            {
                var col = db.GetCollection<TestRegistration>("testregistration");

                col.EnsureIndex(x => x.CenterId);
                var data = col.Find(x => x.CenterId == centerId);
                var result = data ?? new List<TestRegistration>();
                return result.ToList();
            }
        }
    }
}
