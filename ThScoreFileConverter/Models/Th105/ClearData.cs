//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th105
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class ClearData<TChara, TLevel> : IBinaryReadable   // per character
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        public ClearData()
        {
            this.CardsForDeck = null;
            this.SpellCardResults = null;
        }

        public Dictionary<int, CardForDeck> CardsForDeck { get; private set; }

        public Dictionary<(TChara Chara, int CardId), SpellCardResult<TChara, TLevel>> SpellCardResults { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var numCards = reader.ReadInt32();
            this.CardsForDeck = new Dictionary<int, CardForDeck>(numCards);
            for (var index = 0; index < numCards; index++)
            {
                var card = new CardForDeck();
                card.ReadFrom(reader);
                if (!this.CardsForDeck.ContainsKey(card.Id))
                    this.CardsForDeck.Add(card.Id, card);
            }

            var numResults = reader.ReadInt32();
            this.SpellCardResults = new Dictionary<(TChara, int), SpellCardResult<TChara, TLevel>>(numResults);
            for (var index = 0; index < numResults; index++)
            {
                var result = new SpellCardResult<TChara, TLevel>();
                result.ReadFrom(reader);
                var key = (result.Enemy, result.Id);
                if (!this.SpellCardResults.ContainsKey(key))
                    this.SpellCardResults.Add(key, result);
            }
        }
    }
}
