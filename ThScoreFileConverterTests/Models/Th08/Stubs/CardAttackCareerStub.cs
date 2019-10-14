using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class CardAttackCareerStub : ICardAttackCareer
    {
        public CardAttackCareerStub() { }

        public CardAttackCareerStub(ICardAttackCareer career)
            : this()
        {
            this.ClearCounts = career.ClearCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.MaxBonuses = career.MaxBonuses?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.TrialCounts = career.TrialCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<CharaWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, int> TrialCounts { get; set; }
    }
}
