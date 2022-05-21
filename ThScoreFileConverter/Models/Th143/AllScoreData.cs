//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th143;

internal class AllScoreData
    : Th095.AllScoreDataBase<IScore, IStatus>
{
    private readonly Dictionary<ItemWithTotal, IItemStatus> itemStatuses;

    public AllScoreData()
        : base(Definitions.SpellCards.Count)
    {
        this.itemStatuses = new Dictionary<ItemWithTotal, IItemStatus>(EnumHelper<ItemWithTotal>.NumValues);
    }

    public IReadOnlyDictionary<ItemWithTotal, IItemStatus> ItemStatuses => this.itemStatuses;

    public void Set(IItemStatus status)
    {
        _ = this.itemStatuses.TryAdd(status.Item, status);
    }
}
