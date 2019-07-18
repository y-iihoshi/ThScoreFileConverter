//-----------------------------------------------------------------------
// <copyright file="CardForDeck.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th105
{
    using System;
    using System.IO;

    internal class CardForDeck : IBinaryReadable
    {
        public CardForDeck()
        {
        }

        public int Id { get; private set; } // 0-based

        public int MaxNumber { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.Id = reader.ReadInt32();
            this.MaxNumber = reader.ReadInt32();
        }
    }
}
