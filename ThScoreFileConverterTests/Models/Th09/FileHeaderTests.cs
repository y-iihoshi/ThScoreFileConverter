using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class FileHeaderTests
    {
        internal struct Properties
        {
            public ushort checksum;
            public short version;
            public int size;
            public int decodedAllSize;
            public int decodedBodySize;
            public int encodedBodySize;
        }

        internal static Properties ValidProperties { get; } = new Properties()
        {
            checksum = 12,
            version = 0x04,
            size = 0x18,
            decodedAllSize = 78 + 0x18,
            decodedBodySize = 78,
            encodedBodySize = 90,
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                (ushort)0,
                properties.checksum,
                properties.version,
                (ushort)0,
                properties.size,
                properties.decodedAllSize,
                properties.decodedBodySize,
                properties.encodedBodySize);

        internal static void Validate(in FileHeader header, in Properties properties)
        {
            Assert.AreEqual(properties.checksum, header.Checksum);
            Assert.AreEqual(properties.version, header.Version);
            Assert.AreEqual(properties.size, header.Size);
            Assert.AreEqual(properties.decodedAllSize, header.DecodedAllSize);
            Assert.AreEqual(properties.decodedBodySize, header.DecodedBodySize);
            Assert.AreEqual(properties.encodedBodySize, header.EncodedBodySize);
        }

        [TestMethod]
        public void FileHeaderTest()
        {
            var properties = default(Properties);

            var header = new FileHeader();

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var properties = ValidProperties;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void ReadFromTestShortened()
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties);
            array = array.Take(array.Length - 1).ToArray();

            _ = Assert.ThrowsException<EndOfStreamException>(() => _ = TestUtils.Create<FileHeader>(array));
        }

        [TestMethod]
        public void ReadFromTestExceeded()
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

            var header = TestUtils.Create<FileHeader>(array);

            Validate(header, properties);
            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void ReadFromTestInvalidVersion()
        {
            var properties = ValidProperties;
            ++properties.version;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void ReadFromTestInvalidSize()
        {
            var properties = ValidProperties;
            ++properties.size;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void ReadFromTestInvalidDecodedAllSize()
        {
            var properties = ValidProperties;
            ++properties.decodedAllSize;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void ReadFromTestInvalidDecodedBodySize()
        {
            var properties = ValidProperties;
            ++properties.decodedBodySize;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void WriteToTest()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var properties = ValidProperties;
            var array = MakeByteArray(properties);

            var header = TestUtils.Create<FileHeader>(array);
            header.WriteTo(writer);

            writer.Flush();
            CollectionAssert.AreEqual(array, stream.ToArray());
        }
    }
}
