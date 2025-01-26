//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

// %T105CARD[xxx][yy][z]
internal sealed class CardReplacer(
    IReadOnlyDictionary<Chara, IClearData<Chara>> clearDataDictionary, bool hideUntriedCards)
    : CardReplacerBase<Chara>(
        Definitions.FormatPrefix,
        Parsers.CharaParser,
        Core.Models.Th105.Definitions.HasStory,
        Definitions.EnemyCardIdTable,
        Definitions.CardNameTable,
        clearDataDictionary,
        hideUntriedCards)
{
}
