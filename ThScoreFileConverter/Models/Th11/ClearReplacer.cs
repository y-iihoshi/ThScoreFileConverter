//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th11;

namespace ThScoreFileConverter.Models.Th11;

// %T11CLEAR[x][yy]
internal sealed class ClearReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal>> clearDataDictionary)
    : Th10.ClearReplacerBase<Chara, CharaWithTotal>(
        Definitions.FormatPrefix, Parsers.LevelParser, Parsers.CharaParser, clearDataDictionary)
{
}
