//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Immutable;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08;

internal sealed class HighScore : Th06.Chapter, IHighScore // per character, level, rank
{
    public const string ValidSignature = "HSCR";
    public const short ValidSize = 0x0168;

    public HighScore(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000004?
        this.Score = reader.ReadUInt32();
        this.SlowRate = reader.ReadSingle();
        this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
        this.Level = EnumHelper.To<Level>(reader.ReadByte());
        this.StageProgress = EnumHelper.To<StageProgress>(reader.ReadByte());
        this.Name = reader.ReadExactBytes(9);
        this.Date = reader.ReadExactBytes(6);
        this.ContinueCount = reader.ReadUInt16();

        // 01 00 00 00 04 00 09 00 FF FF FF FF FF FF FF FF
        // 05 00 00 00 01 00 08 00 58 02 58 02
        _ = reader.ReadExactBytes(0x1C);

        this.PlayerNum = reader.ReadByte();

        // NN 03 00 01 01 LL 01 00 02 00 00 ** ** 00 00 00
        // 00 00 00 00 00 00 00 00 00 00 00 00 01 40 00 00
        // where NN: PlayerNum, LL: level, **: unknown (0x64 or 0x0A; 0x50 or 0x0A)
        _ = reader.ReadExactBytes(0x1F);

        this.PlayTime = reader.ReadUInt32();
        this.PointItem = reader.ReadInt32();
        _ = reader.ReadUInt32();    // always 0x00000000?
        this.MissCount = reader.ReadInt32();
        this.BombCount = reader.ReadInt32();
        this.LastSpellCount = reader.ReadInt32();
        this.PauseCount = reader.ReadInt32();
        this.TimePoint = reader.ReadInt32();
        this.HumanRate = reader.ReadInt32();
        this.CardFlags = Definitions.CardTable.Keys.ToDictionary(key => key, _ => reader.ReadByte());
        _ = reader.ReadExactBytes(2);
    }

    public HighScore(uint score)    // for InitialRanking only
        : base()
    {
        this.Score = score;
        this.Name = EncodingHelper.Default.GetBytes("--------\0");
        this.Date = EncodingHelper.Default.GetBytes("--/--\0");
        this.CardFlags = ImmutableDictionary<int, byte>.Empty;
    }

    public uint Score { get; }      // Divided by 10

    public float SlowRate { get; }

    public Chara Chara { get; }

    public Level Level { get; }

    public StageProgress StageProgress { get; }

    public IEnumerable<byte> Name { get; }  // Null-terminated

    public IEnumerable<byte> Date { get; }  // "mm/dd\0"

    public ushort ContinueCount { get; }

    public byte PlayerNum { get; }  // 0-based

    public uint PlayTime { get; }   // = seconds * 60fps

    public int PointItem { get; }

    public int MissCount { get; }

    public int BombCount { get; }

    public int LastSpellCount { get; }

    public int PauseCount { get; }

    public int TimePoint { get; }

    public int HumanRate { get; }   // Multiplied by 100

    public IReadOnlyDictionary<int, byte> CardFlags { get; }
}
