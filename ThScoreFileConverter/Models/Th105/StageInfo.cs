//-----------------------------------------------------------------------
// <copyright file="StageInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

internal class StageInfo<TChara>
    where TChara : struct, Enum
{
    public StageInfo(Stage stage, TChara enemy, IEnumerable<int> cardIds)
    {
        this.Stage = stage;
        this.Enemy = enemy;
        this.CardIds = cardIds.ToList();
    }

    public Stage Stage { get; }

    public TChara Enemy { get; }

    public IEnumerable<int> CardIds { get; }    // 0-based
}
