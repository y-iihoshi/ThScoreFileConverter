using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class BinaryReaderExtensionsTests
    {
        [TestMethod]
        public void ReadExactBytesTest()
        {
            MemoryStream stream = null;
            try
            {
                var bytes = new byte[] { 1, 2, 3, 4, 5 };
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    var readBytes = reader.ReadExactBytes(bytes.Length);

                    CollectionAssert.That.AreEqual(bytes, readBytes);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadExactBytesTestNull()
        {
            BinaryReader reader = null;

            reader.ReadExactBytes(1);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadExactBytesTestEmptyStream()
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    reader.ReadExactBytes(1);

                    Assert.Fail(TestUtils.Unreachable);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadExactBytesTestNegative()
        {
            MemoryStream stream = null;
            try
            {
                var bytes = new byte[] { 1, 2, 3, 4, 5 };
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    reader.ReadExactBytes(-1);

                    Assert.Fail(TestUtils.Unreachable);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        public void ReadExactBytesTestZero()
        {
            MemoryStream stream = null;
            try
            {
                var bytes = new byte[] { 1, 2, 3, 4, 5 };
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    var readBytes = reader.ReadExactBytes(0);

                    CollectionAssert.That.AreEqual(new byte[] { }, readBytes);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadExactBytesTestShortened()
        {
            MemoryStream stream = null;
            try
            {
                var bytes = new byte[] { 1, 2, 3, 4, 5 };
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    reader.ReadExactBytes(bytes.Length + 1);

                    Assert.Fail(TestUtils.Unreachable);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        public void ReadExactBytesTestExceeded()
        {
            MemoryStream stream = null;
            try
            {
                var bytes = new byte[] { 1, 2, 3, 4, 5 };
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    var readBytes = reader.ReadExactBytes(bytes.Length - 1);

                    CollectionAssert.That.AreNotEqual(bytes, readBytes);
                    CollectionAssert.That.AreEqual(bytes.Take(readBytes.Length), readBytes);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }
}
