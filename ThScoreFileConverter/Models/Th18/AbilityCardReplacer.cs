//-----------------------------------------------------------------------
// <copyright file="AbilityCardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th18;

// %T18ABIL[xx]
internal sealed class AbilityCardReplacer(IAbilityCardHolder holder) : IStringReplaceable
{
    private static readonly IntegerParser NumberParser = new(@"\d{2}");
    private static readonly string Pattern = StringHelper.Create($@"{Definitions.FormatPrefix}ABIL({NumberParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var number = NumberParser.Parse(match.Groups[1]);

        if (!Definitions.AbilityCardTable.TryGetValue(number - 1, out var card))
            return match.ToString();

        return (holder.AbilityCards.ElementAt(card.Id) > 0) ? card.Name : "??????????";
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
