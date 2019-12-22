//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10
{
    internal class AllScoreData<TCharaWithTotal, TStageProgress>
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        private readonly Dictionary<TCharaWithTotal, IClearData<TCharaWithTotal, TStageProgress>> clearData;

        public AllScoreData()
        {
            this.clearData = new Dictionary<TCharaWithTotal, IClearData<TCharaWithTotal, TStageProgress>>(
                Enum.GetValues(typeof(TCharaWithTotal)).Length);
        }

        public Th095.HeaderBase Header { get; private set; }

        public IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal, TStageProgress>> ClearData
            => this.clearData;

        public IStatus Status { get; private set; }

        public void Set(Th095.HeaderBase header)
        {
            this.Header = header;
        }

        public void Set(IClearData<TCharaWithTotal, TStageProgress> data)
        {
            if (!this.clearData.ContainsKey(data.Chara))
                this.clearData.Add(data.Chara, data);
        }

        public void Set(IStatus status)
        {
            this.Status = status;
        }
    }
}
