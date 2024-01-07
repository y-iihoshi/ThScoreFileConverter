//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Models.Th10;

internal sealed class AllScoreData<TCharaWithTotal>
    where TCharaWithTotal : struct, Enum
{
    private readonly Dictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> clearData;

    public AllScoreData()
    {
        this.clearData = new Dictionary<TCharaWithTotal, IClearData<TCharaWithTotal>>(
            EnumHelper<TCharaWithTotal>.NumValues);
    }

    public Th095.HeaderBase? Header { get; private set; }

    public IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal>> ClearData
        => this.clearData;

    public IStatus? Status { get; private set; }

    public void Set(Th095.HeaderBase header)
    {
        this.Header = header;
    }

    public void Set(IClearData<TCharaWithTotal> data)
    {
        _ = this.clearData.TryAdd(data.Chara, data);
    }

    public void Set(IStatus status)
    {
        this.Status = status;
    }
}
