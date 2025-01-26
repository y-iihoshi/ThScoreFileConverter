using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class VersionInfoTests
{
    internal struct Properties
    {
        public string signature;
        public short size1;
        public short size2;
        public byte[] version;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "VRSM",
        size1 = 0x1C,
        size2 = 0x1C,
        version = TestUtils.MakeRandomArray(6),
    };

    internal static byte[] MakeData(in Properties properties)
    {
        return TestUtils.MakeByteArray(0u, properties.version, new byte[3], new byte[3], 0u);
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));
    }

    internal static void Validate(in Properties expected, in VersionInfo actual)
    {
        var data = MakeData(expected);

        Assert.AreEqual(expected.signature, actual.Signature);
        Assert.AreEqual(expected.size1, actual.Size1);
        Assert.AreEqual(expected.size2, actual.Size2);
        Assert.AreEqual(data[0], actual.FirstByteOfData);
        CollectionAssert.That.AreEqual(expected.version, actual.Version);
    }

    [TestMethod]
    public void VersionInfoTestChapter()
    {
        var properties = ValidProperties;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        var versionInfo = new VersionInfo(chapter);

        Validate(properties, versionInfo);
    }

    [TestMethod]
    public void VersionInfoTestInvalidSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature.ToLowerInvariant();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Assert.ThrowsException<InvalidDataException>(() => new VersionInfo(chapter));
    }

    [TestMethod]
    public void VersionInfoTestInvalidSize1()
    {
        var properties = ValidProperties;
        --properties.size1;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Assert.ThrowsException<InvalidDataException>(() => new VersionInfo(chapter));
    }
}
