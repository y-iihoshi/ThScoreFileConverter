//-----------------------------------------------------------------------
// <copyright file="StageInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th105
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class StageInfo<TStage, TChara>
        where TStage : struct, Enum
        where TChara : struct, Enum
    {
        public StageInfo(TStage stage, TChara enemy, IEnumerable<int> cardIds)
        {
            this.Stage = stage;
            this.Enemy = enemy;
            this.CardIds = cardIds.ToList();
        }

        // [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
        public TStage Stage { get; }

        public TChara Enemy { get; }

        public List<int> CardIds { get; }   // 0-based
    }
}
