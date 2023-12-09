//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverter.Models.Th18;

// %T18CLEAR[x][yy]
internal sealed class ClearReplacer : Th13.ClearReplacerBase<
    Chara, CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Stage, IScoreData>
{
    public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
        CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Stage, IScoreData>> clearDataDictionary)
        : base(Definitions.FormatPrefix, Parsers.LevelParser, Parsers.CharaParser, clearDataDictionary)
    {
    }
}
