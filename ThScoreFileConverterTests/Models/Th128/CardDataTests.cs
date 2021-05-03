using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CardDataTests
    {
        internal static Mock<ICardData> MockCardData()
        {
            static ISpellCard CreateSpellCard(int index)
            {
                var mock = new Mock<ISpellCard>();
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
                _ = mock.SetupGet(s => s.NoMissCount).Returns(123 + index);
                _ = mock.SetupGet(s => s.NoIceCount).Returns(456 + index);
                _ = mock.SetupGet(s => s.TrialCount).Returns(789 + index);
                _ = mock.SetupGet(s => s.Id).Returns(index);
                _ = mock.SetupGet(s => s.Level).Returns(Level.Hard);
                _ = mock.SetupGet(s => s.HasTried).Returns(true);
                return mock.Object;
            }

            var mock = new Mock<ICardData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CD");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x947C);
            _ = mock.SetupGet(m => m.Cards).Returns(
                Enumerable.Range(1, 250).ToDictionary(index => index, index => CreateSpellCard(index)));
            return mock;
        }

        internal static byte[] MakeData(ICardData cardData)
        {
            return TestUtils.MakeByteArray(
                cardData.Cards.Values.SelectMany(card => SpellCardTests.MakeByteArray(card)));
        }

        internal static byte[] MakeByteArray(ICardData cardData)
        {
            return TestUtils.MakeByteArray(
                cardData.Signature.ToCharArray(),
                cardData.Version,
                cardData.Checksum,
                cardData.Size,
                MakeData(cardData));
        }

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
        public void CardDataTestChapter()
        {
            var mock = MockCardData();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var clearData = new CardData(chapter);

            Validate(mock.Object, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        public void CardDataTestInvalidSignature()
        {
            var mock = MockCardData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new CardData(chapter));
        }

        [TestMethod]
        public void CardDataTestInvalidVersion()
        {
            var mock = MockCardData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new CardData(chapter));
        }

        [TestMethod]
        public void CardDataTestInvalidSize()
        {
            var mock = MockCardData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new CardData(chapter));
        }

        [DataTestMethod]
        [DataRow("CD", (ushort)1, 0x947C, true)]
        [DataRow("cd", (ushort)1, 0x947C, false)]
        [DataRow("CD", (ushort)0, 0x947C, false)]
        [DataRow("CD", (ushort)1, 0x947D, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(
                expected, CardData.CanInitialize(chapter));
        }
    }
}
