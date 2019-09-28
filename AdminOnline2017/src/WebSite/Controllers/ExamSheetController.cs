using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebSite.Models;
using WebSite.Services;
using WebSite.ViewModels.Account;
using AdminOnlineModelsBack = WebSite.ViewModels.AdminOnlineModelsBack;
using AdminOnlineModels = WebSite.ViewModels.AdminOnlineModels;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using WebSite.Repositories;
using System.Collections.Specialized;
using System.Text;
using Microsoft.AspNetCore.Cors;
using System.Globalization;
using System.ServiceModel;
using SyncResultDataService;

namespace WebSite.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class ExamSheetController : Controller
    {
        private IRepoForSheetRepository repoSheet;
        private IRepoForRegistrationRepository repoTestRegis;
        private const int trycount = 5;

        public ExamSheetController(IRepoForSheetRepository repoSheet, IRepoForRegistrationRepository repoTestRegis)
        {
            this.repoSheet = repoSheet;
            this.repoTestRegis = repoTestRegis;
        }

        [HttpPost]
        [Route("SubmitExamSheet/{centerid}/{subjectcode}/{examlanguage}/{voicelanguage}/{quantity}")]
        public AdminOnlineModels.TestBankVM SubmitExamSheet(string centerid, string subjectcode, string examlanguage, string voicelanguage, int quantity)
        {
            //string URL = string.Format("http://localhost:7113/api/activated/RandomExamSheet/1/TSS04318002/th/1");

            //string URL = string.Format("http://exambankex.azurewebsites.net/api/activated/RandomExamSheet/{0}/{1}/{2}/{3}", centerid, subjectcode, examlanguage, quantity);
            string URL = string.Format("http://localhost:50273/api/activated/RandomExamSheet/{0}/{1}/{2}/{3}", centerid, subjectcode, examlanguage, quantity);
            //string URL = string.Format("http://localhost:7113/api/activated/RandomExamSheet/{0}/{1}/{2}/{3}", centerid, subjectcode, examlanguage, quantity);

            using (var client = new WebClient())
            {
                //bool tryagain = true;
                //int totaltry;
                //for (int i = 1; i <= trycount; i++)
                //{
                //    if (tryagain)
                //    {
                try
                {
                    client.Encoding = Encoding.UTF8;
                    var responseString = client.DownloadString(URL);
                    var preSheet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AdminOnlineModelsBack.ExamSheet>>(responseString);

                    //AdminOnline Job
                    this.repoSheet.CreatePreExamSheet(preSheet);

                    //return new AdminOnlineModels.TestBankVM()
                    //{
                    //    ExamSheets = preSheet.Select(x => new AdminOnlineModels.ExamSheet()
                    //    {
                    //        _id = x._id,
                    //    }).ToList(),
                    //};
                    return new ViewModels.AdminOnlineModels.TestBankVM { json = responseString };

                    ////Create sheets to local db
                    //var jsonPreSheet = Newtonsoft.Json.JsonConvert.SerializeObject(preSheet);

                    //var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonPreSheet);

                    //client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    ////dataByte = client.UploadData("http://localhost:59684/api/shared/createexamsheet", "POST", dataByte);
                    //dataByte = client.UploadData("http://10.93.77.199/localdb/api/shared/createexamsheet", "POST", dataByte);

                    //var dupQs = preSheet.SelectMany(x => x.RandomQuestions).GroupBy(x => x._id).Select(xx => xx.FirstOrDefault());
                    ////Call local to download assets
                    //var jsonQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(dupQs);
                    //var qb = System.Text.Encoding.UTF8.GetBytes(jsonQuestion);

                    //client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    ////qb = client.UploadData("http://localhost:59684/api/shared/downloadassets", "PUT", qb);
                    //qb = client.UploadData("http://10.93.77.199/localdb/api/shared/downloadassets", "PUT", qb);

                    //Confirm.IsEnabled = false;
                    //MessageBox.Show("Registered");


                    // repo = new Repositories.MongoImpl.SharedRepository();
                    //repo.CreateExamSheet(preSheet);

                    //finish job
                    //tryagain = false;
                    //totaltry = i;
                    //break;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    //throw e
                    return null;
                }
                //}
                //}
            }
        }

        [HttpPut]
        [Route("SendTo3ndParty")]
        public void SendTo3ndParty([FromBody] IEnumerable<AdminOnlineModels.TestRegistration> testRegis)
        {
            var sheetForSend = repoSheet.ListExamSheetByID(testRegis.Where(x => x.SheetId != string.Empty).Select(x => x.SheetId).ToList());

            if (testRegis.FirstOrDefault().SiteId == "01")
            {
                List<TestResultDTO> _testresult = new List<TestResultDTO>();

                foreach (var item in sheetForSend)
                {
                    DateTime regDate = item.TestReg.RegDate;
                    DateTime examDate = item.ExamDateTime.Value;
                    var APPLY_DATE = regDate.Year < 2500 ? regDate.AddYears(543).ToString("yyyyMMdd", CultureInfo.GetCultureInfo("en-us")) : regDate.Year > 3000 ? regDate.AddYears(-543).ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en-us")) : regDate.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("en-us"));
                    var EXAM_DATE = examDate.Year < 2500 ? examDate.AddYears(543).ToString("yyyyMMdd", CultureInfo.GetCultureInfo("en-us")) : examDate.Year > 3000 ? examDate.AddYears(-543).ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en-us")) : examDate.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("en-us"));

                    DateTime currDate = DateTime.Now;
                    string insertDate = currDate.Year < 2500 ? currDate.ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en-us")) : currDate.Year > 3000 ? currDate.AddYears(-543).ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en-us")) : currDate.ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en-us"));

                    var testResult = new TestResultDTO()
                    {
                        CERT_YY = Convert.ToInt32(item.TestReg.CertData.CertYear),
                        CERT_NO = item.TestReg.CertData.CertNo,
                        //REQ_DATE = APPLY_DATE,
                        REQ_DATE = item.TestReg.RegDateString,
                        REQ_NO = item.TestReg.PID,
                        SEQ_NO = item.TestCount.ToString(),
                        EXAM_DATE = EXAM_DATE,
                        PASS_NO = item.TestCount,
                        EXAM_RESULT = item.LatestStatus == "PASS",
                        PLT_CODE = item.Subject.SubjectCode,
                        Qxx = null,
                        EXAM_FLAG = item.TestCount - 1,
                        UPD_USER_CODE = item.TestReg.CertData.UserCode,
                        LAST_UPD_DATE = DateTime.Parse(insertDate),
                        CREATE_USER_CODE = item.TestReg.CertData.UserCode,
                        CREATE_DATE = insertDate,
                        QDETAIL = "NULL",
                        IsPass = item.LatestStatus == "PASS",
                        EXAM_SCORE = item.CorrectScore,
                        PASS_EXAM_DATE = EXAM_DATE,
                        UPD_USER_ID = item.TestReg.CertData.UserCode,
                        //APPLY_DATE = APPLY_DATE,
                        APPLY_DATE = item.TestReg.RegDateString,
                        OUT_COURSE_CODE = item.Subject.SubjectCode == "11" ? "201" : "301",
                    };

                    var qxx = new List<bool>();
                    string QDETAIL = "";

                    foreach (var question in item.RandomQuestions)
                    {
                        bool isCorrect = false;
                        if (question.UserAnswer.IsCorrect.HasValue && question.UserAnswer.IsCorrect.Value)
                        {
                            isCorrect = true;
                        }
                        qxx.Add(isCorrect);

                        //TODO : ADD REf ID
                        //QDETAIL += q.RefId + "|";
                        QDETAIL += question.QuestionNumber + "|";
                    }

                    testResult.Qxx = qxx.ToArray();

                    testResult.QDETAIL = QDETAIL.Substring(0, QDETAIL.Length - 1); //ตัด "|" ตัวสุดท้ายออก

                    _testresult.Add(testResult);

                    item.TestReg.ExamStatus = "SEND";
                }

                var endpointUrl = "http://61.91.64.134/DataSyncResult/Services/SyncResultDataService.svc";
                BasicHttpBinding binding = new BasicHttpBinding();
                EndpointAddress endpoint = new EndpointAddress(endpointUrl);
                ChannelFactory<ISyncResultDataService> channelFactory = new ChannelFactory<ISyncResultDataService>(binding, endpoint);
                ISyncResultDataService clientProxy = channelFactory.CreateChannel();

                WebClient web = new WebClient();
                web.DownloadString("http://61.91.64.134/DataSyncResult/Services/SyncResultQueService.svc");

                clientProxy.SyncResultToFileAsync(_testresult.ToArray());

                channelFactory.Close();

                repoSheet.UpdateExamSheet(sheetForSend.ToList());
            }
        }

        [HttpGet]
        [Route("ListExamLanguage/{centerid}/")]
        public AdminOnlineModels.TestBankVM ListExamSheet(string centerid)
        {
            var occ = repoSheet.ListOccupationGroup(centerid);
            AdminOnlineModels.TestBankVM result = new ViewModels.AdminOnlineModels.TestBankVM();

            string URL = string.Format("http://10.93.77.199/localdb/api/shared/GetLocalExamInfo/{0}", centerid);
            //string URL = string.Format("http://localhost:59684/api/shared/GetLocalExamInfo/{0}", centerid);
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                var responseString = client.DownloadString(URL);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<AdminOnlineModels.TestBankVM>(responseString);


                //Refer to reuse api in [exambankproject]/api/activated/listsubject
                //result.OccupationGroups = occ.Select(x =>
                //    new AdminOnlineModels.OccupationGroup
                //    {
                //        _id = x._id,
                //        Name = x.Name,
                //        SubjectGroups = x.SubjectGroups.Select(xx =>
                //            new AdminOnlineModels.SubjectGroup { _id = xx._id, Name = xx.Name, SubjectCodes = xx.Subjects.Select(xxx=>xxx.Code).ToList() }).ToList()
                //    }).ToList();

                return result;
            }
        }

        [HttpPost]
        [Route("UpdateExamSheetFromOnSite/")]
        public void UpdateExamSheetFromOnSite([FromBody]ViewModels.LocalDBModels.CloseExamRequest closeExamRequest)
        {
            // update sheet
            var serverSheet = repoSheet.ListExamSheetByID(closeExamRequest.ResultSheet.Select(x => x._id).ToList()).ToList();

            if (serverSheet.Count > 0)
            {
                foreach (var item in serverSheet)
                {
                    //No need to check pass or fail cuz this list is only has status
                    //if (item.LatestStatus == "PASS" || item.LatestStatus == "FAIL")
                    //{
                    var sheet = closeExamRequest.ResultSheet.Where(x => x._id == item._id).FirstOrDefault();

                    item.TestReg = new AdminOnlineModelsBack.TestRegistration
                    {
                        _id = sheet.TestReg._id,
                        Title = sheet.TestReg.Title,
                        FirstName = sheet.TestReg.FirstName,
                        LastName = sheet.TestReg.LastName,
                        SubjectCode = sheet.TestReg.SubjectCode,
                        SubjectName = sheet.TestReg.SubjectName,
                        ExamLanguage = sheet.TestReg.ExamLanguage,
                        VoiceLanguage = sheet.TestReg.VoiceLanguage,
                        RegDate = sheet.TestReg.RegDate,
                        RegDateString = sheet.TestReg.RegDateString,
                        ExpiredDate = sheet.TestReg.ExpriedDate,
                        SiteId = sheet.TestReg.SiteId,
                        CenterId = sheet.TestReg.CenterId,
                        ForTestSystem = sheet.TestReg.ForTestSystem,
                        ForPractice = sheet.TestReg.ForPractice,
                        Status = sheet.TestReg.Status,
                        ExamStatus = sheet.TestReg.ExamStatus,
                        PID = sheet.TestReg.PID,
                        ExamNumber = sheet.TestReg.ExamNumber,
                        ExamPeriod = sheet.TestReg.ExamPeriod,
                        AppointDate = sheet.TestReg.AppointDate,
                        Email = sheet.TestReg.Email,
                        Mobile = sheet.TestReg.Mobile,
                        Address = sheet.TestReg.Address,
                        MaxCount = sheet.TestReg.MaxCount,
                        CertData = new AdminOnlineModelsBack.CertData
                        {
                            CertNo = sheet.TestReg.CertData.CertNo,
                            CertYear = sheet.TestReg.CertData.CertYear,
                            UserCode = sheet.TestReg.CertData.UserCode
                        },
                        LatestCount = sheet.TestReg.LatestCount,
                    };

                    item.ExamDateTime = sheet.ExamDateTime;
                    item.LatestStatus = sheet.LatestStatus;
                    item.ClientId = sheet.ClientId;
                    item.StatusExtensions = sheet.StatusExtensions.Select(x => new AdminOnlineModelsBack.StatusExtension
                    {
                        _id = x._id,
                        ClientId = x.ClientId,
                        CreateDate = x.CreateDateTime,
                        Status = x.Status,
                    }).ToList();
                    item.TestCount = sheet.TestCount;
                    item.CorrectScore = sheet.CorrectScore;
                    item.InCorrectScore = sheet.InCorrectScore;

                    foreach (var question in item.RandomQuestions)
                    {
                        var qclient = sheet.RandomQuestion.Where(x => x._id == question._id).FirstOrDefault();

                        question.UserAnswer = new AdminOnlineModelsBack.Choice
                        {
                            _id = qclient?.UserAnswer?._id,
                            Detail = qclient?.UserAnswer?.Detail,
                            IsCorrect = qclient?.UserAnswer?.IsCorrect,
                        };
                    }
                    //}

                }

                repoSheet.UpdateExamSheet(serverSheet);


            }

            // update testregis

            var testRegis = closeExamRequest.ResultSheet.Select(x => x.TestReg);
            if (testRegis.Count() > 0)
            {
                var serverTestRegis = repoTestRegis.ListTestRegisByID(testRegis.Select(x => x._id).ToList()).ToList();

                foreach (var item in serverTestRegis)
                {
                    var regis = testRegis.Where(x => x._id == item._id).FirstOrDefault();

                    if (regis.Status == "PASS" || regis.Status == "FAIL")
                    {
                        item.Status = regis.Status;
                        item.LatestCount = regis.LatestCount;
                    }
                    else
                    {
                        if (regis.Status == "APPOINTED")
                        {
                            item.Status = "MISS";
                            item.LatestCount = regis.LatestCount;
                        }
                        else
                        {
                            item.Status = "MISS";
                            //เพราะเมื่อเร่ิม map ข้อสอบ LatestCount จะถูก +1
                            item.LatestCount = regis.LatestCount - 1;
                        }
                    }
                }
                repoTestRegis.UpdateLatestStatus(serverTestRegis);
            }

            if (closeExamRequest.MissTestReg != null)
            {
                var serverTestRegis2 = repoTestRegis.ListTestRegisByID(closeExamRequest.MissTestReg.Select(x => x._id).ToList()).ToList();
                if (serverTestRegis2.Count() > 0)
                {
                    foreach (var item in serverTestRegis2)
                    {
                        item.Status = "MISS";
                    }

                    repoTestRegis.UpdateLatestStatus(serverTestRegis2);
                }
            }
        }

        [HttpGet]
        [Route("ExamReport/{centerid}/{testdate}")]
        public AdminOnlineModels.TestResultResponse GetTestResultByDate(string centerid, DateTime testdate)
        {
            AdminOnlineModels.TestResultResponse result = new AdminOnlineModels.TestResultResponse();
            var data = repoSheet.GetTestResultByDate(centerid, testdate);
            var center = repoTestRegis.GetCenterData(centerid);

            if (center != null)
            {
                result.id = center._id;
                result.centerName = center.NameTH;
                result.totalsTest = data.Count();
                result.totalsTestPerson = data.GroupBy(x => x.TestReg.PID).Count();
                result.totalsPassTest = data.Where(x => x.LatestStatus == "PASS").Count();
                result.totalsFailTest = data.Where(x => x.LatestStatus == "FAIL").Count();
                result.percentagePassTest = result.totalsTest == 0 ? 0 : (result.totalsPassTest * 100) / result.totalsTest;
                result.percentageFailTest = result.totalsTest == 0 ? 0 : (result.totalsFailTest * 100) / result.totalsTest;
                result.normalTest = data.Where(x => x.TestCount == 1).Count();
                result.retest = data.Where(x => x.TestCount > 1).Count();
                result.examsheets = data.Select(x => new AdminOnlineModels.ExamsheetResult
                {
                    id = x._id,
                    ExamDateTime = x.ExamDateTime.HasValue ? x.ExamDateTime.Value : DateTime.Now,
                    LatestStatus = x.LatestStatus,
                    Score = x.CorrectScore,
                    TestCount = x.TestCount,
                    TestRegis = new AdminOnlineModels.TestRegistration
                    {
                        PID = x.TestReg.PID,
                        FirstName = x.TestReg.FirstName,
                        LastName = x.TestReg.LastName,
                        SubjectName = x.TestReg.SubjectName,
                    }
                }).ToList();
            };

            return result;
        }
    }
}

