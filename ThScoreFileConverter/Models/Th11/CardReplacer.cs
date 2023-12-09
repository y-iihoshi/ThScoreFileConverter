//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th11.CharaWithTotal>;

namespace ThScoreFileConverter.Models.Th11;

// %T11CARD[xxx][y]
internal sealed class CardReplacer : Th10.CardReplacerBase<Stage, Level>
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
