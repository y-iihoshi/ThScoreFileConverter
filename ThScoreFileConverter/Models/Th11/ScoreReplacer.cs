//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;

namespace ThScoreFileConverter.Models.Th11;

// %T11SCR[w][xx][y][z]
internal sealed class ScoreReplacer : Th10.ScoreReplacerBase<Chara>
{
    public ScoreReplacer(
        IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelParser,
              Parsers.CharaParser,
              (level, chara, rank) => GetScore(clearDataDictionary, level, chara, rank),
              formatter)
    {
    }

    private static Th10.IScoreData<Th10.StageProgress> GetScore(
        IReadOnlyDictionary<CharaWithTotal, Th10.IClearData<CharaWithTotal>> clearDataDictionary,
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
