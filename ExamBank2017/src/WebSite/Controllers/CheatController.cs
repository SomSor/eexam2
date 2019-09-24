using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebSite.Repositories;
using System;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;
using System.Collections;
using System.Collections.Generic;

namespace WebSite.Controllers
{
    public class CheatController : Controller
    {
        private IExamForApproveRepository repoForApprove;
        private IQuestionImportRepository repoQ;
        public CheatController(IExamForApproveRepository repoForApprove, IQuestionImportRepository repoQ)
        {
            this.repoForApprove = repoForApprove;
            this.repoQ = repoQ;
        }

        /// <summary>
        /// accept all Question in ExamSuite by Id
        /// </summary>
        /// <param name="ExamSuiteId"></param>
        [HttpGet]
        [Route("all/{ExamSuiteId}")]
        public void ConAllQ(string ExamSuiteId)
        {
            var examSuite = repoQ.GetQuestionSuite(ExamSuiteId);
            var Considerations = new List<InActive.Consideration>();
            foreach (var item in examSuite.Questions)
            {
                Considerations.Add(new InActive.Consideration()
                {
                    _id = repoForApprove.GetNewGuid("").ToString(),
                    CreateDateTime = DateTime.Now,
                    IsAccept = true,
                    UserName = "admin", //TODO : userName ??
                    ExamSuiteId = ExamSuiteId,
                    QuestionNumber = item.No,
                });
            }
            repoForApprove.CreateManyConsideration(Considerations);

            Response.Redirect("/#!/inactiveexamsuiteview/" + ExamSuiteId);
        }

        /// <summary>
        /// Update Question Count in Subject by Subject Id for show on index page
        /// </summary>
        /// <param name="SubjectId"></param>
        [HttpGet]
        [Route("qcnt/{SubjectId}")]
        public string UpdateQuestionCount(string SubjectId)
        {
            var subject = repoQ.GetInActiveSubject(SubjectId);
            var examSuites = repoQ.GetAllQuestionSuiteBySubjectId(SubjectId);
            int accept = 0, reject = 0;
            foreach (var es in examSuites)
            {
                var status = repoForApprove.GetConsiderationStatus(es);
                if (status == "Accepted") accept++;
                else if (status == "Rejected") reject++;
            }
            repoQ.UpdateQuestionCountComment(SubjectId, accept, reject);

            repoQ.UpdateQuestionCount(SubjectId, examSuites.Sum(es => es.Questions.Count()), examSuites.Count());

            //Response.Redirect("/#!/inactivesubjectview/" + SubjectId);
            return SubjectId + " " + subject.SubjectName + " SUCCESS !!";
        }

        /// <summary>
        /// show list of all subject for Update Question Count in Subject
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("qcntall")]
        public IActionResult UpdateQuestionCountAll()
        {
            var siteId = HomeController._centerdata.SiteId;
            var subjects = repoForApprove.ListInActiveSubject(siteId);
            return View(subjects);
        }

        /// <summary>
        /// Fix ExamSuite data MUST Implement case by case
        /// </summary>
        /// <param name="ExamSuiteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("fix/{ExamSuiteId}")]
        public string Fix(string ExamSuiteId)
        {
            ExamSuiteId = ExamSuiteId.Replace('_', '.');
            var examSuites = repoQ.GetQuestionSuite(ExamSuiteId);
            examSuites.SubjectId = examSuites.SubjectId.Replace("TH", "L" + examSuites.Level + "TH");
            repoQ.UpdateFIX(examSuites);
            UpdateQuestionCount(examSuites.SubjectId);
            return ExamSuiteId + " FIX SUCCESS !!";
        }

        /// <summary>
        /// show All Damage Question Suite for Fix ExamSuite data MUST Implement case by case
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("fixall")]
        public IActionResult FixExamSuite()
        {
            var examSuites = repoQ.GetAllDamageQuestionSuite();
            return View(examSuites);
        }
    }
}