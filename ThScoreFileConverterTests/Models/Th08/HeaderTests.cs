using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class HeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] data;
        }

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "TH8K",
            size1 = 12,
            size2 = 12,
            data = new byte[] { 0x10, 0x00, 0x00, 0x00 },
        };

        internal static byte[] MakeByteArray(in Properties properties)
        {
            return TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, properties.data);
        }

        internal static void Validate(in Properties expected, in Header actual)
        {
            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            Assert.AreEqual(expected.data[0], actual.FirstByteOfData);
        }

        [TestMethod]
        public void HeaderTest()
        {
            var properties = ValidProperties;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            var header = new Header(chapter);

            Validate(properties, header);
        }

        [TestMethod]
        public void HeaderTestInvalidSignature()
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Header(chapter));
        }

        [TestMethod]
        public void HeaderTestInvalidSize1()
        {
            var properties = ValidProperties;
            ++properties.size1;
            properties.data = properties.data.Concat(new byte[] { default }).ToArray();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new Header(chapter));
        }
    }
}
