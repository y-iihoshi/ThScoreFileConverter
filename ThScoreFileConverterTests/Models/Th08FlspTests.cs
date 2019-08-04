using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08FlspTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "FLSP",
            size1 = 0x20,
            size2 = 0x20
        };

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "properties")]
#pragma warning disable IDE0060 // Remove unused parameter
        internal static byte[] MakeData(in Properties properties)
#pragma warning restore IDE0060 // Remove unused parameter
            => TestUtils.MakeByteArray(new byte[0x18]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th08FlspWrapper flsp, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, flsp.Signature);
            Assert.AreEqual(properties.size1, flsp.Size1);
            Assert.AreEqual(properties.size2, flsp.Size2);
            CollectionAssert.AreEqual(data, flsp.Data.ToArray());
            Assert.AreEqual(data[0], flsp.FirstByteOfData);
        }

        [TestMethod]
        public void Th08FlspTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var flsp = new Th08FlspWrapper(chapter);

            Validate(flsp, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08FlspTestNullChapter() => TestUtils.Wrap(() =>
        {
            var flsp = new Th08FlspWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08FlspTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var flsp = new Th08FlspWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "flsp")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08FlspTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var flsp = new Th08FlspWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
