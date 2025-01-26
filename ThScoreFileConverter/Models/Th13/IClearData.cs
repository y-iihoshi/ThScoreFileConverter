//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th13;

internal interface IClearData<
    TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice, TScoreData>
    : Th095.IChapter
    where TCharaWithTotal : struct, Enum
    where TLevel : struct, Enum
    where TLevelPractice : struct, Enum
    where TLevelPracticeWithTotal : struct, Enum
    where TStagePractice : struct, Enum
    where TScoreData : Th10.IScoreData<StageProgress>
{
    IReadOnlyDictionary<int, ISpellCard<TLevel>> Cards { get; }

    TCharaWithTotal Chara { get; }

    IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearCounts { get; }

    IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearFlags { get; }

    int PlayTime { get; }

    IReadOnlyDictionary<(TLevelPractice Level, TStagePractice Stage), Th10.IPractice> Practices { get; }

    IReadOnlyDictionary<TLevelPracticeWithTotal, IReadOnlyList<TScoreData>> Rankings { get; }

    int TotalPlayCount { get; }
}
