﻿//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th17
{
    // %T17SCR[w][xx][y][z]
    internal class ScoreReplacer : Th13.ScoreReplacerBase<Chara>
    {
        public ScoreReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.LevelParser,
                  Parsers.CharaParser,
                  (level, chara, rank) => GetScore(clearDataDictionary, level, chara, rank),
                  formatter)
        {
        }

        private static Th10.IScoreData<Th13.StageProgress> GetScore(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, Level level, Chara chara, int rank)
        {
            return clearDataDictionary.TryGetValue((CharaWithTotal)chara, out var clearData)
                && clearData.Rankings.TryGetValue((Th14.LevelPracticeWithTotal)level, out var ranking)
                && (rank < ranking.Count)
                ? ranking[rank] : new ScoreData();
        }
    }
}
