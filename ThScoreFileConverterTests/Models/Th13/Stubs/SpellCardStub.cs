using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models.Th13.Stubs
{
    internal class SpellCardStub<TLevel> : ISpellCard<TLevel>
        where TLevel : struct, Enum
    {
        public SpellCardStub() { }

        public SpellCardStub(ISpellCard<TLevel> spellCard)
            : this()
        {
            this.PracticeClearCount = spellCard.PracticeClearCount;
            this.PracticeScore = spellCard.PracticeScore;
            this.PracticeTrialCount = spellCard.PracticeTrialCount;
            this.ClearCount = spellCard.ClearCount;
            this.HasTried = spellCard.HasTried;
            this.Id = spellCard.Id;
            this.Level = spellCard.Level;
            this.Name = spellCard.Name?.ToArray();
            this.TrialCount = spellCard.TrialCount;
        }

        public int PracticeClearCount { get; set; }

        public int PracticeScore { get; set; }

        public int PracticeTrialCount { get; set; }

        public int ClearCount { get; set; }

        public bool HasTried { get; set; }

        public int Id { get; set; }

        public TLevel Level { get; set; }

        public IEnumerable<byte> Name { get; set; }

        public int TrialCount { get; set; }
    }
}
