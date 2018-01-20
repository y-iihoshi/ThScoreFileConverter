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
    public class Th07HighScoreTests
    {
        [TestMethod()]
        public void Th07HighScoreTestChapter()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x28;
                short size2 = 0x28;
                var unknown = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var progress = Th07Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th07HighScoreWrapper(chapter);

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
        public void Th07HighScoreTestNullChapter()
        {
            try
            {
                var highScore = new Th07HighScoreWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th07HighScoreTestScore()
        {
            try
            {
                var score = 1234567u;
                var name = "--------\0";
                var date = "--/--\0";

                var highScore = new Th07HighScoreWrapper(score);

                Assert.AreEqual(score, highScore.Score);
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th07HighScoreTestZeroScore()
        {
            try
            {
                var score = 0u;
                var name = "--------\0";
                var date = "--/--\0";

                var highScore = new Th07HighScoreWrapper(score);

                Assert.AreEqual(score, highScore.Score);
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
                CollectionAssert.AreEqual(Encoding.Default.GetBytes(date), highScore.Date.ToArray());
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
        public void Th07HighScoreTestInvalidSignature()
        {
            try
            {
                var signature = "hscr";
                short size1 = 0x28;
                short size2 = 0x28;
                var unknown = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var progress = Th07Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23";
                ushort continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th07HighScoreWrapper(chapter);

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
        public void Th07HighScoreTestInvalidSize1()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x29;
                short size2 = 0x28;
                var unknown = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var progress = Th07Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23";
                ushort continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th07HighScoreWrapper(chapter);

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
        public void Th07HighScoreTestInvalidChara()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x28;
                short size2 = 0x28;
                var unknown = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = (Th07Converter.Chara)(-1);
                var level = Th07Converter.Level.Hard;
                var progress = Th07Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th07HighScoreWrapper(chapter);

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
        public void Th07HighScoreTestInvalidLevel()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x28;
                short size2 = 0x28;
                var unknown = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th07Converter.Chara.ReimuB;
                var level = (Th07Converter.Level)(-1);
                var progress = Th07Converter.StageProgress.St3;
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th07HighScoreWrapper(chapter);

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
        public void Th07HighScoreTestInvalidStageProgress()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x28;
                short size2 = 0x28;
                var unknown = 0u;
                var score = 1234567u;
                var slowRate = 9.87f;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var progress = (Th07Converter.StageProgress)(-1);
                var name = "Player1\0\0";
                var date = "01/23\0";
                ushort continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    score,
                    slowRate,
                    (byte)chara,
                    (byte)level,
                    (byte)progress,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    continueCount);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th07HighScoreWrapper(chapter);

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
