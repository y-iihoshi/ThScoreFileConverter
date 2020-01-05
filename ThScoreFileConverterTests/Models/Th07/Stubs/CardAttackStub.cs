using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Th07.Stubs
{
    internal class CardAttackStub : ICardAttack
    {
        public CardAttackStub()
        {
            this.CardName = Enumerable.Empty<byte>();
            this.ClearCounts = ImmutableDictionary<CharaWithTotal, ushort>.Empty;
            this.MaxBonuses = ImmutableDictionary<CharaWithTotal, uint>.Empty;
            this.TrialCounts = ImmutableDictionary<CharaWithTotal, ushort>.Empty;
            this.Signature = string.Empty;
        }

        public CardAttackStub(ICardAttack cardAttack)
        {
            this.CardId = cardAttack.CardId;
            this.CardName = cardAttack.CardName.ToArray();
            this.ClearCounts = cardAttack.ClearCounts.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.MaxBonuses = cardAttack.MaxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.TrialCounts = cardAttack.TrialCounts.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.FirstByteOfData = cardAttack.FirstByteOfData;
            this.Signature = cardAttack.Signature;
            this.Size1 = cardAttack.Size1;
            this.Size2 = cardAttack.Size2;
        }

        public short CardId { get; set; }

        public IEnumerable<byte> CardName { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, ushort> ClearCounts { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; set; }

        public IReadOnlyDictionary<CharaWithTotal, ushort> TrialCounts { get; set; }

        public byte FirstByteOfData { get; set; }

        public string Signature { get; set; }

        public short Size1 { get; set; }

        public short Size2 { get; set; }

        public bool HasTried() => this.TrialCounts.TryGetValue(CharaWithTotal.Total, out var count) && count > 0;
    }
}
