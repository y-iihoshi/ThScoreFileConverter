//-----------------------------------------------------------------------
// <copyright file="IClearCount.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th09;

internal interface IClearCount
{
    IReadOnlyDictionary<Level, int> Counts { get; }
}
