using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.Models.Th06.Wrappers;

namespace ThScoreFileConverter.Tests.Models.Th06;

internal static class ChapterExtensions
{
    internal static void ShouldBe(this IChapter chapter, in ChapterTests.Properties expected)
    {
        chapter.Signature.ShouldBe(expected.signature);
        chapter.Size1.ShouldBe(expected.size1);
        chapter.Size2.ShouldBe(expected.size2);
        chapter.FirstByteOfData.ShouldBe(expected.data?.Length > 0 ? expected.data[0] : default);
    }

    internal static void ShouldBe(this ChapterWrapper chapter, in ChapterTests.Properties expected)
    {
        (chapter as IChapter).ShouldBe(expected);
        chapter.Data.ShouldBe(expected.data);
    }
}

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

    [TestMethod]
    public void ChapterTest()
    {
        var chapter = new ChapterWrapper();

        chapter.ShouldBe(DefaultProperties);
    }

    [TestMethod]
    public void ChapterTestCopy()
    {
        var chapter1 = new Chapter();
        var chapter2 = new ChapterWrapper(chapter1);

        chapter2.ShouldBe(DefaultProperties);
    }

    [TestMethod]
    public void ChapterTestCopyWithExpected()
    {
        var chapter1 = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        var chapter2 = new ChapterWrapper(chapter1, chapter1.Signature, chapter1.Size1);

        chapter2.ShouldBe(ValidProperties);
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

        chapter.ShouldBe(ValidProperties);
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

        (chapter as IChapter).ShouldBe(properties);
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

        chapter.ShouldBe(properties);
    }

    [TestMethod]
    public void ReadFromTestZeroSize2()
    {
        var properties = ValidProperties;
        properties.size2 = 0;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        chapter.ShouldBe(properties);
    }

    [TestMethod]
    public void ReadFromTestShortenedSize2()
    {
        var properties = ValidProperties;
        --properties.size2;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        chapter.ShouldBe(properties);
    }

    [TestMethod]
    public void ReadFromTestExceededSize2()
    {
        var properties = ValidProperties;
        ++properties.size2;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        chapter.ShouldBe(properties);
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
        properties.data = properties.data[..^1];

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        chapter.ShouldBe(properties);
    }
}
