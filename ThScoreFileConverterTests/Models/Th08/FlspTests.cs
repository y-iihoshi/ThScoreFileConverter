using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class FlspTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "FLSP",
            size1 = 0x20,
            size2 = 0x20
        };

        internal static byte[] MakeData(in Properties _)
            => TestUtils.MakeByteArray(new byte[0x18]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Properties expected, in FLSP actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
        }

        [TestMethod]
        public void FlspTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var flsp = new FLSP(chapter.Target);

            Validate(properties, flsp);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FlspTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new FLSP(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void FlspTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new FLSP(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void FlspTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var flsp = new FLSP(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
