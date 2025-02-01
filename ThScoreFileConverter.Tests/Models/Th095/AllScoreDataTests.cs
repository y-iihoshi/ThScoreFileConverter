using NSubstitute;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class AllScoreDataTests
{
    internal static void AllScoreDataTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : class, IStatus
    {
        var allScoreData = new TAllScoreData();

        allScoreData.Header.ShouldBeNull();
        allScoreData.Scores.ShouldBeEmpty();
        allScoreData.Status.ShouldBeNull();
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

        allScoreData.Header.ShouldBeSameAs(header);
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

        allScoreData.Header.ShouldNotBeSameAs(header1);
        allScoreData.Header.ShouldBeSameAs(header2);
    }

    internal static void SetScoreTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : class, IChapter
        where TStatus : IStatus
    {
        var score = Substitute.For<TScore>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(score);

        allScoreData.Scores[0].ShouldBeSameAs(score);
    }

    internal static void SetScoreTestTwiceHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : class, IChapter
        where TStatus : IStatus
    {
        var score1 = Substitute.For<TScore>();
        var score2 = Substitute.For<TScore>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(score1);
        allScoreData.Set(score2);

        allScoreData.Scores[0].ShouldBeSameAs(score1);
        allScoreData.Scores[1].ShouldBeSameAs(score2);
    }

    internal static void SetStatusTestHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : class, IStatus
    {
        var status = Substitute.For<TStatus>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(status);

        allScoreData.Status.ShouldBeSameAs(status);
    }

    internal static void SetStatusTestTwiceHelper<TAllScoreData, TScore, TStatus>()
        where TAllScoreData : AllScoreDataBase<TScore, TStatus>, new()
        where TScore : IChapter
        where TStatus : class, IStatus
    {
        var status1 = Substitute.For<TStatus>();
        var status2 = Substitute.For<TStatus>();

        var allScoreData = new TAllScoreData();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        allScoreData.Status.ShouldNotBeSameAs(status1);
        allScoreData.Status.ShouldBeSameAs(status2);
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
