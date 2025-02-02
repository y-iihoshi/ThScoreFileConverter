using NSubstitute;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th128;
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

        allScoreData.Header.ShouldBeNull();
        allScoreData.ClearData.ShouldBeEmpty();
        allScoreData.CardData.ShouldBeNull();
        allScoreData.Status.ShouldBeNull();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
        var header = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header);

        allScoreData.Header.ShouldBeSameAs(header);
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

        allScoreData.Header.ShouldNotBeSameAs(header1);
        allScoreData.Header.ShouldBeSameAs(header2);
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        var route = RouteWithTotal.A2;
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(route);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        allScoreData.ClearData[route].ShouldBeSameAs(clearData);
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

        allScoreData.ClearData[route].ShouldBeSameAs(clearData1);
        allScoreData.ClearData[route].ShouldNotBeSameAs(clearData2);
    }

    [TestMethod]
    public void SetCardDataTest()
    {
        var cardData = Substitute.For<ICardData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(cardData);

        allScoreData.CardData.ShouldBeSameAs(cardData);
    }

    [TestMethod]
    public void SetCardDataTestTwice()
    {
        var cardData1 = Substitute.For<ICardData>();
        var cardData2 = Substitute.For<ICardData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(cardData1);
        allScoreData.Set(cardData2);

        allScoreData.CardData.ShouldNotBeSameAs(cardData1);
        allScoreData.CardData.ShouldBeSameAs(cardData2);
    }

    [TestMethod]
    public void SetStatusTest()
    {
        var status = Substitute.For<IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        allScoreData.Status.ShouldBeSameAs(status);
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        var status1 = Substitute.For<IStatus>();
        var status2 = Substitute.For<IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        allScoreData.Status.ShouldNotBeSameAs(status1);
        allScoreData.Status.ShouldBeSameAs(status2);
    }
}
