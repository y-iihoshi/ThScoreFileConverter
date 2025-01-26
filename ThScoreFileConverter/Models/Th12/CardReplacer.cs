//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th12.CharaWithTotal>;

namespace ThScoreFileConverter.Models.Th12;

// %T12CARD[xxx][y]
internal sealed class CardReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
    : Th10.CardReplacerBase<Stage, Level>(
        Definitions.FormatPrefix,
        Definitions.CardTable,
        hideUntriedCards,
        cardNumber => CardHasTried(clearDataDictionary, cardNumber))
{
    private static bool CardHasTried(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, int cardNumber)
    {
        return clearDataDictionary.TryGetValue(CharaWithTotal.Total, out var clearData)
            && clearData.Cards.TryGetValue(cardNumber, out var card)
            && card.HasTried;
    }
}
