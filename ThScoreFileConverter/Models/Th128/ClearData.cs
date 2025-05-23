﻿//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128;

internal sealed class ClearData : Th10.Chapter, IClearData // per route
{
    public const string ValidSignature = "CR";
    public const ushort ValidVersion = 0x0003;
    public const int ValidSize = 0x0000066C;

    public ClearData(Th10.Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        var levels = EnumHelper<Level>.Enumerable;

        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.Route = (RouteWithTotal)reader.ReadInt32();

        this.Rankings = levels.ToDictionary(
            level => level,
            _ => Enumerable.Range(0, 10).Select(_ => BinaryReadableHelper.Create<ScoreData>(reader)).ToList()
                as IReadOnlyList<Th10.IScoreData<StageProgress>>);

        this.TotalPlayCount = reader.ReadInt32();
        this.PlayTime = reader.ReadInt32();
        this.ClearCounts = levels.ToDictionary(level => level, _ => reader.ReadInt32());
    }

    public RouteWithTotal Route { get; }

    public IReadOnlyDictionary<Level, IReadOnlyList<Th10.IScoreData<StageProgress>>> Rankings { get; }

    public int TotalPlayCount { get; }

    public int PlayTime { get; }    // = seconds * 60fps

    public IReadOnlyDictionary<Level, int> ClearCounts { get; }

    public static bool CanInitialize(Th10.Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}
