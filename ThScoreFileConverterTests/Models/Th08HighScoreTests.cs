using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08HighScoreTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public uint score;
            public float slowRate;
            public Th08Converter.Chara chara;
            public ThConverter.Level level;
            public Th08Converter.StageProgress stageProgress;
            public byte[] name;
            public byte[] date;
            public ushort continueCount;
            public byte playerNum;
            public uint playTime;
            public int pointItem;
            public int missCount;
            public int bombCount;
            public int lastSpellCount;
            public int pauseCount;
            public int timePoint;
            public int humanRate;
            public Dictionary<int, byte> cardFlags;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "HSCR",
            size1 = 0x0168,
            size2 = 0x0168,
            score = 1234567u,
            slowRate = 9.87f,
            chara = Th08Converter.Chara.MarisaAlice,
            level = ThConverter.Level.Hard,
            stageProgress = Th08Converter.StageProgress.St3,
            name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            date = TestUtils.CP932Encoding.GetBytes("01/23\0"),
            continueCount = 2,
            playerNum = 5,
            playTime = 987654u,
            pointItem = 1234,
            missCount = 9,
            bombCount = 6,
            lastSpellCount = 12,
            pauseCount = 3,
            timePoint = 65432,
            humanRate = 7890,
            cardFlags = TestUtils.MakeRandomArray<byte>(222)
                .Select((value, index) => new { index, value })
                .ToDictionary(pair => pair.index, pair => pair.value)
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
                properties.continueCount,
                new byte[0x1C],
                properties.playerNum,
                new byte[0x1F],
                properties.playTime,
                properties.pointItem,
                0u,
                properties.missCount,
                properties.bombCount,
                properties.lastSpellCount,
                properties.pauseCount,
                properties.timePoint,
                properties.humanRate,
                properties.cardFlags.Values.ToArray(),
                new byte[2]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th08HighScoreWrapper highScore, in Properties properties)
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
            Assert.AreEqual(properties.playerNum, highScore.PlayerNum);
            Assert.AreEqual(properties.playTime, highScore.PlayTime);
            Assert.AreEqual(properties.pointItem, highScore.PointItem);
            Assert.AreEqual(properties.missCount, highScore.MissCount);
            Assert.AreEqual(properties.bombCount, highScore.BombCount);
            Assert.AreEqual(properties.lastSpellCount, highScore.LastSpellCount);
            Assert.AreEqual(properties.pauseCount, highScore.PauseCount);
            Assert.AreEqual(properties.timePoint, highScore.TimePoint);
            Assert.AreEqual(properties.humanRate, highScore.HumanRate);
            CollectionAssert.AreEqual(properties.cardFlags.Values.ToArray(), highScore.CardFlags.Values.ToArray());
        }

        [TestMethod]
        public void Th08HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new Th08HighScoreWrapper(chapter);

            Validate(highScore, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var highScore = new Th08HighScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08HighScoreTestScore() => TestUtils.Wrap(() =>
        {
            var score = 1234567u;
            var name = "--------\0";
            var date = "--/--\0";
            var cardFlags = new byte[] { };

            var highScore = new Th08HighScoreWrapper(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name.ToArray());
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date.ToArray());
            CollectionAssert.AreEqual(cardFlags, highScore.CardFlags.Values.ToArray());
        });

        [TestMethod]
        public void Th08HighScoreTestZeroScore() => TestUtils.Wrap(() =>
        {
            var score = 0u;
            var name = "--------\0";
            var date = "--/--\0";
            var cardFlags = new byte[] { };

            var highScore = new Th08HighScoreWrapper(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name.ToArray());
            CollectionAssert.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date.ToArray());
            CollectionAssert.AreEqual(cardFlags, highScore.CardFlags.Values.ToArray());
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new Th08HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new Th08HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th08Converter.Chara));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th08Converter.Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new Th08HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<ThConverter.Level>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new Th08HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th08Converter.StageProgress));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08HighScoreTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.stageProgress = TestUtils.Cast<Th08Converter.StageProgress>(stageProgress);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var highScore = new Th08HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
