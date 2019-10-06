using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models.Th13.Stubs
{
    internal class ClearDataStub<
        TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice, TStageProgress>
        : IClearData<TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice, TStageProgress>
        where TCharaWithTotal : struct, Enum
        where TLevel : struct, Enum
        where TLevelPractice : struct, Enum
        where TLevelPracticeWithTotal : struct, Enum
        where TStagePractice : struct, Enum
        where TStageProgress : struct, Enum
    {
        public IReadOnlyDictionary<int, ISpellCard<TLevel>> Cards { get; set; }

        public TCharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearCounts { get; set; }

        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearFlags { get; set; }

        public int PlayTime { get; set; }

        public IReadOnlyDictionary<(TLevelPractice, TStagePractice), IPractice> Practices { get; set; }

        public IReadOnlyDictionary<
            TLevelPracticeWithTotal,
            IReadOnlyList<ThScoreFileConverter.Models.Th10.IScoreData<TStageProgress>>> Rankings { get; set; }

        public int TotalPlayCount { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
