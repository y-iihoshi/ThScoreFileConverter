﻿//-----------------------------------------------------------------------
// <copyright file="PlayStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th09;

internal sealed class PlayStatus : Th06.Chapter, IPlayStatus
{
    public const string ValidSignature = "PLST";
    public const short ValidSize = 0x01FC;

    public PlayStatus(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        var charas = EnumHelper<Chara>.Enumerable;

        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000003?
        var hours = reader.ReadInt32();
        var minutes = reader.ReadInt32();
        var seconds = reader.ReadInt32();
        var milliseconds = reader.ReadInt32();
        this.TotalRunningTime = new Time(hours, minutes, seconds, milliseconds, false);
        hours = reader.ReadInt32();
        minutes = reader.ReadInt32();
        seconds = reader.ReadInt32();
        milliseconds = reader.ReadInt32();
        this.TotalPlayTime = new Time(hours, minutes, seconds, milliseconds, false);
        this.BgmFlags = reader.ReadExactBytes(19);
        _ = reader.ReadExactBytes(13);
        this.MatchFlags = charas.ToDictionary(chara => chara, _ => reader.ReadByte());
        this.StoryFlags = charas.ToDictionary(chara => chara, _ => reader.ReadByte());
        this.ExtraFlags = charas.ToDictionary(chara => chara, _ => reader.ReadByte());
        this.ClearCounts = charas.ToDictionary(
            chara => chara, _ => BinaryReadableHelper.Create<ClearCount>(reader) as IClearCount);
    }

    public Time TotalRunningTime { get; }

    public Time TotalPlayTime { get; }  // really...?

    public IEnumerable<byte> BgmFlags { get; }

    public IReadOnlyDictionary<Chara, byte> MatchFlags { get; }

    public IReadOnlyDictionary<Chara, byte> StoryFlags { get; }

    public IReadOnlyDictionary<Chara, byte> ExtraFlags { get; }

    public IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; }
}
