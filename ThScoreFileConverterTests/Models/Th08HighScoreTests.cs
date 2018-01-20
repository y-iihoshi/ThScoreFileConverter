using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th08HighScoreTests
    {
        [TestMethod()]
        public void Th08HighScoreTestChapter()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x0168;
                short size2 = 0x0168;
                var unknown1 = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th08Converter.Chara.MarisaAlice;
                var level = ThConverter.Level.Hard;
                var progress = Th08Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var unknown2 = new byte[0x1C];
                byte playerNum = 5;
                var unknown3 = new byte[0x1F];
                var playTime = 987654u;
                var pointItem = 1234;
                var unknown4 = 0u;
                var missCount = 9;
                var bombCount = 6;
                var lastSpellCount = 12;
                var pauseCount = 3;
                var timePoint = 65432;
                var humanRate = 7890;
                var cardFlags = new byte[222];
                var random = new Random();
                random.NextBytes(cardFlags);
                var unknown5 = new byte[2];
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount,
                    unknown2,
                    playerNum,
                    unknown3,
                    playTime,
                    pointItem,
                    unknown4,
                    missCount,
                    bombCount,
                    lastSpellCount,
                    pauseCount,
                    timePoint,
                    humanRate,
                    cardFlags,
                    unknown5);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th08HighScoreWrapper(chapter);

                Assert.AreEqual(signature, highScore.Signature);
                Assert.AreEqual(size1, highScore.Size1);
                Assert.AreEqual(size2, highScore.Size2);
                CollectionAssert.AreEqual(data, highScore.Data.ToArray());
                Assert.AreEqual(data[0], highScore.FirstByteOfData);
                Assert.AreEqual(score, highScore.Score);
                Assert.AreEqual(slowRate, highScore.SlowRate);
                Assert.AreEqual(chara, highScore.Chara);
                Assert.AreEqual(level, highScore.Level);
                Assert.AreEqual(progress, highScore.StageProgress);
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
                Assert.AreEqual(continueCount, highScore.ContinueCount);
                Assert.AreEqual(playerNum, highScore.PlayerNum);
                Assert.AreEqual(playTime, highScore.PlayTime);
                Assert.AreEqual(pointItem, highScore.PointItem);
                Assert.AreEqual(missCount, highScore.MissCount);
                Assert.AreEqual(bombCount, highScore.BombCount);
                Assert.AreEqual(lastSpellCount, highScore.LastSpellCount);
                Assert.AreEqual(pauseCount, highScore.PauseCount);
                Assert.AreEqual(timePoint, highScore.TimePoint);
                Assert.AreEqual(humanRate, highScore.HumanRate);
                CollectionAssert.AreEqual(cardFlags, highScore.CardFlags.Values.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08HighScoreTestNullChapter()
        {
            try
            {
                var highScore = new Th08HighScoreWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th08HighScoreTestScore()
        {
            try
            {
                var score = 1234567u;
                var name = "--------\0";
                var date = "--/--\0";
                var cardFlags = new byte[] { };

                var highScore = new Th08HighScoreWrapper(score);

                Assert.AreEqual(score, highScore.Score);
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
                CollectionAssert.AreEqual(cardFlags, highScore.CardFlags.Values.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th08HighScoreTestZeroScore()
        {
            try
            {
                var score = 0u;
                var name = "--------\0";
                var date = "--/--\0";
                var cardFlags = new byte[] { };

                var highScore = new Th08HighScoreWrapper(score);

                Assert.AreEqual(score, highScore.Score);
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
                CollectionAssert.AreEqual(cardFlags, highScore.CardFlags.Values.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HighScoreTestInvalidSignature()
        {
            try
            {
                var signature = "hscr";
                short size1 = 0x0168;
                short size2 = 0x0168;
                var unknown1 = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th08Converter.Chara.MarisaAlice;
                var level = ThConverter.Level.Hard;
                var progress = Th08Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var unknown2 = new byte[0x1C];
                byte playerNum = 5;
                var unknown3 = new byte[0x1F];
                var playTime = 987654u;
                var pointItem = 1234;
                var unknown4 = 0u;
                var missCount = 9;
                var bombCount = 6;
                var lastSpellCount = 12;
                var pauseCount = 3;
                var timePoint = 65432;
                var humanRate = 7890;
                var cardFlags = new byte[222];
                var random = new Random();
                random.NextBytes(cardFlags);
                var unknown5 = new byte[2];
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount,
                    unknown2,
                    playerNum,
                    unknown3,
                    playTime,
                    pointItem,
                    unknown4,
                    missCount,
                    bombCount,
                    lastSpellCount,
                    pauseCount,
                    timePoint,
                    humanRate,
                    cardFlags,
                    unknown5);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th08HighScoreWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HighScoreTestInvalidSize1()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x0169;
                short size2 = 0x0168;
                var unknown1 = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th08Converter.Chara.MarisaAlice;
                var level = ThConverter.Level.Hard;
                var progress = Th08Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var unknown2 = new byte[0x1C];
                byte playerNum = 5;
                var unknown3 = new byte[0x1F];
                var playTime = 987654u;
                var pointItem = 1234;
                var unknown4 = 0u;
                var missCount = 9;
                var bombCount = 6;
                var lastSpellCount = 12;
                var pauseCount = 3;
                var timePoint = 65432;
                var humanRate = 7890;
                var cardFlags = new byte[222];
                var random = new Random();
                random.NextBytes(cardFlags);
                var unknown5 = new byte[2];
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount,
                    unknown2,
                    playerNum,
                    unknown3,
                    playTime,
                    pointItem,
                    unknown4,
                    missCount,
                    bombCount,
                    lastSpellCount,
                    pauseCount,
                    timePoint,
                    humanRate,
                    cardFlags,
                    unknown5);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th08HighScoreWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08HighScoreTestInvalidChara()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x0168;
                short size2 = 0x0168;
                var unknown1 = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = (Th08Converter.Chara)(-1);
                var level = ThConverter.Level.Hard;
                var progress = Th08Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var unknown2 = new byte[0x1C];
                byte playerNum = 5;
                var unknown3 = new byte[0x1F];
                var playTime = 987654u;
                var pointItem = 1234;
                var unknown4 = 0u;
                var missCount = 9;
                var bombCount = 6;
                var lastSpellCount = 12;
                var pauseCount = 3;
                var timePoint = 65432;
                var humanRate = 7890;
                var cardFlags = new byte[222];
                var random = new Random();
                random.NextBytes(cardFlags);
                var unknown5 = new byte[2];
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount,
                    unknown2,
                    playerNum,
                    unknown3,
                    playTime,
                    pointItem,
                    unknown4,
                    missCount,
                    bombCount,
                    lastSpellCount,
                    pauseCount,
                    timePoint,
                    humanRate,
                    cardFlags,
                    unknown5);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th08HighScoreWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08HighScoreTestInvalidLevel()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x0168;
                short size2 = 0x0168;
                var unknown1 = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th08Converter.Chara.MarisaAlice;
                var level = (ThConverter.Level)(-1);
                var progress = Th08Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var unknown2 = new byte[0x1C];
                byte playerNum = 5;
                var unknown3 = new byte[0x1F];
                var playTime = 987654u;
                var pointItem = 1234;
                var unknown4 = 0u;
                var missCount = 9;
                var bombCount = 6;
                var lastSpellCount = 12;
                var pauseCount = 3;
                var timePoint = 65432;
                var humanRate = 7890;
                var cardFlags = new byte[222];
                var random = new Random();
                random.NextBytes(cardFlags);
                var unknown5 = new byte[2];
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount,
                    unknown2,
                    playerNum,
                    unknown3,
                    playTime,
                    pointItem,
                    unknown4,
                    missCount,
                    bombCount,
                    lastSpellCount,
                    pauseCount,
                    timePoint,
                    humanRate,
                    cardFlags,
                    unknown5);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th08HighScoreWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08HighScoreTestInvalidStageProgress()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x0168;
                short size2 = 0x0168;
                var unknown1 = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th08Converter.Chara.MarisaAlice;
                var level = ThConverter.Level.Hard;
                var progress = (Th08Converter.StageProgress)(-1);
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var unknown2 = new byte[0x1C];
                byte playerNum = 5;
                var unknown3 = new byte[0x1F];
                var playTime = 987654u;
                var pointItem = 1234;
                var unknown4 = 0u;
                var missCount = 9;
                var bombCount = 6;
                var lastSpellCount = 12;
                var pauseCount = 3;
                var timePoint = 65432;
                var humanRate = 7890;
                var cardFlags = new byte[222];
                var random = new Random();
                random.NextBytes(cardFlags);
                var unknown5 = new byte[2];
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount,
                    unknown2,
                    playerNum,
                    unknown3,
                    playTime,
                    pointItem,
                    unknown4,
                    missCount,
                    bombCount,
                    lastSpellCount,
                    pauseCount,
                    timePoint,
                    humanRate,
                    cardFlags,
                    unknown5);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th08HighScoreWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
