using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CardDataTests
    {
        internal static CardDataStub ValidStub { get; } = new CardDataStub()
        {
            Signature = "CD",
            Version = 1,
            Checksum = 0u,
            Size = 0x947C,
            Cards = Enumerable.Range(1, 250).ToDictionary(
                index => index,
                index => new SpellCardStub()
                {
                    Name = TestUtils.MakeRandomArray<byte>(0x80),
                    NoMissCount = 123 + index,
                    NoIceCount = 456 + index,
                    TrialCount = 789 + index,
                    Id = index,
                    Level = Level.Hard
                } as ISpellCard)
        };

        internal static byte[] MakeData(ICardData cardData)
            => TestUtils.MakeByteArray(
                cardData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray(ICardData cardData)
            => TestUtils.MakeByteArray(
                cardData.Signature.ToCharArray(),
                cardData.Version,
                cardData.Checksum,
                cardData.Size,
                MakeData(cardData));

        internal static void Validate(ICardData expected, ICardData actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);

            foreach (var pair in expected.Cards)
            {
                SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
            }
        }

        [TestMethod]
        public void CardDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var clearData = new CardData(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new CardData(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new CardDataStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new CardDataStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new CardDataStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("CD", (ushort)1, 0x947C, true)]
        [DataRow("cd", (ushort)1, 0x947C, false)]
        [DataRow("CD", (ushort)0, 0x947C, false)]
        [DataRow("CD", (ushort)1, 0x947D, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected) => TestUtils.Wrap(() =>
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(
                expected, CardData.CanInitialize(chapter));
        });
    }
}
