using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Tests.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverter.Tests.Models.Th13;

[TestClass]
public class AllScoreDataTests
{
    internal static void AllScoreDataTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum       // TCharaWithTotal
        where TLv : struct, Enum            // TLevel
        where TLvPrac : struct, Enum        // TLevelPractice
        where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
        where TStPrac : struct, Enum        // TStagePractice
        where TScoreData : IScoreData
        where TStatus : IStatus
    {
        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();

        Assert.IsNull(allScoreData.Header);
        Assert.AreEqual(0, allScoreData.ClearData.Count);
        Assert.IsNull(allScoreData.Status);
    }

    internal static void SetHeaderTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
        where TStatus : IStatus
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
        var header = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();
        allScoreData.Set(header);

        Assert.AreSame(header, allScoreData.Header);
    }

    internal static void SetHeaderTestTwiceHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
        where TStatus : IStatus
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
        var header1 = TestUtils.Create<HeaderBase>(array);
        var header2 = TestUtils.Create<HeaderBase>(array);

        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();
        allScoreData.Set(header1);
        allScoreData.Set(header2);

        Assert.AreNotSame(header1, allScoreData.Header);
        Assert.AreSame(header2, allScoreData.Header);
    }

    internal static void SetClearDataTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
        where TStatus : IStatus
    {
        var chara = TestUtils.Cast<TChWithT>(1);
        var mock = new Mock<IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>>();
        _ = mock.SetupGet(m => m.Chara).Returns(chara);
        var clearData = mock.Object;

        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();
        allScoreData.Set(clearData);

        Assert.AreSame(clearData, allScoreData.ClearData[chara]);
    }

    internal static void SetClearDataTestTwiceHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
        where TStatus : IStatus
    {
        var chara = TestUtils.Cast<TChWithT>(1);
        var mock1 = new Mock<IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>>();
        _ = mock1.SetupGet(m => m.Chara).Returns(chara);
        var clearData1 = mock1.Object;
        var mock2 = new Mock<IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData>>();
        _ = mock2.SetupGet(m => m.Chara).Returns(chara);
        var clearData2 = mock2.Object;

        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();
        allScoreData.Set(clearData1);
        allScoreData.Set(clearData2);

        Assert.AreSame(clearData1, allScoreData.ClearData[chara]);
        Assert.AreNotSame(clearData2, allScoreData.ClearData[chara]);
    }

    internal static void SetStatusTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
        where TStatus : class, IStatus
    {
        var status = Mock.Of<TStatus>();

        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();
        allScoreData.Set(status);

        Assert.AreSame(status, allScoreData.Status);
    }

    internal static void SetStatusTestTwiceHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>()
        where TChWithT : struct, Enum
        where TLv : struct, Enum
        where TLvPrac : struct, Enum
        where TLvPracWithT : struct, Enum
        where TStPrac : struct, Enum
        where TScoreData : IScoreData
        where TStatus : class, IStatus
    {
        var status1 = Mock.Of<TStatus>();
        var status2 = Mock.Of<TStatus>();

        var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TScoreData, TStatus>();
        allScoreData.Set(status1);
        allScoreData.Set(status2);

        Assert.AreNotSame(status1, allScoreData.Status);
        Assert.AreSame(status2, allScoreData.Status);
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        AllScoreDataTestHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTest()
    {
        SetHeaderTestHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetHeaderTestTwice()
    {
        SetHeaderTestTwiceHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetClearDataTest()
    {
        SetClearDataTestHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetClearDataTestTwice()
    {
        SetClearDataTestTwiceHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetStatusTest()
    {
        SetStatusTestHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }

    [TestMethod]
    public void SetStatusTestTwice()
    {
        SetStatusTestTwiceHelper<
            CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
    }
}
