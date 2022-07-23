//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

// %T105CRG[x][yy][z]
internal class CollectRateReplacer : CollectRateReplacerBase<Chara>
{
    public CollectRateReplacer(
        IReadOnlyDictionary<Chara, IClearData<Chara>> clearDataDictionary, INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelWithTotalParser,
              Parsers.CharaParser,
              static (level, chara, type) => true,
              clearDataDictionary,
              formatter)
    {
    }
}
