//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th13;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th13.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th13;

// %T13CARD[xxx][y]
internal sealed class CardReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
    : Th10.CardReplacerBase<StagePractice, LevelPractice>(
        Definitions.FormatPrefix,
        Definitions.CardTable,
        hideUntriedCards,
        cardNumber => CardHasTried(clearDataDictionary, cardNumber),
        static level => level.ToDisplayName())
{
    private static bool CardHasTried(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, int cardNumber)
    {
        return clearDataDictionary.TryGetValue(CharaWithTotal.Total, out var clearData)
            && clearData.Cards.TryGetValue(cardNumber, out var card)
            && card.HasTried;
    }
}
