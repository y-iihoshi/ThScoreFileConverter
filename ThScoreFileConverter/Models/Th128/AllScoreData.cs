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

namespace ThScoreFileConverter.Models.Th128
{
    internal class AllScoreData
    {
        private readonly Dictionary<RouteWithTotal, IClearData> clearData;

        public AllScoreData()
        {
            this.clearData =
                new Dictionary<RouteWithTotal, IClearData>(Enum.GetValues(typeof(RouteWithTotal)).Length);
        }

        public Th095.HeaderBase? Header { get; private set; }

        public IReadOnlyDictionary<RouteWithTotal, IClearData> ClearData => this.clearData;

        public ICardData? CardData { get; private set; }

        public Th125.IStatus? Status { get; private set; }

        public void Set(Th095.HeaderBase header)
        {
            this.Header = header;
        }

        public void Set(IClearData data)
        {
            _ = this.clearData.TryAdd(data.Route, data);
        }

        public void Set(ICardData data)
        {
            this.CardData = data;
        }

        public void Set(Th125.IStatus status)
        {
            this.Status = status;
        }
    }
}
