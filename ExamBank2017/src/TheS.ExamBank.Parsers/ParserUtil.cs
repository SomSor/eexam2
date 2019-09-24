using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheS.ExamBank.DataFormats;

namespace TheS.ExamBank.Parsers
{
    public static class ParserUtil
    {
        public class RegXPatterns
        {
            internal RegXPatterns() { }
            public Regex CodeComponents { get { return new Regex(@"(?<subjCode>\w+)L(?<level>\d+)-(?<layout>[\S]+)\s*"); } }
            public Regex MdCode { get { return new Regex(@"<!--!#@\s*[Cc]ode\s*:\s*(?<code>[\S]+)\s*-->"); } }
            public Regex MdTitle { get { return new Regex(@"<!--!#@\s*[Tt]itle\s*:\s*(?<title>[\w\s\n\./\?\-)(+*#@%,;&!=]+)-->"); } }
            public Regex MdSubject { get { return new Regex(@"<!--!#@\s*[Ss]ubject\s*:\s*(?<subject>[\w\s\n\./\?\-)(+*#@%,;&!=]+)-->"); } }
            public Regex MdDesc { get { return new Regex(@"<!--!#@\s*[Dd]esc\s*:\s*(?<desc>[\w\s\n\./\?\-)(+*#@%,;&!=]+)-->"); } }
            public Regex MdQ { get { return new Regex(@"<!--!#@\s*Q(?<qno>\d+)\s*(?<norandom>[Nn][Oo]\s+[Rr]andom)?\s*-->"); } }
            public Regex MdC { get { return new Regex(@"<!--!#@\s*C(?<correct>[*]*)\s*-->"); } }
            public Regex MdImgLink { get { return new Regex(@"\!\[(?<caption>[^\]]*)\]\((?<link>[^)]+)\)"); } }
            public Regex MdGOpen { get { return new Regex(@"<!--!#@\s*G\(\s*(?<n>all)\s*-->"); } }
            public Regex MdGClose { get { return new Regex(@"<!--!#@\s*G\)\s*-->"); } }
            // TODO: Add Regex for parse question no ref
            //[42](q:42)
        }

        public static readonly RegXPatterns RegexPatterns = new ParserUtil.RegXPatterns();

        public static CodeComponent ParseCode(string code)
        {
            var rx = RegexPatterns.CodeComponents;
            var m = rx.Match(code);
            if (m.Success && m.Length > 0)
            {
                var subjCode = m.Groups["subjCode"].Value;
                var level = m.Groups["level"].Value;
                var layout = m.Groups["layout"].Value;
                int nLevel;
                if ( ! int.TryParse(level, out nLevel))
                {
                    // has no level, use default -- 0
                    nLevel = 0;
                }
                return new CodeComponent
                {
                    SubjectCode = subjCode,
                    Level = nLevel,
                    LayoutCode = layout,
                };
            } else
            {
                throw new ArgumentException("The parameter's format is invalid.", "code");
            }
        }

        public static string TrimNewLines(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            return content.Trim('\r', '\n', ' ');
        }

        public static AssetReplacedContent ReplaceAssets(string content)
        {
            var rgxLink = RegexPatterns.MdImgLink;
            var mLink = rgxLink.Match(content);
            List<ReplacedAsset> replaces = new List<ReplacedAsset>(7); // default capacity is 7 assets

            while (mLink.Success)
            {
                var link = mLink.Groups["link"];
                replaces.Add(new ReplacedAsset
                {
                    Asset = link.Value,
                    Position = link.Index,
                });
                content = rgxLink.Replace(content, string.Format(
                    "![{0}]()", mLink.Groups["caption"].Value), 1);
                mLink = rgxLink.Match(content);
            }

            return new AssetReplacedContent
            {
                Content = content,
                ReplacedAssets = ConvertToAssets(replaces),
            };
        }

        public static IEnumerable<Asset> ConvertToAssets(IEnumerable<ReplacedAsset> assets)
        {
            var qry = (from it in assets
                       group it.Position by it.Asset into grp
                       select new Asset
                       {
                           Resource = grp.Key,
                           Positions = grp.ToArray(),
                       }).ToList();
            return qry;
        }

        public class AssetReplacedContent
        {
            public string Content { get; set; }
            public IEnumerable<Asset> ReplacedAssets { get; set; }
        }

        public class ReplacedAsset
        {
            public string Asset { get; set; }
            public int Position { get; set; }
        }

        public class CodeComponent
        {
            public string SubjectCode { get; set; }
            public int Level { get; set; }
            public string LayoutCode { get; set; }
        }
    }
}
