using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class CharaReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        var levels = EnumHelper<LevelWithTotal>.Enumerable;

        IClearDataPerGameMode MockClearDataPerGameMode(int totalPlayCount, int playTime, Func<LevelWithTotal, int> clearCountFunc)
        {
            var mock = Substitute.For<IClearDataPerGameMode>();
            _ = mock.TotalPlayCount.Returns(totalPlayCount);
            _ = mock.PlayTime.Returns(playTime);
            _ = mock.ClearCounts.Returns(levels.ToDictionary(level => level, clearCountFunc));
            return mock;
        }

        var gameModeData1 = new Dictionary<GameMode, IClearDataPerGameMode>
        {
            { GameMode.Pointdevice, MockClearDataPerGameMode(23, 4567890, level => 100 - (int)level) },
            { GameMode.Legacy, MockClearDataPerGameMode(34, 5678901, level => 150 - (int)level) },
        };
        var clearData1 = Substitute.For<IClearData>();
        _ = clearData1.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData1.GameModeData.Returns(gameModeData1);

        var gameModeData2 = new Dictionary<GameMode, IClearDataPerGameMode>
        {
            { GameMode.Pointdevice, MockClearDataPerGameMode(12, 3456789, level => 50 - (int)level) },
        };
        var clearData2 = Substitute.For<IClearData>();
        _ = clearData2.Chara.Returns(CharaWithTotal.Sanae);
        _ = clearData2.GameModeData.Returns(gameModeData2);

        return new[] { clearData1, clearData2 };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(element => element.Chara);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 23", replacer.Replace("%T15CHARAPMR1"));
    }

    [TestMethod]
    public void ReplaceTestPointdevicePlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("12:41:18", replacer.Replace("%T15CHARAPMR2"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 490", replacer.Replace("%T15CHARAPMR3"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T15CHARAPTL1"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("22:17:26", replacer.Replace("%T15CHARAPTL2"));
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 730", replacer.Replace("%T15CHARAPTL3"));
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T15CHARALMR1"));
    }

    [TestMethod]
    public void ReplaceTestLegacyPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARALMR2"));
    }

    [TestMethod]
    public void ReplaceTestLegacyClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 740", replacer.Replace("%T15CHARALMR3"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalTotalPlayCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T15CHARALTL1"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalPlayTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARALTL2"));
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 740", replacer.Replace("%T15CHARALTL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR1"));
        Assert.AreEqual("0:00:00", replacer.Replace("%T15CHARAPMR2"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyGameModes()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.ClearCounts.Returns(ImmutableDictionary<LevelWithTotal, int>.Empty);
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CharaReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15XXXXXPMR1", replacer.Replace("%T15XXXXXPMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CHARAXMR1", replacer.Replace("%T15CHARAXMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CHARAPXX1", replacer.Replace("%T15CHARAPXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T15CHARAPMRX", replacer.Replace("%T15CHARAPMRX"));
    }
}
