using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models.Th13.Stubs
{
    internal class SpellCardStub<TLevel> : ISpellCard<TLevel>
        where TLevel : struct, Enum
    {
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
