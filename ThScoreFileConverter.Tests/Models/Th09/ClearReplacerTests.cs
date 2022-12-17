using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
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
        new[] { new[] { HighScoreTests.MockHighScore().Object } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

    internal static IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; }

    internal static IReadOnlyDictionary<Chara, IClearCount> ZeroClearCounts { get; }

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    static ClearReplacerTests()
    {
        var highScoreMock = HighScoreTests.MockHighScore();
        var clearCountMock = ClearCountTests.MockClearCount();
        _ = clearCountMock.SetupGet(m => m.Counts).Returns(
            EnumHelper<Level>.Enumerable.ToDictionary(level => level, _ => 0));

        ClearCounts =
            new[] { (highScoreMock.Object.Chara, ClearCountTests.MockClearCount().Object) }.ToDictionary();
        ZeroClearCounts = new[] { (highScoreMock.Object.Chara, clearCountMock.Object) }.ToDictionary();
    }

    [TestMethod]
    public void ClearReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ClearCounts, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ClearCounts, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T09CLEARHMR1"));
    }

    [TestMethod]
    public void ReplaceTestCleared()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("Cleared", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ZeroClearCounts, formatterMock.Object);
        Assert.AreEqual("Not Cleared", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNotTried()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.SetupGet(m => m.Date).Returns(TestUtils.CP932Encoding.GetBytes("--/--\0"));
        var rankings = new[] { new[] { mock.Object } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock.Object);
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock.Object);
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(rankings, ZeroClearCounts, formatterMock.Object);
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var highScoreMock = HighScoreTests.MockHighScore();
        var clearCountMock = ClearCountTests.MockClearCount();
        var counts = clearCountMock.Object.Counts;
        _ = clearCountMock.SetupGet(m => m.Counts).Returns(
            counts.Where(pair => pair.Key != Level.Normal).ToDictionary());
        var clearCounts = new[] { (highScoreMock.Object.Chara, clearCountMock.Object) }.ToDictionary();
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, clearCounts, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09CLEARNMR1"));
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARNMR2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09CLEARHRM1"));
        Assert.AreEqual("-------", replacer.Replace("%T09CLEARHRM2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("%T09XXXXXHMR1", replacer.Replace("%T09XXXXXHMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("%T09CLEARYMR1", replacer.Replace("%T09CLEARYMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("%T09CLEARHXX1", replacer.Replace("%T09CLEARHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ClearReplacer(Rankings, ClearCounts, formatterMock.Object);
        Assert.AreEqual("%T09CLEARHMRX", replacer.Replace("%T09CLEARHMRX"));
    }
}
