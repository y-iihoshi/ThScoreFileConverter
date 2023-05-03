using Moq;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class AllScoreDataTests
{
    internal static void AllScoreDataTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : IStatus
    {
        var allScoreData = new TAllScoreData();

        Assert.IsNull(allScoreData.Header);
        Assert.AreEqual(0, allScoreData.Scores.Count);
        Assert.IsNull(allScoreData.Status);
    }

    internal static void SetHeaderTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : IStatus
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
        var header = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new TAllScoreData();
        allScoreData.Set(header);

        Assert.AreSame(header, allScoreData.Header);
    }

    internal static void SetHeaderTestTwiceHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : IStatus
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
        var header1 = TestUtils.Create<HeaderBase>(array);
        var header2 = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new TAllScoreData();
        allScoreData.Set(header1);
        allScoreData.Set(header2);

        Assert.AreNotSame(header1, allScoreData.Header);
        Assert.AreSame(header2, allScoreData.Header);
    }

    internal static void SetScoreTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : class, IChapter
        where TStatus : IStatus
    {
        var score = Mock.Of<TScore>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(score);

        Assert.AreSame(score, allScoreData.Scores[0]);
    }

    internal static void SetScoreTestTwiceHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : class, IChapter
        where TStatus : IStatus
    {
        var score1 = Mock.Of<TScore>();
        var score2 = Mock.Of<TScore>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        Assert.AreSame(score1, allScoreData.Scores[0]);
        Assert.AreSame(score2, allScoreData.Scores[1]);
    }

    internal static void SetStatusTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : class, IStatus
    {
        var status = Mock.Of<TStatus>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(status);

        Assert.AreSame(status, allScoreData.Status);
    }

    internal static void SetStatusTestTwiceHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : class, IStatus
    {
        var status1 = Mock.Of<TStatus>();
        var status2 = Mock.Of<TStatus>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        Assert.AreNotSame(status1, allScoreData.Status);
        Assert.AreSame(status2, allScoreData.Status);
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        AllScoreDataTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        SetHeaderTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        SetHeaderTestTwiceHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetScoreTest()
    {
        SetScoreTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetScoreTestTwice()
    {
        SetScoreTestTwiceHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetStatusTest()
    {
        SetStatusTestHelper<AllScoreData, IScore, IStatus>();
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        SetStatusTestTwiceHelper<AllScoreData, IScore, IStatus>();
    }
}
