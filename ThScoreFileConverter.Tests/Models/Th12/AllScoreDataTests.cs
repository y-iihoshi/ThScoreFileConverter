using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th12;

namespace ThScoreFileConverter.Tests.Models.Th12;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        Th10.AllScoreDataTests.AllScoreDataTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        Th10.AllScoreDataTests.SetHeaderTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        Th10.AllScoreDataTests.SetHeaderTestTwiceHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        Th10.AllScoreDataTests.SetClearDataTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        Th10.AllScoreDataTests.SetClearDataTestTwiceHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetStatusTest()
    {
        Th10.AllScoreDataTests.SetStatusTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        Th10.AllScoreDataTests.SetStatusTestTwiceHelper<CharaWithTotal>();
    }
}
