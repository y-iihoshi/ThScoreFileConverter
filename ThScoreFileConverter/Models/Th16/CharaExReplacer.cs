//-----------------------------------------------------------------------
// <copyright file="CharaExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;

namespace ThScoreFileConverter.Models.Th16;

// %T16CHARAEX[x][yy][z]
internal class CharaExReplacer : Th13.CharaExReplacerBase<
    CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Core.Models.Th14.StagePractice, IScoreData>
{
    public CharaExReplacer(
        IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
            CharaWithTotal,
            Level,
            Level,
            Core.Models.Th14.LevelPracticeWithTotal,
            Core.Models.Th14.StagePractice,
            IScoreData>> clearDataDictionary,
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
