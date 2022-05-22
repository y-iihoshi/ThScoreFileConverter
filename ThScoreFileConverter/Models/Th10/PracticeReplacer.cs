//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;

namespace ThScoreFileConverter.Models.Th10;

// %T10PRAC[x][yy][z]
internal class PracticeReplacer : PracticeReplacerBase<Level, Chara, Stage>
{
    public PracticeReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelParser,
              Parsers.CharaParser,
              Parsers.StageParser,
              Models.Definitions.CanPractice,
              Models.Definitions.CanPractice,
              (level, chara, stage) => GetPractice(clearDataDictionary, level, chara, stage),
              formatter)
    {
    }

    private static IPractice? GetPractice(
        IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
        Level level,
        Chara chara,
        Stage stage)
    {
        return clearDataDictionary.TryGetValue((CharaWithTotal)chara, out var clearData)
            && clearData.Practices.TryGetValue((level, stage), out var practice)
            ? practice : null;
    }
}
