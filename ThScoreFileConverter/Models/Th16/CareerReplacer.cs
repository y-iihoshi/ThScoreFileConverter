//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;

namespace ThScoreFileConverter.Models.Th16;

// %T16C[w][xxx][yy][z]
internal sealed class CareerReplacer : Th14.CareerReplacerBase<
    GameMode, CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Core.Models.Th14.StagePractice, IScoreData>
{
    public CareerReplacer(
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
              Parsers.GameModeParser,
              Parsers.CharaWithTotalParser,
              Definitions.CardTable.Keys,
              clearDataDictionary,
              formatter)
    {
    }
}
