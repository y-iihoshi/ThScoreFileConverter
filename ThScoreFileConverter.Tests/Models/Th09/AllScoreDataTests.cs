using NSubstitute;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using LastName = ThScoreFileConverter.Models.Th07.LastName;
using LastNameTests = ThScoreFileConverter.Tests.Models.Th07.LastNameTests;
using VersionInfo = ThScoreFileConverter.Models.Th07.VersionInfo;
using VersionInfoTests = ThScoreFileConverter.Tests.Models.Th07.VersionInfoTests;

namespace ThScoreFileConverter.Tests.Models.Th09;

[TestClass]
public class AllScoreDataTests
{
    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.IsNull(allScoreData.Header);
        Assert.AreEqual(0, allScoreData.Rankings.Count);
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
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Score.Returns(87654u);
        _ = mock.Rank.Returns((short)2);

        var chapter = TestUtils.Create<Chapter>(HighScoreTests.MakeByteArray(mock));
        var score = new HighScore(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        Assert.AreSame(score, allScoreData.Rankings[(mock.Chara, mock.Level)][mock.Rank]);
    }

    [TestMethod]
    public void SetHighScoreTestTwice()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Score.Returns(87654u);
        _ = mock.Rank.Returns((short)2);

        var chapter = TestUtils.Create<Chapter>(HighScoreTests.MakeByteArray(mock));
        var score1 = new HighScore(chapter);
        var score2 = new HighScore(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        Assert.AreNotSame(score1, allScoreData.Rankings[(mock.Chara, mock.Level)][mock.Rank]);
        Assert.AreSame(score2, allScoreData.Rankings[(mock.Chara, mock.Level)][mock.Rank]);
    }

    [DataTestMethod]
    [DataRow((short)-1)]
    [DataRow((short)5)]
    public void SetHighScoreTestInvalidRank(short rank)
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Score.Returns(87654u);
        _ = mock.Rank.Returns(rank);

        var chapter = TestUtils.Create<Chapter>(HighScoreTests.MakeByteArray(mock));
        var score = new HighScore(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        for (var index = 0; index < allScoreData.Rankings[(mock.Chara, mock.Level)].Count; ++index)
        {
            Assert.IsNull(allScoreData.Rankings[(mock.Chara, mock.Level)][index]);
        }
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
