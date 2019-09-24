using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;
using WebSite.ViewModels.ExamBankModelsBack;
using WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;

namespace WebSite
{
    public class OracleUtility
    {
        public const string conString = "User Id=c##dsd;Password=1q2w3e4r;Data Source=localhost:1521/db;";

        public static InactiveSubject GetInactiveSubject(DataRow row)
        {
            if (row != null)
            {
                return new InactiveSubject
                {
                    _id = row["Id"].ToString(),
                    SubjectCode = row["SubjectCode"].ToString(),
                    SubjectName = row["SubjectName"].ToString(),
                    CreateDateTime = DateTime.Parse(row["CreateDateTime"].ToString()),
                    IsEReadiness = bool.Parse(row["IsEReadiness"].ToString()),
                    ContentLanguage = row["ContentLanguage"].ToString(),
                    SiteId = row["SiteId"].ToString(),
                    ExamSuiteCount = int.Parse(row["ExamSuiteCount"].ToString()),
                    QuestionCount = int.Parse(row["QuestionCount"].ToString()),
                    ExamSuiteAcceptCount = int.Parse(row["ExamSuiteAcceptCount"].ToString()),
                    ExamSuiteRejectCount = int.Parse(row["ExamSuiteRejectCount"].ToString()),
                };
            }

            return null;
        }

        public static QuestionSuiteVM GetQuestionSuite(DataRow row)
        {
            if (row != null)
            {
                return new QuestionSuiteVM
                {
                    _id = row["Id"].ToString(),
                    Code = row["Code"].ToString(),
                    Title = row["Title"].ToString(),
                    SubjectId = row["SubjectId"].ToString(),
                    SubjectCode = row["SubjectCode"].ToString(),
                    SubjectName = row["SubjectName"].ToString(),
                    Level = int.Parse(row["Level_"].ToString()),
                    LayoutCode = row["LayoutCode"].ToString(),
                    Description = row["Description"].ToString(),
                    Questions = Enumerable.Empty<MultipleChoiceQuestionWithOneCorrectAnswer>(),
                };
            }

            return null;
        }

        public static MultipleChoiceQuestionWithOneCorrectAnswer GetQuestion(DataRow row)
        {
            if (row != null)
            {
                return new MultipleChoiceQuestionWithOneCorrectAnswer
                {
                    _id = row["Id"].ToString(),
                    No = int.Parse(row["No"].ToString()),
                    Code = row["Code"].ToString(),
                    Content = row["Content"].ToString(),
                    GroupId = row["GroupId"].ToString(),
                    NoShuffleChoice = bool.Parse(row["NoShuffleChoice"].ToString()),
                    Assets = Enumerable.Empty<Asset>(),
                    Choices = Enumerable.Empty<SelectableChoice>(),
                };
            }

            return null;
        }

        public static SelectableChoice GetChoice(DataRow row)
        {
            if (row != null)
            {
                return new SelectableChoice
                {
                    Code = row["Code"].ToString(),
                    Content = row["Content"].ToString(),
                    IsCorrectAnswer = bool.Parse(row["IsCorrectAnswer"].ToString()),
                };
            }

            return null;
        }
    }
}
