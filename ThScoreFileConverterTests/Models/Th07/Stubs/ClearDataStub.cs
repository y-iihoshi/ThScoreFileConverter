using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Th07.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub() { }

        public ClearDataStub(IClearData clearData)
            : this()
        {
            this.Chara = clearData.Chara;
            this.PracticeFlags = clearData.PracticeFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.StoryFlags = clearData.StoryFlags?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.FirstByteOfData = clearData.FirstByteOfData;
            this.Signature = clearData.Signature;
            this.Size1 = clearData.Size1;
            this.Size2 = clearData.Size2;
        }

        public Chara Chara { get; set; }

        public IReadOnlyDictionary<Level, byte> PracticeFlags { get; set; }

        public IReadOnlyDictionary<Level, byte> StoryFlags { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }
    }
}
