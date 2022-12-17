using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th07;

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

    internal static void Validate(in Properties expected, in FileHeaderBase actual)
    {
        Assert.AreEqual(expected.checksum, actual.Checksum);
        Assert.AreEqual(expected.version, actual.Version);
        Assert.AreEqual(expected.size, actual.Size);
        Assert.AreEqual(expected.decodedAllSize, actual.DecodedAllSize);
        Assert.AreEqual(expected.decodedBodySize, actual.DecodedBodySize);
        Assert.AreEqual(expected.encodedBodySize, actual.EncodedBodySize);
    }

    [TestMethod]
    public void FileHeaderBaseTest()
    {
        var properties = default(Properties);

        var header = new FileHeaderBase();

        Validate(properties, header);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var properties = ValidProperties;
        var array = MakeByteArray(properties);
        array = array.Take(array.Length - 1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<FileHeaderBase>(array));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var properties = ValidProperties;
        var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

        var header = TestUtils.Create<FileHeaderBase>(array);

        Validate(properties, header);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestInvalidSize()
    {
        var properties = ValidProperties;
        ++properties.size;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestInvalidDecodedAllSize()
    {
        var properties = ValidProperties;
        ++properties.decodedAllSize;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestInvalidDecodedBodySize()
    {
        var properties = ValidProperties;
        ++properties.decodedBodySize;

        var header = TestUtils.Create<FileHeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
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
        CollectionAssert.AreEqual(array, stream.ToArray());
    }
}
