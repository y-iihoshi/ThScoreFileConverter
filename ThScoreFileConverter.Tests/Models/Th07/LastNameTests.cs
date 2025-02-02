using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

internal static class LastNameExtensions
{
    internal static void ShouldBe(this LastName actual, LastNameTests.Properties expected)
    {
        var data = LastNameTests.MakeData(expected);

        actual.Signature.ShouldBe(expected.signature);
        actual.Size1.ShouldBe(expected.size1);
        actual.Size2.ShouldBe(expected.size2);
        actual.FirstByteOfData.ShouldBe(data[0]);
        actual.Name.ShouldBe(expected.name);
    }
}

[TestClass]
public class LastNameTests
{
    internal struct Properties
    {
        public string signature;
        public short size1;
        public short size2;
        public byte[] name;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "LSNM",
        size1 = 0x18,
        size2 = 0x18,
        name = TestUtils.MakeRandomArray(12),
    };

    internal static byte[] MakeData(in Properties properties)
    {
        return TestUtils.MakeByteArray(0u, properties.name);
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));
    }

    [TestMethod]
    public void LastNameTestChapter()
    {
        var properties = ValidProperties;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        var lastName = new LastName(chapter);

        lastName.ShouldBe(properties);
    }

    [TestMethod]
    public void LastNameTestInvalidSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature.ToLowerInvariant();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new LastName(chapter));
    }

    [TestMethod]
    public void LastNameTestInvalidSize1()
    {
        var properties = ValidProperties;
        --properties.size1;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new LastName(chapter));
    }
}
