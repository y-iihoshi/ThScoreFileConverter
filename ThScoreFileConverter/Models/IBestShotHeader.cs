//-----------------------------------------------------------------------
// <copyright file="IBestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models;

internal interface IBestShotHeader
{
    short Height { get; }

    string Signature { get; }

    float SlowRate { get; }

    short Width { get; }
}
