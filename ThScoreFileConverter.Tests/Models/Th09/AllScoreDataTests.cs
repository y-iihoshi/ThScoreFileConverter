using NSubstitute;
using ThScoreFileConverter.Models.Th09;
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

        allScoreData.Header.ShouldBeNull();
        allScoreData.Rankings.ShouldBeEmpty();
        allScoreData.PlayStatus.ShouldBeNull();
        allScoreData.LastName.ShouldBeNull();
        allScoreData.VersionInfo.ShouldBeNull();
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
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Score.Returns(87654u);
        _ = mock.Rank.Returns((short)2);

        var chapter = TestUtils.Create<Chapter>(HighScoreTests.MakeByteArray(mock));
        var score = new HighScore(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(score);

        allScoreData.Rankings[(mock.Chara, mock.Level)][mock.Rank].ShouldBeSameAs(score);
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

        allScoreData.Rankings[(mock.Chara, mock.Level)][mock.Rank].ShouldNotBeSameAs(score1);
        allScoreData.Rankings[(mock.Chara, mock.Level)][mock.Rank].ShouldBeSameAs(score2);
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
#pragma warning disable MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
            allScoreData.Rankings[(mock.Chara, mock.Level)][index].ShouldBeNull();
#pragma warning restore MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
        }
    }

    [TestMethod]
    public void SetPlayStatusTest()
    {
        var status = Substitute.For<IPlayStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status);

        allScoreData.PlayStatus.ShouldBeSameAs(status);
    }

    [TestMethod]
    public void SetPlayStatusTestTwice()
    {
        var status1 = Substitute.For<IPlayStatus>();
        var status2 = Substitute.For<IPlayStatus>();

        var allScoreData = new AllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        allScoreData.PlayStatus.ShouldNotBeSameAs(status1);
        allScoreData.PlayStatus.ShouldBeSameAs(status2);
    }

    [TestMethod]
    public void SetLastNameTest()
    {
        var chapter = TestUtils.Create<Chapter>(LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
        var name = new LastName(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(name);

        allScoreData.LastName.ShouldBeSameAs(name);
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

        allScoreData.LastName.ShouldNotBeSameAs(name1);
        allScoreData.LastName.ShouldBeSameAs(name2);
    }

    [TestMethod]
    public void SetVersionInfoTest()
    {
        var chapter = TestUtils.Create<Chapter>(VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
        var info = new VersionInfo(chapter);

        var allScoreData = new AllScoreData();
        allScoreData.Set(info);

        allScoreData.VersionInfo.ShouldBeSameAs(info);
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

        allScoreData.VersionInfo.ShouldNotBeSameAs(info1);
        allScoreData.VersionInfo.ShouldBeSameAs(info2);
    }
}
