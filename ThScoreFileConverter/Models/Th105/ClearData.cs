//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;

namespace ThScoreFileConverter.Models.Th105
{
    internal class ClearData<TChara> : IBinaryReadable, IClearData<TChara>  // per character
        where TChara : struct, Enum
    {
        private Dictionary<int, ICardForDeck> cardsForDeck;
        private Dictionary<(TChara Chara, int CardId), ISpellCardResult<TChara>> spellCardResults;

        public ClearData()
        {
            this.cardsForDeck = new Dictionary<int, ICardForDeck>();
            this.spellCardResults = new Dictionary<(TChara Chara, int CardId), ISpellCardResult<TChara>>();
        }

        public IReadOnlyDictionary<int, ICardForDeck> CardsForDeck => this.cardsForDeck;

        public IReadOnlyDictionary<(TChara Chara, int CardId), ISpellCardResult<TChara>> SpellCardResults
            => this.spellCardResults;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var numCards = reader.ReadInt32();
            this.cardsForDeck = new Dictionary<int, ICardForDeck>(numCards);
            for (var index = 0; index < numCards; index++)
            {
                var card = new CardForDeck();
                card.ReadFrom(reader);
                if (!this.cardsForDeck.ContainsKey(card.Id))
                    this.cardsForDeck.Add(card.Id, card);
            }

            var numResults = reader.ReadInt32();
            this.spellCardResults = new Dictionary<(TChara, int), ISpellCardResult<TChara>>(numResults);
            for (var index = 0; index < numResults; index++)
            {
                var result = new SpellCardResult<TChara>();
                result.ReadFrom(reader);
                var key = (result.Enemy, result.Id);
                if (!this.spellCardResults.ContainsKey(key))
                    this.spellCardResults.Add(key, result);
            }
        }
    }
}
