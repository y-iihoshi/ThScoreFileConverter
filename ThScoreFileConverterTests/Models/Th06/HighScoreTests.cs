using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
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
            public Chara chara;
            public Level level;
            public StageProgress stageProgress;
            public byte[] name;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "HSCR",
            size1 = 0x1C,
            size2 = 0x1C,
            score = 1234567u,
            chara = Chara.ReimuB,
            level = Level.Hard,
            stageProgress = StageProgress.St3,
            name = TestUtils.CP932Encoding.GetBytes("Player1\0\0")
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.score,
                (byte)properties.chara,
                (byte)properties.level,
                (byte)properties.stageProgress,
                properties.name);

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
            Assert.AreEqual(properties.chara, highScore.Chara);
            Assert.AreEqual(properties.level, highScore.Level);
            Assert.AreEqual(properties.stageProgress, highScore.StageProgress);
            CollectionAssert.AreEqual(properties.name, highScore.Name.ToArray());
        }

        [TestMethod]
        public void HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidProperties));
            var highScore = new HighScore(chapter.Target as Chapter);

            Validate(highScore, ValidProperties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var highScore = new HighScore(null);

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
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new HighScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new HighScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new HighScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Level>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new HighScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<StageProgress>(stageProgress);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new HighScore(chapter.Target as Chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
