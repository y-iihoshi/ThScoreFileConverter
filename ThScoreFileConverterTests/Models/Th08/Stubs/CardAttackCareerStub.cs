using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class CardAttackCareerStub : ICardAttackCareer
    {
        public CardAttackCareerStub()
        {
            this.ClearCounts = ImmutableDictionary<CharaWithTotal, int>.Empty;
            this.MaxBonuses = ImmutableDictionary<CharaWithTotal, uint>.Empty;
            this.TrialCounts = ImmutableDictionary<CharaWithTotal, int>.Empty;
        }

        public CardAttackCareerStub(ICardAttackCareer career)
        {
            this.ClearCounts = career.ClearCounts.ToDictionary();
            this.MaxBonuses = career.MaxBonuses.ToDictionary();
            this.TrialCounts = career.TrialCounts.ToDictionary();
        }

        public IReadOnlyDictionary<CharaWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, int> TrialCounts { get; set; }
    }
}
