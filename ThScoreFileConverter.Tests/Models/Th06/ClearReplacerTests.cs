using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Models.Th06;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Core.Models.Th06.Chara,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class ClearReplacerTests
{
    private static IReadOnlyList<IHighScore>[] CreateRankings()
    {
        var mock1 = HighScoreTests.MockHighScore();
        var stageProgress = mock1.StageProgress;
        var mock2 = HighScoreTests.MockHighScore();
        _ = mock2.StageProgress.Returns(++stageProgress);
        return [[mock1, mock2]];
    }

    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(Rankings);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var replacer = new ClearReplacer(rankings);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearReplacerTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Chara, mock.Level), ImmutableList<IHighScore>.Empty },
        };
        var replacer = new ClearReplacer(rankings);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T06CLEARHRB").ShouldBe(StageProgress.Four.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Level.Returns(Level.Extra);
        _ = mock.StageProgress.Returns(StageProgress.Extra);
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var replacer = new ClearReplacer(rankings);
        replacer.Replace("%T06CLEARXRB").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var replacer = new ClearReplacer(rankings);
        replacer.Replace("%T06CLEARHRB").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Chara, mock.Level), ImmutableList<IHighScore>.Empty },
        };
        var replacer = new ClearReplacer(rankings);
        replacer.Replace("%T06CLEARHRB").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T06CLEARNRB").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T06CLEARHRA").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T06XXXXXHRB").ShouldBe("%T06XXXXXHRB");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T06CLEARYRB").ShouldBe("%T06CLEARYRB");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T06CLEARHXX").ShouldBe("%T06CLEARHXX");
    }
}
