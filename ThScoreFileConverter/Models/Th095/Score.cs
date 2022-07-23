//-----------------------------------------------------------------------
// <copyright file="Score.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th095;

internal class Score : Chapter, IScore  // per scene
{
    public const string ValidSignature = "SC";
    public const ushort ValidVersion = 0x0001;
    public const int ValidSize = 0x00000060;

    public Score(Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        var number = reader.ReadUInt32();
        this.LevelScene = (EnumHelper.To<Level>(number / 10), (int)((number % 10) + 1));
        this.HighScore = reader.ReadInt32();
        _ = reader.ReadUInt32();    // always 0x00000000?
        this.BestshotScore = reader.ReadInt32();
        _ = reader.ReadExactBytes(0x20);
        this.DateTime = reader.ReadUInt32();
        _ = reader.ReadUInt32();    // checksum of the bestshot file?
        this.TrialCount = reader.ReadInt32();
        this.SlowRate1 = reader.ReadSingle();
        this.SlowRate2 = reader.ReadSingle();
        _ = reader.ReadExactBytes(0x10);
    }

    public (Level Level, int Scene) LevelScene { get; }

    public int HighScore { get; }

    public int BestshotScore { get; }

    public uint DateTime { get; }   // UNIX time

    public int TrialCount { get; }

    public float SlowRate1 { get; } // ??

    public float SlowRate2 { get; } // ??

    public static bool CanInitialize(Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}
