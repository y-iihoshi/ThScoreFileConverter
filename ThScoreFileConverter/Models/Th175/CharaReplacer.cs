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
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th175;

// %T175CHR[xx][y]
internal sealed class CharaReplacer(
    IReadOnlyDictionary<Chara, int> useCounts,
    IReadOnlyDictionary<Chara, int> retireCounts,
    IReadOnlyDictionary<Chara, int> clearCounts,
    IReadOnlyDictionary<Chara, int> perfectClearCounts,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[1-4]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CHR({Parsers.CharaWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        static int GetCount(IReadOnlyDictionary<Chara, int> dictionary, CharaWithTotal chara)
        {
            return (chara == CharaWithTotal.Total)
                ? dictionary.Values.Sum()
                : (dictionary.TryGetValue((Chara)chara, out var count) ? count : default);
        }

        var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[1]);
        var type = TypeParser.Parse(match.Groups[2]);

        return type switch
        {
            1 => formatter.FormatNumber(GetCount(useCounts, chara)),
            2 => formatter.FormatNumber(GetCount(retireCounts, chara)),
            3 => formatter.FormatNumber(GetCount(clearCounts, chara)),
            4 => formatter.FormatNumber(GetCount(perfectClearCounts, chara)),
            _ => match.ToString(),  // unreachable
        };
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
