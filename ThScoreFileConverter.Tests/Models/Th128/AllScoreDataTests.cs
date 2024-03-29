﻿using NSubstitute;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverter.Tests.UnitTesting;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.IsNull(allScoreData.Header);
        Assert.AreEqual(0, allScoreData.ClearData.Count);
        Assert.IsNull(allScoreData.CardData);
        Assert.IsNull(allScoreData.Status);
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
        var header = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header);

        Assert.AreSame(header, allScoreData.Header);
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
        var header1 = TestUtils.Create<HeaderBase>(array);
        var header2 = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header1);
        allScoreData.Set(header2);

        Assert.AreNotSame(header1, allScoreData.Header);
        Assert.AreSame(header2, allScoreData.Header);
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        var route = RouteWithTotal.A2;
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(route);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        Assert.AreSame(clearData, allScoreData.ClearData[route]);
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        var route = RouteWithTotal.A2;
        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Route.Returns(route);
        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Route.Returns(route);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        Assert.AreSame(clearData1, allScoreData.ClearData[route]);
        Assert.AreNotSame(clearData2, allScoreData.ClearData[route]);
    }

    [TestMethod]
    public void SetCardDataTest()
    {
        var cardData = Substitute.For<ICardData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(cardData);

        Assert.AreSame(cardData, allScoreData.CardData);
    }

    [TestMethod]
    public void SetCardDataTestTwice()
    {
        var cardData1 = Substitute.For<ICardData>();
        var cardData2 = Substitute.For<ICardData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(cardData1);
        allScoreData.Set(cardData2);

        Assert.AreNotSame(cardData1, allScoreData.CardData);
        Assert.AreSame(cardData2, allScoreData.CardData);
    }

    [TestMethod]
    public void SetStatusTest()
    {
        var status = Substitute.For<IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        Assert.AreSame(status, allScoreData.Status);
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        var status1 = Substitute.For<IStatus>();
        var status2 = Substitute.For<IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        Assert.AreNotSame(status1, allScoreData.Status);
        Assert.AreSame(status2, allScoreData.Status);
    }
}
