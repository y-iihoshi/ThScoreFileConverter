using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CardAttackTests
    {
        internal static Mock<ICardAttack> MockCardAttack()
        {
            var pairs = EnumHelper<CharaWithTotal>.Enumerable.Select((chara, index) => (chara, index));
            var mock = new Mock<ICardAttack>();

            _ = mock.SetupGet(m => m.Signature).Returns("CATK");
            _ = mock.SetupGet(m => m.Size1).Returns(0x78);
            _ = mock.SetupGet(m => m.Size2).Returns(0x78);
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(
                pairs.ToDictionary(pair => pair.chara, pair => (uint)pair.index));
            _ = mock.SetupGet(m => m.CardId).Returns(123);
            _ = mock.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock.SetupGet(m => m.TrialCounts).Returns(
                pairs.ToDictionary(pair => pair.chara, pair => (ushort)(10 + pair.index)));
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                pairs.ToDictionary(pair => pair.chara, pair => (ushort)(10 - pair.index)));

            var hasTried = mock.Object.TrialCounts.TryGetValue(CharaWithTotal.Total, out var count) && (count > 0);
            _ = mock.Setup(m => m.HasTried()).Returns(hasTried);

            return mock;
        }

        internal static byte[] MakeByteArray(ICardAttack cardAttack)
            => TestUtils.MakeByteArray(
                cardAttack.Signature.ToCharArray(),
                cardAttack.Size1,
                cardAttack.Size2,
                0u,
                cardAttack.MaxBonuses.Values.ToArray(),
                (short)(cardAttack.CardId - 1),
                (byte)0,
                cardAttack.CardName,
                (byte)0,
                cardAttack.TrialCounts.Values.ToArray(),
                cardAttack.ClearCounts.Values.ToArray());

        internal static void Validate(ICardAttack expected, ICardAttack actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.MaxBonuses.Values, actual.MaxBonuses.Values);
            Assert.AreEqual(expected.CardId, actual.CardId);
            CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
            CollectionAssert.That.AreEqual(expected.TrialCounts.Values, actual.TrialCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
        }

        [TestMethod]
        public void CardAttackTestChapter()
        {
            var mock = MockCardAttack();
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var cardAttack = new CardAttack(chapter);

            Validate(mock.Object, cardAttack);
            Assert.IsTrue(cardAttack.HasTried());
        }

        [TestMethod]
        public void CardAttackTestInvalidSignature()
        {
            var mock = MockCardAttack();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new CardAttack(chapter));
        }

        [TestMethod]
        public void CardAttackTestInvalidSize1()
        {
            var mock = MockCardAttack();
            var size = mock.Object.Size1;
            _ = mock.SetupGet(m => m.Size1).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new CardAttack(chapter));
        }

        [TestMethod]
        public void CardAttackTestNotTried()
        {
            var mock = MockCardAttack();
            var trialCounts = mock.Object.TrialCounts;
            _ = mock.SetupGet(m => m.TrialCounts).Returns(
                trialCounts.ToDictionary(
                    pair => pair.Key,
                    pair => (pair.Key == CharaWithTotal.Total) ? (ushort)0 : pair.Value));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var cardAttack = new CardAttack(chapter);

            Validate(mock.Object, cardAttack);
            Assert.IsFalse(cardAttack.HasTried());
        }
    }
}
