using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128.Stubs
{
    internal class SpellCardStub : ISpellCard
    {
        public SpellCardStub()
        {
            this.Name = Enumerable.Empty<byte>();
        }

        public SpellCardStub(ISpellCard spellCard)
        {
            this.Id = spellCard.Id;
            this.Level = spellCard.Level;
            this.Name = spellCard.Name.ToArray();
            this.NoIceCount = spellCard.NoIceCount;
            this.NoMissCount = spellCard.NoMissCount;
            this.TrialCount = spellCard.TrialCount;
        }

        public int Id { get; set; }

        public Level Level { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public int NoIceCount { get; set; }

        public int NoMissCount { get; set; }

        public int TrialCount { get; set; }

        public bool HasTried() => this.TrialCount > 0;
    }
}
