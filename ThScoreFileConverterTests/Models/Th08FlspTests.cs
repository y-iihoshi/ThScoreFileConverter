using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th08FlspTests
    {
        [TestMethod()]
        public void Th08FlspTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "FLSP";
            short size1 = 0x20;
            short size2 = 0x20;
            var data = TestUtils.MakeRandomArray<byte>(0x18);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var flsp = new Th08FlspWrapper<Th08Converter>(chapter);

            Assert.AreEqual(signature, flsp.Signature);
            Assert.AreEqual(size1, flsp.Size1);
            Assert.AreEqual(size2, flsp.Size2);
            CollectionAssert.AreEqual(data, flsp.Data.ToArray());
            Assert.AreEqual(data[0], flsp.FirstByteOfData);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08FlspTestNullChapter() => TestUtils.Wrap(() =>
        {
            var flsp = new Th08FlspWrapper<Th08Converter>(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08FlspTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "flsp";
            short size1 = 0x20;
            short size2 = 0x20;
            var data = TestUtils.MakeRandomArray<byte>(0x18);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var flsp = new Th08FlspWrapper<Th08Converter>(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08FlspTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var signature = "FLSP";
            short size1 = 0x21;
            short size2 = 0x20;
            var data = TestUtils.MakeRandomArray<byte>(0x18);

            var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
            var flsp = new Th08FlspWrapper<Th08Converter>(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
