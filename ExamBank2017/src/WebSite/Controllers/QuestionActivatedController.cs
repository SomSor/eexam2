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
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    [Route("api/[controller]")]
    public class QuestionActivatedController : Controller
    {
        private IExamForApproveRepository repoForApprove;
        public QuestionActivatedController(IExamForApproveRepository repoForApprove)
        {
            this.repoForApprove = repoForApprove;
        }

        // HttpPut: api/QuestionActivated/EditQuestion/{QuestionId}  
        [HttpPut]
        [Route("EditQuestion/{QuestionId}")]
        public void EditQuestion(string QuestionId, [FromBody] Question question)
        {
            if (string.IsNullOrEmpty(QuestionId))
            {
                return;
            }

            var _question = repoForApprove.GetQuestionByQID(QuestionId);
            if (_question == null)
            {
                return;
            }

            _question.IsAllowRandomChoice = question?.IsAllowRandomChoice ?? false;
            _question.Choices = question?.Choices?.Select(c => new Activated.Choice
            {
                _id = c.id,
                Detail = c.Detail,
                IsCorrect = c.IsCorrect,
            })?.ToList();

            repoForApprove.UpsertActivedQuestion(_question);

        }
    }
}


