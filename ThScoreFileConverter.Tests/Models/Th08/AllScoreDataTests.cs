using NSubstitute;
using ThScoreFileConverter.Models.Th08;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore;
using LastName = ThScoreFileConverter.Models.Th07.LastName;
using LastNameTests = ThScoreFileConverter.Tests.Models.Th07.LastNameTests;
using VersionInfo = ThScoreFileConverter.Models.Th07.VersionInfo;
using VersionInfoTests = ThScoreFileConverter.Tests.Models.Th07.VersionInfoTests;

namespace ThScoreFileConverter.Tests.Models.Th08;

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
        Assert.IsNull(allScoreData.Flsp);
        Assert.IsNull(allScoreData.PlayStatus);
        Assert.IsNull(allScoreData.LastName);
        Assert.IsNull(allScoreData.VersionInfo);
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
        var score = Substitute.For<IHighScore>();
        _ = score.Score.Returns(87654u);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        Assert.AreSame(score, allScoreData.Rankings[(score.Chara, score.Level)][2]);
    }

    [TestMethod]
    public void SetHighScoreTestTwice()
    {
        var score1 = Substitute.For<IHighScore>();
        _ = score1.Score.Returns(87654u);
        var score2 = Substitute.For<IHighScore>();
        _ = score2.Score.Returns(87654u);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        Assert.AreSame(score1, allScoreData.Rankings[(score1.Chara, score1.Level)][2]);
        Assert.AreSame(score2, allScoreData.Rankings[(score2.Chara, score2.Level)][3]);
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        var clearData = Substitute.For<IClearData>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData);

        Assert.AreSame(clearData, allScoreData.ClearData[clearData.Chara]);
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        var clearData1 = Substitute.For<IClearData>();
        var chara = clearData1.Chara;
        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(chara);

        var allScoreData = new AllScoreData();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        Assert.AreSame(clearData1, allScoreData.ClearData[clearData1.Chara]);
        Assert.AreNotSame(clearData2, allScoreData.ClearData[clearData2.Chara]);
    }

    [TestMethod]
    public void SetCardAttackTest()
    {
        var attack = Substitute.For<ICardAttack>();
        _ = attack.CardId.Returns((short)1);

        var allScoreData = new AllScoreData();
        allScoreData.Set(attack);

        Assert.AreSame(attack, allScoreData.CardAttacks[attack.CardId]);
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

        Assert.AreSame(attack1, allScoreData.CardAttacks[attack1.CardId]);
        Assert.AreNotSame(attack2, allScoreData.CardAttacks[attack2.CardId]);
    }

    [TestMethod]
    public void SetPracticeScoreTest()
    {
        var score = Substitute.For<IPracticeScore>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        Assert.AreSame(score, allScoreData.PracticeScores[score.Chara]);
    }

    [TestMethod]
    public void SetPracticeScoreTestTwice()
    {
        var score1 = Substitute.For<IPracticeScore>();
        var score2 = Substitute.For<IPracticeScore>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        Assert.AreSame(score1, allScoreData.PracticeScores[score1.Chara]);
        Assert.AreNotSame(score2, allScoreData.PracticeScores[score1.Chara]);
    }

    [TestMethod]
    public void SetFlspTest()
    {
        var chapter = TestUtils.Create<Chapter>(FlspTests.MakeByteArray(FlspTests.ValidProperties));
        var flsp = new FLSP(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(flsp);

        Assert.AreSame(flsp, allScoreData.Flsp);
    }

    [TestMethod]
    public void SetFlspTestTwice()
    {
        var chapter = TestUtils.Create<Chapter>(FlspTests.MakeByteArray(FlspTests.ValidProperties));
        var flsp1 = new FLSP(chapter);
        var flsp2 = new FLSP(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(flsp1);
        allScoreData.Set(flsp2);

        Assert.AreNotSame(flsp1, allScoreData.Flsp);
        Assert.AreSame(flsp2, allScoreData.Flsp);
    }

    [TestMethod]
    public void SetPlayStatusTest()
    {
        var status = Substitute.For<IPlayStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        Assert.AreSame(status, allScoreData.PlayStatus);
    }

    [TestMethod]
    public void SetPlayStatusTestTwice()
    {
        var status1 = Substitute.For<IPlayStatus>();
        var status2 = Substitute.For<IPlayStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        Assert.AreNotSame(status1, allScoreData.PlayStatus);
        Assert.AreSame(status2, allScoreData.PlayStatus);
    }

    [TestMethod]
    public void SetLastNameTest()
    {
        var chapter = TestUtils.Create<Chapter>(LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
        var name = new LastName(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(name);

        Assert.AreSame(name, allScoreData.LastName);
    }

    [TestMethod]
    public void SetLastNameTestTwice()
    {
        var chapter = TestUtils.Create<Chapter>(LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
        var name1 = new LastName(chapter);
        var name2 = new LastName(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(name1);
        allScoreData.Set(name2);

        Assert.AreNotSame(name1, allScoreData.LastName);
        Assert.AreSame(name2, allScoreData.LastName);
    }

    [TestMethod]
    public void SetVersionInfoTest()
    {
        var chapter = TestUtils.Create<Chapter>(VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
        var info = new VersionInfo(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(info);

        Assert.AreSame(info, allScoreData.VersionInfo);
    }

    [TestMethod]
    public void SetVersionInfoTestTwice()
    {
        var chapter = TestUtils.Create<Chapter>(VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
        var info1 = new VersionInfo(chapter);
        var info2 = new VersionInfo(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(info1);
        allScoreData.Set(info2);

        Assert.AreNotSame(info1, allScoreData.VersionInfo);
        Assert.AreSame(info2, allScoreData.VersionInfo);
    }
}
