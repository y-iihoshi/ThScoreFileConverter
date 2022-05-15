//-----------------------------------------------------------------------
// <copyright file="ISpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10;

internal interface ISpellCard<TLevel>
    where TLevel : struct, Enum
{
    int ClearCount { get; }

    bool HasTried { get; }

    int Id { get; }

    TLevel Level { get; }

    IEnumerable<byte> Name { get; }

    int TrialCount { get; }
}
