using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class BestShotHeaderTests
{
    internal static Mock<IBestShotHeader<Level>> MockInitialBestShotHeader()
    {
        var mock = new Mock<IBestShotHeader<Level>>();
        _ = mock.SetupGet(m => m.Signature).Returns(string.Empty);
        _ = mock.SetupGet(m => m.CardName).Returns(Enumerable.Empty<byte>());
        return mock;
    }

    internal static Mock<IBestShotHeader<Level>> MockBestShotHeader()
    {
        var mock = new Mock<IBestShotHeader<Level>>();
        _ = mock.SetupGet(m => m.Signature).Returns("BSTS");
        _ = mock.SetupGet(m => m.Level).Returns(Level.Two);
        _ = mock.SetupGet(m => m.Scene).Returns(3);
        _ = mock.SetupGet(m => m.Width).Returns(4);
        _ = mock.SetupGet(m => m.Height).Returns(5);
        _ = mock.SetupGet(m => m.ResultScore).Returns(6);
        _ = mock.SetupGet(m => m.SlowRate).Returns(7f);
        _ = mock.SetupGet(m => m.CardName).Returns(
            TestUtils.CP932Encoding.GetBytes("abcde").Concat(Enumerable.Repeat((byte)'\0', 75)).ToArray());
        return mock;
    }

    internal static byte[] MakeByteArray(IBestShotHeader<Level> header)
    {
        return TestUtils.MakeByteArray(
            header.Signature.ToCharArray(),
            (ushort)0,
            TestUtils.Cast<short>(header.Level + 1),
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
        if (actual is null)
            throw new ArgumentNullException(nameof(actual));

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

        Validate(mock.Object, header);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockBestShotHeader();
        var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object));

        Validate(mock.Object, header);
    }

    [TestMethod]
    public void ReadFromTestEmptySignature()
    {
        var mock = MockBestShotHeader();
        _ = mock.SetupGet(m => m.Signature).Returns(string.Empty);

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSignature()
    {
        var mock = MockBestShotHeader();
        var signature = mock.Object.Signature;
#if NETFRAMEWORK
        _ = mock.SetupGet(m => m.Signature).Returns(signature.Substring(0, signature.Length - 1));
#else
        _ = mock.SetupGet(m => m.Signature).Returns(signature[0..^1]);
#endif

        _ = Assert.ThrowsException<InvalidDataException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestExceededSignature()
    {
        var mock = MockBestShotHeader();
        var signature = mock.Object.Signature;
        _ = mock.SetupGet(m => m.Signature).Returns(signature + "E");

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }

    public static IEnumerable<object[]> InvalidLevels
        => TestUtils.GetInvalidEnumerators(typeof(Level));

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ReadFromTestInvalidLevel(int level)
    {
        var mock = MockBestShotHeader();
        _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<Level>(level));

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestShortenedCardName()
    {
        var mock = MockBestShotHeader();
        var cardName = mock.Object.CardName;
        _ = mock.SetupGet(m => m.CardName).Returns(cardName.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestExceededCardName()
    {
        var mock = MockBestShotHeader();
        var cardName = mock.Object.CardName;
        _ = mock.SetupGet(m => m.CardName).Returns(cardName.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

        var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object));

        Validate(MockBestShotHeader().Object, header);
    }
}
