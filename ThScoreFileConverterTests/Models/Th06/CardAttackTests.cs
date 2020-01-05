using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Stubs;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CardAttackTests
    {
        internal static CardAttackStub ValidStub { get; } = new CardAttackStub()
        {
            Signature = "CATK",
            Size1 = 0x40,
            Size2 = 0x40,
            CardId = 23,
            CardName = TestUtils.MakeRandomArray<byte>(0x24),
            TrialCount = 789,
            ClearCount = 456
        };

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
        public void CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidStub));
            var cardAttack = new CardAttack(chapter);

            Validate(ValidStub, cardAttack);
            Assert.IsTrue(cardAttack.HasTried());
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new CardAttack(null!);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub)
            {
                TrialCount = 0,
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var cardAttack = new CardAttack(chapter);

            Validate(stub, cardAttack);
            Assert.IsFalse(cardAttack.HasTried());
        });
    }
}
