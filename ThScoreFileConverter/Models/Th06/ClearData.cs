//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th06;

internal class ClearData : Chapter, IClearData<Chara, Level>    // per character
{
    public const string ValidSignature = "CLRD";
    public const short ValidSize = 0x0018;

    public ClearData(Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        var levels = EnumHelper<Level>.Enumerable;

        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000010?
        this.StoryFlags = levels.ToDictionary(level => level, level => reader.ReadByte());
        this.PracticeFlags = levels.ToDictionary(level => level, level => reader.ReadByte());
        this.Chara = EnumHelper.To<Chara>(reader.ReadInt16());
    }

    public IReadOnlyDictionary<Level, byte> StoryFlags { get; }     // really...?

    public IReadOnlyDictionary<Level, byte> PracticeFlags { get; }  // really...?

    public Chara Chara { get; }
}
