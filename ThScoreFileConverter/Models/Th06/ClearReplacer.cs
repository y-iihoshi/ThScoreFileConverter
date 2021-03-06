﻿//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Models.Th06.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Models.Th06
{
    // %T06CLEAR[x][yy]
    internal class ClearReplacer : ClearReplacerBase<Level, Chara, StageProgress>
    {
        public ClearReplacer(
            IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.LevelParser,
                  Parsers.CharaParser,
                  (level, chara) => GetRanking(rankings, level, chara),
                  static stageProgress => stageProgress == StageProgress.Extra)
        {
        }

        private static IReadOnlyList<IHighScore>? GetRanking(
            IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
            Level level,
            Chara chara)
        {
            return rankings.TryGetValue((chara, level), out var ranking) ? ranking : null;
        }
    }
}
