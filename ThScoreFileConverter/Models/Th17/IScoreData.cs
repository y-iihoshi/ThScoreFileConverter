//-----------------------------------------------------------------------
// <copyright file="IScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th17
{
    internal interface IScoreData
    {
        byte ContinueCount { get; }

        uint DateTime { get; }

        IEnumerable<byte> Name { get; }

        uint Score { get; }

        float SlowRate { get; }

        StageProgress StageProgress { get; }
    }
}
