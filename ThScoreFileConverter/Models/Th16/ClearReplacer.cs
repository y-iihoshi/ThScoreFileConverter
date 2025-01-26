//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Immutable;
using ThScoreFileConverter.Core.Helpers;
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

// %T16CLEAR[x][yy]
internal sealed class ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
    : Th13.ClearReplacerBase<
        Chara, CharaWithTotal, Level, Level, Core.Models.Th14.LevelPracticeWithTotal, Core.Models.Th14.StagePractice, IScoreData>(
        Definitions.FormatPrefix,
        Parsers.LevelParser,
        Parsers.CharaParser,
        (level, chara) => GetRanking(clearDataDictionary, level, chara))
{
    private static IReadOnlyList<Th10.IScoreData<Th13.StageProgress>> GetRanking(
        IReadOnlyDictionary<CharaWithTotal, IClearData> dictionary, Level level, Chara chara)
    {
        return dictionary.TryGetValue(EnumHelper.To<CharaWithTotal>(chara), out var clearData)
            && clearData.Rankings.TryGetValue(EnumHelper.To<Core.Models.Th14.LevelPracticeWithTotal>(level), out var ranking)
            ? ranking : ImmutableList<Th10.IScoreData<Th13.StageProgress>>.Empty;
    }
}
