using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class HighScoreTests
    {
        internal static HighScoreStub ValidStub => new HighScoreStub()
        {
            Signature = "HSCR",
            Size1 = 0x1C,
            Size2 = 0x1C,
            Score = 1234567u,
            Chara = Chara.ReimuB,
            Level = Level.Hard,
            StageProgress = StageProgress.Three,
            Name = TestUtils.CP932Encoding.GetBytes("Player1\0\0")
        };

        internal static byte[] MakeByteArray(IHighScore highScore)
            => TestUtils.MakeByteArray(
                highScore.Signature.ToCharArray(),
                highScore.Size1,
                highScore.Size2,
                0u,
                highScore.Score,
                (byte)highScore.Chara,
                (byte)highScore.Level,
                (byte)highScore.StageProgress,
                highScore.Name.ToArray());

        internal static void Validate(IHighScore expected, IHighScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.StageProgress, actual.StageProgress);
            CollectionAssert.AreEqual(expected.Name?.ToArray(), actual.Name?.ToArray());
        }

        [TestMethod]
        public void HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidStub));
            var highScore = new HighScore(chapter.Target);

            Validate(ValidStub, highScore);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new HighScore(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void HighScoreTestScore() => TestUtils.Wrap(() =>
        {
            var score = 1234567u;
            var name = "Nanashi\0\0";

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name.ToArray());
        });

        [TestMethod]
        public void HighScoreTestZeroScore() => TestUtils.Wrap(() =>
        {
            var score = 0u;
            var name = "Nanashi\0\0";

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name.ToArray());
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.Chara = TestUtils.Cast<Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.Level = TestUtils.Cast<Level>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;
            stub.StageProgress = TestUtils.Cast<StageProgress>(stageProgress);

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
