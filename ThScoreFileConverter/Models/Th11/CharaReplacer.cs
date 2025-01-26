//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th11;

namespace ThScoreFileConverter.Models.Th11;

// %T11CHARA[xx][y]
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
