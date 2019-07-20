using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th105CardForDeckWrapper
    {
        private readonly CardForDeck original = null;

        public static Th105CardForDeckWrapper Create(byte[] array)
        {
            var cardForDeck = new Th105CardForDeckWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    cardForDeck.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return cardForDeck;
        }

        public Th105CardForDeckWrapper() => this.original = new CardForDeck();

        public object Target => this.original;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int? Id => this.original.Id;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int? MaxNumber => this.original.MaxNumber;

        public void ReadFrom(BinaryReader reader) => this.original.ReadFrom(reader);
    }
}
