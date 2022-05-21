using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using StagePractice = ThScoreFileConverter.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th17;

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
