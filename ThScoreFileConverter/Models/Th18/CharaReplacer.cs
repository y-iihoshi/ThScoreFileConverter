//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th18
{
    // %T18CHARA[xx][y]
    internal class CharaReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CHARA({1})([1-3])", Definitions.FormatPrefix, Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CharaReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[1].Value);
                var type = IntegerHelper.Parse(match.Groups[2].Value);

                Func<IClearData, long> getValueByType = type switch
                {
                    1 => clearData => clearData.TotalPlayCount,
                    2 => clearData => clearData.PlayTime,
                    _ => clearData => clearData.ClearCounts
                        .Where(pair => (pair.Key != Th14.LevelPracticeWithTotal.NotUsed)
                            && (pair.Key != Th14.LevelPracticeWithTotal.Total))
                        .Sum(pair => pair.Value),
                };

                Func<IReadOnlyDictionary<CharaWithTotal, IClearData>, long> getValueByChara = chara switch
                {
                    CharaWithTotal.Total => dictionary => dictionary.Values
                        .Where(clearData => clearData.Chara != chara).Sum(getValueByType),
                    _ => dictionary => dictionary.TryGetValue(chara, out var clearData)
                        ? getValueByType(clearData) : default,
                };

                Func<long, string> toString = type switch
                {
                    2 => value => new Time(value * 10, false).ToString(),
                    _ => formatter.FormatNumber,
                };

                return toString(getValueByChara(clearDataDictionary));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
