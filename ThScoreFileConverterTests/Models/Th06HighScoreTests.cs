using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th06HighScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public uint score;
            public Th06Converter.Chara chara;
            public ThConverter.Level level;
            public Th06Converter.StageProgress stageProgress;
            public byte[] name;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "HSCR",
            size1 = 0x1C,
            size2 = 0x1C,
            score = 1234567u,
            chara = Th06Converter.Chara.ReimuB,
            level = ThConverter.Level.Hard,
            stageProgress = Th06Converter.StageProgress.St3,
            name = Encoding.Default.GetBytes("Player1\0\0")
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

        internal static void Validate(in Th06HighScoreWrapper highScore, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, highScore.Signature);
            Assert.AreEqual(properties.size1, highScore.Size1);
            Assert.AreEqual(properties.size2, highScore.Size2);
            CollectionAssert.AreEqual(data, highScore.Data.ToArray());
            Assert.AreEqual(data[0], highScore.FirstByteOfData);
            Assert.AreEqual(properties.score, highScore.Score);
            Assert.AreEqual(properties.chara, highScore.Chara);
            Assert.AreEqual(properties.level, highScore.Level);
            Assert.AreEqual(properties.stageProgress, highScore.StageProgress);
            CollectionAssert.AreEqual(properties.name, highScore.Name.ToArray());
        }

        [TestMethod()]
        public void Th06HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(ValidProperties));
            var highScore = new Th06HighScoreWrapper(chapter);

            Validate(highScore, ValidProperties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var highScore = new Th06HighScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void Th06HighScoreTestScore() => TestUtils.Wrap(() =>
        {
            var score = 1234567u;
            var name = "Nanashi\0\0";

            var highScore = new Th06HighScoreWrapper(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
        });

        [TestMethod()]
        public void Th06HighScoreTestZeroScore() => TestUtils.Wrap(() =>
        {
            var score = 0u;
            var name = "Nanashi\0\0";

            var highScore = new Th06HighScoreWrapper(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size1;

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th06Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th06Converter.Chara>(chara);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<ThConverter.Level>(level);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th06Converter.StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06HighScoreTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<Th06Converter.StageProgress>(stageProgress);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
