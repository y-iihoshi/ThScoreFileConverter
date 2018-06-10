using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th07ClearDataTests
    {
        [TestMethod()]
        public void Th07ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "CLRD";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var storyFlags = TestUtils.MakeRandomArray<byte>(6);
            var practiceFlags = TestUtils.MakeRandomArray<byte>(6);
            var chara = Th07Converter.Chara.ReimuB;
            var data = TestUtils.MakeByteArray(
                unknown,
                storyFlags,
                practiceFlags,
                (int)chara);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th07ClearDataWrapper(chapter);

            Assert.AreEqual(signature, clearData.Signature);
            Assert.AreEqual(size1, clearData.Size1);
            Assert.AreEqual(size2, clearData.Size2);
            CollectionAssert.AreEqual(data, clearData.Data.ToArray());
            Assert.AreEqual(data[0], clearData.FirstByteOfData);
            CollectionAssert.AreEqual(storyFlags, clearData.StoryFlags.Values.ToArray());
            CollectionAssert.AreEqual(practiceFlags, clearData.PracticeFlags.Values.ToArray());
            Assert.AreEqual(chara, clearData.Chara);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th07ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "clrd";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var storyFlags = TestUtils.MakeRandomArray<byte>(6);
            var practiceFlags = TestUtils.MakeRandomArray<byte>(6);
            var chara = Th07Converter.Chara.ReimuB;
            var data = TestUtils.MakeByteArray(
                unknown,
                storyFlags,
                practiceFlags,
                (int)chara);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th07ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "CLRD";
            short size1 = 0x1D;
            short size2 = 0x1C;
            var unknown = 0u;
            var storyFlags = TestUtils.MakeRandomArray<byte>(6);
            var practiceFlags = TestUtils.MakeRandomArray<byte>(6);
            var chara = Th07Converter.Chara.ReimuB;
            var data = TestUtils.MakeByteArray(
                unknown,
                storyFlags,
                practiceFlags,
                (int)chara);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th07ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th07ClearDataTestInvalidChara() => TestUtils.Wrap(() =>
        {
            var signature = "CLRD";
            short size1 = 0x1C;
            short size2 = 0x1C;
            var unknown = 0u;
            var storyFlags = TestUtils.MakeRandomArray<byte>(6);
            var practiceFlags = TestUtils.MakeRandomArray<byte>(6);
            var chara = (Th07Converter.Chara)(-1);
            var data = TestUtils.MakeByteArray(
                unknown,
                storyFlags,
                practiceFlags,
                (int)chara);

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th07ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
