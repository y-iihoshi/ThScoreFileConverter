//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;

namespace ThScoreFileConverter.Models.Th18;

// %T18CHARAEX[x][yy][z]
internal sealed class CharaExReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
        CharaWithTotal,
        Level,
        Level,
        Core.Models.Th14.LevelPracticeWithTotal,
        Stage,
        Th10.IScoreData<Th13.StageProgress>>> clearDataDictionary,
    INumberFormatter formatter)
    : Th13.CharaExReplacerBase<
        CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Stage, Th10.IScoreData<Th13.StageProgress>>(
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
