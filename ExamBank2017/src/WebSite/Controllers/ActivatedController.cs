using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSite.Repositories;
using WebSite.ViewModels.ExamBankModels;
using ActivatedSubject = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace WebSite.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    [Route("api/[controller]")]
    public class ActivatedController : Controller
    {
        private CloudStorageAccount storageAccount;
        private IExamForApproveRepository repoForApprove;
        private IExamForRandomRepository repoForRandom;

        public ActivatedController(IExamForApproveRepository repoForApprove, CloudStorageAccount storageAccount, IExamForRandomRepository repoForRandom)
        {
            this.repoForApprove = repoForApprove;
            this.repoForRandom = repoForRandom;
            this.storageAccount = storageAccount;
        }

        // HttpGet: api/Activated/ListSubject  
        [EnableCors("AllowAllOrigins")]
        [HttpGet]
        [Route("ListSubject/{siteId}")]
        public SubjectList ListSubject(string siteId)
        {
            //HACK: mock user login
            //var siteId = "01";
            //var siteId = siteId;

            //ListActivedSubject
            var _activateds = repoForApprove.ListActivatedSubject(siteId);

            //ListSubject
            var _subjects = repoForApprove.ListSubject(_activateds.Select(aSub => aSub.SubjectId)?.ToList());

            //ListOccupationGroup
            var _occupationGroups = repoForApprove.ListOccupationGroup(siteId);

            var subjectGroupsBack = _occupationGroups?.SelectMany(occ => occ.SubjectGroups);
            var subjectGroupsFront = _occupationGroups?.SelectMany(occ => occ.SubjectGroups?.Select(sg => new SubjectGroup
            {
                id = sg._id,
                Name = sg.Name,
                OccupationId = occ._id,
            }));

            //ConvertTo SubjectList  
            var subjectList = new SubjectList()
            {
                SubjList = _activateds.Select(aSub =>
                {
                    var _subject = _subjects.FirstOrDefault(sub => sub.SubjectCode == aSub.SubjectCode && sub.ContentLanguage == aSub.ContentLanguage);
                    return new SubjectDetail
                    {
                        id = aSub.SubjectId,
                        SubjectCode = aSub.SubjectCode,
                        SubjectName = _subject?.SubjectName,
                        ContentLanguage = _subject?.ContentLanguage,
                        ExamSuiteCount = _subject?.ExamSuites?.Count() ?? 0,
                        QuestionCount = _subject?.ExamSuites?.SelectMany(es => es.QuestionIds ?? new List<string>())?.Count() ?? 0,
                        Version = _subject?.Version,
                        IsDisabled = aSub?.DisabledDateTime.HasValue ?? false,
                        //SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.Subjects?.Any(s => s.Code == aSub.SubjectCode) ?? false)?._id,
                        SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.SubjectCodes?.Any(scode => scode == aSub.SubjectCode) ?? false)?._id,
                    };
                }),

                Occupations = _occupationGroups?.Select(occ => new Occupation
                {
                    id = occ._id,
                    Name = occ.Name,
                }),
                SubjectGroups = subjectGroupsFront,
            };
            var lang = _activateds.Select(x => new LanguageSource { SubjectCode = x.SubjectCode, Detail = x.ContentText, Code = x.ContentLanguage });
            List<VoiceSource> vs = new List<VoiceSource>();
            foreach (var item in _subjects)
            {
                foreach (var vlang in item.VoiceLanguages)
                {
                    vs.Add(new VoiceSource { SubjectCode = item.SubjectCode, Code = vlang.LanguageCode, Detail = vlang.Language });
                }
            }
            subjectList.LanguageSources = lang;
            subjectList.VoiceSources = vs;
            return subjectList;


            ////ConvertTo SubjectList  
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
            //        SubjList = _activateds.Select(aSub =>
            //        {
            //            i++;
            //            var _subject = _subjects.FirstOrDefault(sub => sub.SubjectCode == aSub.SubjectCode && sub.ContentLanguage == aSub.ContentLanguage);
            //            return new SubjectDetail
            //            {
            //            //id = aSub.SubjectId,
            //            //SubjectCode = aSub.SubjectCode,
            //            ////SubjectName = _subject?.SubjectName + "  " + dt.Rows[i]["name"].ToString(),
            //            //SubjectName = dt.Rows[i]["name"].ToString(),
            //            //ContentLanguage = _subject?.ContentLanguage,
            //            //ExamSuiteCount = _subject?.ExamSuites?.Count() ?? 0,
            //            //QuestionCount = _subject?.ExamSuites?.SelectMany(es => es.QuestionIds ?? new List<string>())?.Count() ?? 0,
            //            //Version = _subject?.Version,
            //            //IsDisabled = aSub?.DisabledDateTime.HasValue ?? false,
            //            ////SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.Subjects?.Any(s => s.Code == aSub.SubjectCode) ?? false)?._id,
            //            //SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.SubjectCodes?.Any(scode => scode == aSub.SubjectCode) ?? false)?._id,

            //            id = aSub.SubjectId,
            //                SubjectCode = dt.Rows[i]["licensetype"].ToString(),
            //                SubjectName = dt.Rows[i]["name"].ToString(),
            //                ContentLanguage = dt.Rows[i]["language"].ToString(),
            //                ExamSuiteCount = Int32.Parse(dt.Rows[i]["quantity"].ToString()),
            //                QuestionCount = Int32.Parse(dt.Rows[i]["questioncount"].ToString()),
            //                Version = dt.Rows[i]["version"].ToString(),
            //                IsDisabled = aSub?.DisabledDateTime.HasValue ?? false,
            //            //SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.Subjects?.Any(s => s.Code == aSub.SubjectCode) ?? false)?._id,
            //            SubjectGroupId = subjectGroupsBack?.FirstOrDefault(sg => sg.SubjectCodes?.Any(scode => scode == aSub.SubjectCode) ?? false)?._id,

            //            };
            //        }),

            //        Occupations = _occupationGroups?.Select(occ => new Occupation
            //        {
            //            id = occ._id,
            //            Name = occ.Name,
            //        }),
            //        SubjectGroups = subjectGroupsFront,
            //    };
            //    var lang = _activateds.Select(x => new LanguageSource { SubjectCode = x.SubjectCode, Detail = x.ContentText, Code = x.ContentLanguage });
            //    List<VoiceSource> vs = new List<VoiceSource>();
            //    foreach (var item in _subjects)
            //    {
            //        foreach (var vlang in item.VoiceLanguages)
            //        {
            //            vs.Add(new VoiceSource { SubjectCode = item.SubjectCode, Code = vlang.LanguageCode, Detail = vlang.Language });
            //        }
            //    }
            //    subjectList.LanguageSources = lang;
            //    subjectList.VoiceSources = vs;

            //    return subjectList;
            //}
        }

        // HttpGet: api/Activated/GetSubject/{SubjectId}   
        [HttpGet]
        [Route("GetSubject/{SubjectId}")]
        public Subject GetSubject(string SubjectId)
        {
            int i = 0;
            var siteId = HomeController._centerdata.SiteId;

            var _subject = repoForApprove.GetSubject(SubjectId);
            var _subjects = repoForApprove.ListSubjectBySubjectCode(_subject?.SubjectCode ?? string.Empty, _subject.ContentLanguage);
            var _activated = repoForApprove.GetActivatedSubjectBySubjectCode(_subject.SubjectCode, _subject.ContentLanguage);

            var _qid = _subject?.ExamSuites?.SelectMany(es => es?.QuestionIds)?.ToList();


            string conString = "User Id=c##dsd;Password=1q2w3e4r;" +
                               "Data Source=localhost:1521/db;";
            DataTable dt_subject = new DataTable();
            DataTable dt_subjects = new DataTable();
            DataTable dt_activated = new DataTable();

            DataSet ds_subject = new DataSet();
            DataSet ds_subjects = new DataSet();
            DataSet ds_activated = new DataSet();

            
            using (OracleConnection objConn = new OracleConnection(conString))
            {
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = objConn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "spGetSubject";
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter oraP = new OracleParameter();
                oraP.OracleDbType = OracleDbType.RefCursor;
                oraP.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("SubjectId", OracleDbType.NVarchar2).Value = "14";
                cmd.Parameters.Add(oraP);
                
                da.SelectCommand = cmd;

                da.Fill(ds_subject);
                dt_subject = ds_subject.Tables[0];
            }

            using (OracleConnection objConn = new OracleConnection(conString))
            {
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = objConn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "spGetSubjectList";
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter oraP = new OracleParameter();
                oraP.OracleDbType = OracleDbType.RefCursor;
                oraP.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(oraP);

                da.SelectCommand = cmd;

                da.Fill(ds_subjects);
                dt_subjects = ds_subjects.Tables[0];
            }


            using (OracleConnection objConn = new OracleConnection(conString))
            {
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = objConn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "spGetSubject";
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter oraP = new OracleParameter();
                oraP.OracleDbType = OracleDbType.RefCursor;
                oraP.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("SubjectId", OracleDbType.NVarchar2).Value = "14";
                cmd.Parameters.Add(oraP);

                da.SelectCommand = cmd;

                da.Fill(ds_subject);
                dt_subject = ds_subject.Tables[0];
            }



            //listAllQuestion
            //var _questions = repoForApprove.ListAllQuestionByQID(_qid ?? new List<string>(),);

            //ListActivedSubject
            var _activateds = repoForApprove.ListActivatedSubject(siteId);
            i++;
            var subject = new Subject
            {

                id = _subject?._id,
                SubjectCode = _subject?.SubjectCode ,
                SubjectName = _subject?.SubjectName,
                ContentLanguage = _subject?.ContentLanguage,
                Version = _subject?.Version,
                IsDisabled = _activated?.DisabledDateTime.HasValue ?? false,
                ExamSuites = _subject?.ExamSuites?.Select(es => new ExamSuiteDetail
                {
                    id = es._id,
                    TitleCode = es.TitleCode,
                    TitleName = es.TitleName,
                    QuestionCount = es.QuestionIds?.Count() ?? 0,
                    ConsiderationStatus = string.Empty,
                }),
                ExamSuiteGroups = _subject?.ExamSuiteGroups?.Select(esg => new ExamSuiteGroup
                {
                    id = esg._id,
                    SubjectId = _subject?._id,
                    ExamSuiteGroupName = esg.ExamSuiteGroupName,
                    IsUsed = esg.IsUsed,
                    PassScore = esg.PassScore ?? 0,
                    ExamDuration = esg.ExamDuration ?? 0,
                    QuestionCount = 0,
                    ExamSuiteGroupMaps = esg.ExamSuiteGroupMaps?.Select(esgm => new ExamSuiteGroupMap
                    {
                        id = esgm._id,
                        ExamSuiteId = esgm.ExamSuiteId,
                        RandomCount = esgm.RandomCount,
                        ExamSuiteGroupId = esg._id,
                    }),

                }),
                VersionList = _subjects?.Select(sub => new SubjectVersion
                {
                    id = sub._id,
                    CreateDateTime = sub.CreateDateTime,
                    IsUsed = _activateds?.Any(aSub => aSub._id == sub._id) ?? false,
                    SubjectId = sub._id,
                    VersionText =  sub.Version,
                }),
                VoiceLanguageList = _subject?.VoiceLanguages.Select(vl => new VoiceLanguage
                {
                    id = vl._id,
                    IsUsed = vl.IsUsed,
                    Language = vl.Language,
                    LanguageCode = vl.LanguageCode
                }),
            };

            return subject;
        }

        // HttpGet: api/Activated/GetExamsuite/{SubjectId}/{ExamSuiteId}  
        [HttpGet]
        [Route("GetExamsuite/{SubjectId}/{ExamSuiteId}")]
        public ExamSuite GetExamsuite(string SubjectId, string ExamSuiteId)
        {
            var _subject = repoForApprove.GetSubject(SubjectId);

            var _examSuite = _subject?.ExamSuites?.FirstOrDefault(es => es._id == ExamSuiteId);

            //listAllQuestion
            var _questions = repoForApprove.ListAllQuestionByQID(_examSuite?.QuestionIds ?? new List<string>(), _examSuite.TitleCode, _subject.ContentLanguage);

            //Move to Repo's job
            //_questions = this.GetQuestion(_questions, ExamSuiteId); 

            var examSuite = new ExamSuite
            {
                id = _examSuite?._id,
                SubjectCode = _subject?.SubjectCode,
                SubjectName = _subject?.SubjectName,
                TitleCode = _examSuite?.TitleCode,
                TitleName = _examSuite?.TitleName,
                SubjectId = _subject?._id,
                Questions = _questions?.Select(q => new Question
                {
                    id = q._id,
                    QuestionNumber = q.QuestionNumber,
                    IsAllowRandomChoice = q.IsAllowRandomChoice,
                    Detail = q.Detail,
                    Choices = q.Choices?.Select(c => new Choice
                    {
                        id = c._id,
                        Detail = c.Detail,
                        Voices = null, // TODO : Add Voice
                        IsCorrect = c.IsCorrect,
                    }),
                    Considerations = null,
                    Voices = null, // TODO : Add Voice
                    GroupId = q.GroupId,
                    ExamSuiteId = _examSuite?._id,
                }),
            };
            return examSuite;
        }

        // HttpGet: api/ConfigurationActive/RandomExamSheet/{ExamSuiteId}    
        [HttpGet]
        [Route("RandomExamSheet/{CenterId}/{SubjectCode}/{ContentLanguage}/{Quantity}")]
        public List<ActivatedSubject.ExamSheet> RandomExamSheet(string centerId, string subjectCode, string contentLanguage, int quantity)
        {
            var activatedSubject = repoForRandom.GetActivatedSubjectBySubjectCode(subjectCode, contentLanguage);
            if (activatedSubject != null)
            {
                var isUsedSubject = repoForRandom.GetInUseSubjectBySubjectId(activatedSubject.SubjectId);
                isUsedSubject.ContentLanguage = activatedSubject.ContentLanguage;

                //Map all QIds in Subject for use as question pool to random in next section
                List<ActivatedSubject.ExamSuiteWithQuestion> questionPool = repoForRandom.MapQuestion(isUsedSubject.ExamSuites);


                ////// Move to AdminOnline Front job ////////

                //Check local ExamSheet if local version lower than lastest version (match version?), delete local ExamSheet and increase download amount            
                //string URL = string.Format("http://10.93.77.199/localdb/api/shared/CheckSubjectVersion/{0}/{1}/{2}", centerId, subjectCode, isUsedSubject.Version);
                ////string URL = string.Format("http://localhost:59684/api/shared/CheckSubjectVersion/{0}/{1}/{2}", centerId, subjectCode, isUsedSubject.Version);
                //using (var client = new WebClient())
                //{
                //    try
                //    {
                //        client.Encoding = Encoding.UTF8;
                //        var responseString = client.DownloadString(URL);
                //        var msgResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WebSite.ViewModels.ExamBankModelsBack.MessageRespone>(responseString);

                //        var extraSheetCount = Convert.ToInt32(msgResponse.Code);
                //        if (extraSheetCount > 0)
                //        {
                //            quantity += extraSheetCount;
                //        }

                //    }
                //    catch (Exception)
                //    {
                //        throw;
                //    }
                //}


                //Random group for each sheet then random question
                List<ActivatedSubject.ExamSheet> preSheet = new List<ActivatedSubject.ExamSheet>();
                for (int i = 1; i <= quantity; i++)
                {
                    int qOrder = 1;
                    var data = RandomData<ActivatedSubject.ExamSuiteGroup>(isUsedSubject.ExamSuiteGroups).FirstOrDefault();

                    //random question each examsuite
                    List<ActivatedSubject.Question> myQ = new List<ActivatedSubject.Question>();
                    foreach (var groupmap in data.ExamSuiteGroupMaps)
                    {
                        //take data in question pool by examsuiteid
                        var questionGroupmap = questionPool.Where(x => x._id == groupmap.ExamSuiteId).FirstOrDefault();
                        var randomQ = RandomQuestion(questionGroupmap, groupmap.RandomCount);

                        myQ.AddRange(randomQ);
                    }
                    foreach (var item in myQ)
                    {
                        item.QuestionNumber = qOrder++;
                        if (item.IsAllowRandomChoice)
                        {
                            item.Choices = RandomData<ActivatedSubject.Choice>(item.Choices);
                        }
                    }
                    preSheet.Add(new ActivatedSubject.ExamSheet
                    {
                        _id = Guid.NewGuid().ToString(),
                        Subject = new ViewModels.ExamBankModelsBack.SubjectResponse
                        {
                            _id = isUsedSubject._id,
                            SubjectCode = isUsedSubject.SubjectCode,
                            SubjectName = isUsedSubject.SubjectName,
                            Version = isUsedSubject.Version,
                            //VoiceLanguages = isUsedSubject.VoiceLanguages,
                            //CreateDateTime = isUsedSubject.CreateDateTime,
                            ContentLanguage = isUsedSubject.ContentLanguage,
                            PassScore = data.PassScore.HasValue ? data.PassScore.Value : 0,
                            ExamDuration = data.ExamDuration.HasValue ? data.ExamDuration.Value : 999,
                            IsEReadiness = activatedSubject.IsEReadiness,
                        },
                        RandomQuestions = myQ.Select(xx => new ActivatedSubject.Question
                        {
                            _id = xx._id,
                            QuestionNumber = xx.QuestionNumber,
                            Detail = xx.Detail,
                            Assets = xx.Assets,
                            Choices = xx.Choices,
                            IsAllowRandomChoice = xx.IsAllowRandomChoice,
                            GroupId = xx.GroupId,
                            CreateDateTime = xx.CreateDateTime,
                            ExamCode = xx.ExamCode
                        }).ToList(),
                        CenterId = centerId,
                        CreateDate = DateTime.Now,
                        ReviewDuration = 999, //HACK get it from site info
                    });
                }
                //return Newtonsoft.Json.JsonConvert.SerializeObject(preSheet);

                ////AdminOnline Job
                //this.repoForRandom.CreatePreExamSheet(preSheet);
                //Repositories.MongoImpl.SharedRepository repo = new Repositories.MongoImpl.SharedRepository();
                //repo.CreateExamSheet(preSheet);

                return preSheet;
            }
            else { return new List<ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSheet>(); }
        }

        private List<ActivatedSubject.Question> RandomQuestion(ActivatedSubject.ExamSuiteWithQuestion data, int randomCount)
        {
            seed = 0;
            List<ActivatedSubject.Question> QQ = new List<ViewModels.ExamBankModelsBack.ActivatedSubject.Question>();

            int remainQ = randomCount;
            var shuffleQ = this.RandomData<ActivatedSubject.QuestionPool>(data.Questions);
            for (int i = 0; i < randomCount; i++)
            {
                var pickQ = shuffleQ.FirstOrDefault();
                if (pickQ.GroupCount <= remainQ)
                {
                    if (pickQ.GroupCount > 1)
                    {
                        var grouped = shuffleQ.Where(x => x.GroupId == pickQ.GroupId).OrderBy(x => x.QuestionNumber);
                        QQ.AddRange(grouped);

                        foreach (var qg in grouped)
                        {
                            shuffleQ.Remove(shuffleQ.Where(x => x._id == qg._id).FirstOrDefault());
                        }
                    }
                    else
                    {
                        QQ.Add(pickQ);

                        shuffleQ.Remove(shuffleQ.Where(x => x._id == pickQ._id).FirstOrDefault());
                    }

                    remainQ -= pickQ.GroupCount;
                }
            }

            return QQ;
        }
        public int seed;
        private List<T> RandomData<T>(List<T> data)
        {
            Random _random = new Random(seed);

            List<KeyValuePair<int, T>> list = new List<KeyValuePair<int, T>>();

            foreach (var s in data)
            {
                list.Add(new KeyValuePair<int, T>(_random.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;

            // Allocate new string array
            List<T> result = new List<T>();

            // Copy values to array
            //int index = 0;
            foreach (var pair in sorted)
            {
                result.Add(pair.Value);
                //index++;
            }
            seed++;
            return result.ToList();
        }
    }
}


