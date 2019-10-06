//-----------------------------------------------------------------------
// <copyright file="IScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10
{
    internal interface IScoreData<TStageProgress>
        where TStageProgress : struct, Enum
    {
        byte ContinueCount { get; }

        uint DateTime { get; }

        IEnumerable<byte> Name { get; }

        uint Score { get; }

        float SlowRate { get; }

        TStageProgress StageProgress { get; }
    }
}
