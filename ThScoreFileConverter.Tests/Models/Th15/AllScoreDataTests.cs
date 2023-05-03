using Moq;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.IsNull(allScoreData.Header);
        Assert.AreEqual(0, allScoreData.ClearData.Count);
        Assert.IsNull(allScoreData.Status);
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
        var header = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header);

        Assert.AreSame(header, allScoreData.Header);
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
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
        var chara = CharaWithTotal.Marisa;
        var clearData = Mock.Of<IClearData>(m => m.Chara == chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        Assert.AreSame(clearData, allScoreData.ClearData[chara]);
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        var chara = CharaWithTotal.Marisa;
        var clearData1 = Mock.Of<IClearData>(m => m.Chara == chara);
        var clearData2 = Mock.Of<IClearData>(m => m.Chara == chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        Assert.AreSame(clearData1, allScoreData.ClearData[chara]);
        Assert.AreNotSame(clearData2, allScoreData.ClearData[chara]);
    }

    [TestMethod]
    public void SetStatusTest()
    {
        var status = Mock.Of<IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        Assert.AreSame(status, allScoreData.Status);
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        var status1 = Mock.Of<IStatus>();
        var status2 = Mock.Of<IStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        Assert.AreNotSame(status1, allScoreData.Status);
        Assert.AreSame(status2, allScoreData.Status);
    }
}
