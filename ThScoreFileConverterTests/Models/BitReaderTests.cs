using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class BitReaderTests
    {
        [TestMethod()]
        public void BitReaderTest()
        {
            using (var stream = new MemoryStream())
            using (var reader = new BitReader(stream))
            {
                // do nothing
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BitReaderTestNoStream()
        {
            using (var reader = new BitReader(null))
            {
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void BitReaderTestUnreadable()
        {
            var stream = new MemoryStream();
            stream.Close();
            using (var reader = new BitReader(stream))
            {
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest()
        {
            using (var stream = new MemoryStream())
            {
                var reader = new BitReader(stream);
                reader.Dispose();
                reader.ReadBits(1);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        public void ReadBitsTestOneBit()
        {
            using (var stream = new MemoryStream(new byte[] { 0x53 }))
            using (var reader = new BitReader(stream))
            {
                Assert.AreEqual(0x0, reader.ReadBits(1));
                Assert.AreEqual(0x1, reader.ReadBits(1));
                Assert.AreEqual(0x0, reader.ReadBits(1));
                Assert.AreEqual(0x1, reader.ReadBits(1));

                Assert.AreEqual(0x0, reader.ReadBits(1));
                Assert.AreEqual(0x0, reader.ReadBits(1));
                Assert.AreEqual(0x1, reader.ReadBits(1));
                Assert.AreEqual(0x1, reader.ReadBits(1));
            }
        }

        [TestMethod()]
        public void ReadBitsTestZeroBit()
        {
            using (var stream = new MemoryStream(new byte[] { 0xFF }))
            using (var reader = new BitReader(stream))
            {
                Assert.AreEqual(0x0, reader.ReadBits(0));
                Assert.AreEqual(0x0, reader.ReadBits(0));
                Assert.AreEqual(0x0, reader.ReadBits(0));
                Assert.AreEqual(0x0, reader.ReadBits(0));

                Assert.AreEqual(0x0, reader.ReadBits(0));
                Assert.AreEqual(0x0, reader.ReadBits(0));
                Assert.AreEqual(0x0, reader.ReadBits(0));
                Assert.AreEqual(0x0, reader.ReadBits(0));

                Assert.AreEqual(0xFF, reader.ReadBits(8));
            }
        }

        [TestMethod()]
        public void ReadBitsTestMultiBits()
        {
            // var buffer = new byte[2] { 0b_0101_0011, 0b_1100_1010 };
            var buffer = new byte[2] { 0x53, 0xCA };
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BitReader(stream))
            {
                Assert.AreEqual(0x1, reader.ReadBits(2));
                Assert.AreEqual(0x2, reader.ReadBits(3));
                Assert.AreEqual(0x3C, reader.ReadBits(7));
                Assert.AreEqual(0xA, reader.ReadBits(4));
            }
        }

        [TestMethod()]
        public void ReadBitsTestMultiBytes()
        {
            var buffer = new byte[6] { 0x53, 0xCA, 0xAC, 0x35, 0x5A, 0xA5 };
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BitReader(stream))
            {
                Assert.AreEqual(0x53CA, reader.ReadBits(16));
                Assert.AreEqual(0xAC355AA5, reader.ReadBits(32));
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadBitsTestNegativeNumBits()
        {
            using (var stream = new MemoryStream(new byte[] { 0x53 }))
            using (var reader = new BitReader(stream))
            {
                reader.ReadBits(-1);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadBitsTestExceededNumBits()
        {
            var buffer = new byte[6] { 0x53, 0xCA, 0xAC, 0x35, 0x5A, 0xA5 };
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BitReader(stream))
            {
                reader.ReadBits(33);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadBitsTestEndOfStream()
        {
            using (var stream = new MemoryStream(new byte[] { 0x53 }))
            using (var reader = new BitReader(stream))
            {
                Assert.AreEqual(0x53, reader.ReadBits(8));

                reader.ReadBits(1);
                Assert.Fail("Unreachable");
            }
        }
    }
}
