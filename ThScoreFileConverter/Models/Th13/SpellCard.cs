//-----------------------------------------------------------------------
// <copyright file="SpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th13
{
    internal class SpellCard<TLevel> : IBinaryReadable, ISpellCard<TLevel>
        where TLevel : struct, Enum
    {
        public IEnumerable<byte> Name { get; private set; }

        public int ClearCount { get; private set; }

        public int PracticeClearCount { get; private set; }

        public int TrialCount { get; private set; }

        public int PracticeTrialCount { get; private set; }

        public int Id { get; private set; } // 1-based

        public TLevel Level { get; private set; }

        public int PracticeScore { get; private set; }

        public virtual bool HasTried => (this.TrialCount > 0) || (this.PracticeTrialCount > 0);

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Name = reader.ReadExactBytes(0x80);
            this.ClearCount = reader.ReadInt32();
            this.PracticeClearCount = reader.ReadInt32();
            this.TrialCount = reader.ReadInt32();
            this.PracticeTrialCount = reader.ReadInt32();
            this.Id = reader.ReadInt32() + 1;
            this.Level = Utils.ToEnum<TLevel>(reader.ReadInt32());
            this.PracticeScore = reader.ReadInt32();
        }
    }
}
