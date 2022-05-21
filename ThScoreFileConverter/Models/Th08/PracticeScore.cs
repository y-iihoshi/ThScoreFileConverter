//-----------------------------------------------------------------------
// <copyright file="PracticeScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th08;

internal class PracticeScore : Th06.Chapter, IPracticeScore  // per character
{
    public const string ValidSignature = "PSCR";
    public const short ValidSize = 0x0178;

    public PracticeScore(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        var stageLevelPairs = EnumHelper<Stage>.Enumerable.SelectMany(
            stage => EnumHelper<Level>.Enumerable.Select(level => (stage, level)));

        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        //// The fields for Stage.Extra and Level.Extra actually exist...

        _ = reader.ReadUInt32();        // always 0x00000002?
        this.PlayCounts = stageLevelPairs.ToDictionary(pair => pair, _ => reader.ReadInt32());
        this.HighScores = stageLevelPairs.ToDictionary(pair => pair, _ => reader.ReadInt32());
        this.Chara = EnumHelper.To<Chara>(reader.ReadByte());
        _ = reader.ReadExactBytes(3);   // always 0x000001?
    }

    public IReadOnlyDictionary<(Stage Stage, Level Level), int> PlayCounts { get; }

    public IReadOnlyDictionary<(Stage Stage, Level Level), int> HighScores { get; } // Divided by 10

    public Chara Chara { get; }
}
