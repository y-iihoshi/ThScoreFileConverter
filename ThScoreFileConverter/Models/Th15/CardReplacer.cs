//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;

namespace ThScoreFileConverter.Models.Th15;

// %T15CARD[xxx][y]
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
            && clearData.GameModeData.Any(
                data => data.Value.Cards.TryGetValue(cardNumber, out var card) && card.HasTried);
    }
}
