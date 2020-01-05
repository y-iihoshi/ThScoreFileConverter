using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class HighScoreTests
    {
        internal static HighScoreStub ValidStub { get; } = new HighScoreStub()
        {
            Signature = "HSCR",
            Size1 = 0x0168,
            Size2 = 0x0168,
            Score = 1234567u,
            SlowRate = 9.87f,
            Chara = Chara.MarisaAlice,
            Level = Level.Hard,
            StageProgress = StageProgress.Three,
            Name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            Date = TestUtils.CP932Encoding.GetBytes("01/23\0"),
            ContinueCount = 2,
            PlayerNum = 5,
            PlayTime = 987654u,
            PointItem = 1234,
            MissCount = 9,
            BombCount = 6,
            LastSpellCount = 12,
            PauseCount = 3,
            TimePoint = 65432,
            HumanRate = 7890,
            CardFlags = Enumerable.Range(1, 222).ToDictionary(id => id, id => (byte)((id == 3) || (id == 7) ? id : 0)),
        };

        internal static byte[] MakeByteArray(IHighScore highScore)
            => TestUtils.MakeByteArray(
                highScore.Signature.ToCharArray(),
                highScore.Size1,
                highScore.Size2,
                0u,
                highScore.Score,
                highScore.SlowRate,
                (byte)highScore.Chara,
                (byte)highScore.Level,
                (byte)highScore.StageProgress,
                highScore.Name,
                highScore.Date,
                highScore.ContinueCount,
                new byte[0x1C],
                highScore.PlayerNum,
                new byte[0x1F],
                highScore.PlayTime,
                highScore.PointItem,
                0u,
                highScore.MissCount,
                highScore.BombCount,
                highScore.LastSpellCount,
                highScore.PauseCount,
                highScore.TimePoint,
                highScore.HumanRate,
                highScore.CardFlags.Values.ToArray(),
                new byte[2]);

        internal static void Validate(IHighScore expected, IHighScore actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
            Assert.AreEqual(expected.Chara, actual.Chara);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.StageProgress, actual.StageProgress);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            CollectionAssert.That.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
            Assert.AreEqual(expected.PlayerNum, actual.PlayerNum);
            Assert.AreEqual(expected.PlayTime, actual.PlayTime);
            Assert.AreEqual(expected.PointItem, actual.PointItem);
            Assert.AreEqual(expected.MissCount, actual.MissCount);
            Assert.AreEqual(expected.BombCount, actual.BombCount);
            Assert.AreEqual(expected.LastSpellCount, actual.LastSpellCount);
            Assert.AreEqual(expected.PauseCount, actual.PauseCount);
            Assert.AreEqual(expected.TimePoint, actual.TimePoint);
            Assert.AreEqual(expected.HumanRate, actual.HumanRate);
            CollectionAssert.That.AreEqual(expected.CardFlags.Values, actual.CardFlags.Values);
        }

        [TestMethod]
        public void HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var highScore = new HighScore(chapter);

            Validate(stub, highScore);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HighScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new HighScore(null!);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void HighScoreTestScore() => TestUtils.Wrap(() =>
        {
            var score = 1234567u;
            var name = "--------\0";
            var date = "--/--\0";
            var cardFlags = new byte[] { };

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date);
            CollectionAssert.That.AreEqual(cardFlags, highScore.CardFlags.Values);
        });

        [TestMethod]
        public void HighScoreTestZeroScore() => TestUtils.Wrap(() =>
        {
            var score = 0u;
            var name = "--------\0";
            var date = "--/--\0";
            var cardFlags = new byte[] { };

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date);
            CollectionAssert.That.AreEqual(cardFlags, highScore.CardFlags.Values);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidStageProgress(int stageProgress) => TestUtils.Wrap(() =>
        {
            var stub = new HighScoreStub(ValidStub)
            {
                StageProgress = TestUtils.Cast<StageProgress>(stageProgress),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
