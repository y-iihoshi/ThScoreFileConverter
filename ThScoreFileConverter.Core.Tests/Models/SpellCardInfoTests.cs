using ThScoreFileConverter.Core.Models;
using CardInfo = ThScoreFileConverter.Core.Models.SpellCardInfo<
    ThScoreFileConverter.Core.Models.Stage, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class SpellCardInfoTests
{
    [TestMethod]
    public void SpellCardInfoTest()
    {
        var info = new CardInfo(1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic);

        info.Id.ShouldBe(1);
        info.Name.ShouldBe("月符「ムーンライトレイ」");
        info.Stage.ShouldBe(Stage.One);
        info.Level.ShouldBe(Level.Hard);
        info.Levels.ShouldBe([Level.Hard, Level.Lunatic]);
    }

    [TestMethod]
    public void SpellCardInfoTestNegativeId()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new CardInfo(-1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic));
    }

    [TestMethod]
    public void SpellCardInfoTestZeroId()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new CardInfo(0, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic));
    }

    [TestMethod]
    public void SpellCardInfoTestEmptyName()
    {
        _ = Should.Throw<ArgumentException>(
            () => new CardInfo(1, string.Empty, Stage.One, Level.Hard, Level.Lunatic));
    }

    public static IEnumerable<object[]> InvalidStages => TestHelper.GetInvalidEnumerators<Stage>();

    [TestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void SpellCardInfoTestInvalidStage(int stage)
    {
        _ = Should.Throw<ArgumentException>(
            () => new CardInfo(1, "月符「ムーンライトレイ」", (Stage)stage, Level.Hard, Level.Lunatic));
    }

    public static IEnumerable<object[]> InvalidLevels => TestHelper.GetInvalidEnumerators<Level>();

    [TestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void SpellCardInfoTestInvalidLevel(int level)
    {
        _ = Should.Throw<ArgumentException>(
            () => new CardInfo(1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, (Level)level));
    }

    [TestMethod]
    public void SpellCardInfoTestOneLevel()
    {
        var info = new CardInfo(1, "霜符「フロストコラムス」", Stage.One, Level.Hard);

        info.Id.ShouldBe(1);
        info.Name.ShouldBe("霜符「フロストコラムス」");
        info.Stage.ShouldBe(Stage.One);
        info.Level.ShouldBe(Level.Hard);
        info.Levels.ShouldBe([Level.Hard]);
    }

    [TestMethod]
    public void SpellCardInfoTestZeroLevels()
    {
        _ = Should.Throw<ArgumentException>(
            () => new CardInfo(1, "霜符「フロストコラムス」", Stage.One));
    }
}
