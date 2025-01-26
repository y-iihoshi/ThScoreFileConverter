//-----------------------------------------------------------------------
// <copyright file="PlayCount.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Immutable;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th08;

namespace ThScoreFileConverter.Models.Th08;

internal sealed class PlayCount : IBinaryReadable, IPlayCount  // per level-with-total
{
    public PlayCount()
    {
        this.Trials = ImmutableDictionary<Chara, int>.Empty;
    }

    public int TotalTrial { get; private set; }

    public IReadOnlyDictionary<Chara, int> Trials { get; private set; }

    public int TotalClear { get; private set; }

    public int TotalContinue { get; private set; }

    public int TotalPractice { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.TotalTrial = reader.ReadInt32();
        this.Trials = EnumHelper<Chara>.Enumerable.ToDictionary(chara => chara, _ => reader.ReadInt32());
        _ = reader.ReadUInt32();    // always 0x00000000?
        this.TotalClear = reader.ReadInt32();
        this.TotalContinue = reader.ReadInt32();
        this.TotalPractice = reader.ReadInt32();
    }
}
