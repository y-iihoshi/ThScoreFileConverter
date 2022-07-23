using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

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

    internal static void Validate(in FileHeader header, in Properties properties)
    {
        Assert.AreEqual(properties.checksum, header.Checksum);
        Assert.AreEqual(properties.version, header.Version);
        Assert.AreEqual(properties.size, header.Size);
        Assert.AreEqual(properties.decodedAllSize, header.DecodedAllSize);
    }

    [TestMethod]
    public void FileHeaderTest()
    {
        var properties = default(Properties);

        var header = new FileHeader();

        Validate(header, properties);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        Validate(header, properties);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var properties = ValidProperties;
        var array = MakeByteArray(properties);
        array = array.Take(array.Length - 1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<FileHeader>(array));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var properties = ValidProperties;
        var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

        var header = TestUtils.Create<FileHeader>(array);

        Validate(header, properties);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestInvalidVersion()
    {
        var properties = ValidProperties;
        ++properties.version;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        Validate(header, properties);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestInvalidSize()
    {
        var properties = ValidProperties;
        ++properties.size;

        var header = TestUtils.Create<FileHeader>(MakeByteArray(properties));

        Validate(header, properties);
        Assert.IsFalse(header.IsValid);
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
        CollectionAssert.AreEqual(array, stream.ToArray());
    }
}
