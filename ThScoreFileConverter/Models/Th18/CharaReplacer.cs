//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th18
{
    // %T18CHARA[xx][y]
    internal class CharaReplacer : Th13.CharaReplacerBase<
        CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Stage, Th10.IScoreData<Th13.StageProgress>>
    {
        public CharaReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.CharaWithTotalParser,
                  Definitions.IsTotal,
                  Th14.Definitions.IsToBeSummed,
                  static centiseconds => new Time(centiseconds * 10, false),
                  clearDataDictionary,
                  formatter)
        {
        }
    }
}
