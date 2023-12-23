using System;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class BitReaderTests
{
    [TestMethod]
    public void BitReaderTest()
    {
        using var stream = new MemoryStream();
        {
            _ = new BitReader(stream);
        }

        Assert.IsTrue(stream.CanRead);
    }

    [TestMethod]
    public void BitReaderTestNoStream()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new BitReader(null!));
    }

    [TestMethod]
    public void BitReaderTestUnreadable()
    {
        var stream = new MemoryStream();
        stream.Close();

        _ = Assert.ThrowsException<ArgumentException>(() => new BitReader(stream));
    }

    [TestMethod]
    public void ReadBitsTestOneBit()
    {
        using var stream = new MemoryStream([0b_0101_0011]);

        var reader = new BitReader(stream);

        Assert.AreEqual(0b0, reader.ReadBits(1));
        Assert.AreEqual(0b1, reader.ReadBits(1));
        Assert.AreEqual(0b0, reader.ReadBits(1));
        Assert.AreEqual(0b1, reader.ReadBits(1));

        Assert.AreEqual(0b0, reader.ReadBits(1));
        Assert.AreEqual(0b0, reader.ReadBits(1));
        Assert.AreEqual(0b1, reader.ReadBits(1));
        Assert.AreEqual(0b1, reader.ReadBits(1));
    }

    [TestMethod]
    public void ReadBitsTestZeroBit()
    {
        using var stream = new MemoryStream([0xFF]);
        var reader = new BitReader(stream);

        Assert.AreEqual(0, reader.ReadBits(0));
        Assert.AreEqual(0, reader.ReadBits(0));
        Assert.AreEqual(0, reader.ReadBits(0));
        Assert.AreEqual(0, reader.ReadBits(0));

        Assert.AreEqual(0, reader.ReadBits(0));
        Assert.AreEqual(0, reader.ReadBits(0));
        Assert.AreEqual(0, reader.ReadBits(0));
        Assert.AreEqual(0, reader.ReadBits(0));

        Assert.AreEqual(0xFF, reader.ReadBits(8));
    }

    [TestMethod]
    public void ReadBitsTestMultiBits()
    {
        var buffer = new byte[2] { 0b_0101_0011, 0b_1100_1010 };
        using var stream = new MemoryStream(buffer);
        var reader = new BitReader(stream);

        Assert.AreEqual(0b01, reader.ReadBits(2));
        Assert.AreEqual(0b010, reader.ReadBits(3));
        Assert.AreEqual(0b_011_1100, reader.ReadBits(7));
        Assert.AreEqual(0b1010, reader.ReadBits(4));
    }

    [TestMethod]
    public void ReadBitsTestMultiBytes()
    {
        var buffer = new byte[6] { 0x53, 0xCA, 0xAC, 0x35, 0x5A, 0xA5 };
        using var stream = new MemoryStream(buffer);
        var reader = new BitReader(stream);

        Assert.AreEqual(0x53CA, reader.ReadBits(16));

        // NOTE: Changing the return type to uint makes BitReader.ReadBits() to CLS-noncompliant.
        Assert.AreEqual(0x_AC35_5AA5, (uint)reader.ReadBits(32));
    }

    [TestMethod]
    public void ReadBitsTestNegativeNumBits()
    {
        using var stream = new MemoryStream([0x53]);

        var reader = new BitReader(stream);

        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => reader.ReadBits(-1));
    }

    [TestMethod]
    public void ReadBitsTestExceededNumBits()
    {
#pragma warning disable IDE0230 // Use UTF-8 string literal
        var buffer = new byte[5]
        {
            0b_0101_0011,
            0b_1100_1010,
            0b_1010_1100,
            0b_0011_0101,
            0b_0101_1010,
        };
#pragma warning restore IDE0230 // Use UTF-8 string literal

        using var stream = new MemoryStream(buffer);
        var reader = new BitReader(stream);

        Assert.AreEqual(0b_1010_0111_1001_0101_0101_1000_0110_1010, (uint)reader.ReadBits(33));
    }

    [TestMethod]
    public void ReadBitsTestEndOfStream()
    {
        using var stream = new MemoryStream([0x53]);

        var reader = new BitReader(stream);

        Assert.AreEqual(0x53, reader.ReadBits(8));
        Assert.AreEqual(0, reader.ReadBits(1));
    }
}
