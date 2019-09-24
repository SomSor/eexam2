using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;

namespace TheS.ExamBank.Parsers
{
    public interface IMarkdownParser
    {
        bool CheckAssets(string markdown, string[] assets);
        IEnumerable<string> GetAssetLinks(string markdown);
        ParsedHeader ParseHeaderFromString(string markdown);
        IEnumerable<Question> ParseQuestionsFromString(string markdown);
    }
}
