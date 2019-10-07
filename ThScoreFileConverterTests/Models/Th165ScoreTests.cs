using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th165.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165ScoreTests
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

        internal static void Validate(IScore expected, in Th165ScoreWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.Number, actual.Number);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
            Assert.AreEqual(expected.ChallengeCount, actual.ChallengeCount);
            Assert.AreEqual(expected.NumPhotos, actual.NumPhotos);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
        }

        [TestMethod]
        public void Th165ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var score = new Th165ScoreWrapper(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th165ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th165ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th165ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th165ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th165ScoreWrapper(chapter);

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
