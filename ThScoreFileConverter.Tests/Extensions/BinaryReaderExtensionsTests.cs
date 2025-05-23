﻿using System.Text;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Extensions;

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

        readBytes.ShouldBe(bytes);
    }

    [TestMethod]
    public void ReadExactBytesTestNull()
    {
        BinaryReader reader = null!;

        _ = Should.Throw<ArgumentNullException>(() => reader.ReadExactBytes(1));
    }

    [TestMethod]
    public void ReadExactBytesTestEmptyStream()
    {
        using var stream = new MemoryStream();
        using var reader = new BinaryReader(stream);

        _ = Should.Throw<EndOfStreamException>(() => reader.ReadExactBytes(1));
    }

    [TestMethod]
    public void ReadExactBytesTestNegative()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        _ = Should.Throw<ArgumentOutOfRangeException>(() => reader.ReadExactBytes(-1));
    }

    [TestMethod]
    public void ReadExactBytesTestZero()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        var readBytes = reader.ReadExactBytes(0);

        readBytes.ShouldBeEmpty();
    }

    [TestMethod]
    public void ReadExactBytesTestShortened()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        _ = Should.Throw<EndOfStreamException>(() => reader.ReadExactBytes(bytes.Length + 1));
    }

    [TestMethod]
    public void ReadExactBytesTestExceeded()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        var readBytes = reader.ReadExactBytes(bytes.Length - 1);

        readBytes.ShouldNotBe(bytes);
        readBytes.ShouldBe(bytes[..readBytes.Length]);
    }

    public static IEnumerable<object[]> ReadNullTerminatedStringTestData
    {
        get
        {
            yield return new object[] { "博麗 霊夢", EncodingHelper.UTF8NoBOM, null!, };
            yield return new object[] { "博麗 霊夢", EncodingHelper.UTF8NoBOM, EncodingHelper.UTF8NoBOM, };
            yield return new object[] { "博麗 霊夢", EncodingHelper.UTF8NoBOM, Encoding.UTF8, };
            yield return new object[] { "博麗 霊夢", Encoding.UTF8, null!, };
            yield return new object[] { "博麗 霊夢", Encoding.UTF8, EncodingHelper.UTF8NoBOM, };
            yield return new object[] { "博麗 霊夢", Encoding.UTF8, Encoding.UTF8, };
            yield return new object[] { "博麗 霊夢", EncodingHelper.CP932, EncodingHelper.CP932, };
            yield return new object[] { string.Empty, EncodingHelper.UTF8NoBOM, null!, };
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(ReadNullTerminatedStringTestData))]
    public void ReadNullTerminatedStringTest(string expected, Encoding encoding, Encoding? encodingToRead)
    {
        var nullTerminated = $"{expected}\0";
        var bytes = encoding.GetBytes(nullTerminated);
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        var actual = encodingToRead is null ? reader.ReadNullTerminatedString() : reader.ReadNullTerminatedString(encodingToRead);

        actual.ShouldBe(expected);
        actual.ShouldNotBe(nullTerminated);
    }

    [TestMethod]
    public void ReadNullTerminatedStringTestNullReader()
    {
        BinaryReader reader = null!;

        _ = Should.Throw<ArgumentNullException>(reader.ReadNullTerminatedString);
    }

    [TestMethod]
    public void ReadNullTerminatedStringTestNullEncoding()
    {
        var expected = "博霊 霊夢";
        var nullTerminated = $"{expected}\0";
        var bytes = Encoding.UTF8.GetBytes(nullTerminated);
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        _ = Should.Throw<ArgumentNullException>(() => reader.ReadNullTerminatedString(null!));
    }

    [TestMethod]
    public void ReadNullTerminatedStringTestEmptyStream()
    {
        using var stream = new MemoryStream();
        using var reader = new BinaryReader(stream);

        _ = Should.Throw<EndOfStreamException>(reader.ReadNullTerminatedString);
    }

    [TestMethod]
    public void ReadNullTerminatedStringTestNotNullTerminated()
    {
        var expected = "博霊 霊夢";
        var bytes = EncodingHelper.UTF8NoBOM.GetBytes(expected);
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        _ = Should.Throw<EndOfStreamException>(reader.ReadNullTerminatedString);
    }

    [TestMethod]
    public void ReadNullTerminatedStringTestInvalidEncoding()
    {
        var expected = "博霊 霊夢";
        var nullTerminated = $"{expected}\0";
        var bytes = EncodingHelper.CP932.GetBytes(nullTerminated);
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        var actual = reader.ReadNullTerminatedString();

        actual.ShouldNotBe(expected);
    }
}
