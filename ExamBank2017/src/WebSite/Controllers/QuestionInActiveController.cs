using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Repositories;
using WebSite.ViewModels.ExamBankModels;
using InActive = WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;

namespace WebSite.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    [Route("api/[controller]")]
    public class QuestionInActiveController : Controller
    {
        private IExamForApproveRepository repoForApprove;
        private IQuestionImportRepository repoQ;
        private readonly IWebConfiguration webConfiguration;

        public QuestionInActiveController(IExamForApproveRepository repoForApprove, IQuestionImportRepository repoQ, IWebConfiguration webConfiguration)
        {
            this.repoForApprove = repoForApprove;
            this.repoQ = repoQ;
            this.webConfiguration = webConfiguration;
        }

        // HttpPost: api/QuestionInActive/ConsiderQuestion/{TitleCode}/{QuestionNumber}   
        [HttpPost]
        [Route("ConsiderQuestion/{TitleCode}/{QuestionNumber}")]
        public Consideration ConsiderQuestion(string TitleCode, int QuestionNumber, [FromBody] Consideration consideration)
        {
            var _consideration = new InActive.Consideration()
            {
                _id = repoForApprove.GetNewGuid("").ToString(),
                CreateDateTime = DateTime.Now,
                RejectComment = consideration?.RejectComment,
                IsAccept = consideration?.IsAccept ?? false,
                UserName = consideration?.UserName, //TODO : userName ??
                ExamSuiteId = consideration.ExamSuiteId,
                QuestionNumber = QuestionNumber,
            };

            var preConsiderationExamSuite = repoQ.GetQuestionSuite(consideration.ExamSuiteId);
            var preStatus = repoForApprove.GetConsiderationStatus(preConsiderationExamSuite);
            repoForApprove.CreateConsideration(_consideration);
            var postConsiderationExamSuite = repoQ.GetQuestionSuite(consideration.ExamSuiteId);
            var postStatus = repoForApprove.GetConsiderationStatus(postConsiderationExamSuite);

            var examSuite = repoQ.GetQuestionSuite(consideration.ExamSuiteId);
            var subject = repoQ.GetInActiveSubject(examSuite.SubjectId);
            int ExamSuiteAcceptCount = subject.ExamSuiteAcceptCount, ExamSuiteRejectCount = subject.ExamSuiteRejectCount;

            if (preStatus == "Accepted" && postStatus == "Rejected")
            {
                ExamSuiteAcceptCount--;
                ExamSuiteRejectCount++;
            }
            else if (preStatus == "Rejected" && postStatus == "Accepted")
            {
                ExamSuiteAcceptCount++;
                ExamSuiteRejectCount--;
            }
            else if (preStatus == "Wait" && postStatus == "Accepted")
            {
                ExamSuiteAcceptCount++;
            }
            else if (preStatus == "Wait" && postStatus == "Rejected")
            {
                ExamSuiteRejectCount++;
            }
            repoQ.UpdateQuestionCountComment(examSuite.SubjectId, ExamSuiteAcceptCount, ExamSuiteRejectCount);

            consideration.id = _consideration._id;
            consideration.CreateDateTime = _consideration.CreateDateTime;
            consideration.ExamSuiteId = consideration.ExamSuiteId;
            consideration.QuestionNumber = QuestionNumber;
            return consideration;
        }

        // HttpPut: api/QuestionInActive/EditQuestion/{TitleCode}/{QuestionId}     
        [HttpPut]
        [Route("EditQuestion/{TitleCode}/{QuestionId}")]
        public void EditQuestion(string TitleCode, int? QuestionId, [FromBody] Question question)
        {
            //For Edit IsAllowrandom กับ เฉลย
            if (question == null || !QuestionId.HasValue)
            {
                return;
            }

            var _suite = repoQ.GetQuestionSuite(question.ExamSuiteId);
            var _question = _suite?.Questions?.Where(x => x.No == question.QuestionNumber)?.FirstOrDefault();

            if (_question != null)
            {
                return;
            }

            _question.NoShuffleChoice = question?.IsAllowRandomChoice ?? false;
            _question.Choices = question.Choices?.Select(x => new TheS.ExamBank.DataFormats.SelectableChoice
            {
                Code = x.id,
                Content = x.Detail,
                IsCorrectAnswer = x.IsCorrect,
            });

            //Convert QuestionSuiteVM To QuestionSuite
            var qsuite = new TheS.ExamBank.DataFormats.QuestionSuite()
            {
                _id = _suite?._id,
                Code = _suite?.Code,
                Title = _suite?.Title,
                SubjectName = _suite?.SubjectName,
                Description = _suite?.Description,
                SubjectId = _suite?.SubjectId,
                SubjectCode = _suite?.SubjectCode,
                Level = _suite?.Level ?? 0,
                LayoutCode = _suite?.LayoutCode,
                Questions = _suite?.Questions,
            };

            repoQ.Upsert(qsuite);
        }

        [HttpPost]
        [Route("CreateExamSuite")]
        public IActionResult CreateExamSuite([FromBody] ExamSuiteDetail request)
        {
            //HACK: use SubjectId instead of ConsiderationStatus
            var _subject = repoQ.GetInActiveSubject(request.ConsiderationStatus);
            //var qid = string.Concat(qcode, "TH");
            var qid = Guid.NewGuid().ToString();
            var qsuite = new TheS.ExamBank.DataFormats.QuestionSuite()
            {
                _id = qid,
                Code = request.TitleCode,
                Title = request.TitleName,
                SubjectName = _subject.SubjectName,
                Description = "Description",
                SubjectId = request.ConsiderationStatus,
                SubjectCode = _subject.SubjectCode,
                Level = 1,
                LayoutCode = "LayoutCode",
                Questions = Enumerable.Empty<TheS.ExamBank.DataFormats.Question>(),
            };

            repoQ.InsertQuestionSuite(qsuite);

            var examSuiteCount = repoQ.GetAllQuestionSuiteBySubjectId(_subject._id).Count();
            repoQ.UpdateQuestionCount(_subject._id, _subject.QuestionCount, examSuiteCount);

            return Ok(new { Message = $"Created!", ExamSuiteId = qid });
        }

        [HttpPut]
        [Route("UpdateExamSuite")]
        public IActionResult UpdateExamSuite([FromBody] ExamSuiteDetail request)
        {
            var qsuite = repoQ.GetQuestionSuite(request.id);
            repoQ.UpdateQuestionSuiteCodeAndName(request.id, request.TitleCode, request.TitleName);

            return Ok(new { Message = $"Updated!", ExamSuiteId = request.id });
        }

        [HttpDelete]
        [Route("DeleteExamSuite/{examSuiteId}")]
        public IActionResult DeleteExamSuite(string examSuiteId)
        {
            repoQ.DeleteExamSuite(examSuiteId);

            return Ok(new { Message = $"Deleted!", ExamSuiteId = examSuiteId });
        }

        [HttpPost]
        [Route("CreateQuestion")]
        public IActionResult CreateQuestion([FromBody] Question request)
        {
            //var questionId = Guid.NewGuid().ToString();
            //repoQ.InsertQuestion(new TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswerWithId
            //{
            //    _id = questionId,
            //    No = request.QuestionNumber,
            //    NoShuffleChoice = request.IsAllowRandomChoice,
            //    Content = request.Detail,
            //    QuestionSuiteId = request.ExamSuiteId,
            //});

            //foreach (var choice in request.Choices)
            //{
            //    var choiceId = $"{questionId}-{choice.id}";
            //    repoQ.InsertChoice(new TheS.ExamBank.DataFormats.SelectableChoiceWithId
            //    {
            //        _id = choiceId,
            //        Code = choice.id,
            //        Content = choice.Detail,
            //        IsCorrectAnswer = choice.IsCorrect,
            //        QuestionId = questionId,
            //    });
            //}

            //var questionSuite = repoQ.GetQuestionSuite(request.ExamSuiteId);
            //var questionSuites = repoQ.GetAllQuestionSuiteBySubjectId(questionSuite.SubjectId);
            //var questionSuiteCount = questionSuites.Count();
            //var questionCount = questionSuites.Sum(qs => qs.Questions.Count());
            //repoQ.UpdateQuestionCount(questionSuite.SubjectId, questionCount, questionSuiteCount);

            var qsuiteVm = repoQ.GetQuestionSuite(request.ExamSuiteId);
            var questions = qsuiteVm.Questions.ToList();
            var questionId = Guid.NewGuid().ToString();
            questions.Add(new TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswer
            {
                _id = questionId,
                No = request.QuestionNumber,
                NoShuffleChoice = request.IsAllowRandomChoice,
                Content = request.Detail,
                Choices = request.Choices.Select(c => new TheS.ExamBank.DataFormats.SelectableChoice
                {
                    Code = c.id,
                    Content = c.Detail,
                    IsCorrectAnswer = c.IsCorrect,
                }).ToList(),
                Assets = Enumerable.Empty<TheS.ExamBank.DataFormats.Asset>(),
            });
            qsuiteVm.Questions = ReOrderQuestionNumber(questions);
            var qsuite = new TheS.ExamBank.DataFormats.QuestionSuite()
            {
                _id = qsuiteVm?._id,
                Code = qsuiteVm?.Code,
                Title = qsuiteVm?.Title,
                SubjectName = qsuiteVm?.SubjectName,
                Description = qsuiteVm?.Description,
                SubjectId = qsuiteVm?.SubjectId,
                SubjectCode = qsuiteVm?.SubjectCode,
                Level = qsuiteVm?.Level ?? 0,
                LayoutCode = qsuiteVm?.LayoutCode,
                Questions = qsuiteVm?.Questions,
            };

            repoQ.Upsert(qsuite);

            return Ok(new { Message = $"Created!", QuestionId = questionId });
        }

        [HttpPut]
        [Route("UpdateQuestion")]
        public IActionResult UpdateQuestion([FromBody] Question request)
        {
            //repoQ.UpdateQuestion(new TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswerWithId
            //{
            //    _id = request.id,
            //    No = request.QuestionNumber,
            //    NoShuffleChoice = request.IsAllowRandomChoice,
            //    Content = request.Detail,
            //    QuestionSuiteId = request.ExamSuiteId,
            //});

            //foreach (var choice in request.Choices)
            //{
            //    var choiceId = $"{request.id}-{choice.id}";
            //    repoQ.UpdateChoice(new TheS.ExamBank.DataFormats.SelectableChoiceWithId
            //    {
            //        _id = choiceId,
            //        Code = choice.id,
            //        Content = choice.Detail,
            //        IsCorrectAnswer = choice.IsCorrect,
            //        QuestionId = request.id,
            //    });
            //}

            var qsuiteVm = repoQ.GetQuestionSuite(request.ExamSuiteId);
            var question = qsuiteVm?.Questions?.Where(x => x._id == request.id)?.FirstOrDefault();
            question.Content = request.Detail;
            question.NoShuffleChoice = request.IsAllowRandomChoice;
            question.Choices = request.Choices?.Select(x => new TheS.ExamBank.DataFormats.SelectableChoice
            {
                Code = x.id,
                Content = x.Detail,
                IsCorrectAnswer = x.IsCorrect,
            });

            var qsuite = new TheS.ExamBank.DataFormats.QuestionSuite()
            {
                _id = qsuiteVm?._id,
                Code = qsuiteVm?.Code,
                Title = qsuiteVm?.Title,
                SubjectName = qsuiteVm?.SubjectName,
                Description = qsuiteVm?.Description,
                SubjectId = qsuiteVm?.SubjectId,
                SubjectCode = qsuiteVm?.SubjectCode,
                Level = qsuiteVm?.Level ?? 0,
                LayoutCode = qsuiteVm?.LayoutCode,
                Questions = ReOrderQuestionNumber(qsuiteVm?.Questions),
            };

            repoQ.Upsert(qsuite);

            return Ok(new { Message = $"Updated!", QuestionId = request.id });
        }

        [HttpDelete]
        [Route("DeleteQuestion/{examsuiteid}/{questionid}")]
        public IActionResult DeleteQuestion(string examsuiteid, string questionid)
        {
            var qsuiteVm = repoQ.GetQuestionSuite(examsuiteid);
            var qsuite = new TheS.ExamBank.DataFormats.QuestionSuite()
            {
                _id = qsuiteVm?._id,
                Code = qsuiteVm?.Code,
                Title = qsuiteVm?.Title,
                SubjectName = qsuiteVm?.SubjectName,
                Description = qsuiteVm?.Description,
                SubjectId = qsuiteVm?.SubjectId,
                SubjectCode = qsuiteVm?.SubjectCode,
                Level = qsuiteVm?.Level ?? 0,
                LayoutCode = qsuiteVm?.LayoutCode,
                Questions = ReOrderQuestionNumber(qsuiteVm.Questions.Where(q => q._id != questionid).ToList()),
            };

            repoQ.Upsert(qsuite);

            return Ok(new { Message = $"Deleted!", QuestionId = questionid });
        }

        [HttpGet]
        [Route("Images")]
        public IActionResult ListImage()
        {
            var files = System.IO.Directory.GetFiles(webConfiguration.UploadPath, "*.*", System.IO.SearchOption.AllDirectories)
                .Where(f =>
                {
                    return !webConfiguration.SearchPatterns.Any(sp => f.Contains(sp));
                })
                .ToList();

            var fileUrls = files.Select(x =>
            {
                return x.Replace(webConfiguration.UploadPath, webConfiguration.UploadUrl).Replace("\\", "/");
            });
            return Ok(new
            {
                fileUrls,
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private IEnumerable<TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswer> ReOrderQuestionNumber(IEnumerable<TheS.ExamBank.DataFormats.MultipleChoiceQuestionWithOneCorrectAnswer> questions)
        {
            var i = 1;
            return questions.Select(q =>
            {
                q.No = i++;
                return q;
            }).ToList();
        }
    }
}


