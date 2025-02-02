using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class CharaReplacerTests
{
    private static IClearData[] CreateClearDataList()
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

        return [clearData1, clearData2];
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(element => element.Chara);

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPMR1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestPointdevicePlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPMR2").ShouldBe("12:41:18");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPMR3").ShouldBe("invoked: 490");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPTL2").ShouldBe("22:17:26");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPTL3").ShouldBe("invoked: 730");
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARALMR1").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestLegacyPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARALMR2").ShouldBe("15:46:29");
    }

    [TestMethod]
    public void ReplaceTestLegacyClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARALMR3").ShouldBe("invoked: 740");
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARALTL1").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARALTL2").ShouldBe("15:46:29");
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARALTL3").ShouldBe("invoked: 740");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(dictionary, formatterMock);
        replacer.Replace("%T15CHARAPMR1").ShouldBe("invoked: 0");
        replacer.Replace("%T15CHARAPMR2").ShouldBe("0:00:00");
        replacer.Replace("%T15CHARAPMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyGameModes()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CharaReplacer(dictionary, formatterMock);
        replacer.Replace("%T15CHARAPMR3").ShouldBe("invoked: 0");
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
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CharaReplacer(dictionary, formatterMock);
        replacer.Replace("%T15CHARAPMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15XXXXXPMR1").ShouldBe("%T15XXXXXPMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAXMR1").ShouldBe("%T15CHARAXMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPXX1").ShouldBe("%T15CHARAPXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAPMRX").ShouldBe("%T15CHARAPMRX");
    }
}
