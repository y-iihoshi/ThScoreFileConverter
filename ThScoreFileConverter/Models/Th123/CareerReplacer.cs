//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th123;

namespace ThScoreFileConverter.Models.Th123;

// %T123C[xx][yy][z]
internal class CareerReplacer : Th105.CareerReplacerBase<Chara>
{
    public CareerReplacer(
        IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary, INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.CharaParser,
              static (number, chara, type) => Core.Models.Th123.Definitions.HasStory(chara),
              Definitions.EnemyCardIdTable,
              clearDataDictionary,
              formatter)
    {
    }
}
