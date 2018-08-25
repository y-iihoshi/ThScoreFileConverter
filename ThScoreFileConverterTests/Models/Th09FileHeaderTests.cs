using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09FileHeaderTests
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

        internal static Properties ValidProperties => new Properties()
        {
            checksum = 12,
            version = 0x04,
            size = 0x18,
            decodedAllSize = 78 + 0x18,
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
                properties.decodedAllSize,
                properties.decodedBodySize,
                properties.encodedBodySize);

        internal static void Validate(in Th09FileHeaderWrapper header, in Properties properties)
        {
            Assert.AreEqual(properties.checksum, header.Checksum);
            Assert.AreEqual(properties.version, header.Version);
            Assert.AreEqual(properties.size, header.Size);
            Assert.AreEqual(properties.decodedAllSize, header.DecodedAllSize);
            Assert.AreEqual(properties.decodedBodySize, header.DecodedBodySize);
            Assert.AreEqual(properties.encodedBodySize, header.EncodedBodySize);
        }

        [TestMethod]
        public void Th09FileHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();

            var header = new Th09FileHeaderWrapper();

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th09FileHeaderReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var header = Th09FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsTrue(header.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09FileHeaderReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th09FileHeaderWrapper();
            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09FileHeaderReadFromTestShortened() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties);
            array = array.Take(array.Length - 1).ToArray();

            Th09FileHeaderWrapper.Create(array);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th09FileHeaderReadFromTestExceeded() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

            var header = Th09FileHeaderWrapper.Create(array);

            Validate(header, properties);
            Assert.IsTrue(header.IsValid.Value);
        });

        [TestMethod]
        public void Th09FileHeaderReadFromTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var header = Th09FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th09FileHeaderReadFromTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            var header = Th09FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th09FileHeaderReadFromTestInvalidDecodedAllSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.decodedAllSize;

            var header = Th09FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th09FileHeaderReadFromTestInvalidDecodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.decodedBodySize;

            var header = Th09FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th09FileHeaderWriteToTest() => TestUtils.Wrap(() =>
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

                    var header = Th09FileHeaderWrapper.Create(array);
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
        public void Th09FileHeaderWriteToTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th09FileHeaderWrapper();
            header.WriteTo(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
