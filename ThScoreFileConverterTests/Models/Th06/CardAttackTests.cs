using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CardAttackTests
    {
        internal static Mock<ICardAttack> MockCardAttack()
        {
            var attack = new Mock<ICardAttack>();
            _ = attack.SetupGet(m => m.Signature).Returns("CATK");
            _ = attack.SetupGet(m => m.Size1).Returns(0x40);
            _ = attack.SetupGet(m => m.Size2).Returns(0x40);
            _ = attack.SetupGet(m => m.CardId).Returns(23);
            _ = attack.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
            _ = attack.SetupGet(m => m.TrialCount).Returns(789);
            _ = attack.SetupGet(m => m.ClearCount).Returns(456);
            _ = attack.Setup(m => m.HasTried()).Returns(true);
            return attack;
        }

        internal static byte[] MakeByteArray(ICardAttack cardAttack)
            => TestUtils.MakeByteArray(
                cardAttack.Signature.ToCharArray(),
                cardAttack.Size1,
                cardAttack.Size2,
                new byte[8],
                (short)(cardAttack.CardId - 1),
                new byte[6],
                cardAttack.CardName.ToArray(),
                cardAttack.TrialCount,
                cardAttack.ClearCount);

        internal static void Validate(ICardAttack expected, ICardAttack actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.CardId, actual.CardId);
            CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
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
        public void CardAttackTestNullChapter()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CardAttack(null!));

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
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
            _ = mock.SetupGet(m => m.TrialCount).Returns(0);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var cardAttack = new CardAttack(chapter);

            Validate(mock.Object, cardAttack);
            Assert.IsFalse(cardAttack.HasTried());
        }
    }
}
