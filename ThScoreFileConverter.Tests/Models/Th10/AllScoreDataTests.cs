using NSubstitute;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Models.Th10;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class AllScoreDataTests
{
    internal static void AllScoreDataTestHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var allScoreData = new AllScoreData<TCharaWithTotal>();

        allScoreData.Header.ShouldBeNull();
        allScoreData.ClearData.ShouldBeEmpty();
        allScoreData.Status.ShouldBeNull();
    }

    internal static void SetHeaderTestHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var header = new HeaderBase();

        var allScoreData = new AllScoreData<TCharaWithTotal>();
        allScoreData.Set(header);

        allScoreData.Header.ShouldBeSameAs(header);
    }

    internal static void SetHeaderTestTwiceHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var header1 = new HeaderBase();
        var header2 = new HeaderBase();

        var allScoreData = new AllScoreData<TCharaWithTotal>();
        allScoreData.Set(header1);
        allScoreData.Set(header2);

        allScoreData.Header.ShouldNotBeSameAs(header1);
        allScoreData.Header.ShouldBeSameAs(header2);
    }

    internal static void SetClearDataTestHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var chara = TestUtils.Cast<TCharaWithTotal>(1);
        var clearData = Substitute.For<IClearData<TCharaWithTotal>>();
        _ = clearData.Chara.Returns(chara);

        var allScoreData = new AllScoreData<TCharaWithTotal>();
        allScoreData.Set(clearData);

        allScoreData.ClearData[chara].ShouldBeSameAs(clearData);
    }

    internal static void SetClearDataTestTwiceHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var chara = TestUtils.Cast<TCharaWithTotal>(1);
        var clearData1 = Substitute.For<IClearData<TCharaWithTotal>>();
        _ = clearData1.Chara.Returns(chara);
        var clearData2 = Substitute.For<IClearData<TCharaWithTotal>>();
        _ = clearData2.Chara.Returns(chara);

        var allScoreData = new AllScoreData<TCharaWithTotal>();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        allScoreData.ClearData[chara].ShouldBeSameAs(clearData1);
        allScoreData.ClearData[chara].ShouldNotBeSameAs(clearData2);
    }

    internal static void SetStatusTestHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var status = Substitute.For<IStatus>();

        var allScoreData = new AllScoreData<TCharaWithTotal>();
        allScoreData.Set(status);

        allScoreData.Status.ShouldBeSameAs(status);
    }

    internal static void SetStatusTestTwiceHelper<TCharaWithTotal>()
        where TCharaWithTotal : struct, Enum
    {
        var status1 = Substitute.For<IStatus>();
        var status2 = Substitute.For<IStatus>();

        var allScoreData = new AllScoreData<TCharaWithTotal>();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        allScoreData.Status.ShouldNotBeSameAs(status1);
        allScoreData.Status.ShouldBeSameAs(status2);
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        AllScoreDataTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        SetHeaderTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        SetHeaderTestTwiceHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        SetClearDataTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        SetClearDataTestTwiceHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetStatusTest()
    {
        SetStatusTestHelper<CharaWithTotal>();
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        SetStatusTestTwiceHelper<CharaWithTotal>();
    }
}
