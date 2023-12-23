//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

// %T105C[xxx][yy][z]
internal sealed class CareerReplacer(
    IReadOnlyDictionary<Chara, IClearData<Chara>> clearDataDictionary, INumberFormatter formatter)
    : CareerReplacerBase<Chara>(
        Definitions.FormatPrefix,
        Parsers.CharaParser,
        static (number, chara, type) => true,
        Definitions.EnemyCardIdTable,
        clearDataDictionary,
        formatter)
{
}
