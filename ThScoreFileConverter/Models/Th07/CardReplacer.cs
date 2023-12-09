//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th07;

namespace ThScoreFileConverter.Models.Th07;

// %T07CARD[xxx][y]
internal sealed class CardReplacer : Th06.CardReplacerBase<Stage, Level>
{
    public CardReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, bool hideUntriedCards)
        : base(
              Definitions.FormatPrefix,
              Definitions.CardTable,
              hideUntriedCards,
              cardNumber => CardHasTried(cardAttacks, cardNumber))
    {
    }

    private static bool CardHasTried(IReadOnlyDictionary<int, ICardAttack> cardAttacks, int cardNumber)
    {
        return cardAttacks.TryGetValue(cardNumber, out var attack) && attack.HasTried;
    }
}
