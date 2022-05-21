using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        Assert.AreEqual(0, allScoreData.ItemStatuses.Count);
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
        var status = Mock.Of<IItemStatus>(m => m.Item == item);

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        Assert.AreSame(status, allScoreData.ItemStatuses[item]);
    }

    [TestMethod]
    public void SetItemStatusTestTwice()
    {
        var item = ItemWithTotal.Fablic;
        var status1 = Mock.Of<IItemStatus>(m => m.Item == item);
        var status2 = Mock.Of<IItemStatus>(m => m.Item == item);

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        Assert.AreSame(status1, allScoreData.ItemStatuses[item]);
        Assert.AreNotSame(status2, allScoreData.ItemStatuses[item]);
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
