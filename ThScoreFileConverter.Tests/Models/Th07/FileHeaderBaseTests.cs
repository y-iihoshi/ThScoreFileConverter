using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverter.Tests.Models.Th07;

internal static class FileHeaderBaseExtensions
{
    internal static void ShouldBe(this FileHeaderBase actual, FileHeaderBaseTests.Properties expected)
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
public class FileHeaderBaseTests
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

    internal static Properties ValidProperties { get; } = MakeProperties(34, 56);

    internal static Properties MakeProperties(short version, int size)
    {
        return new()
        {
            checksum = 12,
            version = version,
            size = size,
            decodedAllSize = 78 + size,
            decodedBodySize = 78,
            encodedBodySize = 90,
        };
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            (ushort)0,
            properties.checksum,
            properties.version,
            (ushort)0,
            properties.size,
            0u,
            properties.decodedAllSize,
            properties.decodedBodySize,
            properties.encodedBodySize);
    }

    [TestMethod]
    public void FileHeaderBaseTest()
    {
        var properties = default(Properties);

        var header = new FileHeaderBase();

        header.ShouldBe(properties);
        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var properties = ValidProperties;
        var array = MakeByteArray(properties)[..^1];

        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<FileHeaderBase>(array));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var properties = ValidProperties;
        var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

        var header = TestUtils.Create<FileHeaderBase>(array);

        header.ShouldBe(properties);
        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestInvalidSize()
    {
        var properties = ValidProperties;
        ++properties.size;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTestInvalidDecodedAllSize()
    {
        var properties = ValidProperties;
        ++properties.decodedAllSize;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        header.ShouldBe(properties);
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTestInvalidDecodedBodySize()
    {
        var properties = ValidProperties;
        ++properties.decodedBodySize;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

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

        var header = TestUtils.Create<FileHeaderBase>(array);
        header.WriteTo(writer);

        writer.Flush();
        stream.ToArray().ShouldBe(array);
    }
}
