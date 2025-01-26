//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th06;

// %T06C[xx][y]
internal sealed class CareerReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly IntegerParser CardNumberParser = new(@"\d{2}");
    private static readonly IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create($"{Definitions.FormatPrefix}C({CardNumberParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var number = CardNumberParser.Parse(match.Groups[1]);
        var type = TypeParser.Parse(match.Groups[2]);

        Func<ICardAttack, int> getCount = type switch
        {
            1 => attack => attack.ClearCount,
            _ => attack => attack.TrialCount,
        };

        if (number == 0)
        {
            return formatter.FormatNumber(cardAttacks.Values.Sum(getCount));
        }
        else if (Definitions.CardTable.ContainsKey(number))
        {
            return formatter.FormatNumber(
                cardAttacks.TryGetValue(number, out var attack) ? getCount(attack) : default);
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
