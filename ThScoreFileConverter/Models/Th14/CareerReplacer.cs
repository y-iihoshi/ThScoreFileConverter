﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th14;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th14;

// %T14C[w][xxx][yy][z]
internal sealed class CareerReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
        CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData>> clearDataDictionary,
    INumberFormatter formatter)
    : CareerReplacerBase<
        GameMode, CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData>(
        Definitions.FormatPrefix,
        Parsers.GameModeParser,
        Parsers.CharaWithTotalParser,
        Definitions.CardTable.Keys,
        clearDataDictionary,
        formatter)
{
}
