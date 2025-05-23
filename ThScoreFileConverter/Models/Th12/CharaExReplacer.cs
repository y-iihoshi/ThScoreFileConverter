﻿//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th12;

namespace ThScoreFileConverter.Models.Th12;

// %T12CHARAEX[x][yy][z]
internal sealed class CharaExReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal>> clearDataDictionary,
    INumberFormatter formatter)
    : Th10.CharaExReplacerBase<CharaWithTotal>(
        Definitions.FormatPrefix,
        Parsers.LevelWithTotalParser,
        Parsers.CharaWithTotalParser,
        Models.Definitions.IsTotal,
        Definitions.IsTotal,
        clearDataDictionary,
        formatter)
{
}
