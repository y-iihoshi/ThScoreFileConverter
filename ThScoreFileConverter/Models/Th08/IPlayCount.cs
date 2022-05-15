//-----------------------------------------------------------------------
// <copyright file="IPlayCount.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th08;

internal interface IPlayCount
{
    int TotalClear { get; }

    int TotalContinue { get; }

    int TotalPractice { get; }

    int TotalTrial { get; }

    IReadOnlyDictionary<Chara, int> Trials { get; }
}
