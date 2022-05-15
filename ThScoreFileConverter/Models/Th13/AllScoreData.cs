//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th13;

internal class AllScoreData<
    TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice, TScoreData, TStatus>
    where TCharaWithTotal : struct, Enum
    where TLevel : struct, Enum
    where TLevelPractice : struct, Enum
    where TLevelPracticeWithTotal : struct, Enum
    where TStagePractice : struct, Enum
    where TScoreData : Th10.IScoreData<StageProgress>
    where TStatus : Th125.IStatus
{
    private readonly Dictionary<
        TCharaWithTotal,
        IClearData<
            TCharaWithTotal,
            TLevel,
            TLevelPractice,
            TLevelPracticeWithTotal,
            TStagePractice,
            TScoreData>> clearData;

    public AllScoreData()
    {
        this.clearData = new Dictionary<
            TCharaWithTotal,
            IClearData<
                TCharaWithTotal,
                TLevel,
                TLevelPractice,
                TLevelPracticeWithTotal,
                TStagePractice,
                TScoreData>>(EnumHelper<TCharaWithTotal>.NumValues);
    }

    public Th095.HeaderBase? Header { get; private set; }

    public IReadOnlyDictionary<
        TCharaWithTotal,
        IClearData<
            TCharaWithTotal,
            TLevel,
            TLevelPractice,
            TLevelPracticeWithTotal,
            TStagePractice,
            TScoreData>> ClearData => this.clearData;

    public TStatus? Status { get; private set; }

    public void Set(Th095.HeaderBase header)
    {
        this.Header = header;
    }

    public void Set(
        IClearData<
            TCharaWithTotal,
            TLevel,
            TLevelPractice,
            TLevelPracticeWithTotal,
            TStagePractice,
            TScoreData> data)
    {
        _ = this.clearData.TryAdd(data.Chara, data);
    }

    public void Set(TStatus status)
    {
        this.Status = status;
    }
}
