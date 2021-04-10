using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Extensions
{
    [TestClass]
    public class BinaryReaderExtensionsTests
    {
        [TestMethod]
        public void ReadExactBytesTest()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            var readBytes = reader.ReadExactBytes(bytes.Length);

            CollectionAssert.That.AreEqual(bytes, readBytes);
        }

        [TestMethod]
        public void ReadExactBytesTestNull()
        {
            BinaryReader reader = null!;

            _ = Assert.ThrowsException<ArgumentNullException>(() => _ = reader.ReadExactBytes(1));
        }

        [TestMethod]
        public void ReadExactBytesTestEmptyStream()
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            _ = Assert.ThrowsException<EndOfStreamException>(() => _ = reader.ReadExactBytes(1));
        }

        [TestMethod]
        public void ReadExactBytesTestNegative()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = reader.ReadExactBytes(-1));
        }

        [TestMethod]
        public void ReadExactBytesTestZero()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            var readBytes = reader.ReadExactBytes(0);

            CollectionAssert.That.AreEqual(Array.Empty<byte>(), readBytes);
        }

        [TestMethod]
        public void ReadExactBytesTestShortened()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            _ = Assert.ThrowsException<EndOfStreamException>(() => _ = reader.ReadExactBytes(bytes.Length + 1));
        }

        [TestMethod]
        public void ReadExactBytesTestExceeded()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            var readBytes = reader.ReadExactBytes(bytes.Length - 1);

            CollectionAssert.That.AreNotEqual(bytes, readBytes);
            CollectionAssert.That.AreEqual(bytes.Take(readBytes.Length), readBytes);
        }
    }
}
