using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128.Stubs
{
    internal class CardDataStub : ICardData
    {
        public CardDataStub() { }

        public CardDataStub(ICardData cardData)
            : this()
        {
            this.Cards = cardData.Cards?.ToDictionary(
                pair => pair.Key, pair => new SpellCardStub(pair.Value) as ISpellCard);
            this.Checksum = cardData.Checksum;
            this.IsValid = cardData.IsValid;
            this.Signature = cardData.Signature;
            this.Size = cardData.Size;
            this.Version = cardData.Version;
        }

        public IReadOnlyDictionary<int, ISpellCard> Cards { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
