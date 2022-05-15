//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10;

// %T10CHARA[xx][y]
internal class CharaReplacer : CharaReplacerBase<CharaWithTotal>
{
    public CharaReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.CharaWithTotalParser,
              Definitions.IsTotal,
              clearDataDictionary,
              formatter)
    {
    }
}
