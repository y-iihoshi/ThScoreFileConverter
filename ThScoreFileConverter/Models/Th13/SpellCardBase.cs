//-----------------------------------------------------------------------
// <copyright file="SpellCardBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th13
{
    internal class SpellCardBase<TLevel> : IBinaryReadable, ISpellCard<TLevel>
        where TLevel : struct, Enum
    {
        private readonly int nameLength;

        protected SpellCardBase(int nameLength)
        {
            this.nameLength = nameLength;
        }

        public IEnumerable<byte> Name { get; private set; } = Enumerable.Empty<byte>();

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
            this.Name = reader.ReadExactBytes(this.nameLength);
            this.ClearCount = reader.ReadInt32();
            this.PracticeClearCount = reader.ReadInt32();
            this.TrialCount = reader.ReadInt32();
            this.PracticeTrialCount = reader.ReadInt32();
            this.Id = reader.ReadInt32() + 1;
            this.Level = EnumHelper.To<TLevel>(reader.ReadInt32());
            this.PracticeScore = reader.ReadInt32();
        }
    }
}
