using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class HeaderTests
{
    internal struct Properties
    {
        public string signature;
        public short size1;
        public short size2;
        public byte[] data;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "TH7K",
        size1 = 12,
        size2 = 12,
        data = [0x10, 0x00, 0x00, 0x00],
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(), properties.size1, properties.size2, properties.data);
    }

    internal static void Validate(in Properties expected, in Header actual)
    {
        actual.Signature.ShouldBe(expected.signature);
        actual.Size1.ShouldBe(expected.size1);
        actual.Size2.ShouldBe(expected.size2);
        actual.FirstByteOfData.ShouldBe(expected.data[0]);
    }

    [TestMethod]
    public void HeaderTest()
    {
        var properties = ValidProperties;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        var header = new Header(chapter);

        Validate(properties, header);
    }

    [TestMethod]
    public void HeaderTestInvalidSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature.ToLowerInvariant();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new Header(chapter));
    }

    [TestMethod]
    public void HeaderTestInvalidSize1()
    {
        var properties = ValidProperties;
        ++properties.size1;
        properties.data = [.. properties.data, .. new byte[] { default }];

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new Header(chapter));
    }
}
