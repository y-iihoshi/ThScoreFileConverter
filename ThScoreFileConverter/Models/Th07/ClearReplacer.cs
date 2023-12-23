//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th07;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Models.Th07;

// %T07CLEAR[x][yy]
internal sealed class ClearReplacer(IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings)
    : Th06.ClearReplacerBase<Level, Chara, StageProgress>(
        Definitions.FormatPrefix,
        Parsers.LevelParser,
        Parsers.CharaParser,
        (level, chara) => GetRanking(rankings, level, chara),
        static stageProgress => stageProgress is StageProgress.Extra or StageProgress.Phantasm)
{
    private static IReadOnlyList<IHighScore>? GetRanking(
        IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings, Level level, Chara chara)
    {
        return rankings.TryGetValue((chara, level), out var ranking) ? ranking : null;
    }
}
