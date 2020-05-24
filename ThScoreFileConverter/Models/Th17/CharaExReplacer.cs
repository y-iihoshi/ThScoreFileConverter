//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th17
{
    // %T17CHARAEX[x][yy][z]
    internal class CharaExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T17CHARAEX({0})({1})([1-3])",
            Parsers.LevelWithTotalParser.Pattern,
            Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CharaExReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

#pragma warning disable IDE0007 // Use implicit type
                Func<IClearData, long> getValueByType = (level, type) switch
                {
                    (_, 1) => clearData => clearData.TotalPlayCount,
                    (_, 2) => clearData => clearData.PlayTime,
                    (LevelWithTotal.Total, _) => clearData => clearData.ClearCounts.Values.Sum(),
                    _ => clearData => clearData.ClearCounts.TryGetValue(level, out var count) ? count : default,
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
                    _ => Utils.ToNumberString,
                };
#pragma warning restore IDE0007 // Use implicit type

                return toString(getValueByChara(clearDataDictionary));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
