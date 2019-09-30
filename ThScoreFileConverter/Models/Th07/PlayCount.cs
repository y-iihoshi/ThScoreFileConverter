//-----------------------------------------------------------------------
// <copyright file="PlayCount.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th07
{
    internal class PlayCount : IBinaryReadable  // per level-with-total
    {
        public PlayCount() => this.Trials = new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);

        public int TotalTrial { get; private set; }

        public IReadOnlyDictionary<Chara, int> Trials { get; private set; }

        public int TotalRetry { get; private set; }

        public int TotalClear { get; private set; }

        public int TotalContinue { get; private set; }

        public int TotalPractice { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.TotalTrial = reader.ReadInt32();
            this.Trials = Utils.GetEnumerator<Chara>().ToDictionary(chara => chara, chara => reader.ReadInt32());
            this.TotalRetry = reader.ReadInt32();
            this.TotalClear = reader.ReadInt32();
            this.TotalContinue = reader.ReadInt32();
            this.TotalPractice = reader.ReadInt32();
        }
    }
}
