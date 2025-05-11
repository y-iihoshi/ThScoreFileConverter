using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

internal static class FileHeaderExtensions
{
    internal static void ShouldBe(this FileHeader actual, FileHeaderTests.Properties expected)
    {
        actual.Checksum.ShouldBe(expected.checksum);
        actual.Version.ShouldBe(expected.version);
        actual.Size.ShouldBe(expected.size);
        actual.DecodedAllSize.ShouldBe(expected.decodedAllSize);
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
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        checksum = 12,
        version = 0x10,
        size = 0x14,
        decodedAllSize = 78,
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            (ushort)0,
            properties.checksum,
            properties.version,
            (ushort)0,
            properties.size,
            0u,
            properties.decodedAllSize);
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
