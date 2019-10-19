//-----------------------------------------------------------------------
// <copyright file="IBestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th095
{
    internal interface IBestShotHeader
    {
        IEnumerable<byte> CardName { get; }

        short Height { get; }

        Level Level { get; }

        short Scene { get; }

        int Score { get; }

        string Signature { get; }

        float SlowRate { get; }

        short Width { get; }
    }
}
