using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th06;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Core.Models.Th06.Chara, ThScoreFileConverter.Core.Models.Level>;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Core.Models.Th06.Chara,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        allScoreData.Header.ShouldBeNull();
        allScoreData.Rankings.ShouldBeEmpty();
        allScoreData.ClearData.ShouldBeEmpty();
        allScoreData.CardAttacks.ShouldBeEmpty();
        allScoreData.PracticeScores.ShouldBeEmpty();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
        var header = new Header(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(header);

        allScoreData.Header.ShouldBeSameAs(header);
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

        allScoreData.Header.ShouldNotBeSameAs(header1);
        allScoreData.Header.ShouldBeSameAs(header2);
    }

    [TestMethod]
    public void SetHighScoreTest()
    {
        var score = Substitute.For<IHighScore>();
        _ = score.Score.Returns(876543u);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        allScoreData.Rankings[(score.Chara, score.Level)][2].ShouldBeSameAs(score);
    }

    [TestMethod]
    public void SetHighScoreTestTwice()
    {
        var score1 = Substitute.For<IHighScore>();
        _ = score1.Score.Returns(876543u);
        var score2 = Substitute.For<IHighScore>();
        _ = score2.Score.Returns(876543u);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        allScoreData.Rankings[(score1.Chara, score1.Level)][2].ShouldBeSameAs(score1);
        allScoreData.Rankings[(score2.Chara, score2.Level)][3].ShouldBeSameAs(score2);
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        var clearData = Substitute.For<IClearData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        allScoreData.ClearData[clearData.Chara].ShouldBeSameAs(clearData);
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        var clearData1 = Substitute.For<IClearData>();
        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(clearData1.Chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        allScoreData.ClearData[clearData1.Chara].ShouldBeSameAs(clearData1);
        allScoreData.ClearData[clearData2.Chara].ShouldNotBeSameAs(clearData2);
    }

    [TestMethod]
    public void SetCardAttackTest()
    {
        var attack = Substitute.For<ICardAttack>();
        _ = attack.CardId.Returns((short)1);

        var allScoreData = new AllScoreData();
        allScoreData.Set(attack);

        allScoreData.CardAttacks[attack.CardId].ShouldBeSameAs(attack);
    }

    [TestMethod]
    public void SetCardAttackTestTwice()
    {
        var attack1 = Substitute.For<ICardAttack>();
        _ = attack1.CardId.Returns((short)1);
        var attack2 = Substitute.For<ICardAttack>();
        _ = attack2.CardId.Returns((short)1);

        var allScoreData = new AllScoreData();
        allScoreData.Set(attack1);
        allScoreData.Set(attack2);

        allScoreData.CardAttacks[attack1.CardId].ShouldBeSameAs(attack1);
        allScoreData.CardAttacks[attack2.CardId].ShouldNotBeSameAs(attack2);
    }

    [TestMethod]
    public void SetPracticeScoreTest()
    {
        var score = Substitute.For<IPracticeScore>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        allScoreData.PracticeScores[(score.Chara, score.Level, score.Stage)].ShouldBeSameAs(score);
    }

    [TestMethod]
    public void SetPracticeScoreTestTwice()
    {
        var score1 = Substitute.For<IPracticeScore>();
        var score2 = Substitute.For<IPracticeScore>();
        _ = score2.Level.Returns(score1.Level);
        _ = score2.Stage.Returns(score1.Stage);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        allScoreData.PracticeScores[(score1.Chara, score1.Level, score1.Stage)].ShouldBeSameAs(score1);
        allScoreData.PracticeScores[(score2.Chara, score2.Level, score2.Stage)].ShouldNotBeSameAs(score2);
    }

    [DataTestMethod]
    [DataRow(Level.Easy, Stage.Six)]
    [DataRow(Level.Extra, Stage.Extra)]
    [DataRow(Level.Extra, Stage.Six)]
    [DataRow(Level.Normal, Stage.Extra)]
    public void SetPracticeScoreTestInvalidPracticeStage(Level level, Stage stage)
    {
        var score = Substitute.For<IPracticeScore>();
        _ = score.Level.Returns(level);
        _ = score.Stage.Returns(stage);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        allScoreData.PracticeScores.ShouldBeEmpty();
    }
}
