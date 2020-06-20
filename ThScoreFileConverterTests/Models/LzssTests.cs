using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class LzssTests
    {
        // "AbcAbc"
        private readonly byte[] decompressed = new byte[]
        {
            0x41, 0x62, 0x63, 0x41, 0x62, 0x63,
        };

        // f <- ch -->
        // 1 0100_0001
        // 1 0110_0010
        // 1 0110_0011
        // f <--- offset ---> len>
        // 0 0_0000_0000_0001 0000
        // 0 0_0000_0000_0000
        private readonly byte[] compressed = new byte[]
        {
            0b_1010_0000,
            0b_1101_1000,
            0b_1010_1100,
            0b_0110_0000,
            0b_0000_0000,
            0b_1000_0000,
            0b_0000_0000,
            0b_0000_0000,
        };

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CompressTest()
        {
            using var input = new MemoryStream(this.decompressed);
            using var output = new MemoryStream();

            Lzss.Compress(input, output);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void DecompressTest()
        {
            using var input = new MemoryStream(this.compressed);
            using var output = new MemoryStream();

            Lzss.Decompress(input, output);

            var actual = new byte[output.Length];
            output.Position = 0;
            _ = output.Read(actual, 0, actual.Length);

            CollectionAssert.AreEqual(this.decompressed, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecompressTestNullInput()
        {
            using var output = new MemoryStream();

            Lzss.Decompress(null!, output);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void DecompressTestNullStreamInput()
        {
            using var output = new MemoryStream();

            Lzss.Decompress(Stream.Null, output);
            Assert.AreEqual(0, output.Length);
        }

        [TestMethod]
        public void DecompressTestEmptyInput()
        {
            using var input = new MemoryStream();
            using var output = new MemoryStream();

            Lzss.Decompress(input, output);
            Assert.AreEqual(0, output.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecompressTestUnreadableInput()
        {
            using var input = new UnreadableMemoryStream();
            using var output = new MemoryStream();

            Lzss.Decompress(input, output);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecompressTestClosedInput()
        {
            using var output = new MemoryStream();
            var input = new MemoryStream(this.compressed);
            input.Close();

            Lzss.Decompress(input, output);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void DecompressTestShortenedInput()
        {
            // Since the last 2 bytes of this.compressed are zero, we should subtract 3 to pass this test.
            using var input = new MemoryStream(this.compressed, 0, this.compressed.Length - 3);
            using var output = new MemoryStream();

            Lzss.Decompress(input, output);

            var actual = new byte[output.Length];
            output.Position = 0;
            _ = output.Read(actual, 0, actual.Length);

            CollectionAssert.AreNotEqual(this.decompressed, actual);
        }

        [TestMethod]
        public void DecompressTestInvalidInput()
        {
            var invalid = new byte[this.compressed.Length];
            this.compressed.CopyTo(invalid, 0);
#if NETFRAMEWORK
            invalid[invalid.Length - 1] ^= 0x80;
#else
            invalid[^1] ^= 0x80;
#endif

            using var input = new MemoryStream(invalid);
            using var output = new MemoryStream();

            Lzss.Decompress(input, output);

            var actual = new byte[output.Length];
            output.Position = 0;
            _ = output.Read(actual, 0, actual.Length);

            CollectionAssert.AreNotEqual(this.decompressed, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DecompressTestNullOutput()
        {
            using var input = new MemoryStream(this.compressed);

            Lzss.Decompress(input, null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DecompressTestUnwritableOutput()
        {
            using var input = new MemoryStream(this.compressed);
            using var output = new MemoryStream(Array.Empty<byte>(), false);

            Lzss.Decompress(input, output);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DecompressTestClosedOutput()
        {
            using var input = new MemoryStream(this.compressed);
            var output = new MemoryStream();
            output.Close();

            Lzss.Decompress(input, output);
            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
