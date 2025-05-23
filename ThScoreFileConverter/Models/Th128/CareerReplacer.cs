﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

// %T128C[xxx][z]
internal sealed class CareerReplacer(IReadOnlyDictionary<int, ISpellCard> spellCards, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser CardNumberParser = new(@"\d{3}");
    private static readonly IntegerParser TypeParser = new(@"[1-3]");
    private static readonly string Pattern = StringHelper.Create($"{Definitions.FormatPrefix}C({CardNumberParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var number = CardNumberParser.Parse(match.Groups[1]);
        var type = TypeParser.Parse(match.Groups[2]);

        Func<ISpellCard, int> getCount = type switch
        {
            1 => card => card.NoIceCount,
            2 => card => card.NoMissCount,
            _ => card => card.TrialCount,
        };

        if (number == 0)
        {
            return formatter.FormatNumber(spellCards.Values.Sum(getCount));
        }
        else if (Definitions.CardTable.ContainsKey(number))
        {
            return formatter.FormatNumber(
                spellCards.TryGetValue(number, out var card) ? getCount(card) : default);
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
