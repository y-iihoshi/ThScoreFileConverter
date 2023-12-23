//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th10;

namespace ThScoreFileConverter.Models.Th10;

// %T10CHARAEX[x][yy][z]
internal sealed class CharaExReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
    INumberFormatter formatter)
    : CharaExReplacerBase<CharaWithTotal>(
        Definitions.FormatPrefix,
        Parsers.LevelWithTotalParser,
        Parsers.CharaWithTotalParser,
        Models.Definitions.IsTotal,
        Definitions.IsTotal,
        clearDataDictionary,
        formatter)
{
}
