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

namespace ThScoreFileConverter.Models.Th15
{
    // %T15CHARA[x][yy][z]
    internal class CharaReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CHARA({1})({2})([1-3])",
            Definitions.FormatPrefix,
            Parsers.GameModeParser.Pattern,
            Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CharaReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                Func<IClearDataPerGameMode, long> getValueByType = type switch
                {
                    1 => clearData => clearData.TotalPlayCount,
                    2 => clearData => clearData.PlayTime,
                    _ => clearData => clearData.ClearCounts
                        .Where(pair => !Models.Definitions.IsTotal(pair.Key)).Sum(pair => pair.Value),
                };

                Func<IReadOnlyDictionary<CharaWithTotal, IClearData>, long> getValueByChara = chara switch
                {
                    _ when Definitions.IsTotal(chara) => dictionary => dictionary.Values
                        .Where(clearData => !Definitions.IsTotal(clearData.Chara))
                        .Sum(clearData => clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
                            ? getValueByType(clearDataPerGameMode) : default),
                    _ => dictionary => dictionary.TryGetValue(chara, out var clearData)
                        && clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
                        ? getValueByType(clearDataPerGameMode) : default,
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
