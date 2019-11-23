using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143ScoreTests
    {
        internal static ScoreStub ValidStub { get; } = new ScoreStub()
        {
            Signature = "SN",
            Version = 1,
            Checksum = 0u,
            Size = 0x314,
            Number = 123,
            ClearCounts = Utils.GetEnumerator<ItemWithTotal>()
                .ToDictionary(item => item, item => (int)item * 10),
            ChallengeCounts = Utils.GetEnumerator<ItemWithTotal>()
                .ToDictionary(item => item, item => (int)item * 100),
            HighScore = 456789
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

        internal static void Validate(IScore expected, in Th143ScoreWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.Number, actual.Number);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
            CollectionAssert.That.AreEqual(expected.ChallengeCounts.Values, actual.ChallengeCounts.Values);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
        }

        [TestMethod]
        public void Th143ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var score = new Th143ScoreWrapper(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th143ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ScoreWrapper(chapter);

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
