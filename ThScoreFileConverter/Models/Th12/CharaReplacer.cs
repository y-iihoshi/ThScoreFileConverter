//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th12;

namespace ThScoreFileConverter.Models.Th12;

// %T12CHARA[xx][y]
internal sealed class CharaReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal>> clearDataDictionary,
    INumberFormatter formatter)
    : Th10.CharaReplacerBase<CharaWithTotal>(
        Definitions.FormatPrefix,
        Parsers.CharaWithTotalParser,
        Definitions.IsTotal,
        clearDataDictionary,
        formatter)
{
}
