using System;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105.Stubs
{
    internal class SpellCardResultStub<TChara> : ISpellCardResult<TChara>
        where TChara : struct, Enum
    {
        public SpellCardResultStub() { }

        public SpellCardResultStub(ISpellCardResult<TChara> result)
            : base()
        {
            this.Enemy = result.Enemy;
            this.Frames = result.Frames;
            this.GotCount = result.GotCount;
            this.Id = result.Id;
            this.Level = result.Level;
            this.TrialCount = result.TrialCount;
        }

        public TChara Enemy { get; set; }

        public uint Frames { get; set; }

        public int GotCount { get; set; }

        public int Id { get; set; }

        public Level Level { get; set; }

        public int TrialCount { get; set; }
    }
}
