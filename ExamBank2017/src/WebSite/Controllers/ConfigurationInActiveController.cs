using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Repositories;
using WebSite.ViewModels.ExamBankModels;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;
using Activated = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;
using WebSite.ViewModels.ExamBankModelsBack;
using Microsoft.WindowsAzure.Storage;

namespace WebSite.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("api/[controller]")]
    public class ConfigurationInActiveController : Controller
    {
        private IExamForApproveRepository repoForApprove;
        private IQuestionImportRepository repoQ;

        private enum VersionModifyOption { InitialVersion, IncreaseMajor, IncreaseMinor, IncreaseBuild, }

        public ConfigurationInActiveController(IExamForApproveRepository repoForApprove, IQuestionImportRepository repoQ)
        {
            this.repoForApprove = repoForApprove;
            this.repoQ = repoQ;
        }

        // HttpGet: api/ConfigurationInActive/ActivateSubject/{SubjectId}/{ActivateOption}  
        [HttpGet]
        [Route("ActivateSubject/{SubjectId}/{ActivateOption}")]
        public InactiveApiResponse ActivateSubject(string SubjectId, string ActivateOption)
        {
            // ActivateOption : New ExamSuite Mode: , New or replace ExamSuite Mode , Replace Subject Mode:

            //InActive.SubjectId IS InActive.SubjectCode
            var siteId = HomeController._centerdata.SiteId;
            var _inActiveSubject = repoForApprove.GetInActiveSubject(SubjectId);
            string SubjectCode = _inActiveSubject.SubjectCode;
            var now = DateTime.Now;
            if (_inActiveSubject == null)
            { return new InactiveApiResponse { Message = "ไม่มีวิชานี้", Code = InactiveApiResponse.ResponseCode.noinactivesubject }; }

            //get ActivedSubject
            var _activatedSubject = repoForApprove.GetActivatedSubjectBySubjectCode(SubjectCode, _inActiveSubject.ContentLanguage);

            if (_activatedSubject == null)
            {
                _activatedSubject = new ViewModels.ExamBankModelsBack.ShareData.ActivatedSubject
                {
                    _id = repoForApprove.GetNewGuid("").ToString(),
                    SubjectCode = SubjectCode,
                    SiteId = siteId,
                    CreateDateTime = now,
                    LastUpdateDateTime = now,
                    IsEReadiness = false,
                    DisabledDateTime = null,
                    SubjectId = "",//จะถูกใส่ตอน Upsert 
                    ContentLanguage = _inActiveSubject.ContentLanguage,
                };
            }

            //Get InActive for Convert
            var _inActiveExamSuite = repoQ.GetAllQuestionSuiteBySubjectId(SubjectId);
            //get old Subject
            var _oldSubject = repoForApprove.GetSubject(_activatedSubject.SubjectId);

            //var inActCtrl = new InActiveController(repoForApprove, null, null);
            foreach (var es in _inActiveExamSuite)
            {
                if (repoForApprove.GetConsiderationStatus(es) == "Wait")
                { return new InactiveApiResponse { Message = "ยังตรวจข้อสอบไม่ครบ", Code = InactiveApiResponse.ResponseCode.notacceptallquestion }; }
                else if (repoForApprove.GetConsiderationStatus(es) == "Rejected")
                { return new InactiveApiResponse { Message = "มีข้อสอบที่ถูกปฏิเสธ", Code = InactiveApiResponse.ResponseCode.notacceptallquestion }; }
            }

            // New ExamSuite Mode
            if (ActivateOption == "INS")
            {
                // Check ต้องเป็น suite ใหม่ทั้งหมด ถ้ามี return false 
                var _allNewTitleCode = _inActiveExamSuite.Select(x => x.Code)?.ToList();
                //if has exist
                var isExistExamSuite = _oldSubject?.ExamSuites?.Any(es => _allNewTitleCode?.Any(code => code == es.TitleCode) ?? true) ?? false;
                if (isExistExamSuite)
                { return new InactiveApiResponse { Message = "ไม่สามารถเริ่มใช้รายวิชาได้ เนื่องจากมีหมวดข้อสอบซ้ำกันในรายวิชาที่เริ่มใช้งานอยู่แล้ว", Code = InactiveApiResponse.ResponseCode.existingexamsuite }; }
            }


            var _newQuestion = new List<Activated.Question>();
            var _newSubject = this.ConvertToActivatedSubject(_inActiveSubject, _inActiveExamSuite, now, out _newQuestion);
            var _lastestSubjectVersion = repoForApprove.ListSubjectBySubjectCode(_activatedSubject.SubjectCode, _newSubject.ContentLanguage)
                ?.OrderByDescending(sub => sub.CreateDateTime)
                ?.FirstOrDefault()?.Version ?? this.ManageVersion(string.Empty, VersionModifyOption.InitialVersion);

            //Update ActivedSubject 
            _activatedSubject.SubjectId = _newSubject._id;

            if (_oldSubject == null)
            {
                //Update Version
                _newSubject.Version = this.ManageVersion(string.Empty, VersionModifyOption.InitialVersion);
                //Create ActivedSubject 
                repoForApprove.CreateActivatedSubject(_activatedSubject);
            }
            // New ExamSuite Mode
            // New or replace ExamSuite Mode (upsert)
            else if (ActivateOption == "INS" || ActivateOption == "UPS")
            {
                // Add Old suite
                var _newSuite = _newSubject.ExamSuites.Where(x => _oldSubject.ExamSuites.All(y => y.TitleCode != x.TitleCode))?.ToList();
                var _oldAndDupSuite = _oldSubject.ExamSuites.Except(_newSuite)?.ToList();
                var _oldSuite = _oldAndDupSuite.Where(x => _newSubject.ExamSuites.All(y => y.TitleCode != x.TitleCode))?.ToList();
                //_newSubject.ExamSuites = _oldAndDupSuite.Concat(_newSuite).ToList();
                _newSubject.ExamSuites.AddRange(_oldSuite);

                //add old group
                var _oldGroup = _oldSubject.ExamSuiteGroups.Where(x => _newSubject.ExamSuiteGroups.All(y => y._id != x._id))?.ToList();
                var _dupGroupFromNew = _newSubject.ExamSuiteGroups.Where(og => _oldSubject.ExamSuiteGroups.Any(ng => ng._id == og._id))?.ToList();
                //var _newGroup = _newSubject.ExamSuiteGroups.Except(_oldSubject.ExamSuiteGroups);
                //var _newAndDupGroup = _newSubject.ExamSuiteGroups.Except(_oldGroup);
                var _newGroup = _newSubject.ExamSuiteGroups.Where(x => _oldSubject.ExamSuiteGroups.All(y => y._id != x._id))?.ToList();

                //add new map to _oldGroup
                foreach (var group in _oldGroup)
                {
                    group.ExamSuiteGroupMaps.AddRange(_newSuite.Select(es => new Activated.ExamSuiteGroupMap()
                    {
                        _id = Guid.NewGuid().ToString(),
                        ExamSuiteId = es._id,
                        RandomCount = 0,
                    }).ToList());
                }

                //add old map to _dupGroupFromNew
                foreach (var group in _dupGroupFromNew)
                {
                    group.ExamSuiteGroupMaps.AddRange(_oldSuite.Select(es => new Activated.ExamSuiteGroupMap()
                    {
                        _id = Guid.NewGuid().ToString(),
                        ExamSuiteId = es._id,
                        RandomCount = _oldSubject.ExamSuiteGroups?.Select(g => g.ExamSuiteGroupMaps?.FirstOrDefault(m => m.ExamSuiteId == es._id))?.FirstOrDefault()?.RandomCount ?? 0,
                    }).ToList());
                }

                //add old map to _newGroup
                foreach (var group in _newGroup)
                {
                    group.ExamSuiteGroupMaps.AddRange(_oldSuite.Select(es => new Activated.ExamSuiteGroupMap()
                    {
                        _id = Guid.NewGuid().ToString(),
                        ExamSuiteId = es._id,
                        RandomCount = 0,
                    }).ToList());
                }

                _newSubject.ExamSuiteGroups = _oldGroup.Concat(_dupGroupFromNew).Concat(_newGroup).ToList();

                //Update Version
                _newSubject.Version = this.ManageVersion(_lastestSubjectVersion, VersionModifyOption.IncreaseMinor);
                //Update ActivedSubject 
                repoForApprove.UpsertActivatedSubject(_activatedSubject);
            }
            //Replace Subject Mode
            else if (ActivateOption == "REP")
            {
                //Update Version
                _newSubject.Version = ManageVersion(_lastestSubjectVersion, VersionModifyOption.IncreaseMajor);
                //Update ActivedSubject 
                repoForApprove.UpdateActivatedSubject(_activatedSubject);
            }
            else { return new InactiveApiResponse { Message = "กรุณาเลือกวิธีการ Activate", Code = InactiveApiResponse.ResponseCode.nooption }; }

            //All option must create newSubject and newQuestion
            repoForApprove.CreateSubject(_newSubject);
            repoForApprove.CreateQuestion(_newQuestion);

            //All option must delete InActiveSubject and Considerratio
            repoForApprove.DeleteAllInSubject(SubjectId);
            repoForApprove.DeleteAllConsiderration(_newSubject.ExamSuites.Select(es => es._id).ToList());
            return new InactiveApiResponse { Message = "เริ่มใช้รายวิชาสำเร็จ", Code = InactiveApiResponse.ResponseCode.success };
        }

        // HttpPut: api/ConfigurationInActive/AddExamSuiteGroup/{SubjectId}        
        [HttpPut]
        [Route("AddExamSuiteGroup/{SubjectId}")]
        public ExamSuiteGroup AddExamSuiteGruop(string SubjectId, [FromBody] ExamSuiteGroup examsuitegroup)
        {
            if (examsuitegroup == null)
            {
                return null;
            }

            //TODO: return false if ExamSuiteGroupName exist

            var _subject = repoForApprove.GetInActiveSubject(SubjectId);
            var _examSuites = repoQ.GetAllQuestionSuiteBySubjectId(SubjectId);

            // GetallSuite Create NewExamSuiteGruopMap 
            var _examSuitIdInSubject = _examSuites.Select(x => new InActive.ExamSuiteGroupMap
            {
                _id = repoForApprove.GetNewGuid("").ToString(),
                ExamSuiteId = x._id,
                RandomCount = 0,
            }).ToList();

            // Create NewExamSuiteGruop to InActiveSubject
            var newExamsuitegroup = new InActive.ExamSuiteGroup()
            {
                _id = examsuitegroup.ExamSuiteGroupName,
                ExamSuiteGroupName = examsuitegroup.ExamSuiteGroupName,
                PassScore = examsuitegroup.PassScore,
                ExamDuration = examsuitegroup.ExamDuration,
                ExamSuiteGroupMaps = _examSuitIdInSubject,
                IsUsed = examsuitegroup.IsUsed,
            };
            if (_subject.ExamSuiteGroups == null)
            {
                _subject.ExamSuiteGroups = new List<InActive.ExamSuiteGroup>();
            }
            _subject.ExamSuiteGroups.Add(newExamsuitegroup);

            repoForApprove.UpsertInactiveSubject(_subject);

            examsuitegroup.id = newExamsuitegroup._id;
            examsuitegroup.ExamSuiteGroupMaps = _examSuitIdInSubject.Select(x => new ExamSuiteGroupMap()
            {
                id = x._id,
                ExamSuiteId = x.ExamSuiteId,
                RandomCount = x.RandomCount,
                ExamSuiteGroupId = newExamsuitegroup._id,
                SubjectId = SubjectId,
            });
            return examsuitegroup;
        }

        // HttpPut: api/ConfigurationInActive/EditExamSuiteGroup/{SubjectId}/{ExamSuiteGroupId}  
        [HttpPut]
        [Route("EditExamSuiteGroup/{SubjectId}/{ExamSuiteGroupId}")]
        public void EditExamSuiteGruop(string SubjectId, int? ExamSuiteGroupId, [FromBody] ExamSuiteGroup examsuitegroup)
        {
            //EditExamSuiteGruop to InActiveSubject

            // get InactiveSubject    
            var _subject = repoForApprove.GetInActiveSubject(SubjectId);

            // edit ExamSuiteGruop
            var _group = _subject?.ExamSuiteGroups?.FirstOrDefault(x => x._id == examsuitegroup.id) ?? null;

            if (_group == null)
            {
                return;
            }

            _group.PassScore = examsuitegroup.PassScore;
            _group.ExamDuration = examsuitegroup.ExamDuration;
            _group.ExamSuiteGroupName = examsuitegroup.ExamSuiteGroupName;
            _group.IsUsed = examsuitegroup.IsUsed;

            // update InActiveSubject
            repoForApprove.UpsertInactiveSubject(_subject);
        }

        // HttpPut: api/ConfigurationInActive/EditExamsuiteGroupMap/{SubjectId}/{ExamSuiteGroupMapId}
        [HttpPut]
        [Route("EditExamsuiteGroupMap/{SubjectId}/{ExamSuiteGroupMapId}")]
        public void EditExamsuiteGroupMap(string SubjectId, string ExamSuiteGroupMapId, [FromBody] ExamSuiteGroupMap examsuiteGroupMap)
        {
            //EditExamsuiteGroupMap to InActiveSubject

            // get InactiveSubject
            var _inactiveSubject = repoForApprove.GetInActiveSubject(SubjectId);

            var map = _inactiveSubject?.ExamSuiteGroups?.FirstOrDefault(x => x._id == examsuiteGroupMap.ExamSuiteGroupId)?.ExamSuiteGroupMaps?.FirstOrDefault(x => x._id == examsuiteGroupMap.id) ?? null;

            if (map == null)
            {
                return;
            }

            map.RandomCount = examsuiteGroupMap.RandomCount;

            //var _group = _inactiveSubject.ExamSuiteGroups.Where(x => x._id == ExamSuiteGroupId).FirstOrDefault();

            //// edit ExamSuiteGroupMap
            //_group.ExamSuiteGroupMaps = examsuiteGroupMaps.Select(x => new InActive.ExamSuiteGroupMap
            //{
            //    _id = x.id,
            //    ExamSuiteId = x.ExamSuiteId,
            //    RandomCount = x.RandomCount,
            //}).ToList();

            // update InActiveSubject
            repoForApprove.UpsertInactiveSubject(_inactiveSubject);
        }

        // HttpGet: api/ConfigurationInActive/DeleteExamSuite/{ExamSuiteId}    
        [HttpGet]
        [Route("DeleteExamSuiteGroup/{SubjectId}/{ExamSuiteGroupId}")]
        public void DeleteExamSuiteGroup(string SubjectId, string ExamSuiteGroupId)
        {
            //get InactiveSubject
            var _inactiveSubject = repoForApprove.GetInActiveSubject(SubjectId);

            //get suite group
            var _inactivExamSuiteGroup = _inactiveSubject?.ExamSuiteGroups?.FirstOrDefault(x => x._id == ExamSuiteGroupId) ?? null;

            if (_inactivExamSuiteGroup == null)
            {
                return;
            }

            //remove suit in groupMap
            _inactiveSubject.ExamSuiteGroups.Remove(_inactivExamSuiteGroup);
            repoForApprove.UpsertInactiveSubject(_inactiveSubject);
        }

        // HttpGet: api/ConfigurationInActive/DeleteExamSuite/{ExamSuiteId}    
        [HttpGet]
        [Route("DeleteExamSuite/{ExamSuiteId}")]
        public void DeleteExamSuite(string ExamSuiteId)
        {
            //get suite
            var _inactivExamSuite = repoQ.GetQuestionSuite(ExamSuiteId);
            if (_inactivExamSuite == null)
            {
                return;
            }

            //getInactiveSubject
            var _inactiveSubject = repoForApprove.GetInActiveSubject(_inactivExamSuite.SubjectId);
            if (_inactiveSubject == null || _inactiveSubject.ExamSuiteGroups == null)
            {
                return;
            }

            //delete suit     
            repoQ.DeleteExamSuite(ExamSuiteId);

            //DeleteConsideration
            repoForApprove.DeleteAllConsiderration(ExamSuiteId);

            //remove suit in groupMap
            foreach (var examSuiteGroup in _inactiveSubject.ExamSuiteGroups)
            {
                if (examSuiteGroup.ExamSuiteGroupMaps == null)
                {
                    continue;
                }
                examSuiteGroup.ExamSuiteGroupMaps.RemoveAll(x => x.ExamSuiteId == ExamSuiteId);
            }
            repoForApprove.UpsertInactiveSubject(_inactiveSubject);

            repoQ.UpdateQuestionCount(_inactivExamSuite.SubjectId,
                _inactiveSubject.QuestionCount - _inactivExamSuite.Questions.Count(),
                _inactiveSubject.ExamSuiteCount - 1);

            var status = repoForApprove.GetConsiderationStatus(_inactivExamSuite);
            repoQ.UpdateQuestionCountComment(_inactivExamSuite.SubjectId,
                _inactiveSubject.ExamSuiteAcceptCount + (status == "Accepted" ? -1 : 0),
                _inactiveSubject.ExamSuiteRejectCount + (status == "Rejected" ? -1 : 0));
        }

        private Activated.Subject ConvertToActivatedSubject(InActive.InactiveSubject inActiceSubject, IEnumerable<QuestionSuiteVM> inActiveSuite, DateTime now, out List<Activated.Question> question)
        {
            if (inActiceSubject == null || inActiveSuite.Count() == 0)
            {
                question = new List<Activated.Question>();
                return null;
            }

            var newSubId = repoForApprove.GetNewSubId("").ToString();

            Activated.Subject newSubject = new Activated.Subject()
            {
                _id = newSubId,
                SubjectCode = inActiceSubject.SubjectCode,
                SubjectName = inActiceSubject.SubjectName,
                CreateDateTime = now,
                ContentLanguage = inActiceSubject.ContentLanguage,
                Version = "", // รอถามโต 
                ExamSuites = new List<Activated.ExamSuite>(),
                //ExamSuites = inActiveSuite.Select(x => new Activated.ExamSuite
                //{
                //    _id = x.Code,
                //    TitleCode = x.Code,
                //    TitleName = x.Title,
                //    CreateDateTime = now,
                //    QuestionIds = x.Questions.Select(y => y._id).ToList(),
                //}).ToList(),
                ExamSuiteGroups = inActiceSubject.ExamSuiteGroups.Select(esg => new Activated.ExamSuiteGroup
                {
                    _id = esg.ExamSuiteGroupName,
                    ExamSuiteGroupName = esg.ExamSuiteGroupName,
                    IsUsed = esg.IsUsed,
                    PassScore = esg.PassScore,
                    ExamDuration = esg.ExamDuration,
                    ExamSuiteGroupMaps = esg.ExamSuiteGroupMaps.Select(esgm => new Activated.ExamSuiteGroupMap
                    {
                        _id = esgm._id,
                        ExamSuiteId = esgm.ExamSuiteId,
                        RandomCount = esgm.RandomCount,
                    }).ToList(),
                }).ToList(),
                //  TODO: Add VoiceLanguages ??
                VoiceLanguages = new List<Activated.VoiceLanguage> {
                        new Activated.VoiceLanguage {
                            Language = inActiceSubject.ContentLanguage,
                            LanguageCode = inActiceSubject.ContentLanguage,
                            IsUsed = true
                        }
                    },
            };

            List<Activated.Question> newQuestion = new List<Activated.Question>();

            foreach (var item in inActiveSuite)
            {
                var qInSuite = ConvertToActivedQuestion(item, now);

                newQuestion.AddRange(qInSuite);

                newSubject.ExamSuites.Add(new Activated.ExamSuite
                {
                    _id = item._id,
                    TitleCode = item.Code,
                    TitleName = item.Title,
                    CreateDateTime = now,
                    QuestionIds = qInSuite.Select(x => x._id).ToList(),
                });
            }

            question = newQuestion;

            return newSubject;
        }

        private IEnumerable<Activated.Question> ConvertToActivedQuestion(QuestionSuiteVM inActiveSuite, DateTime now)
        {
            if (inActiveSuite == null)
            {
                return null;
            }

            List<Activated.Question> newQuestion = new List<Activated.Question>();

            var _question = inActiveSuite.Questions?.Select(x => new Activated.Question
            {
                _id = repoForApprove.GetNewQId("").ToString(),
                GroupId = x.GroupId,
                QuestionNumber = x.No,
                Detail = x.Content,
                //IsAllowRandomChoice = !x.NoShuffleChoice,
                IsAllowRandomChoice = x.NoShuffleChoice,
                Choices = x.Choices?.Select(y => new Activated.Choice
                {
                    _id = y.Code,
                    Detail = y.Content,
                    IsCorrect = y.IsCorrectAnswer,
                })?.ToList() ?? new List<Activated.Choice>(),
                CreateDateTime = now,
                Assets = x.Assets?.Select(ass => new Activated.Asset()
                {
                    id = Guid.NewGuid().ToString(),
                    Resource = ass.Resource,
                    ApplyTo = ass.ApplyTo,
                    Positions = ass.Positions,
                }) ?? new List<Activated.Asset>(),
            }).ToList();

            newQuestion.AddRange(_question);

            return newQuestion;
        }

        //private IEnumerable<Activated.Question> ConvertSuitesToActivedQuestion(IEnumerable<QuestionSuiteVM> inActiveSuite, DateTime now)
        //{

        //    if (inActiveSuite.Count() > 0)
        //    {
        //        List<Activated.Question> newQuestion = new List<Activated.Question>();
        //        foreach (var suite in inActiveSuite)
        //        {
        //            var _question = suite.Questions.Select(x => new Activated.Question
        //            {
        //                _id = repoForApprove.GetNewGuid().ToString(),
        //                GroupId = x.GroupId,
        //                QuestionNumber = x.No,
        //                Detail = x.Content,
        //                isAllowRandomChoice = x.NoShuffleChoice,
        //                Choices = x.Choices.Select(y => new Activated.Choice
        //                {
        //                    _id = y.Code,
        //                    Detail = y.Content,
        //                    IsCorrect = y.IsCorrectAnswer
        //                }).ToList(),
        //                CreateDateTime = now
        //            }).ToList();

        //            newQuestion.AddRange(_question);
        //        }

        //        return newQuestion;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        //private IEnumerable<Activated.ExamSuiteGroup> MergeGroup(Activated.Subject newSubject, Activated.Subject oldSubject)
        //{
        //    //TODO: Implemented   
        //    if (oldSubject != null)
        //    {
        //        var allGroup = newSubject.ExamSuiteGroups.Concat(oldSubject.ExamSuiteGroups);

        //        //Merge group
        //        //var mergeGroup = newSubject.ExamSuiteGroups.Union(oldSubject.ExamSuiteGroups, new WebSite.ActivatedSubjectComparer());

        //        var mergeSuite = oldSubject.ExamSuiteGroups.Union(newSubject.ExamSuiteGroups, new WebSite.ActivatedSubjectComparer());
        //        var oldGroup = oldSubject.ExamSuiteGroups.FirstOrDefault().ExamSuiteGroupMaps;

        //        List<WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroup> merged = new List<WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroup>();
        //        foreach (var element in mergeSuite)
        //        {
        //            WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroup data = new WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroup();
        //            data._id = element._id;
        //            data.ExamSuiteGroupName = element.ExamSuiteGroupName;
        //            data.IsUsed = element.IsUsed;
        //            data.PassScore = element.PassScore;
        //            data.ExamDuration = element.ExamDuration;
        //            data.ExamSuiteGroupMaps = new List<WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroupMap>();
        //            foreach (var element2 in oldGroup)
        //            {
        //                data.ExamSuiteGroupMaps.Add(new WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroupMap { _id = element2._id, ExamSuiteId = element2.ExamSuiteId, RandomCount = element2.RandomCount });
        //            }
        //            merged.Add(data);
        //        }

        //        foreach (var element in merged)
        //        {
        //            foreach (var element2 in newSubject.ExamSuiteGroups)
        //            {
        //                if (element._id == element2._id)
        //                {
        //                    element.ExamSuiteGroupMaps.ForEach(x => x.RandomCount = 0);
        //                    element.ExamSuiteGroupMaps.AddRange(element2.ExamSuiteGroupMaps);
        //                }
        //                else
        //                {
        //                    foreach (var element3 in element2.ExamSuiteGroupMaps)
        //                    {
        //                        element.ExamSuiteGroupMaps.Add(new WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject.ExamSuiteGroupMap { _id = element2._id, ExamSuiteId = element3.ExamSuiteId, RandomCount = 0 });
        //                    }
        //                }
        //            }
        //        }
        //        return merged;
        //    }
        //    else
        //    {
        //        return newSubject.ExamSuiteGroups;
        //    }
        //}

        private string ManageVersion(string version, VersionModifyOption option)
        {
            if (option == VersionModifyOption.InitialVersion) return "1.0.0";

            var versionArray = version.Split('.');
            try
            {
                if (versionArray.Length == 3)
                {
                    if (option == VersionModifyOption.IncreaseMajor)
                    {
                        versionArray[0] = (int.Parse(versionArray[0]) + 1).ToString();
                        versionArray[1] = "0";
                        versionArray[2] = "0";
                    }
                    else if (option == VersionModifyOption.IncreaseMinor)
                    {
                        versionArray[1] = (int.Parse(versionArray[1]) + 1).ToString();
                        versionArray[2] = "0";
                    }
                    else if (option == VersionModifyOption.IncreaseBuild)
                    {
                        versionArray[2] = (int.Parse(versionArray[2]) + 1).ToString();
                    }
                    return string.Join(".", versionArray);
                }
            }
            catch
            {
                throw new Exception("invalid version.");
            }
            return version;
        }
    }
}


