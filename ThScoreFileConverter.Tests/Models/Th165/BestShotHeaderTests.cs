using System;
using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Diagnostics;
using Moq;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class BestShotHeaderTests
{
    internal static Mock<IBestShotHeader> MockInitialBestShotHeader()
    {
        var mock = new Mock<IBestShotHeader>();
        _ = mock.SetupGet(m => m.Signature).Returns(string.Empty);
        return mock;
    }

    internal static Mock<IBestShotHeader> MockBestShotHeader()
    {
        var mock = new Mock<IBestShotHeader>();
        _ = mock.SetupGet(m => m.Signature).Returns("BST4");
        _ = mock.SetupGet(m => m.Weekday).Returns(Day.Monday);
        _ = mock.SetupGet(m => m.Dream).Returns(3);
        _ = mock.SetupGet(m => m.Width).Returns(4);
        _ = mock.SetupGet(m => m.Height).Returns(5);
        _ = mock.SetupGet(m => m.Width2).Returns(6);
        _ = mock.SetupGet(m => m.Height2).Returns(7);
        _ = mock.SetupGet(m => m.HalfWidth).Returns(8);
        _ = mock.SetupGet(m => m.HalfHeight).Returns(9);
        _ = mock.SetupGet(m => m.SlowRate).Returns(10f);
        _ = mock.SetupGet(m => m.DateTime).Returns(11u);
        _ = mock.SetupGet(m => m.Angle).Returns(12f);
        _ = mock.SetupGet(m => m.Score).Returns(13);
        _ = mock.SetupGet(m => m.Fields).Returns(new HashtagFields(14, 15, 16));
        _ = mock.SetupGet(m => m.Score2).Returns(17);
        _ = mock.SetupGet(m => m.BasePoint).Returns(18);
        _ = mock.SetupGet(m => m.NumViewed).Returns(19);
        _ = mock.SetupGet(m => m.NumLikes).Returns(20);
        _ = mock.SetupGet(m => m.NumFavs).Returns(21);
        _ = mock.SetupGet(m => m.NumBullets).Returns(22);
        _ = mock.SetupGet(m => m.NumBulletsNearby).Returns(23);
        _ = mock.SetupGet(m => m.RiskBonus).Returns(24);
        _ = mock.SetupGet(m => m.BossShot).Returns(25);
        _ = mock.SetupGet(m => m.AngleBonus).Returns(26);
        _ = mock.SetupGet(m => m.MacroBonus).Returns(27);
        _ = mock.SetupGet(m => m.LikesPerView).Returns(28);
        _ = mock.SetupGet(m => m.FavsPerView).Returns(29);
        _ = mock.SetupGet(m => m.NumHashtags).Returns(30);
        _ = mock.SetupGet(m => m.NumRedBullets).Returns(31);
        _ = mock.SetupGet(m => m.NumPurpleBullets).Returns(32);
        _ = mock.SetupGet(m => m.NumBlueBullets).Returns(33);
        _ = mock.SetupGet(m => m.NumCyanBullets).Returns(34);
        _ = mock.SetupGet(m => m.NumGreenBullets).Returns(35);
        _ = mock.SetupGet(m => m.NumYellowBullets).Returns(36);
        _ = mock.SetupGet(m => m.NumOrangeBullets).Returns(37);
        _ = mock.SetupGet(m => m.NumLightBullets).Returns(38);
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
        _ = mock.SetupGet(m => m.Signature).Returns(signature.Substring(signature.Length - 1));
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

    public static IEnumerable<object[]> InvalidDays => TestUtils.GetInvalidEnumerators<Day>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidDays))]
    public void ReadFromTestInvalidDay(int day)
    {
        var mock = MockBestShotHeader();
        _ = mock.SetupGet(m => m.Weekday).Returns((Day)day);

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
    }
}
