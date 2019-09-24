using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats.Attributes;

namespace TheS.ExamBank.DataFormats.DataModels
{
    [DataModel(Root = false)]
    public class QuestionRef
    {
        public string QuestionId { get; set; }

        /// <summary>
        /// Use for specify the group which this question is belonged to.
        /// </summary>
        public string GroupId { get; set; }
    }
}
