//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th10;

namespace ThScoreFileConverter.Models.Th10;

// %T10CLEAR[x][yy]
internal class ClearReplacer : ClearReplacerBase<Chara, CharaWithTotal>
{
    public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary)
        : base(Definitions.FormatPrefix, Parsers.LevelParser, Parsers.CharaParser, clearDataDictionary)
    {
    }
}
