//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10
{
    // %T10SCR[w][xx][y][z]
    internal class ScoreReplacer : ScoreReplacerBase<Chara>
    {
        public ScoreReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
            INumberFormatter formatter)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.LevelParser,
                  Parsers.CharaParser,
                  (level, chara, rank) => GetScore(clearDataDictionary, level, chara, rank),
                  formatter)
        {
        }

        private static IScoreData<StageProgress> GetScore(
            IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
            Level level,
            Chara chara,
            int rank)
        {
            return clearDataDictionary.TryGetValue((CharaWithTotal)chara, out var clearData)
                && clearData.Rankings.TryGetValue(level, out var ranking)
                && (rank < ranking.Count)
                ? ranking[rank] : new ScoreData();
        }
    }
}
