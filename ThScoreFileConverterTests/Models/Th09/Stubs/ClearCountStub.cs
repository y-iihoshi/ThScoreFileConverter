using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverterTests.Models.Th09.Stubs
{
    internal class ClearCountStub : IClearCount
    {
        public ClearCountStub() { }

        public ClearCountStub(IClearCount clearCount)
            : this()
            => this.Counts = clearCount.Counts?.ToDictionary(pair => pair.Key, pair => pair.Value);

        public IReadOnlyDictionary<Level, int> Counts { get; set; }
    }
}
