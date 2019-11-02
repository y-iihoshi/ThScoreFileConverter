using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105.Stubs
{
    internal class ClearDataStub<TChara, TLevel> : IClearData<TChara, TLevel>
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        public ClearDataStub() { }

        public ClearDataStub(IClearData<TChara, TLevel> clearData)
            : base()
        {
            this.CardsForDeck = clearData.CardsForDeck.ToDictionary(
                pair => pair.Key,
                pair => new CardForDeckStub(pair.Value) as ICardForDeck);
            this.SpellCardResults = clearData.SpellCardResults.ToDictionary(
                pair => pair.Key,
                pair => new SpellCardResultStub<TChara, TLevel>(pair.Value) as ISpellCardResult<TChara, TLevel>);
        }

        public IReadOnlyDictionary<int, ICardForDeck> CardsForDeck { get; set; }

        public IReadOnlyDictionary<(TChara Chara, int CardId), ISpellCardResult<TChara, TLevel>> SpellCardResults { get; set; }
    }
}
