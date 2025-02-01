using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.Models.Th06.Wrappers;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class ChapterTests
{
    internal struct Properties
    {
        public string signature;
        public short size1;
        public short size2;
        public byte[] data;
    }

    internal static Properties DefaultProperties { get; } = new Properties()
    {
        signature = string.Empty,
        size1 = default,
        size2 = default,
        data = [],
    };

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "ABCD",
        size1 = 12,
        size2 = 34,
        data = [0x56, 0x78, 0x9A, 0xBC],
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(),
            properties.size1,
            properties.size2,
            properties.data);
    }

    internal static void Validate(in Properties expected, IChapter actual)
    {
        actual.Signature.ShouldBe(expected.signature);
        actual.Size1.ShouldBe(expected.size1);
        actual.Size2.ShouldBe(expected.size2);
        actual.FirstByteOfData.ShouldBe(expected.data?.Length > 0 ? expected.data[0] : default);
    }

    internal static void Validate(in Properties expected, in ChapterWrapper actual)
    {
        Validate(expected, actual as IChapter);
        actual.Data.ShouldBe(expected.data);
    }

    [TestMethod]
    public void ChapterTest()
    {
        var chapter = new ChapterWrapper();

        Validate(DefaultProperties, chapter);
    }

    [TestMethod]
    public void ChapterTestCopy()
    {
        var chapter1 = new Chapter();
        var chapter2 = new ChapterWrapper(chapter1);

        Validate(DefaultProperties, chapter2);
    }

    [TestMethod]
    public void ChapterTestCopyWithExpected()
    {
        var chapter1 = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        var chapter2 = new ChapterWrapper(chapter1, chapter1.Signature, chapter1.Size1);

        Validate(ValidProperties, chapter2);
    }

    [TestMethod]
    public void ChapterTestInvalidSignature()
    {
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        _ = Should.Throw<InvalidDataException>(
            () => new ChapterWrapper(chapter, chapter.Signature.ToLowerInvariant(), chapter.Size1));
    }

    [TestMethod]
    public void ChapterTestInvalidSize()
    {
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        _ = Should.Throw<InvalidDataException>(
            () => new ChapterWrapper(chapter, chapter.Signature, (short)(chapter.Size1 - 1)));
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(ValidProperties));

        Validate(ValidProperties, chapter);
    }

    [TestMethod]
    public void ReadFromTestEmptySignature()
    {
        var properties = ValidProperties;
        properties.signature = string.Empty;

        // <-- sig --> size1 size2 <- data -->
        // __ __ __ __ 0c 00 22 00 56 78 9a bc
        //             <-- sig --> size1 size2 <dat>

        // The actual value of the Size1 property becomes too large and
        // the Data property becomes empty,
        // so EndOfStreamException will be thrown.
        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature[0..^1];

        // <-- sig --> size1 size2 <- data -->
        // __ 41 42 43 0c 00 22 00 56 78 9a bc
        //    <-- sig --> size1 size2 < dat ->

        // The actual value of the Size1 property becomes too large,
        // so EndOfStreamException will be thrown.
        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestExceededSignature()
    {
        var properties = ValidProperties;
        properties.signature += "E";

        // <--- sig ----> size1 size2 <- data -->
        // 41 42 43 44 45 0c 00 22 00 56 78 9a bc
        // <-- sig --> size1 size2 <---- data ---->

        // The actual value of the Size1 property becomes too large,
        // so EndOfStreamException will be thrown.
        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestNegativeSize1()
    {
        var properties = ValidProperties;
        properties.size1 = -1;

        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestZeroSize1()
    {
        var properties = ValidProperties;
        properties.size1 = 0;

        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSize1()
    {
        var properties = ValidProperties;
        --properties.size1;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        Validate(properties, chapter as IChapter);
        chapter.Data.ShouldNotBe(properties.data);
    }

    [TestMethod]
    public void ReadFromTestExceededSize1()
    {
        var properties = ValidProperties;
        ++properties.size1;

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestNegativeSize2()
    {
        var properties = ValidProperties;
        properties.size2 = -1;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        Validate(properties, chapter);
    }

    [TestMethod]
    public void ReadFromTestZeroSize2()
    {
        var properties = ValidProperties;
        properties.size2 = 0;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        Validate(properties, chapter);
    }

    [TestMethod]
    public void ReadFromTestShortenedSize2()
    {
        var properties = ValidProperties;
        --properties.size2;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        Validate(properties, chapter);
    }

    [TestMethod]
    public void ReadFromTestExceededSize2()
    {
        var properties = ValidProperties;
        ++properties.size2;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        Validate(properties, chapter);
    }

    [TestMethod]
    public void ReadFromTestEmptyData()
    {
        var properties = ValidProperties;
        properties.data = [];

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestMisalignedData()
    {
        var properties = ValidProperties;
        --properties.size1;
        properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        Validate(properties, chapter);
    }
}
