//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th175
{
    // %T175CHR[xx][y]
    internal class CharaReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CHR({1})([1-4])", Definitions.FormatPrefix, Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CharaReplacer(
            IReadOnlyDictionary<Chara, int> useCounts,
            IReadOnlyDictionary<Chara, int> retireCounts,
            IReadOnlyDictionary<Chara, int> clearCounts,
            IReadOnlyDictionary<Chara, int> perfectClearCounts,
            INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                static int GetCount(IReadOnlyDictionary<Chara, int> dictionary, CharaWithTotal chara)
                {
                    return (chara == CharaWithTotal.Total)
                        ? dictionary.Values.Sum()
                        : (dictionary.TryGetValue((Chara)chara, out var count) ? count : default);
                }

                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[1].Value);
                var type = IntegerHelper.Parse(match.Groups[2].Value);

                return type switch
                {
                    1 => formatter.FormatNumber(GetCount(useCounts, chara)),
                    2 => formatter.FormatNumber(GetCount(retireCounts, chara)),
                    3 => formatter.FormatNumber(GetCount(clearCounts, chara)),
                    4 => formatter.FormatNumber(GetCount(perfectClearCounts, chara)),
                    _ => match.ToString(),  // unreachable
                };
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
