using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverterTests.Models.Th075.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub()
        {
            this.CardGotCount = Array.Empty<short>();
            this.CardTrialCount = Array.Empty<short>();
            this.CardTrulyGot = Array.Empty<byte>();
            this.MaxBonuses = Array.Empty<int>();
            this.Ranking = Array.Empty<IHighScore>();
        }

        public ClearDataStub(IClearData clearData)
        {
            this.CardGotCount = clearData.CardGotCount.ToList();
            this.CardTrialCount = clearData.CardTrialCount.ToList();
            this.CardTrulyGot = clearData.CardTrulyGot.ToList();
            this.ClearCount = clearData.ClearCount;
            this.MaxBonuses = clearData.MaxBonuses.ToList();
            this.MaxCombo = clearData.MaxCombo;
            this.MaxDamage = clearData.MaxDamage;
            this.Ranking = clearData.Ranking.ToList();
            this.UseCount = clearData.UseCount;
        }

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
