using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;

namespace TheS.ExamBank.Parsers
{
    public class MarkdownParser : IMarkdownParser
    {
        public const int LimitLineCount2Look4Code = 9;

        public ParsedHeader ParseHeader(Stream mdStream)
        {
            var lineCount = 0;
            var sb = new StringBuilder();

            using (var rdr = new StreamReader(mdStream, System.Text.Encoding.UTF8, true))
            {
                var line = rdr.ReadLine();
                while (line != null)
                {
                    ++lineCount;

                    sb.AppendLine(line);

                    if (lineCount >= LimitLineCount2Look4Code) // assume limit should be reasonable
                    {
                        break;
                    }
                    // read next line
                    line = rdr.ReadLine();
                }
            }

            return ParseHeaderFromString(sb.ToString());
        }

        public ParsedHeader ParseHeaderFromString(string markdown)
        {
            var rgxCode = ParserUtil.RegexPatterns.MdCode;
            var matCode = rgxCode.Match(markdown);
            if (matCode.Success && matCode.Length > 0)
            {
                var code = matCode.Groups["code"].Value;
                var header = new ParsedHeader
                {
                    Code = code,
                    CodeFound = true,
                };

                try
                {
                    var codeComp = ParserUtil.ParseCode(code);

                    header.SubjectCode = codeComp.SubjectCode;
                    header.Level = codeComp.Level;
                    header.LayoutCode = codeComp.LayoutCode;
                }
                catch (Exception)
                {
                    return new ParsedHeader
                    {
                        Code = code,
                        CodeFound = false,
                    };
                }
                var matTitle = ParserUtil.RegexPatterns.MdTitle.Match(markdown);
                if (matTitle.Success)
                {
                    var title = matTitle.Groups["title"].Value;
                    header.Title = string.IsNullOrEmpty(title) ? title : title.Trim();
                }
                var matSubject = ParserUtil.RegexPatterns.MdSubject.Match(markdown);
                if (matSubject.Success)
                {
                    var subj = matSubject.Groups["subject"].Value;
                    header.SubjectName = string.IsNullOrEmpty(subj) ? subj : subj.Trim();
                }
                var matDesc = ParserUtil.RegexPatterns.MdDesc.Match(markdown);
                if (matDesc.Success)
                {
                    var desc = matDesc.Groups["desc"].Value;
                    header.Description = string.IsNullOrEmpty(desc) ? desc : desc.Trim();
                }

                using (var md = new StringReader(markdown))
                {
                    var rgxQ = ParserUtil.RegexPatterns.MdQ;
                    var line = md.ReadLine();
                    var lineCount = 0;

                    while (line != null)
                    {
                        if (rgxQ.IsMatch(line))
                        {
                            header.HeaderLineCount = lineCount;
                            break;
                        }
                        ++lineCount;
                        line = md.ReadLine();
                    }
                }

                return header;
            }
            else
            {
                return new ParsedHeader
                {
                    CodeFound = false,
                };
            }
        }

        public bool CheckAssets(string markdown, string[] assets)
        {
            //var links = GetAssetLinks(markdown).ToArray();
            //return links.All(lnk => assets.Any(it => it.EndsWith(lnk)));
            var links = GetAssetLinks(markdown).Select(x => x.ToLower()).ToArray();
            var tmpAssets = assets.Where(x => !x.EndsWith("/")).ToList();
            var bb = links.All(lnk => tmpAssets.Any(it => it.ToLower().EndsWith(lnk.ToLower())));
            return bb;
        }

        public IEnumerable<string> GetAssetLinks(string markdown)
        {
            var assets = new List<string>(20);  // init with capacity for 20 items
            var rgxQ = ParserUtil.RegexPatterns.MdQ;
            var rgxC = ParserUtil.RegexPatterns.MdC;
            var rgxImgLink = ParserUtil.RegexPatterns.MdImgLink;

            using (var rdr = new StringReader(markdown))
            {
                var line = rdr.ReadLine();
                while (line != null)
                {
                    if (!rgxQ.Match(line).Success && !rgxC.Match(line).Success)
                    {
                        var matImg = rgxImgLink.Match(line);
                        while (matImg.Success)
                        {
                            var link = matImg.Groups["link"].Value;
                            assets.Add(link);
                            matImg = matImg.NextMatch();
                        }
                    }

                    line = rdr.ReadLine();
                }
            }

            return assets;
        }

        public IEnumerable<Question> ParseQuestionsFromString(string markdown)
        {
            List<Question> questions = new List<Question>(20); // default capacity at 20 questions
            var rgxQ = ParserUtil.RegexPatterns.MdQ;
            var rgxC = ParserUtil.RegexPatterns.MdC;
            var rgxGOpen = ParserUtil.RegexPatterns.MdGOpen;
            var rgxGClose = ParserUtil.RegexPatterns.MdGClose;

            var matGOpen = rgxGOpen.Match(markdown);
            var matGClose = rgxGClose.Match(markdown);
            var gindex = -1;
            var groupNo = 0;
            if (matGOpen.Success && matGClose.Success)
            {
                gindex = matGOpen.Index;
            }

            var matQ = rgxQ.Match(markdown);
            while (matQ.Success)
            {
                if (gindex >= 0 && matQ.Index > gindex)
                {
                    // found grouping G()
                    // TODO: Current implementation do nothing with all/some -- n variable
                    ParseGroup(markdown, questions, ref matGOpen, ref matGClose, ref gindex, ref groupNo, ref matQ);
                    continue;
                }
                List<Asset> assets = new List<Asset>(7); // default capacity at 7 assets
                var cindex = 0;
                var startQIndex = matQ.Index + matQ.Length;
                var md4c = markdown.Substring(startQIndex);
                var matC = rgxC.Match(md4c);

                var q = new MultipleChoiceQuestionWithOneCorrectAnswer
                {
                    _id = Guid.NewGuid().ToString(),
                    No = int.Parse(matQ.Groups["qno"].Value),
                    NoShuffleChoice = !string.IsNullOrWhiteSpace(matQ.Groups["norandom"].Value),
                };

                matQ = matQ.NextMatch();
                var lastQ = !matQ.Success;
                var choices = new List<SelectableChoice>(5);    // defualt capacity is 5 choices
                var nextQIndex = lastQ ? md4c.Length : matQ.Index - startQIndex;
                var lastCIndex = nextQIndex;
                while (matC.Success && matC.Index < nextQIndex)
                {
                    if (q.Content == null)
                    {
                        q.Content = md4c.Substring(0, matC.Index).Trim();
                    }

                    var c = new SelectableChoice
                    {
                        IsCorrectAnswer = !string.IsNullOrWhiteSpace(matC.Groups["correct"].Value),
                    };
                    var startCIndex = matC.Index + matC.Length;
                    matC = matC.NextMatch();
                    if (matC.Success)
                    {
                        if (matC.Index < nextQIndex)
                        {
                            c.Content = md4c.Substring(startCIndex, matC.Index - startCIndex);
                        }
                        else
                        {
                            c.Content = md4c.Substring(startCIndex, nextQIndex - startCIndex);
                        }
                    }
                    else if (lastQ)
                    {
                        c.Content = md4c.Substring(startCIndex);
                        lastCIndex = md4c.Length;
                    }
                    c.Content = ParserUtil.TrimNewLines(c.Content);
                    var replace = ParserUtil.ReplaceAssets(c.Content);
                    c.Content = replace.Content;
                    c.Code = ((char)(Convert.ToInt32('a') + cindex)).ToString();
                    var apt2 = ++cindex;
                    foreach (var ast in replace.ReplacedAssets)
                    {
                        ast.ApplyTo = apt2;
                    }
                    assets.AddRange(replace.ReplacedAssets);
                    choices.Add(c);
                }
                q.Content = ParserUtil.TrimNewLines(q.Content);
                q.Choices = choices.ToArray();

                var replaceQ = ParserUtil.ReplaceAssets(q.Content);
                q.Content = replaceQ.Content;
                foreach (var ast in replaceQ.ReplacedAssets)
                {
                    ast.ApplyTo = 0;
                }
                assets.AddRange(replaceQ.ReplacedAssets);
                q.Assets = assets;

                questions.Add(q);
            }

            return questions.ToArray();
        }

        private void ParseGroup(string markdown, List<Question> questions, ref System.Text.RegularExpressions.Match matGOpen, ref System.Text.RegularExpressions.Match matGClose, ref int gindex, ref int groupNo, ref System.Text.RegularExpressions.Match matQ)
        {
            var openGroupIndex = gindex + matGOpen.Length;
            var closeGroupIndex = matGClose.Index;
            var mdGrouped = markdown.Substring(openGroupIndex, closeGroupIndex - openGroupIndex);
            var grouped = ParseQuestionsFromString(mdGrouped);
            var gno = ++groupNo;
            var gid = gno.ToString();
            foreach (var gq in grouped)
            {
                gq.GroupId = gid;
            }
            questions.AddRange(grouped);
            while (matGOpen.Success && matGOpen.Index < closeGroupIndex)
            {
                matGOpen = matGOpen.NextMatch();
            }
            if (matGOpen.Success)
            {
                matGClose = matGClose.NextMatch();
                if (matGOpen.Success && matGClose.Success)
                {
                    gindex = matGOpen.Index;
                }
            }
            else
            {
                gindex = -1;
            }
            while (matQ.Success && matQ.Index < closeGroupIndex)
            {
                matQ = matQ.NextMatch();
            }
        }
    }

    public class ParsedHeader
    {
        public bool CodeFound { get; set; }
        public int HeaderLineCount { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Level { get; set; }
        public string LayoutCode { get; set; }
        public string Description { get; set; }
    }
}
