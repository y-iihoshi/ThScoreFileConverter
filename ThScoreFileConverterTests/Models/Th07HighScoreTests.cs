using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th07HighScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public uint score;
            public float slowRate;
            public Th07Converter.Chara chara;
            public Th07Converter.Level level;
            public Th07Converter.StageProgress stageProgress;
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
            chara = Th07Converter.Chara.ReimuB,
            level = Th07Converter.Level.Hard,
            stageProgress = Th07Converter.StageProgress.St3,
            name = Encoding.Default.GetBytes("Player1\0\0"),
            date = Encoding.Default.GetBytes("01/23\0"),
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

        internal static void Validate(in Th07HighScoreWrapper highScore, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, highScore.Signature);
            Assert.AreEqual(properties.size1, highScore.Size1);
            Assert.AreEqual(properties.size2, highScore.Size2);
            CollectionAssert.AreEqual(data, highScore.Data.ToArray());
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
        public void Th07HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(ValidProperties));
            var highScore = new Th07HighScoreWrapper(chapter);

            Validate(highScore, ValidProperties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var highScore = new Th07HighScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th07HighScoreTestScore() => TestUtils.Wrap(() =>
        {
            var score = 1234567u;
            var name = "--------\0";
            var date = "--/--\0";

            var highScore = new Th07HighScoreWrapper(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
        });

        [TestMethod]
        public void Th07HighScoreTestZeroScore() => TestUtils.Wrap(() =>
        {
            var score = 0u;
            var name = "--------\0";
            var date = "--/--\0";

            var highScore = new Th07HighScoreWrapper(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var highScore = new Th07HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var highScore = new Th07HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th07Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th07HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th07Converter.Chara>(chara);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var highScore = new Th07HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th07Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th07HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Th07Converter.Level>(level);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var highScore = new Th07HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th07Converter.StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th07HighScoreTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<Th07Converter.StageProgress>(stageProgress);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var highScore = new Th07HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
