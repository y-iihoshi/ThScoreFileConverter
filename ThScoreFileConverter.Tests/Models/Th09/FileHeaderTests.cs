﻿using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverter.Tests.Models.Th09;

internal static class FileHeaderExtensions
{
    internal static void ShouldBe(this FileHeader actual, FileHeaderTests.Properties expected)
    {
        actual.Checksum.ShouldBe(expected.checksum);
        actual.Version.ShouldBe(expected.version);
        actual.Size.ShouldBe(expected.size);
        actual.DecodedAllSize.ShouldBe(expected.decodedAllSize);
        actual.DecodedBodySize.ShouldBe(expected.decodedBodySize);
        actual.EncodedBodySize.ShouldBe(expected.encodedBodySize);
    }
}

[TestClass]
public class FileHeaderTests
{
    internal struct Properties
    {
        public ushort checksum;
        public short version;
        public int size;
        public int decodedAllSize;
        public int decodedBodySize;
        public int encodedBodySize;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        checksum = 12,
        version = 0x04,
        size = 0x18,
        decodedAllSize = 78 + 0x18,
        decodedBodySize = 78,
        encodedBodySize = 90,
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            (ushort)0,
            properties.checksum,
            properties.version,
            (ushort)0,
            properties.size,
            properties.decodedAllSize,
            properties.decodedBodySize,
            properties.encodedBodySize);
    }

    [TestMethod]
    public void FileHeaderTest()
    {
        var properties = default(Properties);

        var header = new FileHeader();

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var properties = ValidProperties;

        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<FileHeader>(MakeByteArray(properties)[..^1]));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var properties = ValidProperties;

        var header = TestUtils.Create<FileHeader>([.. MakeByteArray(properties), 1]);

        header.ShouldBe(properties);
        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestInvalidVersion()
    {
        var properties = ValidProperties;
        ++properties.version;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTestInvalidSize()
    {
        var properties = ValidProperties;
        ++properties.size;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTestInvalidDecodedAllSize()
    {
        var properties = ValidProperties;
        ++properties.decodedAllSize;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTestInvalidDecodedBodySize()
    {
        var properties = ValidProperties;
        ++properties.decodedBodySize;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void WriteToTest()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        var properties = ValidProperties;
        var array = MakeByteArray(properties);

        var header = TestUtils.Create<FileHeader>(array);
        header.WriteTo(writer);

        writer.Flush();
        stream.ToArray().ShouldBe(array);
    }
}
