//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th123;

namespace ThScoreFileConverter.Models.Th123;

// %T123CRG[x][yy][z]
internal class CollectRateReplacer : Th105.CollectRateReplacerBase<Chara>
{
    public CollectRateReplacer(
        IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary, INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelWithTotalParser,
              Parsers.CharaParser,
              static (level, chara, type) => Definitions.HasStory(chara),
              clearDataDictionary,
              formatter)
    {
    }
}
