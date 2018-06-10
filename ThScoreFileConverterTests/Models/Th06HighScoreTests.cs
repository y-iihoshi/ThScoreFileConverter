using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        [TestMethod()]
        public void Th06HighScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "HSCR";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var score = 1234567u;
            var chara = Th06Converter.Chara.ReimuB;
            var level = ThConverter.Level.Hard;
            var progress = Th06Converter.StageProgress.St3;
            var name = "Player1\0\0";
            var data = TestUtils.MakeByteArray(
                unknown,
                score,
                (byte)chara,
                (byte)level,
                (byte)progress,
                name.ToCharArray());

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.AreEqual(signature, highScore.Signature);
            Assert.AreEqual(size1, highScore.Size1);
            Assert.AreEqual(size2, highScore.Size2);
            CollectionAssert.AreEqual(data, highScore.Data.ToArray());
            Assert.AreEqual(data[0], highScore.FirstByteOfData);
            Assert.AreEqual(score, highScore.Score);
            Assert.AreEqual(chara, highScore.Chara);
            Assert.AreEqual(level, highScore.Level);
            Assert.AreEqual(progress, highScore.StageProgress);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(name), highScore.Name.ToArray());
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

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HighScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "hscr";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var score = 1234567u;
            var chara = Th06Converter.Chara.ReimuA;
            var level = ThConverter.Level.Easy;
            var progress = Th06Converter.StageProgress.St1;
            var name = "Player1\0\0";
            var data = TestUtils.MakeByteArray(
                unknown,
                score,
                (byte)chara,
                (byte)level,
                (byte)progress,
                name.ToCharArray());

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HighScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "HSCR";
            short size1 = 0x1D;
            short size2 = 0x1C;
            var unknown = 0u;
            var score = 1234567u;
            var chara = Th06Converter.Chara.ReimuA;
            var level = ThConverter.Level.Easy;
            var progress = Th06Converter.StageProgress.St1;
            var name = "Player1\0\0";
            var data = TestUtils.MakeByteArray(
                unknown,
                score,
                (byte)chara,
                (byte)level,
                (byte)progress,
                name.ToCharArray());

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06HighScoreTestInvalidChara() => TestUtils.Wrap(() =>
        {
            var signature = "HSCR";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var score = 1234567u;
            var chara = (Th06Converter.Chara)(-1);
            var level = ThConverter.Level.Hard;
            var progress = Th06Converter.StageProgress.St3;
            var name = "Player1\0\0";
            var data = TestUtils.MakeByteArray(
                unknown,
                score,
                (byte)chara,
                (byte)level,
                (byte)progress,
                name.ToCharArray());

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06HighScoreTestInvalidLevel() => TestUtils.Wrap(() =>
        {
            var signature = "HSCR";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var score = 1234567u;
            var chara = Th06Converter.Chara.ReimuB;
            var level = (ThConverter.Level)(-1);
            var progress = Th06Converter.StageProgress.St3;
            var name = "Player1\0\0";
            var data = TestUtils.MakeByteArray(
                unknown,
                score,
                (byte)chara,
                (byte)level,
                (byte)progress,
                name.ToCharArray());

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "highScore")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06HighScoreTestInvalidStageProgress() => TestUtils.Wrap(() =>
        {
            var signature = "HSCR";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var score = 1234567u;
            var chara = Th06Converter.Chara.ReimuB;
            var level = ThConverter.Level.Hard;
            var progress = (Th06Converter.StageProgress)(-1);
            var name = "Player1\0\0";
            var data = TestUtils.MakeByteArray(
                unknown,
                score,
                (byte)chara,
                (byte)level,
                (byte)progress,
                name.ToCharArray());

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var highScore = new Th06HighScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
