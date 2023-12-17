﻿using System;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class LzssTests
{
    private readonly byte[] decompressed = "AbcAbc"u8.ToArray();

    // f <- ch -->
    // 1 0100_0001
    // 1 0110_0010
    // 1 0110_0011
    // f <--- offset ---> len>
    // 0 0_0000_0000_0001 0000
    // 0 0_0000_0000_0000
    private readonly byte[] compressed =
    [
        0b_1010_0000,
        0b_1101_1000,
        0b_1010_1100,
        0b_0110_0000,
        0b_0000_0000,
        0b_1000_0000,
        0b_0000_0000,
        0b_0000_0000,
    ];

    [TestMethod]
    public void CompressTest()
    {
        using var input = new MemoryStream(this.decompressed);
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<NotImplementedException>(() => Lzss.Compress(input, output));
    }

    [TestMethod]
    public void DecompressTest()
    {
        using var input = new MemoryStream(this.compressed);
        using var output = new MemoryStream();

        Lzss.Decompress(input, output);
        CollectionAssert.AreEqual(this.decompressed, output.ToArray());
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
    public void DecompressTestUnreadableInput()
    {
        using var input = new UnreadableMemoryStream();
        using var output = new MemoryStream();

        _ = Assert.ThrowsException<ArgumentException>(() => Lzss.Decompress(input, output));
    }

    [TestMethod]
    public void DecompressTestClosedInput()
    {
        using var output = new MemoryStream();
        var input = new MemoryStream(this.compressed);
        input.Close();

        _ = Assert.ThrowsException<ArgumentException>(() => Lzss.Decompress(input, output));
    }

    [TestMethod]
    public void DecompressTestShortenedInput()
    {
        // Since the last 2 bytes of this.compressed are zero, we should subtract 3 to pass this test.
        using var input = new MemoryStream(this.compressed, 0, this.compressed.Length - 3);
        using var output = new MemoryStream();

        Lzss.Decompress(input, output);
        CollectionAssert.AreNotEqual(this.decompressed, output.ToArray());
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
        CollectionAssert.AreNotEqual(this.decompressed, output.ToArray());
    }

    [TestMethod]
    public void DecompressTestUnwritableOutput()
    {
        using var input = new MemoryStream(this.compressed);
        using var output = new MemoryStream(Array.Empty<byte>(), false);

        _ = Assert.ThrowsException<NotSupportedException>(() => Lzss.Decompress(input, output));
    }

    [TestMethod]
    public void DecompressTestClosedOutput()
    {
        using var input = new MemoryStream(this.compressed);
        var output = new MemoryStream();
        output.Close();

        _ = Assert.ThrowsException<ObjectDisposedException>(() => Lzss.Decompress(input, output));
    }
}
