using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class CardAttackTests
    {
        internal static CardAttackStub ValidStub => new CardAttackStub()
        {
            Signature = "CATK",
            Size1 = 0x40,
            Size2 = 0x40,
            CardId = 23,
            CardName = TestUtils.MakeRandomArray<byte>(0x24),
            TrialCount = 789,
            ClearCount = 456
        };

        internal static byte[] MakeByteArray(in ICardAttack cardAttack)
            => TestUtils.MakeByteArray(
                cardAttack.Signature.ToCharArray(),
                cardAttack.Size1,
                cardAttack.Size2,
                new byte[8],
                (short)(cardAttack.CardId - 1),
                new byte[6],
                cardAttack.CardName,
                cardAttack.TrialCount,
                cardAttack.ClearCount);

        internal static void Validate(in ICardAttack expected, in ICardAttack actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.CardId, actual.CardId);
            CollectionAssert.AreEqual(expected.CardName.ToArray(), actual.CardName.ToArray());
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
        }

        [TestMethod]
        public void CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var cardAttack = new CardAttack(chapter.Target);

            Validate(stub, cardAttack);
            Assert.IsTrue(cardAttack.HasTried());
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new CardAttack(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new CardAttack(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new CardAttack(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.TrialCount = 0;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var cardAttack = new CardAttack(chapter.Target);

            Validate(stub, cardAttack);
            Assert.IsFalse(cardAttack.HasTried());
        });
    }
}
