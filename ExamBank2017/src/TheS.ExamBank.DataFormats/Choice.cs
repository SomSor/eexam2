using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheS.ExamBank.DataFormats
{
    public class Choice
    {
        public string Code { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// Selectable choice, use with multiple choice question - not a matching question.
    /// </summary>
    public class SelectableChoice : Choice
    {
        public bool IsCorrectAnswer { get; set; }
    }

    public sealed class SelectableChoiceWithId : SelectableChoice
    {
        public string _id { get; set; }
        public string QuestionId { get; set; }
    }
}
