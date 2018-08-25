using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06FileHeaderTests
    {
        internal struct Properties
        {
            public ushort checksum;
            public short version;
            public int size;
            public int decodedAllSize;
        };

        internal static Properties ValidProperties => new Properties()
        {
            checksum = 12,
            version = 0x10,
            size = 0x14,
            decodedAllSize = 78
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                (ushort)0,
                properties.checksum,
                properties.version,
                (ushort)0,
                properties.size,
                0u,
                properties.decodedAllSize);

        internal static void Validate(in Th06FileHeaderWrapper header, in Properties properties)
        {
            Assert.AreEqual(properties.checksum, header.Checksum);
            Assert.AreEqual(properties.version, header.Version);
            Assert.AreEqual(properties.size, header.Size);
            Assert.AreEqual(properties.decodedAllSize, header.DecodedAllSize);
        }

        [TestMethod]
        public void Th06FileHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();

            var header = new Th06FileHeaderWrapper();

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th06FileHeaderReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var header = Th06FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsTrue(header.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06FileHeaderReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th06FileHeaderWrapper();
            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06FileHeaderReadFromTestShortened() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties);
            array = array.Take(array.Length - 1).ToArray();

            Th06FileHeaderWrapper.Create(array);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th06FileHeaderReadFromTestExceeded() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

            var header = Th06FileHeaderWrapper.Create(array);

            Validate(header, properties);
            Assert.IsTrue(header.IsValid.Value);
        });

        [TestMethod]
        public void Th06FileHeaderReadFromTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var header = Th06FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th06FileHeaderReadFromTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            var header = Th06FileHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
            Assert.IsFalse(header.IsValid.Value);
        });

        [TestMethod]
        public void Th06FileHeaderWriteToTest() => TestUtils.Wrap(() =>
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

                    var header = Th06FileHeaderWrapper.Create(array);
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
        public void Th06FileHeaderWriteToTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th06FileHeaderWrapper();
            header.WriteTo(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
