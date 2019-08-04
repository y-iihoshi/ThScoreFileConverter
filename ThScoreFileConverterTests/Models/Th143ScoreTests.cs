using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143ScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public int number;
            public Dictionary<Th143Converter.ItemWithTotal, int> clearCounts;
            public Dictionary<Th143Converter.ItemWithTotal, int> challengeCounts;
            public int highScore;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "SN",
            version = 1,
            checksum = 0u,
            size = 0x314,
            number = 123,
            clearCounts = Utils.GetEnumerator<Th143Converter.ItemWithTotal>()
                .ToDictionary(item => item, item => (int)item * 10),
            challengeCounts = Utils.GetEnumerator<Th143Converter.ItemWithTotal>()
                .ToDictionary(item => item, item => (int)item * 100),
            highScore = 456789
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.number,
                properties.clearCounts.Values.ToArray(),
                properties.challengeCounts.Values.ToArray(),
                properties.highScore,
                new byte[0x2A8]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th143ScoreWrapper score, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, score.Signature);
            Assert.AreEqual(properties.version, score.Version);
            Assert.AreEqual(properties.checksum, score.Checksum);
            Assert.AreEqual(properties.size, score.Size);
            CollectionAssert.AreEqual(data, score.Data.ToArray());
            Assert.AreEqual(properties.number, score.Number);
            CollectionAssert.AreEqual(properties.clearCounts.Values, score.ClearCounts.Values.ToArray());
            CollectionAssert.AreEqual(properties.challengeCounts.Values, score.ChallengeCounts.Values.ToArray());
            Assert.AreEqual(properties.highScore, score.HighScore);
        }

        [TestMethod]
        public void Th143ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th143ScoreWrapper(chapter);

            Validate(score, properties);
            Assert.IsFalse(score.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th143ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th143ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th143ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var score = new Th143ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SN", (ushort)1, 0x314, true)]
        [DataRow("sn", (ushort)1, 0x314, false)]
        [DataRow("SN", (ushort)0, 0x314, false)]
        [DataRow("SN", (ushort)1, 0x315, false)]
        public void Th143ScoreCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th143ScoreWrapper.CanInitialize(chapter));
            });
    }
}
