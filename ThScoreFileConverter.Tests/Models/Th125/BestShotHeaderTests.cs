using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Diagnostics;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class BestShotHeaderTests
{
    internal static IBestShotHeader MockInitialBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader>();
        _ = mock.Signature.Returns(string.Empty);
        _ = mock.CardName.Returns([]);
        return mock;
    }

    internal static IBestShotHeader MockBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader>();
        _ = mock.Signature.Returns("BST2");
        _ = mock.Level.Returns(Level.Two);
        _ = mock.Scene.Returns((short)3);
        _ = mock.Width.Returns((short)4);
        _ = mock.Height.Returns((short)5);
        _ = mock.Width2.Returns((short)6);
        _ = mock.Height2.Returns((short)7);
        _ = mock.HalfWidth.Returns((short)8);
        _ = mock.HalfHeight.Returns((short)9);
        _ = mock.DateTime.Returns(10u);
        _ = mock.SlowRate.Returns(11f);
        _ = mock.Fields.Returns(new BonusFields(12));
        _ = mock.ResultScore.Returns(13);
        _ = mock.BasePoint.Returns(14);
        _ = mock.RiskBonus.Returns(15);
        _ = mock.BossShot.Returns(16f);
        _ = mock.NiceShot.Returns(17f);
        _ = mock.AngleBonus.Returns(18f);
        _ = mock.MacroBonus.Returns(19);
        _ = mock.FrontSideBackShot.Returns(20);
        _ = mock.ClearShot.Returns(21);
        _ = mock.Angle.Returns(22);
        _ = mock.ResultScore2.Returns(23);
        _ = mock.CardName.Returns([.. TestUtils.CP932Encoding.GetBytes("abcde"), .. Enumerable.Repeat((byte)'\0', 75)]);
        return mock;
    }

    internal static byte[] MakeByteArray(IBestShotHeader header)
    {
        return TestUtils.MakeByteArray(
            header.Signature.ToCharArray(),
            (ushort)0,
            (short)(header.Level + 1),
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
            TestUtils.MakeRandomArray(0x8),
            header.RiskBonus,
            header.BossShot,
            header.NiceShot,
            header.AngleBonus,
            header.MacroBonus,
            header.FrontSideBackShot,
            header.ClearShot,
            TestUtils.MakeRandomArray(0x30),
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
