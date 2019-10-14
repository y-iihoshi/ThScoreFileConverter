//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th09.Parsers;

namespace ThScoreFileConverter.Models.Th09
{
    // %T09CLEAR[x][yy][z]
    internal class ClearReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T09CLEAR({0})({1})([12])", LevelParser.Pattern, CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearReplacer(
            IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> rankings,
            IReadOnlyDictionary<Chara, IClearCount> clearCounts)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var count = clearCounts[chara].Counts[level];

                if (type == 1)
                {
                    return Utils.ToNumberString(count);
                }
                else
                {
                    if (count > 0)
                    {
                        return "Cleared";
                    }
                    else
                    {
                        var score = rankings[(chara, level)][0];
                        var date = Encoding.Default.GetString(score.Date.ToArray()).TrimEnd('\0');
                        return (date != "--/--") ? "Not Cleared" : "-------";
                    }
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
