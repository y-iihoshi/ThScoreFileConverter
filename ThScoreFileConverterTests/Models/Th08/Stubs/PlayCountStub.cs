using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class PlayCountStub : IPlayCount
    {
        public PlayCountStub() { }

        public PlayCountStub(IPlayCount playCount)
            : this()
        {
            this.TotalClear = playCount.TotalClear;
            this.TotalContinue = playCount.TotalContinue;
            this.TotalPractice = playCount.TotalPractice;
            this.TotalTrial = playCount.TotalTrial;
            this.Trials = playCount.Trials.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public int TotalClear { get; set; }

        public int TotalContinue { get; set; }

        public int TotalPractice { get; set; }

        public int TotalTrial { get; set; }

        public IReadOnlyDictionary<Th08Converter.Chara, int> Trials { get; set; }
    }
}
