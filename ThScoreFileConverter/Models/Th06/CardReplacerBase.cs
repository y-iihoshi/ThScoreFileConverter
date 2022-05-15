//-----------------------------------------------------------------------
// <copyright file="CardReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th06;

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
        : this(formatPrefix, cardTable, hideUntriedCards, cardHasTried, static cardInfo => cardInfo.Level.ToString())
    {
    }

    protected CardReplacerBase(
        string formatPrefix,
        IReadOnlyDictionary<int, SpellCardInfo<TStage, TLevel>> cardTable,
        bool hideUntriedCards,
        Func<int, bool> cardHasTried,
        Func<SpellCardInfo<TStage, TLevel>, string> cardLevelToString)
    {
        this.pattern = Utils.Format(
            @"{0}CARD(\d{{{1}}})([NR])", formatPrefix, IntegerHelper.GetNumDigits(cardTable.Count));
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = IntegerHelper.Parse(match.Groups[1].Value);
            var type = match.Groups[2].Value.ToUpperInvariant();

            if (cardTable.TryGetValue(number, out var cardInfo))
            {
                if (hideUntriedCards && !cardHasTried(number))
                    return (type == "N") ? "??????????" : "?????";

                return (type == "N") ? cardInfo.Name : cardLevelToString(cardInfo);
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
