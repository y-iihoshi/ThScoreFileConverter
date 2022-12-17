using System.IO;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class HeaderBaseTests
{
    internal struct Properties
    {
        public string signature;
        public int encodedAllSize;
        public int encodedBodySize;
        public int decodedBodySize;
    }

    internal static Properties DefaultProperties { get; } = new Properties()
    {
        signature = string.Empty,
        encodedAllSize = default,
        encodedBodySize = default,
        decodedBodySize = default,
    };

    internal static Properties ValidProperties { get; } = MakeProperties("abcd");

    internal static Properties MakeProperties(string signature)
    {
        return new()
        {
            signature = signature,
            encodedAllSize = 36,
            encodedBodySize = 12,
            decodedBodySize = 56,
        };
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.signature.ToCharArray(),
            properties.encodedAllSize,
            0u,
            0u,
            properties.encodedBodySize,
            properties.decodedBodySize);
    }

    internal static void Validate(in Properties expected, in HeaderBase actual)
    {
        Assert.AreEqual(expected.signature, actual.Signature);
        Assert.AreEqual(expected.encodedAllSize, actual.EncodedAllSize);
        Assert.AreEqual(expected.encodedBodySize, actual.EncodedBodySize);
        Assert.AreEqual(expected.decodedBodySize, actual.DecodedBodySize);
    }

    [TestMethod]
    public void HeaderTest()
    {
        var header = new HeaderBase();

        Validate(DefaultProperties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestEmptySignature()
    {
        var properties = MakeProperties(string.Empty);

        // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
        // __ __ __ __ 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
        //             <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody >

        // The actual value of the DecodedBodySize property can not be read.
        // so EndOfStreamException will be thrown.
        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<HeaderBase>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSignature()
    {
#if NETFRAMEWORK
        var properties = MakeProperties(ValidProperties.signature.Substring(0, 3));
#else
        var properties = MakeProperties(ValidProperties.signature[0..3]);
#endif

        // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
        // __ xx xx xx 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
        //    <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

        // The actual value of the DecodedBodySize property can not be read.
        // so EndOfStreamException will be thrown.
        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<HeaderBase>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestExceededSignature()
    {
        var properties = MakeProperties(ValidProperties.signature + "e");

        // <--- sig ----> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
        // xx xx xx xx 65 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
        // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Assert.AreEqual(ValidProperties.signature, header.Signature);
        Assert.AreNotEqual(properties.encodedAllSize, header.EncodedAllSize);
        Assert.AreNotEqual(properties.encodedBodySize, header.EncodedBodySize);
        Assert.AreNotEqual(properties.decodedBodySize, header.DecodedBodySize);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestNegativeEncodedAllSize()
    {
        var properties = ValidProperties;
        properties.encodedAllSize = -1;

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<HeaderBase>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestZeroEncodedAllSize()
    {
        var properties = ValidProperties;
        properties.encodedAllSize = 0;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestShortenedEncodedAllSize()
    {
        var properties = ValidProperties;
        --properties.encodedAllSize;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestExceededEncodedAllSize()
    {
        var properties = ValidProperties;
        ++properties.encodedAllSize;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestNegativeEncodedBodySize()
    {
        var properties = ValidProperties;
        properties.encodedBodySize = -1;

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<HeaderBase>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestZeroEncodedBodySize()
    {
        var properties = ValidProperties;
        properties.encodedBodySize = 0;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestShortenedEncodedBodySize()
    {
        var properties = ValidProperties;
        --properties.encodedBodySize;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestExceededEncodedBodySize()
    {
        var properties = ValidProperties;
        ++properties.encodedBodySize;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void ReadFromTestNegativeDecodedBodySize()
    {
        var properties = ValidProperties;
        properties.decodedBodySize = -1;

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<HeaderBase>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestZeroDecodedBodySize()
    {
        var properties = ValidProperties;
        properties.decodedBodySize = 0;

        var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

        Validate(properties, header);
        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void WriteToTest()
    {
        var properties = ValidProperties;
        var byteArray = MakeByteArray(properties);

        var header = TestUtils.Create<HeaderBase>(byteArray);

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        header.WriteTo(writer);

        writer.Flush();
        CollectionAssert.AreEqual(byteArray, stream.ToArray());
    }
}
