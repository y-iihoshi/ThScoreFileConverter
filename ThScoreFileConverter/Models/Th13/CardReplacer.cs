//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Extensions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th13
{
    // %T13CARD[xxx][y]
    internal class CardReplacer : Th10.CardReplacerBase<StagePractice, LevelPractice>
    {
        public CardReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
            : base(
                  Definitions.FormatPrefix,
                  Definitions.CardTable,
                  hideUntriedCards,
                  cardNumber => CardHasTried(clearDataDictionary, cardNumber),
                  static level => LevelToString(level))
        {
        }

        private static bool CardHasTried(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, int cardNumber)
        {
            return clearDataDictionary.TryGetValue(CharaWithTotal.Total, out var clearData)
                && clearData.Cards.TryGetValue(cardNumber, out var card)
                && card.HasTried;
        }

        private static string LevelToString(LevelPractice level)
        {
            var levelName = level.ToLongName();
            return (levelName.Length > 0) ? levelName : level.ToString();
        }
    }
}
