using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models.Th16;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;
using StagePractice = ThScoreFileConverter.Core.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th16;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        Th13.AllScoreDataTests.AllScoreDataTestHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        Th13.AllScoreDataTests.SetHeaderTestHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        Th13.AllScoreDataTests.SetHeaderTestTwiceHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        Th13.AllScoreDataTests.SetClearDataTestHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        Th13.AllScoreDataTests.SetClearDataTestTwiceHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetStatusTest()
    {
        Th13.AllScoreDataTests.SetStatusTestHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        Th13.AllScoreDataTests.SetStatusTestTwiceHelper<
            CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData, IStatus>();
    }
}
