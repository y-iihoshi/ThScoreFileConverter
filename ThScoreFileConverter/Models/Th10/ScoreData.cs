//-----------------------------------------------------------------------
// <copyright file="ScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th10
{
    using System;
    using System.IO;

    internal class ScoreData : IBinaryReadable
    {
        public uint Score { get; private set; }     // Divided by 10

        public byte ContinueCount { get; private set; }

        public byte[] Name { get; private set; }    // The last 2 bytes are always 0x00 ?

        public uint DateTime { get; private set; }  // UNIX time

        public float SlowRate { get; private set; } // Really...?

        protected byte StageProgressImpl { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Score = reader.ReadUInt32();
            this.StageProgressImpl = reader.ReadByte();
            this.ContinueCount = reader.ReadByte();
            this.Name = reader.ReadExactBytes(10);
            this.DateTime = reader.ReadUInt32();
            this.SlowRate = reader.ReadSingle();
        }
    }
}
