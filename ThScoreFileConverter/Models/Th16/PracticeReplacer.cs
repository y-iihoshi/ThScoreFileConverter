﻿//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;

namespace ThScoreFileConverter.Models.Th16;

// %T16PRAC[x][yy][z]
internal class PracticeReplacer : Th10.PracticeReplacerBase<Level, Chara, Stage>
{
    public PracticeReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelParser,
              Parsers.CharaParser,
              Parsers.StageParser,
              Core.Models.Definitions.CanPractice,
              Core.Models.Definitions.CanPractice,
              (level, chara, stage) => GetPractice(clearDataDictionary, level, chara, stage),
              formatter)
    {
    }

    private static Th10.IPractice? GetPractice(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, Level level, Chara chara, Stage stage)
    {
        return clearDataDictionary.TryGetValue((CharaWithTotal)chara, out var clearData)
            && clearData.Practices.TryGetValue((level, (Th14.StagePractice)stage), out var practice)
            ? practice : null;
    }
}
