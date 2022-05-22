using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Models.Th06.Chara, ThScoreFileConverter.Models.Level>;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Models.Th06.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.IsNull(allScoreData.Header);
        Assert.AreEqual(0, allScoreData.Rankings.Count);
        Assert.AreEqual(0, allScoreData.ClearData.Count);
        Assert.AreEqual(0, allScoreData.CardAttacks.Count);
        Assert.AreEqual(0, allScoreData.PracticeScores.Count);
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
        var header = new Header(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header);

        Assert.AreSame(header, allScoreData.Header);
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
        var header1 = new Header(chapter);
        var header2 = new Header(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header1);
        allScoreData.Set(header2);

        Assert.AreNotSame(header1, allScoreData.Header);
        Assert.AreSame(header2, allScoreData.Header);
    }

    [TestMethod]
    public void SetHighScoreTest()
    {
        var score = Mock.Of<IHighScore>(m => m.Score == 876543u);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        Assert.AreSame(score, allScoreData.Rankings[(score.Chara, score.Level)][2]);
    }

    [TestMethod]
    public void SetHighScoreTestTwice()
    {
        var score1 = Mock.Of<IHighScore>(m => m.Score == 876543u);
        var score2 = Mock.Of<IHighScore>(m => m.Score == 876543u);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        Assert.AreSame(score1, allScoreData.Rankings[(score1.Chara, score1.Level)][2]);
        Assert.AreSame(score2, allScoreData.Rankings[(score2.Chara, score2.Level)][3]);
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        var clearData = Mock.Of<IClearData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        Assert.AreSame(clearData, allScoreData.ClearData[clearData.Chara]);
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        var clearData1 = Mock.Of<IClearData>();
        var clearData2 = Mock.Of<IClearData>(m => m.Chara == clearData1.Chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        Assert.AreSame(clearData1, allScoreData.ClearData[clearData1.Chara]);
        Assert.AreNotSame(clearData2, allScoreData.ClearData[clearData2.Chara]);
    }

    [TestMethod]
    public void SetCardAttackTest()
    {
        var attack = Mock.Of<ICardAttack>(m => m.CardId == 1);

        var allScoreData = new AllScoreData();
        allScoreData.Set(attack);

        Assert.AreSame(attack, allScoreData.CardAttacks[attack.CardId]);
    }

    [TestMethod]
    public void SetCardAttackTestTwice()
    {
        var attack1 = Mock.Of<ICardAttack>(m => m.CardId == 1);
        var attack2 = Mock.Of<ICardAttack>(m => m.CardId == attack1.CardId);

        var allScoreData = new AllScoreData();
        allScoreData.Set(attack1);
        allScoreData.Set(attack2);

        Assert.AreSame(attack1, allScoreData.CardAttacks[attack1.CardId]);
        Assert.AreNotSame(attack2, allScoreData.CardAttacks[attack2.CardId]);
    }

    [TestMethod]
    public void SetPracticeScoreTest()
    {
        var score = Mock.Of<IPracticeScore>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        Assert.AreSame(score, allScoreData.PracticeScores[(score.Chara, score.Level, score.Stage)]);
    }

    [TestMethod]
    public void SetPracticeScoreTestTwice()
    {
        var score1 = Mock.Of<IPracticeScore>();
        var score2 = Mock.Of<IPracticeScore>(m => (m.Level == score1.Level) && (m.Stage == score1.Stage));

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        Assert.AreSame(score1, allScoreData.PracticeScores[(score1.Chara, score1.Level, score1.Stage)]);
        Assert.AreNotSame(score2, allScoreData.PracticeScores[(score2.Chara, score2.Level, score2.Stage)]);
    }

    [DataTestMethod]
    [DataRow(Level.Easy, Stage.Six)]
    [DataRow(Level.Extra, Stage.Extra)]
    [DataRow(Level.Extra, Stage.Six)]
    [DataRow(Level.Normal, Stage.Extra)]
    public void SetPracticeScoreTestInvalidPracticeStage(int level, int stage)
    {
        var score = Mock.Of<IPracticeScore>(
            m => (m.Level == TestUtils.Cast<Level>(level)) && (m.Stage == TestUtils.Cast<Stage>(stage)));

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        Assert.AreEqual(0, allScoreData.PracticeScores.Count);
    }
}
