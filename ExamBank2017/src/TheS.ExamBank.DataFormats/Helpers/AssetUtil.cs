using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheS.ExamBank.DataFormats.Helpers
{
    public static class AssetUtil
    {
        public static Question ApplyAssets(Question question, IEnumerable<Asset> assets)
        {
            var qasset = (from it in assets
                          where it.ApplyTo == 0
                          from pos in it.Positions
                          select new { Position = pos, it.Resource })
                          .OrderByDescending(it => it.Position)
                          .ToArray();
            var content = question.Content ?? string.Empty;
            foreach (var ast in qasset)
            {
                content = content.Insert(ast.Position, ast.Resource);
            }
            var target = question.Clone();
            var qmulti = target as MultipleChoiceQuestion;
            if (qmulti != null)
            {
                var choices = qmulti.Choices.ToArray();

                for (int i = 0; i < choices.Length; i++)
                {
                    var casset = (from it in assets
                                  where it.ApplyTo - 1 == i
                                  from pos in it.Positions
                                  select new { Position = pos, it.Resource })
                                  .OrderByDescending(it => it.Position)
                                  .ToArray();
                    var cnt = choices[i].Content;
                    foreach (var ast in casset)
                    {
                        cnt = cnt.Insert(ast.Position, ast.Resource);
                    }
                    choices[i].Content = cnt;
                }
            }
            target.Content = content;

            return target;
        }
    }
}
