﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th18;

// %T18C[w][xxx][yy][z]
internal class CareerReplacer : Th14.CareerReplacerBase<
    GameMode, CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Stage, IScoreData>
{
    public CareerReplacer(
        IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
            CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Stage, IScoreData>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.GameModeParser,
              Parsers.CharaWithTotalParser,
              Definitions.CardTable.Keys,
              clearDataDictionary,
              formatter)
    {
    }
}
