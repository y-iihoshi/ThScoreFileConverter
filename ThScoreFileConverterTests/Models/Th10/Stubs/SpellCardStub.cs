using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Stubs
{
    internal class SpellCardStub : ISpellCard<Level>
    {
        public SpellCardStub()
        {
            this.Name = Enumerable.Empty<byte>();
        }

        public SpellCardStub(ISpellCard<Level> spellCard)
        {
            this.ClearCount = spellCard.ClearCount;
            this.HasTried = spellCard.HasTried;
            this.Id = spellCard.Id;
            this.Level = spellCard.Level;
            this.Name = spellCard.Name.ToArray();
            this.TrialCount = spellCard.TrialCount;
        }

        public int ClearCount { get; set; }

        public bool HasTried { get; set; }

        public int Id { get; set; }

        public Level Level { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public int TrialCount { get; set; }
    }
}
