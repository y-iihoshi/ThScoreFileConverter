//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th11;

namespace ThScoreFileConverter.Models.Th11;

// %T11CRG[w][xx][y][z]
internal sealed class CollectRateReplacer : Th10.CollectRateReplacerBase<CharaWithTotal>
{
    public CollectRateReplacer(
        IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelWithTotalParser,
              Parsers.CharaWithTotalParser,
              Parsers.StageWithTotalParser,
              Definitions.CardTable,
              clearDataDictionary,
              formatter)
    {
    }
}
