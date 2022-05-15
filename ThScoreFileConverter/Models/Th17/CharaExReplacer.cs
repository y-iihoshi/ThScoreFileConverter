﻿//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th17;

// %T17CHARAEX[x][yy][z]
internal class CharaExReplacer : Th13.CharaExReplacerBase<
    CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Th14.StagePractice, Th10.IScoreData<Th13.StageProgress>>
{
    public CharaExReplacer(
        IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
            CharaWithTotal,
            Level,
            Level,
            Th14.LevelPracticeWithTotal,
            Th14.StagePractice,
            Th10.IScoreData<Th13.StageProgress>>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelWithTotalParser,
              Parsers.CharaWithTotalParser,
              Models.Definitions.IsTotal,
              Definitions.IsTotal,
              Th14.Definitions.IsToBeSummed,
              static centiseconds => new Time(centiseconds * 10, false),
              clearDataDictionary,
              formatter)
    {
    }
}
