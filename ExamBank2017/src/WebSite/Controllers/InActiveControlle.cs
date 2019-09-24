using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Repositories;
using WebSite.Repositories.MongoImpl;
using WebSite.ViewModels.ExamBankModels;

namespace WebSite.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    [Route("api/[controller]")]
    public class InActiveController : Controller
    {
        private CloudStorageAccount storageAccount;
        private IExamForApproveRepository repoForApprove;
        private IQuestionImportRepository repoQ;
        private MongoHelper helper;

        public InActiveController(MongoHelper helper, IExamForApproveRepository repoForApprove, IQuestionImportRepository repoQ, CloudStorageAccount storageAccount)
        {
            this.repoForApprove = repoForApprove;
            this.repoQ = repoQ;
            this.storageAccount = storageAccount;
            this.helper = helper;
        }

        [HttpGet]
        [Route("ListSubject")]
        public SubjectList ListSubject()
        {
            var siteId = HomeController._centerdata.SiteId;

            //ListActivedSubject
            var _activateds = repoForApprove.ListInActiveSubject(siteId);

            var _occupationGroups = repoForApprove.ListOccupationGroup(siteId);

            //var _allExamSuites = repoQ.GetAllQuestionSuiteBySubjectIds(_activateds.Select(allEs => allEs._id));

            var subjectGroupsBack = _occupationGroups?.SelectMany(occ => occ.SubjectGroups);
            var subjectGroupsFront = _occupationGroups?.SelectMany(occ => occ.SubjectGroups?.Select(sg => new SubjectGroup
            {
                id = sg._id,
                Name = sg.Name,
                OccupationId = occ._id,
            }));

            var subjectList = new SubjectList()
            {
                SubjList = _activateds?.Select(iSub =>
                {
                    //var _examSuites = _allExamSuites.Where(allEs => allEs.SubjectId == iSub._id);
                    return new SubjectDetail
                    {
                        id = iSub._id,
                        SubjectCode = iSub.SubjectCode,
                        SubjectName = iSub.SubjectName,
                        ContentLanguage = iSub.ContentLanguage,
                        //ExamSuiteCount = _examSuites?.Count() ?? 0,
                        //QuestionCount = _examSuites?.Sum(es => es.Questions?.Count() ?? 0) ?? 0,
                        ExamSuiteCount = iSub.ExamSuiteCount,
                        QuestionCount = iSub.QuestionCount,
                        ExamSuiteAcceptCount = iSub.ExamSuiteAcceptCount,
                        ExamSuiteRejectCount = iSub.ExamSuiteRejectCount,
                        Version = string.Empty,
                        IsDisabled = false,
                        SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.SubjectCodes?.Any(scode => scode == iSub.SubjectCode) ?? false)?._id,
                    };
                }),
                Occupations = _occupationGroups?.Select(occ => new Occupation
                {
                    id = occ._id,
                    Name = occ.Name,
                }),
                SubjectGroups = subjectGroupsFront,
            };

            return subjectList;


            //ConvertTo SubjectList  
            //string conString = "User Id=c##dsd;Password=1q2w3e4r;" +
            //                   "Data Source=localhost:1521/db;";
            //DataTable dt = new DataTable();
            //DataSet ds = new DataSet();
            //OracleCommand cmd = new OracleCommand();
            //using (OracleConnection objConn = new OracleConnection(conString))
            //{
            //    OracleDataAdapter da = new OracleDataAdapter();
            //    cmd.Connection = objConn;
            //    cmd.InitialLONGFetchSize = 1000;
            //    cmd.CommandText = "spGetSubjectList";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    OracleParameter oraP = new OracleParameter();
            //    oraP.OracleDbType = OracleDbType.RefCursor;
            //    oraP.Direction = System.Data.ParameterDirection.Output;
            //    cmd.Parameters.Add(oraP);

            //    da.SelectCommand = cmd;

            //    da.Fill(ds);
            //    dt = ds.Tables[0];

            //    int i = 0;
            //    var subjectList = new SubjectList()
            //    {
            //        SubjList = _activateds?.Select(iSub =>
            //        {
            //            i++;
            //            var _examSuites = _allExamSuites.Where(allEs => allEs.SubjectId == iSub._id);
            //            return new SubjectDetail
            //            {
            //                    id = iSub._id,
            //                    SubjectCode = iSub.SubjectCode,
            //                    SubjectName = iSub.SubjectName,
            //                    ContentLanguage = iSub.ContentLanguage,
            //                //ExamSuiteCount = _examSuites?.Count() ?? 0,
            //                //QuestionCount = _examSuites?.Sum(es => es.Questions?.Count() ?? 0) ?? 0,
            //                ExamSuiteCount = iSub.ExamSuiteCount,
            //                    QuestionCount = iSub.QuestionCount,
            //                ExamSuiteAcceptCount = iSub.ExamSuiteAcceptCount,
            //                ExamSuiteRejectCount = iSub.ExamSuiteRejectCount,
            //                Version = string.Empty,
            //                IsDisabled = false,
            //                SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.SubjectCodes?.Any(scode => scode == iSub.SubjectCode) ?? false)?._id,



            //                id = iSub._id,
            //                SubjectCode = dt.Rows[i]["licensetype"].ToString(),
            //                SubjectName = dt.Rows[i]["name"].ToString(),
            //                ContentLanguage = dt.Rows[i]["language"].ToString(),
            //                ExamSuiteCount = Int32.Parse(dt.Rows[i]["quantity"].ToString()),
            //                QuestionCount = Int32.Parse(dt.Rows[i]["questioncount"].ToString()),
            //            };
            //        }),
            //        Occupations = _occupationGroups?.Select(occ => new Occupation
            //        {
            //            id = occ._id,
            //            Name = occ.Name,
            //        }),
            //        SubjectGroups = subjectGroupsFront,
            //    };

            //    return subjectList;
            //}
        }

        [HttpGet]
        [Route("GetSubject/{SubjectId}")]
        public Subject GetSubject(string SubjectId)
        {
            var _subject = repoForApprove.GetInActiveSubject(SubjectId);
            var _examSuites = repoQ.GetAllQuestionSuiteBySubjectId(SubjectId);

            var subject = new Subject
            {
                id = _subject?._id,
                SubjectCode = _subject?.SubjectCode,
                SubjectName = _subject?.SubjectName,
                ContentLanguage = _subject?.ContentLanguage,
                Version = string.Empty,
                IsDisabled = false,
                ExamSuites = _examSuites?.Select(es =>
                {
                    var _questionCount = es.Questions?.Count() ?? 0;
                    var _ConsiderationStatus = repoForApprove.GetConsiderationStatus(es);

                    return new ExamSuiteDetail
                    {
                        id = es._id,
                        TitleCode = es.Code,
                        TitleName = es.Title,
                        QuestionCount = _questionCount,
                        ConsiderationStatus = _ConsiderationStatus,
                    };
                }),
                ExamSuiteGroups = _subject?.ExamSuiteGroups?.Select(esg => new ExamSuiteGroup
                {
                    id = esg._id,
                    ExamSuiteGroupName = esg.ExamSuiteGroupName,
                    IsUsed = esg.IsUsed,
                    PassScore = esg.PassScore ?? 0,
                    ExamDuration = esg.ExamDuration ?? 0,
                    QuestionCount = 0,
                    ExamSuiteGroupMaps = esg.ExamSuiteGroupMaps.Select(esgm => new ExamSuiteGroupMap
                    {
                        id = esgm._id,
                        ExamSuiteId = esgm.ExamSuiteId,
                        RandomCount = esgm.RandomCount,
                        ExamSuiteGroupId = esg._id,
                        SubjectId = _subject?._id,
                    }),
                    SubjectId = _subject?._id,
                }),
                VersionList = null,
                VoiceLanguageList = null, //TODO : add VoiceLanguageList
            };

            return subject;
        }

        [HttpGet]
        [Route("GetExamsuite/{ExamSuiteId}")]
        public ExamSuite GetExamsuite(string ExamSuiteId)
        {
            var _examSuite = repoQ.GetQuestionSuite(ExamSuiteId);
            var _considerations = repoForApprove.ListConsiderationByExamSuiteId(_examSuite?._id);
            //_examSuite.Questions = this.GetQuestion(_examSuite);
            if (_examSuite.Questions != null) _examSuite.Questions = this.helper.GetQuestion(_examSuite);

            var examSuite = new ExamSuite
            {
                id = _examSuite?._id,
                SubjectCode = _examSuite?.SubjectCode,
                SubjectName = _examSuite?.SubjectName,
                TitleCode = _examSuite?.Code,
                TitleName = _examSuite?.Title,
                SubjectId = _examSuite?.SubjectId,
                Questions = _examSuite?.Questions?.Select(q => new Question
                {
                    id = q._id,
                    QuestionNumber = q.No,
                    IsAllowRandomChoice = q.NoShuffleChoice,
                    Detail = q.Content,
                    Choices = q.Choices?.Select(c => new Choice
                    {
                        id = c.Code,
                        Detail = c.Content,
                        Voices = null, // TODO : Add Voice
                        IsCorrect = c.IsCorrectAnswer,
                    }),
                    Considerations = _considerations?.Where(con => con.QuestionNumber == q.No)?.Select(c => new Consideration
                    {
                        id = c._id,
                        CreateDateTime = c.CreateDateTime,
                        IsAccept = c.IsAccept,
                        QuestionNumber = c.QuestionNumber,
                        RejectComment = c.RejectComment,
                        ExamSuiteId = _examSuite?._id,
                        UserName = c.UserName,
                    }).ToList(),
                    Voices = null, // TODO : Add Voice
                    GroupId = q.GroupId,
                    ExamSuiteId = _examSuite?._id,
                }),
            };

            return examSuite;
        }

        [HttpPost]
        [Route("CreateInactiveSubject")]
        public IActionResult CreateInactiveSubject([FromBody] SubjectDetail examSuite)
        {
            //var subjectId = $"{examSuite.SubjectCode}{examSuite.ContentLanguage}";
            var subjectId = Guid.NewGuid().ToString();

            var newSubject = new ViewModels.ExamBankModelsBack.InActiveSubject.InactiveSubject
            {
                _id = subjectId,
                SubjectCode = examSuite.SubjectCode,
                SubjectName = examSuite.SubjectName,
                CreateDateTime = DateTime.Now,
                IsEReadiness = false, //Hack
                ContentLanguage = examSuite.ContentLanguage,
                SiteId = HomeController._centerdata.SiteId,
                QuestionCount = 0,
                ExamSuiteCount = 0,
                ExamSuiteAcceptCount = 0,
                ExamSuiteRejectCount = 0,
            };
            repoQ.InsertInActiveSubject(newSubject);

            return Ok(new { Message = $"Created!", SubjectId = subjectId });
        }

        [HttpPut]
        [Route("UpdateInactiveSubject")]
        public IActionResult UpdateInactiveSubject([FromBody] SubjectDetail request)
        {
            var inActiveSubject = repoQ.GetInActiveSubject(request.id);
            repoQ.UpdateSubjectCodeAndNameAndContentLanguage(request.id, request.SubjectCode, request.SubjectName, request.ContentLanguage);

            return Ok(new { Message = $"Updated!", SubjectId = request.id });
        }

        [HttpDelete]
        [Route("DeleteInactiveSubject/{SubjectId}")]
        public IActionResult DeleteInactiveSubject(string subjectId)
        {
            repoQ.DeleteExamSuiteBySubjectId(subjectId);
            repoQ.DeleteInActiveSubject(subjectId);

            return Ok(new { Message = $"Deleted!", SubjectId = subjectId });
        }
    }
}



