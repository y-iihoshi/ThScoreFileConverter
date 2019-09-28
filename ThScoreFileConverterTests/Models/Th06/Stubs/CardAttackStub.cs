using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Th06.Stubs
{
    internal class CardAttackStub : ICardAttack
    {
        public CardAttackStub() { }

        public CardAttackStub(ICardAttack cardAttack)
            : this()
        {
            this.CardId = cardAttack.CardId;
            this.CardName = cardAttack.CardName?.ToArray();
            this.ClearCount = cardAttack.ClearCount;
            this.TrialCount = cardAttack.TrialCount;
            this.FirstByteOfData = cardAttack.FirstByteOfData;
            this.Signature = cardAttack.Signature;
            this.Size1 = cardAttack.Size1;
            this.Size2 = cardAttack.Size2;
        }

        public short CardId { get; set; }

        public IEnumerable<byte> CardName { get; set; }

        public ushort ClearCount { get; set; }

        public ushort TrialCount { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }

        public bool HasTried() => this.TrialCount > 0;
    }
}
