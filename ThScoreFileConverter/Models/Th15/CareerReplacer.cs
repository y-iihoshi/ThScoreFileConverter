//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th15;

// %T15C[w][xxx][yy][z]
internal sealed class CareerReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser CardNumberParser = new(@"\d{3}");
    private static readonly IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}C({Parsers.GameModeParser.Pattern})({CardNumberParser.Pattern})({Parsers.CharaWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var mode = Parsers.GameModeParser.Parse(match.Groups[1]);
        var number = CardNumberParser.Parse(match.Groups[2]);
        var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3]);
        var type = TypeParser.Parse(match.Groups[4]);

        Func<Th13.ISpellCard<Level>, int> getCount = type switch
        {
            1 => card => card.ClearCount,
            _ => card => card.TrialCount,
        };

        var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
            && clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
            ? clearDataPerGameMode.Cards : ImmutableDictionary<int, Th13.ISpellCard<Level>>.Empty;
        if (number == 0)
        {
            return formatter.FormatNumber(cards.Values.Sum(getCount));
        }
        else if (Definitions.CardTable.ContainsKey(number))
        {
            return formatter.FormatNumber(cards.TryGetValue(number, out var card) ? getCount(card) : default);
        }
        else
        {
            return match.ToString();
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
