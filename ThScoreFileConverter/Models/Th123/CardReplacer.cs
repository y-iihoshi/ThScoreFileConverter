﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th123;

namespace ThScoreFileConverter.Models.Th123;

// %T123CARD[xx][yy][z]
internal sealed class CardReplacer(
    IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary, bool hideUntriedCards)
    : Th105.CardReplacerBase<Chara>(
        Definitions.FormatPrefix,
        Parsers.CharaParser,
        Core.Models.Th123.Definitions.HasStory,
        Definitions.EnemyCardIdTable,
        Definitions.CardNameTable,
        clearDataDictionary,
        hideUntriedCards)
{
}
