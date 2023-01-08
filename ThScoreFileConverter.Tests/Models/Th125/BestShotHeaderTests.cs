using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Diagnostics;
using Moq;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverter.Tests.UnitTesting;

#if NETFRAMEWORK
using ThScoreFileConverter.Core.Extensions;
#endif

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class BestShotHeaderTests
{
    internal static Mock<IBestShotHeader> MockInitialBestShotHeader()
    {
        var mock = new Mock<IBestShotHeader>();
        _ = mock.SetupGet(m => m.Signature).Returns(string.Empty);
        _ = mock.SetupGet(m => m.CardName).Returns(Enumerable.Empty<byte>());
        return mock;
    }

    internal static Mock<IBestShotHeader> MockBestShotHeader()
    {
        var mock = new Mock<IBestShotHeader>();
        _ = mock.SetupGet(m => m.Signature).Returns("BST2");
        _ = mock.SetupGet(m => m.Level).Returns(Level.Two);
        _ = mock.SetupGet(m => m.Scene).Returns(3);
        _ = mock.SetupGet(m => m.Width).Returns(4);
        _ = mock.SetupGet(m => m.Height).Returns(5);
        _ = mock.SetupGet(m => m.Width2).Returns(6);
        _ = mock.SetupGet(m => m.Height2).Returns(7);
        _ = mock.SetupGet(m => m.HalfWidth).Returns(8);
        _ = mock.SetupGet(m => m.HalfHeight).Returns(9);
        _ = mock.SetupGet(m => m.DateTime).Returns(10u);
        _ = mock.SetupGet(m => m.SlowRate).Returns(11f);
        _ = mock.SetupGet(m => m.Fields).Returns(new BonusFields(12));
        _ = mock.SetupGet(m => m.ResultScore).Returns(13);
        _ = mock.SetupGet(m => m.BasePoint).Returns(14);
        _ = mock.SetupGet(m => m.RiskBonus).Returns(15);
        _ = mock.SetupGet(m => m.BossShot).Returns(16f);
        _ = mock.SetupGet(m => m.NiceShot).Returns(17f);
        _ = mock.SetupGet(m => m.AngleBonus).Returns(18f);
        _ = mock.SetupGet(m => m.MacroBonus).Returns(19);
        _ = mock.SetupGet(m => m.FrontSideBackShot).Returns(20);
        _ = mock.SetupGet(m => m.ClearShot).Returns(21);
        _ = mock.SetupGet(m => m.Angle).Returns(22);
        _ = mock.SetupGet(m => m.ResultScore2).Returns(23);
        _ = mock.SetupGet(m => m.CardName).Returns(
            TestUtils.CP932Encoding.GetBytes("abcde").Concat(Enumerable.Repeat((byte)'\0', 75)).ToArray());
        return mock;
    }

    internal static byte[] MakeByteArray(IBestShotHeader header)
    {
        return TestUtils.MakeByteArray(
            header.Signature.ToCharArray(),
            (ushort)0,
            TestUtils.Cast<short>(header.Level + 1),
            header.Scene,
            (ushort)0,
            header.Width,
            header.Height,
            0u,
            header.Width2,
            header.Height2,
            header.HalfWidth,
            header.HalfHeight,
            header.DateTime,
            0u,
            header.SlowRate,
            header.Fields.Data,
            header.ResultScore,
            header.BasePoint,
            TestUtils.MakeRandomArray<byte>(0x8),
            header.RiskBonus,
            header.BossShot,
            header.NiceShot,
            header.AngleBonus,
            header.MacroBonus,
            header.FrontSideBackShot,
            header.ClearShot,
            TestUtils.MakeRandomArray<byte>(0x30),
            header.Angle,
            header.ResultScore2,
            0u,
            header.CardName);
    }

    internal static void Validate(IBestShotHeader expected, IBestShotHeader actual)
    {
        Guard.IsNotNull(actual);

        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.Scene, actual.Scene);
        Assert.AreEqual(expected.Width, actual.Width);
        Assert.AreEqual(expected.Height, actual.Height);
        Assert.AreEqual(expected.Width2, actual.Width2);
        Assert.AreEqual(expected.Height2, actual.Height2);
        Assert.AreEqual(expected.HalfWidth, actual.HalfWidth);
        Assert.AreEqual(expected.HalfHeight, actual.HalfHeight);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        Assert.AreEqual(expected.Fields, actual.Fields);
        Assert.AreEqual(expected.ResultScore, actual.ResultScore);
        Assert.AreEqual(expected.BasePoint, actual.BasePoint);
        Assert.AreEqual(expected.RiskBonus, actual.RiskBonus);
        Assert.AreEqual(expected.BossShot, actual.BossShot);
        Assert.AreEqual(expected.NiceShot, actual.NiceShot);
        Assert.AreEqual(expected.AngleBonus, actual.AngleBonus);
        Assert.AreEqual(expected.MacroBonus, actual.MacroBonus);
        Assert.AreEqual(expected.FrontSideBackShot, actual.FrontSideBackShot);
        Assert.AreEqual(expected.ClearShot, actual.ClearShot);
        Assert.AreEqual(expected.Angle, actual.Angle);
        Assert.AreEqual(expected.ResultScore2, actual.ResultScore2);
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
        _ = mock.SetupGet(m => m.Signature).Returns($"{signature}E");

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

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
