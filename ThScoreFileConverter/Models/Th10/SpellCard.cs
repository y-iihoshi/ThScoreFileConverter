//-----------------------------------------------------------------------
// <copyright file="SpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th10
{
    using System;
    using System.IO;

    internal class SpellCard : IBinaryReadable
    {
        // [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
        public byte[] Name { get; private set; }

        public int ClearCount { get; private set; }

        public int TrialCount { get; private set; }

        public int Id { get; private set; } // 1-based

        public ThConverter.Level Level { get; private set; }

        public bool HasTried => this.TrialCount > 0;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Name = reader.ReadExactBytes(0x80);
            this.ClearCount = reader.ReadInt32();
            this.TrialCount = reader.ReadInt32();
            this.Id = reader.ReadInt32() + 1;
            this.Level = Utils.ToEnum<ThConverter.Level>(reader.ReadInt32());
        }
    }
}
