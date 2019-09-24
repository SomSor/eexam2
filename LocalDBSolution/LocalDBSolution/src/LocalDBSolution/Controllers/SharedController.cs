using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using LocalDBSolution.ViewModels;
using LiteDB;
using LocalDBSolution.Repositories;
using LocalDBSolution.Repositories.Implementation;
using Microsoft.AspNetCore.Cors;
using System.Text;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LocalDBSolution.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    public class SharedController : Controller
    {
        //private string blobStorage;
        //private string blobLogoStorage;
        //private string logoextension;
        //private string root;
        //private string config;
        //private string storageCon;
        private ISharedRepository shareRepo;
        private IRepoForOnSite onsiteRepo;

        private localconfig config;

        public SharedController(ISharedRepository shareRepo, IRepoForOnSite onsiteRepo,
            IOptions<localconfig> config)
        {
            this.shareRepo = shareRepo;
            this.onsiteRepo = onsiteRepo;

            //blobStorage = System.Configuration.ConfigurationSettings.AppSettings.Get("blobStorage");
            //blobLogoStorage = System.Configuration.ConfigurationSettings.AppSettings.Get("blobLogoStorage");
            //logoextension = System.Configuration.ConfigurationSettings.AppSettings.Get("logoextension");
            //root = System.Configuration.ConfigurationSettings.AppSettings.Get("root");
            //config = System.Configuration.ConfigurationSettings.AppSettings.Get("config");
            //storageCon = System.Configuration.ConfigurationSettings.AppSettings.Get("StorageConnectionString");

            this.config = config.Value;
        }

        // GET: api/values
        [HttpGet]
        public localconfig Get()
        {
            return this.config;
        }

        [HttpPost]
        [Route("LoginToLocalDB/")]
        public void LoginToLocalDB([FromBody]Center center)
        {
            var logoPath = this.config.logoname + config.logoextension;
            var logoblob = center._id + config.logoextension;
            //var logoPath = Path.Combine(question.ExamCode, asset.Resource);
            //asset = "TSS04318002L1-2/imgs/q007img1.png";

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.StorageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(config.blobStorage);

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(config.blobLogoStorage + "/" + logoblob);

            // Save blob contents to a file.
            try
            {
                //var localPath = @"C:\examtest\" + asset;
                var localPath = Path.Combine(config.root, config.config, logoPath);
                var di = Path.GetDirectoryName(localPath);
                var imgs = new DirectoryInfo(di);

                if (!imgs.Exists)
                {
                    Directory.CreateDirectory(imgs.FullName);
                }

                if (blockBlob.Exists())
                {
                    using (var fileStream = System.IO.File.OpenWrite(localPath))
                    {
                        blockBlob.DownloadToStream(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                var d = ex;
            }

            //substring only real username
            center.LatestUser = center.LatestUser.Split(new char[] { '.', '@' })[1];

            //Save center data
            this.shareRepo.CreateCenter(center);
        }

        [HttpPut]
        [Route("DownloadAssets/")]
        public void DownloadAssets([FromBody] List<Question> questions)
        {
            //List<string> assets = new List<string>();
            foreach (var question in questions)
            {
                foreach (var asset in question.Assets)
                {
                    //var assetPath = Path.Combine(question.ExamCode, asset.Resource);
                    var assetPath = new StringBuilder().Append(question.ExamCode).Append("/").Append(asset.Resource).ToString();
                    //asset = "TSS04318002L1-2/imgs/q007img1.png";

                    // Retrieve storage account from connection string.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.StorageConnectionString);

                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Retrieve reference to a previously created container.
                    CloudBlobContainer container = blobClient.GetContainerReference(config.blobStorage);

                    // Retrieve reference to a blob named "photo1.jpg".
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(assetPath);

                    // Save blob contents to a file.
                    try
                    {
                        //var localPath = @"C:\examtest\" + asset;
                        var localPath = Path.Combine(config.root, config.blobStorage, assetPath);
                        var di = Path.GetDirectoryName(localPath);
                        var imgs = new DirectoryInfo(di);

                        if (!imgs.Exists)
                        {
                            Directory.CreateDirectory(imgs.FullName);
                        }

                        if (blockBlob.Exists())
                        {
                            using (var fileStream = System.IO.File.OpenWrite(localPath))
                            {
                                blockBlob.DownloadToStream(fileStream);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var d = ex;
                    }
                }
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value" + id;
        }

        // GET api/values/5
        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("GetLocalExamInfo/{centerid}/")]
        public TestBankVMResponse GetLocalExamInfo(string centerid)
        {
            var sheet = shareRepo.GetAvailableExamSheet(centerid);
            var testRegis = onsiteRepo.ListTestRegis(centerid);

            var group = from item in sheet
                        group item by new
                        {
                            item.Subject.SubjectCode,
                            item.Subject.SubjectName,
                            item.Subject.ContentLanguage,
                            item.Subject.Version,

                            //iten.Subject.Voices, TODO : Groupby Voice
                        } into gitem
                        let qty = gitem.Count()
                        let book = testRegis.Where(x => x.SubjectCode == gitem.Key.SubjectCode && x.ExamLanguage == gitem.Key.ContentLanguage).Count()
                        select new ExamSheetOnSiteRespone
                        {
                            SubjectCode = gitem.Key.SubjectCode,
                            SubjectName = gitem.Key.SubjectName,
                            ExamLanguage = gitem.Key.ContentLanguage,
                            Version = gitem.Key.Version,
                            Quantity = qty,
                            Book = book,
                            VoiceLanguage = "th", //TODO : Groupby Voice
                            IsExamEnough = qty >= book
                        };

            var result = new TestBankVMResponse
            {
                ExamSheets = group.ToList(),
            };
            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("CheckSubjectVersion/{centerid}/{subjectcode}/{version}/")]
        public MessageRespone CheckSubjectVersion(string centerid, string subjectcode, string version)
        {
            //If current sheets not match by request version then clear all local sheets
            var difSheets = shareRepo.GetDifferentAvailableSheet(centerid, subjectcode, version);

            int extraSheetCount = difSheets.Count();
            if (difSheets.Count > 0)
            {
                //then delete diff version sheet
                shareRepo.CloseDifSheets(difSheets);

                //and return deleted sheets for download new ver for replace
                return new MessageRespone { Message = "Founded different sheets : " + extraSheetCount, Code = extraSheetCount.ToString() };
            }
            else
            {
                return new MessageRespone { Message = "no extra" + extraSheetCount, Code = 0.ToString() };
            }
        }

        // GET api/values/5
        //[HttpGet]
        //[Route("GetLocalExamInfo/{subjectcode}/{contentlanguage}/{voicelanguage}")]
        //public TestBankVMResponse GetLocalExamInfoSpecific(string subjectcode, string contentlanguage, string voicelanguage, string version)
        //{

        //}

        // POST api/values
        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("CreateExamSheet/")]
        //public void CreateExamSheet([FromBody] List<ExamSheet> sheets)
        public void CreateExamSheet([FromBody]TestBankVMResponse data)
        {
            ////Create sheets to local db
            var jsonPreSheet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExamSheet>>(data.json);

            //var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonPreSheet);

            //Create sheets to local db
            this.shareRepo.CreateExamSheet(jsonPreSheet);

            var dupQs = jsonPreSheet.SelectMany(x => x.RandomQuestions).GroupBy(x => x._id).Select(xx => xx.FirstOrDefault());
            //Call local to download assets
            var jsonQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(dupQs);
            var qb = System.Text.Encoding.UTF8.GetBytes(jsonQuestion);

            this.DownloadAssets(dupQs.ToList());
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("CreateTestRegistration/")]
        //public void CreateTestRegistration([FromBody] List<TestRegistration> testregists)
        public void CreateTestRegistration([FromBody] SyncTestRegis testregist)
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestRegistration>>(testregist.json);
            this.shareRepo.CreateTestRegistration(data);
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("SaveCenterData/")]
        //public void CreateTestRegistration([FromBody] List<TestRegistration> testregists)
        public void SaveCenterData([FromBody] Center center)
        {
            this.shareRepo.CreateCenter(center);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
