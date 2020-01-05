using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverterTests.Models.Th09.Stubs
{
    internal class ClearCountStub : IClearCount
    {
        public ClearCountStub()
        {
            this.Counts = ImmutableDictionary<Level, int>.Empty;
        }

        public ClearCountStub(IClearCount clearCount)
        {
            this.Counts = clearCount.Counts.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IReadOnlyDictionary<Level, int> Counts { get; set; }
    }
}
