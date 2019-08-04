using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th10PracticeWrapper
    {
        private readonly Practice original = null;

        public static Th10PracticeWrapper Create(byte[] array)
        {
            var practice = new Th10PracticeWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    practice.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return practice;
        }

        public Th10PracticeWrapper() => this.original = new Practice();

        public object Target => this.original;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public uint? Score => this.original.Score;
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public uint? StageFlag => this.original.StageFlag;

        public void ReadFrom(BinaryReader reader) => this.original.ReadFrom(reader);
    }
}
