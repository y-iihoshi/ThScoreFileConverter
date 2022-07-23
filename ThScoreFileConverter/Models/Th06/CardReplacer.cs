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

namespace ThScoreFileConverter.Models.Th06;

// %T06CARD[xx][y]
internal class CardReplacer : CardReplacerBase<Stage, Level>
{
    public CardReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, bool hideUntriedCards)
        : base(
              Definitions.FormatPrefix,
              Definitions.CardTable,
              hideUntriedCards,
              cardNumber => CardHasTried(cardAttacks, cardNumber),
              static cardInfo => CardLevelToString(cardInfo))
    {
    }

    private static bool CardHasTried(IReadOnlyDictionary<int, ICardAttack> cardAttacks, int cardNumber)
    {
        return cardAttacks.TryGetValue(cardNumber, out var attack) && attack.HasTried;
    }

    private static string CardLevelToString(SpellCardInfo<Stage, Level> cardInfo)
    {
        return string.Join(", ", cardInfo.Levels.Select(static level => level.ToString()));
    }
}
