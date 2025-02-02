using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        Th095.AllScoreDataTests.AllScoreDataTestHelper<AllScoreData, IScore, IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.ItemStatuses.ShouldBeEmpty();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        Th095.AllScoreDataTests.SetHeaderTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        Th095.AllScoreDataTests.SetHeaderTestTwiceHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetScoreTest()
    {
        Th095.AllScoreDataTests.SetScoreTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetScoreTestTwice()
    {
        Th095.AllScoreDataTests.SetScoreTestTwiceHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetItemStatusTest()
    {
        var item = ItemWithTotal.Fablic;
        var status = Substitute.For<IItemStatus>();
        _ = status.Item.Returns(item);

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        allScoreData.ItemStatuses[item].ShouldBeSameAs(status);
    }

    [TestMethod]
    public void SetItemStatusTestTwice()
    {
        var item = ItemWithTotal.Fablic;
        var status1 = Substitute.For<IItemStatus>();
        _ = status1.Item.Returns(item);
        var status2 = Substitute.For<IItemStatus>();
        _ = status2.Item.Returns(item);

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        allScoreData.ItemStatuses[item].ShouldBeSameAs(status1);
        allScoreData.ItemStatuses[item].ShouldNotBeSameAs(status2);
    }

    [TestMethod]
    public void SetStatusTest()
    {
        Th095.AllScoreDataTests.SetStatusTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        Th095.AllScoreDataTests.SetStatusTestTwiceHelper<AllScoreData, IScore, IStatus>();
    }
}
