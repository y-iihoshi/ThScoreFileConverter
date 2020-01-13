using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class ScoreTests
    {
        internal static ScoreStub ValidStub { get; } = new ScoreStub()
        {
            Signature = "SN",
            Version = 1,
            Checksum = 0u,
            Size = 0x314,
            Number = 69,
            ClearCounts = Utils.GetEnumerable<ItemWithTotal>().ToDictionary(item => item, item => (int)item * 10),
            ChallengeCounts = Utils.GetEnumerable<ItemWithTotal>().ToDictionary(item => item, item => (int)item * 100),
            HighScore = 456789,
        };

        internal static byte[] MakeData(IScore score)
            => TestUtils.MakeByteArray(
                score.Number,
                score.ClearCounts.Values.ToArray(),
                score.ChallengeCounts.Values.ToArray(),
                score.HighScore,
                new byte[0x2A8]);

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
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ChallengeCounts.Values, actual.ChallengeCounts.Values);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
        }

        [TestMethod]
        public void ScoreTestChapter()
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var score = new Score(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTestNullChapter()
        {
            _ = new Score(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidSignature()
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidVersion()
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ScoreTestInvalidSize()
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Score(chapter);

            Assert.Fail(TestUtils.Unreachable);
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
