//-----------------------------------------------------------------------
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
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th14;

internal class CareerReplacerBase<
    TGameMode, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData> : IStringReplaceable
    where TGameMode : struct, Enum
    where TChWithT : struct, Enum
    where TLv : struct, Enum
    where TLvPrac : struct, Enum
    where TLvPracWithT : struct, Enum
    where TStPrac : struct, Enum
    where TScoreData : IScoreData
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CareerReplacerBase(
        string formatPrefix,
        EnumShortNameParser<TGameMode> gameModeParser,
        EnumShortNameParser<TChWithT> charaWithTotalParser,
        IEnumerable<int> validCardNumbers,
        IReadOnlyDictionary<TChWithT, Th13.IClearData<
            TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>> clearDataDictionary,
        INumberFormatter formatter)
    {
        var numDigits = IntegerHelper.GetNumDigits(validCardNumbers.Count());

        this.pattern = StringHelper.Create(
            $@"{formatPrefix}C({gameModeParser.Pattern})(\d{{{numDigits}}})({charaWithTotalParser.Pattern})([12])");
        this.evaluator = new MatchEvaluator(match =>
        {
            var mode = gameModeParser.Parse(match.Groups[1].Value);
            var number = IntegerHelper.Parse(match.Groups[2].Value);
            var chara = charaWithTotalParser.Parse(match.Groups[3].Value);
            var type = IntegerHelper.Parse(match.Groups[4].Value);

            Func<Th13.ISpellCard<TLv>, int> getCount = (mode, type) switch
            {
                (GameMode.Story, 1) => card => card.ClearCount,
                (GameMode.Story, _) => card => card.TrialCount,
                (_, 1) => card => card.PracticeClearCount,
                _ => card => card.PracticeTrialCount,
            };

            var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.Cards : ImmutableDictionary<int, Th13.ISpellCard<TLv>>.Empty;
            if (number == 0)
            {
                return formatter.FormatNumber(cards.Values.Sum(getCount));
            }
            else if (validCardNumbers.Contains(number))
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
