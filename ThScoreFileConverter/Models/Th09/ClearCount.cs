//-----------------------------------------------------------------------
// <copyright file="ClearCount.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Models.Th09;

internal class ClearCount : IBinaryReadable, IClearCount
{
    public ClearCount()
    {
        this.Counts = ImmutableDictionary<Level, int>.Empty;
    }

    public IReadOnlyDictionary<Level, int> Counts { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.Counts = EnumHelper<Level>.Enumerable.ToDictionary(level => level, _ => reader.ReadInt32());
        _ = reader.ReadUInt32();
    }
}
