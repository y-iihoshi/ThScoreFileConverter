using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

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
        }

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "FLSP",
            size1 = 0x20,
            size2 = 0x20,
        };

        internal static byte[] MakeData(in Properties _)
        {
            return TestUtils.MakeByteArray(new byte[0x18]);
        }

        internal static byte[] MakeByteArray(in Properties properties)
        {
            return TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));
        }

        internal static void Validate(in Properties expected, in FLSP actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
        }

        [TestMethod]
        public void FlspTestChapter()
        {
            var properties = ValidProperties;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            var flsp = new FLSP(chapter);

            Validate(properties, flsp);
        }

        [TestMethod]
        public void FlspTestInvalidSignature()
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new FLSP(chapter));
        }

        [TestMethod]
        public void FlspTestInvalidSize1()
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new FLSP(chapter));
        }
    }
}
