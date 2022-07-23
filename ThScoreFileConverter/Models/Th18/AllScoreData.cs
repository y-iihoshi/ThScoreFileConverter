//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Extensions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th18;

internal class AllScoreData
{
    private readonly Dictionary<CharaWithTotal, IClearData> clearData;

    public AllScoreData()
    {
        this.clearData = new Dictionary<CharaWithTotal, IClearData>(EnumHelper<CharaWithTotal>.NumValues);
    }

    public Th095.HeaderBase? Header { get; private set; }

    public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData => this.clearData;

    public IStatus? Status { get; private set; }

    public void Set(Th095.HeaderBase header)
    {
        this.Header = header;
    }

    public void Set(IClearData data)
    {
        _ = this.clearData.TryAdd(data.Chara, data);
    }

    public void Set(IStatus status)
    {
        this.Status = status;
    }
}
