using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th07.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CardAttackTests
    {
        internal static CardAttackStub ValidStub { get; } = new CardAttackStub()
        {
            Signature = "CATK",
            Size1 = 0x78,
            Size2 = 0x78,
            MaxBonuses = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (uint)pair.index),
            CardId = 123,
            CardName = TestUtils.MakeRandomArray<byte>(0x30),
            TrialCounts = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (ushort)(10 + pair.index)),
            ClearCounts = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (ushort)(10 - pair.index))
        };

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
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidStub));
            var cardAttack = new CardAttack(chapter);

            Validate(ValidStub, cardAttack);
            Assert.IsTrue(cardAttack.HasTried());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardAttackTestNullChapter()
        {
            _ = new CardAttack(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSignature()
        {
            var stub = new CardAttackStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSize1()
        {
            var stub = new CardAttackStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardAttackTestNotTried()
        {
            var stub = new CardAttackStub(ValidStub)
            {
                TrialCounts = ValidStub.TrialCounts.ToDictionary(
                    pair => pair.Key,
                    pair => (pair.Key == CharaWithTotal.Total) ? (ushort)0 : pair.Value),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var cardAttack = new CardAttack(chapter);

            Validate(stub, cardAttack);
            Assert.IsFalse(cardAttack.HasTried());
        }
    }
}
