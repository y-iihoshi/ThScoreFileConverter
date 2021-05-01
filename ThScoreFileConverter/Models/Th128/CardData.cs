//-----------------------------------------------------------------------
// <copyright file="CardData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th128
{
    internal class CardData : Th10.Chapter, ICardData
    {
        public const string ValidSignature = "CD";
        public const ushort ValidVersion = 0x0001;
        public const int ValidSize = 0x0000947C;

        public CardData(Th10.Chapter chapter)
            : base(chapter, ValidSignature, ValidVersion, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            this.Cards = Enumerable.Range(0, Definitions.CardTable.Count)
                .Select(_ => BinaryReadableHelper.Create<SpellCard>(reader) as ISpellCard)
                .ToDictionary(card => card.Id);
        }

        public IReadOnlyDictionary<int, ISpellCard> Cards { get; }

        public static bool CanInitialize(Th10.Chapter chapter)
        {
            return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                && (chapter.Version == ValidVersion)
                && (chapter.Size == ValidSize);
        }
    }
}
