using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LocalDBSolution.Repositories;
using LocalDBSolution.ViewModels;
using System.Net;

namespace LocalDBSolution.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private IRepoForOnSite repoOnSite;
        private string blobStorage;
        private string root;
        private string storageCon;
        private string localIP;
        private string getsheet0;
        private string getsheet1;
        private string getsheet2;
        private string getsheet3;


        public ClientController(IRepoForOnSite repoOnSite)
        {
            this.repoOnSite = repoOnSite;

            localIP = System.Configuration.ConfigurationSettings.AppSettings.Get("LocalIP");
            blobStorage = System.Configuration.ConfigurationSettings.AppSettings.Get("blobStorage");
            root = System.Configuration.ConfigurationSettings.AppSettings.Get("root");
            storageCon = System.Configuration.ConfigurationSettings.AppSettings.Get("StorageConnectionString");
            getsheet0 = System.Configuration.ConfigurationSettings.AppSettings.Get("getsheet0");
            getsheet1 = System.Configuration.ConfigurationSettings.AppSettings.Get("getsheet1");
            getsheet2 = System.Configuration.ConfigurationSettings.AppSettings.Get("getsheet2");
            getsheet3 = System.Configuration.ConfigurationSettings.AppSettings.Get("getsheet3");

        }

        [HttpGet]
        [Route("GetExamRegis/{pid}")]
        public PreExamRespone GetExamRegis(string pid)
        {
            var data = repoOnSite.GetExamRegisInfo(pid);

            if (data.Count() > 0)
            {
                var regisInfo = data.FirstOrDefault();

                var result = new PreExamRespone
                {
                    PID = regisInfo.PID,
                    Firstname = regisInfo.FirstName,
                    LastName = regisInfo.LastName,
                    ExamNumber = regisInfo.ExamNumber,
                    Title = regisInfo.Title,
                    SubjectRespones = data.Select(x => new SubjectRespone
                    {
                        SubjectCode = x.SubjectCode,
                        SubjectName = x.SubjectName,

                    }).ToList(),
                };
                return result;
            }
            else
            {
                return null;
            }


        }

        [HttpGet]
        [Route("GetLastActive")]
        public ActiveRespone GetLastActive()
        {
            var data = repoOnSite.GetLastActive();

            var result = new ActiveRespone
            {
                ActiveDateTime = data.ActiveFromDateTime,
                ActiveThruTime = data.ActiveThruDateTime,
                IsActive = DateTime.Now <= data.ActiveThruDateTime
            };

            return result;
        }

        [HttpGet]
        [Route("GetClientId/{uuid}")]
        public ClientMapResponse GetClientId(string uuid)
        {
            var data = repoOnSite.GetClientId(uuid);
            return data;
        }

        [HttpGet]
        [Route("GetSheet/{pid}/{subjectCode}/{clientid}")]
        public ExamSheetRespone GetSheet(string pid, string subjectCode, string clientid)
        {
            var regis = repoOnSite.GetTestRegisInfo(pid, subjectCode);
            if (regis.MaxCount <= regis.LatestCount)
            {
                return null;
            }

            ExamSheet sheet = new ExamSheet();

            //Get available exam sheet if haven't availble return null
            if (repoOnSite.GetSheetBySubjectCodeForMap(subjectCode, regis.ExamLanguage) == null)
            {
                return new ExamSheetRespone { Message = new ViewModels.MessageRespone { Code = "1", Message = string.Format(getsheet1, subjectCode, regis.ExamLanguage).ToString() } };
            }

            var oldsheet = repoOnSite.GetSheetByPIDAndSubjectCode(pid, subjectCode, regis.ExamLanguage);
            if (oldsheet.ClientId == null)
            {
                sheet = repoOnSite.GetSheetBySubjectCodeForMap(subjectCode, regis.ExamLanguage);
                sheet.RandomQuestions = new List<Question>(GetQuestion(sheet.RandomQuestions));
            }
            else
            {
                if (oldsheet.LatestStatus == "FAIL")
                {
                    sheet = repoOnSite.GetSheetBySubjectCodeForMap(subjectCode, regis.ExamLanguage);
                }
                else if (oldsheet.LatestStatus == "RESUME")
                {
                    sheet = oldsheet;
                }
                else if (oldsheet.LatestStatus != "RESUME")
                {
                    return new ExamSheetRespone { Message = new ViewModels.MessageRespone { Code = "2", Message = getsheet2 } };
                }
                else if (oldsheet.LatestStatus == "PASS")
                {
                    return new ExamSheetRespone { Message = new ViewModels.MessageRespone { Code = "3", Message = getsheet3 } };

                }
            }

            if (sheet.LatestStatus != "RESUME")
            {
                int testCount = regis.LatestCount + 1;

                sheet.TestCount = testCount;
                regis.LatestCount = testCount;
            }

            sheet.TestRegisID = regis._id;
            sheet.PID = regis.PID;

            var newStatus = new StatusExtension
            {
                _id = Guid.NewGuid().ToString(),
                ClientId = clientid,
                CreateDateTime = DateTime.Now,
                Status = "Ready",
            };

            if (sheet.StatusExtensions == null)
            {
                sheet.StatusExtensions = new List<StatusExtension>();
                sheet.StatusExtensions.Add(newStatus);
            }
            else
            {
                sheet.StatusExtensions.Add(newStatus);
            }

            sheet.ClientId = clientid;
            sheet.LatestStatus = "Ready";

            repoOnSite.UpdateSheet(sheet);
            repoOnSite.UpdateTestRegis(regis);

            var activeInfo = repoOnSite.GetLastActive();

            var result = new ExamSheetRespone
            {
                _id = sheet._id,
                Title = regis.Title,
                FirstName = regis.FirstName,
                LastName = regis.LastName,
                PID = regis.PID,
                ExamNumber = regis.ExamNumber,
                SubjectCode = sheet.Subject.SubjectCode,
                SubjectName = sheet.Subject.SubjectName,
                IsEReadiness = sheet.Subject.IsEReadiness,
                ContentLanguage = sheet.Subject.ContentLanguage,
                TestCount = sheet.TestCount,
                LastedStatus = sheet.LatestStatus,
                PassScore = sheet.Subject.PassScore,
                CorrectScore = 0,
                InCorrectScore = 0,
                ExamDuration = sheet.Subject.ExamDuration,
                ReviewDuration = sheet.ReviewDuration,
                ActiveThruDateTime = activeInfo.ActiveThruDateTime,
                ClientId = clientid,
                Questions = sheet.RandomQuestions.Select(q => new Question
                {
                    _id = q._id,
                    QuestionNumber = q.QuestionNumber,
                    IsAllowRandomChoice = q.IsAllowRandomChoice,
                    Detail = q.Detail,
                    GroupId = q.GroupId,
                    //UserAnswer = q.UserAnswer == null ? new Choice() :
                    //new Choice
                    //{
                    //    _id = q.UserAnswer._id,
                    //    Detail = q.UserAnswer.Detail,
                    //    IsCorrect = q.UserAnswer.IsCorrect.Value,
                    //},
                    UserAnswer = q.UserAnswer == null ? null :
                    new Choice
                    {
                        _id = q.UserAnswer._id,
                        Detail = q.UserAnswer.Detail,
                        IsCorrect = q.UserAnswer.IsCorrect.Value,
                    },

                    Choices = q.Choices.Select(c => new Choice
                    {
                        _id = c._id,
                        Detail = c.Detail,
                        IsCorrect = null,
                    }).ToList(),
                }).ToList(),
                Message = new MessageRespone { Code = "0", Message = getsheet0 }
            };

            return result;
        }

        [HttpPost]
        [Route("Answer")]
        public MessageRespone Answer([FromBody] AnswerRequest answerRequest)
        {
            var sheet = repoOnSite.GetSheetBySheetId(answerRequest.ExamSheetId);

            //Normal
            if (sheet != null && sheet.LatestStatus != "RESUME" && answerRequest.ClientId == sheet.ClientId)
            {
                var question = sheet.RandomQuestions.Where(q => q._id == answerRequest.QID).FirstOrDefault();

                question.UserAnswer = question.Choices.Where(c => c._id == answerRequest.ChoiceId).FirstOrDefault();

                repoOnSite.UpdateSheet(sheet);

                return new MessageRespone { Code = "SUCCESS", Message = "ตอบสำเร็จ" };
            }
            //Resume
            else if (sheet != null && sheet.LatestStatus == "RESUME")
            {
                var question = sheet.RandomQuestions.Where(q => q._id == answerRequest.QID).FirstOrDefault();

                question.UserAnswer = question.Choices.Where(c => c._id == answerRequest.ChoiceId).FirstOrDefault();

                //override client id
                sheet.ClientId = answerRequest.ClientId;

                repoOnSite.UpdateSheet(sheet);

                return new MessageRespone { Code = "SUCCESS", Message = "ตอบสำเร็จ" };
            }
            else
            {
                return new MessageRespone { Code = "DUPLICATE", Message = "ซ้ำซ้อน" };
            }
        }

        [HttpGet]
        [Route("SendExam/{sheetId}/{clientid}")]
        public ResultRespone SendExam(string sheetId, string clientid)
        {
            var sheet = repoOnSite.GetSheetBySheetId(sheetId);

            if (sheet != null)
            {
                foreach (var item in sheet.RandomQuestions)
                {
                    if (item.UserAnswer != null)
                    {
                        item.UserAnswer.IsCorrect = item.Choices.Where(x => x._id == item.UserAnswer._id).FirstOrDefault().IsCorrect;
                    }
                }

                //sheet.CorrectScore = sheet.RandomQuestions.Where(x => x.UserAnswer.IsCorrect.Value).Count();
                sheet.CorrectScore = sheet.RandomQuestions.Where(x => x.UserAnswer != null && x.UserAnswer.IsCorrect.Value).Count();
                //sheet.InCorrectScore = sheet.RandomQuestions.Where(x => !x.UserAnswer.IsCorrect.Value).Count();
                sheet.InCorrectScore = sheet.RandomQuestions.Count() - sheet.CorrectScore;

                var newStatus = new StatusExtension
                {
                    _id = Guid.NewGuid().ToString(),
                    ClientId = clientid,
                    CreateDateTime = DateTime.Now,
                    Status = "DONE"
                };
                sheet.StatusExtensions.Add(newStatus);
                sheet.LatestStatus = newStatus.Status;
                sheet.DoneDateTime = DateTime.Now;
                repoOnSite.UpdateSheet(sheet);

                var testregis = repoOnSite.GetTestRegisById(sheet.TestRegisID);


                ResultRespone result = new ResultRespone()
                {
                    //IsShowAnswer = repoOnSite.GetCenter().IsShowAnswer,
                    IsShowAnswer = true,
                    ExamSheetRespones = new ExamSheetRespone()
                    {
                        _id = sheet._id,
                        Title = testregis.Title,
                        FirstName = testregis.FirstName,
                        LastName = testregis.LastName,
                        PID = testregis.PID,
                        ExamNumber = testregis.ExamNumber,
                        SubjectCode = sheet.Subject.SubjectCode,
                        SubjectName = sheet.Subject.SubjectName,
                        IsEReadiness = sheet.Subject.IsEReadiness,
                        ContentLanguage = sheet.Subject.ContentLanguage,
                        TestCount = sheet.TestCount,
                        LastedStatus = sheet.LatestStatus,
                        PassScore = sheet.Subject.PassScore,
                        CorrectScore = sheet.CorrectScore,
                        InCorrectScore = sheet.InCorrectScore,
                        ExamDuration = sheet.Subject.ExamDuration,
                        ReviewDuration = sheet.ReviewDuration,
                        ActiveThruDateTime = sheet.ActiveThruDateTime.Value,
                        ClientId = clientid,
                        Questions = sheet.RandomQuestions.Select(q => new Question
                        {
                            _id = q._id,
                            QuestionNumber = q.QuestionNumber,
                            IsAllowRandomChoice = q.IsAllowRandomChoice,
                            Detail = q.Detail,
                            GroupId = q.GroupId,
                            //UserAnswer = q.UserAnswer == null ? new Choice() :
                            //new Choice
                            //{
                            //    _id = q.UserAnswer._id,
                            //    Detail = q.UserAnswer.Detail,
                            //    IsCorrect = q.UserAnswer.IsCorrect.Value,
                            //},
                            UserAnswer = q.UserAnswer == null ? null :
                            new Choice
                            {
                                _id = q.UserAnswer._id,
                                Detail = q.UserAnswer.Detail,
                                IsCorrect = q.UserAnswer.IsCorrect.Value,
                            },
                            Choices = q.Choices.Select(c => new Choice
                            {
                                _id = c._id,
                                Detail = c.Detail,
                                IsCorrect = c.IsCorrect,
                            }).ToList(),
                        }).ToList(),
                    },
                };

                return result;

            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [Route("StartExam")]
        public void StartExam([FromBody]ClientSheetRequest clientSheetRequest)
        {
            var sheet = repoOnSite.GetSheetBySheetId(clientSheetRequest.SheetId);

            if (sheet != null)
            {
                var newStatus = new StatusExtension
                {
                    _id = Guid.NewGuid().ToString(),
                    ClientId = clientSheetRequest.ClientId,
                    CreateDateTime = DateTime.Now,
                    Status = "TESTING",
                };

                var active = repoOnSite.GetLastActive().ActiveThruDateTime;
                if (active >= DateTime.Now.AddMinutes(sheet.Subject.ExamDuration))
                {
                    sheet.ActiveThruDateTime = DateTime.Now.AddMinutes(sheet.Subject.ExamDuration);
                }
                else
                {
                    sheet.ActiveThruDateTime = active;
                }
                sheet.StatusExtensions.Add(newStatus);
                sheet.LatestStatus = "TESTING";
                sheet.StartDateTime = DateTime.Now;

                repoOnSite.UpdateSheet(sheet);
            }
        }

        [HttpPost]
        [Route("EndExam")]
        public void EndExam([FromBody]ClientSheetRequest clientSheetRequest)
        {
            var sheet = repoOnSite.GetSheetBySheetId(clientSheetRequest.SheetId);
            var newStatus = new StatusExtension
            {
                _id = Guid.NewGuid().ToString(),
                ClientId = clientSheetRequest.ClientId,
                CreateDateTime = DateTime.Now,
                Status = sheet.CorrectScore >= sheet.Subject.PassScore ? "PASS" : "FAIL",
            };

            sheet.StatusExtensions.Add(newStatus);
            sheet.LatestStatus = newStatus.Status;
            sheet.ExamDateTime = DateTime.Now;

            repoOnSite.UpdateSheet(sheet);

            //Update testreg status by sirinarin
            //var testreg = repoOnSite.GetTestRegisById(sheet.TestRegisID);
            //testreg.Status = newStatus.Status;
            //repoOnSite.UpdateTestRegis(testreg);

            if (sheet.LatestStatus == "PASS")
            {
                SyncExam(sheet);
            }
        }


        public IEnumerable<Question> GetQuestion(IEnumerable<Question> questions)
        {
            if (questions != null)
            {
                foreach (var q in questions)
                {
                    if (q.Assets == null || !q.Assets.Any())
                    {
                        continue;
                    }

                    var assets = (from it in q.Assets
                                  select new Asset
                                  {
                                      Resource = Combine(this.localIP + q.ExamCode, it.Resource), // TODO Add IP:local
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
            }
            else { return new List<Question>(); }
            return questions.OrderBy(x => x.QuestionNumber);
        }

        private string Combine(string dir, string path)
        {
            dir = dir ?? string.Empty;
            path = path ?? string.Empty;

            dir = dir.TrimEnd('/').Trim();
            path = path.TrimStart('/').Trim();

            var combined = string.Format("{0}/{1}", dir, path);
            return combined.TrimStart('/');
        }

        private void SyncExam(ExamSheet mysheet)
        {
            //var mysheet = repoOnSite.GetSheetBySheetId(sheetid);
            var myregis = repoOnSite.GetTestRegisById(mysheet.TestRegisID);
            if (mysheet.LatestStatus == "PASS")
            {
                mysheet.IsSync = true;
                myregis.IsSync = true;
            }

            List<ExamSheet> sheetList = new List<ExamSheet>();
            sheetList.Add(mysheet);

            List<ExamSheetOnline> sheetForOnline = new List<ExamSheetOnline>();

            var listReg = repoOnSite.GetTestRegisByIds(sheetList.Select(x => x.TestRegisID).ToList());

            foreach (var item in sheetList)
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

            CloseExamRequest request = new CloseExamRequest()
            {
                ResultSheet = sheetForOnline,
            };

            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    //dataByte = client.UploadData("http://localhost:10585/api/ExamSheet/UpdateExamSheetFromOnSite/", "POST", dataByte);
                    dataByte = client.UploadData("http://eexamthaiex.azurewebsites.net/api/ExamSheet/UpdateExamSheetFromOnSite/", "POST", dataByte);

                    //update sheet
                    repoOnSite.UpdateSheet(mysheet);
                    //update Regis
                    repoOnSite.UpdateTestRegis(myregis);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }
    }
}
