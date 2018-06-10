using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th08ClearDataTests
    {
        [TestMethod()]
        public void Th08ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "CLRD";
            short size1 = 0x24;
            short size2 = 0x24;
            var unknown1 = 0u;
            var storyFlags = TestUtils.MakeRandomArray<ushort>(5);
            var practiceFlags = TestUtils.MakeRandomArray<ushort>(5);
            byte unknown2 = 0;
            var chara = Th08Converter.CharaWithTotal.MarisaAlice;
            ushort unknown3 = 0;
            var data = TestUtils.MakeByteArray(
                unknown1,
                storyFlags,
                practiceFlags,
                unknown2,
                (byte)chara,
                unknown3);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.AreEqual(signature, clearData.Signature);
            Assert.AreEqual(size1, clearData.Size1);
            Assert.AreEqual(size2, clearData.Size2);
            CollectionAssert.AreEqual(data, clearData.Data.ToArray());
            Assert.AreEqual(data[0], clearData.FirstByteOfData);
            CollectionAssert.AreEqual(storyFlags.Select(i => (int)i).ToArray(), clearData.ValuesOfStoryFlags);
            CollectionAssert.AreEqual(practiceFlags.Select(i => (int)i).ToArray(), clearData.ValuesOfPracticeFlags);
            Assert.AreEqual(chara, clearData.Chara);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th08ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "clrd";
            short size1 = 0x24;
            short size2 = 0x24;
            var unknown1 = 0u;
            var storyFlags = TestUtils.MakeRandomArray<ushort>(5);
            var practiceFlags = TestUtils.MakeRandomArray<ushort>(5);
            byte unknown2 = 0;
            var chara = Th08Converter.CharaWithTotal.MarisaAlice;
            ushort unknown3 = 0;
            var data = TestUtils.MakeByteArray(
                unknown1,
                storyFlags,
                practiceFlags,
                unknown2,
                (byte)chara,
                unknown3);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "CLRD";
            short size1 = 0x25;
            short size2 = 0x24;
            var unknown1 = 0u;
            var storyFlags = TestUtils.MakeRandomArray<ushort>(5);
            var practiceFlags = TestUtils.MakeRandomArray<ushort>(5);
            byte unknown2 = 0;
            var chara = Th08Converter.CharaWithTotal.MarisaAlice;
            ushort unknown3 = 0;
            var data = TestUtils.MakeByteArray(
                unknown1,
                storyFlags,
                practiceFlags,
                unknown2,
                (byte)chara,
                unknown3);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08ClearDataTestInvalidChara() => TestUtils.Wrap(() =>
        {
            var signature = "CLRD";
            short size1 = 0x24;
            short size2 = 0x24;
            var unknown1 = 0u;
            var storyFlags = TestUtils.MakeRandomArray<ushort>(5);
            var practiceFlags = TestUtils.MakeRandomArray<ushort>(5);
            byte unknown2 = 0;
            var chara = (Th08Converter.CharaWithTotal)(-1);
            ushort unknown3 = 0;
            var data = TestUtils.MakeByteArray(
                unknown1,
                storyFlags,
                practiceFlags,
                unknown2,
                (byte)chara,
                unknown3);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
