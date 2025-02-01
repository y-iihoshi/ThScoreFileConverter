using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class ClearReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static IScoreData MockScoreData(LevelWithTotal level, int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.StageProgress.Returns(
                level == LevelWithTotal.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
            _ = mock.DateTime.Returns((uint)index % 2);
            return mock;
        }

        static IClearDataPerGameMode MockClearDataPerGameMode()
        {
            var rankings = EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => MockScoreData(level, index)).ToList() as IReadOnlyList<IScoreData>);
            var mock = Substitute.For<IClearDataPerGameMode>();
            _ = mock.Rankings.Returns(rankings);
            return mock;
        }

        var gameModeData = new[] { (GameMode.Pointdevice, MockClearDataPerGameMode()) }.ToDictionary();
        var mock = Substitute.For<IClearData>();
        _ = mock.Chara.Returns(CharaWithTotal.Marisa);
        _ = mock.GameModeData.Returns(gameModeData);
        return [mock];
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T15CLEARPHMR").ShouldBe("Stage 5");
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T15CLEARPXMR").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestExtraClear()
    {
        static IScoreData MockScoreData()
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.StageProgress.Returns(StageProgress.ExtraClear);
            _ = mock.DateTime.Returns(1u);
            return mock;
        }

        var rankings = EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
            level => level,
            level => new[] { MockScoreData() } as IReadOnlyList<IScoreData>);

        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.Rankings.Returns(rankings);

        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(element => element.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T15CLEARPXMR").ShouldBe("All Clear");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T15CLEARPHMR").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyGameModes()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T15CLEARPHMR").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.Rankings.Returns(ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty);
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T15CLEARPHMR").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var clearDataPerGameMode = Substitute.For<IClearDataPerGameMode>();
        _ = clearDataPerGameMode.Rankings.Returns(
            EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>));
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.GameModeData.Returns(new[] { (GameMode.Pointdevice, clearDataPerGameMode) }.ToDictionary());
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T15CLEARPHMR").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T15XXXXXPHMR").ShouldBe("%T15XXXXXPHMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T15CLEARXHMR").ShouldBe("%T15CLEARXHMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T15CLEARPYMR").ShouldBe("%T15CLEARPYMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T15CLEARPHXX").ShouldBe("%T15CLEARPHXX");
    }
}
