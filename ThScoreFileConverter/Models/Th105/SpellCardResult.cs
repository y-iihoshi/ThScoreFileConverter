//-----------------------------------------------------------------------
// <copyright file="SpellCardResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;

namespace ThScoreFileConverter.Models.Th105
{
    internal class SpellCardResult<TChara, TLevel> : IBinaryReadable, ISpellCardResult<TChara, TLevel>
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        public SpellCardResult()
        {
        }

        public TChara Enemy { get; private set; }

        public TLevel Level { get; private set; }

        public int Id { get; private set; } // 0-based

        public int TrialCount { get; private set; }

        public int GotCount { get; private set; }

        public uint Frames { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.Enemy = Utils.ToEnum<TChara>(reader.ReadInt32());
            this.Level = Utils.ToEnum<TLevel>(reader.ReadInt32());
            this.Id = reader.ReadInt32();
            this.TrialCount = reader.ReadInt32();
            this.GotCount = reader.ReadInt32();
            this.Frames = reader.ReadUInt32();
        }
    }
}
