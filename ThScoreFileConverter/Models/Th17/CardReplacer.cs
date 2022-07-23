//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th17;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th17;

// %T17CARD[xxx][y]
internal class CardReplacer : Th10.CardReplacerBase<Stage, Level>
{
    public CardReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
        : base(
              Definitions.FormatPrefix,
              Definitions.CardTable,
              hideUntriedCards,
              cardNumber => CardHasTried(clearDataDictionary, cardNumber))
    {
    }

    private static bool CardHasTried(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, int cardNumber)
    {
        return clearDataDictionary.TryGetValue(CharaWithTotal.Total, out var clearData)
            && clearData.Cards.TryGetValue(cardNumber, out var card)
            && card.HasTried;
    }
}
