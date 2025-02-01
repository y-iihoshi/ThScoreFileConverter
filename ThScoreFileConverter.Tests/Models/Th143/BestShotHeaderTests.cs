using CommunityToolkit.Diagnostics;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class BestShotHeaderTests
{
    internal static IBestShotHeader MockInitialBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader>();
        _ = mock.Signature.Returns(string.Empty);
        return mock;
    }

    internal static IBestShotHeader MockBestShotHeader()
    {
        var mock = Substitute.For<IBestShotHeader>();
        _ = mock.Signature.Returns("BST3");
        _ = mock.Day.Returns(Day.Second);
        _ = mock.Scene.Returns((short)3);
        _ = mock.Width.Returns((short)4);
        _ = mock.Height.Returns((short)5);
        _ = mock.DateTime.Returns(6u);
        _ = mock.SlowRate.Returns(7);
        return mock;
    }

    internal static byte[] MakeByteArray(IBestShotHeader header)
    {
        return TestUtils.MakeByteArray(
            header.Signature.ToCharArray(),
            (ushort)0,
            (short)header.Day,
            (short)(header.Scene - 1),
            (ushort)0,
            header.Width,
            header.Height,
            0u,
            header.DateTime,
            header.SlowRate,
            TestUtils.MakeRandomArray(0x58));
    }

    internal static void Validate(IBestShotHeader expected, IBestShotHeader actual)
    {
        Guard.IsNotNull(actual);

        actual.Signature.ShouldBe(expected.Signature);
        actual.Day.ShouldBe(expected.Day);
        actual.Scene.ShouldBe(expected.Scene);
        actual.Width.ShouldBe(expected.Width);
        actual.Height.ShouldBe(expected.Height);
        actual.DateTime.ShouldBe(expected.DateTime);
        actual.SlowRate.ShouldBe(expected.SlowRate);
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
        var mock = Substitute.For<IBestShotHeader>();
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

    public static IEnumerable<object[]> InvalidDays => TestUtils.GetInvalidEnumerators<Day>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidDays))]
    public void ReadFromTestInvalidDay(int day)
    {
        var mock = MockBestShotHeader();
        _ = mock.Day.Returns((Day)day);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }
}
