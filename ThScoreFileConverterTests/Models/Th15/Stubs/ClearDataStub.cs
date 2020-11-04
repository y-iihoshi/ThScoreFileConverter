using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;

namespace ThScoreFileConverterTests.Models.Th15.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub()
        {
            this.GameModeData = ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty;
            this.Practices = ImmutableDictionary<(Level, StagePractice), IPractice>.Empty;
            this.Signature = string.Empty;
        }

        public CharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<GameMode, IClearDataPerGameMode> GameModeData { get; set; }

        public IReadOnlyDictionary<(Level, StagePractice), IPractice> Practices { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
