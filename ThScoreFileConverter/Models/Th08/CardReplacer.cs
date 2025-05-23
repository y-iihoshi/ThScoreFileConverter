﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;

namespace ThScoreFileConverter.Models.Th08;

// %T08CARD[xxx][y]
internal sealed class CardReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, bool hideUntriedCards)
    : Th06.CardReplacerBase<StagePractice, LevelPractice>(
        Definitions.FormatPrefix,
        Definitions.CardTable,
        hideUntriedCards,
        cardNumber => CardHasTried(cardAttacks, cardNumber),
        CardLevelToString)
{
    private static bool CardHasTried(IReadOnlyDictionary<int, ICardAttack> cardAttacks, int cardNumber)
    {
        return cardAttacks.TryGetValue(cardNumber, out var attack) && attack.HasTried;
    }

    private static string CardLevelToString(SpellCardInfo<StagePractice, LevelPractice> cardInfo)
    {
        return cardInfo.Level.ToDisplayName();
    }
}
