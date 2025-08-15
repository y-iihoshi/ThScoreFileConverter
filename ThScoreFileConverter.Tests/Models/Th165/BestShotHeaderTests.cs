using NSubstitute;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

internal static class BestShotHeaderExtensions
{
    internal static void ShouldBe(this IBestShotHeader actual, IBestShotHeader expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Weekday.ShouldBe(expected.Weekday);
        actual.Dream.ShouldBe(expected.Dream);
        actual.Width.ShouldBe(expected.Width);
        actual.Height.ShouldBe(expected.Height);
        actual.Width2.ShouldBe(expected.Width2);
        actual.Height2.ShouldBe(expected.Height2);
        actual.HalfWidth.ShouldBe(expected.HalfWidth);
        actual.HalfHeight.ShouldBe(expected.HalfHeight);
        actual.SlowRate.ShouldBe(expected.SlowRate);
        actual.DateTime.ShouldBe(expected.DateTime);
        actual.Angle.ShouldBe(expected.Angle);
        actual.Score.ShouldBe(expected.Score);
        actual.Fields.Data.ShouldBe(expected.Fields.Data);
        actual.Score2.ShouldBe(expected.Score2);
        actual.BasePoint.ShouldBe(expected.BasePoint);
        actual.NumViewed.ShouldBe(expected.NumViewed);
        actual.NumLikes.ShouldBe(expected.NumLikes);
        actual.NumFavs.ShouldBe(expected.NumFavs);
        actual.NumBullets.ShouldBe(expected.NumBullets);
        actual.NumBulletsNearby.ShouldBe(expected.NumBulletsNearby);
        actual.RiskBonus.ShouldBe(expected.RiskBonus);
        actual.BossShot.ShouldBe(expected.BossShot);
        actual.AngleBonus.ShouldBe(expected.AngleBonus);
        actual.MacroBonus.ShouldBe(expected.MacroBonus);
        actual.LikesPerView.ShouldBe(expected.LikesPerView);
        actual.FavsPerView.ShouldBe(expected.FavsPerView);
        actual.NumHashtags.ShouldBe(expected.NumHashtags);
        actual.NumRedBullets.ShouldBe(expected.NumRedBullets);
        actual.NumPurpleBullets.ShouldBe(expected.NumPurpleBullets);
        actual.NumBlueBullets.ShouldBe(expected.NumBlueBullets);
        actual.NumCyanBullets.ShouldBe(expected.NumCyanBullets);
        actual.NumGreenBullets.ShouldBe(expected.NumGreenBullets);
        actual.NumYellowBullets.ShouldBe(expected.NumYellowBullets);
        actual.NumOrangeBullets.ShouldBe(expected.NumOrangeBullets);
        actual.NumLightBullets.ShouldBe(expected.NumLightBullets);
    }
}

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
        _ = mock.Signature.Returns("BST4");
        _ = mock.Weekday.Returns(Day.Monday);
        _ = mock.Dream.Returns((short)3);
        _ = mock.Width.Returns((short)4);
        _ = mock.Height.Returns((short)5);
        _ = mock.Width2.Returns((short)6);
        _ = mock.Height2.Returns((short)7);
        _ = mock.HalfWidth.Returns((short)8);
        _ = mock.HalfHeight.Returns((short)9);
        _ = mock.SlowRate.Returns(10f);
        _ = mock.DateTime.Returns(11u);
        _ = mock.Angle.Returns(12f);
        _ = mock.Score.Returns(13);
        _ = mock.Fields.Returns(new HashtagFields(14, 15, 16));
        _ = mock.Score2.Returns(17);
        _ = mock.BasePoint.Returns(18);
        _ = mock.NumViewed.Returns(19);
        _ = mock.NumLikes.Returns(20);
        _ = mock.NumFavs.Returns(21);
        _ = mock.NumBullets.Returns(22);
        _ = mock.NumBulletsNearby.Returns(23);
        _ = mock.RiskBonus.Returns(24);
        _ = mock.BossShot.Returns(25);
        _ = mock.AngleBonus.Returns(26);
        _ = mock.MacroBonus.Returns(27);
        _ = mock.LikesPerView.Returns(28);
        _ = mock.FavsPerView.Returns(29);
        _ = mock.NumHashtags.Returns(30);
        _ = mock.NumRedBullets.Returns(31);
        _ = mock.NumPurpleBullets.Returns(32);
        _ = mock.NumBlueBullets.Returns(33);
        _ = mock.NumCyanBullets.Returns(34);
        _ = mock.NumGreenBullets.Returns(35);
        _ = mock.NumYellowBullets.Returns(36);
        _ = mock.NumOrangeBullets.Returns(37);
        _ = mock.NumLightBullets.Returns(38);
        return mock;
    }

    internal static byte[] MakeByteArray(IBestShotHeader header)
    {
        return TestUtils.MakeByteArray(
            header.Signature.ToCharArray(),
            (ushort)0,
            (short)header.Weekday,
            (short)(header.Dream - 1),
            (ushort)0,
            header.Width,
            header.Height,
            0u,
            header.Width2,
            header.Height2,
            header.HalfWidth,
            header.HalfHeight,
            0u,
            header.SlowRate,
            header.DateTime,
            0u,
            header.Angle,
            header.Score,
            0u,
            header.Fields.Data,
            TestUtils.MakeRandomArray(0x28),
            header.Score2,
            header.BasePoint,
            header.NumViewed,
            header.NumLikes,
            header.NumFavs,
            header.NumBullets,
            header.NumBulletsNearby,
            header.RiskBonus,
            header.BossShot,
            0u,
            header.AngleBonus,
            header.MacroBonus,
            0u,
            0u,
            header.LikesPerView,
            header.FavsPerView,
            header.NumHashtags,
            header.NumRedBullets,
            header.NumPurpleBullets,
            header.NumBlueBullets,
            header.NumCyanBullets,
            header.NumGreenBullets,
            header.NumYellowBullets,
            header.NumOrangeBullets,
            header.NumLightBullets,
            TestUtils.MakeRandomArray(0x78));
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

    public static IEnumerable<object[]> InvalidDays => TestUtils.GetInvalidEnumerators<Day>();

    [TestMethod]
    [DynamicData(nameof(InvalidDays))]
    public void ReadFromTestInvalidDay(int day)
    {
        var mock = MockBestShotHeader();
        _ = mock.Weekday.Returns((Day)day);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }
}
