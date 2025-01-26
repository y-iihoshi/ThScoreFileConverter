//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;

namespace ThScoreFileConverter.Models.Th15;

// %T15PRAC[x][yy][z]
internal sealed class PracticeReplacer(
    IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    : Th10.PracticeReplacerBase<Level, Chara, Stage>(
        Definitions.FormatPrefix,
        Parsers.LevelParser,
        Parsers.CharaParser,
        Parsers.StageParser,
        Core.Models.Definitions.CanPractice,
        Core.Models.Definitions.CanPractice,
        (level, chara, stage) => GetPractice(clearDataDictionary, level, chara, stage),
        formatter)
{
    private static Th10.IPractice? GetPractice(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, Level level, Chara chara, Stage stage)
    {
        return clearDataDictionary.TryGetValue((CharaWithTotal)chara, out var clearData)
            && clearData.Practices.TryGetValue((level, (Core.Models.Th14.StagePractice)stage), out var practice)
            ? practice : null;
    }
}
