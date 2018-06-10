using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th08PracticeScoreTests
    {
        [TestMethod()]
        public void Th08PracticeScoreTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x178;
            short size2 = 0x178;
            var unknown1 = 1u;
            var playCounts = TestUtils.MakeRandomArray<int>(45);
            var highScores = TestUtils.MakeRandomArray<int>(45);
            var chara = Th08Converter.Chara.MarisaAlice;
            var unknown2 = TestUtils.MakeRandomArray<byte>(3);
            var data = TestUtils.MakeByteArray(unknown1, playCounts, highScores, (byte)chara, unknown2);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.AreEqual(signature, score.Signature);
            Assert.AreEqual(size1, score.Size1);
            Assert.AreEqual(size2, score.Size2);
            CollectionAssert.AreEqual(data, score.Data.ToArray());
            Assert.AreEqual(data[0], score.FirstByteOfData);
            CollectionAssert.AreEqual(playCounts, score.PlayCountsValues.ToArray());
            CollectionAssert.AreEqual(highScores, score.HighScoresValues.ToArray());
            Assert.AreEqual(chara, score.Chara.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08PracticeScoreTestNullChapter() => TestUtils.Wrap(() =>
        {
            var score = new Th08PracticeScoreWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PracticeScoreTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "pscr";
            short size1 = 0x178;
            short size2 = 0x178;
            var unknown1 = 1u;
            var playCounts = TestUtils.MakeRandomArray<int>(45);
            var highScores = TestUtils.MakeRandomArray<int>(45);
            var chara = Th08Converter.Chara.MarisaAlice;
            var unknown2 = TestUtils.MakeRandomArray<byte>(3);
            var data = TestUtils.MakeByteArray(unknown1, playCounts, highScores, (byte)chara, unknown2);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PracticeScoreTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x179;
            short size2 = 0x178;
            var unknown1 = 1u;
            var playCounts = TestUtils.MakeRandomArray<int>(45);
            var highScores = TestUtils.MakeRandomArray<int>(45);
            var chara = Th08Converter.Chara.MarisaAlice;
            var unknown2 = TestUtils.MakeRandomArray<byte>(3);
            var data = TestUtils.MakeByteArray(unknown1, playCounts, highScores, (byte)chara, unknown2);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "score")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08PracticeScoreTestInvalidChara() => TestUtils.Wrap(() =>
        {
            var signature = "PSCR";
            short size1 = 0x178;
            short size2 = 0x178;
            var unknown1 = 1u;
            var playCounts = TestUtils.MakeRandomArray<int>(45);
            var highScores = TestUtils.MakeRandomArray<int>(45);
            var chara = (Th08Converter.Chara)(-1);
            var unknown2 = TestUtils.MakeRandomArray<byte>(3);
            var data = TestUtils.MakeByteArray(unknown1, playCounts, highScores, (byte)chara, unknown2);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var score = new Th08PracticeScoreWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
