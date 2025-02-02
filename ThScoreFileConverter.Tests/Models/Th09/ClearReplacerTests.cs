using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverter.Tests.Models.Th09;

[TestClass]
public class ClearReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        new[] { new[] { HighScoreTests.MockHighScore() } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

    internal static IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; }

    internal static IReadOnlyDictionary<Chara, IClearCount> ZeroClearCounts { get; }

    static ClearReplacerTests()
    {
        var highScoreMock = HighScoreTests.MockHighScore();
        var clearCountMock = ClearCountTests.MockClearCount();
        _ = clearCountMock.Counts.Returns(EnumHelper<Level>.Enumerable.ToDictionary(level => level, _ => 0));

        ClearCounts = new[] { (highScoreMock.Chara, ClearCountTests.MockClearCount()) }.ToDictionary();
        ZeroClearCounts = new[] { (highScoreMock.Chara, clearCountMock) }.ToDictionary();
    }

    [TestMethod]
    public void ClearReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(rankings, ClearCounts, formatterMock);
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
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(rankings, ClearCounts, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMR1").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestCleared()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMR2").ShouldBe("Cleared");
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ZeroClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMR2").ShouldBe("Not Cleared");
    }

    [TestMethod]
    public void ReplaceTestNotTried()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Date.Returns(TestUtils.CP932Encoding.GetBytes("--/--\0"));
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMR2").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMR2").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Chara, mock.Level), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMR2").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var highScoreMock = HighScoreTests.MockHighScore();
        var clearCountMock = ClearCountTests.MockClearCount();
        var counts = clearCountMock.Counts;
        _ = clearCountMock.Counts.Returns(counts.Where(pair => pair.Key != Level.Normal).ToDictionary());
        var clearCounts = new[] { (highScoreMock.Chara, clearCountMock) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, clearCounts, formatterMock);
        replacer.Replace("%T09CLEARNMR1").ShouldBe("invoked: 0");
        replacer.Replace("%T09CLEARNMR2").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHRM1").ShouldBe("invoked: 0");
        replacer.Replace("%T09CLEARHRM2").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09XXXXXHMR1").ShouldBe("%T09XXXXXHMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09CLEARYMR1").ShouldBe("%T09CLEARYMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHXX1").ShouldBe("%T09CLEARHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        replacer.Replace("%T09CLEARHMRX").ShouldBe("%T09CLEARHMRX");
    }
}
