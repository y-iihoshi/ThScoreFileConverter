using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class CharaExReplacerTests
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
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CharaExReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CharaExReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHMR1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestPointdevicePlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHMR2").ShouldBe("12:41:18");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHMR3").ShouldBe("invoked: 98");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPTMR1").ShouldBe("invoked: 23");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPTMR2").ShouldBe("12:41:18");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPTMR3").ShouldBe("invoked: 490");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHTL2").ShouldBe("22:17:26");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHTL3").ShouldBe("invoked: 146");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPTTL1").ShouldBe("invoked: 35");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPTTL2").ShouldBe("22:17:26");
    }

    [TestMethod]
    public void ReplaceTestPointdeviceTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPTTL3").ShouldBe("invoked: 730");
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLHMR1").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestLegacyPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLHMR2").ShouldBe("15:46:29");
    }

    [TestMethod]
    public void ReplaceTestLegacyClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLHMR3").ShouldBe("invoked: 148");
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLTMR1").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLTMR2").ShouldBe("15:46:29");
    }

    [TestMethod]
    public void ReplaceTestLegacyLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLTMR3").ShouldBe("invoked: 740");
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLHTL1").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLHTL2").ShouldBe("15:46:29");
    }

    [TestMethod]
    public void ReplaceTestLegacyCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLHTL3").ShouldBe("invoked: 148");
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalTotalPlayCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLTTL1").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalPlayTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLTTL2").ShouldBe("15:46:29");
    }

    [TestMethod]
    public void ReplaceTestLegacyTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXLTTL3").ShouldBe("invoked: 740");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(dictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHMR1").ShouldBe("invoked: 0");
        replacer.Replace("%T15CHARAEXPHMR2").ShouldBe("0:00:00");
        replacer.Replace("%T15CHARAEXPHMR3").ShouldBe("invoked: 0");
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

        var replacer = new CharaExReplacer(dictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15XXXXXXXPHMR1").ShouldBe("%T15XXXXXXXPHMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXXHMR1").ShouldBe("%T15CHARAEXXHMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPYMR1").ShouldBe("%T15CHARAEXPYMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHXX1").ShouldBe("%T15CHARAEXPHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T15CHARAEXPHMRX").ShouldBe("%T15CHARAEXPHMRX");
    }
}
