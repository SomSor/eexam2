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
using WebSite.ViewModels.AdminOnlineModels;
using AdminOnlineModelsBack = WebSite.ViewModels.AdminOnlineModelsBack;
using WebSite.Repositories;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace WebSite.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class TestRegistrationController : Controller
    {
        private IRepoForRegistrationRepository repoRegis;
        private IRepoForSheetRepository repoSheet;
        public TestRegistrationController(IRepoForRegistrationRepository repoRegis, IRepoForSheetRepository repoSheet)
        {
            this.repoRegis = repoRegis;
            this.repoSheet = repoSheet;
        }


        [HttpGet]
        [Route("GetCenterData/{centerId}")]
        public AdminOnlineModelsBack.Center GetCenterData(string centerId)
        {
            return repoRegis.GetCenterData(centerId);
        }


        [HttpPost]
        [Route("SubmitTestRegis")]
        public SyncsTestRegisVM SubmitTestRegis([FromBody] List<TestRegistration> listTestRegis)
        {
            List<AdminOnlineModelsBack.TestRegistration> testregists = new List<ViewModels.AdminOnlineModelsBack.TestRegistration>();
            foreach (var item in listTestRegis)
            {
                testregists.Add(new ViewModels.AdminOnlineModelsBack.TestRegistration
                {
                    _id = item._id,
                    Title = item.Title,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    //SubjectCode = "th11_TH", //HACK
                    //SubjectName = "รถยนต์ส่วนบุคคลชั่วคราว ภาษาไทย", //HACK
                    SubjectCode = item.SubjectCode,
                    SubjectName = item.SubjectName,
                    ExamLanguage = item.ExamLanguage.ToUpper(),
                    VoiceLanguage = item.VoiceLanguage,
                    RegDate = item.RegDate,
                    RegDateString = item.RegDateString,
                    ExpiredDate = item.ExpiredDate,
                    SiteId = item.SiteId,
                    CenterId = item.CenterId,
                    ForTestSystem = item.ForTestSystem,
                    ForPractice = item.ForPractice,
                    Status = "APPOINTED",
                    PID = item.PID,
                    ExamNumber = item.ExamNumber,
                    //ExamPeriod = item.ExamPeriod,
                    ExamPeriod = "all", //HACK
                    AppointDate = item.AppointDate,
                    //Email =
                    //Mobile = 
                    //Address = 
                    MaxCount = item.MaxCount,
                    
                    //HACK : mock cert data
                    CertData = new ViewModels.AdminOnlineModelsBack.CertData { CertNo = "4050007", CertYear = "2550", UserCode = "9901" },
                    LatestCount = 0
                });
            }

            var ApprovedTestRegis = repoRegis.ListTestRegisByID(listTestRegis.Select(x => x._id).ToList());

            foreach (var item in ApprovedTestRegis)
            {
                item.Status = "APPOINTED";
                item.AppointDate = testregists.Where(x => x._id == item._id).FirstOrDefault().AppointDate;
            }
            repoRegis.UpdateAppointStatus(ApprovedTestRegis);

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(testregists);

            return new SyncsTestRegisVM { json = result };

            //Create Appoint To Onsiteoy
            //var jsonPreSheet = Newtonsoft.Json.JsonConvert.SerializeObject(testregists);

            //var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonPreSheet);

            //using (var client = new WebClient())
            //{
            //    client.Headers[HttpRequestHeader.ContentType] = "application/json";

            //    //dataByte = client.UploadData("http://localhost/localdb/api/shared/createtestregistration", "POST", dataByte);
            //    dataByte = client.UploadData("http://10.93.77.199/localdb/api/shared/createtestregistration", "POST", dataByte);
            //}
        }

        [HttpPost]
        [Route("SubmitTestRegisMiss")]
        public void SubmitTestRegisMiss([FromBody] List<TestRegistration> listTestRegis)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("SubmitTestRegisFail")]
        public void SubmitTestRegisFail([FromBody] List<TestRegistration> listTestRegis)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("ListAppointTestRegisByDate/{centerId}/{date}")]
        public MainVM ListAppointTestRegisByDate(string centerId, DateTime date)
        {
            var _testRegis = repoRegis.ListTestRegisByDate(centerId, date);

            var Result = new MainVM
            {
                Testregistrations = _testRegis.Select(x => new TestRegistration
                {
                    _id = x._id,
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SubjectCode = x.SubjectCode,
                    SubjectName = x.SubjectName,
                    ExamLanguage = x.ExamLanguage,
                    VoiceLanguage = x.VoiceLanguage,
                    RegDate = x.RegDate,
                    RegDateString = x.RegDateString,
                    ExpiredDate = x.ExpiredDate,
                    SiteId = x.SiteId,
                    CenterId = x.CenterId,
                    ForPractice = x.ForPractice,
                    ForTestSystem = x.ForTestSystem,
                    Status = x.Status,
                    PID = x.PID,
                    ExamNumber = x.ExamNumber,
                    ExamPeriod = x.ExamPeriod,
                    AppointDate = x.AppointDate,
                    MaxCount = x.MaxCount
                }).ToList(),
                AppointDates = repoRegis.ListAppointDate(centerId).OrderBy(x => x).ToList(),
            };
            return Result;
        }

        [HttpGet]
        [Route("SerachAppointTestRegis/{centerId}/{txt}")]
        public MainVM SerachAppointTestRegis(string centerId, string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
            {
                var _testRegis = repoRegis.SearchTestRegis(centerId, txt);

                var Result = new MainVM
                {
                    Testregistrations = _testRegis.Select(x => new TestRegistration
                    {
                        _id = x._id,
                        Title = x.Title,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        SubjectCode = x.SubjectCode,
                        SubjectName = x.SubjectName,
                        ExamLanguage = x.ExamLanguage,
                        VoiceLanguage = x.VoiceLanguage,
                        RegDate = x.RegDate,
                        RegDateString = x.RegDateString,
                        ExpiredDate = x.ExpiredDate,
                        SiteId = x.SiteId,
                        CenterId = x.CenterId,
                        ForPractice = x.ForPractice,
                        ForTestSystem = x.ForTestSystem,
                        Status = x.Status,
                        PID = x.PID,
                        ExamNumber = x.ExamNumber,
                        ExamPeriod = x.ExamPeriod,
                        AppointDate = x.AppointDate,
                        MaxCount = x.MaxCount
                    }).ToList(),
                    AppointDates = repoRegis.ListAppointDate(centerId).OrderBy(x => x).ToList(),
                };

                return Result;
            }
            else
            {
                return new MainVM
                {
                    Testregistrations = new List<TestRegistration>(),
                    AppointDates = repoRegis.ListAppointDate(centerId).OrderBy(x => x).ToList(),
                };
            }

        }

        [HttpGet]
        [Route("ListTestRegisForApproved/{centerId}")]
        public SyncsTestRegisVM ListTestRegisForApproved(string centerId)
        {
            //FIX FOR ขนส่ง
            //var site = repoRegis.GetSiteData(centerId);
            //if (site._id == "02")
            //{
            //GetTestRegisFrom3rd(centerId);
            //}

            var _testRegis = repoRegis.ListForAproved(centerId);

            var Result = new SyncsTestRegisVM
            {
                TestRegistrations = _testRegis.Select(x => new TestRegistration
                {
                    _id = x._id,
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SubjectCode = x.SubjectCode,
                    SubjectName = x.SubjectName,
                    ExamLanguage = x.ExamLanguage,
                    VoiceLanguage = x.VoiceLanguage,
                    RegDate = x.RegDate,
                    RegDateString = x.RegDateString,
                    ExpiredDate = x.ExpiredDate,
                    SiteId = x.SiteId,
                    CenterId = x.CenterId,
                    ForPractice = x.ForPractice,
                    ForTestSystem = x.ForTestSystem,
                    Status = x.Status,
                    PID = x.PID,
                    ExamNumber = x.ExamNumber,
                    ExamPeriod = x.ExamPeriod,
                    AppointDate = x.AppointDate,
                    MaxCount = x.MaxCount
                }).ToList(),
                LanguageSources = null,
            };
            return Result;

            //return new SyncsTestRegisVM()
            //{
            //    TestRegistrations = new List<TestRegistration>()
            //    {
            //        new TestRegistration() { _id="1", FirstName = "1", Status = "APPROVED" },
            //        new TestRegistration() { _id="2", FirstName = "2", Status = "APPROVED" },
            //        new TestRegistration() { _id="3", FirstName = "3", Status = "MISS" },
            //        new TestRegistration() { _id="4", FirstName = "4", Status = "MISS" },
            //        new TestRegistration() { _id="5", FirstName = "5", Status = "FAIL" },
            //        new TestRegistration() { _id="6", FirstName = "6", Status = "FAIL" },
            //    }
            //};
        }

        [HttpGet]
        [Route("ListResultTestRegis/{centerId}")]
        public List<TestRegistration> ListResultTestRegis(string centerId)
        {
            //var _testRegis = repoRegis.ListResultTestRegis(centerId);            

            //var Result = _testRegis.Select(x => new TestRegistration
            //{
            //    _id = x._id,
            //    Title = x.Title,
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    SubjectCode = x.SubjectCode,
            //    SubjectName = x.SubjectName,
            //    ExamLanguage = x.ExamLanguage,
            //    VoiceLanguage = x.VoiceLanguage,
            //    RegDate = x.RegDate,
            //      RegDateString = x.RegDateString,
            //    ExpiredDate = x.ExpiredDate,
            //    SiteId = x.SiteId,
            //    CenterId = x.CenterId,
            //    ForPractice = x.ForPractice,
            //    ForTestSystem = x.ForTestSystem,
            //    Status = x.Status,
            //    PID = x.PID,
            //    ExamNumber = x.ExamNumber,
            //    ExamPeriod = x.ExamPeriod,
            //    AppointDate = x.AppointDate.Value,
            //    MaxCount = x.MaxCount
            //}).ToList();
            //return Result;

            var sheet = repoSheet.ListExamSheetForSendTo3rd(centerId);

            var Result = sheet.Select(x => new TestRegistration
            {
                _id = x.TestReg._id,
                Title = x.TestReg.Title,
                FirstName = x.TestReg.FirstName,
                LastName = x.TestReg.LastName,
                SubjectCode = x.TestReg.SubjectCode,
                SubjectName = x.TestReg.SubjectName,
                ExamLanguage = x.TestReg.ExamLanguage,
                VoiceLanguage = x.TestReg.VoiceLanguage,
                RegDate = x.TestReg.RegDate,
                RegDateString = x.TestReg.RegDateString,
                ExpiredDate = x.TestReg.ExpiredDate,
                SiteId = x.TestReg.SiteId,
                CenterId = x.TestReg.CenterId,
                ForPractice = x.TestReg.ForPractice,
                ForTestSystem = x.TestReg.ForTestSystem,
                Status = x.LatestStatus,
                PID = x.TestReg.PID,
                ExamNumber = x.TestReg.ExamNumber,
                ExamPeriod = x.TestReg.ExamPeriod,
                AppointDate = x.TestReg.AppointDate.Value,
                MaxCount = x.TestReg.MaxCount,
                SheetId = x._id,
                TestCount = x.TestCount
            }).ToList();
            return Result;
        }


        [HttpGet]
        [Route("GetTestRegisFrom3rd/{centerId}")]
        public void GetTestRegisFrom3rd(string centerId)
        {
            //getcenterInfo
            var center = repoRegis.GetCenterData(centerId);
            var site = repoRegis.GetSiteData(center.SiteId);

            List<AdminOnlineModelsBack.TestTaker> From3rd = new List<AdminOnlineModelsBack.TestTaker>();

            //ForTest
            //var site = new ViewModels.AdminOnlineModelsBack.Site
            //{
            //    MaxTestCount = 10
            //};
            //var center = new ViewModels.AdminOnlineModelsBack.Center
            //{
            //    CertDatas = new List<ViewModels.AdminOnlineModelsBack.CertData> {
            //        new ViewModels.AdminOnlineModelsBack.CertData { CertNo = "4050007", CertYear = "2550"},
            //    }
            //};

            //get from 3rd 

            using (var client = new WebClient())
            {
                try
                {
                    foreach (var item in center.CertDatas)
                    {
                        string URL = string.Format("http://61.91.64.134/examservice/api/SyncData/{0}/{1}", item.CertNo, item.CertYear);

                        //var responseString = client.DownloadString(URL);
                        var dataByte = client.DownloadData(URL);
                        var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<AdminOnlineModelsBack.TestTaker>>(responseString);

                        if (data.Count() > 0)
                        {
                            From3rd.AddRange(data);
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(" การเชื่อมการ กรม ขัดข้อง " + e.ToString());
                }
            }

            //get from testTaker
            if (From3rd.Count() > 0)
            {
                // เอา เปรียบเทียบ เอาคนที่ยังไม่เคย นำเช้า

                var oldRegis = repoRegis.ListTestRegisByPID(From3rd.Select(x => x.ID_NO).ToList());

                List<AdminOnlineModelsBack.TestRegistration> _listNewTestRegis = new List<AdminOnlineModelsBack.TestRegistration>();
                foreach (var item in From3rd)
                {
                    string subjectName = string.Empty;

                    if (item.PLT_CODE == "11")
                    {
                        subjectName = "รถยนต์ส่วนบุคคลชั่วคราว";
                    }
                    else if (item.PLT_CODE == "13")
                    {
                        subjectName = "รถจักรยานยนต์ส่วนบุคคลชั่วคราว";
                    }

                    var regDate = DateTime.Parse(item.REQ_DATE).ToUniversalTime();

                    //var ss = oldRegis.Where(x => x.PID == item.ID_NO && x.RegDate.Date == regDate.Date && x.CenterId == centerId).ToList();
                    //var ss2 = oldRegis.Where(x => x.PID == item.ID_NO && x.RegDate.Day == regDate.Day && x.RegDate.Month == regDate.Month && x.RegDate.Year == regDate.Year && x.CenterId == centerId).ToList();
                    //var ss3 = oldRegis.Where(x => x.PID == item.ID_NO && x.CenterId == centerId).ToList();
                    //var ss4 = oldRegis.Where(x => x.PID == item.ID_NO && x.RegDate >= regDate.Date && x.RegDate < regDate.Date.AddDays(1) && x.CenterId == centerId).ToList();



                    if (oldRegis.Where(x => x.PID == item.ID_NO && x.CenterId == centerId && x.RegDate == regDate).Count() < 1)
                    {
                        var newTestreggis = new AdminOnlineModelsBack.TestRegistration
                        {
                            _id = Guid.NewGuid().ToString(),
                            Title = item.TITLE_CODE,
                            FirstName = item.FNAME,
                            LastName = item.LNAME,
                            SubjectCode = item.PLT_CODE,
                            SubjectName = subjectName,
                            ExamLanguage = "th",
                            VoiceLanguage = "th",
                            RegDate = regDate, //TODO : Bug datetime mongo 
                            RegDateString = item.REQ_DATE,
                            ExpiredDate = regDate.AddDays(90),
                            SiteId = site._id,
                            CenterId = center._id,
                            ForPractice = false,
                            ForTestSystem = false,
                            Status = "APPROVED",
                            ExamStatus = "UNSEND",
                            PID = item.ID_NO,
                            ExamNumber = item.ID_NO,
                            ExamPeriod = "all",
                            AppointDate = null, //กำหนดได้อีกทีตอนนัด
                            MaxCount = site.MaxTestCount,
                            CertData = center.CertDatas.Where(x => x.CertNo == item.CERT_NO && x.CertYear == item.CERT_YY).FirstOrDefault(),
                        };
                        _listNewTestRegis.Add(newTestreggis);
                    }
                }

                if (_listNewTestRegis.Count > 0)
                {
                    // บันทึก ใส่ db  TestRegistration
                    repoRegis.CreateTestRegis(_listNewTestRegis.ToList());
                }


            }

            else
            {

            }


        }





    }
}
