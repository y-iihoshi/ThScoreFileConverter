//-----------------------------------------------------------------------
// <copyright file="ISpellCardResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

internal interface ISpellCardResult<TChara>
    where TChara : struct, Enum
{
    TChara Enemy { get; }

    uint Frames { get; }

    int GotCount { get; }

    int Id { get; }

    Level Level { get; }

    int TrialCount { get; }
}
