using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128CardDataTests
    {
        internal static CardDataStub ValidStub => new CardDataStub()
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
                cardData.Cards.Values.SelectMany(card => Th128SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray(ICardData cardData)
            => TestUtils.MakeByteArray(
                cardData.Signature.ToCharArray(),
                cardData.Version,
                cardData.Checksum,
                cardData.Size,
                MakeData(cardData));

        internal static void Validate(ICardData expected, in Th128CardDataWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);

            foreach (var pair in expected.Cards)
            {
                Th128SpellCardTests.Validate(pair.Value, actual.CardsItem(pair.Key));
            }
        }

        [TestMethod]
        public void Th128CardDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var clearData = new Th128CardDataWrapper(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128CardDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th128CardDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128CardDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new CardDataStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th128CardDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128CardDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new CardDataStub(ValidStub);
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th128CardDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128CardDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new CardDataStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th128CardDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CD", (ushort)1, 0x947C, true)]
        [DataRow("cd", (ushort)1, 0x947C, false)]
        [DataRow("CD", (ushort)0, 0x947C, false)]
        [DataRow("CD", (ushort)1, 0x947D, false)]
        public void Th128CardDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(
                    expected, Th128CardDataWrapper.CanInitialize(chapter));
            });
    }
}
