using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;
using TheS.ExamBank.Parsers;
using WebSite.ViewModels.ExamBankModelsBack;
using WebSite.ViewModels.ExamBankModelsBack.InActiveSubject;

namespace WebSite.Repositories.OracleImpl
{
    public class OraQuestionImportRepository : IQuestionImportRepository
    {
        public void DeleteExamSuite(string ExamSuiteId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"DELETE FROM {OracleDbTableName.QuestionSuite} WHERE Id=:Id";
                cmd.Parameters.Add("Id", ExamSuiteId);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteExamSuiteBySubjectId(string SubjectId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"DELETE FROM {OracleDbTableName.QuestionSuite} WHERE SubjectId=:SubjectId";
                cmd.Parameters.Add("SubjectId", SubjectId);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteInActiveSubject(string SubjectId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"DELETE FROM {OracleDbTableName.InactiveSubject} WHERE Id=:Id";
                cmd.Parameters.Add("Id", SubjectId);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<QuestionSuite> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionSuiteVM> GetAllDamageQuestionSuite()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionSuiteVM> GetAllQuestionSuiteBySubjectId(string SubjectId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {OracleDbTableName.QuestionSuite} WHERE SubjectId=:SubjectId";
                cmd.Parameters.Add("SubjectId", SubjectId);
                cmd.CommandType = CommandType.Text;

                var da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                var ds = new DataSet();
                da.Fill(ds);
                var dt = ds.Tables[0];

                var questionSuites = Enumerable.Empty<QuestionSuiteVM>().ToList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    questionSuites.Add(OracleUtility.GetQuestionSuite(dt.Rows[i]));
                }

                foreach (var questionSuite in questionSuites)
                {
                    var cmd2 = con.CreateCommand();
                    cmd2.CommandText = $"SELECT * FROM {OracleDbTableName.Question} WHERE QuestionSuiteId=:QuestionSuiteId";
                    cmd2.Parameters.Add("QuestionSuiteId", questionSuite._id);
                    cmd2.CommandType = CommandType.Text;

                    var da2 = new OracleDataAdapter();
                    da2.SelectCommand = cmd2;
                    ds = new DataSet();
                    da2.Fill(ds);
                    dt = ds.Tables[0];

                    var questions = Enumerable.Empty<MultipleChoiceQuestionWithOneCorrectAnswer>().ToList();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        questions.Add(OracleUtility.GetQuestion(dt.Rows[i]));
                    }
                    questionSuite.Questions = questions;

                    foreach (var question in questions)
                    {
                        var cmd3 = con.CreateCommand();
                        cmd3.CommandText = $"SELECT * FROM {OracleDbTableName.Choice} WHERE QuestionId=:QuestionId";
                        cmd3.Parameters.Add("QuestionId", question._id);
                        cmd3.CommandType = CommandType.Text;

                        var da3 = new OracleDataAdapter();
                        da3.SelectCommand = cmd3;
                        ds = new DataSet();
                        da3.Fill(ds);
                        dt = ds.Tables[0];

                        var choices = Enumerable.Empty<SelectableChoice>().ToList();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            choices.Add(OracleUtility.GetChoice(dt.Rows[i]));
                        }
                        question.Choices = choices;
                    }
                }

                return questionSuites;
            }
        }

        public InactiveSubject GetInActiveSubject(string inactiveSubjectId)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {OracleDbTableName.InactiveSubject} WHERE Id=:Id";
                cmd.Parameters.Add("Id", inactiveSubjectId);
                cmd.CommandType = CommandType.Text;

                var da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                var ds = new DataSet();
                da.Fill(ds);
                var dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return OracleUtility.GetInactiveSubject(dt.Rows[0]);
                }

                return null;
            }
        }

        public QuestionSuiteVM GetQuestionSuite(string id)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {OracleDbTableName.QuestionSuite} WHERE Id=:Id";
                cmd.Parameters.Add("Id", id);
                cmd.CommandType = CommandType.Text;

                var da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                var ds = new DataSet();
                da.Fill(ds);
                var dt = ds.Tables[0];


                if (dt.Rows.Count > 0)
                {
                    var questionSuite = OracleUtility.GetQuestionSuite(dt.Rows[0]);

                    var cmd2 = con.CreateCommand();
                    cmd2.CommandText = $"SELECT * FROM {OracleDbTableName.Question} WHERE QuestionSuiteId=:QuestionSuiteId";
                    cmd2.Parameters.Add("QuestionSuiteId", questionSuite._id);
                    cmd2.CommandType = CommandType.Text;

                    var da2 = new OracleDataAdapter();
                    da2.SelectCommand = cmd2;
                    ds = new DataSet();
                    da2.Fill(ds);
                    dt = ds.Tables[0];

                    var questions = Enumerable.Empty<MultipleChoiceQuestionWithOneCorrectAnswer>().ToList();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        questions.Add(OracleUtility.GetQuestion(dt.Rows[i]));
                    }
                    questionSuite.Questions = questions;

                    foreach (var question in questions)
                    {
                        var cmd3 = con.CreateCommand();
                        cmd3.CommandText = $"SELECT * FROM {OracleDbTableName.Choice} WHERE QuestionId=:QuestionId";
                        cmd3.Parameters.Add("QuestionId", question._id);
                        cmd3.CommandType = CommandType.Text;

                        var da3 = new OracleDataAdapter();
                        da3.SelectCommand = cmd3;
                        ds = new DataSet();
                        da3.Fill(ds);
                        dt = ds.Tables[0];

                        var choices = Enumerable.Empty<SelectableChoice>().ToList();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            choices.Add(OracleUtility.GetChoice(dt.Rows[i]));
                        }
                        question.Choices = choices;
                    }

                    return questionSuite;
                }
                return null;
            }
        }

        public void InsertChoice(SelectableChoiceWithId choice)
        {
            using (OracleConnection con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"INSERT INTO {OracleDbTableName.Choice} " +
                    $"VALUES(:Id,:Code,:Content,:IsCorrectAnswer,:QuestionId)";
                cmd.Parameters.Add("Id", choice._id);
                cmd.Parameters.Add("Code", choice.Code);
                cmd.Parameters.Add("Content", choice.Content);
                cmd.Parameters.Add("IsCorrectAnswer", choice.IsCorrectAnswer.ToString());
                cmd.Parameters.Add("QuestionId", choice.QuestionId);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertInActiveSubject(InactiveSubject InactiveSubject)
        {
            using (OracleConnection con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"INSERT INTO {OracleDbTableName.InactiveSubject} " +
                    $"VALUES(:Id,:SubjectCode,:SubjectName,:CreateDateTime,:IsEReadiness,:ContentLanguage,:SiteId," +
                    $":ExamSuiteCount,:QuestionCount,:ExamSuiteAcceptCount,:ExamSuiteRejectCount)";
                cmd.Parameters.Add("Id", InactiveSubject._id);
                cmd.Parameters.Add("SubjectCode", InactiveSubject.SubjectCode);
                cmd.Parameters.Add("SubjectName", InactiveSubject.SubjectName);
                cmd.Parameters.Add("CreateDateTime", InactiveSubject.CreateDateTime);
                cmd.Parameters.Add("IsEReadiness", InactiveSubject.IsEReadiness.ToString());
                cmd.Parameters.Add("ContentLanguage", InactiveSubject.ContentLanguage);
                cmd.Parameters.Add("SiteId", InactiveSubject.SiteId);
                cmd.Parameters.Add("ExamSuiteCount", InactiveSubject.ExamSuiteCount);
                cmd.Parameters.Add("QuestionCount", InactiveSubject.QuestionCount);
                cmd.Parameters.Add("ExamSuiteAcceptCount", InactiveSubject.ExamSuiteAcceptCount);
                cmd.Parameters.Add("ExamSuiteRejectCount", InactiveSubject.ExamSuiteRejectCount);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertQuestion(MultipleChoiceQuestionWithOneCorrectAnswerWithId question)
        {
            using (OracleConnection con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"INSERT INTO {OracleDbTableName.Question} " +
                    $"VALUES(:Id,:No,:Code,:Content,:GroupId,:NoShuffleChoice,:QuestionSuiteId)";
                cmd.Parameters.Add("Id", question._id);
                cmd.Parameters.Add("No", question.No);
                cmd.Parameters.Add("Code", question.Code);
                cmd.Parameters.Add("Content", question.Content);
                cmd.Parameters.Add("GroupId", question.GroupId);
                cmd.Parameters.Add("NoShuffleChoice", question.NoShuffleChoice.ToString());
                cmd.Parameters.Add("QuestionSuiteId", question.QuestionSuiteId);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertQuestionSuite(QuestionSuite qsuite)
        {
            using (OracleConnection con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"INSERT INTO {OracleDbTableName.QuestionSuite} " +
                    $"VALUES(:Id,:Code,:Title,:SubjectId,:SubjectCode,:SubjectName,:Level_,:LayoutCode,:Description)";
                cmd.Parameters.Add("Id", qsuite._id);
                cmd.Parameters.Add("Code", qsuite.Code);
                cmd.Parameters.Add("Title", qsuite.Title);
                cmd.Parameters.Add("SubjectId", qsuite.SubjectId);
                cmd.Parameters.Add("SubjectCode", qsuite.SubjectCode);
                cmd.Parameters.Add("SubjectName", qsuite.SubjectName);
                cmd.Parameters.Add("Level_", qsuite.Level);
                cmd.Parameters.Add("LayoutCode", qsuite.LayoutCode);
                cmd.Parameters.Add("Description", qsuite.Description);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateChoice(SelectableChoiceWithId choice)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"UPDATE {OracleDbTableName.Choice} SET Code=:Code, Content=:Content, IsCorrectAnswer=:IsCorrectAnswer WHERE Id=:Id";
                cmd.Parameters.Add("Code", choice.Code);
                cmd.Parameters.Add("Content", choice.Content);
                cmd.Parameters.Add("IsCorrectAnswer", choice.IsCorrectAnswer.ToString());
                cmd.Parameters.Add("Id", choice._id);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateFIX(QuestionSuiteVM examSuite)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(MultipleChoiceQuestionWithOneCorrectAnswerWithId question)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"UPDATE {OracleDbTableName.Question} SET No=:No, Code=:Code, Content=:Content, GroupId=:GroupId, NoShuffleChoice=:NoShuffleChoice WHERE Id=:Id";
                cmd.Parameters.Add("No", question.No);
                cmd.Parameters.Add("Code", question.Code);
                cmd.Parameters.Add("Content", question.Content);
                cmd.Parameters.Add("GroupId", question.GroupId);
                cmd.Parameters.Add("NoShuffleChoice", question.NoShuffleChoice.ToString());
                cmd.Parameters.Add("Id", question._id);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateQuestionCount(string subjectId, int QuestionCount, int ExamSuiteCount)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"UPDATE {OracleDbTableName.InactiveSubject} SET ExamSuiteCount=:ExamSuiteCount, QuestionCount=:QuestionCount WHERE Id=:Id";
                cmd.Parameters.Add("ExamSuiteCount", ExamSuiteCount);
                cmd.Parameters.Add("QuestionCount", QuestionCount);
                cmd.Parameters.Add("Id", subjectId);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateQuestionCountComment(string subjectId, int ExamSuiteAcceptCount, int ExamSuiteRejectCount)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestionSuiteCodeAndName(string QuestionSuiteId, object Code, object Title)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"UPDATE {OracleDbTableName.QuestionSuite} SET Code=:Code, Title=:Title WHERE Id=:Id";
                cmd.Parameters.Add("Code", Code);
                cmd.Parameters.Add("Title", Title);
                cmd.Parameters.Add("Id", QuestionSuiteId);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSubjectCodeAndNameAndContentLanguage(string subjectId, string subjectCode, string subjectName, string contentLanguage)
        {
            using (var con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"UPDATE {OracleDbTableName.InactiveSubject} SET SubjectCode=:SubjectCode, SubjectName=:SubjectName, ContentLanguage=:ContentLanguage WHERE Id=:Id";
                cmd.Parameters.Add("SubjectCode", subjectCode);
                cmd.Parameters.Add("SubjectName", subjectName);
                cmd.Parameters.Add("ContentLanguage", contentLanguage);
                cmd.Parameters.Add("Id", subjectId);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSubjectNameAndContentLanguage(string subjectId, string subjectName, string contentLanguage)
        {
            throw new NotImplementedException();
        }

        public void UploadAssets(IEnumerable<AssetFileUtil.Asset> assetFiles, string qid, ICloudStorage cloudStorage)
        {
            throw new NotImplementedException();
        }

        public void Upsert(QuestionSuite qsuite)
        {
            using (OracleConnection con = new OracleConnection(OracleUtility.conString))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = $"INSERT INTO {OracleDbTableName.QuestionSuite} " +
                    $"VALUES(:Id,:Code,:Title,:SubjectId,:SubjectCode,:SubjectName,:Level,:LayoutCode,:Description)" +
                    $"WHEN duplicate THEN SET Code=:Code, Title=:Title, SubjectId=:SubjectId, SubjectCode=:SubjectCode, " +
                    $"SubjectName=:SubjectName, Level_=:Level, LayoutCode=:LayoutCode, Description=:Description, ";
                cmd.Parameters.Add("Id", qsuite._id);
                cmd.Parameters.Add("Code", qsuite.Code);
                cmd.Parameters.Add("Title", qsuite.Title);
                cmd.Parameters.Add("SubjectId", qsuite.SubjectId);
                cmd.Parameters.Add("SubjectCode", qsuite.SubjectCode);
                cmd.Parameters.Add("SubjectName", qsuite.SubjectName);
                cmd.Parameters.Add("Level", qsuite.Level);
                cmd.Parameters.Add("LayoutCode", qsuite.LayoutCode);
                cmd.Parameters.Add("Description", qsuite.Description);
                cmd.ExecuteNonQuery();

                var cmd2 = con.CreateCommand();
                var commandText = $"REPLACE INTO {OracleDbTableName.Question} VALUES ";
                int i = 0;
                foreach (MultipleChoiceQuestionWithOneCorrectAnswer question in qsuite.Questions)
                {
                    commandText += $"(:Id{i},:No{i},:Code{i},:Content{i},:GroupId{i},:NoShuffleChoice{i})";
                    if (i < qsuite.Questions.Count()) commandText += ",";

                    cmd2.Parameters.Add($"Id{i}", question._id);
                    cmd2.Parameters.Add($"No{i}", question.No);
                    cmd2.Parameters.Add($"Code{i}", question.Code);
                    cmd2.Parameters.Add($"Content{i}", question.Content);
                    cmd2.Parameters.Add($"GroupId{i}", question.GroupId);
                    cmd2.Parameters.Add($"NoShuffleChoice{i}", question.NoShuffleChoice);

                    i++;
                }
                cmd2.CommandText = commandText;
                cmd2.ExecuteNonQuery();

                var choices = qsuite.Questions.SelectMany(q => ((MultipleChoiceQuestionWithOneCorrectAnswer)q).Choices.Select(c =>
                {
                    return new TheS.ExamBank.DataFormats.SelectableChoiceWithId
                    {
                        _id = $"{q._id}-{c.Code}",
                        Code = c.Code,
                        Content = c.Content,
                        IsCorrectAnswer = c.IsCorrectAnswer,
                        QuestionId = q._id,
                    };
                })).ToList();
                var cmd3 = con.CreateCommand();
                commandText = $"REPLACE INTO {OracleDbTableName.Choice} VALUES ";
                i = 0;
                foreach (SelectableChoiceWithId choice in choices)
                {
                    commandText += $"(:Id{i},:Code{i},:Content{i},:IsCorrectAnswer{i})";
                    if (i < qsuite.Questions.Count()) commandText += ",";

                    cmd3.Parameters.Add($"Id{i}", choice._id);
                    cmd3.Parameters.Add($"Code{i}", choice.Code);
                    cmd3.Parameters.Add($"Content{i}", choice.Content);
                    cmd3.Parameters.Add($"IsCorrectAnswer{i}", choice.IsCorrectAnswer);

                    i++;
                }
                cmd3.CommandText = commandText;
                cmd3.ExecuteNonQuery();
            }
        }

        public void UpsertInActiveSubject(InactiveSubject InactiveSubject)
        {
            throw new NotImplementedException();
        }
    }
}
