﻿//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th10;

namespace ThScoreFileConverter.Models.Th10;

// %T10CHARA[xx][y]
internal sealed class CharaReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
    INumberFormatter formatter)
    : CharaReplacerBase<CharaWithTotal>(
        Definitions.FormatPrefix,
        Parsers.CharaWithTotalParser,
        Definitions.IsTotal,
        clearDataDictionary,
        formatter)
{
}
