using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats.Attributes;

namespace TheS.ExamBank.DataFormats
{
    /// <summary>
    /// Represent a question concept
    /// </summary>
    [DataModel]
    public abstract class Question
    {
        public string _id { get; set; }

        public IEnumerable<Asset> Assets { get; set; }

        /// <summary>
        /// Reference to Qxx in the source file (markdown).
        /// </summary>
        public int No { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// Use for specify the group which this question is belonged to.
        /// </summary>
        public string GroupId { get; set; }

        public abstract Question Clone();
    }

    /// <summary>
    /// A concept of multiple choice question
    /// </summary>
    public abstract class MultipleChoiceQuestion : Question
    {
        public bool NoShuffleChoice { get; set; }
        public IEnumerable<SelectableChoice> Choices { get; set; }
    }

    /// <summary>
    /// A multiple choice question with only one correct answer
    /// </summary>
    public class MultipleChoiceQuestionWithOneCorrectAnswer : MultipleChoiceQuestion
    {
        public override Question Clone()
        {
            return new MultipleChoiceQuestionWithOneCorrectAnswer
            {
                _id = this._id,
                Code = this.Code,
                Content = this.Content,
                No = this.No,
                NoShuffleChoice = this.NoShuffleChoice,
                GroupId = this.GroupId,
                Assets = this.Assets,
                Choices = (from it in this.Choices
                           select new SelectableChoice
                           {
                               Code = it.Code,
                               Content = it.Content,
                               IsCorrectAnswer = it.IsCorrectAnswer,
                           }).ToList(),
            };
        }
    }

    public class MultipleChoiceQuestionWithOneCorrectAnswerWithId : MultipleChoiceQuestionWithOneCorrectAnswer
    {
        public string QuestionSuiteId { get; set; }
    }
}
