using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverter.Tests.Models.Th10.Wrappers;
using IChapter = ThScoreFileConverter.Models.Th095.IChapter;

namespace ThScoreFileConverter.Tests.Models.Th10;

internal static class ChapterExtensions
{
    internal static void ShouldBe(this IChapter actual, ChapterTests.Properties expected)
    {
        actual.Signature.ShouldBe(expected.signature);
        actual.Version.ShouldBe(expected.version);
        actual.Checksum.ShouldBe(expected.checksum);
        actual.Size.ShouldBe(expected.size);
    }

    internal static void ShouldBe(this ChapterWrapper actual, ChapterTests.Properties expected)
    {
        (actual as IChapter).ShouldBe(expected);
        actual.Data.ShouldBe(expected.data);
    }
}

[TestClass]
public class ChapterTests
{
    internal struct Properties
    {
        public string signature;
        public ushort version;
        public uint checksum;
        public int size;
        public byte[] data;
    }

    internal static Properties DefaultProperties { get; } = new Properties()
    {
        signature = string.Empty,
        version = default,
        checksum = default,
        size = default,
        data = [],
    };

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "AB",
        version = 1234,
        checksum = 0xE0u,
        size = 16,
        data = [0x01, 0x23, 0x45, 0x67],
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(),
            properties.version,
            properties.checksum,
            properties.size,
            properties.data);
    }

    [TestMethod]
    public void ChapterTest()
    {
        var chapter = new ChapterWrapper();

        chapter.ShouldBe(DefaultProperties);
        chapter.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ChapterTestCopy()
    {
        var chapter1 = new Chapter();
        var chapter2 = new ChapterWrapper(chapter1);

        chapter2.ShouldBe(DefaultProperties);
        chapter2.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ChapterTestCopyWithExpected()
    {
        var chapter1 = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        var chapter2 = new ChapterWrapper(chapter1, chapter1.Signature, chapter1.Version, chapter1.Size);

        chapter2.ShouldBe(ValidProperties);
        chapter2.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ChapterTestInvalidSignature()
    {
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        _ = Should.Throw<InvalidDataException>(
            () => new ChapterWrapper(chapter, chapter.Signature.ToLowerInvariant(), chapter.Version, chapter.Size));
    }

    [TestMethod]
    public void ChapterTestInvalidVersion()
    {
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        _ = Should.Throw<InvalidDataException>(
            () => new ChapterWrapper(chapter, chapter.Signature, (ushort)(chapter.Version - 1), chapter.Size));
    }

    [TestMethod]
    public void ChapterTestInvalidSize()
    {
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
        _ = Should.Throw<InvalidDataException>(
            () => new ChapterWrapper(chapter, chapter.Signature, chapter.Version, chapter.Size - 1));
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(ValidProperties));

        chapter.ShouldBe(ValidProperties);
        chapter.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestEmptySignature()
    {
        var properties = ValidProperties;
        properties.signature = string.Empty;

        // <sig> <ver> < chksum -> <- size --> <- data -->
        // __ __ d2 04 E0 00 00 00 10 00 00 00 01 23 45 67
        //       <sig> <ver> < chksum -> <- size --> <dat>

        // The actual value of the Size property becomes too large,
        // so EndOfStreamException will be thrown.
        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature[0..^1];

        // <sig> <ver> < chksum -> <- size --> <- data -->
        // __ 41 d2 04 E0 00 00 00 10 00 00 00 01 23 45 67
        //    <sig> <ver> < chksum -> <- size --> < data >

        // The actual value of the Size property becomes too large,
        // so EndOfStreamException will be thrown.
        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestExceededSignature()
    {
        var properties = ValidProperties;
        properties.signature += "C";

        // < sig -> <ver> < chksum -> <- size --> <- data -->
        // 41 42 43 d2 04 E0 00 00 00 10 00 00 00 01 23 45 67
        // <sig> <ver> < chksum -> <- size --> <---- data ---->

        // The actual value of the Size property becomes large,
        // so EndOfStreamException will be thrown.
        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestNegativeSize()
    {
        var properties = ValidProperties;
        properties.size = -1;

        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestZeroSize()
    {
        var properties = ValidProperties;
        properties.size = 0;

        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSize()
    {
        var properties = ValidProperties;
        --properties.size;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        (chapter as IChapter).ShouldBe(properties);
        chapter.Data.ShouldNotBe(properties.data);
        chapter.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTestExceededSize()
    {
        var properties = ValidProperties;
        ++properties.size;

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<Chapter>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestInvalidChecksum()
    {
        var properties = ValidProperties;
        --properties.checksum;

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        chapter.ShouldBe(properties);
        chapter.IsValid.ShouldBeFalse();
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
        --properties.size;
        properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

        var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

        chapter.ShouldBe(properties);
        chapter.IsValid.ShouldBeFalse();
    }
}
