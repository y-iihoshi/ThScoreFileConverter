//-----------------------------------------------------------------------
// <copyright file="PracticeScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;

namespace ThScoreFileConverter.Models.Th07;

internal sealed class PracticeScore : Th06.Chapter, IPracticeScore // per character, level, stage
{
    public const string ValidSignature = "PSCR";
    public const short ValidSize = 0x0018;

    public PracticeScore(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000001?
        this.TrialCount = reader.ReadInt32();
        this.HighScore = reader.ReadInt32();
        this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
        this.Level = EnumHelper.To<Level>(reader.ReadByte());
        this.Stage = EnumHelper.To<Stage>(reader.ReadByte());
        _ = reader.ReadByte();      // always 0x00?
    }

    public int TrialCount { get; }  // really...?

    public int HighScore { get; }

    public Chara Chara { get; }

    public Level Level { get; }

    public Stage Stage { get; }
}
