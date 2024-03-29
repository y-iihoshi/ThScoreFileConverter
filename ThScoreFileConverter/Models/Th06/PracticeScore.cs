﻿//-----------------------------------------------------------------------
// <copyright file="PracticeScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;

namespace ThScoreFileConverter.Models.Th06;

internal sealed class PracticeScore : Chapter, IPracticeScore  // per character, level, stage
{
    public const string ValidSignature = "PSCR";
    public const short ValidSize = 0x0014;

    public PracticeScore(Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000010?
        this.HighScore = reader.ReadInt32();
        this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
        this.Level = EnumHelper.To<Level>(reader.ReadByte());
        this.Stage = EnumHelper.To<Stage>(reader.ReadByte());
        _ = reader.ReadByte();
    }

    public int HighScore { get; }

    public Chara Chara { get; }

    public Level Level { get; }

    public Stage Stage { get; }
}
