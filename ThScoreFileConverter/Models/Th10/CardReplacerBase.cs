﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th10;

internal class CardReplacerBase<TStage, TLevel> : IStringReplaceable
    where TStage : struct, Enum
    where TLevel : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CardReplacerBase(
        string formatPrefix,
        IReadOnlyDictionary<int, SpellCardInfo<TStage, TLevel>> cardTable,
        bool hideUntriedCards,
        Func<int, bool> cardHasTried)
        : this(formatPrefix, cardTable, hideUntriedCards, cardHasTried, static level => level.ToName())
    {
    }

    protected CardReplacerBase(
        string formatPrefix,
        IReadOnlyDictionary<int, SpellCardInfo<TStage, TLevel>> cardTable,
        bool hideUntriedCards,
        Func<int, bool> cardHasTried,
        Func<TLevel, string> levelToString)
    {
        var numDigits = IntegerHelper.GetNumDigits(cardTable.Count);
        var cardNumberParser = new IntegerParser($@"\d{{{numDigits}}}");

        this.pattern = StringHelper.Create($"{formatPrefix}CARD({cardNumberParser.Pattern})([NR])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = cardNumberParser.Parse(match.Groups[1]);
            var type = match.Groups[2].Value.ToUpperInvariant();

            if (cardTable.TryGetValue(number, out var cardInfo))
            {
                if (type == "N")
                {
                    return hideUntriedCards && !cardHasTried(number) ? "??????????" : cardInfo.Name;
                }
                else
                {
                    return levelToString(cardInfo.Level);
                }
            }
            else
            {
                return match.ToString();
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
