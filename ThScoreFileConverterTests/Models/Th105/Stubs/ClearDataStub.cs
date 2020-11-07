using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105.Stubs
{
    internal class ClearDataStub<TChara> : IClearData<TChara>
        where TChara : struct, Enum
    {
        public ClearDataStub()
        {
            this.CardsForDeck = ImmutableDictionary<int, ICardForDeck>.Empty;
            this.SpellCardResults = ImmutableDictionary<(TChara, int), ISpellCardResult<TChara>>.Empty;
        }

        public IReadOnlyDictionary<int, ICardForDeck> CardsForDeck { get; set; }

        public IReadOnlyDictionary<(TChara Chara, int CardId), ISpellCardResult<TChara>> SpellCardResults { get; set; }
    }
}
