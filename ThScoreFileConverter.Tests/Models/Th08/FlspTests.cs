﻿using ThScoreFileConverter.Models.Th08;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th08;

internal static class FlspExtensions
{
    internal static void ShouldBe(this FLSP actual, FlspTests.Properties expected)
    {
        var data = FlspTests.MakeData(expected);

        actual.Signature.ShouldBe(expected.signature);
        actual.Size1.ShouldBe(expected.size1);
        actual.Size2.ShouldBe(expected.size2);
        actual.FirstByteOfData.ShouldBe(data[0]);
    }
}

[TestClass]
public class FlspTests
{
    internal struct Properties
    {
        public string signature;
        public short size1;
        public short size2;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        signature = "FLSP",
        size1 = 0x20,
        size2 = 0x20,
    };

    internal static byte[] MakeData(in Properties _)
    {
        return TestUtils.MakeByteArray(new byte[0x18]);
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));
    }

    [TestMethod]
    public void FlspTestChapter()
    {
        var properties = ValidProperties;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        var flsp = new FLSP(chapter);

        flsp.ShouldBe(properties);
    }

    [TestMethod]
    public void FlspTestInvalidSignature()
    {
        var properties = ValidProperties;
        properties.signature = properties.signature.ToLowerInvariant();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new FLSP(chapter));
    }

    [TestMethod]
    public void FlspTestInvalidSize1()
    {
        var properties = ValidProperties;
        --properties.size1;

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
        _ = Should.Throw<InvalidDataException>(() => new FLSP(chapter));
    }
}
