using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th06PracticeScoreTests
    {
        [TestMethod()]
        public void Th06PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x14;
            short size2 = 0x14;
            var unknown1 = 1u;
            var highScore = 123456;
            var chara = Th06Converter.Chara.ReimuB;
            var level = ThConverter.Level.Hard;
            var stage = ThConverter.Stage.Extra;
            byte unknown2 = 2;
            var data = TestUtils.MakeByteArray(
                unknown1, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.AreEqual(signature, score.Signature);
            Assert.AreEqual(size1, score.Size1);
            Assert.AreEqual(size2, score.Size2);
            CollectionAssert.AreEqual(data, score.Data.ToArray());
            Assert.AreEqual(data[0], score.FirstByteOfData);
            Assert.AreEqual(highScore, score.HighScore.Value);
            Assert.AreEqual(chara, score.Chara.Value);
            Assert.AreEqual(level, score.Level.Value);
            Assert.AreEqual(stage, score.Stage.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06PracticeScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th06PracticeScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "pscr";
            short size1 = 0x14;
            short size2 = 0x14;
            var unknown1 = 1u;
            var highScore = 123456;
            var chara = Th06Converter.Chara.ReimuB;
            var level = ThConverter.Level.Hard;
            var stage = ThConverter.Stage.Extra;
            byte unknown2 = 2;
            var data = TestUtils.MakeByteArray(
                unknown1, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x15;
            short size2 = 0x14;
            var unknown1 = 1u;
            var highScore = 123456;
            var chara = Th06Converter.Chara.ReimuB;
            var level = ThConverter.Level.Hard;
            var stage = ThConverter.Stage.Extra;
            byte unknown2 = 2;
            var data = TestUtils.MakeByteArray(
                unknown1, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidChara() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x14;
            short size2 = 0x14;
            var unknown1 = 1u;
            var highScore = 123456;
            var chara = (Th06Converter.Chara)(-1);
            var level = ThConverter.Level.Hard;
            var stage = ThConverter.Stage.Extra;
            byte unknown2 = 2;
            var data = TestUtils.MakeByteArray(
                unknown1, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidLevel() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x14;
            short size2 = 0x14;
            var unknown1 = 1u;
            var highScore = 123456;
            var chara = Th06Converter.Chara.ReimuB;
            var level = (ThConverter.Level)(-1);
            var stage = ThConverter.Stage.Extra;
            byte unknown2 = 2;
            var data = TestUtils.MakeByteArray(
                unknown1, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06PracticeScoreTestInvalidStage() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x14;
            short size2 = 0x14;
            var unknown1 = 1u;
            var highScore = 123456;
            var chara = Th06Converter.Chara.ReimuB;
            var level = ThConverter.Level.Hard;
            var stage = (ThConverter.Stage)(-1);
            byte unknown2 = 2;
            var data = TestUtils.MakeByteArray(
                unknown1, highScore, (byte)chara, (byte)level, (byte)stage, unknown2);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th06PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
