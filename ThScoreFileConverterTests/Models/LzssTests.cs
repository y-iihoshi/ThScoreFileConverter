using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class LzssTests
    {
        // f <- ch -->
        // 1 0100_0001
        // 1 0110_0010
        // 1 0110_0011
        // f <--- offset ---> len>
        // 0 0_0000_0000_0001 0000
        // 0 0_0000_0000_0000
        private byte[] compressed = new byte[]
        {
            0xA0, 0xD8, 0xAC, 0x60, 0x00, 0x80, 0x00, 0x00
        };

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CompressTest()
        {
            Lzss.Compress(null, null);
            Assert.Fail("Unreachable");
        }

        [TestMethod()]
        public void ExtractTest()
        {
            using (var input = new MemoryStream(this.compressed))
            using (var output = new MemoryStream())
            {
                // FIXME: Should be renamed
                Lzss.Extract(input, output);
                Assert.AreEqual(6, output.Length);

                output.Seek(0, SeekOrigin.Begin);
                Assert.AreEqual('A', (char)output.ReadByte());
                Assert.AreEqual('b', (char)output.ReadByte());
                Assert.AreEqual('c', (char)output.ReadByte());
                Assert.AreEqual('A', (char)output.ReadByte());
                Assert.AreEqual('b', (char)output.ReadByte());
                Assert.AreEqual('c', (char)output.ReadByte());
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtractTestNullBoth()
        {
            Lzss.Extract(null, null);
            Assert.Fail("Unreachable");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtractTestNullInput()
        {
            using (var output = new MemoryStream())
            {
                Lzss.Extract(null, output);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ExtractTestEmptyInput()
        {
            using (var input = new MemoryStream())
            using (var output = new MemoryStream())
            {
                Lzss.Extract(input, output);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtractTestUnreadableInput()
        {
            using (var input = new MemoryStream())
            using (var output = new MemoryStream())
            {
                input.Close();
                Lzss.Extract(input, output);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ExtractTestInvalidInput()
        {
            using (var input = new MemoryStream(this.compressed, 0, this.compressed.Length - 1))
            using (var output = new MemoryStream())
            {
                Lzss.Extract(input, output);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExtractTestNullOutput()
        {
            using (var input = new MemoryStream(this.compressed))
            {
                Lzss.Extract(input, null);
                Assert.Fail("Unreachable");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(NotSupportedException))]
        public void ExtractTestUnwritableOutput()
        {
            using (var input = new MemoryStream(this.compressed))
            using (var output = new MemoryStream(new byte[] { }, false))
            {
                Lzss.Extract(input, output);
                Assert.Fail("Unreachable");
            }
        }
    }
}
