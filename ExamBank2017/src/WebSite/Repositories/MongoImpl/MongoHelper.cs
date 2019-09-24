using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Repositories.MongoImpl
{
    public class MongoHelper
    {
        private MongoClient client;
        private CloudStorageAccount storage;
        private string blobName;
        private string databaseName;

        public MongoHelper(MongoClient client, CloudStorageAccount storage, IWebConfiguration config)
        {
            this.blobName = config.BlobName;
            this.databaseName = config.MongoDatabaseName;
            this.client = client;
            this.storage = storage;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var db = client.GetDatabase(databaseName);
            return db.GetCollection<T>(collectionName);
        }

        public IEnumerable<ViewModels.ExamBankModelsBack.ActivatedSubject.Question> GetQuestion(IEnumerable<ViewModels.ExamBankModelsBack.ActivatedSubject.Question> questions, string examCode, string Language)
        {
            CloudBlobClient blobClient = storage.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(blobName);
            foreach (var q in questions)
            {
                if (q.Assets == null || !q.Assets.Any())
                {
                    continue;
                }

                var assets = (from it in q.Assets
                              let blockBlob = container.GetBlockBlobReference(WebUrlUtil.Combine(examCode + Language, it.Resource))
                              select new TheS.ExamBank.DataFormats.Asset
                              {
                                  Resource = blockBlob.Uri.AbsoluteUri,
                                  ApplyTo = it.ApplyTo,
                                  Positions = it.Positions
                              }).SelectMany(ass => ass.Positions.Select(pos => new { Position = pos, ApplyTo = ass.ApplyTo, ass.Resource }))
                              .OrderByDescending(it => it.Position)
                              .ToArray();
                var qassets = assets.Where(x => x.ApplyTo == 0).ToArray();

                foreach (var ast in qassets)
                {
                    q.Detail = q.Detail.Insert(ast.Position, ast.Resource);
                }

                if (q.Choices != null)
                {
                    var choices = q.Choices.ToArray();

                    for (int i = 0; i < choices.Length; i++)
                    {
                        var casset = assets.Where(x => x.ApplyTo - 1 == i).ToArray();
                        string cnt = choices[i].Detail;
                        foreach (var ast in casset)
                        {
                            choices[i].Detail = choices[i].Detail.Insert(ast.Position, ast.Resource);
                        }
                    }
                }
            }
            return questions.OrderBy(x => x.QuestionNumber); ;
        }

        public IEnumerable<TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswer> GetQuestion(ViewModels.ExamBankModelsBack.QuestionSuiteVM s)
        {
            var newQs = new List<TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswer>(s.Questions.Count());

            CloudBlobClient blobClient = this.storage.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(blobName);
            foreach (var q in s.Questions)
            {
                var assets = (from it in q.Assets
                              let blockBlob = container.GetBlockBlobReference(WebUrlUtil.Combine(s._id, it.Resource))
                              select new TheS.ExamBank.DataFormats.Asset
                              {
                                  Resource = blockBlob.Uri.AbsoluteUri,
                                  ApplyTo = it.ApplyTo,
                                  Positions = it.Positions
                              });
                newQs.Add(TheS.ExamBank.DataFormats.Helpers.AssetUtil.ApplyAssets(q, assets) as TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswer);
            }
            return newQs;
        }
    }
}
