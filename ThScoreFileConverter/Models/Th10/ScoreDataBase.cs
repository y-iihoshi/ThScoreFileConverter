//-----------------------------------------------------------------------
// <copyright file="ScoreDataBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th10
{
    internal class ScoreDataBase<TStageProgress> : IBinaryReadable, IScoreData<TStageProgress>
        where TStageProgress : struct, Enum
    {
        public uint Score { get; private set; }     // Divided by 10

        public TStageProgress StageProgress { get; private set; }

        public byte ContinueCount { get; private set; }

        public IEnumerable<byte> Name { get; private set; } // The last 2 bytes are always 0x00 ?

        public uint DateTime { get; private set; }  // UNIX time

        public float SlowRate { get; private set; } // Really...?

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Score = reader.ReadUInt32();
            this.StageProgress = Utils.ToEnum<TStageProgress>(reader.ReadByte());
            this.ContinueCount = reader.ReadByte();
            this.Name = reader.ReadExactBytes(10);
            this.DateTime = reader.ReadUInt32();
            this.SlowRate = reader.ReadSingle();
        }
    }
}
