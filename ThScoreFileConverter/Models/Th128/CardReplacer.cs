//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using Stage = ThScoreFileConverter.Core.Models.Th128.Stage;

namespace ThScoreFileConverter.Models.Th128;

// %T128CARD[xxx][y]
internal sealed class CardReplacer(IReadOnlyDictionary<int, ISpellCard> spellCards, bool hideUntriedCards)
    : Th10.CardReplacerBase<Stage, Level>(
        Definitions.FormatPrefix,
        Definitions.CardTable,
        hideUntriedCards,
        cardNumber => CardHasTried(spellCards, cardNumber))
{
    private static bool CardHasTried(IReadOnlyDictionary<int, ISpellCard> spellCards, int cardNumber)
    {
        return spellCards.TryGetValue(cardNumber, out var card) && card.HasTried;
    }
}
