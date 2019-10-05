using System.Collections.Generic;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverterTests.Models.Th075.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public IReadOnlyList<short> CardGotCount { get; set; }

        public IReadOnlyList<short> CardTrialCount { get; set; }

        public IReadOnlyList<byte> CardTrulyGot { get; set; }

        public int ClearCount { get; set; }

        public IReadOnlyList<int> MaxBonuses { get; set; }

        public int MaxCombo { get; set; }

        public int MaxDamage { get; set; }

        public IReadOnlyList<IHighScore> Ranking { get; set; }

        public int UseCount { get; set; }
    }
}
