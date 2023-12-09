//-----------------------------------------------------------------------
// <copyright file="ItemStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th143;

namespace ThScoreFileConverter.Models.Th143;

internal sealed class ItemStatus : Th10.Chapter, IItemStatus
{
    public const string ValidSignature = "TI";
    public const ushort ValidVersion = 0x0001;
    public const int ValidSize = 0x00000034;

    public ItemStatus(Th10.Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.Item = EnumHelper.To<ItemWithTotal>(reader.ReadInt32());
        this.UseCount = reader.ReadInt32();
        this.ClearedCount = reader.ReadInt32();
        this.ClearedScenes = reader.ReadInt32();
        this.ItemLevel = reader.ReadInt32();
        _ = reader.ReadInt32();
        this.AvailableCount = reader.ReadInt32();
        this.FramesOrRanges = reader.ReadInt32();
        _ = reader.ReadInt32(); // always 0?
        _ = reader.ReadInt32(); // always 0?
    }

    public ItemWithTotal Item { get; }

    public int UseCount { get; }

    public int ClearedCount { get; }

    public int ClearedScenes { get; }

    public int ItemLevel { get; }

    public int AvailableCount { get; }

    public int FramesOrRanges { get; }

    public static bool CanInitialize(Th10.Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}
