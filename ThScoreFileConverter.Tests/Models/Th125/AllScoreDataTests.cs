using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        Th095.AllScoreDataTests.AllScoreDataTestHelper<AllScoreData, IScore, IStatus>();
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
