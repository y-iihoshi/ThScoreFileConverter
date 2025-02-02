using NSubstitute;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.Models.Th095;
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

        allScoreData.Header.ShouldBeNull();
        allScoreData.ClearData.ShouldBeEmpty();
        allScoreData.Status.ShouldBeNull();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
        var header = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header);

        allScoreData.Header.ShouldBeSameAs(header);
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

        allScoreData.Header.ShouldNotBeSameAs(header1);
        allScoreData.Header.ShouldBeSameAs(header2);
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        var chara = CharaWithTotal.Marisa;
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        allScoreData.ClearData[chara].ShouldBeSameAs(clearData);
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        var chara = CharaWithTotal.Marisa;
        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Chara.Returns(chara);
        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        allScoreData.ClearData[chara].ShouldBeSameAs(clearData1);
        allScoreData.ClearData[chara].ShouldNotBeSameAs(clearData2);
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
