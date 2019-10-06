using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th15;

namespace ThScoreFileConverterTests.Models.Th15.Stubs
{
    internal class ClearDataPerGameModeStub : IClearDataPerGameMode
    {
        public ClearDataPerGameModeStub() { }

        public ClearDataPerGameModeStub(IClearDataPerGameMode clearData)
            : this()
        {
            this.Cards = clearData.Cards?.ToDictionary(
                pair => pair.Key,
                pair => new Th13.Stubs.SpellCardStub<Level>(pair.Value) as ISpellCard<Level>);
            this.ClearCounts = clearData.ClearCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ClearFlags = clearData.ClearFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.PlayTime = clearData.PlayTime;
            this.Rankings = clearData.Rankings?.ToDictionary(
                pair => pair.Key,
                pair => pair.Value?.Select(score => new ScoreDataStub(score))?.ToList() as IReadOnlyList<IScoreData>);
            this.TotalPlayCount = clearData.TotalPlayCount;
        }

        public IReadOnlyDictionary<int, ISpellCard<Level>> Cards { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; set; }

        public int TotalPlayCount { get; set; }
    }
}
