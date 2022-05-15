//-----------------------------------------------------------------------
// <copyright file="ISpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th128;

internal interface ISpellCard
{
    bool HasTried { get; }

    int Id { get; }

    Level Level { get; }

    IEnumerable<byte> Name { get; }

    int NoIceCount { get; }

    int NoMissCount { get; }

    int TrialCount { get; }
}
