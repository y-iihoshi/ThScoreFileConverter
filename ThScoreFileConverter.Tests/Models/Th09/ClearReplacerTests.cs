using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th09;

[TestClass]
public class ClearReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        new[] { new[] { HighScoreTests.MockHighScore() } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

    internal static IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; }

    internal static IReadOnlyDictionary<Chara, IClearCount> ZeroClearCounts { get; }

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

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
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Chara, mock.Level), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T09CLEARHMR1"));
    }

    [TestMethod]
    public void ReplaceTestCleared()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("Cleared", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ZeroClearCounts, formatterMock);
        Assert.AreEqual("Not Cleared", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNotTried()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.Date.Returns(TestUtils.CP932Encoding.GetBytes("--/--\0"));
        var rankings = new[] { new[] { mock } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock);
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock);
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Chara, mock.Level), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock);
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var highScoreMock = HighScoreTests.MockHighScore();
        var clearCountMock = ClearCountTests.MockClearCount();
        var counts = clearCountMock.Counts;
        _ = clearCountMock.Counts.Returns(counts.Where(pair => pair.Key != Level.Normal).ToDictionary());
        var clearCounts = new[] { (highScoreMock.Chara, clearCountMock) }.ToDictionary();
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, clearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09CLEARNMR1"));
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARNMR2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09CLEARHRM1"));
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHRM2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("%T09XXXXXHMR1", replacer.Replace("%T09XXXXXHMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("%T09CLEARYMR1", replacer.Replace("%T09CLEARYMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("%T09CLEARHXX1", replacer.Replace("%T09CLEARHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock);
        Assert.AreEqual("%T09CLEARHMRX", replacer.Replace("%T09CLEARHMRX"));
    }
}
