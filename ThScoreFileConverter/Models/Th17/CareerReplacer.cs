//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th17;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th17;

// %T17C[w][xxx][yy][z]
internal sealed class CareerReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
        CharaWithTotal,
        Level,
        Level,
        Core.Models.Th14.LevelPracticeWithTotal,
        Core.Models.Th14.StagePractice,
        IScoreData>> clearDataDictionary,
    INumberFormatter formatter)
    : Th14.CareerReplacerBase<
        GameMode, CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Core.Models.Th14.StagePractice, IScoreData>(
        Definitions.FormatPrefix,
        Parsers.GameModeParser,
        Parsers.CharaWithTotalParser,
        Definitions.CardTable.Keys,
        clearDataDictionary,
        formatter)
{
}
