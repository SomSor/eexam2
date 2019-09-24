using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;
using WebSite.ViewModels.ExamBankModelsBack;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;
using TheS.ExamBank.Parsers;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace WebSite.Repositories.MongoImpl
{
    public class QuestionImportRepository : IQuestionImportRepository
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


        public QuestionImportRepository(MongoHelper helper)
        {
            this.helper = helper;
        }

        public IEnumerable<QuestionSuite> GetAll()
        {
            //var coltn = helper.GetCollection<QuestionSuite>("exambank");
            var coltn = helper.GetCollection<QuestionSuite>(InactiveSubject_ExamSuite);
            var suites = coltn.Find(Builders<QuestionSuite>.Filter.Empty)
                .Project(it => new { it.Code, it.SubjectCode, it.SubjectName, it.Description }).ToList();
            return suites.Select(it => new QuestionSuite
            {
                Code = it.Code,
                SubjectCode = it.SubjectCode,
                SubjectName = it.SubjectName,
                Description = it.Description,
            }).ToList();
        }

        public QuestionSuiteVM GetQuestionSuite(string id)
        {
            //var coltn = helper.GetCollection<QuestionSuiteVM>("exambank");
            var coltn = helper.GetCollection<QuestionSuiteVM>(InactiveSubject_ExamSuite);
            return coltn.Find(Builders<QuestionSuiteVM>.Filter.Eq(it => it._id, id)).FirstOrDefault();
        }

        public void InsertQuestionSuite(QuestionSuite qsuite)
        {
            throw new NotImplementedException();
        }

        public void Upsert(QuestionSuite qsuite)
        {
            //var coltn = helper.GetCollection<QuestionSuite>("exambank");
            var coltn = helper.GetCollection<QuestionSuite>(InactiveSubject_ExamSuite);
            var opt = new UpdateOptions()
            {
                IsUpsert = true,
            };
            coltn.ReplaceOne(it => it._id == qsuite._id, qsuite, opt);
        }

        public InActive.InactiveSubject GetInActiveSubject(string inactiveSubjectId)
        {
            var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
            //return coltn.Find(Builders<InActive.InactiveSubject>.Filter.Eq(it => it.SubjectCode, inactiveSubjectCode)).FirstOrDefault();
            return coltn.Find(x => x._id == inactiveSubjectId).FirstOrDefault();
        }

        //public void CreateInActiveSubject(InActive.InactiveSubject InactiveSubject)
        //{
        //    if (InactiveSubject != null)
        //    {
        //        var coltn = helper.GetCollection<InActive.InactiveSubject>("InactiveSubject.InactiveSubject");
        //        coltn.InsertOne(InactiveSubject);
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException("null input from InactiveSubject repo");
        //    }
        //}

        public IEnumerable<QuestionSuiteVM> GetAllQuestionSuiteBySubjectId(string SubjectId)
        {
            var coltn = helper.GetCollection<QuestionSuiteVM>(InactiveSubject_ExamSuite);
            return coltn.Find(Builders<QuestionSuiteVM>.Filter.Eq(it => it.SubjectId, SubjectId)).ToList();
        }

        public void DeleteInActiveSubject(string SubjectId)
        {
            var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
            var filter = Builders<InActive.InactiveSubject>.Filter.Eq("_id", SubjectId);
            var result = coltn.DeleteOne(filter);
        }

        public void DeleteExamSuite(string ExamSuiteId)
        {
            var coltn = helper.GetCollection<QuestionSuite>(InactiveSubject_ExamSuite);
            var filter = Builders<QuestionSuite>.Filter.Eq("_id", ExamSuiteId);
            var result = coltn.DeleteOne(filter);
        }

        public void DeleteExamSuiteBySubjectId(string SubjectId)
        {
            var coltn = helper.GetCollection<QuestionSuite>(InactiveSubject_ExamSuite);
            var filter = Builders<QuestionSuite>.Filter.Eq("SubjectId", SubjectId);
            var result = coltn.DeleteOne(filter);
        }

        public void UpsertInActiveSubject(InActive.InactiveSubject InactiveSubject)
        {
            var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
            var opt = new UpdateOptions()
            {
                IsUpsert = true,
            };
            coltn.ReplaceOne(it => it._id == InactiveSubject._id, InactiveSubject, opt);
        }

        public void UploadAssets(IEnumerable<AssetFileUtil.Asset> assetFiles, string qid, ICloudStorage cloudStorage)
        {
            // Create the blob client.
            CloudBlobClient blobClient = cloudStorage.GetCloudStorageAcc().CreateCloudBlobClient();
            //CloudBlobClient blobClient = cloudStorage.CreateCloudBlobClient(storageAcc);

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("examtest");
            if (!container.Exists())
            {
                container.Create();
            }
            foreach (var asst in assetFiles)
            {
                var blobName = WebUrlUtil.Combine(qid, asst.AssetName);
                // Retrieve reference to a blob.
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                blockBlob.UploadFromFile(asst.Path);
            }
        }

        public void InsertInActiveSubject(InActive.InactiveSubject InactiveSubject)
        {
            var coltn = helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject);
            //var opt = new UpdateOptions()
            //{
            //    IsUpsert = true,
            //};
            //coltn.ReplaceOne(it => it._id == InactiveSubject._id, InactiveSubject, opt);
            coltn.InsertOne(InactiveSubject);
        }

        public void UpdateSubjectNameAndContentLanguage(string subjectId, string subjectName, string contentLanguage)
        {
            var update = Builders<InActive.InactiveSubject>.Update
                .Set(it => it.SubjectName, subjectName)
                .Set(it => it.ContentLanguage, contentLanguage);
            helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject)
                 .UpdateOneAsync(it => it._id == subjectId, update);
        }

        public void UpdateSubjectCodeAndNameAndContentLanguage(string subjectId, string subjectCode, string subjectName, string contentLanguage)
        {
            var update = Builders<InActive.InactiveSubject>.Update
                .Set(it => it.SubjectCode, subjectCode)
                .Set(it => it.SubjectName, subjectName)
                .Set(it => it.ContentLanguage, contentLanguage);
            helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject)
                 .UpdateOneAsync(it => it._id == subjectId, update);
        }

        public void UpdateQuestionCount(string subjectId, int QuestionCount, int ExamSuiteCount)
        {
            var update = Builders<InActive.InactiveSubject>.Update
                .Set(it => it.QuestionCount, QuestionCount)
                .Set(it => it.ExamSuiteCount, ExamSuiteCount);
            helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject)
                 .UpdateOneAsync(it => it._id == subjectId, update);
        }

        public void UpdateQuestionCountComment(string subjectId, int ExamSuiteAcceptCount, int ExamSuiteRejectCount)
        {
            var update = Builders<InActive.InactiveSubject>.Update
                .Set(it => it.ExamSuiteAcceptCount, ExamSuiteAcceptCount)
                .Set(it => it.ExamSuiteRejectCount, ExamSuiteRejectCount);
            helper.GetCollection<InActive.InactiveSubject>(InactiveSubject_InactiveSubject)
                 .UpdateOneAsync(it => it._id == subjectId, update);
        }

        public void UpdateFIX(QuestionSuiteVM examSuite)
        {
            var update = Builders<QuestionSuiteVM>.Update
                .Set(it => it.SubjectId, examSuite.SubjectId);
            helper.GetCollection<QuestionSuiteVM>(InactiveSubject_ExamSuite)
                 .UpdateOneAsync(it => it._id == examSuite._id, update);
        }

        public IEnumerable<QuestionSuiteVM> GetAllDamageQuestionSuite()
        {
            var coltn = helper.GetCollection<QuestionSuiteVM>(InactiveSubject_ExamSuite).Find(x => !x.SubjectId.Contains("L")).ToList();
            return coltn;
        }

        public void UpdateQuestionSuiteCodeAndName(string QuestionSuiteId, object Code, object Title)
        {
            var update = Builders<QuestionSuiteVM>.Update
                .Set(it => it.Code, Code)
                .Set(it => it.Title, Title);
            helper.GetCollection<QuestionSuiteVM>(InactiveSubject_ExamSuite)
                 .UpdateOneAsync(it => it._id == QuestionSuiteId, update);
        }

        public void InsertQuestion(MultipleChoiceQuestionWithOneCorrectAnswerWithId question)
        {
            throw new NotImplementedException();
        }

        public void InsertChoice(SelectableChoiceWithId choice)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(MultipleChoiceQuestionWithOneCorrectAnswerWithId question)
        {
            throw new NotImplementedException();
        }

        public void UpdateChoice(SelectableChoiceWithId choice)
        {
            throw new NotImplementedException();
        }
    }
}

