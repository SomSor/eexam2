using Ionic.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.Parsers;
using WebSite.Repositories;
using WebSite.ViewModels.ExamBankModels;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;


namespace WebSite.Controllers
{
    public class UploadController : Controller
    {
        private ICloudStorage iCloudStorage;
        //private CloudStorageAccount storageAccount;
        private IQuestionImportRepository repoQ;
        private IExamForApproveRepository repoExamForApprove;
        private IFile iFile;

        public UploadController(ICloudStorage icloud, IQuestionImportRepository repoQ, IExamForApproveRepository repoExamForApprove, IFile iFile)
        {
            //this.storageAccount = storageAccount;
            this.iCloudStorage = icloud;
            this.repoQ = repoQ;
            this.repoExamForApprove = repoExamForApprove;
            this.iFile = iFile;
        }

        // HttpPost: api/Markdown/Verify
        [HttpPost]
        [Route("api/Markdown/Verify")]
        public IActionResult VerifyMarkdown(IEnumerable<IFormFile> files, string Language)
        {
            var mdFi = files.FirstOrDefault(f => System.IO.Path.GetExtension(ContentDispositionHeaderValue.Parse(f.ContentDisposition).FileName.Trim('"')) == ".md");
            string md = string.Empty;

            if (mdFi != null)
            {
                using (var fi = new System.IO.StreamReader(mdFi.OpenReadStream()))
                {
                    md = fi.ReadToEnd();
                }
            }

            List<string> fileList = new List<string>();
            var subjectId = string.Empty;

            foreach (var f in files)
            {
                using (var fi = f.OpenReadStream())
                {
                    if (ZipFile.IsZipFile(fi, true))
                    {
                        fi.Seek(0, System.IO.SeekOrigin.Begin);
                        var assets = TheS.ExamBank.Parsers.AssetFileUtil.ReadZipEntries(fi).ToArray();
                        var parser = new TheS.ExamBank.Parsers.MarkdownParser();
                        if (parser.CheckAssets(md, assets))
                        {
                            fileList = assets.ToList();
                            assets = parser.GetAssetLinks(md).ToArray();

                            fi.Seek(0, System.IO.SeekOrigin.Begin);
                            var assetFiles = TheS.ExamBank.Parsers.AssetFileUtil.GetAssets(fi, assets);
                            try
                            {
                                var questions = parser.ParseQuestionsFromString(md);
                                var header = parser.ParseHeaderFromString(md);
                                subjectId = string.Concat(header.SubjectCode + "L" + header.Level, Language);

                                var qcode = (header.Code ?? string.Empty).Trim();
                                var qid = string.Concat(qcode, Language);
                                var qsuite = new TheS.ExamBank.DataFormats.QuestionSuite()
                                {
                                    _id = qid,
                                    Code = qcode,
                                    Title = header.Title,
                                    SubjectName = header.SubjectName,
                                    Description = header.Description,
                                    SubjectId = subjectId,
                                    SubjectCode = header.SubjectCode,
                                    Level = header.Level,
                                    LayoutCode = header.LayoutCode,

                                    Questions = questions,
                                };

                                var inActiveSubject = repoQ.GetInActiveSubject(subjectId);
                                var suite = repoQ.GetQuestionSuite(qid);

                                //no suite and no subject  : create new suite and subject
                                if (suite == null && inActiveSubject == null)
                                {
                                    if (inActiveSubject == null)
                                    {
                                        var newSubject = new InActive.InactiveSubject
                                        {
                                            _id = subjectId,
                                            SubjectCode = qsuite.SubjectCode,
                                            SubjectName = qsuite.SubjectName,
                                            CreateDateTime = DateTime.Now,
                                            IsEReadiness = false, //Hack
                                            ContentLanguage = Language,
                                            SiteId = HomeController._centerdata.SiteId,
                                            QuestionCount = questions.Count(),
                                            ExamSuiteCount = 1,
                                            ExamSuiteAcceptCount = 0,
                                            ExamSuiteRejectCount = 0,
                                            ExamSuiteGroups = new List<InActive.ExamSuiteGroup>()
                                            {
                                                new InActive.ExamSuiteGroup {
                                                    _id = "ก",
                                                    ExamSuiteGroupName = "ก", // HACK
                                                    IsUsed = true,
                                                    PassScore = null,
                                                    ExamDuration = null ,
                                                    ExamSuiteGroupMaps = new List<InActive.ExamSuiteGroupMap>
                                                    {
                                                        new InActive.ExamSuiteGroupMap
                                                        {
                                                            _id = Guid.NewGuid().ToString(),
                                                            ExamSuiteId = qid ,
                                                            RandomCount = 0,
                                                        }
                                                    },

                                                }
                                            },
                                        };
                                        repoQ.InsertInActiveSubject(newSubject);
                                    }
                                }
                                //has old suite and already has subject  : upsert suite   add groupmap to new suite
                                //ใช้ group map เดิม
                                else if (inActiveSubject != null && suite != null)
                                {
                                    //suite upsert ปกติ  subject ไม่เปลี่ยนอะไร
                                }
                                //new suite and already has subject : add groupmap to new suite
                                //เพิ่ม group map ใหม่
                                else if (inActiveSubject != null && suite == null)
                                {
                                    foreach (var item in inActiveSubject.ExamSuiteGroups)
                                    {
                                        item.ExamSuiteGroupMaps.Add(new InActive.ExamSuiteGroupMap
                                        {
                                            _id = repoExamForApprove.GetNewGuid("").ToString(),
                                            ExamSuiteId = qid,
                                            RandomCount = 0,
                                        });
                                    }

                                    repoQ.UpsertInActiveSubject(inActiveSubject);
                                }

                                if (inActiveSubject != null)
                                {
                                    //update SubjectName and ContentLanguage
                                    repoQ.UpdateSubjectNameAndContentLanguage(inActiveSubject._id, qsuite.SubjectName, Language);
                                    //UpdateQuestionCount UpdateExamSuiteCount
                                    var examSuites = repoQ.GetAllQuestionSuiteBySubjectId(subjectId);
                                    var questionCount = examSuites.Sum(es => es.Questions.Count());
                                    repoQ.UpdateQuestionCount(subjectId,
                                        questionCount + questions.Count(),
                                        examSuites.Count() + 1);
                                }

                                repoQ.Upsert(qsuite);
                                repoQ.UploadAssets(assetFiles, qid, this.iCloudStorage);
                            }
                            finally
                            {
                                TheS.ExamBank.Parsers.AssetFileUtil.CleanUpAssets(assetFiles, iFile);
                            }
                        }
                    }
                }
            }

            ViewData["files"] = fileList;

            return Redirect("~/#!/inactivesubjectview/" + subjectId);
        }

        //private void UploadAssets(IEnumerable<TheS.ExamBank.Parsers.AssetFileUtil.Asset> assetFiles, string qid, )
        //{
        //    // Create the blob client.
        //    //CloudBlobClient blobClient = this.storageAccount.CreateCloudBlobClient();
        //    CloudBlobClient blobClient = this.iCloudStorage.CreateCloudBlobClient(this.storageAccount);

        //    // Retrieve reference to a previously created container.
        //    CloudBlobContainer container = blobClient.GetContainerReference("examtest");
        //    foreach (var asst in assetFiles)
        //    {
        //        var blobName = WebUrlUtil.Combine(qid, asst.AssetName);
        //        // Retrieve reference to a blob.
        //        CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
        //        blockBlob.UploadFromFile(asst.Path, System.IO.FileMode.Open);
        //    }
        //}

        // HttpPost: api/Markdown/Save
        //[HttpPost]
        //[Route("api/Markdown/Save")]
        //public void SaveMarkdown(List<UploadExamSuite> uploadExamSuite)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
