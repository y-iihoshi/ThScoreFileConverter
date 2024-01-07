using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Diagnostics;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class BestShotHeaderTests
{
    internal static IBestShotHeader<Level> MockInitialBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader<Level>>();
        _ = mock.Signature.Returns(string.Empty);
        _ = mock.CardName.Returns(Enumerable.Empty<byte>());
        return mock;
    }

    internal static IBestShotHeader<Level> MockBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader<Level>>();
        _ = mock.Signature.Returns("BSTS");
        _ = mock.Level.Returns(Level.Two);
        _ = mock.Scene.Returns((byte)3);
        _ = mock.Width.Returns((short)4);
        _ = mock.Height.Returns((short)5);
        _ = mock.ResultScore.Returns(6);
        _ = mock.SlowRate.Returns(7f);
        _ = mock.CardName.Returns(
            TestUtils.CP932Encoding.GetBytes("abcde").Concat(Enumerable.Repeat((byte)'\0', 75)).ToArray());
        return mock;
    }

    internal static byte[] MakeByteArray(IBestShotHeader<Level> header)
    {
        return TestUtils.MakeByteArray(
            header.Signature.ToCharArray(),
            (ushort)0,
            (short)(header.Level + 1),
            header.Scene,
            (ushort)0,
            header.Width,
            header.Height,
            header.ResultScore,
            header.SlowRate,
            header.CardName);
    }

    internal static void Validate(IBestShotHeader<Level> expected, in IBestShotHeader<Level> actual)
    {
        Guard.IsNotNull(actual);

        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.Scene, actual.Scene);
        Assert.AreEqual(expected.Width, actual.Width);
        Assert.AreEqual(expected.Height, actual.Height);
        Assert.AreEqual(expected.ResultScore, actual.ResultScore);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
    }

    [TestMethod]
    public void BestShotHeaderTest()
    {
        var mock = MockInitialBestShotHeader();
        var header = new BestShotHeader();

        Validate(mock, header);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockBestShotHeader();
        var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock));

        Validate(mock, header);
    }

    [TestMethod]
    public void ReadFromTestEmptySignature()
    {
        var mock = MockBestShotHeader();
        _ = mock.Signature.Returns(string.Empty);

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSignature()
    {
        var mock = MockBestShotHeader();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature[0..^1]);

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededSignature()
    {
        var mock = MockBestShotHeader();
        var signature = mock.Signature;
        _ = mock.Signature.Returns($"{signature}E");

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ReadFromTestInvalidLevel(int level)
    {
        var mock = MockBestShotHeader();
        _ = mock.Level.Returns((Level)level);

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedCardName()
    {
        var mock = MockBestShotHeader();
        var cardName = mock.CardName;
        _ = mock.CardName.Returns(cardName.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededCardName()
    {
        var mock = MockBestShotHeader();
        var cardName = mock.CardName;
        _ = mock.CardName.Returns(cardName.Concat(TestUtils.MakeRandomArray(1)).ToArray());

        var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock));

        Validate(MockBestShotHeader(), header);
    }
}
