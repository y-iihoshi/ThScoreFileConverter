//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacerBase.cs" company="None">
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
using ThScoreFileConverter.Helpers;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th13
{
    internal class CollectRateReplacerBase<
        TGameMode, TChWithT, TLv, TLvWithT, TLvPrac, TLvPracWithT, TStPrac, TScoreData> : IStringReplaceable
        where TGameMode : struct, Enum
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvWithT : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
    {
        private readonly string pattern;
        private readonly MatchEvaluator evaluator;

        protected CollectRateReplacerBase(
            string formatPrefix,
            EnumShortNameParser<TGameMode> gameModeParser,
            EnumShortNameParser<TLvWithT> levelWithTotalParser,
            EnumShortNameParser<TChWithT> charaWithTotalParser,
            EnumShortNameParser<StageWithTotal> stageWithTotalParser,
            Func<TGameMode, TLvWithT, TChWithT, StageWithTotal, bool> canReplace,
            Func<TGameMode, int, Func<ISpellCard<TLv>, bool>> findCardByModeType,
            Func<TLvWithT, Func<ISpellCard<TLv>, bool>> findCardByLevel,
            Func<TLvWithT, StageWithTotal, Func<ISpellCard<TLv>, bool>> findCardByLevelStage,
            IReadOnlyDictionary<TChWithT, IClearData<
                TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>> clearDataDictionary,
            INumberFormatter formatter)
            : this(
                  formatPrefix,
                  gameModeParser,
                  levelWithTotalParser,
                  charaWithTotalParser,
                  stageWithTotalParser,
                  canReplace,
                  findCardByModeType,
                  findCardByLevel,
                  findCardByLevelStage,
                  (mode, chara) => GetSpellCards(clearDataDictionary, chara),
                  formatter)
        {
        }

        protected CollectRateReplacerBase(
            string formatPrefix,
            EnumShortNameParser<TGameMode> gameModeParser,
            EnumShortNameParser<TLvWithT> levelWithTotalParser,
            EnumShortNameParser<TChWithT> charaWithTotalParser,
            EnumShortNameParser<StageWithTotal> stageWithTotalParser,
            Func<TGameMode, TLvWithT, TChWithT, StageWithTotal, bool> canReplace,
            Func<TGameMode, int, Func<ISpellCard<TLv>, bool>> findCardByModeType,
            Func<TLvWithT, Func<ISpellCard<TLv>, bool>> findCardByLevel,
            Func<TLvWithT, StageWithTotal, Func<ISpellCard<TLv>, bool>> findCardByLevelStage,
            Func<TGameMode, TChWithT, IEnumerable<ISpellCard<TLv>>> getSpellCards,
            INumberFormatter formatter)
        {
            this.pattern = Utils.Format(
                @"{0}CRG({1})({2})({3})({4})([12])",
                formatPrefix,
                gameModeParser.Pattern,
                levelWithTotalParser.Pattern,
                charaWithTotalParser.Pattern,
                stageWithTotalParser.Pattern);
            this.evaluator = new MatchEvaluator(match =>
            {
                var mode = gameModeParser.Parse(match.Groups[1].Value);
                var level = levelWithTotalParser.Parse(match.Groups[2].Value);
                var chara = charaWithTotalParser.Parse(match.Groups[3].Value);
                var stage = stageWithTotalParser.Parse(match.Groups[4].Value);
                var type = IntegerHelper.Parse(match.Groups[5].Value);

                if (!canReplace(mode, level, chara, stage))
                    return match.ToString();

                var findByModeType = findCardByModeType(mode, type);
                var findByLevel = findCardByLevel(level);
                var findByLevelStage = findCardByLevelStage(level, stage);

                return formatter.FormatNumber(
                    getSpellCards(mode, chara).Count(
                        FuncHelper.MakeAndPredicate(findByModeType, findByLevel, findByLevelStage)));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        private static IEnumerable<ISpellCard<TLv>> GetSpellCards(
            IReadOnlyDictionary<TChWithT, IClearData<
                TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>> clearDataDictionary,
            TChWithT chara)
        {
            return clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.Cards.Values : ImmutableList<ISpellCard<TLv>>.Empty;
        }
    }
}
