//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th13;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th13;

// %T13CLEAR[x][yy]
internal class ClearReplacer : ClearReplacerBase<
    Chara, CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData>
{
    public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData<
        CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData>> clearDataDictionary)
        : base(Definitions.FormatPrefix, Parsers.LevelParser, Parsers.CharaParser, clearDataDictionary)
    {
    }
}
