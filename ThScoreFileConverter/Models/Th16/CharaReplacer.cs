//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;

namespace ThScoreFileConverter.Models.Th16;

// %T16CHARA[xx][y]
internal sealed class CharaReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    : Th13.CharaReplacerBase<
        CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Core.Models.Th14.StagePractice, IScoreData>(
        Definitions.FormatPrefix,
        Parsers.CharaWithTotalParser,
        Definitions.IsTotal,
        Th14.Definitions.IsToBeSummed,
        static centiseconds => new Time(centiseconds * 10, false),
        clearDataDictionary,
        formatter)
{
}
