using System;
using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Diagnostics;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th165;

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

    internal static void Validate(IBestShotHeader expected, IBestShotHeader actual)
    {
        Guard.IsNotNull(actual);

        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Weekday, actual.Weekday);
        Assert.AreEqual(expected.Dream, actual.Dream);
        Assert.AreEqual(expected.Width, actual.Width);
        Assert.AreEqual(expected.Height, actual.Height);
        Assert.AreEqual(expected.Width2, actual.Width2);
        Assert.AreEqual(expected.Height2, actual.Height2);
        Assert.AreEqual(expected.HalfWidth, actual.HalfWidth);
        Assert.AreEqual(expected.HalfHeight, actual.HalfHeight);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.Angle, actual.Angle);
        Assert.AreEqual(expected.Score, actual.Score);
        CollectionAssert.That.AreEqual(expected.Fields.Data, actual.Fields.Data);
        Assert.AreEqual(expected.Score2, actual.Score2);
        Assert.AreEqual(expected.BasePoint, actual.BasePoint);
        Assert.AreEqual(expected.NumViewed, actual.NumViewed);
        Assert.AreEqual(expected.NumLikes, actual.NumLikes);
        Assert.AreEqual(expected.NumFavs, actual.NumFavs);
        Assert.AreEqual(expected.NumBullets, actual.NumBullets);
        Assert.AreEqual(expected.NumBulletsNearby, actual.NumBulletsNearby);
        Assert.AreEqual(expected.RiskBonus, actual.RiskBonus);
        Assert.AreEqual(expected.BossShot, actual.BossShot);
        Assert.AreEqual(expected.AngleBonus, actual.AngleBonus);
        Assert.AreEqual(expected.MacroBonus, actual.MacroBonus);
        Assert.AreEqual(expected.LikesPerView, actual.LikesPerView);
        Assert.AreEqual(expected.FavsPerView, actual.FavsPerView);
        Assert.AreEqual(expected.NumHashtags, actual.NumHashtags);
        Assert.AreEqual(expected.NumRedBullets, actual.NumRedBullets);
        Assert.AreEqual(expected.NumPurpleBullets, actual.NumPurpleBullets);
        Assert.AreEqual(expected.NumBlueBullets, actual.NumBlueBullets);
        Assert.AreEqual(expected.NumCyanBullets, actual.NumCyanBullets);
        Assert.AreEqual(expected.NumGreenBullets, actual.NumGreenBullets);
        Assert.AreEqual(expected.NumYellowBullets, actual.NumYellowBullets);
        Assert.AreEqual(expected.NumOrangeBullets, actual.NumOrangeBullets);
        Assert.AreEqual(expected.NumLightBullets, actual.NumLightBullets);
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
#if NETFRAMEWORK
        _ = mock.Signature.Returns(signature.Substring(signature.Length - 1));
#else
        _ = mock.Signature.Returns(signature[0..^1]);
#endif

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

    public static IEnumerable<object[]> InvalidDays => TestUtils.GetInvalidEnumerators<Day>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidDays))]
    public void ReadFromTestInvalidDay(int day)
    {
        var mock = MockBestShotHeader();
        _ = mock.Weekday.Returns((Day)day);

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock)));
    }
}
