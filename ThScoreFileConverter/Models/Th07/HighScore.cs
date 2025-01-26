//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Models.Th07;

internal sealed class HighScore : Th06.Chapter, IHighScore // per character, level, rank
{
    public const string ValidSignature = "HSCR";
    public const short ValidSize = 0x0028;

    public HighScore(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000001?
        this.Score = reader.ReadUInt32();
        this.SlowRate = reader.ReadSingle();
        this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
        this.Level = EnumHelper.To<Level>(reader.ReadByte());
        this.StageProgress = EnumHelper.To<StageProgress>(reader.ReadByte());
        this.Name = reader.ReadExactBytes(9);
        this.Date = reader.ReadExactBytes(6);
        this.ContinueCount = reader.ReadUInt16();
    }

    public HighScore(uint score)    // for InitialRanking only
        : base()
    {
        this.Score = score;
        this.Name = EncodingHelper.Default.GetBytes("--------\0");
        this.Date = EncodingHelper.Default.GetBytes("--/--\0");
    }

    public uint Score { get; }  // Divided by 10

    public float SlowRate { get; }

    public Chara Chara { get; }

    public Level Level { get; }

    public StageProgress StageProgress { get; }

    public IEnumerable<byte> Name { get; }  // Null-terminated

    public IEnumerable<byte> Date { get; }  // "mm/dd\0"

    public ushort ContinueCount { get; }
}
