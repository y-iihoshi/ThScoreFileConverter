using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class HighScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public uint score;
            public float slowRate;
            public Chara chara;
            public Level level;
            public StageProgress stageProgress;
            public byte[] name;
            public byte[] date;
            public ushort continueCount;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "HSCR",
            size1 = 0x28,
            size2 = 0x28,
            score = 1234567u,
            slowRate = 9.87f,
            chara = Chara.ReimuB,
            level = Level.Hard,
            stageProgress = StageProgress.Three,
            name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            date = TestUtils.CP932Encoding.GetBytes("01/23\0"),
            continueCount = 2
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.score,
                properties.slowRate,
                (byte)properties.chara,
                (byte)properties.level,
                (byte)properties.stageProgress,
                properties.name,
                properties.date,
                properties.continueCount);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in HighScore highScore, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, highScore.Signature);
            Assert.AreEqual(properties.size1, highScore.Size1);
            Assert.AreEqual(properties.size2, highScore.Size2);
            Assert.AreEqual(data[0], highScore.FirstByteOfData);
            Assert.AreEqual(properties.score, highScore.Score);
            Assert.AreEqual(properties.slowRate, highScore.SlowRate);
            Assert.AreEqual(properties.chara, highScore.Chara);
            Assert.AreEqual(properties.level, highScore.Level);
            Assert.AreEqual(properties.stageProgress, highScore.StageProgress);
            CollectionAssert.AreEqual(properties.name, highScore.Name.ToArray());
            CollectionAssert.AreEqual(properties.date, highScore.Date.ToArray());
            Assert.AreEqual(properties.continueCount, highScore.ContinueCount);
        }

        [TestMethod]
        public void HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidProperties));
            var highScore = new HighScore(chapter.Target);

            Validate(highScore, ValidProperties);
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
            var name = "--------\0";
            var date = "--/--\0";

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name.ToArray());
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date.ToArray());
        });

        [TestMethod]
        public void HighScoreTestZeroScore() => TestUtils.Wrap(() =>
        {
            var score = 0u;
            var name = "--------\0";
            var date = "--/--\0";

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name.ToArray());
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date.ToArray());
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Level>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<StageProgress>(stageProgress);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new HighScore(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
