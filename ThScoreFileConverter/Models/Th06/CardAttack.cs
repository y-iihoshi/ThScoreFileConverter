//-----------------------------------------------------------------------
// <copyright file="CardAttack.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th06
{
    internal class CardAttack : Chapter, ICardAttack    // per card
    {
        public const string ValidSignature = "CATK";
        public const short ValidSize = 0x0040;

        public CardAttack(Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            _ = reader.ReadExactBytes(8);
            this.CardId = (short)(reader.ReadInt16() + 1);
            _ = reader.ReadExactBytes(6);
            this.CardName = reader.ReadExactBytes(0x24);
            this.TrialCount = reader.ReadUInt16();
            this.ClearCount = reader.ReadUInt16();
        }

        public short CardId { get; }    // 1-based

        public IEnumerable<byte> CardName { get; }  // Null-terminated

        public ushort TrialCount { get; }

        public ushort ClearCount { get; }

        public bool HasTried => this.TrialCount > 0;
    }
}
