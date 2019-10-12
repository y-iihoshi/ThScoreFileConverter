using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08
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
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            checksum = 12,
            version = 0x01,
            size = 0x1C,
            decodedAllSize = 78 + 0x1C,
            decodedBodySize = 78,
            encodedBodySize = 90
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                (ushort)0,
                properties.checksum,
                properties.version,
                (ushort)0,
                properties.size,
                0u,
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
        public void FileHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();

            var header = new FileHeader();

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsTrue(header.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new FileHeader();
            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties);
            array = array.Take(array.Length - 1).ToArray();

            _ = TestUtils.Create<FileHeader>(array);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceeded() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

            var header = TestUtils.Create<FileHeader>(array);

            Validate(header, properties);
            Assert.IsTrue(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestInvalidDecodedAllSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.decodedAllSize;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestInvalidDecodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.decodedBodySize;

            var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void WriteToTest() => TestUtils.Wrap(() =>
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (var writer = new BinaryWriter(stream))
                {
                    stream = null;

                    var properties = ValidProperties;
                    var array = MakeByteArray(properties);

                    var header = TestUtils.Create<FileHeader>(array);
                    header.WriteTo(writer);

                    writer.Flush();
                    writer.Seek(0, SeekOrigin.Begin);

                    var actualArray = new byte[writer.BaseStream.Length];
                    writer.BaseStream.Read(actualArray, 0, actualArray.Length);

                    CollectionAssert.AreEqual(array, actualArray);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteToTestNull() => TestUtils.Wrap(() =>
        {
            var header = new FileHeader();
            header.WriteTo(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
