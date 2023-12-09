//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Core.Models.Th06.Chara,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Models.Th06;

internal sealed class HighScore : Chapter, IHighScore  // per character, level, rank
{
    public const string ValidSignature = "HSCR";
    public const short ValidSize = 0x001C;

    public HighScore(Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000001?
        this.Score = reader.ReadUInt32();
        this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
        this.Level = EnumHelper.To<Level>(reader.ReadByte());
        this.StageProgress = EnumHelper.To<StageProgress>(reader.ReadByte());
        this.Name = reader.ReadExactBytes(9);
    }

    public HighScore(uint score)    // for InitialRanking only
        : base()
    {
        this.Score = score;
        this.Name = EncodingHelper.Default.GetBytes("Nanashi\0\0");
    }

    public uint Score { get; }

    public Chara Chara { get; }

    public Level Level { get; }

    public StageProgress StageProgress { get; }

    public IEnumerable<byte> Name { get; }  // Null-terminated
}
