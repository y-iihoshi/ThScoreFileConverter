using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Th165.Stubs;
using ThScoreFileConverter.Models.Th165;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class ScoreTests
    {
        internal static ScoreStub ValidStub { get; } = new ScoreStub()
        {
            Signature = "SN",
            Version = 1,
            Checksum = 0u,
            Size = 0x234,
            Number = 12,
            ClearCount = 34,
            ChallengeCount = 56,
            NumPhotos = 78,
            HighScore = 1234567,
        };

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
        public void ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var score = new Score(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Score(null!);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("SN", (ushort)1, 0x234, true)]
        [DataRow("sn", (ushort)1, 0x234, false)]
        [DataRow("SN", (ushort)0, 0x234, false)]
        [DataRow("SN", (ushort)1, 0x235, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = TestUtils.Create<Chapter>(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Score.CanInitialize(chapter));
            });
    }
}
