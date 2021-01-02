using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th165;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class ScoreTests
    {
        internal static Mock<IScore> MockScore()
        {
            var mock = new Mock<IScore>();
            _ = mock.SetupGet(m => m.Signature).Returns("SN");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x234);
            _ = mock.SetupGet(m => m.Number).Returns(12);
            _ = mock.SetupGet(m => m.ClearCount).Returns(34);
            _ = mock.SetupGet(m => m.ChallengeCount).Returns(56);
            _ = mock.SetupGet(m => m.NumPhotos).Returns(78);
            _ = mock.SetupGet(m => m.HighScore).Returns(1234567);
            return mock;
        }

        internal static byte[] MakeData(IScore score)
            => TestUtils.MakeByteArray(
                score.Number,
                score.ClearCount,
                0,
                score.ChallengeCount,
                score.NumPhotos,
                score.HighScore,
                new byte[0x210]);

        internal static byte[] MakeByteArray(IScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Version,
                score.Checksum,
                score.Size,
                MakeData(score));

        internal static void Validate(IScore expected, IScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Number, actual.Number);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
            Assert.AreEqual(expected.ChallengeCount, actual.ChallengeCount);
            Assert.AreEqual(expected.NumPhotos, actual.NumPhotos);
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
        [DataRow("SN", (ushort)1, 0x234, true)]
        [DataRow("sn", (ushort)1, 0x234, false)]
        [DataRow("SN", (ushort)0, 0x234, false)]
        [DataRow("SN", (ushort)1, 0x235, false)]
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
