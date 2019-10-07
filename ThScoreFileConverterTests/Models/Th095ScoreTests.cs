using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th095.Stubs;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095ScoreTests
    {
        internal static ScoreStub ValidStub { get; } = new ScoreStub()
        {
            Signature = "SC",
            Version = 1,
            Size = 0x60,
            Checksum = 0u,
            LevelScene = (Th095Converter.Level.Lv9, 6),
            HighScore = 1234567,
            BestshotScore = 23456,
            DateTime = 34567890,
            TrialCount = 9876,
            SlowRate1 = 1.23f,
            SlowRate2 = 2.34f
        };

        internal static byte[] MakeData(IScore score)
            => TestUtils.MakeByteArray(
                (int)score.LevelScene.Level * 10 + score.LevelScene.Scene - 1,
                score.HighScore,
                0u,
                score.BestshotScore,
                new byte[0x20],
                score.DateTime,
                0u,
                score.TrialCount,
                score.SlowRate1,
                score.SlowRate2,
                new byte[0x10]);

        internal static byte[] MakeByteArray(IScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Version,
                score.Size,
                score.Checksum,
                MakeData(score));

        internal static void Validate(IScore expected, in Th095ScoreWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.LevelScene, actual.LevelScene);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
            Assert.AreEqual(expected.BestshotScore, actual.BestshotScore);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.SlowRate1, actual.SlowRate1);
            Assert.AreEqual(expected.SlowRate2, actual.SlowRate2);
        }

        [TestMethod]
        public void Th095ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var score = new Th095ScoreWrapper(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th095ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th095Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th095ScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.LevelScene = (TestUtils.Cast<Th095Converter.Level>(level), stub.LevelScene.Scene);

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th095ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SC", (ushort)1, 0x60, true)]
        [DataRow("sc", (ushort)1, 0x60, false)]
        [DataRow("SC", (ushort)0, 0x60, false)]
        [DataRow("SC", (ushort)1, 0x61, false)]
        public void Th095ScoreCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th095ScoreWrapper.CanInitialize(chapter));
            });
    }
}
