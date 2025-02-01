using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th07;

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
        replacer.Replace("%T07CLEARHRB").ShouldBe(StageProgress.Four.ToDisplayName());
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
        replacer.Replace("%T07CLEARXRB").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestPhantasm()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Level.Returns(Level.Phantasm);
        _ = mock.StageProgress.Returns(StageProgress.Phantasm);
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var replacer = new ClearReplacer(rankings);
        replacer.Replace("%T07CLEARPRB").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var replacer = new ClearReplacer(rankings);
        replacer.Replace("%T07CLEARHRB").ShouldBe(StageProgress.None.ToDisplayName());
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
        replacer.Replace("%T07CLEARHRB").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T07CLEARNRB").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T07CLEARHRA").ShouldBe(StageProgress.None.ToDisplayName());
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T07XXXXXHRB").ShouldBe("%T07XXXXXHRB");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T07CLEARYRB").ShouldBe("%T07CLEARYRB");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(Rankings);
        replacer.Replace("%T07CLEARHXX").ShouldBe("%T07CLEARHXX");
    }
}
