using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub()
        {
            this.PracticeFlags = ImmutableDictionary<Level, PlayableStages>.Empty;
            this.StoryFlags = ImmutableDictionary<Level, PlayableStages>.Empty;
            this.Signature = string.Empty;
        }

        public ClearDataStub(IClearData clearData)
        {
            this.Chara = clearData.Chara;
            this.PracticeFlags = clearData.PracticeFlags.ToDictionary();
            this.StoryFlags = clearData.StoryFlags.ToDictionary();
            this.FirstByteOfData = clearData.FirstByteOfData;
            this.Signature = clearData.Signature;
            this.Size1 = clearData.Size1;
            this.Size2 = clearData.Size2;
        }

        public CharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<Level, PlayableStages> PracticeFlags { get; set; }

        public IReadOnlyDictionary<Level, PlayableStages> StoryFlags { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}
