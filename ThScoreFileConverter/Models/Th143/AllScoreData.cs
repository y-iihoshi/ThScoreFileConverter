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

namespace ThScoreFileConverter.Models.Th143
{
    internal class AllScoreData
    {
        private readonly List<IScore> scores;
        private readonly Dictionary<ItemWithTotal, IItemStatus> itemStatuses;

        public AllScoreData()
        {
            this.scores = new List<IScore>(Definitions.SpellCards.Count);
            this.itemStatuses = new Dictionary<ItemWithTotal, IItemStatus>(
                Enum.GetValues(typeof(ItemWithTotal)).Length);
        }

        public Th095.HeaderBase? Header { get; private set; }

        public IReadOnlyList<IScore> Scores => this.scores;

        public IReadOnlyDictionary<ItemWithTotal, IItemStatus> ItemStatuses => this.itemStatuses;

        public IStatus? Status { get; private set; }

        public void Set(Th095.HeaderBase header)
        {
            this.Header = header;
        }

        public void Set(IScore score)
        {
            this.scores.Add(score);
        }

        public void Set(IItemStatus status)
        {
            _ = this.itemStatuses.TryAdd(status.Item, status);
        }

        public void Set(IStatus status)
        {
            this.Status = status;
        }
    }
}
