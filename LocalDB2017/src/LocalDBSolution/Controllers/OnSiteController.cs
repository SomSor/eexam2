using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LocalDBSolution.Repositories;
using LocalDBSolution.ViewModels;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace LocalDBSolution.Controllers
{
    [Route("api/[controller]")]
    public class OnSiteController : Controller
    {
        private IRepoForOnSite repoOnSite;

        public OnSiteController(IRepoForOnSite repoOnSite)
        {
            this.repoOnSite = repoOnSite;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("GetCenterData")]
        public Center GetCenterData()
        {
            var data = repoOnSite.GetCenter();

            var result = new Center
            {
                _id = data._id,
                NameTh = data.NameTh,
                NameEn = data.NameEn,
                CertDatas = data.CertDatas.Select(x => new CertData { CertNo = x.CertNo, CertYear = x.CertYear, UserCode = x.UserCode }).ToList(),
                SiteName = data.SiteName,
                SiteId = data.SiteId
            };

            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("Login/{user}/{pass}/{centerId}")]
        public MessageRespone Login(string user, string pass, string centerId)
        {
            var data = repoOnSite.Login(user, pass, centerId);

            var result = new MessageRespone
            {
                Code = data.Code,
                Message = data.Message,
            };
            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("GetMainInfo/{centerid}")]
        public MainVM GetMainInfo(string centerid)
        {
            var result = new MainVM
            {
                TestRegisCount = repoOnSite.GetTestRegisCount(centerid),
                ActiveThruDateTime = repoOnSite.GetActive(centerid).ActiveThruDateTime,
                IsExamEnough = repoOnSite.CheckExamEnough(centerid),
            };

            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("ListTestRegistration/{centerid}")]
        public DisplayAllVM ListTestRegistration(string centerid)
        {
            var fromtestRegis = repoOnSite.ListTestRegis(centerid);

            if (fromtestRegis.Count() > 0)
            {
                var mappedsheet = repoOnSite.GetMappedSheet(centerid);

                var fromtestRegisExistMappedsheet = fromtestRegis.Where(p2 => mappedsheet.All(p1 => p1.TestRegisID != p2._id));

                var _listTestRegisBySheet = repoOnSite.GetTestRegisByIds(mappedsheet.Select(x => x.TestRegisID).ToList());

                List<TestRegistrationRespone> formsheet = new List<TestRegistrationRespone>();

                foreach (var item in mappedsheet)
                {
                    var regis = _listTestRegisBySheet.Where(x => x._id == item.TestRegisID).FirstOrDefault();

                    if (regis != null)
                    {
                        var sheetRegis = new TestRegistrationRespone()
                        {
                            _id = regis._id,
                            Title = regis.Title,
                            FirstName = regis.FirstName,
                            LastName = regis.LastName,
                            SubjectCode = regis.SubjectCode,
                            SubjectName = regis.SubjectName,
                            ExamLanguage = regis.ExamLanguage,
                            VoiceLanguage = regis.VoiceLanguage,
                            RegDate = regis.RegDate,
                            RegDateString = regis.RegDateString,
                            ExpriedDate = regis.ExpriedDate,
                            SiteId = regis.SiteId,
                            CenterId = regis.CenterId,
                            ForPractice = regis.ForPractice,
                            ForTestSystem = regis.ForTestSystem,
                            Status = item.LatestStatus,
                            PID = regis.PID,
                            ExamNumber = regis.ExamNumber,
                            ExamPeriod = regis.ExamPeriod,
                            AppointDate = regis.AppointDate,
                            Address = regis.Address,
                            MaxCount = regis.MaxCount,
                            LatestCount = item.TestCount,
                            CertData = new CertData
                            {
                                CertNo = regis.CertData.CertNo,
                                CertYear = regis.CertData.CertYear,
                                UserCode = regis.CertData.UserCode
                            },
                            IsSync = item.IsSync,
                            SheetId = item._id,
                            CorrectCount = item.CorrectScore,
                            QuestionCount = item.RandomQuestions.Count(),
                        };

                        formsheet.Add(sheetRegis); 
                    }
                }

                var result = new DisplayAllVM
                {
                    TestRegistrations = fromtestRegisExistMappedsheet.Select(x => new TestRegistrationRespone
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
                        ExpriedDate = x.ExpriedDate,
                        SiteId = x.SiteId,
                        CenterId = x.CenterId,
                        ForPractice = x.ForPractice,
                        ForTestSystem = x.ForTestSystem,
                        Status = x.Status,
                        PID = x.PID,
                        ExamNumber = x.ExamNumber,
                        ExamPeriod = x.ExamPeriod,
                        AppointDate = x.AppointDate,
                        Address = x.Address,
                        MaxCount = x.MaxCount,
                        CertData = new CertData
                        {
                            CertNo = x.CertData.CertNo,
                            CertYear = x.CertData.CertYear,
                            UserCode = x.CertData.UserCode
                        },
                        LatestCount = x.LatestCount <= 1 ? 1 : x.LatestCount,
                        IsSync = x.IsSync,
                        SheetId = string.Empty,
                        CorrectCount = 0,
                        QuestionCount = 0,
                    }).ToList(),
                };
                result.TestRegistrations.AddRange(formsheet);

                return result; 
            }else { return new DisplayAllVM { TestRegistrations = new List<ViewModels.TestRegistrationRespone>() }; }
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("Active")]
        public void Active([FromBody]ActiveRequest activeRequest)
        {
            repoOnSite.Active(activeRequest.ActiveThruDateTime, activeRequest.centerId);
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("EndTest")]
        public void EndTest([FromBody] ActionSheetRequest actionSheetRequest)
        {
            var sheet = repoOnSite.GetSheetBySheetId(actionSheetRequest.sheetId);

            if (sheet != null)
            {
                //foreach (var item in sheet.RandomQuestions)
                //{
                //    if (item.UserAnswer != null)
                //    {
                //        item.UserAnswer.IsCorrect = item.Choices.Where(x => x._id == item.UserAnswer._id).FirstOrDefault().IsCorrect;
                //    }
                //}

                //sheet.CorrectScore = sheet.RandomQuestions.Where(x => x.UserAnswer.IsCorrect.Value).Count();
                //sheet.InCorrectScore = sheet.RandomQuestions.Where(x => !x.UserAnswer.IsCorrect.Value).Count();

                //if (sheet.CorrectScore >= sheet.Subject.PassScore)
                //{
                //    sheet.LatestStatus = "PASS";
                //}
                //else
                //{
                //    sheet.LatestStatus = "FAIL";
                //}

                var newStatus = new StatusExtension
                {
                    _id = Guid.NewGuid().ToString(),
                    ClientId = "admin",
                    CreateDateTime = DateTime.Now,
                    //Status = sheet.LatestStatus
                    Status = sheet.CorrectScore >= sheet.Subject.PassScore ? "PASS" : "FAIL",
                };
                sheet.StatusExtensions.Add(newStatus);
                sheet.LatestStatus = newStatus.Status;
                sheet.ExamDateTime = DateTime.Now;

                repoOnSite.UpdateSheet(sheet);

                if (sheet.LatestStatus == "PASS")
                {
                    SyncExam(sheet._id);
                }

            }
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("Resume")]
        public void Resume([FromBody] ActionSheetRequest actionSheetRequest)
        {
            var sheet = repoOnSite.GetSheetBySheetId(actionSheetRequest.sheetId);

            if (sheet != null)
            {
                var newStatus = new StatusExtension
                {
                    _id = Guid.NewGuid().ToString(),
                    ClientId = "admin",
                    CreateDateTime = DateTime.Now,
                    Status = "RESUME",
                };

                sheet.StatusExtensions.Add(newStatus);
                sheet.LatestStatus = "RESUME";
                repoOnSite.UpdateSheet(sheet);
            }
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("Cancel")]
        public void Cancel([FromBody] ActionSheetRequest actionSheetRequest)
        {
            var sheet = repoOnSite.GetSheetBySheetId(actionSheetRequest.sheetId);

            if (sheet != null)
            {
                var newStatus = new StatusExtension
                {
                    _id = Guid.NewGuid().ToString(),
                    ClientId = "ForTest",
                    CreateDateTime = DateTime.Now,
                    Status = "CANCEL",
                };

                sheet.StatusExtensions.Add(newStatus);
                sheet.LatestStatus = "CANCEL";
                repoOnSite.UpdateSheet(sheet);
            }
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("SearchTestRegis/{txt}/{centerid}")]
        public DisplayAllVM SearchTestRegis(string txt, string centerid)
        {
            var data = repoOnSite.SearchTestRegis(txt, centerid);

            var result = new DisplayAllVM
            {
                TestRegistrations = data.Select(x => new TestRegistrationRespone
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
                    ExpriedDate = x.ExpriedDate,
                    SiteId = x.SiteId,
                    CenterId = x.CenterId,
                    ForPractice = x.ForPractice,
                    ForTestSystem = x.ForTestSystem,
                    Status = x.Status,
                    PID = x.PID,
                    ExamNumber = x.ExamNumber,
                    ExamPeriod = x.ExamPeriod,
                    AppointDate = x.AppointDate,
                    Address = x.Address,
                    MaxCount = x.MaxCount,
                    CertData = new CertData
                    {
                        CertNo = x.CertData.CertNo,
                        CertYear = x.CertData.CertYear,
                        UserCode = x.CertData.UserCode
                    }


                }).ToList(),
            };

            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("ListTesting/{centerid}")]
        public TestingVM ListTesting(string centerid)
        {
            var testingSheet = repoOnSite.ListTestingSheet(centerid);
            if (testingSheet.Count() > 0)
            {
                var _listTestRegisBySheet = repoOnSite.GetTestRegisByIds(testingSheet.Select(x => x.TestRegisID).ToList());

                List<TestRegistrationRespone> formsheet = new List<TestRegistrationRespone>();

                foreach (var item in testingSheet)
                {
                    var regis = _listTestRegisBySheet.Where(x => x._id == item.TestRegisID).FirstOrDefault();

                    var sheetRegis = new TestRegistrationRespone()
                    {
                        _id = regis._id,
                        Title = regis.Title,
                        FirstName = regis.FirstName,
                        LastName = regis.LastName,
                        SubjectCode = regis.SubjectCode,
                        SubjectName = regis.SubjectName,
                        ExamLanguage = regis.ExamLanguage,
                        VoiceLanguage = regis.VoiceLanguage,
                        RegDate = regis.RegDate,
                        ExpriedDate = regis.ExpriedDate,
                        SiteId = regis.SiteId,
                        CenterId = regis.CenterId,
                        ForPractice = regis.ForPractice,
                        ForTestSystem = regis.ForTestSystem,
                        Status = item.LatestStatus,
                        PID = regis.PID,
                        ExamNumber = regis.ExamNumber,
                        ExamPeriod = regis.ExamPeriod,
                        AppointDate = regis.AppointDate,
                        Address = regis.Address,
                        MaxCount = regis.MaxCount,
                        LatestCount = regis.LatestCount,
                        CertData = new CertData
                        {
                            CertNo = regis.CertData.CertNo,
                            CertYear = regis.CertData.CertYear,
                            UserCode = regis.CertData.UserCode
                        },
                        IsSync = item.IsSync,
                        SheetId = item._id,
                        EndExamThruTime = item.ActiveThruDateTime,
                    };

                    formsheet.Add(sheetRegis);
                }

                var result = new TestingVM
                {
                    TestRegistrations = formsheet,
                };

                return result;
            }
            else
            {
                return null;
            }

        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("GetInfoForPrintQR/{pid}")]
        public PrintQRVM GetInfoForPrintQR(string pid)
        {
            var data = repoOnSite.GetTestRegisInfoForPrintQR(pid);

            var result = new PrintQRVM
            {
                TestRegistrations = new TestRegistration
                {
                    _id = data._id,
                    Title = data.Title,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    SubjectCode = data.SubjectCode,
                    SubjectName = data.SubjectName,
                    ExamLanguage = data.ExamLanguage,
                    VoiceLanguage = data.VoiceLanguage,
                    RegDate = data.RegDate,
                    RegDateString = data.RegDateString,
                    ExpriedDate = data.ExpriedDate,
                    SiteId = data.SiteId,
                    CenterId = data.CenterId,
                    ForPractice = data.ForPractice,
                    ForTestSystem = data.ForTestSystem,
                    Status = data.Status,
                    PID = data.PID,
                    ExamNumber = data.ExamNumber,
                    ExamPeriod = data.ExamPeriod,
                    AppointDate = data.AppointDate,
                    Address = data.Address,
                    MaxCount = data.MaxCount,
                    CertData = new CertData
                    {
                        CertNo = data.CertData.CertNo,
                        CertYear = data.CertData.CertYear,
                        UserCode = data.CertData.UserCode
                    }

                },
            };

            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("GetResult/{pid}")]
        public CheckResultVM GetResult(string pid)
        {
            var resultData = repoOnSite.GetResultInfo(pid);

            if (resultData.Count() > 0)
            {
                var _listTestRegisBySheet = repoOnSite.GetTestRegisByIds(resultData.Select(x => x.TestRegisID).ToList());

                List<Result> formsheet = new List<Result>();

                foreach (var item in resultData)
                {
                    var regis = _listTestRegisBySheet.Where(x => x._id == item.TestRegisID).FirstOrDefault();

                    var dd = new Result
                    {
                        _id = item._id,
                        Title = regis.Title,
                        Firstname = regis.FirstName,
                        LastName = regis.LastName,
                        SubjectCode = item.Subject.SubjectCode,
                        SubjectName = item.Subject.SubjectName,
                        ExamNumber = regis.ExamNumber,
                        PID = regis.PID,
                        Status = item.LatestStatus,
                        TestCount = item.TestCount,
                        CorrectCount = item.CorrectScore,
                        InCorrectCount = item.InCorrectScore
                    };
                    formsheet.Add(dd);
                }

                var result = new CheckResultVM
                {
                    Results = formsheet,
                };

                return result;
            }
            else
            {
                return null;
            }
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("GetResult/{pid}/{sheetid}")]
        public Result GetTestResult(string pid, string sheetid)
        {
            var sheet = repoOnSite.GetTestResultInfo(pid, sheetid);

            var regis = repoOnSite.GetTestRegisById(sheet.TestRegisID);

            var center = this.GetCenterData();

            var result = new Result
            {
                _id = sheet._id,
                Title = regis.Title,
                Firstname = regis.FirstName,
                LastName = regis.LastName,
                SubjectCode = sheet.Subject.SubjectCode,
                SubjectName = sheet.Subject.SubjectName,
                ExamNumber = regis.ExamNumber,
                PID = regis.PID,
                Status = sheet.LatestStatus,
                TestCount = sheet.TestCount,
                CorrectCount = sheet.CorrectScore,
                InCorrectCount = sheet.InCorrectScore,
                ExamDateTime = sheet.StartDateTime.HasValue ? sheet.StartDateTime.Value : new DateTime(),
                CenterNameTH = center.NameTh,
            };

            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("ListExamData/{centerid}")]
        public ExamDataVM ListExamData(string centerid)
        {
            var sheet = repoOnSite.ListExamData(centerid).Where(x=> string.IsNullOrEmpty(x.TestRegisID));
            var testRegis = repoOnSite.ListTestRegis(centerid);

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

            var result = new ExamDataVM
            {
                ExamSheets = group.ToList(),
            };
            return result;
        }

        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("CloseExamData")]
        public void CloseExamData()
        {
            var center = repoOnSite.GetCenter();
            var listSheet = repoOnSite.ListExamData(center._id).Where(x => !String.IsNullOrEmpty(x.TestRegisID)).ToList();

            foreach (var sheet in listSheet)
            {
                if (sheet.LatestStatus == "DONE")
                {
                    foreach (var q in sheet.RandomQuestions)
                    {
                        if (q.UserAnswer != null)
                        {
                            q.UserAnswer.IsCorrect = q.Choices.Where(x => x._id == q.UserAnswer._id).FirstOrDefault().IsCorrect;
                        }
                    }

                    sheet.CorrectScore = sheet.RandomQuestions.Where(x => x.UserAnswer.IsCorrect.Value).Count();
                    sheet.InCorrectScore = sheet.RandomQuestions.Where(x => !x.UserAnswer.IsCorrect.Value).Count();

                    if (sheet.CorrectScore >= sheet.Subject.PassScore)
                        sheet.LatestStatus = "PASS";
                    else
                        sheet.LatestStatus = "FAIL";

                    var newStatus = new StatusExtension
                    {
                        _id = Guid.NewGuid().ToString(),
                        ClientId = "admin",
                        CreateDateTime = DateTime.Now,
                        Status = sheet.LatestStatus
                    };
                    sheet.StatusExtensions.Add(newStatus);
                    sheet.ExamDateTime = DateTime.Now;
                }


                sheet.IsCloseExam = true;
                sheet.IsSync = true;
            }

            var regisList = repoOnSite.ListTestRegis(center._id).ToList();

            foreach (var item in regisList)
            {
                item.IsCloseExam = true;
            }

            List<ExamSheetOnline> sheetForOnline = new List<ExamSheetOnline>();

            var listReg = repoOnSite.GetTestRegisByIds(listSheet.Select(x => x.TestRegisID).ToList());

            foreach (var item in listSheet)
            {
                var regis = listReg.Where(x => x._id == item.TestRegisID).FirstOrDefault();

                var regisOnline = new TestRegistrationOnline
                {
                    _id = regis._id,
                    Title = regis.Title,
                    FirstName = regis.FirstName,
                    LastName = regis.LastName,
                    SubjectCode = regis.SubjectCode,
                    SubjectName = regis.SubjectName,
                    ExamLanguage = regis.ExamLanguage,
                    VoiceLanguage = regis.VoiceLanguage,
                    RegDate = regis.RegDate,
                    RegDateString = regis.RegDateString,
                    ExpriedDate = regis.ExpriedDate,
                    SiteId = regis.SiteId,
                    CenterId = regis.CenterId,
                    ForTestSystem = regis.ForTestSystem,
                    ForPractice = regis.ForPractice,
                    Status = regis.Status,
                    ExamStatus = regis.ExamStatus,
                    PID = regis.PID,
                    ExamNumber = regis.ExamNumber,
                    ExamPeriod = regis.ExamPeriod,
                    AppointDate = regis.AppointDate,
                    Email = regis.Email,
                    Mobile = regis.Mobile,
                    Address = regis.Address,
                    MaxCount = regis.MaxCount,
                    CertData = regis.CertData,
                    LatestCount = regis.LatestCount,
                };


                var sheet = new ExamSheetOnline
                {
                    _id = item._id,
                    Subject = item.Subject,
                    TestReg = regisOnline,
                    TestCount = item.TestCount,
                    LatestStatus = item.LatestStatus,
                    ExamDateTime = item.ExamDateTime,
                    StatusExtensions = item.StatusExtensions,
                    RandomQuestion = item.RandomQuestions,
                    CenterId = item.CenterId,
                    CorrectScore = item.CorrectScore,
                    InCorrectScore = item.InCorrectScore,
                    ReviewDuration = item.ReviewDuration,
                    CreateDate = item.CreateDate,
                    ClientId = item.ClientId
                };

                sheetForOnline.Add(sheet);
            }

            var missTestReg = regisList.Where(x => !listSheet.Select(y => y.TestRegisID).Contains(x._id)).Select(regis => new TestRegistrationOnline()
            {
                _id = regis._id,
                Title = regis.Title,
                FirstName = regis.FirstName,
                LastName = regis.LastName,
                SubjectCode = regis.SubjectCode,
                SubjectName = regis.SubjectName,
                ExamLanguage = regis.ExamLanguage,
                VoiceLanguage = regis.VoiceLanguage,
                RegDate = regis.RegDate,
                RegDateString = regis.RegDateString,
                ExpriedDate = regis.ExpriedDate,
                SiteId = regis.SiteId,
                CenterId = regis.CenterId,
                ForTestSystem = regis.ForTestSystem,
                ForPractice = regis.ForPractice,
                Status = regis.Status,
                ExamStatus = regis.ExamStatus,
                PID = regis.PID,
                ExamNumber = regis.ExamNumber,
                ExamPeriod = regis.ExamPeriod,
                AppointDate = regis.AppointDate,
                Email = regis.Email,
                Mobile = regis.Mobile,
                Address = regis.Address,
                MaxCount = regis.MaxCount,
                CertData = regis.CertData,
                LatestCount = regis.LatestCount,
            }).ToList();

            CloseExamRequest request = new CloseExamRequest()
            {
                ResultSheet = sheetForOnline,
                MissTestReg = missTestReg,
            };

            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    dataByte = client.UploadData("http://eexamthaiex.azurewebsites.net/api/ExamSheet/UpdateExamSheetFromOnSite/", "POST", dataByte);
                    //dataByte = client.UploadData("http://localhost:10585/api/ExamSheet/UpdateExamSheetFromOnSite/", "POST", dataByte);

                    //update sheet
                    repoOnSite.CloseSheet(listSheet);
                    //update Regis
                    repoOnSite.CloseTestRegis(regisList);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }


        }

        private void SyncExam(string sheetid)
        {
            var sheet = repoOnSite.GetSheetBySheetId(sheetid);
            var regis = repoOnSite.GetTestRegisById(sheet.TestRegisID);

            if (sheet.LatestStatus == "PASS")
            {
                sheet.IsSync = true;
                regis.IsSync = true;
            }

            List<ExamSheet> sheetList = new List<ExamSheet>();
            sheetList.Add(sheet);

            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(sheetList);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData("http://eexamthaiex.azurewebsites.net/api/ExamSheet/UpdateExamSheetFromOnSite/", "POST", dataByte);

                    //update sheet
                    repoOnSite.UpdateSheet(sheet);
                    //update Regis
                    repoOnSite.UpdateTestRegis(regis);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost]
        [Route("ChangeLanguage")]
        public void ChangeLanguage([FromBody] TestRegistration testreg)
        {
            var reg = repoOnSite.GetTestRegisById(testreg._id);

            if (reg != null)
            {
                reg.ExamLanguage = testreg.ExamLanguage;
                reg.VoiceLanguage = testreg.VoiceLanguage;
                repoOnSite.UpdateTestRegis(reg);
            }
        }
    }
}
