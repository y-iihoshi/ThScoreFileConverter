using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105.Wrappers
{
    public sealed class StageInfoWrapper<TStage, TChara>
        where TStage : struct, Enum
        where TChara : struct, Enum
    {
        private readonly StageInfo<TStage, TChara> original = null;

        public StageInfoWrapper(TStage stage, TChara enemy, IEnumerable<int> cardIds)
            => this.original = new StageInfo<TStage, TChara>(stage, enemy, cardIds);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target => this.original;
        public TStage? Stage => this.original.Stage;
        public TChara? Enemy => this.original.Enemy;
        public IReadOnlyCollection<int> CardIds => this.original.CardIds;
    }
}
