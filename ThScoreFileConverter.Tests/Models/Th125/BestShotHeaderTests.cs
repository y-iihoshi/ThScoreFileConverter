using NSubstitute;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

internal static class BestShotHeaderExtensions
{
    internal static void ShouldBe(this IBestShotHeader actual, IBestShotHeader expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Level.ShouldBe(expected.Level);
        actual.Scene.ShouldBe(expected.Scene);
        actual.Width.ShouldBe(expected.Width);
        actual.Height.ShouldBe(expected.Height);
        actual.Width2.ShouldBe(expected.Width2);
        actual.Height2.ShouldBe(expected.Height2);
        actual.HalfWidth.ShouldBe(expected.HalfWidth);
        actual.HalfHeight.ShouldBe(expected.HalfHeight);
        actual.DateTime.ShouldBe(expected.DateTime);
        actual.SlowRate.ShouldBe(expected.SlowRate);
        actual.Fields.ShouldBe(expected.Fields);
        actual.ResultScore.ShouldBe(expected.ResultScore);
        actual.BasePoint.ShouldBe(expected.BasePoint);
        actual.RiskBonus.ShouldBe(expected.RiskBonus);
        actual.BossShot.ShouldBe(expected.BossShot);
        actual.NiceShot.ShouldBe(expected.NiceShot);
        actual.AngleBonus.ShouldBe(expected.AngleBonus);
        actual.MacroBonus.ShouldBe(expected.MacroBonus);
        actual.FrontSideBackShot.ShouldBe(expected.FrontSideBackShot);
        actual.ClearShot.ShouldBe(expected.ClearShot);
        actual.Angle.ShouldBe(expected.Angle);
        actual.ResultScore2.ShouldBe(expected.ResultScore2);
        actual.CardName.ShouldBe(expected.CardName);
    }
}

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

    [TestMethod]
    public void BestShotHeaderTest()
    {
        var mock = MockInitialBestShotHeader();
        var header = new BestShotHeader();

        header.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockBestShotHeader();
        var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock));

        header.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTestEmptySignature()
    {
        var mock = MockBestShotHeader();
        _ = mock.Signature.Returns(string.Empty);

        _ = Should.Throw<InvalidDataException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedSignature()
    {
        var mock = MockBestShotHeader();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature[0..^1]);

        _ = Should.Throw<InvalidDataException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededSignature()
    {
        var mock = MockBestShotHeader();
        var signature = mock.Signature;
        _ = mock.Signature.Returns($"{signature}E");

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [TestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ReadFromTestInvalidLevel(int level)
    {
        var mock = MockBestShotHeader();
        _ = mock.Level.Returns((Level)level);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestShortenedCardName()
    {
        var mock = MockBestShotHeader();
        var cardName = mock.CardName;
        _ = mock.CardName.Returns([.. cardName.SkipLast(1)]);

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededCardName()
    {
        var mock = MockBestShotHeader();
        var cardName = mock.CardName;
        _ = mock.CardName.Returns([.. cardName, .. TestUtils.MakeRandomArray(1)]);

        var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock));

        header.ShouldBe(MockBestShotHeader());
    }
}
