//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th105;

internal sealed class ClearData<TChara> : IBinaryReadable, IClearData<TChara>  // per character
    where TChara : struct, Enum
{
    private Dictionary<int, ICardForDeck> cardsForDeck;
    private Dictionary<(TChara Chara, int CardId), ISpellCardResult<TChara>> spellCardResults;

    public ClearData()
    {
        this.cardsForDeck = [];
        this.spellCardResults = [];
    }

    public IReadOnlyDictionary<int, ICardForDeck> CardsForDeck => this.cardsForDeck;

    public IReadOnlyDictionary<(TChara Chara, int CardId), ISpellCardResult<TChara>> SpellCardResults
        => this.spellCardResults;

    public void ReadFrom(BinaryReader reader)
    {
        var numCards = reader.ReadInt32();
        this.cardsForDeck = new Dictionary<int, ICardForDeck>(numCards);
        for (var index = 0; index < numCards; index++)
        {
            var card = BinaryReadableHelper.Create<CardForDeck>(reader);
            _ = this.cardsForDeck.TryAdd(card.Id, card);
        }

        var numResults = reader.ReadInt32();
        this.spellCardResults = new Dictionary<(TChara, int), ISpellCardResult<TChara>>(numResults);
        for (var index = 0; index < numResults; index++)
        {
            var result = BinaryReadableHelper.Create<SpellCardResult<TChara>>(reader);
            var key = (result.Enemy, result.Id);
            _ = this.spellCardResults.TryAdd(key, result);
        }
    }
}
