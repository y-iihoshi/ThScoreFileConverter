//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;

namespace ThScoreFileConverter.Models.Th16;

// %T16CLEAR[x][yy]
internal class ClearReplacer : Th13.ClearReplacerBase<
    Chara, CharaWithTotal, Level, Level, Th14.LevelPracticeWithTotal, Th14.StagePractice, IScoreData>
{
    public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        : base(
              Definitions.FormatPrefix,
              Parsers.LevelParser,
              Parsers.CharaParser,
              (level, chara) => GetRanking(clearDataDictionary, level, chara))
    {
    }

    private static IReadOnlyList<Th10.IScoreData<Th13.StageProgress>> GetRanking(
        IReadOnlyDictionary<CharaWithTotal, IClearData> dictionary, Level level, Chara chara)
    {
        return dictionary.TryGetValue(EnumHelper.To<CharaWithTotal>(chara), out var clearData)
            && clearData.Rankings.TryGetValue(EnumHelper.To<Th14.LevelPracticeWithTotal>(level), out var ranking)
            ? ranking : ImmutableList<Th10.IScoreData<Th13.StageProgress>>.Empty;
    }
}
