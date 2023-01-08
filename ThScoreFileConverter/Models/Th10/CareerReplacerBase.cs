﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th10;

internal class CareerReplacerBase<TCharaWithTotal, TStage, TLevel> : IStringReplaceable
    where TCharaWithTotal : struct, Enum
    where TStage : struct, Enum
    where TLevel : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CareerReplacerBase(
        string formatPrefix,
        EnumShortNameParser<TCharaWithTotal> charaWithTotalParser,
        IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> clearDataDictionary,
        IReadOnlyDictionary<int, SpellCardInfo<TStage, TLevel>> cardTable,
        INumberFormatter formatter)
    {
        this.pattern = StringHelper.Create($@"{formatPrefix}C(\d{{3}})({charaWithTotalParser.Pattern})([12])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = IntegerHelper.Parse(match.Groups[1].Value);
            var chara = charaWithTotalParser.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            Func<ISpellCard<Level>, int> getCount = type switch
            {
                1 => card => card.ClearCount,
                _ => card => card.TrialCount,
            };

            var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.Cards : ImmutableDictionary<int, ISpellCard<Level>>.Empty;
            if (number == 0)
            {
                return formatter.FormatNumber(cards.Values.Sum(getCount));
            }
            else if (cardTable.ContainsKey(number))
            {
                return formatter.FormatNumber(cards.TryGetValue(number, out var card) ? getCount(card) : default);
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
