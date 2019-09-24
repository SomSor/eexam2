using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Repositories;
using WebSite.ViewModels.ExamBankModels;
using Activated = WebSite.ViewModels.ExamBankModelsBack.ActivatedSubject;

namespace WebSite.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("api/[controller]")]
    public class ConfigurationActivatedController : Controller
    {
        private IExamForApproveRepository repoForApprove;

        public ConfigurationActivatedController(IExamForApproveRepository repoForApprove)
        {
            this.repoForApprove = repoForApprove;
        }
        // HttpGet: api/ConfigurationActivated/ReversionSubject/{SubjectId}
        [HttpGet]
        [Route("ReversionSubject/{SubjectCode}/{SubjectId}")]
        public void ReversionSubject(string SubjectCode, string SubjectId)
        {
            //get subject
            var subject = repoForApprove.GetSubject(SubjectId);
            //get ActivedSubject
            var ActivedSubject = repoForApprove.GetActivatedSubjectBySubjectCode(subject.SubjectCode, subject.ContentLanguage);
            //Update ActivedSubject 
            ActivedSubject.SubjectId = SubjectId;
            ActivedSubject.LastUpdateDateTime = DateTime.Now;

            repoForApprove.UpdateActivatedSubject(ActivedSubject);
        }

        // HttpGet: api/ConfigurationActivated/DisableSubject/{SubjectId}      
        [HttpGet]
        [Route("DisableSubject/{SubjectId}")]
        public void DisableSubject(string SubjectId)
        {
            var activatedSubjec = repoForApprove.GetActivatedSubjectBySubjectId(SubjectId);

            var now = DateTime.Now;
            activatedSubjec.DisabledDateTime = now;
            activatedSubjec.LastUpdateDateTime = now;

            repoForApprove.UpsertActivatedSubject(activatedSubjec);
        }

        // HttpGet: api/ConfigurationActivated/DisableSubject/{SubjectId}      
        [HttpGet]
        [Route("EnableSubject/{SubjectId}")]
        public void EnableSubject(string SubjectId)
        {
            var activatedSubjec = repoForApprove.GetActivatedSubjectBySubjectId(SubjectId);

            var now = DateTime.Now;
            activatedSubjec.DisabledDateTime = null;
            activatedSubjec.LastUpdateDateTime = now;

            repoForApprove.UpsertActivatedSubject(activatedSubjec);
        }

        // HttpPut: api/ConfigurationActivated/ActivedSubjectVoice/{SubjectId} 		
        [HttpPut]
        [Route("ActivedSubjectVoice/{SubjectId}")]
        public void ActivedSubjectVoice(string SubjectId)
        {
            throw new NotImplementedException();
        }

        // HttpPut: api/ConfigurationActivated/AddExamSuiteGroup/{SubjectId}  
        [HttpPut]
        [Route("AddExamSuiteGroup/{SubjectId}")]
        public ExamSuiteGroup AddExamSuiteGruop(string SubjectId, [FromBody] ExamSuiteGroup examsuitegroup)
        {
            if (examsuitegroup != null)
            {
                //getsubject
                var _subject = repoForApprove.GetSubject(examsuitegroup.SubjectId);
                //addnew group

                var _examSuitIdInSubject = _subject.ExamSuites.Select(x => new Activated.ExamSuiteGroupMap
                {
                    _id = Guid.NewGuid().ToString(),
                    ExamSuiteId = x._id,
                    RandomCount = 0,
                }).ToList();

                var newExamsuitegroup = new Activated.ExamSuiteGroup
                {
                    _id = examsuitegroup.ExamSuiteGroupName,
                    ExamDuration = examsuitegroup.ExamDuration,
                    IsUsed = examsuitegroup.IsUsed,
                    PassScore = examsuitegroup.PassScore,
                    ExamSuiteGroupName = examsuitegroup.ExamSuiteGroupName,
                    ExamSuiteGroupMaps = _examSuitIdInSubject
                };
                _subject.ExamSuiteGroups.Add(newExamsuitegroup);
                //upsertsubject
                repoForApprove.UpsertSubject(_subject);

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
            return null;
        }

        // HttpGet: api/ConfigurationInActive/DeleteExamSuite/{ExamSuiteId}    
        [HttpGet]
        [Route("DeleteExamSuiteGroup/{SubjectId}/{ExamSuiteGroupId}")]
        public void DeleteExamSuiteGroup(string SubjectId, string ExamSuiteGroupId)
        {
            //get InactiveSubject
            var _subject = repoForApprove.GetSubject(SubjectId);

            //get suite group
            var _activatedExamSuiteGroup = _subject.ExamSuiteGroups.FirstOrDefault(x => x._id == ExamSuiteGroupId);

            //remove suit in groupMap
            if (_activatedExamSuiteGroup != null)
            {
                _subject.ExamSuiteGroups.Remove(_activatedExamSuiteGroup);
                repoForApprove.UpsertSubject(_subject);
            }
        }

        // HttpGet: api/ConfigurationActive/RandomExamSheet/{ExamSuiteId}    
        [HttpGet]
        [Route("RandomExamSheet/{CenterId}/{SubjectId}/{ContentLanguage}")]
        public void RandomExamSheet(string centerId, string subjectId, string contentLanguage, int quantity)
        {
            throw new NotImplementedException();
        }

        // HttpPut: api/ConfigurationActivated/EditExamSuiteGroup/{SubjectId}/{ExamSuiteGroupId}  
        [HttpPut]
        [Route("EditExamSuiteGroup/{SubjectId}/{ExamSuiteGroupId}")]
        public void EditExamSuiteGruop(string SubjectId, string ExamSuiteGroupId, [FromBody]  ExamSuiteGroup examsuitegroup)
        {
            //getsubject
            var _subject = repoForApprove.GetSubject(examsuitegroup.SubjectId);
            //EditExamSuiteGroup
            var _group = _subject.ExamSuiteGroups.Where(x => x._id == ExamSuiteGroupId).FirstOrDefault();
            _group.PassScore = examsuitegroup.PassScore;
            _group.ExamDuration = examsuitegroup.ExamDuration;
            _group.ExamSuiteGroupName = examsuitegroup.ExamSuiteGroupName;
            _group.IsUsed = examsuitegroup.IsUsed;
            //upsertsubject
            repoForApprove.UpsertSubject(_subject);
        }

        // HttpPut: api/ConfigurationActivated/EditExamsuiteGroupMap/{SubjectId}/{ExamSuiteGroupMapId}  
        [HttpPut]
        [Route("EditExamsuiteGroupMap/{SubjectId}/{ExamSuiteGroupMapId}")]
        public void EditExamsuiteGroupMap(string SubjectId, string ExamSuiteGroupMapId, [FromBody]  ExamSuiteGroupMap examsuiteGroupMap)
        {
            //getsubject
            var _subject = repoForApprove.GetSubject(examsuiteGroupMap.SubjectId);
            //EditExamsuiteGroupMap

            var map = _subject.ExamSuiteGroups.FirstOrDefault(x => x._id == examsuiteGroupMap.ExamSuiteGroupId).ExamSuiteGroupMaps.FirstOrDefault(x => x._id == examsuiteGroupMap.id);
            //var map = _subject.ExamSuiteGroups.Select(x => x.ExamSuiteGroupMaps.Where(y => y._id == ExamSuiteGroupMapId).FirstOrDefault()).FirstOrDefault();

            map.RandomCount = examsuiteGroupMap.RandomCount;

            //var _group = _subject.ExamSubjectGroups.Where(x => x._id == ExamSuiteGroupMapId).FirstOrDefault();

            //_group.ExamSubjectGroupMaps = examsuiteGroupMaps.Select(x => new Activated.ExamSubjectGroupMap
            //{
            //    _id = x.id,
            //    ExamSuiteId = x.ExamSuiteId,
            //    RandomCount = x.RandomCount,
            //}).ToList();

            //upsertsubject
            repoForApprove.UpsertSubject(_subject);
        }
    }
}


