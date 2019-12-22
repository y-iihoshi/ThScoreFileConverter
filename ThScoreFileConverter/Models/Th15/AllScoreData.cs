//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th15
{
    internal class AllScoreData
    {
        private readonly Dictionary<CharaWithTotal, IClearData> clearData;

        public AllScoreData()
        {
            this.clearData =
                new Dictionary<CharaWithTotal, IClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
        }

        public Th095.HeaderBase Header { get; private set; }

        public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData => this.clearData;

        public Th125.IStatus Status { get; private set; }

        public void Set(Th095.HeaderBase header)
        {
            this.Header = header;
        }

        public void Set(IClearData data)
        {
            if (!this.clearData.ContainsKey(data.Chara))
                this.clearData.Add(data.Chara, data);
        }

        public void Set(Th125.IStatus status)
        {
            this.Status = status;
        }
    }
}
