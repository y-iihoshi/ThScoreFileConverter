using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th09HighScoreTests
    {
        [TestMethod()]
        public void Th09HighScoreTestChapter()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x2C;
                short size2 = 0x2C;
                var unknown1 = 0u;
                var score = 1234567u;
                var unknown2 = 0u;
                var chara = Th09Converter.Chara.Marisa;
                var level = ThConverter.Level.Hard;
                short rank = 987;
                var name = "Player1\0\0";
                var date = "06/01/23\0";
                byte unknown3 = 0;
                byte continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    unknown2,
                    (byte)chara,
                    (byte)level,
                    rank,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    unknown3,
                    continueCount);

                var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th09HighScoreWrapper(chapter);

                Assert.AreEqual(signature, highScore.Signature);
                Assert.AreEqual(size1, highScore.Size1);
                Assert.AreEqual(size2, highScore.Size2);
                CollectionAssert.AreEqual(data, highScore.Data.ToArray());
                Assert.AreEqual(data[0], highScore.FirstByteOfData);
                Assert.AreEqual(score, highScore.Score);
                Assert.AreEqual(chara, highScore.Chara);
                Assert.AreEqual(level, highScore.Level);
                Assert.AreEqual(rank, highScore.Rank);
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
        public void Th09HighScoreTestNullChapter()
        {
            try
            {
                var highScore = new Th09HighScoreWrapper(null);

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
        public void Th09HighScoreTestInvalidSignature()
        {
            try
            {
                var signature = "hscr";
                short size1 = 0x2C;
                short size2 = 0x2C;
                var unknown1 = 0u;
                var score = 1234567u;
                var unknown2 = 0u;
                var chara = Th09Converter.Chara.Marisa;
                var level = ThConverter.Level.Hard;
                short rank = 987;
                var name = "Player1\0\0";
                var date = "06/01/23\0";
                byte unknown3 = 0;
                byte continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    unknown2,
                    (byte)chara,
                    (byte)level,
                    rank,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    unknown3,
                    continueCount);

                var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th09HighScoreWrapper(chapter);

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
        public void Th09HighScoreTestInvalidSize()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x2D;
                short size2 = 0x2C;
                var unknown1 = 0u;
                var score = 1234567u;
                var unknown2 = 0u;
                var chara = Th09Converter.Chara.Marisa;
                var level = ThConverter.Level.Hard;
                short rank = 987;
                var name = "Player1\0\0";
                var date = "06/01/23\0";
                byte unknown3 = 0;
                byte continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    unknown2,
                    (byte)chara,
                    (byte)level,
                    rank,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    unknown3,
                    continueCount);

                var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th09HighScoreWrapper(chapter);

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
        public void Th09HighScoreTestInvalidChara()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x2C;
                short size2 = 0x2C;
                var unknown1 = 0u;
                var score = 1234567u;
                var unknown2 = 0u;
                var chara = (Th09Converter.Chara)(-1);
                var level = ThConverter.Level.Hard;
                short rank = 987;
                var name = "Player1\0\0";
                var date = "06/01/23\0";
                byte unknown3 = 0;
                byte continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    unknown2,
                    (byte)chara,
                    (byte)level,
                    rank,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    unknown3,
                    continueCount);

                var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th09HighScoreWrapper(chapter);

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
        public void Th09HighScoreTestInvalidLevel()
        {
            try
            {
                var signature = "HSCR";
                short size1 = 0x2C;
                short size2 = 0x2C;
                var unknown1 = 0u;
                var score = 1234567u;
                var unknown2 = 0u;
                var chara = Th09Converter.Chara.Marisa;
                var level = (ThConverter.Level)(-1);
                short rank = 987;
                var name = "Player1\0\0";
                var date = "06/01/23\0";
                byte unknown3 = 0;
                byte continueCount = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    score,
                    unknown2,
                    (byte)chara,
                    (byte)level,
                    rank,
                    name.ToCharArray(),
                    date.ToCharArray(),
                    unknown3,
                    continueCount);

                var chapter = Th06ChapterWrapper<Th09Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var highScore = new Th09HighScoreWrapper(chapter);

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
