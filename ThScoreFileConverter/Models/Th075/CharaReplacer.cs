//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

// %T75CHR[x][yy][z]
internal sealed class CharaReplacer(
    IReadOnlyDictionary<(CharaWithReserved Chara, Level Level), IClearData> clearData,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[1-4]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CHR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2]);
        var type = TypeParser.Parse(match.Groups[3]);

        if (chara == Chara.Meiling)
            return match.ToString();

        var data = clearData.TryGetValue(((CharaWithReserved)chara, level), out var value)
            ? value : new ClearData();
        return type switch
        {
            1 => formatter.FormatNumber(data.UseCount),
            2 => formatter.FormatNumber(data.ClearCount),
            3 => formatter.FormatNumber(data.MaxCombo),
            4 => formatter.FormatNumber(data.MaxDamage),
            _ => match.ToString(),  // unreachable
        };
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
