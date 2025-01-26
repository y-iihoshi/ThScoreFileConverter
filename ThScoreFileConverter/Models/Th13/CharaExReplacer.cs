﻿//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th13;

namespace ThScoreFileConverter.Models.Th13;

// %T13CHARAEX[x][yy][z]
internal sealed class CharaExReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData<
        CharaWithTotal,
        LevelPractice,
        LevelPractice,
        LevelPracticeWithTotal,
        StagePractice,
        Th10.IScoreData<StageProgress>>> clearDataDictionary,
    INumberFormatter formatter)
    : CharaExReplacerBase<
        CharaWithTotal,
        LevelPractice,
        LevelPractice,
        LevelPracticeWithTotal,
        StagePractice,
        Th10.IScoreData<StageProgress>>(
        Definitions.FormatPrefix,
        Parsers.LevelWithTotalParser,
        Parsers.CharaWithTotalParser,
        Models.Definitions.IsTotal,
        Definitions.IsTotal,
        Definitions.IsToBeSummed,
        static frames => new Time(frames),
        clearDataDictionary,
        formatter)
{
}
