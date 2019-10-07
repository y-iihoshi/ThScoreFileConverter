using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125ScoreTests
    {
        internal static ScoreStub ValidStub => new ScoreStub()
        {
            Signature = "SC",
            Version = 0,
            Size = 0x48,
            Checksum = 0u,
            LevelScene = (Th125Converter.Level.Lv9, 7),
            HighScore = 1234567,
            Chara = Th125Converter.Chara.Hatate,
            TrialCount = 9876,
            FirstSuccess = 5432,
            DateTime = 34567890,
            BestshotScore = 23456
        };

        internal static byte[] MakeData(IScore score)
            => TestUtils.MakeByteArray(
                (int)score.LevelScene.Level * 10 + score.LevelScene.Scene - 1,
                score.HighScore,
                new byte[4],
                (int)score.Chara,
                new byte[4],
                score.TrialCount,
                score.FirstSuccess,
                0u,
                score.DateTime,
                new uint[3],
                score.BestshotScore,
                new byte[8]);

        internal static byte[] MakeByteArray(IScore score)
            => TestUtils.MakeByteArray(
                score.Signature.ToCharArray(),
                score.Version,
                score.Size,
                score.Checksum,
                MakeData(score));

        internal static void Validate(IScore expected, in Th125ScoreWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.LevelScene, actual.LevelScene);
            Assert.AreEqual(expected.HighScore, actual.HighScore);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.FirstSuccess, actual.FirstSuccess);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.BestshotScore, actual.BestshotScore);
        }

        [TestMethod]
        public void Th125ScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var score = new Th125ScoreWrapper(chapter);

            Validate(stub, score);
            Assert.IsFalse(score.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th125ScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125ScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125ScoreTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125ScoreTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th125Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th125ScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub);
            stub.LevelScene = (TestUtils.Cast<Th125Converter.Level>(level), stub.LevelScene.Scene);

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th125Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th125ScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new ScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Th125Converter.Chara>(chara),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th125ScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("SC", (ushort)0, 0x48, true)]
        [DataRow("sc", (ushort)0, 0x48, false)]
        [DataRow("SC", (ushort)1, 0x48, false)]
        [DataRow("SC", (ushort)0, 0x49, false)]
        public void Th125ScoreCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th125ScoreWrapper.CanInitialize(chapter));
            });
    }
}
