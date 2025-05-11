using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

internal static class BestShotHeaderExtensions
{
    internal static void ShouldBe(this IBestShotHeader<Level> actual, IBestShotHeader<Level> expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Level.ShouldBe(expected.Level);
        actual.Scene.ShouldBe(expected.Scene);
        actual.Width.ShouldBe(expected.Width);
        actual.Height.ShouldBe(expected.Height);
        actual.ResultScore.ShouldBe(expected.ResultScore);
        actual.SlowRate.ShouldBe(expected.SlowRate);
        actual.CardName.ShouldBe(expected.CardName);
    }
}

[TestClass]
public class BestShotHeaderTests
{
    internal static IBestShotHeader<Level> MockInitialBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader<Level>>();
        _ = mock.Signature.Returns(string.Empty);
        _ = mock.CardName.Returns([]);
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
        _ = mock.CardName.Returns([.. TestUtils.CP932Encoding.GetBytes("abcde"), .. Enumerable.Repeat((byte)'\0', 75)]);
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

    [DataTestMethod]
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
