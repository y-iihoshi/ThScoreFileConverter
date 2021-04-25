using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class ScoreTests
    {
        internal static Mock<IScore> MockScore()
        {
            var items = EnumHelper<ItemWithTotal>.Enumerable;
            var mock = new Mock<IScore>();
            _ = mock.SetupGet(m => m.Signature).Returns("SN");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x314);
            _ = mock.SetupGet(m => m.Number).Returns(69);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                items.ToDictionary(item => item, item => (int)item * 10));
            _ = mock.SetupGet(m => m.ChallengeCounts).Returns(
                items.ToDictionary(item => item, item => (int)item * 100));
            _ = mock.SetupGet(m => m.HighScore).Returns(456789);
            return mock;
        }

        internal static byte[] MakeData(IScore score)
        {
            return TestUtils.MakeByteArray(
                score.Number,
                score.ClearCounts.Values.ToArray(),
                score.ChallengeCounts.Values.ToArray(),
                score.HighScore,
                new byte[0x2A8]);
        }

        internal static byte[] MakeByteArray(IScore score)
        {
            return TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Version,
                score.Checksum,
                score.Size,
                MakeData(score));
        }

        internal static void Validate(IScore expected, IScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Number, actual.Number);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ChallengeCounts.Values, actual.ChallengeCounts.Values);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
        }

        [TestMethod]
        public void ScoreTestChapter()
        {
            var mock = MockScore();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var score = new Score(chapter);

            Validate(mock.Object, score);
            Assert.IsFalse(score.IsValid);
        }

        [TestMethod]
        public void ScoreTestInvalidSignature()
        {
            var mock = MockScore();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Score(chapter));
        }

        [TestMethod]
        public void ScoreTestInvalidVersion()
        {
            var mock = MockScore();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Score(chapter));
        }

        [TestMethod]
        public void ScoreTestInvalidSize()
        {
            var mock = MockScore();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Score(chapter));
        }

        [DataTestMethod]
        [DataRow("SN", (ushort)1, 0x314, true)]
        [DataRow("sn", (ushort)1, 0x314, false)]
        [DataRow("SN", (ushort)0, 0x314, false)]
        [DataRow("SN", (ushort)1, 0x315, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, Score.CanInitialize(chapter));
        }
    }
}
