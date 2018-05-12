using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th07PracticeScoreTests
    {
        [TestMethod()]
        public void Th07PracticeScoreTestChapter()
        {
            try
            {
                var signature = "PSCR";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown1 = 1u;
                var trialCount = 987;
                var highScore = 123456;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var stage = Th07Converter.Stage.Extra;
                byte unknown2 = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1, trialCount, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var score = new Th07PracticeScoreWrapper(chapter);

                Assert.AreEqual(signature, score.Signature);
                Assert.AreEqual(size1, score.Size1);
                Assert.AreEqual(size2, score.Size2);
                CollectionAssert.AreEqual(data, score.Data.ToArray());
                Assert.AreEqual(data[0], score.FirstByteOfData);
                Assert.AreEqual(trialCount, score.TrialCount.Value);
                Assert.AreEqual(highScore, score.HighScore.Value);
                Assert.AreEqual(chara, score.Chara.Value);
                Assert.AreEqual(level, score.Level.Value);
                Assert.AreEqual(stage, score.Stage.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07PracticeScoreTestNullChapter()
        {
            try
            {
                var score = new Th07PracticeScoreWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07PracticeScoreTestInvalidSignature()
        {
            try
            {
                var signature = "pscr";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown1 = 1u;
                var trialCount = 987;
                var highScore = 123456;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var stage = Th07Converter.Stage.Extra;
                byte unknown2 = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1, trialCount, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var score = new Th07PracticeScoreWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07PracticeScoreTestInvalidSize1()
        {
            try
            {
                var signature = "PSCR";
                short size1 = 0x19;
                short size2 = 0x18;
                var unknown1 = 1u;
                var trialCount = 987;
                var highScore = 123456;
                var chara = Th07Converter.Chara.ReimuB;
                var level = Th07Converter.Level.Hard;
                var stage = Th07Converter.Stage.Extra;
                byte unknown2 = 2;
                var data = TestUtils.MakeByteArray(
                    unknown1, trialCount, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var score = new Th07PracticeScoreWrapper(chapter);

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
