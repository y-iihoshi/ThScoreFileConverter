//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th15;

// %T15CHARAEX[w][x][yy][z]
internal sealed class CharaExReplacer : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[1-3]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CHARAEX({Parsers.GameModeParser.Pattern})({Parsers.LevelWithTotalParser.Pattern})({Parsers.CharaWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public CharaExReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var mode = Parsers.GameModeParser.Parse(match.Groups[1]);
            var level = Parsers.LevelWithTotalParser.Parse(match.Groups[2]);
            var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3]);
            var type = TypeParser.Parse(match.Groups[4]);

            Func<IClearDataPerGameMode, long> getValueByType = (level, type) switch
            {
                (_, 1) => clearData => clearData.TotalPlayCount,
                (_, 2) => clearData => clearData.PlayTime,
                _ when Models.Definitions.IsTotal(level) => clearData => clearData.ClearCounts
                    .Where(pair => !Models.Definitions.IsTotal(pair.Key)).Sum(pair => pair.Value),
                _ => clearData => clearData.ClearCounts.TryGetValue(level, out var count) ? count : default,
            };

#pragma warning disable IDE0072 // Add missing cases to switch expression
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
#pragma warning restore IDE0072 // Add missing cases to switch expression

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
