using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th07.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Models.Th07.Chara,
    ThScoreFileConverter.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class HighScoreTests
    {
        internal static HighScoreStub ValidStub { get; } = new HighScoreStub()
        {
            Signature = "HSCR",
            Size1 = 0x28,
            Size2 = 0x28,
            Score = 1234567u,
            SlowRate = 9.87f,
            Chara = Chara.ReimuB,
            Level = Level.Hard,
            StageProgress = StageProgress.Three,
            Name = TestUtils.CP932Encoding.GetBytes("Player1\0\0"),
            Date = TestUtils.CP932Encoding.GetBytes("01/23\0"),
            ContinueCount = 2
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
                highScore.ContinueCount);

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
        }

        [TestMethod]
        public void HighScoreTestChapter()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidStub));
            var highScore = new HighScore(chapter);

            Validate(ValidStub, highScore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HighScoreTestNullChapter()
        {
            _ = new HighScore(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void HighScoreTestScore()
        {
            var score = 1234567u;
            var name = "--------\0";
            var date = "--/--\0";

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date);
        }

        [TestMethod]
        public void HighScoreTestZeroScore()
        {
            var score = 0u;
            var name = "--------\0";
            var date = "--/--\0";

            var highScore = new HighScore(score);

            Assert.AreEqual(score, highScore.Score);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name);
            CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSignature()
        {
            var stub = new HighScoreStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void HighScoreTestInvalidSize1()
        {
            var stub = new HighScoreStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidChara(int chara)
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidLevel(int level)
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void HighScoreTestInvalidStageProgress(int stageProgress)
        {
            var stub = new HighScoreStub(ValidStub)
            {
                StageProgress = TestUtils.Cast<StageProgress>(stageProgress),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new HighScore(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
