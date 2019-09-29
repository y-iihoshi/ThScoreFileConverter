using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165ScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public int number;
            public int clearCount;
            public int challengeCount;
            public int numPhotos;
            public int highScore;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "SN",
            version = 1,
            checksum = 0u,
            size = 0x234,
            number = 12,
            clearCount = 34,
            challengeCount = 56,
            numPhotos = 78,
            highScore = 1234567,
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.number,
                properties.clearCount,
                0,
                properties.challengeCount,
                properties.numPhotos,
                properties.highScore,
                new byte[0x210]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th165ScoreWrapper score, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, score.Signature);
            Assert.AreEqual(properties.version, score.Version);
            Assert.AreEqual(properties.checksum, score.Checksum);
            Assert.AreEqual(properties.size, score.Size);
            CollectionAssert.That.AreEqual(data, score.Data);
            Assert.AreEqual(properties.number, score.Number);
            Assert.AreEqual(properties.clearCount, score.ClearCount);
            Assert.AreEqual(properties.challengeCount, score.ChallengeCount);
            Assert.AreEqual(properties.numPhotos, score.NumPhotos);
            Assert.AreEqual(properties.highScore, score.HighScore);
        }

        [TestMethod]
        public void Th165ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th165ScoreWrapper(chapter);

            Validate(score, properties);
            Assert.IsFalse(score.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th165ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th165ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th165ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th165ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th165ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SN", (ushort)1, 0x234, true)]
        [DataRow("sn", (ushort)1, 0x234, false)]
        [DataRow("SN", (ushort)0, 0x234, false)]
        [DataRow("SN", (ushort)1, 0x235, false)]
        public void Th165ScoreCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th165ScoreWrapper.CanInitialize(chapter));
            });
    }
}
