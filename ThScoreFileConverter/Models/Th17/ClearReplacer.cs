//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th17
{
    // %T17CLEAR[x][yy]
    internal class ClearReplacer : Th13.ClearReplacerBase<
        Chara, CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Th14.StagePractice, IScoreData>
    {
        public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
            CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Th14.StagePractice, IScoreData>> clearDataDictionary)
            : base(Definitions.FormatPrefix, Parsers.LevelParser, Parsers.CharaParser, clearDataDictionary)
        {
        }
    }
}
