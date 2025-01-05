//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th07;

// %T07C[xxx][yy][z]
internal sealed class CareerReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser CardNumberParser = new(@"\d{3}");
    private static readonly IntegerParser TypeParser = new(@"[1-3]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}C({CardNumberParser.Pattern})({Parsers.CharaWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var number = CardNumberParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2]);
        var type = TypeParser.Parse(match.Groups[3]);

        Func<ICardAttack, long> getValue = type switch
        {
            1 => attack => attack.MaxBonuses[chara],
            2 => attack => attack.ClearCounts[chara],
            _ => attack => attack.TrialCounts[chara],
        };

        if (number == 0)
        {
            return formatter.FormatNumber(cardAttacks.Values.Sum(getValue));
        }
        else if (Definitions.CardTable.ContainsKey(number))
        {
            return formatter.FormatNumber(
                cardAttacks.TryGetValue(number, out var attack) ? getValue(attack) : default);
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
